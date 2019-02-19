using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MigrationEf.Controllers
{

    [Route("api/[controller]")]
    public class TestController : Controller
    {
        /// <summary>
        /// 这是测试使用的
        /// </summary>
        /// <param name="stu"></param>
        /// <returns></returns>
        [HttpGet]
        [Auth]
        public IActionResult GetbyID([FromQuery] student  stu)
        {
            stu.Name = "这是测试名字";

            return Json(stu);
        }

        [HttpPost]
        [Auth]
        public IActionResult PostById([FromBody]  course cr)
        {

            cr.Name = "这是post";
            return Json(cr);

        }

        [HttpPut]
        public IActionResult PutById([FromBody]  course cr)
        {

            cr.Name = "这是Put";
            return Json(cr);

        }

        [HttpDelete]
        public IActionResult DeleteById([FromBody] course cr)
        {

            cr.Name = "这是Delete";
            return Json(cr);

        }
    }
}