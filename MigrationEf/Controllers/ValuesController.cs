using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static iTextSharp.text.Font;

namespace MigrationEf.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private IHostingEnvironment _host;
        public ValuesController(IHostingEnvironment host)
        {

            _host = host;
        }

        public const string a = "cc";

        /// <summary>
        /// 将文件转换为byte数组
        /// </summary>
        /// <param name="path">文件地址</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] File2Bytes(string path)
        {
            if (!System.IO.File.Exists(path))
            {
                return new byte[0];
            }

            FileInfo fi = new FileInfo(path);
            byte[] buff = new byte[fi.Length];

            FileStream fs = fi.OpenRead();
            fs.Read(buff, 0, Convert.ToInt32(fs.Length));
            fs.Close();

            return buff;
        }


        public class MyNameTransfom : INameTransform
        {

            #region INameTransform 成员

            public string TransformDirectory(string name)
            {
                return null;
            }

            public string TransformFile(string name)
            {
                return Path.GetFileName(name);
            }

            #endregion
        }


        public class GetDataSource : IStaticDataSource
        {

            public GetDataSource(string filepath)
            {
                FilePath = filepath;

            }
            public GetDataSource(Stream  stream)
            {
                FileStream = stream;

            }
            public string FilePath { get; set; }
            public Stream FileStream { get; set; }
            public Stream GetSource()
            {
                if (string.IsNullOrEmpty(FilePath))
                {
                    return FileStream;
                }
                var bys = File2Bytes(FilePath);

                Stream str = new MemoryStream(bys);

                return str;

            }

        }
        // GET api/values
        [HttpGet]
        public ActionResult Getff()
        {

            var filePaths = new List<string>() { _host.WebRootPath + "/cc.jpg", _host.WebRootPath + "/sy.jpg" };

            byte[] buffer;
            MemoryStream ms = new MemoryStream();
            using (
                ZipFile file = ZipFile.Create(ms))
            {
                file.BeginUpdate();
                file.NameTransform = new MyNameTransfom();
                filePaths.ForEach(t =>
                {
                    var item = new GetDataSource(t);
                    file.Add(item, Path.GetFileName(t));

                });

                file.CommitUpdate();
                buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
            }

            return File(buffer, "application/zip", "这是一个打包的文件.zip");

            //var client = new sRestClient("http://localhost:5001/api");

            //var dic = new Dictionary<string, string>();

            //dic.Add("CompanyId", "中国");

            //int status;

            //var cc = client.Get("account/companyrole", dic, out status);

            //var stude = new student { ID = 34, Name = "张三" };

            //var corse = GetTest.ConvertToCoruse(stude);

            ////get.Get(_host);

            //return corse;
        }

        public class PDFFooter : PdfPageEventHelper
        {
            // write on top of document
            public override void OnOpenDocument(PdfWriter writer, Document document)
            {
                base.OnOpenDocument(writer, document);
                PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                tabFot.SpacingAfter = 10F;
                PdfPCell cell;
                tabFot.TotalWidth = 300F;
                cell = new PdfPCell(new Phrase("Header"));
                tabFot.AddCell(cell);
                tabFot.WriteSelectedRows(0, -1, 150, document.Top, writer.DirectContent);
            }

            // write on start of each page
            public override void OnStartPage(PdfWriter writer, Document document)
            {
                base.OnStartPage(writer, document);
            }

            // write on end of each page
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                base.OnEndPage(writer, document);
                //PdfPTable tabFot = new PdfPTable(new float[] { 1F });
                //tabFot.TotalWidth = 700f;
                //tabFot.DefaultCell.Border = 0;
                ////  var footFont = FontFactory.GetFont("Lato", 12 * 0.667f, new Color(60, 60, 60));
                //string fontpath = HttpContext.Current.Server.MapPath("~/App_Data");
                //BaseFont customfont = BaseFont.CreateFont(fontpath + "\\Lato-Regular.ttf", BaseFont.CP1252, BaseFont.EMBEDDED);
                //var footFont = new Font(customfont, 12 * 0.667f, Font.NORMAL, new Color(170, 170, 170));

                //PdfPCell cell;
                //cell = new PdfPCell(new Phrase("@ 2016 . All Rights Reserved", footFont));
                //cell.VerticalAlignment = Element.ALIGN_CENTER;
                //cell.Border = 0;
                //cell.PaddingLeft = 100f;
                //tabFot.AddCell(cell);
                //tabFot.WriteSelectedRows(0, -1, 150, document.Bottom, writer.DirectContent);
            }

            //write on close of document
            public override void OnCloseDocument(PdfWriter writer, Document document)
            {
                base.OnCloseDocument(writer, document);
            }
        }

        // GET api/values/5
        [HttpGet("resthttp")]
        public void Get()
        {

            var  stu= new student() { Name = "李四" };

            var cou = new course() { Name = "语文" };
            var  dd= new RestApi<student>().Post("api/test", cou, null);

            var cc = 1;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public HttpResponseMessage Get(int id)
        {

            var claim = new[] { new Claim("username", "dfhdjfhdjfjd") };
            var claimprincipal = new ClaimsPrincipal(new ClaimsIdentity(claim, CookieAuthenticationDefaults.AuthenticationScheme));

            var filepath = Path.Combine(_host.WebRootPath, "cc.jpg");
            FileStream a = new FileStream(filepath, FileMode.OpenOrCreate);

            HttpContext.Session.Set("fff", Encoding.UTF8.GetBytes("dshfdsjskhfkdj"));
            HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimprincipal, new AuthenticationProperties { ExpiresUtc = DateTime.UtcNow.AddMinutes(60), IsPersistent = false }).Wait();

            var res = new HttpResponseMessage(System.Net.HttpStatusCode.OK) { Content = new StreamContent(a) };
            res.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attach");
            res.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");


            //Response.Redirect("http://localhost:44330/htmlpage.html");


            return res;
        }

        // POST api/values
        [HttpPost]
        [AllowAnonymous]
        public void Post([FromQuery] string value)
        {

            var fff = HttpContext.User.Claims.First().Value;
            var bytes = new byte[10];
            var gg = HttpContext.Session.TryGetValue("fff", out bytes);

            Encoding.UTF8.GetString(bytes);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {

            restSharp.GetRestSharp();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public FileResult Delete(int id)
        {

            //获取中文字体，第三个参数表示为是否潜入字体，但只要是编码字体就都会嵌入。
            BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
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

        [HttpGet("ccc")]
        //导出PDF
        public ActionResult ImportFile()
        {
            try
            {

                string typeData = "日";

                string typeData2 = "月";
                DateTime dateTT;
                //try
                //{
                //    dateTT = Convert.ToDateTime(date);
                //}
                //catch (Exception)
                //{
                //    return Content("<script>alert('日期格式不正确');</script>");
                //}
                //导出日报                   
                //string date1 = dateTT.ToString("yyyyMM") + "01";
                //string date2 = Convert.ToDateTime(date).ToString("yyyyMMdd");
                //list = new IncomingHeatDao().GetReportData(date1, date2);



                //字体读取的是windows系统宋体
                BaseFont baseFont = BaseFont.CreateFont(@"C:\Windows\Fonts\simsun.ttc,0", BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                iTextSharp.text.Font font = new Font(baseFont, 10);//设置字体为宋体和大小
                iTextSharp.text.Font font2 = new Font(baseFont, 9);//设置字体为宋体和大小
                                                                   //font.Color = BaseColor.BLUE;//字体颜色

                iTextSharp.text.Document document = new iTextSharp.text.Document(PageSize.A4.Rotate(), 15, 15, 15, 15);

                System.IO.MemoryStream stream = new System.IO.MemoryStream();
                PdfWriter.GetInstance(document, stream);
                document.Open();

                document.AddTitle("入厂入炉热值差" + typeData + "报表");
                Paragraph element = new Paragraph("入厂入炉热值差" + typeData + "报表", font);
                element.SpacingAfter = 10; //设置离后面内容的间距
                element.Alignment = Element.ALIGN_CENTER;
                document.Add(element);

                //打印时间
                Paragraph element2 = new Paragraph("打印时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm"), font);
                element2.SpacingAfter = 5; //设置离后面内容的间距             
                document.Add(element2);

                PdfPTable table = new PdfPTable(15);
                table.WidthPercentage = 100;//设置表格宽度占用百分比               
                                            //table.SetTotalWidth(new float[] { 90f, 90f, 150f, 90f, 150f, 90f, 150f, 90f, 150f, 90f, 150f, 90f, 150f, 150f, 150f });

                #region 表头
                //第一行表头
                PdfPCell cell = new PdfPCell(new Paragraph("时间", font));
                cell.Rowspan = 3;
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("入厂煤", font));
                cell.Colspan = 4; //定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("入炉煤", font));
                cell.Colspan = 8;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("入厂入炉热值差", font));
                cell.Colspan = 2;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                                                                 //cell.FixedHeight = 130f;//设置高度             
                table.AddCell(cell);

                //第二行
                cell = new PdfPCell(new Paragraph("全厂当" + typeData, font));
                cell.Colspan = 2;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                //cell = new PdfPCell(new Paragraph("全厂" + typeData2 + "累计", font));
                //cell.Colspan = 2;//定义一个表格单元的跨度
                //cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                //cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                //table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("一二期入炉", font));
                cell.Colspan = 2;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("三期入炉", font));
                cell.Colspan = 2;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("全厂当" + typeData, font));
                cell.Colspan = 2;//定义一个表格单元的跨度
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                //cell = new PdfPCell(new Paragraph("全厂" + typeData2 + "累计", font));
                //cell.Colspan = 2;//定义一个表格单元的跨度
                //cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                //cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                //table.AddCell(cell);

                cell = new PdfPCell(new Paragraph("全厂当" + typeData, font));
                cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                table.AddCell(cell);

                //cell = new PdfPCell(new Paragraph("全厂" + typeData2 + "累计", font));
                //cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                //cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                //table.AddCell(cell);

                string[] titleArr = { "吨量(t)", "热值(kj/kg)", "吨量(t)", "热值(kj/kg)", "吨量(t)", "热值(kj/kg)", "吨量(t)", "热值(kj/kg)", "吨量(t)", "热值(kj/kg)", "吨量(t)", "热值(kj/kg)", "热值(kj/kg)", "热值(kj/kg)" };
                foreach (var item in titleArr)
                {
                    cell = new PdfPCell(new Paragraph(item, font));
                    cell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    cell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    table.AddCell(cell);
                }

                #endregion

                var list = new List<pdfData> { new pdfData { date = "1", qcdd_wt = "66" }, new pdfData { date = "2", qcdd_wt = "77" } };
                #region 数据载入
                foreach (var item in list)
                {
                    PdfPCell cell_data = new PdfPCell(new Paragraph(item.date, font));//时间
                    cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    table.AddCell(cell_data);

                    cell_data = new PdfPCell(new Paragraph(item.qcdd_wt.ToString(), font));//全厂当日吨量
                    cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.qcdd_qr.ToString(), font));//全厂当日热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.qcmmsum_wt.ToString(), font));//全厂当月累计吨量
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.qcmmsum_qr.ToString(), font));//全厂当月累计热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.rulu12_wt.ToString(), font));//一二期入炉吨量
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.rulu12_qr.ToString(), font));//一二期入炉热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.rulu3_wt.ToString(), font));//三期入炉吨量
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.rulu3_qr.ToString(), font));//三期入炉热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.ruluqcddsum_wt.ToString(), font));//全厂当日入炉吨量
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.ruluqcddsum_qr.ToString(), font));//全厂当日入炉热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.ruluqcmmsum_wt.ToString(), font));//全厂月累计入炉吨量
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.ruluqcmmsum_qr.ToString(), font));//全厂月累计入炉热值
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.qccha_qr1.ToString(), font));//入厂入炉热值差，全厂当日
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);

                    //cell_data = new PdfPCell(new Paragraph(item.qccha_qr2.ToString(), font));//入厂入炉热值差，全厂月累计
                    //cell_data.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;  //垂直居中
                    //cell_data.HorizontalAlignment = PdfPCell.ALIGN_CENTER;//水平居中
                    //table.AddCell(cell_data);
                }

                #endregion

                document.Add(table);
                document.Close();

                byte[] fileBytes = stream.GetBuffer();
                stream.Close();
                stream.Dispose();
                return File(fileBytes, "application/pdf", DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf");
            }
            catch (Exception ex)
            {
                string msg = string.Empty;
                if (ex.InnerException != null)
                {
                    msg = ex.InnerException.Message;
                }
                else
                {
                    msg = ex.Message;
                }
                return Content("<script>alert(\"异常：" + msg + "\");</script>");
            }
        }

        //public ActionResult pdf()
        //{


        //    try
        //    {
        //        Document document = new Document();
        //        PdfWriter.GetInstance(document, new FileStream(Server.MapPath("Chap0101.pdf"), FileMode.Create));
        //        document.Open();
        //        BaseFont bfChinese = BaseFont.CreateFont("C:WINDOWSFontssimsun.ttc,1", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        //        Font fontChinese = new Font(bfChinese, 12, Font.NORMAL, new Color(0, 0, 0));

        //        document.Add(new Paragraph(this.TextBox1.Text.ToString(), fontChinese));

        //        iTextSharp.text.Image jpeg = iTextSharp.text.Image.getInstance(Server.MapPath("pic015.jpg"));
        //        document.Add(jpeg);
        //        PdfPTable table = new PdfPTable(datatable.Columns.Count);

        //        for (int i = 0; i < datatable.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < datatable.Columns.Count; j++)
        //            {
        //                table.addCell(new Phrase(datatable.Rows[i][j].ToString(), fontChinese));
        //            }
        //        }
        //        document.Add(table);

        //        document.Close();
        //    }
        //    catch (DocumentException de)
        //    {

        //    }



        //}
        public class pdfData
        {

            public string date { get; set; }

            public string qcdd_wt { get; set; }

        }
        [Route("api/pdf/export")]
        public FileResult GetPdf()
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


    }
}
