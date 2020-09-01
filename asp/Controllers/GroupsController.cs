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
    public class GroupsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Groups
        public IActionResult Index()
        {
            return View(unitOfWork.GroupRepository.GetAll());
        }

        // GET: Groups/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var group = unitOfWork.GroupRepository.GetByID(id);
                return View(group);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // GET: Groups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("GroupId,CourseId,GroupName")] Group group)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.GroupRepository.Add(group);
                unitOfWork.GroupRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var group = unitOfWork.GroupRepository.GetByID(id);
                return View(group);
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("GroupId,CourseId,GroupName")] Group @group)
        {
            if (id != @group.GroupId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    unitOfWork.GroupRepository.Update(group);
                    unitOfWork.GroupRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
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
            return View(@group);
        }

        // GET: Groups/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var group = unitOfWork.GroupRepository.GetByID(id);
                var students = from m in unitOfWork.StudentRepository.GetAll()
                               where m.GroupId == id
                               select m;
                if (students.ToList().Count == 0)
                {
                    return View(@group);
                }
                else
                {
                    return Content("Error. This group has students!");
                }
            }
            catch (ArgumentNullException)
            {
                throw new ArgumentNullException();
            }
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var group = unitOfWork.GroupRepository.GetByID(id);
            unitOfWork.GroupRepository.Delete(group);
            unitOfWork.GroupRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            try
            {
                unitOfWork.GroupRepository.GetByID(id);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IActionResult Show(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var students = from m in unitOfWork.StudentRepository.GetAll()
                           where m.GroupId == id
                           select m;

            return View(students);
        }
    }
}
