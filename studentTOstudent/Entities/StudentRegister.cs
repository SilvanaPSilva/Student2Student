using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using studentTOstudent.Entities;

namespace studentTOstudent.Entities
{
    public abstract class StudentRegister
    {
        //"Tutor or Learner"
        public string StudentRole { get; set; }
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public int PhoneNumber { get; set; }
        public string University { get; set; }
        public string Course { get; set; }
        public int AcademicYear { get; set; }  
        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
