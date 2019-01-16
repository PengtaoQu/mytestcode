using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationEf
{
    public class course
    {

         public int ID { get; set; }

        public String Name { get; set; }

    }


    public class student
    {

        public int ID { get; set; }

        public String Name { get; set; }

    }

    public class db : DbContext
    {

        public DbSet<course> courses { get; set; }

        public DbSet<student> students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseSqlServer(@"Server=.;Database=Test2;Trusted_Connection=True;");
        }
    }
    public class db1 : DbContext
    {

        public DbSet<course> courses { get; set; }

        public DbSet<student> students { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseSqlServer(@"Server=.;Database=Test2;Trusted_Connection=True;");
        }
    }

}
