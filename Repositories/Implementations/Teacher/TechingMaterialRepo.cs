using Npgsql;
using Edutrack.Models;
using Edutrack;

namespace Edutrack
{
    public class TechingMaterialRepo : ITeachingMaterial
    {
        private readonly NpgsqlConnection _connection;
        public TechingMaterialRepo(NpgsqlConnection npgsqlConnection)
        {
            _connection = npgsqlConnection;
        }


        public async Task<(bool success, string message)> addMaterial(TeachingMaterial teachingMaterial)
        {
            try
            {
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    await _connection.OpenAsync();
                }
                string query = @"
                        INSERT INTO t_teaching_material 
                        (c_file_name, c_file_type, c_upload_date, c_subject_id, c_teacher_id, c_file_path) 
                        VALUES (@FileName, @FileType, @UploadDate, @SubjectId, @TeacherId, @FilePath)";

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@FileName", teachingMaterial.C_File_Name);
                    cmd.Parameters.AddWithValue("@FileType", teachingMaterial.C_File_Type);
                    cmd.Parameters.AddWithValue("@UploadDate", teachingMaterial.C_Upload_Date);
                    cmd.Parameters.AddWithValue("@SubjectId", teachingMaterial.C_Subject_Id);
                    cmd.Parameters.AddWithValue("@TeacherId", teachingMaterial.C_Teacher_Id);
                    cmd.Parameters.AddWithValue("@FilePath", teachingMaterial.C_File_Path);

                    await cmd.ExecuteNonQueryAsync();
                }
                return (true, "Teaching material uploaded successfully");
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                return (false, $"Database error: {ex.Message}");

            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<(bool success, string message, List<TeachingMaterial>)> GetMaterialsByTeacherId(int id)
        {
            try
            {

                List<TeachingMaterial> materials = new List<TeachingMaterial>();
                string query = @"
                SELECT 
                    tm.c_material_id, 
                    tm.c_file_name, 
                    tm.c_file_type, 
                    tm.c_upload_date, 
                    tm.c_subject_id, 
                    tm.c_teacher_id, 
                    tm.c_file_path, 
                    u.c_userName AS teacher_name,
                    s.c_subject_name 
                FROM t_teaching_material tm
                LEFT JOIN t_teacher t ON tm.c_teacher_id = t.c_teacher_id
                LEFT JOIN t_user u ON t.c_user_id = u.c_user_id
                LEFT JOIN t_subject s ON tm.c_subject_id = s.c_subject_id
                WHERE tm.c_teacher_id = @TeacherId";
                await _connection.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@TeacherId", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            materials.Add(new TeachingMaterial
                            {
                                C_Material_Id = reader.GetInt32(0),
                                C_File_Name = reader.GetString(1),
                                C_File_Type = reader.GetString(2),
                                C_Upload_Date = reader.GetDateTime(3),
                                C_Subject_Id = reader.GetInt32(4),
                                C_Teacher_Id = reader.GetInt32(5),
                                C_File_Path = reader.GetString(6),
                                c_Teacher_Name = reader.IsDBNull(7) ? null : reader.GetString(7),
                                C_Subject_Name = reader.IsDBNull(8) ? null : reader.GetString(8)
                            });
                        }
                    }
                }
                if (materials.Count > 0)
                {
                    return (true, "Materials retrieved successfully", materials);
                }
                return (false, "No materials found for the given teacher", new List<TeachingMaterial>());

            }
            catch (Exception ex)
            {
                return (false, "Error retrieving materials: " + ex.Message, new List<TeachingMaterial>());
            }
            finally
            {
                await _connection.CloseAsync();
            }

        }


        public async Task<(bool success, string message, List<Student>)> Getstudentbyteacher(int id)
        {
            try
            {
                string query = @"
            SELECT 
                c_student_id, 
                c_user_id, 
                c_full_name, 
                c_date_of_birth, 
                c_gender, 
                c_class_id, 
                c_section_id, 
                c_guardian_details, 
                c_enrollment_date, 
                c_image, 
                c_status, 
                c_created_at, 
                c_teacher_id
            FROM t_student
            WHERE c_teacher_id = @TeacherId";

                List<Student> students = new List<Student>();

                await _connection.OpenAsync(); // Open DB connection

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("@TeacherId", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            students.Add(new Student
                            {
                                C_Student_Id = reader.GetInt32(0),
                                C_User_Id = reader.GetInt32(1),
                                C_Full_Name = reader.GetString(2),
                                C_Date_Of_Birth = reader.GetDateTime(3),
                                C_Gender = reader.GetString(4),
                                C_Class_Id = reader.GetInt32(5),
                                C_Section_Id = reader.GetInt32(6),
                                C_Guardian_Details = reader.GetString(7),
                                C_Enrollment_Date = reader.GetDateTime(8),
                                C_Image = reader.IsDBNull(9) ? null : reader.GetString(9),
                                C_Status = reader.GetBoolean(10),
                                C_Created_At = reader.GetDateTime(11),
                                C_Teacher_Id = reader.GetInt32(12)
                            });
                        }
                    }
                }

                return students.Count > 0
                    ? (true, "Students retrieved successfully", students)
                    : (false, "No students found for this teacher", new List<Student>());
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", new List<Student>());
            }
            finally
            {
                await _connection.CloseAsync(); // Ensure connection is closed
            }
        }

        public async Task<(bool success, string message, TeacherModel)> GetTeacherById(int id)
        {
            try
            {
                // Define SQL query
                string query = @"
            SELECT 
                t_teacher.c_teacher_id,
                t_user.c_userName AS username,
                t_teacher.c_phone_number,
                t_teacher.c_date_of_birth,
                t_teacher.c_qualification,
                t_teacher.c_experience,
                t_teacher.c_subject_expertise,
                t_teacher.c_createdAt,
                t_teacher.c_status
            FROM t_teacher
            JOIN t_user ON t_teacher.c_user_id = t_user.c_user_id
            WHERE t_teacher.c_teacher_id = @TeacherId";

                // Open PostgreSQL connection
                await _connection.OpenAsync();

                using (var cmd = new NpgsqlCommand(query, _connection))
                {
                    cmd.Parameters.AddWithValue("TeacherId", id);

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            // Manually mapping result to TeacherModel
                            var teacher = new TeacherModel
                            {
                                TeacherId = reader.GetInt32(reader.GetOrdinal("c_teacher_id")),
                                username = reader.GetString(reader.GetOrdinal("username")),
                                PhoneNumber = reader.GetString(reader.GetOrdinal("c_phone_number")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("c_date_of_birth")),
                                Qualification = reader.GetString(reader.GetOrdinal("c_qualification")),
                                Experience = reader.GetInt32(reader.GetOrdinal("c_experience")),
                                SubjectExpertise = reader.GetString(reader.GetOrdinal("c_subject_expertise")),
                                CreatedAt = reader.GetDateTime(reader.GetOrdinal("c_createdAt")),
                                Status = reader.GetString(reader.GetOrdinal("c_status"))
                            };

                            return (true, "Teacher found successfully", teacher);
                        }
                        else
                        {
                            return (false, "Teacher not found", null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error: {ex.Message}", null);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<t_notification> GetNotification()
        {
            if (_connection.State == System.Data.ConnectionState.Closed)
            {
                await _connection.OpenAsync();
            }

            try
            {
                string query = "SELECT * FROM t_notifications WHERE c_receiver = 'Teacher'";
                using (var cmd = new NpgsqlCommand(query, _connection))
                {

                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new t_notification
                            {
                                c_title_name = reader.GetString(reader.GetOrdinal("c_title_name")),
                                c_title_description = reader.GetString(reader.GetOrdinal("c_title_description")),
                                c_receiver = reader.GetString(reader.GetOrdinal("c_receiver"))

                            };
                        }
                        else
                        {
                            return null; // No payment found
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetNotification Exception: " + ex.Message);
                return null;
            }
        }

    }
}
