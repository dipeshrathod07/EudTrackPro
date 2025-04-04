using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface INotificationInterface
    {
        Task<int> Add(t_notification data);

        Task<List<t_notification>> GetAll();
        Task<int> Delete(int id);
    }
}