using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Npgsql;
using Repositories.Interfaces;
using Repositories.Models;

namespace Repositories.Implementations
{
    public class ClassRepository : IClassInterface
    {
        private readonly NpgsqlConnection _conn;
        public ClassRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }
        public async Task<int> Create(t_class classData)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_class (c_class_name) VALUES (@c_class_name)", _conn);
                cmd.Parameters.AddWithValue("@c_class_name", classData.c_class_name);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating class: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }

        public async Task<int> Delete(int id)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM t_class WHERE c_class_id = @c_class_id", _conn);
                cmd.Parameters.AddWithValue("@c_class_id", id);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting class: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }

        public async Task<List<t_class>> GetAll()
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_class_id, c_class_name FROM t_class", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dt.Load(dr);
            }

            List<t_class> classes = new List<t_class>();
            classes = (from DataRow row in dt.Rows
                       select new t_class()
                       {
                           c_class_id = Convert.ToInt32(row["c_class_id"]),
                           c_class_name = row["c_class_name"].ToString()
                       }).ToList();

            _conn.Close();
            return classes;
        }

        public async Task<t_class> GetOne(int id)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_class_id, c_class_name FROM t_class WHERE c_class_id = @c_class_id", _conn);
            cmd.Parameters.AddWithValue("@c_class_id", id);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            t_class classData = null;
            if (dr.Read())
            {
                classData = new t_class()
                {
                    c_class_id = Convert.ToInt32(dr["c_class_id"]),
                    c_class_name = dr["c_class_name"].ToString()
                };
            }

            _conn.Close();
            return classData;
        }

        public async Task<int> Update(t_class classData)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE t_class SET c_class_name = @c_class_name WHERE c_class_id = @c_class_id", _conn);
                cmd.Parameters.AddWithValue("@c_class_name", classData.c_class_name);
                cmd.Parameters.AddWithValue("@c_class_id", classData.c_class_id);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating class: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }


        public async Task<int> GetClassCount()
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("SELECT COUNT(*) FROM t_class", _conn);
                _conn.Open();
                int count = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                _conn.Close();
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error fetching class count: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }
    }
}