using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ISectionInterface
    {
        Task<List<t_section>> GetAll();
        Task<t_section> GetOne(int id);
        Task<int> Create(t_section section);
        Task<int> Update(t_section section);
        Task<int> Delete(int id);
        Task<List<t_class>> GetAllClass();
    }
}