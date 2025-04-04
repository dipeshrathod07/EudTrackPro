using Edutrack.Models;
namespace Edutrack;

public interface IRegisterInterface
{
    Task<int> RegisterRepo(t_Register RegisterData);
}