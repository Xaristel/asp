using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using asp.Models;

namespace asp.Controllers
{
    public class CoursesController : Controller
    {
        private IGroupRepository groupRepository;
        private ICourseRepository courseRepository;

        public CoursesController()
        {
            this.groupRepository = new GroupRepository(new DataContext());
            this.courseRepository = new CourseRepository(new DataContext());
        }

        // GET: Courses
        public IActionResult Index()
        {
            return View(courseRepository.GetCourses());
        }

        // GET: Courses/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var course = courseRepository.GetCourseByID(id);
                return View(course);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("CourseId,CourseName,CourseDescription")] Course course)
        {
            if (ModelState.IsValid)
            {
                courseRepository.AddCourse(course);
                courseRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var course = courseRepository.GetCourseByID(id);
                return View(course);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("CourseId,CourseName,CourseDescription")] Course @course)
        {
            if (id != @course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    courseRepository.UpdateCourse(course);
                    courseRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(@course.CourseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var course = courseRepository.GetCourseByID(id);
                var groups = from m in groupRepository.GetGroups()
                             where m.CourseId == id
                             select m;

                if (groups.ToList().Count == 0)
                {
                    return View(@course);
                }
                else
                {
                    return Content("Error. This course has groups!");
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var course = courseRepository.GetCourseByID(id);
            courseRepository.DeleteCourse(course);
            courseRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            try
            {
                courseRepository.GetCourseByID(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // GET: Courses/Show
        public IActionResult Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var groups = from m in groupRepository.GetGroups()
                         where m.CourseId == id
                         select m;

            return View(groups);
        }
    }
}
