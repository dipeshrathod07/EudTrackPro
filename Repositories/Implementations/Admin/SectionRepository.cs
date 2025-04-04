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
    public class SectionRepository : ISectionInterface
    {

        private readonly NpgsqlConnection _conn;
        public SectionRepository(NpgsqlConnection connection)
        {
            _conn = connection;
        }
        public async Task<int> Create(t_section section)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("INSERT INTO t_section (c_section_name, c_class_id) VALUES (@c_section_name, @c_class_id)", _conn);
                cmd.Parameters.AddWithValue("@c_section_name", section.c_section_name);
                cmd.Parameters.AddWithValue("@c_class_id", section.c_class_id);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error creating section: " + ex.Message);
                _conn.Close();
                return 0;
            }
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

        public async Task<int> Delete(int id)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("DELETE FROM t_section WHERE c_section_id = @c_section_id", _conn);
                cmd.Parameters.AddWithValue("@c_section_id", id);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting section: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }

        public async Task<List<t_section>> GetAll()
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("Select * from t_section as sec join t_class as cls on cls.c_class_id=sec.c_class_id;", _conn);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                dt.Load(dr);
            }

            List<t_section> sections = new List<t_section>();
            sections = (from DataRow row in dt.Rows
                        select new t_section()
                        {
                            c_section_id = Convert.ToInt32(row["c_section_id"]),
                            c_section_name = row["c_section_name"].ToString(),
                            c_class_id = Convert.ToInt32(row["c_class_id"]),
                            c_class_name = row["c_class_name"].ToString()
                        }).ToList();

            _conn.Close();
            return sections;
        }

        public async Task<t_section> GetOne(int id)
        {
            DataTable dt = new DataTable();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT c_section_id, c_section_name, c_class_id FROM t_section WHERE c_section_id = @c_section_id", _conn);
            cmd.Parameters.AddWithValue("@c_section_id", id);
            _conn.Close();
            _conn.Open();
            NpgsqlDataReader dr = cmd.ExecuteReader();

            t_section section = null;
            if (dr.Read())
            {
                section = new t_section()
                {
                    c_section_id = Convert.ToInt32(dr["c_section_id"]),
                    c_section_name = dr["c_section_name"].ToString(),
                    c_class_id = Convert.ToInt32(dr["c_class_id"])
                };
            }

            _conn.Close();
            return section;
        }

        public async Task<int> Update(t_section section)
        {
            try
            {
                NpgsqlCommand cmd = new NpgsqlCommand("UPDATE t_section SET c_section_name = @c_section_name, c_class_id = @c_class_id WHERE c_section_id = @c_section_id", _conn);
                cmd.Parameters.AddWithValue("@c_section_name", section.c_section_name);
                cmd.Parameters.AddWithValue("@c_class_id", section.c_class_id);
                cmd.Parameters.AddWithValue("@c_section_id", section.c_section_id);
                _conn.Close();
                _conn.Open();
                await cmd.ExecuteNonQueryAsync();
                _conn.Close();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating section: " + ex.Message);
                _conn.Close();
                return 0;
            }
        }
    }
}