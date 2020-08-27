using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public class CourseRepository : ICourseRepository, IDisposable
    {
        private DataContext context;

        public CourseRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Course> GetCourses()
        {
            return context.Course.ToList();
        }

        public Course GetCourseByID(int id)
        {
            return context.Course.Find(id);
        }

        public void AddCourse(Course course)
        {
            context.Course.Add(course);
        }

        public void DeleteCourse(Course course)
        {
            Course _course = context.Course.Find(course.CourseId);
            context.Course.Remove(_course);
        }

        public void UpdateCourse(Course course)
        {
            context.Entry(course).State = EntityState.Modified;
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
