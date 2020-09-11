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
    public class StudentsController : Controller
    {
        private UnitOfWork unitOfWork;

        public StudentsController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        // GET: Students
        public IActionResult Index()
        {
            return View("Index", unitOfWork.StudentRepository.GetAll());
        }

        // GET: Students/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var student = unitOfWork.StudentRepository.GetByID(id);
                return View("Details", student);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("StudentId,FirstName,LastName,GroupId")] Student student)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.StudentRepository.Add(student);
                unitOfWork.StudentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var student = unitOfWork.StudentRepository.GetByID(id);
                return View("Edit", student);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("StudentId,FirstName,LastName,GroupId")] Student student)
        {
            if (id != student.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.StudentRepository.Update(student);
                    unitOfWork.StudentRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw new DbUpdateConcurrencyException();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", student);
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var student = unitOfWork.StudentRepository.GetByID(id);
                return View("Delete", student);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = unitOfWork.StudentRepository.GetByID(id);
            unitOfWork.StudentRepository.Delete(student);
            unitOfWork.StudentRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        public bool StudentExists(int id)
        {
            try
            {
                unitOfWork.StudentRepository.GetByID(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
