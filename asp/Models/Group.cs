using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace asp.Models
{
    public class Group
    {
        public int GroupId { get; set; }

        [Display(Name = "Course Id")]
        public int CourseId { get; set; }

        [Display(Name = "Group Name")]
        public string GroupName { get; set; }
    }
}
