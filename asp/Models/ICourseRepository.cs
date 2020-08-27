using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    interface ICourseRepository : IDisposable
    {
        IEnumerable<Course> GetCourses();
        Course GetCourseByID(int CourseId);
        void AddCourse(Course course);
        void DeleteCourse(Course course);
        void UpdateCourse(Course course);
        void Save();
    }
}
