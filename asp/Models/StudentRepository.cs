using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private DataContext context;

        public StudentRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Student> GetStudents()
        {
            return context.Student.ToList();
        }

        public Student GetStudentByID(int id)
        {
            return context.Student.Find(id);
        }

        public void AddStudent(Student student)
        {
            context.Student.Add(student);
        }

        public void DeleteStudent(Student student)
        {
            Student _student = context.Student.Find(student.StudentId);
            context.Student.Remove(_student);
        }

        public void UpdateStudent(Student student)
        {
            context.Entry(student).State = (System.Data.Entity.EntityState)EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
