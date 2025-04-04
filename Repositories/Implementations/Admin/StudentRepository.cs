using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class StudentRepository : IStudentInterface
    {

        private readonly NpgsqlConnection _conn;
        public StudentRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        public async Task<List<t_class>> GetAllClass()
        {

            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_class_id, c_class_name from t_class", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();

            if (datar.HasRows)
            {
                dt.Load(datar);
            }

            List<t_class> Class = new List<t_class>();

            Class = (from DataRow dr in dt.Rows
                     select new t_class()
                     {
                         c_class_name = dr["c_class_name"].ToString(),
                         c_class_id = Convert.ToInt32(dr["c_class_id"])
                     }).ToList();

            _conn.Close();
            return Class;

        }

        public async Task<object> GetAllTeacher()
        {

            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_user as u join t_teacher as t on t.c_user_id = u.c_user_id;", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();

            if (datar.HasRows)
            {
                dt.Load(datar);
            }


            var Class = (from DataRow dr in dt.Rows
                         select new
                         {
                             c_teacher_name = dr["c_username"].ToString(),
                             c_teacher_id = Convert.ToInt32(dr["c_teacher_id"])
                         }).ToList();

            _conn.Close();
            return Class;

        }

        public async Task<List<t_section>> GetSectionByClass(string classId)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_section_id, c_class_id, c_section_name FROM t_section WHERE c_class_id = @c_class_id", _conn);

            cmd.Parameters.AddWithValue("@c_class_id", int.Parse(classId));
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();

            if (datar.HasRows)
            {
                dt.Load(datar);
            }

            List<t_section> section = new List<t_section>();

            section = (from DataRow dr in dt.Rows
                       select new t_section()
                       {
                           c_section_name = dr["c_section_name"].ToString(),
                           c_section_id = Convert.ToInt32(dr["c_section_id"]),
                           c_class_id = Convert.ToInt32(dr["c_class_id"])

                       }).ToList();

            _conn.Close();
            return section;
        }

        public async Task<int> Register(t_student student)
        {
            try
            {

                await _conn.CloseAsync();
                NpgsqlCommand comcheck = new NpgsqlCommand(@"SELECT * FROM t_student WHERE c_email = @c_email ;", _conn);
                comcheck.Parameters.AddWithValue("@c_email", student.c_email);
                await _conn.OpenAsync();
                using (NpgsqlDataReader datadr = await comcheck.ExecuteReaderAsync())
                {
                    if (datadr.HasRows)
                    {

                        await _conn.CloseAsync();
                        return 0;
                    }
                    else
                    {
                        await _conn.CloseAsync();
                        NpgsqlCommand cmd = new NpgsqlCommand(@"INSERT INTO t_student
            (c_user_id,c_full_name, c_email,c_password,c_date_of_birth,c_gender, c_class_id,c_section_id,  c_guardian_details,c_enrollment_date,c_image,c_status, c_teacher_id)
            VALUES (@c_user_id, @c_full_name, @c_email,@c_password,@c_date_of_birth,@c_gender,@c_class_id,@c_section_id,@c_guardian_details, @c_enrollment_date,@c_image, @c_status, @c_teacher_id)", _conn);
                        cmd.Parameters.AddWithValue("@c_user_id", student.c_user_id);
                        cmd.Parameters.AddWithValue("@c_full_name", student.c_full_name);
                        cmd.Parameters.AddWithValue("@c_email", student.c_email);
                        cmd.Parameters.AddWithValue("@c_password", student.c_password);

                        cmd.Parameters.AddWithValue("@c_date_of_birth", student.c_date_of_birth);
                        cmd.Parameters.AddWithValue("@c_gender", student.c_gender);
                        cmd.Parameters.AddWithValue("@c_class_id", student.c_class_id);
                        cmd.Parameters.AddWithValue("@c_class_id", student.c_class_id);
                        cmd.Parameters.AddWithValue("@c_teacher_id", student.c_teacher_id);

                        cmd.Parameters.AddWithValue("@c_section_id", student.c_section_id);
                        cmd.Parameters.AddWithValue("@c_guardian_details", student.c_guardian_details);
                        cmd.Parameters.AddWithValue("@c_enrollment_date", student.c_enrollment_date);

                        cmd.Parameters.AddWithValue("@c_image", student.c_image == null ? DBNull.Value : student.c_image);
                        cmd.Parameters.AddWithValue("@c_status", student.c_status);

                        await _conn.OpenAsync();
                        await cmd.ExecuteNonQueryAsync();
                        await _conn.CloseAsync();
                        return 1;
                    }
                }
            }
            catch (Exception e)
            {
                await _conn.CloseAsync();
                Console.WriteLine("Register Failed , Error :- " + e.Message);
                return -1;
            }

        }

        public async Task<int> Update(t_student studentData)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(@"UPDATE t_student SET c_user_id = @c_user_id, c_full_name = @c_full_name, c_email = @c_email, c_date_of_birth = @c_date_of_birth, c_gender = @c_gender, c_class_id = @c_class_id, c_section_id = @c_section_id, c_teacher_id=@c_teacher_id, c_guardian_details = @c_guardian_details, c_enrollment_date = @c_enrollment_date, c_image = @c_image, c_status = @c_status, c_password = @c_password  WHERE c_student_id = @c_student_id", _conn);
                cmd.Parameters.AddWithValue("@c_user_id", studentData.c_user_id);
                cmd.Parameters.AddWithValue("@c_full_name", studentData.c_full_name);
                cmd.Parameters.AddWithValue("@c_email", studentData.c_email);
                cmd.Parameters.AddWithValue("@c_date_of_birth", studentData.c_date_of_birth);
                cmd.Parameters.AddWithValue("@c_gender", studentData.c_gender);
                cmd.Parameters.AddWithValue("@c_class_id", studentData.c_class_id);
                cmd.Parameters.AddWithValue("@c_section_id", studentData.c_section_id);
                cmd.Parameters.AddWithValue("@c_teacher_id", studentData.c_teacher_id);
                cmd.Parameters.AddWithValue("@c_password", studentData.c_password);
                cmd.Parameters.AddWithValue("@c_guardian_details", studentData.c_guardian_details);
                cmd.Parameters.AddWithValue("@c_enrollment_date", studentData.c_enrollment_date);
                cmd.Parameters.AddWithValue("@c_image", studentData.c_image == null ? DBNull.Value : studentData.c_image);
                cmd.Parameters.AddWithValue("@c_status", studentData.c_status);
                cmd.Parameters.AddWithValue("@c_student_id", studentData.c_student_id);

                _conn.Close();
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }

        public async Task<int> Delete(string studentid)
        {
            try
            {
                using (var cmd = new NpgsqlCommand(@"DELETE FROM t_student WHERE c_student_id = @c_student_id", _conn))
                {
                    cmd.Parameters.AddWithValue("@c_student_id", int.Parse(studentid));
                    await _conn.OpenAsync();
                    int rowsAffected = await cmd.ExecuteNonQueryAsync();
                    await _conn.CloseAsync();
                    return rowsAffected > 0 ? 1 : 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting student: " + ex.Message);
                await _conn.CloseAsync();
                return 0;
            }
        }

        public async Task<List<t_student>> GetAll()
        {
            DataTable dt = new DataTable();
            string query = @"Select * from t_student as s join (Select * from t_user as u join t_teacher as t on u.c_user_id = t.c_user_id) as us on s.c_teacher_id = us.c_teacher_id join t_class as c on c.c_class_id = s.c_class_id join t_section as sec on s.c_section_id = sec.c_section_id;";

            using (NpgsqlCommand cmd = new NpgsqlCommand(query, _conn))
            {
                try
                {
                    _conn.Close();
                    _conn.Open();
                    using (NpgsqlDataReader datar = await cmd.ExecuteReaderAsync())
                    {
                        if (datar.HasRows)
                        {
                            dt.Load(datar);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    _conn.Close();
                }
            }

            List<t_student> studentList = (from DataRow dr in dt.Rows
                                           select new t_student()
                                           {
                                               c_student_id = Convert.ToInt32(dr["c_student_id"]),
                                               c_user_id = Convert.ToInt32(dr["c_user_id"]),
                                               c_full_name = dr["c_full_name"].ToString(),
                                               c_email = dr["c_email"].ToString(),
                                               c_password = dr["c_password"].ToString(),
                                               c_date_of_birth = Convert.ToDateTime(dr["c_date_of_birth"]),
                                               c_gender = dr["c_gender"].ToString(),
                                               c_class_id = Convert.ToInt32(dr["c_class_id"]),
                                               c_class_name = dr["c_class_name"].ToString(),
                                               c_section_id = Convert.ToInt32(dr["c_section_id"]),
                                               c_section_name = dr["c_section_name"].ToString(),
                                               c_teacher_id = Convert.ToInt32(dr["c_teacher_id"]),
                                               c_teacher_name = dr["c_username"].ToString(),

                                               c_guardian_details = dr["c_guardian_details"].ToString(),
                                               c_enrollment_date = Convert.ToDateTime(dr["c_enrollment_date"]),
                                               c_created_at = Convert.ToDateTime(dr["c_created_at"]),
                                               c_image = dr["c_image"] == DBNull.Value ? null : dr["c_image"].ToString(),
                                               c_status = Convert.ToBoolean(dr["c_status"])
                                           }).ToList();

            return studentList;
        }

        public async Task<t_student> GetOne(string studentid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_student as s join (Select * from t_user as u join t_teacher as t on u.c_user_id = t.c_user_id) as us on s.c_teacher_id = us.c_teacher_id join t_class as c on c.c_class_id = s.c_class_id join t_section as sec on s.c_section_id = sec.c_section_id where c_student_id = @c_student_id;", _conn);
            cmd.Parameters.AddWithValue("@c_student_id", int.Parse(studentid));
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
            t_student student = null;
            // bdfgh
            if (dr.Read())
            {
                student = new t_student()
                {
                    c_student_id = Convert.ToInt32(dr["c_student_id"]),
                    c_user_id = Convert.ToInt32(dr["c_user_id"]),
                    c_full_name = dr["c_full_name"].ToString(),
                    c_email = dr["c_email"].ToString(),
                    c_password = dr["c_password"].ToString(),
                    c_date_of_birth = Convert.ToDateTime(dr["c_date_of_birth"]),
                    c_gender = dr["c_gender"].ToString(),
                    c_class_id = Convert.ToInt32(dr["c_class_id"]),
                    c_class_name = dr["c_class_name"].ToString(),
                    c_teacher_id = Convert.ToInt32(dr["c_teacher_id"]),
                    c_teacher_name = dr["c_username"].ToString(),
                    c_section_id = Convert.ToInt32(dr["c_section_id"]),
                    c_section_name = dr["c_section_name"].ToString(),
                    c_guardian_details = dr["c_guardian_details"].ToString(),
                    c_enrollment_date = Convert.ToDateTime(dr["c_enrollment_date"]),
                    c_created_at = Convert.ToDateTime(dr["c_created_at"]),
                    c_image = dr["c_image"] == DBNull.Value ? null : dr["c_image"].ToString(),
                    c_status = Convert.ToBoolean(dr["c_status"])
                };
            }
            _conn.Close();
            return student;
        }

        public async Task<List<t_list>> GetData()
        {
            var values = new List<t_list>();

            if (_conn.State != System.Data.ConnectionState.Open)
                await _conn.OpenAsync();  // Use OpenAsync for better performance

            var query = @"
                SELECT 
                    'C_' || c.c_class_id AS id,  
                    c.c_class_name AS name, 
                    NULL AS parentId 
                FROM t_class c

                UNION ALL

                SELECT 
                    'S_' || s.c_section_id AS id,  
                    s.c_section_name AS name, 
                    'C_' || s.c_class_id AS parentId  
                FROM t_section s

                UNION ALL

                SELECT 
                    'ST_' || st.c_student_id AS id,  
                    st.c_full_name AS name, 
                    'S_' || st.c_section_id AS parentId  
                FROM t_student st

                ORDER BY parentId NULLS FIRST;
            ";

            using (var cmd = new NpgsqlCommand(query, _conn))
            {
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        values.Add(new t_list
                        {
                            Id = reader.GetString(0),  // 👈 Change to string
                            Name = reader.GetString(1),
                            ParentId = reader.IsDBNull(2) ? null : reader.GetString(2)  // 👈 Change to string
                        });
                    }
                }
            }
            _conn.Close();
            return values;
        }

        public async Task<int> GetStudentCount()
        {
            try
            {
                _conn.Close();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM t_student", _conn);
                _conn.Open();
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                _conn.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching student count: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }

        public async Task<byte[]> ExportStudentsToPDF()
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                    await _conn.OpenAsync();

                string query = "Select s.c_student_id, s.c_full_name, s.c_date_of_birth, s.c_gender, s.c_guardian_details, s.c_enrollment_date, s.c_email, s.c_password, c.c_class_name, sec.c_section_name, us.c_username from t_student as s join (Select * from t_user as u join t_teacher as t on u.c_user_id = t.c_user_id) as us on s.c_teacher_id = us.c_teacher_id join t_class as c on c.c_class_id = s.c_class_id join t_section as sec on s.c_section_id = sec.c_section_id";
                var students = new List<t_student>();

                using (var cmd = new NpgsqlCommand(query, _conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(new t_student
                        {
                            c_student_id = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("c_student_id"))),
                            c_full_name = reader.GetString(reader.GetOrdinal("c_full_name")),
                            c_email = reader.GetString(reader.GetOrdinal("c_email")),
                            c_date_of_birth = reader.GetDateTime(reader.GetOrdinal("c_date_of_birth")),
                            c_gender = reader.GetString(reader.GetOrdinal("c_gender")),
                            c_guardian_details = reader.GetString(reader.GetOrdinal("c_guardian_details")),
                            c_password = reader.GetString(reader.GetOrdinal("c_password")),
                            c_class_name = reader.GetString(reader.GetOrdinal("c_class_name")),
                            c_section_name = reader.GetString(reader.GetOrdinal("c_section_name")),
                            c_teacher_name = reader.GetString(reader.GetOrdinal("c_username")),
                            c_enrollment_date = reader.GetDateTime(reader.GetOrdinal("c_enrollment_date"))

                        });
                    }
                }

                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A2);
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    Font font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
                    doc.Add(new Paragraph("Student List", font));
                    doc.Add(new Paragraph("\n"));

                    PdfPTable table = new PdfPTable(11) { WidthPercentage = 100 };
                    table.AddCell("Student ID");
                    table.AddCell("Full Name");
                    table.AddCell("Date of birth");
                    table.AddCell("Gender");
                    table.AddCell("Guardian Details");
                    table.AddCell("Enrollment Date");
                    table.AddCell("Email");
                    table.AddCell("Password");
                    table.AddCell("Class Name");
                    table.AddCell("Section Name");
                    table.AddCell("Teacher Name");

                    foreach (var student in students)
                    {
                        table.AddCell(student.c_student_id.ToString());
                        table.AddCell(student.c_full_name);
                        table.AddCell(student.c_date_of_birth.ToShortDateString());
                        table.AddCell(student.c_gender);
                        table.AddCell(student.c_guardian_details);
                        table.AddCell(student.c_enrollment_date.ToShortDateString());
                        table.AddCell(student.c_email);
                        table.AddCell(student.c_password);
                        table.AddCell(student.c_class_name);
                        table.AddCell(student.c_section_name);
                        table.AddCell(student.c_teacher_name);
                    }

                    doc.Add(table);
                    doc.Close();

                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportStudentsToPDF: {ex.Message}");
                return null;
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }

        #region Student
        public async Task<t_Student> GetStudent(int id)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"SELECT * FROM t_student WHERE c_student_id = @id";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("id", id);
                    using (NpgsqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new t_Student
                            {
                                StudentId = reader.GetInt32(reader.GetOrdinal("c_student_id")),
                                FullName = reader.GetString(reader.GetOrdinal("c_full_name")),
                                DateOfBirth = reader.GetDateTime(reader.GetOrdinal("c_date_of_birth")),
                                Gender = reader.GetString(reader.GetOrdinal("c_gender")),
                                ClassId = reader.GetInt32(reader.GetOrdinal("c_class_id")),
                                SectionId = reader.GetInt32(reader.GetOrdinal("c_section_id")),
                                GuardianDetails = reader.GetString(reader.GetOrdinal("c_guardian_details")),
                                EnrollmentDate = reader.GetDateTime(reader.GetOrdinal("c_enrollment_date")),
                                Image = reader.IsDBNull(reader.GetOrdinal("c_image")) ? null : reader.GetString(reader.GetOrdinal("c_image")),
                                UserId = reader.GetInt32(reader.GetOrdinal("c_user_id")),
                                Password = reader.GetString(reader.GetOrdinal("c_password"))

                            };
                        }
                        else
                        {
                            return new t_Student();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Asseset Get Exception : " + ex.Message);
                return new t_Student();
            }
        }

        public async Task<int> GetStudentIdByUserId(int userId)
        {
            System.Console.WriteLine(userId + "usedid from function");
            int studentId = 0;
            string query = @"SELECT c_student_id FROM t_student WHERE c_user_id = @id";
            await _conn.OpenAsync();
            using (var cmd = new NpgsqlCommand(query, _conn))
            {
                cmd.Parameters.AddWithValue("@id", userId);
                var result = await cmd.ExecuteScalarAsync();
                if (result != null)
                {
                    studentId = Convert.ToInt32(result);
                }

            }
            await _conn.CloseAsync();
            System.Console.WriteLine(studentId + "student id from function");
            return studentId;
        }

        public async Task<List<t_SyllabusTracking>> GetSyllabusTrackingByStudent(int studentId)
        {
            if (_conn.State == System.Data.ConnectionState.Closed)
            {
                await _conn.OpenAsync();
            }
            try
            {
                string query = @"SELECT * FROM t_syllabustracking WHERE c_student_id = @studentId";
                using (var cmd = new NpgsqlCommand(query, _conn))
                {
                    cmd.Parameters.AddWithValue("studentId", studentId);
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        List<t_SyllabusTracking> syllabusList = new List<t_SyllabusTracking>();
                        while (await reader.ReadAsync())
                        {
                            syllabusList.Add(new t_SyllabusTracking
                            {
                                SyllabusId = reader.GetInt32(reader.GetOrdinal("c_syllabusid")),
                                StudentId = reader.GetInt32(reader.GetOrdinal("c_student_id")),
                                SubjectId = reader.GetInt32(reader.GetOrdinal("c_subject_id")),
                                Topic = reader.GetString(reader.GetOrdinal("c_topic")),
                                CompletionPercentage = reader.GetInt32(reader.GetOrdinal("c_completionpercentage")),
                                CompletionDate = reader.GetDateTime(reader.GetOrdinal("c_completiondate"))
                            });
                        }
                        return syllabusList;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetSyllabusTrackingByStudent Exception: " + ex.Message);
                return new List<t_SyllabusTracking>(); // Return an empty list instead of null
            }
        }
        public async Task<int> teacherFeedback(t_teacherFeedback feedback)
        {
            try
            {

                await using var cm = new NpgsqlCommand(@"
        INSERT INTO t_teacherFeedback 
        (c_teacher_id, c_student_id, c_rating, c_feedback_text, c_created_date, c_teacher_name) 
        VALUES 
        (@c_teacher_id, @c_student_id, @c_rating, @c_feedback_text, @c_created_date, 
        (SELECT u.c_userName FROM t_teacher t 
         JOIN t_user u ON t.c_user_id = u.c_user_id 
         WHERE t.c_teacher_id = @c_teacher_id))", _conn);

                cm.Parameters.AddWithValue("@c_teacher_id", feedback.C_Teacher_Id);
                cm.Parameters.AddWithValue("@c_student_id", feedback.C_Student_Id);
                cm.Parameters.AddWithValue("@c_rating", feedback.C_Rating);
                cm.Parameters.AddWithValue("@c_feedback_text", feedback.C_Feedback_Text);
                cm.Parameters.AddWithValue("@c_created_date", feedback.C_Created_Date);

                await _conn.OpenAsync();
                await cm.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return 0;
            }
        }
        public async Task<List<vm_subjectimetable>> GetTaskByClass(string classid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM t_subjectimetable", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();
            if (datar.HasRows)
            {
                dt.Load(datar);
            }
            List<vm_subjectimetable> taskList = new List<vm_subjectimetable>();
            taskList = (from DataRow dr in dt.Rows
                        where dr["c_class_id"].ToString() == classid
                        select new vm_subjectimetable()
                        {
                            c_Subject_Id = Convert.ToInt32(dr["c_subject_id"]),
                            c_Teacher_Id = Convert.ToInt32(dr["c_teacher_id"]),
                            c_Task_End_Time = (DateTime)(dr["c_task_end_time"] != DBNull.Value ? Convert.ToDateTime(dr["c_task_end_time"]) : (DateTime?)null),
                            c_Task_Start_Time = (DateTime)(dr["c_task_start_time"] != DBNull.Value ? Convert.ToDateTime(dr["c_task_start_time"]) : (DateTime?)null),
                            c_Task_Description = dr["c_task_description"].ToString()

                        }).ToList();
            _conn.Close();
            return taskList;
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
                await _conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(query, _conn))
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
                await _conn.CloseAsync();
            }
        }

        #endregion


    }
}