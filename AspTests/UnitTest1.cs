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

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestStudentPages()
        {
            Mock<IRepository<Student>> mockStudentRepository = new Mock<IRepository<Student>>();
            var students = GetTestStudents();

            mockStudentRepository.Setup(mr => mr.GetAll()).Returns(students);
            mockStudentRepository.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int i) => students.Where(x => x.StudentId == i).Single());
            this.MockStudentRepository = mockStudentRepository.Object;

            Mock<IRepository<Group>> mockGroupRepository = new Mock<IRepository<Group>>();
            var groups = GetTestGroups();

            mockGroupRepository.Setup(mr => mr.GetAll()).Returns(groups);
            mockGroupRepository.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int i) => groups.Where(x => x.GroupId == i).Single());
            this.MockGroupRepository = mockGroupRepository.Object;

            Mock<IRepository<Course>> mockCourseRepository = new Mock<IRepository<Course>>();
            var courses = GetTestCourses();

            mockCourseRepository.Setup(mr => mr.GetAll()).Returns(courses);
            mockCourseRepository.Setup(mr => mr.GetByID(It.IsAny<int>())).Returns((int i) => courses.Where(x => x.CourseId == i).Single());
            this.MockCourseRepository = mockCourseRepository.Object;

            //Arrange
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
        }

        [Test]
        public void TestCoursePages()
        {
        }

    }
}