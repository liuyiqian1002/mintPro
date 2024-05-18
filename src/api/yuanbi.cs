using LeaRun.Application.Busines.ZhongLiManage;
using LeaRun.Application.Entity.ZhongLiManage;
using LeaRun.Util;
using LeaRun.Util.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Text;
using System.Data;
using LeaRun.Util.WebControl;
using LeaRun.Util.Extension;
using System.IO;
using System.Data.Common;
using LeaRun.Data;

namespace WebApplication1
{
    //私钥：8GfE5qsL
    //请求接口：https://webapi.eyuanbi.com/Index.asmx/接口名称
    //接口域名：https://webapi.eyuanbi.com

    //yuanbi2@yuanbi.com
    //qwer123456
    //362514
contract_trad?SymbolsId=5203364881b6440ea60f287a7be36268
  //  h5链接：
    //注册协议
    //https://124.221.191.238:9090/News/AppSynopsis/8f14ea47-5fce-4741-aee8-4d85eeb7a69c?LanguageVersion={语言版本标识zh-CN/zh-hk/en/jpn}
    //资讯详情
    //https://124.221.191.238:9090/News/Detail/{资讯主键}
    //帮助中心
    //https://124.221.191.238:9090/News/AppReplicate/c8ebc9cc-8d27-46a5-a57e-c54d1287c4d4?LanguageVersion={语言版本标识zh-CN/zh-hk/en/jpn}
    //下载链接
    //https://124.221.191.238:9090/WebUser/Download/{语言版本标识zh-CN/zh-hk/en/jpn}
    //客服链接：
    //https://124.221.191.238:9090/News/CustomerService/{语言版本标识zh-CN/zh-hk/en/jpn}
    //合约协议
    //https://124.221.191.238:9090/News/AppSynopsis/16515ae1-eb5b-4aa3-99cc-fc1ace43ede4?LanguageVersion={语言版本标识zh-CN/zh-hk/en/jpn}
    /// <summary>
    /// Index 的摘要说明
    /// </summary>
    [WebService(Namespace = "https://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class Index : System.Web.Services.WebService
    {
        private ZhongLiExpandBLL bll = new ZhongLiExpandBLL();

        #region 公共方法
        /// <summary>
        /// 公共加密输出方法
        /// </summary>
        /// <param name="dto"></param>
        public void ResponseMethod(ResultDto dto)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
            Context.Response.End();
        }

        /// <summary>
        /// 公共不加密输出方法
        /// </summary>
        /// <param name="dto"></param>
        public void ResponseNoEncryptMethod(ResultDto dto)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            Context.Response.Write(Json.ToJson(dto));
            Context.Response.End();
        }
        /// <summary>
        /// 日志记录[系统日志]
        /// </summary>
        /// <param name="txt"></param>
        public void Write(string txt)
        {
            try
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory + "\\Log\\";
                if(!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string _FilePath = path + $"{DateTime.Now.ToString("yyyy-MM-dd")}.log";
                FileStream fs = new FileStream(_FilePath, FileMode.Append);
                //获得字节数组
                byte[] data = System.Text.Encoding.Default.GetBytes(DateTime.Now + $": [IP:{Net.Ip}]" + txt + "\r\n");
                //开始写入
                fs.Write(data, 0, data.Length);
                //清空缓冲区、关闭流
                fs.Flush();
                fs.Close();
            }
            catch { }
        }

        #endregion

        //==========会员中心（1-20）================//

        #region [1]用户 => 用户注册
        //[WebMethod(Description = @"[1]用户 => 用户注册(输入参数string InJson)</br>
        //RegisterMark:注册方式(Mobile、Email、QQ、WeChat、Alipay)</br>
        //RegisterType:注册类型(Web、Android、Ios、Pc)</br>
        //Account：登录账户(说明：根据注册方式取对应值)</br>
        //Password:登录密码(说明：快捷注册为default)</br>
        //OpenId:第三方快捷返回值说明：不是快捷注册传default)</br>
        //RecommendEnCode:邀请码（选填）</br>
        //Code:验证码<br/>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void Registered(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "RegisterMark", "RegisterType", "Account", "Password", "OpenId", "RecommendEnCode" , "Code", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    list.Add("RegisterIp", HttpContext.Current.Request.UserHostAddress);
                    dto = bll.Registered(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【Registered】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【Registered】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region [2]用户 => 用户登录
        //[WebMethod(Description = @" [2]用户 => 用户登录(输入参数string InJson)</br>
        //LoginMark: 登录方式(Mobile、Email、QQ、WeChat、Alipay) </br>
        //Account：登录账户(说明：根据登录方式取对应值) </br>
        //Password:登录密码(说明：快捷注册传default)</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void Login(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "LoginMark", "Account", "Password", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.Login(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【Login】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【Login】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }


        #endregion

        #region [3]用户 => 修改登录密码
        //[WebMethod(Description = @"[3]用户 => 修改登录密码(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //OldPassWord:旧密码</br>
        //NewPassWord:新密码</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void UpdatePassword(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "OldPassWord", "NewPassWord", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.UpdatePassword(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UpdatePassword】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【UpdatePassword】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region [4]用户 => 重置登录密码(密码找回)
        //[WebMethod(Description = @"[4]用户 => 重置登录密码(输入参数string InJson)</br>
        //Account:登录账户(邮箱)</br>
        //NewPassword:新密码</br>
        //Code:验证码<br/>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void ResetPassword(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "Account", "NewPassword","Code", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.ResetPassword(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【ResetPassword】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【ResetPassword】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }


        #endregion

        #region [5]用户 => 获取用户实体
        //[WebMethod(Description = @" [5]用户 => 获取用户实体(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>
        //----------------返回字段说明------------------
        //</br>EnabledMark：是否有效用户(0-黑名单、1-有效)
        //</br>Gender:性别(1-男，2-女)
        //</br>IsOpenFutures:是否已同意合于协议  0-默认(跳协议界面，勾选同意时调用UpdateUserEntity接口)、1-已同意
        //</br>UserGrade：用户等级标识(0-9==>VIP1-VIP9)
        //</br>UserGradeFilePath:用户等级图标")]
        [WebMethod(Description ="")]
        public void GetEntityUser(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetEntityUser(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetEntityUser】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetEntityUser】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region [6]用户 => 变更用户基本信息
        //[WebMethod(Description = @" [6]用户 => 变更用户基本信息(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //Action:修改信息:<br/>
        //提现密码-WithdrawPassword、昵称-NickName、头像-HeadIcon<br/> 
        //性别-Gender（1-男、2-女）、生日-Birthday、同意合约协议-OpenFutures<br/>
        //ActionValue:值(设置交易密码拼接==>交易密码#验证码)</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void UpdateUserEntity(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Action", "ActionValue", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Fail, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.UpdateUserEntity(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UpdateUserEntity】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【UpdateUserEntity】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region [7]用户 => 更换邮箱
        //[WebMethod(Description = @"[7]用户 => 更换邮箱(输入参数string InJson)</br>
        //UserId用户编号</br>
        //NewEmail:新邮箱</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void UpdateEmail(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "NewEmail", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    //dto = bll.UpdateEmail(list);
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UpdateEmail】非法请求");
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UpdateEmail】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【UpdateEmail】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 用户 => 添加提币地址
        //[WebMethod(Description = @" 用户 => 添加提币地址(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //Remark:备注</br>
        //CoinAddress:钱包地址</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void AddUserCoinAddress(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Remark", "CoinAddress" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.InsertUserCoinAddress(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【AddUserCoinAddress】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【AddUserCoinAddress】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 获取用户提币地址列表
        //[WebMethod(Description = @" 用户 => 获取用户提币地址列表(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void GetPageUserCoinAddress(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "PageSize", "PageIndex", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);

                    //检查用户是否在线
                    UserTokenEntity userTokenEntity = bll.IsOnLine(list["UserId"]);
                    if (userTokenEntity != null)
                    {
                        //重置UserId
                        list["UserId"] = userTokenEntity.UserId;

                        Pagination pagination = new Pagination();//分页实体类
                        var watch = CommonHelper.TimerStart();
                        DataTable data = bll.GetPageUserCoinAddress(pagination, list);
                        if (data != null && data.Rows.Count > 0)
                        {
                            var JsonData = new
                            {
                                rows = data,//数据
                                total = pagination.total,//总页数
                                page = pagination.page,//当前页
                                records = pagination.records,//总记录数
                                costtime = CommonHelper.TimerEnd(watch)//耗时(毫秒)
                            };
                            dto.Code = CodeEnumClass.Success;
                            dto.Message = MessageEnumClass.Msg001[lv];
                            dto.Data = JsonData;
                        }
                        else
                        {
                            dto.Code = CodeEnumClass.DataNull;
                            dto.Message = MessageEnumClass.Msg005[lv];
                        }
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.SignOut;
                        dto.Message = MessageEnumClass.Msg100[lv];
                    }                    
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetPageUserCoinAddress】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetPageUserCoinAddress】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 删除提币地址
        //[WebMethod(Description = @" 用户 => 删除提币地址(输入参数string InJson)</br>
        //UserCoinAddressId:主键编号</br>
        //UserId:用户编号</br>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void DeleteUserCoinAddress(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserCoinAddressId", "UserId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.DeleteUserCoinAddress(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【DeleteUserCoinAddress】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【DeleteUserCoinAddress】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 获取我的推荐列表信息
        //[WebMethod(Description = @"用户 => 获取我的推荐列表信息(输入参数string InJson)<br/>
        //UserId:用户Id<br/>
        //PageSize:每页数<br/>
        //PageIndex：当前页<br/>
        //LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        [WebMethod(Description ="")]
        public void GetUserTeamInfo(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "PageSize", "PageIndex" , "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserTeamInfo(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserTeamInfo】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserTeamInfo】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 用户 => 获取我的资产信息
        //[WebMethod(Description = @"用户 => 获取我的资产信息(输入参数string InJson)</br>
        //UserId:用户Id</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/> 
        //Coin:币 <br/> 
        //Balance可用 <br/> 
        //Freeze：冻结 <br/> 
        //CoinAddress：ERC20链钱包地址 <br/> 
        //CoinAddressTRC20：OMNI钱包地址(只有USDT才有此值) <br/> 
        //USDTBalance：可用换算成USDT <br/> 
        //USDTFreeze:冻结换算成USDT <br/> 
        //CNYBalance:可用换算成CNY <br/> 
        //CNYFreeze:冻结换算成CNY <br/>
        //SetValue：提币最低数量#提币手续费(个数) <br/>")]
        [WebMethod(Description ="")]
        public void GetUserCommodityAccountsInfo(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserCommodityAccounts(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserCommodityAccountsInfo】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserCommodityAccountsInfo】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 用户 => 获取资产账户明细信息
        //[WebMethod(Description = @" 用户 => 获取资产账户明细信息(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //Coin:币种Code<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void GetPageUserCommodityAccountsDetail(string InJson)
        {
            Context.Response.ContentType = "application/json;charset=utf-8";
            string ErrMsg = "";
            string[] parm = { "UserId", "PageSize", "PageIndex", "Coin", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)

                    //检查用户是否在线
                    UserTokenEntity userTokenEntity = bll.IsOnLine(list["UserId"]);
                    if (userTokenEntity != null)
                    {
                        //重置UserId
                        list["UserId"] = userTokenEntity.UserId;

                        Pagination pagination = new Pagination();//分页实体类
                        var watch = CommonHelper.TimerStart();
                        DataTable data = bll.GetPageUserCommodityAccountsDetail(pagination, list);
                        if (data != null && data.Rows.Count > 0)
                        {
                            var JsonData = new
                            {
                                rows = data,
                                total = pagination.total,
                                page = pagination.page,
                                records = pagination.records,
                                costtime = CommonHelper.TimerEnd(watch)
                            };
                            dto.Code = CodeEnumClass.Success;
                            dto.Message = MessageEnumClass.Msg001[lv];
                            dto.Data = JsonData;
                        }
                        else
                        {
                            dto.Code = CodeEnumClass.DataNull;
                            dto.Message = MessageEnumClass.Msg005[lv];
                        }
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.SignOut;
                        dto.Message = MessageEnumClass.Msg100[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetPageUserCommodityAccountsDetail】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetPageUserCommodityAccountsDetail】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 转账提交
        //[WebMethod(Description = @" 用户 => 转账提交 (输入参数string InJson) <br/>
        //UserId：发起人用户主键</br>
        //Coin:币<br/>
        //Quantity：数量(0.01起转)</br>
        //ReceiverEnCode：目标账户EnCode</br>
        //WithdrawPassword：支付密码</br>
        //Code:验证码<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
        [WebMethod(Description ="")]
        public void UserTransfer(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Quantity", "ReceiverEnCode", "WithdrawPassword", "Code" , "Coin", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.UserTransfer(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UserTransfer】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【UserTransfer】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 用户 => 获取用户转账记录 
        //[WebMethod(Description = @"用户 => 获取用户转账记录(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------
        //</br>code:转账流水号
        //</br>createdate:转账时间
        //</br>quantity:数量
        //</br>account:对方账户
        //</br>operatdirection:收支方向(1-支出，0-收入)
        //</br>coin:币种Code")]
        [WebMethod(Description ="")]
        public void GetPageUserTransfer(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId","PageSize", "PageIndex", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)

                    //检查用户是否在线
                    UserTokenEntity userTokenEntity = bll.IsOnLine(list["UserId"]);
                    if (userTokenEntity != null)
                    {
                        //重置UserId
                        list["UserId"] = userTokenEntity.UserId;

                        Pagination pagination = new Pagination();//分页实体类
                        var watch = CommonHelper.TimerStart();
                        DataTable data = bll.GetPageUserTransfer(pagination, list);
                        if (data != null && data.Rows.Count > 0)
                        {
                            var JsonData = new
                            {
                                rows = data,
                                total = pagination.total,
                                page = pagination.page,
                                records = pagination.records,
                                costtime = CommonHelper.TimerEnd(watch)
                            };
                            dto.Code = CodeEnumClass.Success;
                            dto.Message = MessageEnumClass.Msg001[lv];
                            dto.Data = JsonData;
                        }
                        else
                        {
                            dto.Code = CodeEnumClass.DataNull;
                            dto.Message = MessageEnumClass.Msg005[lv];
                        }
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.SignOut;
                        dto.Message = MessageEnumClass.Msg100[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetPageUserTransfer】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetPageUserTransfer】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户=>提币申请
        //[WebMethod(Description = @"用户=>提币申请(输入参数string InJson) </br>
        //UserId:用户编号</br>
        //ChainName:链名称(只有USDT有ERC20,OMNI，其他的默认传defailt)</br>
        //Coin:币</br>
        //Amount:提币数量</br>
        //ToAddress:提币地址</br>
        //WithdrawPassword：支付密码</br>
        //Code:验证码<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
        [WebMethod(Description ="")]
        public void SubmitUserTakeOutCoins(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Amount", "ToAddress", "WithdrawPassword", "Code", "ChainName", "Coin", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SubmitUserTakeOutCoins(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SubmitUserTakeOutCoins】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SubmitUserTakeOutCoins】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 获取用户提币记录 
        //[WebMethod(Description = @"用户 => 获取用户提币记录(输入参数string InJson)</br>
        //UserId:用户编号</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/> 
        //coin:币 <br/> 
        //mark:状态（0-待到账、1-已到账、2-已退回） <br/> 
        //amount:数量 <br/> 
        //createdtae：提交时间 <br/> 
        //txhash:交易哈希 <br/> ")]
        [WebMethod(Description ="")]
        public void GetPageUserTakeOutCoins(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "PageSize", "PageIndex", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)

                    //检查用户是否在线
                    UserTokenEntity userTokenEntity = bll.IsOnLine(list["UserId"]);
                    if (userTokenEntity != null)
                    {
                        //重置UserId
                        list["UserId"] = userTokenEntity.UserId;

                        Pagination pagination = new Pagination();//分页实体类
                        var watch = CommonHelper.TimerStart();
                        DataTable data = bll.GetPageUserTakeOutCoins(pagination, list);
                        if (data != null && data.Rows.Count > 0)
                        {
                            var JsonData = new
                            {
                                rows = data,
                                total = pagination.total,
                                page = pagination.page,
                                records = pagination.records,
                                costtime = CommonHelper.TimerEnd(watch)
                            };
                            dto.Code = CodeEnumClass.Success;
                            dto.Message = MessageEnumClass.Msg001[lv];
                            dto.Data = JsonData;
                        }
                        else
                        {
                            dto.Code = CodeEnumClass.DataNull;
                            dto.Message = MessageEnumClass.Msg005[lv];
                        }
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.SignOut;
                        dto.Message = MessageEnumClass.Msg100[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetPageUserTakeOutCoins】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetPageUserTakeOutCoins】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 获取我的指定币种的钱包地址
        //[WebMethod(Description = @"用户 => 获取我的指定币种的钱包地址(string InJson)</br>
        //UserId:用户Id</br>
        //Coin:币种</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/> 
        //Coin:币 <br/>
        //CoinAddress：ERC20链钱包地址 <br/> 
        //CoinAddressTRC20：OMNI钱包地址(只有USDT才有此值,其他都为空) <br/> ")]
        [WebMethod(Description ="")]
        public void GetUserCoindAddress(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Coin","LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserCoindAddress(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserCoindAddress】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserCoindAddress】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 用户 => 退出登录
        //[WebMethod(Description = @"用户 => 退出登录(输入参数string InJson)
        //</br>UserId:当前用户主键
        //</br>LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)")]
        [WebMethod(Description = @"")]
        public void SignOut(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SignOut(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SignOut】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SignOut】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 用户 => 上传充币凭证
        //[WebMethod(Description = @" 用户 => 上传充币凭证 (输入参数string InJson) <br/>
        //UserId：用户token</br>
        //FilePath：图片凭证路径(必填)</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
        [WebMethod(Description = "")]
        public void UploadDepositCertificate(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "FilePath","LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.UploadDepositCertificate(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【UploadDepositCertificate】:\r\n" + ex.Message);
                }
            }
            else
            {
                Write("接口【UploadDepositCertificate】请求参数错误:\r\n" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        //==========辅助功能（81-100）ok

        #region [81]系统 => 删除文件 【无效】
        //[WebMethod(Description = "[81]系统 => 删除文件(输入参数string Json)[可对接]</br>FilePath:文件路径")]
        //public void DeleteFile(string InJson)
        //{
        //    Context.Response.ContentType = "application/json;charset=utf-8";
        //    string ErrMsg = "";
        //    string[] parm = { "FilePath" };
        //    ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
        //    Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
        //    if (ErrMsg != "")
        //    {
        //        dto.Message = ErrMsg;
        //        Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
        //        Context.Response.End();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            DirFileHelper.DeleteFile(list["FilePath"]);
        //            dto.Code = CodeEnumClass.Success;
        //            dto.Message = MessageEnumClass.Msg002;
        //        }
        //        catch (Exception ex)
        //        {
        //            dto.Code = CodeEnumClass.Abnormal;
        //            dto.Message = ex.Message;
        //        }
        //        Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
        //        Context.Response.End();
        //    }
        //}
        #endregion

        #region [82]系统 => 图片上传
        //[WebMethod(Description = @" [82]系统 => 图片上传</br>
        //EnCode:用户编号</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
        [WebMethod(Description ="")]
        public void UpLoadImg()
        {
            try
            {
                string _LanguageVersion = HttpContext.Current.Request.Form["LanguageVersion"];
                ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                string _UserCode = HttpContext.Current.Request.Form["EnCode"];
                if (file != null&& !_UserCode.IsEmpty()&&!_LanguageVersion.IsEmpty())
                {
                    int lv = MessageEnumClass.Choice(_LanguageVersion);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)
                    string _Extension = System.IO.Path.GetExtension(file.FileName).ToLower();
                    //取得文件的扩展名,并转换成小写
                    string _FileName = string.Format("{0}{1}", DateTime.Now.ToString("yyyyMMddHHmmssfff"), _Extension);
                    //文件名
                    string _AbsolutePath = System.Web.HttpContext.Current.Server.MapPath("/Resource/WebUserImage/" + _UserCode + "/");

                    //绝对路径
                    string _RelativePath = string.Format("/Resource/WebUserImage/{0}/{1}", _UserCode, _FileName);

                    //验证上传文件是否为图片
                    if (!".jpg,.gif,.bmp,.png".Contains(_Extension))
                    {
                        dto.Code = CodeEnumClass.Fail;
                        dto.Message = MessageEnumClass.Msg030[lv];
                        ResponseMethod(dto);
                    }
                    ZhongLiExpandBLL bll = new ZhongLiExpandBLL();

                    var count = bll.GetObjectValue("select count(1) from Expand_User with(nolock) where EnCode=@EnCode",new DbParameter[] { DbParameters.CreateDbParameter("@EnCode", _UserCode) }).ToInt();
                    if (count == 0) {
                        dto.Code = CodeEnumClass.Fail;
                        dto.Message = MessageEnumClass.Msg016[lv];
                        ResponseMethod(dto);
                    }

                    WebUserImageViewDto viewdto = new WebUserImageViewDto();
                    if (!Directory.Exists(_AbsolutePath))
                    {
                        Directory.CreateDirectory(_AbsolutePath);
                    }
                    file.SaveAs(string.Format("{0}{1}", _AbsolutePath, _FileName));

                    #region 返回Data实体
                    viewdto.FilePath = _RelativePath;
                    #endregion

                    #region 返回Json实体
                    dto.Code = CodeEnumClass.Success;
                    dto.Message = MessageEnumClass.Msg003[lv];
                    dto.Data = viewdto;
                    #endregion
                }

                ResponseMethod(dto);
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region [85]系统 => 获取行政区域集   【无效】
        //[WebMethod(Description = "[85]系统 => 获取行政区域集[可对接]")]
        //public void GetAreaList()
        //{
        //    ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
        //    List<AreaViewDto> list = bll.GetAreaList();
        //    if (list != null && list.Count > 0)
        //    {
        //        dto.Code = CodeEnumClass.Success;
        //        dto.Message = MessageEnumClass.Msg001;
        //        dto.Data = list;
        //    }
        //    else
        //    {
        //        dto.Code = CodeEnumClass.DataNull;
        //        dto.Message = MessageEnumClass.Msg005;
        //    }
        //    Context.Response.ContentType = "application/json;charset=utf-8";
        //    Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
        //    Context.Response.End();
        //}
        #endregion

        #region [86]系统 => 获取行政区域最后修改时间 【无效】
        //[WebMethod(Description = "[86]获取行政区域最后修改时间")]
        //public void GetAreaLastDate()
        //{
        //    ResultDto resultdto = new ResultDto { Code = CodeEnumClass.Success, Message = MessageEnumClass.Msg001 };
        //    AreaLastDateViewDto viewdto = new AreaLastDateViewDto();

        //    #region 返回的数据实体
        //    object objValue = bll.GetObjectValue("select top 1 ModifyDate from Base_Area order by ModifyDate desc");
        //    if (objValue == null)
        //        objValue = DateTime.Now;
        //    viewdto.LastDate = Extensions.ToDateTimeString(Extensions.ToDate(objValue));
        //    #endregion

        //    #region 返回的Json实体
        //    resultdto.Data = viewdto;
        //    #endregion

        //    Context.Response.ContentType = "application/json;charset=utf-8";
        //    Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(resultdto)));
        //    Context.Response.End();
        //}
        #endregion

        #region [87]系统 => 意见建议 【无效】
        //[WebMethod(Description = "[87]系统 =>意见建议(输入参数string Json)</br>UserId:用户编号（默认:default）</br>Title:标题(默认:default)</br>Content:留言内容</br>Mobile:手机(默认:default)</br>Email:邮箱(默认:default)</br>NickName:昵称(默认:游客、登陆用户-真实姓名or昵称)</br>LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        //public void InsertLeaveComments(string InJson)
        //{
        //    Context.Response.ContentType = "application/json;charset=utf-8";
        //    string ErrMsg = "";
        //    string[] parm = { "UserId", "Title", "Content", "Mobile", "Email", "NickName","LanguageVersion" };
        //    ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
        //    Dictionary<string, string> list = Json.JsonToDictionary(true,true, InJson, out ErrMsg, parm);
        //    if (ErrMsg != "")
        //    {
        //        dto.Message = ErrMsg;
        //        Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
        //        Context.Response.End();
        //    }
        //    else
        //    {
        //        dto = bll.InsertLeaveComments(list);
        //        Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(dto)));
        //        Context.Response.End();
        //    }
        //}
        #endregion

        #region [88]系统 => 发送验证码
        //[WebMethod(Description = @"[88]系统 =>发送验证码(输入参数string Json)</br>
        //Mobile:邮箱</br>
        //Title:发送类别(用户注册、密码找回、交易密码、转账、提币)</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
        [WebMethod(Description ="")]
        public void GetMobileCode(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "Mobile", "Title", "LanguageVersion" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SendMobileCode(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetMobileCode】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetMobileCode】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        //=========================//

        #region [89]系统 => 获取通知公告
        //[WebMethod(Description = @"[89]系统 =>获取通知公告(输入参数string InJson)</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void GetPageNews(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "PageSize", "PageIndex" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)
                    Pagination pagination = new Pagination();//分页实体类
                    var watch = CommonHelper.TimerStart();
                    DataTable data = bll.GetPageNews(pagination, list);
                    if (data != null && data.Rows.Count > 0)
                    {
                        var JsonData = new
                        {
                            rows = data,
                            total = pagination.total,
                            page = pagination.page,
                            records = pagination.records,
                            costtime = CommonHelper.TimerEnd(watch)
                        };
                        dto.Code = CodeEnumClass.Success;
                        dto.Message = MessageEnumClass.Msg001[lv];
                        dto.Data = JsonData;
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.DataNull;
                        dto.Message = MessageEnumClass.Msg005[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetPageNews】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetPageNews】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region [90]系统 => 获取APP版本 ok
        //[WebMethod(Description = @"[90]系统 =>获取APP版本(输入参数string InJson)</br>
        //Action:客户端（安卓、苹果）
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void GetAppVersion(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "Action", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)
                    #region 获取数据
                    StringBuilder sbSql = new StringBuilder();
                    switch (list["Action"])
                    {
                        case "安卓":
                            sbSql.Append("select top 1 * from Base_DataItemDetail where  ParentId='3c38bfb4-8947-46f8-b322-3d770e8f967d' and SortCode=@lv order by CreateDate desc");
                            break;
                        case "苹果":
                            sbSql.Append("select top 1 * from Base_DataItemDetail where   ParentId='78e0f095-7b9b-4d4a-b03a-2a39ad706208'  and SortCode=@lv order by CreateDate desc");
                            break;
                    }

                    DbParameter[] param = new DbParameter[] {
                        DbParameters.CreateDbParameter("@lv",lv)
                    };

                    DataTable dt = bll.GetListTable(sbSql.ToString(),param);
                    #endregion

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        #region 返回数据实体
                        VersionViewDto viewdto = new VersionViewDto();
                        viewdto.ItemName = dt.Rows[0]["ItemName"].ToString();
                        viewdto.ItemValue = dt.Rows[0]["ItemValue"].ToString();
                        viewdto.Description = dt.Rows[0]["Description"].ToString();
                        #endregion

                        #region 返回Json实体
                        dto.Code = CodeEnumClass.Success;
                        dto.Message = MessageEnumClass.Msg001[lv];
                        dto.Data = viewdto;
                        #endregion
                    }
                    else
                    {
                        #region 返回Json实体
                        dto.Code = CodeEnumClass.DataNull;
                        dto.Message = MessageEnumClass.Msg005[lv];
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetAppVersion】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetAppVersion】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region [91]系统 => 轮播图片 ok
        //[WebMethod(Description = @"[91]系统 =>轮播图片[可对接]</br>
        //ShowWay:显示位置（0--首页轮播图）</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void GetRotateImageList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "ShowWay", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);//语言版本(zh-CN,zh-TW,en)中简，中繁，英文
                    var images = bll.GetRotateImageList(list);
                    if (list != null && list.Count > 0)
                    {
                        dto.Code = CodeEnumClass.Success;
                        dto.Message = MessageEnumClass.Msg001[lv];
                        dto.Data = images;
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.DataNull;
                        dto.Message = MessageEnumClass.Msg005[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetRotateImageList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetRotateImageList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region [100]资讯 => 获取资讯列表 
        //[WebMethod(Description = @"[100]资讯 =>获取资讯列表(输入参数string InJson)</br>
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //----------------返回字段说明------------------
        //</br>NewsId：资讯主键
        //</br>Category:所属类别主键
        //</br>CreateDate：创建时间
        //<br/>Description:简介
        //</br>FullHead：标题
        //</br>FilePath：标题图
        //<br/>ReleaseTime:置顶时间
        //<br/>SortCode:是否置顶（等于1视为置顶）")]
        [WebMethod(Description ="")]
        public void GetInformationPageList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "PageSize", "PageIndex" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    int lv = MessageEnumClass.Choice(list["LanguageVersion"]);
                    Pagination pagination = new Pagination();//分页实体类
                    var watch = CommonHelper.TimerStart();

                    list.Add("Category", "c377c513-9322-4a83-ad1c-218b79c933b5");
                    DataTable data = bll.GetInformationPageList(pagination, list);
                    if (data != null && data.Rows.Count > 0)
                    {
                        var JsonData = new
                        {
                            rows = data,
                            total = pagination.total,
                            page = pagination.page,
                            records = pagination.records,
                            costtime = CommonHelper.TimerEnd(watch)
                        };
                        dto.Code = CodeEnumClass.Success;
                        dto.Message = MessageEnumClass.Msg001[lv];
                        dto.Data = JsonData;
                    }
                    else
                    {
                        dto.Code = CodeEnumClass.DataNull;
                        dto.Message = MessageEnumClass.Msg005[lv];
                    }
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetInformationPageList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetInformationPageList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 行情

        #region 行情 =>获取行情区域菜单列表
        //[WebMethod(Description = @"行情 =>获取行情区域菜单列表(输入参数string InJson)</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //------------------------------返回数据说明:---------------------------------------")]
        [WebMethod(Description ="")]
        public void GetSymbolsArea(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetSymbolsArea(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetSymbolsArea】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetSymbolsArea】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 行情 => 获取全部symbol的交易行情列表
        //[WebMethod(Description = @" 行情 => 获取全部symbol的交易行情列表(输入参数string InJson)</br>
        //PageIndex:页码<br/>
        //PageSize:显示条数<br/> 
        //AreaId:区域主键(默认传default)<br/> 
        //Sidx:排序字段(排序号-SortCode,涨跌幅-GainRate，最新价-ClosePrice,名称-BaseCurrency，默认是排序号升序)<br/>
        //Sord:排序方式（desc-降序,asc-升序）<br/>
        //KeyWord:关键字(默认传default)<br/>
        //Flag:列表标识(默认传default,1-站内转账币列表、2-币币行情列表、3-合约行情列表)<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //------------------------------返回数据说明:---------------------------------------
        //<br/>SymbolsId:主键
        //<br/>Symbol：交易对
        //<br/>BaseCurrency：基础币种
        //<br/>QuoteCurrency：计价币种
        //<br/>Amount：24小时成交量
        //<br/>OpenPrice：开盘价
        //<br/>ClosePrice：当前成交价
        //<br/>HighPrice：最高价
        //<br/>LowPrice：最低价
        //<br/>Vol：24小时成交额
        //<br/>EnabledMark：是否有效
        //<br/>SortCode：排序号
        //<br/>CreateDate：创建时间
        //<br/>GainRate：涨幅比例((现价-开盘价)/开盘价),此值自行换算成百分比形式
        //<br/>FilePath:币种图片(此项目App无用)
        //<br/>RisingValue：涨幅值
        //<br/>Unit:是否撮合交易（0-不支持撮合交易、1-支持撮合交易，不支持时行情也详情下面不出现买入卖出按钮）
        //<br/>IsC2C:是否支持C2C交易（0-不支持、1-支持）
        //<br/>Buy1Price:当前价换算成CNY的值
        //<br/>IsFutures:是否支持合约交易(0-不支持、1-支持)")]
        [WebMethod(Description ="")]
        public void GetSymbolsPageList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "PageIndex", "PageSize", "AreaId", "Sidx", "Sord", "Flag", "KeyWord", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetSymbolsPageList(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetSymbolsPageList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetSymbolsPageList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 行情 =>获取symbol的详情信息
        //[WebMethod(Description = @"行情 =>获取symbol的详情信息(输入参数string InJson)</br>
        //SymbolsId:主键编号<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //------------------------------返回数据说明:---------------------------------------
        //<br/>SymbolsId:主键
        //<br/>Symbol：交易对
        //<br/>BaseCurrency：基础币种
        //<br/>QuoteCurrency：计价币种
        //<br/>Amount：24小时成交量
        //<br/>OpenPrice：开盘价
        //<br/>ClosePrice：当前成交价
        //<br/>HighPrice：最高价
        //<br/>LowPrice：最低价
        //<br/>Vol：24小时成交额
        //<br/>EnabledMark：是否有效
        //<br/>SortCode：排序号
        //<br/>CreateDate：创建时间
        //<br/>GainRate：涨幅比例((现价-开盘价)/开盘价),此值自行换算成百分比形式
        //<br/>FilePath:币种图片(此项目App无用)
        //<br/>RisingValue：涨幅值
        //<br/>Unit:是否撮合交易（0-不支持撮合交易、1-支持撮合交易，不支持时行情也详情下面不出现买入卖出按钮）
        //<br/>IsC2C:是否主流币（0-不是、1-是）
        //<br/>Buy1Price:当前价换算成CNY的值")]
        [WebMethod(Description ="")]
        public void GetSymbolsEntity(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "SymbolsId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error"};
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetSymbolsEntity(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetSymbolsEntity】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetSymbolsEntity】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 行情 =>获取单个symbol的K线数据列表
        //[WebMethod(Description = @"行情 =>获取单个symbol的K线数据列表(输入参数string InJson)</br>
        //SymbolsId:主键编号<br/>
        //Period：K线类型:1min, 5min, 15min, 30min, 60min,4hour,1day,1week<br/>
        //LastTimeStamp:上次的时间撮(默认传default)<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>
        //------------------------------返回数据说明:---------------------------------------
        //<br/>TimeStamp:时间撮
        //<br/>OpenPrice：开盘价
        //<br/>ClosePrice：收盘价(最后成交价)
        //<br/>LowPrice:最低价
        //<br/>HighPrice:最高价
        //<br/>Amount：成交量
        //<br/>CreateDate:时间刻度")]
        [WebMethod(Description ="")]
        public void GetSymbolsKline(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "SymbolsId", "Period", "LastTimeStamp", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetSymbolsKline(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetSymbolsKline】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetSymbolsKline】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 行情 =>获取交易对的买1-买5，卖1-卖5数据(只有支持撮合交易的交易对有)
        //[WebMethod(Description = @" 行情 =>获取交易对的买1-买5，卖1-卖5数据(输入参数string InJson)</br>
        //SymbolsId:主键编号<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //------------------------------返回数据说明:---------------------------------------
        //<br/>SymbolsId:主键        
        //<br/>JsonString:买1-5，卖1-5Json数据（bids(买盘,[price(成交价), amount(成交量)], 按price降序,)，asks（卖盘,[price(成交价), amount(成交量)], 按price升序））
        //<br/>ClosePrice:当前价
        //<br/>ClosePriceCNY：当前CNY价
        //<br/>GainRate：涨幅比例")]
        [WebMethod(Description ="")]
        public void GetSymbolsMarketDepth(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "SymbolsId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetSymbolsMarketDepth(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetSymbolsMarketDepth】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetSymbolsMarketDepth】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #endregion

        #region 撮合交易

        #region 撮合交易 => 用户提交委托
        //[WebMethod(Description = @"撮合交易 => 用户提交委托(输入参数string InJson)</br>
        //UserId:用户ID</br>
        //Category:委托方向(1-买入、2-卖出)<br/>        
        //SymbolsId:交易对主键编号<br/>        
        //Quantity:数量</br>
        //Price:价格<br/>
        //Password:支付密码<br/> 
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void SubmitDelegateOrders(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Quantity", "Price", "SymbolsId", "Category", "Password", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
           
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SubmitDelegateOrders(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SubmitDelegateOrders】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SubmitDelegateOrders】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #region 撮合交易=> 获取用户当前委托列表
        //[WebMethod(Description = @"撮合交易=> 获取用户当前委托列表(输入参数string InJson)</br> 
        //UserId:用户主键<br/>        
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //----------字段返回---------------
        //</br>DelegateOrdersId:委托单主键
        //</br>Category：委托方向(1-买入、2-卖出)
        //</br>CreateDate：委托时间
        //</br>BaseCurrency：基础币种
        //</br>QuoteCurrency：计价币种
        //</br>Quantity：委托剩余量
        //</br>Price：委托单价")]
        [WebMethod(Description ="")]
        public void GetDelegateOrdersPageList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "PageSize", "PageIndex", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetDelegateOrdersPageList(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetDelegateOrdersPageList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetDelegateOrdersPageList】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #region 撮合交易=> 获取用户历史委托(成交记录)列表
        //[WebMethod(Description = @"撮合交易=> 获取用户历史委托(成交记录)列表(输入参数string InJson) [可对接]</br> 
        //UserId:用户主键<br/>
        //Category:委托方向(1-买入、2-卖出)<br/>     
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //----------字段返回---------------
        //</br>CreateDate：成交时间
        //</br>BaseCurrency：基础币种
        //</br>QuoteCurrency：计价币种
        //</br>Quantity：成交量
        //</br>Price：成交单价")]
        [WebMethod(Description ="")]
        public void GetCouplingOrdersPageList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "Category", "PageSize", "PageIndex" , "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
          
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetCouplingOrdersPageList(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetCouplingOrdersPageList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetCouplingOrdersPageList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 撮合交易 => 撤回委托订单
        //[WebMethod(Description = @" 撮合交易 => 撤回委托订单[可对接]</br>
        //UserId:用户编号</br>
        //DelegateOrdersId:委托订单主键<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description ="")]
        public void CancelDelegateOrders(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "DelegateOrdersId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.CancelDelegateOrders(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【CancelDelegateOrders】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【CancelDelegateOrders】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 撮合交易 => 获取账户针对Symbol的详情信息
        //[WebMethod(Description = @"撮合交易 => 获取Symbol的详情信息</br>
        //UserId:用户编号</br>
        //SymbolsId:交易对主键编号<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //----------------返回字段说明-----------------
        //<br/>SymbolsId：主键
        //<br/>Symbol：交易对名称
        //<br/>BaseCurrency：基础币种
        //<br/>QuoteCurrency：计价币种
        //<br/>EnabledMark：状态(1-有效，0-无效)
        //<br/>ClosePrice：当前价
        //<br/>GainRate：涨幅比例
        //<br/>ClosePriceCNY:当前价换数成CNY价格
        //<br/>BaseCurrencyBalance：基础币种账户余额(卖出用)
        //<br/>QuoteCurrencyBalance：计价币种账户余额(买入用)")]
        [WebMethod(Description ="")]
        public void GetCombinationSymbolsEntity(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
           
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetCombinationSymbolsEntity(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetCombinationSymbolsEntity】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetCombinationSymbolsEntity】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #endregion

        #region 合约交易

        #region 合约交易 => 获取合约账户划入、划出页面可用余额
        //[WebMethod(Description = @"合约交易 => 获取合约账户划入、划出页面可用余额(输入参数string InJson)</br>
        //UserId:用户ID</br>     
        //SymbolsId:交易对主键编号<br/>        
        //Direction:操作方向(Into-划入页面可用余额、DrawOut-划出页面可用余额)</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description = "")]
        public void GetTransferBalanceByDirection(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId",  "SymbolsId", "Direction", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetTransferBalanceByDirection(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetTransferBalanceByDirection】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetTransferBalanceByDirection】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #region 合约交易 =>提交合约账户划入、划出
        //[WebMethod(Description = @"合约交易 => 提交合约账户划入、划出(输入参数string InJson)</br>
        //UserId:用户ID</br>     
        //SymbolsId:交易对主键编号<br/>        
        //Direction:操作方向(Into-币币账户划入到合约账户、DrawOut-合约账户划出到币币账户)</br>
        //Quantity：数量<br/>
        //Password:支付密码<br/>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description = "")]
        public void SubmitFuturesTransfer(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "Direction", "Quantity", "Password", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SubmitFuturesTransfer_new(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SubmitFuturesTransfer】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SubmitFuturesTransfer】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #region 合约交易 => 获取我的合约资产列表
        //[WebMethod(Description = @"合约交易 => 获取我的合约资产列表(输入参数string InJson)</br>
        //UserId:用户Id</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/> 
        //symbolsid:合约交易对主键<br/>
        //basecurrency：合约名称<br/>
        //balance:账户总权益(USDT)<br/>
        //stoplossprice：预估强平价(USDT)<br/>
        //profitlossratio:保证金资产率(自行换算成百分比形式)<br/>
        //")]
        [WebMethod(Description = "")]
        public void GetUserFuturesAccountsList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserFuturesAccountsList(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserFuturesAccountsList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserFuturesAccountsList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 合约交易 => 获取我的单个合约资产
        //[WebMethod(Description = @"合约交易 => 获取我的单个合约资产(输入参数string InJson)</br>
        //UserId:用户Id</br>
        //SymbolsId:交易对主键</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/> 
        //SymbolsId:合约交易对主键<br/>
        //BaseCurrency：合约名称<br/>
        //TotalAssets:总资产(USDT)<br/>
        //Balance:USDT账户总权益<br/>
        //StopLossPrice：预估强平价USDT<br/>
        //ProfitLossRatio:保证金资产率(自行换算成百分比形式)<br/>
        //")]
        [WebMethod(Description = "")]
        public void GetUserFuturesAccountsEntity(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserFuturesAccountsEntity(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserFuturesAccountsEntity】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserFuturesAccountsEntity】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 合约交易 => 用户提交合约开仓
        //[WebMethod(Description = @"合约交易 => 用户提交合约开仓(输入参数string InJson)</br>
        //UserId:用户ID</br>
        //SymbolsId:交易对主键编号<br/>        
        //Flag:委托方向(0-开多、1-开空)<br/>        
        //LeverageMultiple:杠杆倍数(3-50倍证数)</br>
        //OpenType:开仓类型(1-全仓、2-半仓)<br/>
        //Password:支付密码<br/> 
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description = "")]
        public void SubmitOpenFutures(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "Flag", "LeverageMultiple", "OpenType", "Password", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SubmitOpenFutures(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SubmitOpenFutures】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SubmitOpenFutures】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #region 合约交易 => 获取我的单个合约的持仓信息
        //[WebMethod(Description = @"合约交易 => 获取我的单个合约的持仓信息(输入参数string InJson)</br>
        //UserId:用户Id</br>
        //SymbolsId:交易对主键</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
        //-------------返回字段备注------------ <br/>
        //OrderId:持仓订单主键<br/>
        //Code:流水号<br/>
        //BaseCurrency:合约名称<br/>
        //Flag:委托类型（0-开多、1-开空)<br/>
        //PositionNum:持仓量/可平量(单位USDT)<br/>
        //CreatePrice:开仓均价(单位USDT)<br/>
        //NowPrice:持仓均价(单位USDT)<br/>
        //Margin:保证金(单位USDT)<br/>
        //LeverageMultiple:杠杆倍数<br/>
        //CreateDate:开仓时间<br/>
        //Income:收益(单位USDT)<br/>
        //IncomeRate:收益率(自行换算成百分比形式)<br/>
        //")]
        [WebMethod(Description = "")]
        public void GetUserPositionEntity(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);
            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.GetUserPositionEntity(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserPositionEntity】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserPositionEntity】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }

        #endregion

        #region 合约交易=> 获取用户合约历史记录
        //[WebMethod(Description = @"合约交易=> 获取用户合约历史记录(输入参数string InJson)</br> 
        //UserId:用户主键<br/>
        //SymbolsId:交易对主键<br/>     
        //PageSize:每页数</br>
        //PageIndex:当前页</br>
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
        //----------字段返回---------------
        //<br/>OrderId:订单主键
        //<br/>Code：流水号
        //<br/>BaseCurrency：合约名称
        //<br/>Flag：委托类型(0-开多、1-开空)
        //<br/>CloseDate：平仓时间
        //<br/>PositionNum：委托量/成交量(单位USDT)
        //<br/>CreatePrice：委托价(单位USDT)
        //<br/>Income：收益(单位USDT)
        //<br/>ClosePrice：成交均价(单位USDT)
        //<br/>Fee：手续费(单位USDT)
        //<br/>LeverageMultiple：杠杆倍数
        //")]
        [WebMethod(Description = "")]
        public void GetUserFuturesHistoryList(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "SymbolsId", "PageSize", "PageIndex", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    Pagination pagination = new Pagination();
                    dto = bll.GetUserFuturesHistoryList(pagination,list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【GetUserFuturesHistoryList】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【GetUserFuturesHistoryList】请求参数错误:<br/>" + ErrMsg);
            }
            ResponseMethod(dto);
        }
        #endregion

        #region 合约交易 => 提交平仓
        //[WebMethod(Description = @"合约交易 => 提交平仓(输入参数string InJson)</br>
        //UserId:用户ID</br>
        //OrderId:持仓订单主键<br/>        
        //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
        [WebMethod(Description = "")]
        public void SubmitCloseFutures(string InJson)
        {
            string ErrMsg = "";
            string[] parm = { "UserId", "OrderId", "LanguageVersion" };
            ResultDto dto = new ResultDto { Code = CodeEnumClass.Abnormal, Message = "Parameter error" };
            Dictionary<string, string> list = Json.JsonToDictionary(true, true, InJson, out ErrMsg, parm);

            if (ErrMsg.IsEmpty())
            {
                try
                {
                    dto = bll.SubmitCloseFutures(list);
                }
                catch (Exception ex)
                {
                    dto.Code = CodeEnumClass.Abnormal;
                    dto.Message = "Program error";
                    Write("接口【SubmitCloseFutures】:<br/>" + ex.Message);
                }
            }
            else
            {
                Write("接口【SubmitCloseFutures】请求参数错误:<br/>" + ErrMsg);
            }

            ResponseMethod(dto);
        }
        #endregion

        #endregion

    }
}

// 竞猜=》获取竞猜界面初始化数据
// 接口名：GetGamePageData
// 接口请求参数：
// UserId:用户token
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// Balance:账户USDT余额
// GameProjectId：当前开奖期数的主键
// Period：当前期数
// TotalSecond：开奖时间剩余秒数(当剩余秒数小于等于0时调用【GetCurrentProjectData】接口获取当前开奖期数相关数据，等待本期剩余秒数小于等于0再次调用【GetCurrentProjectData】接口)
// GameRule：玩法介绍

// 竞猜=》获取游戏当前开奖期数相关数据
// 接口名：GetCurrentProjectData
// 接口请求参数：
// UserId:用户token
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// GameProjectId：当前开奖期数的主键
// Period：当前期数
// TotalSecond：开奖时间剩余秒数(当剩余秒数小于等于0时调用【GetCurrentProjectData】接口获取当前开奖期数相关数据，等待本期剩余秒数小于等于0再次调用【GetCurrentProjectData】接口)

// 竞猜 => 获取中奖广播列表
// 接口名：GetWinningBroadcastList
// 接口请求参数：
// UserId:用户token
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// nickname：中奖账号
// amount：中奖金额(USDT)
// description：轮播的内容

// 竞猜 => 下注
// 接口名：GameBetting
// 接口请求参数：
// UserId:用户token
// GameProjectId：期数主键
// BettingTarget:投注对象(1-苹果、2-和局、3-西瓜)
// Amount:下注金额
// WithdrawPassword:交易密码
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// Balance:余额

// 竞猜 => 获取开奖记录
// 接口名：GetGameLotteryRecord
// 接口请求参数：
// UserId:用户token
// PageSize:每页数
// PageIndex:当前页
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// period:期数
// lotteryresult:开奖结果(1-看涨、2-看跌)
// startprice:本期标记价格
// endprice:行权价格
// completedate:开奖时间

// 竞猜 => 下注记录
// 接口名：GameBettingRecord
// 接口请求参数：
// UserId:用户token
// PageSize:每页数
// PageIndex:当前页
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 接口返回参数：
// recordid:下注记录主键
// period:期数
// status:下注状态(1-投注中、2-待开奖、3-待结算、4-已结束)
// bettingtarget:开奖结果(1-看涨、2-看跌)
// profit：盈亏(如果等于0时用“--”显示)
// Amount:下注金额(USDT)
// lotteryresult:开奖结果(1-看涨、2-看跌)
// startprice:本本期标记价格
// endprice:行权价格
// createdate：下注时间
// deliverytype:交割类型(1-正常、2-快速交割；如果是快速交割，期数后面就显示闪电的图标，否则不显示)

// 竞猜 => 快速交割
// 接口名：FastDelivery
// 接口请求参数：
// UserId:用户token
// RecordId：下注记录主键
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)