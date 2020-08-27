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
        private IGroupRepository groupRepository;
        private IStudentRepository studentRepository;

        public GroupsController()
        {
            this.groupRepository = new GroupRepository(new DataContext());
            this.studentRepository = new StudentRepository(new DataContext());
        }

        // GET: Groups
        public IActionResult Index()
        {
            return View(groupRepository.GetGroups());
        }

        // GET: Groups/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                var group = groupRepository.GetGroupByID(id);
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
                groupRepository.AddGroup(group);
                groupRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(group);
        }

        // GET: Groups/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                var group = groupRepository.GetGroupByID(id);
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
                    groupRepository.UpdateGroup(group);
                    groupRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(@group.GroupId))
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
            return View(@group);
        }

        // GET: Groups/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                var group = groupRepository.GetGroupByID(id);
                var students = from m in studentRepository.GetStudents()
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
            var group = groupRepository.GetGroupByID(id);
            groupRepository.DeleteGroup(group);
            groupRepository.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool GroupExists(int id)
        {
            try
            {
                groupRepository.GetGroupByID(id);
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

            var students = from m in studentRepository.GetStudents()
                           where m.GroupId == id
                           select m;

            return View(students);
        }
    }
}
