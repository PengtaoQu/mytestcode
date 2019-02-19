using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace MigrationEf
{
    public class course
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public String Name { get; set; }
       
        public  virtual ICollection<student> student { get; set; }

    }


    public class student
    {
        /// <summary>
        /// id
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public String Name { get; set; }

        public virtual course course { get; set; }

    }


    //public class activity
    //{
    //      public int activityID { get; set; }

    //    public string Name { get; set; }
    //    public ICollection<trip> trips { get; set; }

        
    //}

    //public class tripacti
    //{

    //    public int id { get; set; }

    //    public int activityID { get; set; }

    //    public activity activity { get; set; }

    //    public trip trip { get; set; }
    //    public int tripID { get; set; }

    //}

    //public class trip
    //{
    //    public int tripID { get; set; }

    //    public string nAME { get; set; }

    //    public ICollection<activity> acti { get; set; }
    //}

    public class db : DbContext
    {

        public DbSet<course> courses { get; set; }

        public DbSet<student> students { get; set; }

        //public DbSet<trip> trips { get; set; }

        //public DbSet<activity> activities { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //注入Sql链接字符串
            optionsBuilder.UseSqlServer(@"Server=.;Database=Test4;Trusted_Connection=True;");
        }

     
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //modelBuilder.Entity<tripacti>().HasKey(x => new { x.tripID, x.activityID });
            //modelBuilder.Entity<tripacti>().HasOne(x => x.trip).WithMany(p => p.tripacti).HasForeignKey(x => x.tripID);
            //modelBuilder.Entity<tripacti>().HasOne(x => x.activity).WithMany(p => p.tripacti).HasForeignKey(x => x.activityID).OnDelete(DeleteBehavior.ClientSetNull);

        }


    }

    public class GetTest
    {

        /// <summary>
        /// 自定义Mapper
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        public  static course ConvertToCoruse(student stu)
        {
            var config2 = new MapperConfiguration(cfg => cfg.CreateMap<student, course>()
             .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name + s.ID))  //指定字段一一对应
             .ForMember(d => d.ID, opt => opt.MapFrom(src => src.ID + src.ID)));//指定字段，并转化指定的格

            var mapper2 = config2.CreateMapper();

            var corse = mapper2.Map<course>(stu);
            return corse;
        }
        public    static  void GetAll(IHostingEnvironment _host)
        {

            //var stu = new List<student> { new student { Name="张三"},new student {  Name="李四"},new student {
            // Name="王五"} };
            //var cour = new List<course> { new course { Name = "语文", student = stu }, new course { Name = "英语" } };

            var dbc = new db();

            var c = dbc.courses.FirstOrDefault();
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            var  dddd=  JsonConvert.SerializeObject(c,settings);

             //dbc.courses.AddRange(cour);

             //var shuw = new course { ID = 10 };

             ////var stu = dbc.students.Where(x => x.Name == "张三").FirstOrDefault();
            ////dbc.students.Remove(stu);
            //dbc.courses.Remove(shuw);

           //var  CC= new get().RequestWechatInterface("http://localhost:44330/api/values/5",  _host, null);

            var stu = new student { ID = 5};
            dbc.students.Remove(stu);

            dbc.SaveChanges();

        }
        private  string RequestWechatInterface(string url, IHostingEnvironment _host,object postDataModel)
        {
            string result = "";

            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);

            req.Method = "GET";

            req.Timeout = 10000000;

            req.ContentType = "application/json";

            //var postData = JsonConvert.SerializeObject(postDataModel);
            //byte[] data = Encoding.UTF8.GetBytes(postData);

            //req.ContentLength = data.Length;

            //using (Stream reqStream = req.GetRequestStream())
            //{
            //    reqStream.Write(data, 0, data.Length);

            //    reqStream.Close();
            //}

            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();

            Stream stream = resp.GetResponseStream();

           var  ff=  _host.WebRootPath;

            var filepath =Path.Combine(_host.WebRootPath ,"cc.jpg");

            if (!File.Exists(filepath))
            {

                File.Create(filepath);

            }

           
            FileStream st = new FileStream(filepath,FileMode.OpenOrCreate);

            stream.CopyTo(st);

            //获取响应内容
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                result = reader.ReadToEnd();
            }
            return result;
            //return result;
        }
    }

}
