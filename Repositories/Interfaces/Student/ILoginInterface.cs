using Edutrack.Models;

namespace EdutrackPro;

public interface ILoginInterface
{
    public Task<int> Login(t_Login login);

    public Task<string> CheckTeacherStatus(int userID);

}