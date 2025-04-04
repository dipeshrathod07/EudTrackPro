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
    public class TeacherRepository : ITeacherInterface
    {
        private readonly NpgsqlConnection _conn;
        public TeacherRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        #region GetAll

        public async Task<List<t_teacher>> GetAll()
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("select * from t_teacher as t join t_user as u on t.c_user_id = u.c_user_id;", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader datar = cmd.ExecuteReader();
            if (datar.HasRows)
            {
                dt.Load(datar);
            }
            List<t_teacher> teacherList = new List<t_teacher>();
            teacherList = (from DataRow dr in dt.Rows
                           select new t_teacher()
                           {
                               c_teacher_id = Convert.ToInt32(dr["c_teacher_id"]),
                               c_user_id = int.Parse(dr["c_user_id"].ToString()),
                               c_teacher_name = dr["c_username"].ToString(),
                               c_email = dr["c_email"].ToString(),

                               c_phone_number = dr["c_phone_number"].ToString(),
                               c_date_of_birth = Convert.ToDateTime(dr["c_date_of_birth"]),
                               c_qualification = dr["c_qualification"].ToString(),
                               c_experience = Convert.ToInt32(dr["c_experience"]),
                               c_subject_expertise = dr["c_subject_expertise"].ToString(),
                               c_status = dr["c_status"].ToString()
                           }).ToList();
            _conn.Close();
            return teacherList;

        }
        #endregion
        #region GetOneTeacher
        public async Task<t_teacher> GetOne(string teacherid)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT * FROM t_teacher WHERE c_teacher_id = @c_teacher_id", _conn);
            cmd.Parameters.AddWithValue("@c_teacher_id", int.Parse(teacherid));
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();
            t_teacher teacher = null;
            if (dr.Read())
            {
                teacher = new t_teacher()
                {
                    c_teacher_id = Convert.ToInt32(dr["c_teacher_id"]),
                    c_user_id = int.Parse(dr["c_user_id"].ToString()),
                    c_phone_number = dr["c_phone_number"].ToString(),
                    c_date_of_birth = Convert.ToDateTime(dr["c_date_of_birth"]),
                    c_qualification = dr["c_qualification"].ToString(),
                    c_experience = Convert.ToInt32(dr["c_experience"]),
                    c_subject_expertise = dr["c_subject_expertise"].ToString(),
                    c_status = dr["c_status"].ToString()
                };
            }
            _conn.Close();
            return teacher;
        }
        #endregion
        #region Update
        public async Task<int> UpdateStatus(string teacherId, string status)
        {
            try
            {

                await _conn.OpenAsync();
                using (var cmd = new NpgsqlCommand(@"UPDATE t_teacher SET 
                                                c_status = @c_status
                                                WHERE c_teacher_id = @c_teacher_id", _conn))
                {
                    cmd.Parameters.AddWithValue("@c_status", status);
                    cmd.Parameters.AddWithValue("@c_teacher_id", int.Parse(teacherId));

                    await cmd.ExecuteNonQueryAsync();
                    return 1; // Success
                }

            }
            catch (Exception ex)
            {
                return 0; // Failure
            }
        }
        #endregion
        #region Delete
        public async Task<int> Delete(string teacherid)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand(@"DELETE FROM t_teacher WHERE c_teacher_id = @c_teacher_id", _conn);

                cmd.Parameters.AddWithValue("@c_teacher_id", int.Parse(teacherid));
                _conn.Close();
                _conn.Open();
                cmd.ExecuteNonQuery();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }



        }
        #endregion
        #region  GetPendingTeacherCount...
        public async Task<int> GetPendingTeacherCount()
        {
            try
            {
                await _conn.OpenAsync();
                using (var cmd = new NpgsqlCommand("SELECT COUNT(*) FROM t_teacher WHERE c_status ILIKE 'Pending'", _conn))
                {
                    var count = await cmd.ExecuteScalarAsync();
                    return Convert.ToInt32(count);
                }
            }
            catch (Exception ex)
            {
                // Log the error
                return 0;
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }
        #endregion

        #region GetTeacherCount
        public async Task<int> GetTeacherCount()
        {
            try
            {
                _conn.Close();
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM t_teacher", _conn);
                _conn.Open();
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                _conn.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching teacher count: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }
        #endregion

        #region GetTeacher
        public async Task<List<object>> GetTeacher()
        {
            var teachers = new List<object>();
            string query = @"SELECT t.c_teacher_id, u.c_username FROM t_teacher t 
                            INNER JOIN t_user u 
                            ON t.c_user_id = u.c_user_id";

            _conn.Close();
            _conn.Open();
            using (var cmd = new NpgsqlCommand(query, _conn))
            {

                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (reader.Read())
                    {
                        teachers.Add(new
                        {
                            c_teacher_id = reader.GetInt32(0),
                            c_teacher_name = reader.GetString(1) // Ye naam bhejne ke liye
                        });
                    }
                }

                _conn.Close();
            }
            return teachers;
        }
        #endregion

        #region ExportTeachersToPDF
        public async Task<byte[]> ExportTeachersToPDF()
        {
            try
            {
                if (_conn.State == System.Data.ConnectionState.Closed)
                    await _conn.OpenAsync();

                // Query to fetch teacher data
                string query = "SELECT t.c_teacher_id, u.c_username AS c_teacher_name, u.c_email, t.c_phone_number, t.c_date_of_birth, t.c_qualification, t.c_experience, t.c_subject_expertise, t.c_status FROM t_teacher AS t JOIN t_user AS u ON t.c_user_id = u.c_user_id;";

                var teachers = new List<t_teacher>();

                using (var cmd = new NpgsqlCommand(query, _conn))
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        teachers.Add(new t_teacher
                        {
                            c_teacher_id = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("c_teacher_id"))),
                            // c_user_id = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("c_user_id"))),
                            c_teacher_name = reader.GetString(reader.GetOrdinal("c_teacher_name")),
                            c_email = reader.GetString(reader.GetOrdinal("c_email")),
                            c_phone_number = reader.GetString(reader.GetOrdinal("c_phone_number")),
                            c_date_of_birth = reader.GetDateTime(reader.GetOrdinal("c_date_of_birth")),
                            c_qualification = reader.GetString(reader.GetOrdinal("c_qualification")),
                            c_experience = Convert.ToInt32(reader.GetInt32(reader.GetOrdinal("c_experience"))),
                            c_subject_expertise = reader.GetString(reader.GetOrdinal("c_subject_expertise")),
                            c_status = reader.GetString(reader.GetOrdinal("c_status"))
                        });
                    }
                }

                // Create PDF document
                using (MemoryStream ms = new MemoryStream())
                {
                    Document doc = new Document(PageSize.A2);
                    PdfWriter.GetInstance(doc, ms);
                    doc.Open();

                    // Add title to the document
                    Font font = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    doc.Add(new Paragraph("Teacher List", font));
                    doc.Add(new Paragraph("\n"));

                    // Create a table with columns for teacher data
                    PdfPTable table = new PdfPTable(9) { WidthPercentage = 100 };
                    table.AddCell("Teacher ID");
                    // table.AddCell("User ID");
                    table.AddCell("Teacher Name");
                    table.AddCell("Email");
                    table.AddCell("Phone Number");
                    table.AddCell("Date of Birth");
                    table.AddCell("Qualification");
                    table.AddCell("Experience");
                    table.AddCell("Subject Expertise");
                    table.AddCell("Status");

                    // Add teacher data to the table
                    foreach (var teacher in teachers)
                    {
                        table.AddCell(teacher.c_teacher_id.ToString());
                        // table.AddCell(teacher.c_user_id.ToString());
                        table.AddCell(teacher.c_teacher_name);
                        table.AddCell(teacher.c_email);
                        table.AddCell(teacher.c_phone_number);
                        table.AddCell(teacher.c_date_of_birth.ToShortDateString());
                        table.AddCell(teacher.c_qualification);
                        table.AddCell(teacher.c_experience.ToString());
                        table.AddCell(teacher.c_subject_expertise);
                        table.AddCell(teacher.c_status);
                    }

                    // Add the table to the document
                    doc.Add(table);
                    doc.Close();

                    // Return the PDF as a byte array
                    return ms.ToArray();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ExportTeachersToPDF: {ex.Message}");
                return null;
            }
            finally
            {
                await _conn.CloseAsync();
            }
        }
        #endregion
    }
}