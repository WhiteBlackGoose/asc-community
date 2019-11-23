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
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Post> Posts { get; set; }
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

    public class Relationship
    {
        [Key]
        public int UserId { get; set; }
        public int ProjectId { get; set; }
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
}
