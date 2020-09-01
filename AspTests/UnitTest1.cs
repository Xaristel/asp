using NUnit.Framework;
using asp;
using asp.Controllers;
using asp.Models;
using System;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AspTests
{
    public class Tests
    {
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
        public void Test1()
        {
            var mock = new Mock<UnitOfWork>();
            mock.Setup(repo => repo.StudentRepository.GetAll()).Returns(GetTestStudents());
            var controller = new StudentsController();

            var result = controller.Index();

            Assert.IsNotNull(result);
        }


    }
}