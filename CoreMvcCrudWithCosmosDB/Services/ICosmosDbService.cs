using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreMvcCrudWithCosmosDB.Models;


namespace CoreMvcCrudWithCosmosDB.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Student>> GetStudentsAsync(string selectQuery);
        Task<Student> GetStudentAsync(string id);
        Task AddStudentAsync(Student student);
        Task UpdateStudentAsync(string id, Student student);
        Task DeleteStudentAsync(string id);
    }
}
