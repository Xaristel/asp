using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    interface IGroupRepository : IDisposable
    {
        IEnumerable<Group> GetGroups();
        Group GetGroupByID(int groupId);
        void AddGroup(Group group);
        void DeleteGroup(Group group);
        void UpdateGroup(Group group);
        void Save();
    }
}
