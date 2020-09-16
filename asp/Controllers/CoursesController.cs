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
        private UnitOfWork unitOfWork;

        public CoursesController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Courses
        public IActionResult Index()
        {
            return View("Index", unitOfWork.CourseRepository.GetAll());
        }

        // GET: Courses/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var course = unitOfWork.CourseRepository.GetByID(id);
                return View("Details", course);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View("Create");
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
                unitOfWork.CourseRepository.Add(course);
                unitOfWork.CourseRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", course);
        }

        // GET: Courses/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var course = unitOfWork.CourseRepository.GetByID(id);
                return View("Edit", course);
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
                    unitOfWork.CourseRepository.Update(course);
                    unitOfWork.CourseRepository.Save();
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
            return View("Edit", @course);
        }

        // GET: Courses/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var course = unitOfWork.CourseRepository.GetByID(id);
                var groups = from m in unitOfWork.GroupRepository.GetAll()
                             where m.CourseId == id
                             select m;

                if (groups.ToList().Count == 0)
                {
                    return View("Delete", @course);
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
            var course = unitOfWork.CourseRepository.GetByID(id);
            unitOfWork.CourseRepository.Delete(course);
            unitOfWork.CourseRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public bool CourseExists(int id)
        {
            try
            {
                unitOfWork.CourseRepository.GetByID(id);
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

            var groups = from m in unitOfWork.GroupRepository.GetAll()
                         where m.CourseId == id
                         select m;

            return View("Show", groups);
        }
    }
}
