using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Repositories.Models
{
    public class t_exam
    {
        public int c_exam_id {get; set;}
         public t_class? Class { get; set; }
        public int c_class_id {get; set;}
        public string? c_class_name {get; set;}
        // public string? c_exam_image {get;set;}
        // public IFormFile? ExamImage{get; set;}
        public string? c_exam_image {get;set;}
        public IFormFile? ExamFile{get; set;}

    }
}