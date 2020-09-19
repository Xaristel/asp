using NUnit.Framework;
using asp;
using asp.Controllers;
using asp.Models;
using System;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq;

namespace AspTests
{
    public class Tests
    {
        public IRepository<Student> MockStudentRepository;
        public IRepository<Group> MockGroupRepository;
        public IRepository<Course> MockCourseRepository;

        private List<Student> GetTestStudents()
        {
            var students = new List<Student>
            {
                new Student { FirstName="Mary", GroupId=1, LastName="Victorov", StudentId=1 },
                new Student { FirstName="John", GroupId=2, LastName="Lencov", StudentId=2},
                new Student { FirstName="Liza", GroupId=3, LastName="Vlasov", StudentId=3},
                new Student { FirstName="Anton", GroupId=4, LastName="Digger", StudentId=4}
            };
            return students;
        }

        private List<Group> GetTestGroups()
        {
            var groups = new List<Group>
            {
                new Group { CourseId=1, GroupId=1, GroupName="A" },
                new Group { CourseId=2, GroupId=2, GroupName="B" },
                new Group { CourseId=3, GroupId=3, GroupName="C" },
                new Group { CourseId=4, GroupId=4, GroupName="D" }
            };
            return groups;
        }

        private List<Course> GetTestCourses()
        {
            var courses = new List<Course>
            {
                new Course { CourseDescription="Aa", CourseId=1, CourseName="A" },
                new Course { CourseDescription="Bb", CourseId=2, CourseName="B"},
                new Course { CourseDescription="Cc", CourseId=3, CourseName="C"},
                new Course { CourseDescription="Dd", CourseId=4, CourseName="D"}
            };
            return courses;
        }

        private void SetMockRepository()
        {
            Mock<IRepository<Student>> mockStudentRepository = new Mock<IRepository<Student>>();
            List<Student> students = GetTestStudents();

            mockStudentRepository.Setup(mr => mr.GetAll()).Returns(students);
            mockStudentRepository.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int i) => students.Where(x => x.StudentId == i).Single());
            this.MockStudentRepository = mockStudentRepository.Object;
            ////
            Mock<IRepository<Group>> mockGroupRepository = new Mock<IRepository<Group>>();
            List<Group> groups = GetTestGroups();

            mockGroupRepository.Setup(gr => gr.GetAll()).Returns(groups);
            mockGroupRepository.Setup(gr => gr.GetByID(It.IsAny<int>())).Returns((int i) => groups.Where(x => x.GroupId == i).Single());
            this.MockGroupRepository = mockGroupRepository.Object;
            ////
            Mock<IRepository<Course>> mockCourseRepository = new Mock<IRepository<Course>>();
            List<Course> courses = GetTestCourses();

            mockCourseRepository.Setup(qr => qr.GetAll()).Returns(courses);
            mockCourseRepository.Setup(qr => qr.GetByID(It.IsAny<int>())).Returns((int i) => courses.Where(x => x.CourseId == i).Single());
            this.MockCourseRepository = mockCourseRepository.Object;
        }

        [Test]
        public void TestStudentPages()
        {
            //Arrange
            SetMockRepository();
            UnitOfWork unitOfWork = new UnitOfWork(MockStudentRepository, MockGroupRepository, MockCourseRepository);

            StudentsController controller = new StudentsController(unitOfWork);
            //Act
            var resultIndex = controller.Index();
            var resultDetails = controller.Details(1);
            var resultEdit = controller.Edit(1);
            var resultDelete = controller.Delete(1);
            var resultStudentExit = controller.StudentExists(2);
            var resultDeleteConfirmed = controller.DeleteConfirmed(1);

            //Assert
            try
            {
                var viewResultIndex = resultIndex as ViewResult;
                var modelResultIndex = viewResultIndex.ViewData.Model;

                var viewResultDetails = resultDetails as ViewResult;
                var modelResultDetails = viewResultDetails.ViewData.Model;

                var viewResultEdit = resultEdit as ViewResult;
                var modelResultEdit = viewResultEdit.ViewData.Model;

                var viewResultDelete = resultDelete as ViewResult;
                var modelResultDelete = viewResultDelete.ViewData.Model;

                var redirectDeleteConfirmed = resultDeleteConfirmed as RedirectToActionResult;

                Assert.IsAssignableFrom<Student>(modelResultIndex);
                Assert.IsAssignableFrom<Student>(modelResultDetails);
                Assert.IsAssignableFrom<Student>(modelResultEdit);
                Assert.IsAssignableFrom<Student>(modelResultDelete);
                Assert.AreEqual("Index", viewResultIndex.ViewName);
                Assert.AreEqual("Details", viewResultDetails.ViewName);
                Assert.AreEqual("Edit", viewResultEdit.ViewName);
                Assert.AreEqual("Delete", viewResultDelete.ViewName);
                Assert.IsTrue(resultStudentExit);
                Assert.AreEqual("Index", redirectDeleteConfirmed.ActionName);
                Assert.IsFalse(redirectDeleteConfirmed.Permanent);
            }
            catch
            {
                Assert.Fail("Fail tests");
            }
        }

        [Test]
        public void TestGroupPages()
        {
            //Arrange
            SetMockRepository();
            UnitOfWork unitOfWork = new UnitOfWork(MockStudentRepository, MockGroupRepository, MockCourseRepository);

            GroupsController controller = new GroupsController(unitOfWork);
            //Act
            var resultIndex = controller.Index();
            var resultDetails = controller.Details(1);
            var resultEdit = controller.Edit(1);
            var resultDelete = controller.Delete(1);
            var resultGroupExist = controller.GroupExists(1);
            var resultDeleteConfirmed = controller.DeleteConfirmed(1);
            //Assert
            try
            {
                var viewResultIndex = resultIndex as ViewResult;
                var modelResultIndex = viewResultIndex.ViewData.Model;

                var viewResultDetails = resultDetails as ViewResult;
                var modelResultDetails = viewResultDetails.ViewData.Model;

                var viewResultEdit = resultEdit as ViewResult;
                var modelResultEdit = viewResultEdit.ViewData.Model;

                var viewResultDelete = resultDelete as ContentResult;
                var modelResultDelete = viewResultDelete.Content;

                var redirectDeleteConfirmed = resultDeleteConfirmed as RedirectToActionResult;

                Assert.IsAssignableFrom<Group>(modelResultIndex);
                Assert.IsAssignableFrom<Group>(modelResultDetails);
                Assert.IsAssignableFrom<Group>(modelResultEdit);
                Assert.IsAssignableFrom<String>(modelResultDelete);
                Assert.AreEqual("Index", viewResultIndex.ViewName);
                Assert.AreEqual("Details", viewResultDetails.ViewName);
                Assert.AreEqual("Edit", viewResultEdit.ViewName);
                Assert.AreEqual("Error. This group has students!", viewResultDelete.Content);
                Assert.IsTrue(resultGroupExist);
                Assert.AreEqual("Index", redirectDeleteConfirmed.ActionName);
                Assert.IsFalse(redirectDeleteConfirmed.Permanent);

                var resultShow = controller.Show(1);
                var viewResultShow = resultShow as ViewResult;
                var modelResultShow = viewResultShow.ViewData.Model;
                Assert.IsAssignableFrom<List<Student>>(modelResultShow);
                Assert.AreEqual("Show", viewResultShow.ViewName);
            }
            catch
            {
                Assert.Fail("Fail tests");
            }
        }

        [Test]
        public void TestCoursePages()
        {
            //Arrange
            SetMockRepository();
            UnitOfWork unitOfWork = new UnitOfWork(MockStudentRepository, MockGroupRepository, MockCourseRepository);

            CoursesController controller = new CoursesController(unitOfWork);
            //Act
            var resultIndex = controller.Index();
            var resultDetails = controller.Details(1);
            var resultEdit = controller.Edit(1);
            var resultDelete = controller.Delete(1);
            var resultCourseExist = controller.CourseExists(1);
            var resultDeleteConfirmed = controller.DeleteConfirmed(1);

            //Assert
            try
            {
                var viewResultIndex = resultIndex as ViewResult;
                var modelResultIndex = viewResultIndex.ViewData.Model;

                var viewResultDetails = resultDetails as ViewResult;
                var modelResultDetails = viewResultDetails.ViewData.Model;

                var viewResultEdit = resultEdit as ViewResult;
                var modelResultEdit = viewResultEdit.ViewData.Model;

                var viewResultDelete = resultDelete as ContentResult;
                var modelResultDelete = viewResultDelete.Content;

                var redirectDeleteConfirmed = resultDeleteConfirmed as RedirectToActionResult;

                Assert.IsAssignableFrom<Course>(modelResultIndex);
                Assert.IsAssignableFrom<Course>(modelResultDetails);
                Assert.IsAssignableFrom<Course>(modelResultEdit);
                Assert.IsAssignableFrom<String>(modelResultDelete);
                Assert.AreEqual("Index", viewResultIndex.ViewName);
                Assert.AreEqual("Details", viewResultDetails.ViewName);
                Assert.AreEqual("Edit", viewResultEdit.ViewName);
                Assert.AreEqual("Error. This course has groups!", viewResultDelete.Content);
                Assert.IsTrue(resultCourseExist);
                Assert.AreEqual("Index", redirectDeleteConfirmed.ActionName);
                Assert.IsFalse(redirectDeleteConfirmed.Permanent);

                var resultShow = controller.Show(1);

                var viewResultShow = resultShow as ViewResult;
                var modelResultShow = viewResultShow.ViewData.Model;

                Assert.IsAssignableFrom<List<Group>>(modelResultShow);
                Assert.AreEqual("Show", viewResultShow.ViewName);
            }
            catch
            {
                Assert.Fail("Fail tests");
            }
        }

    }
}