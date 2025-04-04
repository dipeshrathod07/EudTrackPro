using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ExamRepository : IExamInterface
    {
        private readonly NpgsqlConnection _conn;
        public ExamRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }

        public async Task<int> AddExam(t_exam exam)
        {
            try
            {


                await _conn.CloseAsync();
                NpgsqlCommand cmd = new NpgsqlCommand(@"INSERT INTO t_exam_timetable
            (c_class_id,c_exam_image)
            VALUES ( @c_class_id,@c_exam_image)", _conn);
               cmd.Parameters.AddWithValue("@c_class_id", exam.c_class_id);
               cmd.Parameters.AddWithValue("@c_exam_image", exam.c_exam_image == null ? DBNull.Value : exam.c_exam_image);

                await _conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1;


            }
            catch (Exception e)
            {
                await _conn.CloseAsync();
                Console.WriteLine("Insertation Failed , Error :- " + e.Message);
                return -1;
            }
        }
    }
}