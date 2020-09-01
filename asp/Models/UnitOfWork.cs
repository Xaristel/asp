using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public class UnitOfWork : IDisposable
    {
        private DataContext context = new DataContext();
        private Repository<Student> studentRepository;
        private Repository<Group> groupRepository;
        private Repository<Course> courseRepository;

        public Repository<Student> StudentRepository
        {
            get
            {
                if (this.studentRepository == null)
                {
                    this.studentRepository = new Repository<Student>(context);
                }
                return this.studentRepository;
            }
        }

        public Repository<Group> GroupRepository
        {
            get
            {
                if (this.groupRepository == null)
                {
                    this.groupRepository = new Repository<Group>(context);
                }
                return this.groupRepository;
            }
        }

        public Repository<Course> CourseRepository
        {
            get
            {
                if (this.courseRepository == null)
                {
                    this.courseRepository = new Repository<Course>(context);
                }
                return this.courseRepository;
            }
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
