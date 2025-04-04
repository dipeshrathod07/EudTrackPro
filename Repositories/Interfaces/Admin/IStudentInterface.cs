using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IStudentInterface
    {

        Task<int> Register(t_student student);
        Task<List<t_student>> GetAll();
        Task<t_student> GetOne(string studentid);
        Task<List<t_class>> GetAllClass();
        Task<List<t_section>> GetSectionByClass(string classId);
        Task<object> GetAllTeacher();

        Task<int> Update(t_student studentData);
        Task<int> Delete(string studentid);
        Task<List<t_list>> GetData();
        Task<int> GetStudentCount();
        Task<byte[]> ExportStudentsToPDF();

        #region Student
        Task<t_Student> GetStudent(int id);

        Task<int> GetStudentIdByUserId(int userId);
        Task<List<t_SyllabusTracking>> GetSyllabusTrackingByStudent(int studentId);

        Task<int> teacherFeedback(t_teacherFeedback feedback);
        Task<List<vm_subjectimetable>> GetTaskByClass(string classid);
        Task<(bool success, string message, List<TeachingMaterial>)> GetMaterialsByTeacherId(int id);

        #endregion
    }
}