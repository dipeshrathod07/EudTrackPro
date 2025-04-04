using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class vm_subjectimetable
    {
         public int c_Subject_Id { get; set; }
        public int c_Teacher_Id { get; set; }
        public DateTime? c_Task_Start_Time { get; set; }
        public DateTime? c_Task_End_Time { get; set; }
        public string c_Task_Description { get; set; }

    }
}