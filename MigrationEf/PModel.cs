using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MigrationEf
{
    public class ResultModel
    {
        public string code { get; set; }  
        public string msg { get; set; }
        public object data { get; set; }
    }

    public class candidate
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string gender { get; set; }
        /// <summary>
        /// 毕业时间
        /// </summary>
        public string graduationDate { get; set; }	

        /// <summary>
        /// 最高学历
        /// </summary>
        public string  highestDegree {get;set;}

        /// <summary>
        /// 邮箱
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 身份证
        /// </summary>
        public string idNumber { get; set; }

        /// <summary>
        /// 最新一次工作时开始时间
        /// </summary>
        public string lastWorkStartDate { get; set; }

        /// <summary>
        /// 最近一次工作结束时间
        /// </summary>
        public string lastWorkEndDate { get; set; }

        /// <summary>
        /// 最近一份工作公司名称
        /// </summary>
        public string lastWorkCompany { get; set; }

        /// <summary>
        /// 具体工作的内容
        /// </summary>
        public string specificWorkingContent { get; set; }

        /// <summary>
        /// 其他工作经历
        /// </summary>
        public string othersJobExperience { get; set; }

        /// <summary>
        /// 工作内容
        /// </summary>
        public string workingContent { get; set; }

        /// <summary>
        /// 工作开始时间
        /// </summary>
        public string workingStartDate { get; set; }

        /// <summary>
        /// 工作结束时间
        /// </summary>
        public string workingEndDate { get; set; }

        /// <summary>
        /// 毕业院校
        /// </summary>
        public string graduationSchool { get; set; }

        /// <summary>
        /// 最高学历开始时间
        /// </summary>
        public string highestDegreeStartDate { get; set; }

        /// <summary>
        /// 最高学历结束时间
        /// </summary>
        public string highestDegreeGraduationDate { get; set; }

        /// <summary>
        /// 所学专业
        /// </summary>
        public string major { get; set; }

        /// <summary>
        /// 是否是华晨宝马员工
        /// </summary>
        public string hasBBAWorkingExperience { get; set; }

        /// <summary>
        /// 身高
        /// </summary>
        public string height { get; set; }

        /// <summary>
        /// 自我评价
        /// </summary>
        public string selfEvaluation { get; set; }

        /// <summary>
        /// 现居住地
        /// </summary>
        public string residence { get; set; }

        /// <summary>
        /// 姓名拼音
        /// </summary>
        public string namePinYin { get; set; }

        /// <summary>
        /// 招聘渠道
        /// </summary>
        public string recruitingChannel { get; set; }

        /// <summary>
        /// 工装号码
        /// </summary>
        public string uniformSize { get; set; }
        /// <summary>
        /// 鞋子号码
        /// </summary>
        public string shoesSize { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }

        /// <summary>
        /// 申请列表
        /// </summary>
         public List<Application> applications { get; set; }
    }

    public class Application
    {
        /// <summary>
        /// 申请人编号
        /// </summary>
       public string applicantId { get; set; }
        /// <summary>
        /// 报名时间
        /// </summary>
        public string applicationDate { get; set; }
        /// <summary>
        ///当前阶段   Pre-selection简历筛选   Online Test 在线测试   Interview 面试   HPE Check 面试   Assign Position岗位匹配  Onboarding 岗位匹配
        /// </summary>
        public string currentStage { get; set; }
        /// <summary>
        /// 状态  (前五个模块为Status，Onboarding模块为Onboarding Status)	status：  Pending 未决定   Disqualify 取消  Blank无效	  Onboarding Status：Confirmed  确定  Postponed  延期  Dropout 中途退出 
        ///        TBD 待定    Blank无效  			
        /// </summary>
        public string status { get; set; }
        /// <summary>
        /// 应聘职位
        /// </summary>
        public string appliedPosition { get; set; }
        /// <summary>
        /// 厂区选择
        /// </summary>
        public string plantSelection { get; set; }
        /// <summary>
        /// 面试地方
        /// </summary>
        public string interviewPosition { get; set; }
        /// <summary>
        /// 分配位置en
        /// </summary>
        public string assignedPositionTitleEn { get; set; }
        /// <summary>
        /// 被分配的位置cn
        /// </summary>
        public string assignedPositionTitleCn { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string jcNumber { get; set; }
        public string assignedSubDepCode { get; set; }
        /// <summary>
        /// 工作时间
        /// </summary>
        public string functionStartDate { get; set; }
        public string jcType { get; set; }
        public string rarType { get; set; }

    }


    public class Transfrom
    {

        public static  string ConvertEnCurrentStage(string CurrentStage)
        {
            switch (CurrentStage)
            {
                case "Pre-selection":
                    return "简历筛选";
                  
                case "Online Test":
                    return "在线测试";
                
                case "Interview":
                    return "面试";
                 
                case "HPE Check":
                    return "面试";
               
                case "Assign Position":
                    return "岗位匹配";
                 
                case "Onboarding":
                    return "岗位匹配";
                default:
                    return "未知";
            }

        }

        public static string ConvertStatus(string Status, LanguageEnum language)
        {
            switch (Status)
            {
                case "Pending":
                    if (language == LanguageEnum.CH)
                        return "进行中";
                    return "In Process";

                case "Disqualify":
                    if (language == LanguageEnum.CH)
                        return "未通过";
                    return "Fail";

                case "Blank":
                    if (language == LanguageEnum.CH)
                        return "进行中";
                    return "In Process";

                case "Confirmed ":
                    if (language == LanguageEnum.CH)
                        return "通过";
                    return "Success";

                case "Postponed":
                    if (language == LanguageEnum.CH)
                        return "进行中";
                    return "In Process";

                case "Dropout":
                    if (language == LanguageEnum.CH)
                        return "未通过";
                    return "Fail";
                case "TBD":
                    if (language == LanguageEnum.CH)
                        return "进行中";
                    return "In Process";
                default:
                    return "未知";
            }
        }

    }
    public enum LanguageEnum
    {
        EN,
        CH

    }

}
