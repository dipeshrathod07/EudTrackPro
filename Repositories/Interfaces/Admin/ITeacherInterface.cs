using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Repositories.Models;

namespace Repositories.Interfaces
{
    public interface ITeacherInterface
    {
        Task<t_teacher> GetOne(string teacherid);

        Task<List<t_teacher>> GetAll();
        Task<int> UpdateStatus(string id, string status);
        Task<int> Delete(string teacherid);
        Task<int> GetPendingTeacherCount();
        Task<int> GetTeacherCount();
        Task<List<object>> GetTeacher();
        Task<byte[]> ExportTeachersToPDF();

    }
}