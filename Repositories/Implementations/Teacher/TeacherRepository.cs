using System.Data.Common;
using Edutrack.Models;
using Npgsql;
namespace Edutrack;


public class TeacherRepository : ITeacherInterface
{
    private readonly NpgsqlConnection _conn;

    public TeacherRepository(NpgsqlConnection connection)
    {
        _conn = connection;
    }


    #region Schedular
    public async Task<int> AddScheduling(t_Schedular schedulingData)
    {
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand EventCmd = new NpgsqlCommand(@"INSERT INTO t_subjectimetable
            (c_class_id,
            c_section_id,
            c_subject_id,
            c_teacher_id,
            c_task_title,
            c_task_start_time,
            c_task_end_time,
            c_task_description,
            c_task_is_all_day)
            VALUES
            (@c_class_id,
            @c_section_id,
            @c_subject_id,
            @c_teacher_id,
            @c_task_title,
            @c_task_start_time,
            @c_task_end_time,
            @c_task_description,
            @c_task_is_all_day)", _conn);
            EventCmd.Parameters.AddWithValue("@c_class_id", schedulingData.C_Class_Id);
            EventCmd.Parameters.AddWithValue("@c_section_id", schedulingData.C_Section_Id);
            EventCmd.Parameters.AddWithValue("@c_subject_id", schedulingData.C_Subject_Id);
            EventCmd.Parameters.AddWithValue("@c_teacher_id", schedulingData.C_Teacher_Id);
            EventCmd.Parameters.AddWithValue("@c_task_title", schedulingData.C_Task_Title);
            EventCmd.Parameters.AddWithValue("@c_task_start_time", schedulingData.C_Task_Start_Time);
            EventCmd.Parameters.AddWithValue("@c_task_end_time", schedulingData.C_Task_End_Time);
            EventCmd.Parameters.AddWithValue("@c_task_description", schedulingData.C_Task_Description);
            EventCmd.Parameters.AddWithValue("@c_task_is_all_day", schedulingData.C_Task_Is_All_Day);
            await _conn.OpenAsync();
            int result = await EventCmd.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Add Event Error : " + ex.Message);
            return 0;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }

    public async Task<List<t_Schedular>> GetSchedulings(int teacherId, int classId, int sectionId)
    {
        List<t_Schedular> schedulings = new List<t_Schedular>();
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand GetSchedulerCmd = new NpgsqlCommand(@"
            SELECT * FROM  
            t_subjectimetable
            WHERE c_teacher_id = @c_teacher_id
            AND
            c_class_id = @c_class_id
            AND
            c_section_id = @c_section_id", _conn);
            GetSchedulerCmd.Parameters.AddWithValue("@c_teacher_id", teacherId);
            GetSchedulerCmd.Parameters.AddWithValue("@c_Class_id", classId);
            GetSchedulerCmd.Parameters.AddWithValue("@c_section_id", sectionId);

            await _conn.OpenAsync();
            NpgsqlDataReader datar = await GetSchedulerCmd.ExecuteReaderAsync();
            while (datar.Read())
            {
                schedulings.Add(new t_Schedular
                {
                    C_TimeTable_Id = datar.GetInt32(0),
                    C_Class_Id = datar.GetInt32(1),
                    C_Section_Id = datar.GetInt32(2),
                    C_Subject_Id = datar.GetInt32(3),
                    C_Teacher_Id = datar.GetInt32(4),
                    C_Task_Title = datar.GetString(5),
                    C_Task_Start_Time = datar.GetDateTime(6),
                    C_Task_End_Time = datar.GetDateTime(7),
                    C_Task_Description = datar.GetString(8),
                    C_Task_Is_All_Day = datar.GetBoolean(9),
                    C_CreatedAt = datar.GetDateTime(10),
                    // C_TimeTable_Id = datar.GetInt32(0),
                    // C_Task_Title = datar.GetString(1),
                    // C_Task_Start_Time = datar.GetDateTime(2),
                    // C_Task_End_Time = datar.GetDateTime(3),
                    // C_Task_Description = datar.GetString(4),
                    // C_Task_Is_All_Day = datar.GetBoolean(5),
                });
            }
            return schedulings;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo GetScheduling Error : " + ex.Message);
            return schedulings;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }


    public async Task<int> UpdateScheduling(t_Schedular schedulingData)
    {
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand EventCmd = new NpgsqlCommand(@"UPDATE t_subjectimetable
            SET c_class_id = @c_class_id,
            c_section_id = @c_section_id,
            c_subject_id = @c_subject_id,
            c_teacher_id = @c_teacher_id,
            c_task_title = @c_task_title,
            c_task_start_time = @c_task_start_time,
            c_task_end_time = @c_task_end_time,
            c_task_description = @c_task_description,
            c_task_is_all_day = @c_task_is_all_day
            WHERE 
            c_timetable_id = @c_timetable_id", _conn);
            EventCmd.Parameters.AddWithValue("@c_class_id", schedulingData.C_Class_Id);
            EventCmd.Parameters.AddWithValue("@c_section_id", schedulingData.C_Section_Id);
            EventCmd.Parameters.AddWithValue("@c_subject_id", schedulingData.C_Subject_Id);
            EventCmd.Parameters.AddWithValue("@c_teacher_id", schedulingData.C_Teacher_Id);
            EventCmd.Parameters.AddWithValue("@c_task_title", schedulingData.C_Task_Title);
            EventCmd.Parameters.AddWithValue("@c_task_start_time", schedulingData.C_Task_Start_Time);
            EventCmd.Parameters.AddWithValue("@c_task_end_time", schedulingData.C_Task_End_Time);
            EventCmd.Parameters.AddWithValue("@c_task_description", schedulingData.C_Task_Description);
            EventCmd.Parameters.AddWithValue("@c_task_is_all_day", schedulingData.C_Task_Is_All_Day);
            EventCmd.Parameters.AddWithValue("@c_timetable_id", schedulingData.C_TimeTable_Id);
            await _conn.OpenAsync();
            int result = await EventCmd.ExecuteNonQueryAsync();
            await _conn.CloseAsync();
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Update Event Error : " + ex.Message);
            return 0;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }

    public async Task<int> DeleteSchedule(int scheduleId)
    {
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand DeleteCmd = new NpgsqlCommand(@"DELETE FROM t_subjectimetable
            WHERE
            c_timetable_id = @c_timetable_id", _conn);
            DeleteCmd.Parameters.AddWithValue("@c_timetable_id", scheduleId);
            await _conn.OpenAsync();
            int result = await DeleteCmd.ExecuteNonQueryAsync();
            if (result == 1)
            {
                return 1;
            }
            return 0;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Delete Event Error : " + ex.Message);
            return 0;
        }
        finally
        {
            _conn.CloseAsync();
        }
    }

    public async Task<List<t_Class>> GetClasses()
    {
        List<t_Class> Classes = new List<t_Class>();
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand FetchClasses = new NpgsqlCommand(@"SELECT * FROM t_class", _conn);
            await _conn.OpenAsync();
            NpgsqlDataReader ClassDatar = await FetchClasses.ExecuteReaderAsync();
            while (ClassDatar.Read())
            {
                Classes.Add(new t_Class
                {
                    C_Class_Id = ClassDatar.GetInt32(0),
                    C_Class_Name = ClassDatar.GetString(1),
                });
            }
            return Classes;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Fetch Class Event Error : " + ex.Message);
            return Classes;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }


    public async Task<List<t_Section>> GetSections(int classId)
    {
        List<t_Section> Sections = new List<t_Section>();
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand FetchSections = new NpgsqlCommand(@"SELECT * FROM t_section
            WHERE
            c_class_id = @c_class_id", _conn);
            FetchSections.Parameters.Add(new NpgsqlParameter("@c_class_id", classId));
            await _conn.OpenAsync();
            NpgsqlDataReader SectionDatar = await FetchSections.ExecuteReaderAsync();
            while (SectionDatar.Read())
            {
                Sections.Add(new t_Section
                {
                    C_Section_Id = SectionDatar.GetInt32(0),
                    C_Section_Name = SectionDatar.GetString(1),
                    C_Class_Id = SectionDatar.GetInt32(2),
                });
            }
            return Sections;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Fetch Section Event Error : " + ex.Message);
            return Sections;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }
    #endregion

    #region Teacher Profile

    public async Task<t_UpdateTeacher> GetTeacher(int userID)
    {

        try
        {
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                await _conn.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("SELECT c_phone_number,c_date_of_birth,c_qualification,c_experience,c_subject_expertise FROM t_teacher WHERE c_user_id = @userID", _conn))
            {
                cmd.Parameters.AddWithValue("userID", userID);
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new t_UpdateTeacher
                        {
                            C_phone = reader["c_phone_number"].ToString(),
                            C_date = DateOnly.FromDateTime(Convert.ToDateTime(reader["c_date_of_birth"])),
                            C_qualification = reader["c_qualification"].ToString(),
                            C_experience = Convert.ToInt32(reader["c_experience"]),
                            C_subjectExpertise = reader["c_subject_expertise"].ToString()
                        };

                    }
                    else
                    {
                        return null;
                    }
                }
            }

        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Error: " + ex.Message);
        }
        finally
        {
            if (_conn.State == System.Data.ConnectionState.Open)
            {
                await _conn.CloseAsync();
            }
        }

        return null;
    }


    public async Task<int> UpdateTeacher(t_UpdateTeacher teacher, int userID)
    {
        try
        {
            if (_conn.State != System.Data.ConnectionState.Open)
            {
                await _conn.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("UPDATE t_teacher SET c_phone_number=@phone, c_date_of_birth = @date,c_qualification = @qualification,c_experience = @experience ,c_subject_expertise = @subject_expertise WHERE c_user_id = @userID", _conn))
            {
                cmd.Parameters.AddWithValue("phone", teacher.C_phone);
                cmd.Parameters.AddWithValue("date", teacher.C_date);
                cmd.Parameters.AddWithValue("qualification", teacher.C_qualification);
                cmd.Parameters.AddWithValue("experience", teacher.C_experience);
                cmd.Parameters.AddWithValue("subject_expertise", teacher.C_subjectExpertise);
                cmd.Parameters.AddWithValue("userID", userID);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception ex)
        {

            Console.WriteLine("Error: " + ex.Message);
            return 0;
        }

        finally
        {
            if (_conn.State == System.Data.ConnectionState.Open)
            {
                await _conn.CloseAsync();
            }
        }
    }

    public async Task<int> GetTeacherIdByuserId(int userId)
    {
        try
        {
            await _conn.CloseAsync();
            NpgsqlCommand TeacherIdCmd = new NpgsqlCommand(@"SELECT c_teacher_id FROM t_teacher
            WHERE
            c_user_id = @c_user_id", _conn);
            TeacherIdCmd.Parameters.AddWithValue("@c_user_id", userId);
            await _conn.OpenAsync();
            object? result = await TeacherIdCmd.ExecuteScalarAsync();
            int resultInt = (int)result;
            if (resultInt == 0)
            {
                return 0;
            }
            return resultInt;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Teacher Repo Get Teacher id Error : " + ex.Message);
            return 0;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }

    #endregion

}
