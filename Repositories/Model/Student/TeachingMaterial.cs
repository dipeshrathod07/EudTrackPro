using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class TeachingMaterial
    {
        public int C_Material_Id { get; set; }
        public string C_File_Name { get; set; }
        public string C_File_Type { get; set; }
        public DateTime C_Upload_Date { get; set; }
        public int C_Subject_Id { get; set; }
        public int C_Teacher_Id { get; set; }
        public string C_File_Path { get; set; }

        public string? c_Teacher_Name { get; set; }
        public string? C_Subject_Name { get; set; }


    }
}