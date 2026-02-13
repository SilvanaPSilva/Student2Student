using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studentTOstudent.Entities
{
    internal class LessonRequest
    {
        public int Id { get; set; }

        public StudentLearner Learner { get; set; }

        public Module Module { get; set; }
        public string Topic { get; set; } = string.Empty;

        public bool HasAttachment { get; set; }
        public string AttachmentPath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
