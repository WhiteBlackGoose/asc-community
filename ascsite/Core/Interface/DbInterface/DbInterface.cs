using ascsite.Core;
using AscSite.Core.Interface.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AscSite.Core.Interface.DbInterface
{
    using RelationType = AscSite.Core.Interface.Database.UserPostContribution.TYPE;
    using PostType = AscSite.Core.Interface.Database.Post.TYPE;
    public static class DbInterface
    {
        public class PostUserEntry
        {
            public RelationType postRelation;
            public User userData;
        }

        private static IQueryable<Post> GetPostsByType(DbSet<Post> db, PostType type)
        {
            var intType = (int)type;
            return db.Where(p => p.Type == intType);
        }

        public static List<PostUserEntry> GetUsersRelatedToPostById(int postId)
        {
            // we need more generic Linq expression returning UserEntry here because 
            // db cannot perform `where` search on non primary key fields
            using(var db = new DbAscContext())
            {
                var entries = from contr in db.UserPostContributions
                              join user in db.Users on contr.UserId equals user.Id
                              where contr.PostId == postId
                              select new PostUserEntry
                              { 
                                  postRelation = contr.GetRelationType(), 
                                  userData = user 
                              };

                return entries.ToList();
            }
        }

        public static int Count()
        {
            using (var db = new DbAscContext())
            {
                return db.Posts.Count();
            }
        }
        public static Post GetPostById(int id)
        {
            using (var db = new DbAscContext())
            {
                var res = db.Posts.FirstOrDefault(p => p.Id == id);
                if (res == null)
                    throw new InvalidRequestException();
                return res;
            }
        }
        public static List<Post> GetPosts(PostType type, int offset = 0, int limit = -1)
        {
            using(var db = new DbAscContext())
            {
                var prjs = GetPostsByType(db.Posts, type).Where(p => p.Name != null).Skip(offset);
                if (limit != -1)
                    prjs = prjs.Take(limit);
                return prjs.ToList();
            }
        }

        //TODO REMOVING

        public static void AddPost(Post project)
        {
            using (var db = new DbAscContext())
            {
                db.Posts.Add(project);
                db.SaveChanges();
            }
        }

        public static void AddOrUpdatePost(Post project)
        { 
            using(var db = new DbAscContext())
            {
                var found = db.Posts.FirstOrDefault(p => p.Id == project.Id);
                if (found != null)
                {
                    found.Assign(project);
                    db.Posts.Update(found);
                }
                else
                {
                    db.Posts.Add(project);
                }
                db.SaveChanges();
            }
        }

        public static void DeletePostRelations(int postId)
        {
            using (var db = new DbAscContext())
            {
                foreach(var entry in db.UserPostContributions.Where(e => e.PostId == postId))
                {
                    db.Remove(entry);
                }

                db.SaveChanges();
            }
        }
        public static void AddRelations(int postId, List<PostUserEntry> relations)
        {
            using(var db = new DbAscContext())
            {
                foreach(var entry in relations)
                {
                    if (entry.userData == null) 
                        throw new NullReferenceException("UserData must not be null");

                    db.UserPostContributions.Add(new UserPostContribution
                    {
                        PostId = postId,
                        UserId = entry.userData.Id,
                        Type = (int)entry.postRelation
                    });
                }
                
                db.SaveChanges();
            }
        }

        public static void RemovePostById(int id)
        {
            using (var db = new DbAscContext())
            {
                db.Posts.Remove(db.Posts.FirstOrDefault(p => p.Id == id));
                db.SaveChanges();
            }
        }
    }
}
