using studentTOstudent.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace studentTOstudent.Entities 
{
    public class StudentLearner : StudentRegister
    {
        // User input the topic that would like learn about
        public string Topic { get; set; } = string.Empty;

        // Módulos/matérias de interesse
        public List<Module> Modules { get; set; } = new List<Module>();

        // Indica se o learner costuma anexar arquivos
        public bool HasAttachment { get; set; }
    }
}
