using Npgsql;
using Repositories.Models;

namespace Repositories.Implementations;

public class TeacherSubjectRepository
{
    private readonly NpgsqlConnection _conn;
    public TeacherSubjectRepository(NpgsqlConnection connection)
    {
        _conn = connection;
    }

    public async Task<int> Add(t_assignSubject ts)
    {
        string query = "INSERT INTO t_teacher_subject (c_teacher_id, c_subject_id) VALUES (@teacherId, @subjectId)";
        using (var cmd = new NpgsqlCommand(query, _conn))
        {
            _conn.Close();
            _conn.Open();
            cmd.Parameters.AddWithValue("@teacherId", ts.TeacherId);
            cmd.Parameters.AddWithValue("@subjectId", ts.SubjectId);

            await cmd.ExecuteNonQueryAsync();
            _conn.Close();
            return 1;
        }
        return 0;
    }
}
