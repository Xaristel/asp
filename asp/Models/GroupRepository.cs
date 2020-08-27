using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace asp.Models
{
    public class GroupRepository : IGroupRepository, IDisposable
    {
        private DataContext context;

        public GroupRepository(DataContext context)
        {
            this.context = context;
        }

        public IEnumerable<Group> GetGroups()
        {
            return context.Group.ToList();
        }

        public Group GetGroupByID(int id)
        {
            return context.Group.Find(id);
        }

        public void AddGroup(Group group)
        {
            context.Group.Add(group);
        }

        public void DeleteGroup(Group group)
        {
            Group _group = context.Group.Find(group.GroupId);
            context.Group.Remove(_group);
        }

        public void UpdateGroup(Group group)
        {
            context.Entry(group).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
