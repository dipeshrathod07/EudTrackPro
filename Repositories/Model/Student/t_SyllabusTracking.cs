using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Models
{
    public class t_SyllabusTracking
    {
         [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SyllabusId { get; set; }  // c_syllabusid (Primary Key)

        [ForeignKey("Student")]
        public int StudentId { get; set; }  // c_student_id (Foreign Key)

        [ForeignKey("Subject")]
        public int SubjectId { get; set; }  // c_subject_id (Foreign Key)

        [Required]
        public string Topic { get; set; }  // c_topic (NOT NULL)

        [Range(0, 100)]
        public int CompletionPercentage { get; set; }  // c_completionpercentage (0-100 range)

        public DateTime? CompletionDate { get; set; }  // c_completiondate (Nullable Date)


    }
}