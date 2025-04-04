using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class t_teacherFeedback
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? C_Feedback_Id { get; set; }

        public int? C_Teacher_Id { get; set; }

        public int ?C_Student_Id { get; set; }

        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int C_Rating { get; set; }
        public string? C_Feedback_Text { get; set; }
        public DateTime C_Created_Date { get; set; } = DateTime.UtcNow;

    }
}