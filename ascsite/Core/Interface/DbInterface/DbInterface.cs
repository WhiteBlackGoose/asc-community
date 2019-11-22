using ascsite.Core;
using AscSite.Core.Interface.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
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
                var prjs = db.Projects.Skip(offset).Where(p => p.Name != null);
                if (limit != -1)
                    prjs = prjs.Take(limit);
                return prjs.ToList();
            }
        }

        //TODO
        public static void AddOrUpdateProject(Project project)
        { 
            using(var db = new DbAscContext())
            {
                var found = db.Projects.FirstOrDefault(p => p.Id == project.Id);
                if (found != null)
                {
                    found.Assign(project);
                    db.Projects.Update(found);
                }
                else
                {
                    db.Projects.Add(project);
                }
                db.SaveChanges();
            }
        }

        public static void RemoveProjectById(int id)
        {
            using (var db = new DbAscContext())
            {
                db.Projects.Remove(db.Projects.FirstOrDefault(p => p.Id == id));
                db.SaveChanges();
            }
        }
    }
}
