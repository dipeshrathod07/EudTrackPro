using Npgsql;
using Edutrack.Models;


namespace EdutrackPro;

public class LoginRepository : ILoginInterface
{
    private readonly NpgsqlConnection _con;


    public LoginRepository(NpgsqlConnection con)
    {
        _con = con;

    }


    public async Task<int> Login(t_Login login)
    {
        try
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("SELECT c_user_id FROM t_user WHERE c_email = @email AND c_password = @password AND c_role = @role", _con))
            {
                cmd.Parameters.AddWithValue("email", login.Email);
                cmd.Parameters.AddWithValue("password", login.Password);
                cmd.Parameters.AddWithValue("role", login.Role);

                object tempUserID = await cmd.ExecuteScalarAsync();

                if (tempUserID == null || tempUserID == DBNull.Value)
                {
                    return 0; // User not found
                }
                return Convert.ToInt32(tempUserID); // Return user ID

            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return 0;
        }
        finally
        {
            if (_con.State == System.Data.ConnectionState.Open)
            {
                await _con.CloseAsync();
            }
        }
    }

    public async Task<string> CheckTeacherStatus(int userID)
    {
        try
        {
            if (_con.State != System.Data.ConnectionState.Open)
            {
                await _con.OpenAsync();
            }

            using (var cmd = new NpgsqlCommand("SELECT c_status FROM t_teacher WHERE c_user_id = @userID", _con))
            {
                cmd.Parameters.AddWithValue("userID", userID);
                object status = await cmd.ExecuteScalarAsync();

                if (status == null || status == DBNull.Value)
                {
                    return "Pending"; // Default to pending if no status is found
                }

                return status.ToString(); // Return status value
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return "Pending"; // Default error handling
        }
        finally
        {
            if (_con.State == System.Data.ConnectionState.Open)
            {
                await _con.CloseAsync();
            }
        }
    }



}

internal interface IHttpContextAccessor
{
}


// code
//     NpgsqlCommand GetUserIdCmd = new NpgsqlCommand(@"SELECT c_user_id FROM t_user
// WHERE
// c_username = @c_username AND
// c_password = @c_password AND
// c_email = @c_email", _conn);
// GetUserIdCmd.Parameters.AddWithValue("@c_username", RegisterData.C_UserName);
// GetUserIdCmd.Parameters.AddWithValue("@c_password", RegisterData.C_Password);
// GetUserIdCmd.Parameters.AddWithValue("@c_email", RegisterData.C_UserEmail);
// await _conn.OpenAsync();
// object TempUserID = await GetUserIdCmd.ExecuteScalarAsync();
// int UserID = (int)TempUserID;
// await _conn.CloseAsync();