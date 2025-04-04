using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface IClassInterface
    {
        Task<List<t_class>> GetAll();
        Task<t_class> GetOne(int id);
        Task<int> Create(t_class classData);
        Task<int> Update(t_class classData);
        Task<int> Delete(int id);
         Task<int> GetClassCount();
    }
}