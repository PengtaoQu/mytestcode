using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Rotativa.AspNetCore;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MigrationEf
{
    public class Startup
    {

        public static ILoggerRepository repository { get; set; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //初始化log4net
            repository = LogManager.CreateRepository("NETCoreRepository");
            XmlConfigurator.Configure(repository, new FileInfo("log4net.config"));
        }

        public static IConfiguration Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //添加 json 文件路径
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            //创建配置根对象
            var configurationRoot = builder.Build();

            services.AddAutoMapper();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            services.AddAuthentication().AddCookie();
            services.AddSession();

          
            services.AddSwaggerGen(xp =>
            {
                xp.OperationFilter<AddAuthTokenHeaderParameter>();
                xp.SwaggerDoc("V2", new Info { Title = "api  测试", Contact = new Contact { Email = "496887756@qq.com", Name = "allen", Url = "http://" }, Description = "这是整个项目的API描述", License = new License { Url = "HTTP", Name = "ALLEN" }, Version = "V2.2.2" });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "SwaggerDemo.xml");

                if (!File.Exists(xmlPath))
                {
                
                    File.Create(xmlPath).Close();
                    
                }
                xp.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

         
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }
            else
            {
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseSwagger();
            app.UseSwaggerUI(C =>
            {
                C.SwaggerEndpoint("/SWAGGER/V2/SWAGGER.JSON", "MY API  TEST");
                C.RoutePrefix = String.Empty;
            }
             );
            app.UseSession();
            app.UseAuthentication();
   
            app.UseMvc();
        }
    }

    public class AddAuthTokenHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (operation.Parameters == null)
                operation.Parameters = new List<IParameter>();
            var attrs = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            foreach (var attr in attrs)
            {
                // 如果 Attribute 是我们自定义的验证过滤器
                if (attr.GetType() == typeof(Auth))
                {
                    operation.Parameters.Add(new NonBodyParameter()
                    {
                        Name = "AuthToken",
                        In = "header",
                        Type = "string",
                        Required = false
                    });
                }
            }
        }
    }

    public class Auth : ActionFilterAttribute
    {



    }
}
