namespace Edutrack.Models
{
    
    public class TeachingMaterial
{
    public int C_Material_Id { get; set; }
    public string C_File_Name { get; set; } = string.Empty;
    public string C_File_Type { get; set; } = string.Empty;
    public DateTime C_Upload_Date { get; set; } = DateTime.UtcNow;
    public int C_Subject_Id { get; set; }
    public int C_Teacher_Id { get; set; }
    public string C_File_Path { get; set; } = string.Empty;

    public string? c_Teacher_Name { get; set; }
    public string? C_Subject_Name { get; set; }
}
}