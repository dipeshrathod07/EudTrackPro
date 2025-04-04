using Edutrack.Models;

namespace Edutrack
{
    public interface ITeachingMaterial
    {
        Task<(bool success, string message)> addMaterial(TeachingMaterial teachingMaterial);
        Task<(bool success, string message, List<TeachingMaterial>)> GetMaterialsByTeacherId(int id);
        Task<(bool success, string message, List<Student>)> Getstudentbyteacher(int id);
        Task<(bool success, string message, TeacherModel)> GetTeacherById(int id);
        Task<t_notification> GetNotification();

    }
}