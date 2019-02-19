using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MigrationEf.Controllers
{

    [Route("api/[controller]")]
    public class PDFController : Controller
    {
        // GET: PDF
        public ActionResult Index()
        {
           

             
                //获取中文字体，第三个参数表示为是否潜入字体，但只要是编码字体就都会嵌入。
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                //读取模板文件
                PdfReader reader = new PdfReader(@"E:\CodeFirst\MigrationEf\MigrationEf\wwwroot\年龄.pdf");

                //创建文件流用来保存填充模板后的文件
                System.IO.MemoryStream stream = new System.IO.MemoryStream();

                PdfStamper stamp = new PdfStamper(reader, stream);
                //设置表单字体，在高版本有用，高版本加入这句话就不会插入字体，低版本无用
                //stamp.AcroFields.AddSubstitutionFont(baseFont);

                AcroFields form = stamp.AcroFields;

                //表单文本框是否锁定
                stamp.FormFlattening = true;

                Dictionary<string, string> para = new Dictionary<string, string>();
                para.Add("1", "国科");
                para.Add("2", "133333333");
                para.Add("3", "电脑开不机,可能是电源问题,维修前报价(主机缺侧盖,有重要资料,不能重装)");
                para.Add("4", "2017年12月11日 12:24");
                para.Add("5", "某某某");
                para.Add("6", "某某某");
           

                //填充表单,para为表单的一个（属性-值）字典
                foreach (KeyValuePair<string, string> parameter in para)
                {
                    //要输入中文就要设置域的字体;
                    form.SetFieldProperty(parameter.Key, "textfont", baseFont, null);
                    //为需要赋值的域设置值;
                    form.SetField(parameter.Key, parameter.Value);
                }

                //按顺序关闭io流

                stamp.Close();
                reader.Close();
                //生成文件
                FileResult fileResult = new FileContentResult(stream.ToArray(), "application/pdf");
                //fileResult.FileDownloadName = "4.pdf";
                return fileResult;
     
        }

        // GET: PDF/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PDF/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PDF/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PDF/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PDF/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PDF/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PDF/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}