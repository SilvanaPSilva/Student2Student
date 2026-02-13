
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studentTOstudent.Entities
{
    public class Booking
    {
        public DateTime CreateAtDate { get; set; }
        public DateTime ReservationDate { get; set; }
        public Course Course { get; set; }
        public Module Module { get; set; }
        public string Topic { get; set; } = string.Empty;
        public StudentLearner StudentLearner { get; set; }
        public StudentTutor StudentTutor { get; set; }
        public bool HasAttachment { get; set; }
        public string AttachmentPath { get; set; } = string.Empty;
        public bool IsAccepting { get; set; }

    }
}
