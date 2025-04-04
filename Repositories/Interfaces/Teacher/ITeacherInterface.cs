using Edutrack.Models;
using Npgsql;
namespace Edutrack;

public interface ITeacherInterface
{

    #region Schedular
    Task<int> AddScheduling(t_Schedular schedulingData);

    Task<List<t_Schedular>> GetSchedulings(int teacherid, int classId, int sectionId);

    Task<int> UpdateScheduling(t_Schedular schedulingData);

    Task<int> DeleteSchedule(int scheduleId);

    Task<List<t_Class>> GetClasses();

    Task<List<t_Section>> GetSections(int classId);

    #endregion

    #region teacher profile
    Task<t_UpdateTeacher> GetTeacher(int userID);
    Task<int> UpdateTeacher(t_UpdateTeacher teacher, int userID);

    Task<int> GetTeacherIdByuserId(int userId);
    
    #endregion
}