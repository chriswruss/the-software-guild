using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exercises.Models.Data {
    public class EditStudent {
        public Student Student { get; set; }
        public List<int> SelectedCourseIds { get; set; }
    }
}