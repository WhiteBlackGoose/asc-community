using ascsite.Core;
using AscSite.Core.Interface.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Core.Interface.DbInterface
{
    public static class DbInterface
    {
        public static Project GetProjectById(int id)
        {
            using (var db = new DbAscContext())
            {
                var res = db.Projects.FirstOrDefault(p => p.Id == id);
                if (res == null)
                    throw new InvalidRequestException();
                return res;
            }
        }
        public static List<Project> GetProjects(int offset = 0, int limit = -1)
        {
            using(var db = new DbAscContext())
            {
                if(limit == -1)
                    return db.Projects.Skip(offset).ToList();
                else
                    return db.Projects.Skip(offset).Take(limit).ToList();
            }
        }
    }
}
