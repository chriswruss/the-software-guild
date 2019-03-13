using Exercises.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Exercises.Models.Data;
using Exercises.Models.ViewModels;

namespace Exercises.Controllers
{
    public class StudentController : Controller
    {
        // When a student is being edited, the old information will be assigned to _studentPulled.
        // This information will be used to repopulate the previous data in the event that saving
        // the information fails validation.
        private static EditStudent _studentPulled = new EditStudent();

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult List()
        {
            var model = StudentRepository.GetAll();

            return View(model);
        }

        [HttpGet]
        public ActionResult Add()
        {
            var viewModel = new StudentVM();
            viewModel.SetCourseItems(CourseRepository.GetAll());
            viewModel.SetMajorItems(MajorRepository.GetAll());
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Add(StudentVM studentVM) {

            // Validation
            if (string.IsNullOrWhiteSpace(studentVM.Student.FirstName)) {
                ModelState.AddModelError("Student.FirstName", "First name is required.");
            }

            if (string.IsNullOrWhiteSpace(studentVM.Student.LastName)) {
                ModelState.AddModelError("Student.LastName", "Last name is required.");
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors);
            
            if (errors.Count() == 0) {
                // When adding a student, the major that gets passed in only has the majorId
                // Need to use that id to pull the appropriate major.
                studentVM.Student.Major = MajorRepository.Get(studentVM.Student.Major.MajorId); 

                studentVM.Student.Courses = new List<Course>();

                foreach (var id in studentVM.SelectedCourseIds)
                    studentVM.Student.Courses.Add(CourseRepository.Get(id));

                StudentRepository.Add(studentVM.Student);

                return RedirectToAction("List");
            }

            // If it fails validation, the CourseItems and MajorItems are cleared out.
            // Need to use the GetAll method from the Course and Major repository to 
            // repopulate the course and major list.
            studentVM = new StudentVM();
            studentVM.SetCourseItems(CourseRepository.GetAll());
            studentVM.SetMajorItems(MajorRepository.GetAll());

            return View("Add", studentVM);
        }

        [HttpGet]
        public ActionResult Edit(int id) {
            var student = StudentRepository.Get(id);
            var editStudent = new EditStudent {
                Student = student
            };
            _studentPulled = editStudent;
            return View(editStudent);
        }

        [HttpPost]
        public ActionResult Edit(EditStudent editStudent) {
            // Since validation is taking place here, need to have a way to store
            // any error messages that will be added to the ModelState. This also
            // is used to determine whether it passed validation. If no ModelErrors
            // are added, the size will be zero.
            IEnumerable<ModelError> errors;
            
            // If the user is updating the student's personal details, the address will
            // be passed to the controller as null. If the address is null, perform validation
            // on the Personal Details form.
            if (editStudent.Student.Address == null) {
                if (string.IsNullOrWhiteSpace(editStudent.Student.FirstName)) {
                    ModelState.AddModelError("Student.FirstName", "First name is required.");
                }

                if (string.IsNullOrWhiteSpace(editStudent.Student.LastName)) {
                    ModelState.AddModelError("Student.LastName", "Last name is required.");
                }

                errors = ModelState.Values.SelectMany(v => v.Errors);

                if (errors.Count() == 0) {
                    if (editStudent.SelectedCourseIds != null) {
                        editStudent.Student.Courses = new List<Course>();
                        foreach (var id in editStudent.SelectedCourseIds) {
                            editStudent.Student.Courses.Add(CourseRepository.Get(id));
                        }
                    }

                    // When adding a student, the major that gets passed in only has the majorId
                    // Need to use that id to pull the appropriate major.
                    editStudent.Student.Major = MajorRepository.Get(editStudent.Student.Major.MajorId);

                    StudentRepository.Edit(editStudent.Student);

                    return RedirectToAction("List");
                }

                // Set the editStudent information back to the information before attempting to edit
                editStudent = _studentPulled;
                return View("Edit", editStudent);
            }

            // For some unknown reason, I was unable to create a new int inside of the TryParse that
            // is used below when validating the postal code.
            int Garbage;

            if (string.IsNullOrWhiteSpace(editStudent.Student.Address.Street1)) {
                ModelState.AddModelError("Student.Address.Street1", "Street name is required.");
            }

            if (string.IsNullOrWhiteSpace(editStudent.Student.Address.City)) {
                ModelState.AddModelError("Student.Address.City", "City is required.");
            }

            if (string.IsNullOrWhiteSpace(editStudent.Student.Address.PostalCode)) {
                ModelState.AddModelError("Student.Address.PostalCode", "Postal code is required.");
            }
            else if (editStudent.Student.Address.PostalCode.Length != 5 || !int.TryParse(editStudent.Student.Address.PostalCode, out Garbage)) {
                ModelState.AddModelError("Student.Address.PostalCode", "Postal code must be five numbers.");
            }

            errors = ModelState.Values.SelectMany(v => v.Errors);

            if (errors.Count() == 0) {
                editStudent.Student.Address.State = StateRepository.Get(editStudent.Student.Address.State.StateAbbreviation);
                StudentRepository.SaveAddress(editStudent.Student.StudentId, editStudent.Student.Address);

                return RedirectToAction("List");
            }

            // Set the editStudent back to the information it was before attempting to edit.
            editStudent = _studentPulled;
            return View("Edit", editStudent);
        }

        [HttpGet]
        public ActionResult Delete(int id) {
            var student = StudentRepository.Get(id);
            return View(student);
        }

        [HttpPost]
        public ActionResult Delete(Student student) {
            StudentRepository.Delete(student.StudentId);
            return RedirectToAction("List");
        }
    }
}