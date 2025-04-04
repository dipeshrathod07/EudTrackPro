using Npgsql;

namespace Repositories.Implementations;

public class SubjectRepository
{
    private readonly NpgsqlConnection _conn;
    public SubjectRepository(NpgsqlConnection connection)
    {
        _conn = connection;
    }
    public async Task<List<object>> GetStudent()
    {
        var subjects = new List<object>();
        string query = "SELECT c_subject_id, c_subject_name FROM t_subject";

        using (var cmd = new NpgsqlCommand(query, _conn))
        {
            _conn.Close();
            _conn.Open();
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (reader.Read())
                {
                    subjects.Add(new
                    {
                        SubjectId = reader.GetInt32(0),
                        SubjectName = reader.GetString(1) // Ye naam bhejne ke liye
                    });
                }
            }
        }
        _conn.Close();
        return subjects;
    }
}
