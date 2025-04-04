namespace Edutrack.Models;

public class TeacherModel
{
    public int TeacherId { get; set; }
    public int UserId { get; set; }
    public string PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Qualification { get; set; }
    public int Experience { get; set; }
    public string SubjectExpertise { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; }

    public string username { get; set; }
}
