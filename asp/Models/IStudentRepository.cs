using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    interface IStudentRepository : IDisposable
    {
        IEnumerable<Student> GetStudents();
        Student GetStudentByID(int studentId);
        void AddStudent(Student student);
        void DeleteStudent(Student student);
        void UpdateStudent(Student student);
        void Save();
    }
}
