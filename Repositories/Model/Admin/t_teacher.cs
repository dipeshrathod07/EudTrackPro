using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models;

public class t_teacher
{
    public int c_teacher_id { get; set; }
    public int c_user_id { get; set; }
    public string? c_teacher_name { get; set; }     // New Field
    public string? c_email { get; set; }
    public string? c_phone_number { get; set; }
    public DateTime c_date_of_birth { get; set; }
    public string? c_qualification { get; set; }
    public int c_experience { get; set; }
    public string c_subject_expertise { get; set; }
    public DateTime c_created_at { get; set; }
    public string? c_status { get; set; }



}
