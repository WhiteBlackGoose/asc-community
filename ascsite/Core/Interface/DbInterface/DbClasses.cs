using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AscSite.Core.Interface.Database
{
    public class DbAscContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Project> Projects { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

    public class Project
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Announcement { get; set; }
        public string Body { get; set; }
    }
}
