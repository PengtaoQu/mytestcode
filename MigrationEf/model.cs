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

        public ICollection<student> student { get; set; }

    }


    public class student
    {

        public int ID { get; set; }

        public String Name { get; set; }

        public course course { get; set; }

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


    public class get
    {


        public    static  void Get()
        {


            //var stu = new List<student> { new student { Name="张三"},new student {  Name="李四"},new student {
            // Name="王五"} };
            //var cour = new List<course> { new course { Name = "语文", student = stu }, new course { Name = "英语" } };

            var dbc = new db();

            //dbc.courses.AddRange(cour);

            //var shuw = new course { ID = 10 };

            ////var stu = dbc.students.Where(x => x.Name == "张三").FirstOrDefault();
            ////dbc.students.Remove(stu);
            //dbc.courses.Remove(shuw);


            var stu = new student { ID = 5};
            dbc.students.Remove(stu);

            dbc.SaveChanges();




        }

    
    
    }

}
