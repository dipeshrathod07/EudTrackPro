namespace Edutrack.Models;

public class Student

{
    public int C_Student_Id { get; set; }  // Primary Key
    public int C_User_Id { get; set; }  // Foreign Key (User ID)
    public string C_Full_Name { get; set; } // Student’s full name
    public DateTime C_Date_Of_Birth { get; set; } // Date of birth
    public string C_Gender { get; set; }  // Male, Female, or Other
    public int C_Class_Id { get; set; }  // Foreign Key (Class ID)
    public int C_Section_Id { get; set; }  // Foreign Key (Section ID)
    public string C_Guardian_Details { get; set; } // Guardian information
    public DateTime C_Enrollment_Date { get; set; }  // Enrollment date
    public string C_Image { get; set; }  // Image URL or path
    public bool C_Status { get; set; }  // Active/Inactive
    public DateTime C_Created_At { get; set; } = DateTime.UtcNow; // Record creation timestamp
    public int C_Teacher_Id { get; set; }  // Foreign Key (Teacher ID)


}
