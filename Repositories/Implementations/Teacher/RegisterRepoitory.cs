using Edutrack.Models;
using Npgsql;
namespace Edutrack;

public class RegisterRepository : IRegisterInterface
{
    private readonly NpgsqlConnection _conn;

    public RegisterRepository(NpgsqlConnection conn)
    {
        _conn = conn;
    }

    public async Task<int> RegisterRepo(t_Register RegisterData)
    {
        int result = 0;
        try
        {
            await _conn.CloseAsync();
            // check email
            NpgsqlCommand CheckeEmailCmd = new NpgsqlCommand(@"SELECT c_user_id FROM t_user WHERE c_email = @c_email", _conn);
            CheckeEmailCmd.Parameters.AddWithValue("@c_email", RegisterData.C_UserEmail);
            await _conn.OpenAsync();
            object? CheckEmailResultObj = await CheckeEmailCmd.ExecuteScalarAsync();
            int CheckEmailResult = CheckEmailResultObj != null ? (int)CheckEmailResultObj : 0;
            if (CheckEmailResult != 0)
            {
                return 2; // email already exists
            }
            await _conn.CloseAsync();
            // insert querry
            NpgsqlCommand RegisterCmd = new NpgsqlCommand(@"INSERT INTO t_user (c_userName,c_password,c_email,c_role) VALUES 
            (@c_userName,@c_password,@c_email,@c_role)", _conn);
            RegisterCmd.Parameters.AddWithValue("@c_username", RegisterData.C_UserName);
            RegisterCmd.Parameters.AddWithValue("@c_password", RegisterData.C_Password);
            RegisterCmd.Parameters.AddWithValue("@c_email", RegisterData.C_UserEmail);
            RegisterCmd.Parameters.AddWithValue("@c_role", "Teacher");
            await _conn.OpenAsync();
            result = await RegisterCmd.ExecuteNonQueryAsync();
            await _conn.CloseAsync();

            // getting userid for inserting into t_teacher table for pending status
            NpgsqlCommand GetUserIdCmd = new NpgsqlCommand(@"SELECT c_user_id FROM t_user
            WHERE
            c_username = @c_username AND
            c_password = @c_password AND
            c_email = @c_email", _conn);
            GetUserIdCmd.Parameters.AddWithValue("@c_username", RegisterData.C_UserName);
            GetUserIdCmd.Parameters.AddWithValue("@c_password", RegisterData.C_Password);
            GetUserIdCmd.Parameters.AddWithValue("@c_email", RegisterData.C_UserEmail);
            await _conn.OpenAsync();
            object TempUserID = await GetUserIdCmd.ExecuteScalarAsync();
            int UserID = (int)TempUserID;
            await _conn.CloseAsync();

            if (result == 1)
            {
                // inserting into t_teacher table for pending status
                NpgsqlCommand Register2Cmd = new NpgsqlCommand(@"INSERT INTO t_teacher 
                (c_user_id, c_phone_number, c_date_of_birth, c_qualification, c_experience, c_subject_expertise)
                VALUES 
                (@c_user_id, '0000000002', '2000-01-01', 'None', 0, 'None');", _conn);
                Register2Cmd.Parameters.AddWithValue("@c_user_id", UserID);
                await _conn.OpenAsync();
                result = await Register2Cmd.ExecuteNonQueryAsync();
                await _conn.CloseAsync();
                return 1; // successful
            }
            else
            {
                return 0; //no data was inserted
            }
        }
        catch (Exception ex)
        {
            System.Console.WriteLine("Register Repo Error : " + ex.Message);
            return 0;
        }
        finally
        {
            await _conn.CloseAsync();
        }
    }
}