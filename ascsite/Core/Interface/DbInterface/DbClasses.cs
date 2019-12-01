using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Core.Interface.Database
{
    public class DbAscContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserPostContribution> UserPostContributions { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<CodeLink> CodeLinks { get; set; }
        /*
        private List<string> logLines;
        private void LogWrite(string s)
        {
            logLines.Add(s);
        }
        public string GetLog()
        {
            return string.Join("\n", logLines.ToArray());
        }
        */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //Database.Log = LogWrite;
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=asc;Integrated Security=True");
        }
    }

    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class UserPostContribution
    {
        public enum TYPE
        {
            AUTHOR,
            INVESTIGATOR,
            SOLVER,
            CONTRIBUTOR,
        }
        public TYPE GetRelationType() => (TYPE)Type;
        public bool IsAuthor() => GetRelationType() == TYPE.AUTHOR;
        public bool IsInvestigator() => GetRelationType() == TYPE.INVESTIGATOR;
        public bool IsSolver() => GetRelationType() == TYPE.SOLVER;
        public bool IsContributor() => GetRelationType() == TYPE.CONTRIBUTOR;

        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public int Type { get; set; }
    }

    public class Post
    {
        public enum TYPE
        {
            PROJECT,
            PROBLEM
        }

        public TYPE GetPostType() => (TYPE)Type;
        public bool IsProject() => GetPostType() == TYPE.PROJECT;
        public bool IsProblem() => GetPostType() == TYPE.PROBLEM;

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Announcement { get; set; }
        public string Body { get; set; }
        public int Type { get; set; }
        public void Assign(Post src)
        {
            Id = src.Id;
            Name = src.Name;
            Announcement = src.Announcement;
            Body = src.Body;
            Type = src.Type;
        }
    }

    public class CodeLink
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
    }
}
