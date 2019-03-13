using Exercises.Models.Data;
using Exercises.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Exercises.Controllers
{
    public class AdminController : Controller
    {
        // Since validation is taking place here, need to have a way to store
        // any error messages that will be added to the ModelState. This also
        // is used to determine whether it passed validation. If no ModelErrors
        // are added, the size will be zero.
        IEnumerable<ModelError> errors;

        [HttpGet]
        public ActionResult Majors()
        {
            var model = MajorRepository.GetAll();
            return View(model.ToList());
        }

        [HttpGet]
        public ActionResult AddMajor()
        {
            return View(new Major());
        }

        [HttpPost]
        public ActionResult AddMajor(Major major)
        {
            if (string.IsNullOrWhiteSpace(major.MajorName)) {
                ModelState.AddModelError("MajorName", "Major name is required.");
                return View("AddMajor", major);
            }
            if (ModelState.IsValid) {
                MajorRepository.Add(major.MajorName);
                return RedirectToAction("Majors");
            }

            return View("AddMajor", major);
        }

        [HttpGet]
        public ActionResult EditMajor(int id)
        {
            var major = MajorRepository.Get(id);
            return View(major);
        }

        [HttpPost]
        public ActionResult EditMajor(Major major)
        {
            if (string.IsNullOrWhiteSpace(major.MajorName)) {
                ModelState.AddModelError("MajorName", "Major name is required.");
                return View("EditMajor", major);
            }
            if (ModelState.IsValid) {
                MajorRepository.Edit(major);
                return RedirectToAction("Majors");
            }

            return View("EditMajor", major);
        }

        [HttpGet]
        public ActionResult DeleteMajor(int id)
        {
            var major = MajorRepository.Get(id);
            return View(major);
        }

        [HttpPost]
        public ActionResult DeleteMajor(Major major)
        {
            MajorRepository.Delete(major.MajorId);
            return RedirectToAction("Majors");
        }

        [HttpGet]
        public ActionResult States() {
            var model = StateRepository.GetAll();
            return View(model.ToList());
        }

        [HttpGet]
        public ActionResult AddState() {
            return View(new State());
        }

        [HttpPost]
        public ActionResult AddState(State state) {
            if (string.IsNullOrWhiteSpace(state.StateAbbreviation)) {
                ModelState.AddModelError("StateAbbreviation", "State abbreviation is required.");
                return View("AddState", state);
            }

            if (string.IsNullOrWhiteSpace(state.StateName)) {
                ModelState.AddModelError("StateName", "State name is required.");
                return View("AddState", state);
            }

            StateRepository.Add(state);
            return RedirectToAction("States");
        }

        [HttpGet]
        public ActionResult EditState(string abbreviation) {
            var state = StateRepository.Get(abbreviation);
            return View(state);
        }

        [HttpPost]
        public ActionResult EditState(State state) {
            if (string.IsNullOrWhiteSpace(state.StateName)) {
                ModelState.AddModelError("StateName", "State name is required.");
            }

            errors = ModelState.Values.SelectMany(x => x.Errors);

            if (errors.Count() == 0) {
                StateRepository.Edit(state);
                return RedirectToAction("States");
            }

            return View("EditState", state);
        }

        [HttpGet]
        public ActionResult DeleteState(string abbreviation) {
            var state = StateRepository.Get(abbreviation);
            return View(state);
        }

        [HttpPost]
        public ActionResult DeleteState(State state) {
            StateRepository.Delete(state.StateAbbreviation);
            return RedirectToAction("States");
        }

        [HttpGet]
        public ActionResult Courses() {
            var model = CourseRepository.GetAll();
            return View(model.ToList());
        }

        [HttpGet]
        public ActionResult AddCourse() {
            return View(new Course());
        }

        [HttpPost]
        public ActionResult AddCourse(Course course) {
            if (string.IsNullOrWhiteSpace(course.CourseName)) {
                ModelState.AddModelError("CourseName", "Course name is required.");
                return View("AddCourse", course);
            }

            CourseRepository.Add(course.CourseName);
            return RedirectToAction("Courses");
        }

        [HttpGet]
        public ActionResult EditCourse(int id) {
            var course = CourseRepository.Get(id);
            return View(course);
        }

        [HttpPost]
        public ActionResult EditCourse(Course course) {
            if (string.IsNullOrWhiteSpace(course.CourseName)) {
                ModelState.AddModelError("CourseName", "Course name is required.");
                return View("EditCourse", course);
            }

            CourseRepository.Edit(course);
            return RedirectToAction("Courses");
        }

        [HttpGet]
        public ActionResult DeleteCourse(int id) {
            var course = CourseRepository.Get(id);
            return View(course);
        }

        [HttpPost]
        public ActionResult DeleteCourse(Course course) {
            CourseRepository.Delete(course.CourseId);
            return RedirectToAction("Courses");
        }
    }
}