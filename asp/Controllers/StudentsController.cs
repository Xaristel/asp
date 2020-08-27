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
        private IStudentRepository studentRepository;

        public StudentsController()
        {
            this.studentRepository = new StudentRepository(new DataContext());
        }

        // GET: Students
        public IActionResult Index()
        {
            return View(studentRepository.GetStudents());
        }

        // GET: Students/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var student = studentRepository.GetStudentByID(id);
                return View(student);
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
                studentRepository.AddStudent(student);
                studentRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var student = studentRepository.GetStudentByID(id);
                return View(student);
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
                    studentRepository.UpdateStudent(student);
                    studentRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.StudentId))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var student = studentRepository.GetStudentByID(id);
                return View(student);
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
            var student = studentRepository.GetStudentByID(id);
            studentRepository.DeleteStudent(student);
            studentRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            try
            {
                studentRepository.GetStudentByID(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
