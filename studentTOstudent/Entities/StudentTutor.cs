using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using studentTOstudent.Entities;

namespace studentTOstudent.Entities
{
    public class StudentTutor : StudentRegister
    {
        public string Bio { get; set; } = string.Empty;
        public List<string> Subjects { get; set; } = new List<string>();
        public bool IsAvailable { get; set; } = true;        
        public decimal HourlyRate { get; set; }
        public double Rating { get; set; }
    }
}
