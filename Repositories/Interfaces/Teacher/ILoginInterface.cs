using Edutrack.Models;

namespace Edutrack;

public interface ILoginInterface
{
    public Task<int> Login(t_Login login);

    public Task<string> CheckTeacherStatus(int userID);

}