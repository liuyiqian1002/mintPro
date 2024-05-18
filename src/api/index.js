import axios from '@/libs/api.request'
import qs from 'qs';
/**
 * 
 * @param {Requst Payload} params 
 * let data = params;
 * @param {Form Data} params 
 * let data = qs.stringify(params);
 */
// test
// //region [1]用户 => 用户注册
export const Registered = (data) => {
        return axios.request({
            url: '/Registered',
            data,
            method: 'post'
        })
    }
    // //region [2]用户 => 用户登录
export const Login = (data) => {
    return axios.request({
        url: '/Login',
        data,
        method: 'post'
    })
}
export const GetMobileAreaCodeList = (data) => {
    return axios.request({
        url: '/GetMobileAreaCodeList',
        data,
        method: 'post'
    })
}

// //region [3]用户 => 修改登录密码
//[WebMethod(Description = @"[3]用户 => 修改登录密码(输入参数string InJson)</br>
//UserId:用户编号</br>
//OldPassWord:旧密码</br>
//NewPassWord:新密码</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void UpdatePassword(string InJson)
export const UpdatePassword = (data) => {
    return axios.request({
        url: '/UpdatePassword',
        data,
        method: 'post'
    })
}

// //region [4]用户 => 重置登录密码(密码找回)
//[WebMethod(Description = @"[4]用户 => 重置登录密码(输入参数string InJson)</br>
//Account:登录账户(邮箱)</br>
//NewPassword:新密码</br>
//Code:验证码<br/>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void ResetPassword(string InJson)
export const ResetPassword = (data) => {
    return axios.request({
        url: '/ResetPassword',
        data,
        method: 'post'
    })
}




// //region [5]用户 => 获取用户实体

//[WebMethod(Description = @" [5]用户 => 获取用户实体(输入参数string InJson)</br>
//UserId:用户编号</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>
//----------------返回字段说明------------------
//</br>EnabledMark：是否有效用户(0-黑名单、1-有效)
//</br>Gender:性别(1-男，2-女)
//</br>IsOpenFutures:是否已同意合于协议  0-默认(跳协议界面，勾选同意时调用UpdateUserEntity接口)、1-已同意
//</br>UserGrade：用户等级标识(0-9==>VIP1-VIP9)
//</br>UserGradeFilePath:用户等级图标")]
// //public void GetEntityUser(string InJson)
export const GetEntityUser = (data) => {
    return axios.request({
        url: '/GetEntityUser',
        data,
        method: 'post'
    })
}
export const GetMinSellCoinQuantity = (data) => {
    return axios.request({
        url: '/GetMinSellCoinQuantity',
        data,
        method: 'post'
    })
}

// //region [6]用户 => 变更用户基本信息

//[WebMethod(Description = @" [6]用户 => 变更用户基本信息(输入参数string InJson)</br>
//UserId:用户编号</br>
//Action:修改信息:<br/>
//提现密码-WithdrawPassword、昵称-NickName、头像-HeadIcon<br/> 
//性别-Gender（1-男、2-女）、生日-Birthday、同意合约协议-OpenFutures<bååår/>
//ActionValue:值(设置交易密码拼接==>交易密码//验证码)</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void UpdateUserEntity(string InJson)
export const UpdateUserEntity = (data) => {
    return axios.request({
        url: '/UpdateUserEntity',
        data,
        method: 'post'
    })
}
// CancelQuickBuyCoin
// 快捷买币卖币 => 提交取消买币订单
export const CancelQuickBuyCoin = (data) => {
    return axios.request({
        url: '/CancelQuickBuyCoin',
        data,
        method: 'post'
    })
}
// GetBuyCoinOrderDetail
// 快捷买币卖币 => 获取买币详情
export const GetBuyCoinOrderDetail = (data) => {
    return axios.request({
        url: '/GetBuyCoinOrderDetail',
        data,
        method: 'post'
    })
}
export const GetSellCoinOrderDetail = (data) => {
    return axios.request({
        url: '/GetSellCoinOrderDetail',
        data,
        method: 'post'
    })
}
// GetBuyLegalCurrencyList
// 快捷买币卖币 => 获取买币支持的法币列表
export const GetBuyLegalCurrencyList = (data) => {
    return axios.request({
        url: '/GetBuyLegalCurrencyList',
        data,
        method: 'post'
    })
}
// GetPageBuyCoinOrder
// 快捷买币卖币 => 获取买币记录
export const GetPageBuyCoinOrder = (data) => {
    return axios.request({
        url: '/GetPageBuyCoinOrder',
        data,
        method: 'post'
    })
}
// GetPageSellCoinOrder
// 快捷买币卖币 => 获取卖币记录
export const GetPageSellCoinOrder = (data) => {
    return axios.request({
        url: '/GetPageSellCoinOrder',
        data,
        method: 'post'
    })
}
// GetSellLegalCurrencyList
// 快捷买币卖币 => 获取卖币支持的法币列表
export const GetSellLegalCurrencyList = (data) => {
    return axios.request({
        url: '/GetSellLegalCurrencyList',
        data,
        method: 'post'
    })
}
// SubmitQuickBuyCoin
// 快捷买币卖币 => 提交快捷买币
export const SubmitQuickBuyCoin = (data) => {
    return axios.request({
        url: '/SubmitQuickBuyCoin',
        data,
        method: 'post'
    })
}
// SubmitQuickSellCoin
// 快捷买币卖币 => 提交快捷卖币
export const SubmitQuickSellCoin = (data) => {
    return axios.request({
        url: '/SubmitQuickSellCoin',
        data,
        method: 'post'
    })
}
// GetQuicklyBuyCoinsUrl
// 快捷买币卖币 =>获取快捷买币链接(输入参数string InJson)

// //region [7]用户 => 更换邮箱
//[WebMethod(Description = @"[7]用户 => 更换邮箱(输入参数string InJson)</br>
//UserId用户编号</br>
//NewEmail:新邮箱</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// [WebMethod(Description = "")]
// //public void UpdateEmail(string InJson)
export const UpdateEmail = (data) => {
    return axios.request({
        url: '/UpdateEmail',
        data,
        method: 'post'
    })
}

// //region 用户 => 添加提币地址
//[WebMethod(Description = @" 用户 => 添加提币地址(输入参数string InJson)</br>
//UserId:用户编号</br>
//Remark:备注</br>
//CoinAddress:钱包地址</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void AddUserCoinAddress(string InJson)
export const AddUserCoinAddress = (data) => {
    return axios.request({
        url: '/AddUserCoinAddress',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取用户提币地址列表
//[WebMethod(Description = @" 用户 => 获取用户提币地址列表(输入参数string InJson)</br>
//UserId:用户编号</br>
//PageSize:每页数</br>
//PageIndex:当前页</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void GetPageUserCoinAddress(string InJson)
export const GetPageUserCoinAddress = (data) => {
        return axios.request({
            url: '/GetPageUserCoinAddress',
            data,
            method: 'post'
        })
    }
    // //region 用户 => 删除提币地址
export const DeleteUserCoinAddress = (data) => {
    return axios.request({
        url: '/DeleteUserCoinAddress',
        data,
        method: 'post'
    })
}

// //region 用户 => 获取我的推荐列表信息
export const GetUserTeamInfo = (data) => {
    return axios.request({
        url: '/GetUserTeamInfo',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取我的资产信息
export const GetUserCommodityAccountsInfo = (data) => {
    return axios.request({
        url: '/GetUserCommodityAccountsInfo',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取资产账户明细信息
export const GetPageUserCommodityAccountsDetail = (data) => {
    return axios.request({
        url: '/GetPageUserCommodityAccountsDetail',
        data,
        method: 'post'
    })
}

// //region 用户 => 转账提交
//[WebMethod(Description = @" 用户 => 转账提交 (输入参数string InJson) <br/>
//UserId：发起人用户主键</br>
//Coin:币<br/>
//Quantity：数量(0.01起转)</br>
//ReceiverEnCode：目标账户EnCode</br>
//WithdrawPassword：支付密码</br>
//Code:验证码<br/>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
// //public void UserTransfer(string InJson)
export const UserTransfer = (data) => {
    return axios.request({
        url: '/UserTransfer',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取用户转账记录 
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
// //public void GetPageUserTransfer(string InJson)
export const GetPageUserTransfer = (data) => {
        return axios.request({
            url: '/GetPageUserTransfer',
            data,
            method: 'post'
        })
    }
    // //region 用户=>提币申请
    //[WebMethod(Description = @"用户=>提币申请(输入参数string InJson) </br>
    //UserId:用户编号</br>
    //ChainName:链名称(只有USDT有ERC20,OMNI，其他的默认传defailt)</br>
    //Coin:币</br>
    //Amount:提币数量</br>
    //ToAddress:提币地址</br>
    //WithdrawPassword：支付密码</br>
    //Code:验证码<br/>
    //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
    // //public void SubmitUserTakeOutCoins(string InJson)
export const SubmitUserTakeOutCoins = (data) => {
    return axios.request({
        url: '/SubmitUserTakeOutCoinsV2',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取用户提币记录 
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
// //public void GetPageUserTakeOutCoins(string InJson)
export const GetPageUserTakeOutCoins = (data) => {
    return axios.request({
        url: '/GetPageUserTakeOutCoins',
        data,
        method: 'post'
    })
}


// //region 用户 => 获取我的指定币种的钱包地址
//[WebMethod(Description = @"用户 => 获取我的指定币种的钱包地址(string InJson)</br>
//UserId:用户Id</br>
//Coin:币种</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> 
//-------------返回字段备注------------ <br/> 
//Coin:币 <br/>
//CoinAddress：ERC20链钱包地址 <br/> 
//CoinAddressTRC20：OMNI钱包地址(只有USDT才有此值,其他都为空) <br/> ")]
// [WebMethod(Description ="")]
// //public void GetUserCoindAddress(string InJson)
export const GetUserCoindAddress = (data) => {
        return axios.request({
            url: '/GetUserCoindAddress',
            data,
            method: 'post'
        })
    }
    // //region 用户 => 退出登录
export const SignOut = (data) => {
    return axios.request({
        url: '/SignOut',
        data,
        method: 'post'
    })
}


// //region 用户 => 上传充币凭证
//[WebMethod(Description = @" 用户 => 上传充币凭证 (输入参数string InJson) <br/>
//UserId：用户token</br>
//FilePath：图片凭证路径(必填)</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
// [WebMethod(Description = "")]
////public void UploadDepositCertificate(string InJson)
export const UploadDepositCertificate = (data) => {
    return axios.request({
        url: '/UploadDepositCertificate',
        data,
        method: 'post'
    })
}

//==========辅助功能（81-100）ok

//region [81]系统 => 删除文件 【无效】
//[WebMethod(Description = "[81]系统 => 删除文件(输入参数string Json)[可对接]</br>FilePath:文件路径")]
////public void DeleteFile(string InJson)
export const DeleteFile = (data) => {
    return axios.request({
        url: '/DeleteFile',
        data,
        method: 'post'
    })
}

//region [82]系统 => 图片上传
//[WebMethod(Description = @" [82]系统 => 图片上传</br>
//EnCode:用户编号</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
//public void UpLoadImg()
export const UpLoadImg = (data) => {
        return axios.request({
            url: '/UpLoadImg',
            data,
            method: 'post'
        })
    }
    //endregion

//region [85]系统 => 获取行政区域集   【无效】
//[WebMethod(Description = "[85]系统 => 获取行政区域集[可对接]")]
////public void GetAreaList()
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
//endregion

//region [86]系统 => 获取行政区域最后修改时间 【无效】
//[WebMethod(Description = "[86]获取行政区域最后修改时间")]
////public void GetAreaLastDate()
//{
//    ResultDto resultdto = new ResultDto { Code = CodeEnumClass.Success, Message = MessageEnumClass.Msg001 };
//    AreaLastDateViewDto viewdto = new AreaLastDateViewDto();

//    //region 返回的数据实体
//    object objValue = bll.GetObjectValue("select top 1 ModifyDate from Base_Area order by ModifyDate desc");
//    if (objValue == null)
//        objValue = DateTime.Now;
//    viewdto.LastDate = Extensions.ToDateTimeString(Extensions.ToDate(objValue));
//    //endregion

//    //region 返回的Json实体
//    resultdto.Data = viewdto;
//    //endregion

//    Context.Response.ContentType = "application/json;charset=utf-8";
//    Context.Response.Write(AppDataEncrypt.Encrypt(Json.ToJson(resultdto)));
//    Context.Response.End();
//}
//endregion

//region [87]系统 => 意见建议 【无效】
//[WebMethod(Description = "[87]系统 =>意见建议(输入参数string Json)</br>UserId:用户编号（默认:default）</br>Title:标题(默认:default)</br>Content:留言内容</br>Mobile:手机(默认:default)</br>Email:邮箱(默认:default)</br>NickName:昵称(默认:游客、登陆用户-真实姓名or昵称)</br>LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
////public void InsertLeaveComments(string InJson)
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
//endregion

//region [88]系统 => 发送验证码
//[WebMethod(Description = @"[88]系统 =>发送验证码(输入参数string Json)</br>
//Mobile:邮箱</br>
//Title:发送类别(用户注册、密码找回、交易密码、转账、提币)</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/> ")]
//public void GetMobileCode(string InJson)
export const GetMobileCode = (data) => {
    return axios.request({
        url: '/GetMobileCode',
        data,
        method: 'post'
    })
}

//=========================//

//region [89]系统 => 获取通知公告
//[WebMethod(Description = @"[89]系统 =>获取通知公告(输入参数string InJson)</br>
//PageSize:每页数</br>
//PageIndex:当前页</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
// [WebMethod(Description ="")]
//public void GetPageNews(string InJson)
export const GetPageNews = (data) => {
    return axios.request({
        url: '/GetPageNews',
        data,
        method: 'post'
    })
}

//region [90]系统 => 获取APP版本 ok
//[WebMethod(Description = @"[90]系统 =>获取APP版本(输入参数string InJson)</br>
//Action:客户端（安卓、苹果）
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
// [WebMethod(Description = "")]
//public void GetAppVersion(string InJson)
export const GetAppVersion = (data) => {
        return axios.request({
            url: '/GetAppVersion',
            data,
            method: 'post'
        })
    }
    //region [91]系统 => 轮播图片 ok
    //[WebMethod(Description = @"[91]系统 =>轮播图片[可对接]</br>
    //ShowWay:显示位置（0--首页轮播图）</br>
    //LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
    //public void GetRotateImageList(string InJson)
export const GetRotateImageList = (data) => {
        return axios.request({
            url: '/GetRotateImageList',
            data,
            method: 'post'
        })
    }
    //region [100]资讯 => 获取资讯列表 
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
    // [WebMethod(Description ="")]
    //public void GetInformationPageList(string InJson)
export const GetInformationPageList = (data) => {
    return axios.request({
        url: '/GetInformationPageListV2',
        data,
        method: 'post'
    })
}

//region 行情
//region 行情 =>获取行情区域菜单列表
//[WebMethod(Description = @"行情 =>获取行情区域菜单列表(输入参数string InJson)</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
//------------------------------返回数据说明:---------------------------------------")]
//public void GetSymbolsArea(string InJson)
export const GetSymbolsArea = (data) => {
    return axios.request({
        url: '/GetSymbolsArea',
        data,
        method: 'post'
    })
}

//endregion

//region 行情 => 获取全部symbol的交易行情列表
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
//public void GetSymbolsPageList(string InJson)
export const GetSymbolsPageList = (data) => {
        return axios.request({
            url: '/GetSymbolsPageList',
            data,
            method: 'post'
        })
    }
    //endregion

//region 行情 =>获取symbol的详情信息
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
// [WebMethod(Description = "")]
//public void GetSymbolsEntity(string InJson)
export const GetSymbolsEntity = (data) => {
        return axios.request({
            url: '/GetSymbolsEntity',
            data,
            method: 'post'
        })
    }
    //endregion

//region 行情 =>获取单个symbol的K线数据列表
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
//public void GetSymbolsKline(string InJson)
export const GetSymbolsKline = (data) => {
    return axios.request({
        url: '/GetSymbolsKline',
        data,
        method: 'post'
    })
}

//endregion

//region 行情 =>获取交易对的买1-买5，卖1-卖5数据(只有支持撮合交易的交易对有)
//[WebMethod(Description = @" 行情 =>获取交易对的买1-买5，卖1-卖5数据(输入参数string InJson)</br>
//SymbolsId:主键编号<br/>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>
//------------------------------返回数据说明:---------------------------------------
//<br/>SymbolsId:主键        
//<br/>JsonString:买1-5，卖1-5Json数据（bids(买盘,[price(成交价), amount(成交量)], 按price降序,)，asks（卖盘,[price(成交价), amount(成交量)], 按price升序））
//<br/>ClosePrice:当前价
//<br/>ClosePriceCNY：当前CNY价
//<br/>GainRate：涨幅比例")]
// [WebMethod(Description ="")]
//public void GetSymbolsMarketDepth(string InJson)
export const GetSymbolsMarketDepth = (data) => {
        return axios.request({
            url: '/GetSymbolsMarketDepth',
            data,
            method: 'post'
        })
    }
    //endregion

//endregion

//region 撮合交易

//region 撮合交易 => 用户提交委托
//[WebMethod(Description = @"撮合交易 => 用户提交委托(输入参数string InJson)</br>
//UserId:用户ID</br>
//Category:委托方向(1-买入、2-卖出)<br/>        
//SymbolsId:交易对主键编号<br/>        
//Quantity:数量</br>
//Price:价格<br/>
//Password:支付密码<br/> 
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
//public void SubmitDelegateOrders(string InJson)
export const SubmitDelegateOrders = (data) => {
        return axios.request({
            url: '/SubmitDelegateOrders',
            data,
            method: 'post'
        })
    }
    //endregion

//region 撮合交易=> 获取用户当前委托列表
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
//public void GetDelegateOrdersPageList(string InJson)
export const GetDelegateOrdersPageList = (data) => {
        return axios.request({
            url: '/GetDelegateOrdersPageList',
            data,
            method: 'post'
        })
    }
    //endregion

//region 撮合交易=> 获取用户历史委托(成交记录)列表
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
//public void GetCouplingOrdersPageList(string InJson)
export const GetCouplingOrdersPageList = (data) => {
        return axios.request({
            url: '/GetCouplingOrdersPageList',
            data,
            method: 'post'
        })
    }
    //endregion

//region 撮合交易 => 撤回委托订单
//[WebMethod(Description = @" 撮合交易 => 撤回委托订单[可对接]</br>
//UserId:用户编号</br>
//DelegateOrdersId:委托订单主键<br/>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
//public void CancelDelegateOrders(string InJson)
export const CancelDelegateOrders = (data) => {
        return axios.request({
            url: '/CancelDelegateOrders',
            data,
            method: 'post'
        })
    }
    //endregion

//region  => 获取账户针对Symbol的详情信息
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
//public void GetCombinationSymbolsEntity(string InJson)
export const GetCombinationSymbolsEntity = (data) => {
    return axios.request({
        url: '/GetCombinationSymbolsEntity',
        data,
        method: 'post'
    })
}

//region 合约交易

//region 合约交易 => 获取合约账户划入、划出页面可用余额
//[WebMethod(Description = @"合约交易 => 获取合约账户划入、划出页面可用余额(输入参数string InJson)</br>
//UserId:用户ID</br>     
//SymbolsId:交易对主键编号<br/>        
//Direction:操作方向(Into-划入页面可用余额、DrawOut-划出页面可用余额)</br>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
//public void GetTransferBalanceByDirection(string InJson)
export const GetTransferBalanceByDirection = (data) => {
        return axios.request({
            url: '/GetTransferBalanceByDirection',
            data,
            method: 'post'
        })
    }
    //endregion

//region 合约交易 =>提交合约账户划入、划出
//[WebMethod(Description = @"合约交易 => 提交合约账户划入、划出(输入参数string InJson)</br>
//UserId:用户ID</br>     
//SymbolsId:交易对主键编号<br/>        
//Direction:操作方向(Into-币币账户划入到合约账户、DrawOut-合约账户划出到币币账户)</br>
//Quantity：数量<br/>
//Password:支付密码<br/>
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
//public void SubmitFuturesTransfer(string InJson)
export const SubmitFuturesTransfer = (data) => {
        return axios.request({
            url: '/SubmitFuturesTransfer',
            data,
            method: 'post'
        })
    }
    //endregion

//region 合约交易 => 获取我的合约资产列表
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
//public void GetUserFuturesAccountsList(string InJson)
export const GetUserFuturesAccountsList = (data) => {
    return axios.request({
        url: '/GetUserFuturesAccountsList',
        data,
        method: 'post'
    })
}

//endregion

//region 合约交易 => 获取我的单个合约资产
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
//public void GetUserFuturesAccountsEntity(string InJson)
export const GetUserFuturesAccountsEntity = (data) => {
    return axios.request({
        url: '/GetUserFuturesAccountsEntity',
        data,
        method: 'post'
    })
}

//endregion

//region 合约交易 => 用户提交合约开仓
//[WebMethod(Description = @"合约交易 => 用户提交合约开仓(输入参数string InJson)</br>
//UserId:用户ID</br>
//SymbolsId:交易对主键编号<br/>        
//Flag:委托方向(0-开多、1-开空)<br/>        
//LeverageMultiple:杠杆倍数(3-50倍证数)</br>
//OpenType:开仓类型(1-全仓、2-半仓)<br/>
//Password:支付密码<br/> 
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
//public void SubmitOpenFutures(string InJson)
export const SubmitOpenFutures = (data) => {
    return axios.request({
        url: '/SubmitOpenFutures',
        data,
        method: 'post'
    })
}

//region 合约交易 => 获取我的单个合约的持仓信息
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
//public void GetUserPositionEntity(string InJson)
export const GetUserPositionEntity = (data) => {
        return axios.request({
            url: '/GetUserPositionEntity',
            data,
            method: 'post'
        })
    }
    //region 合约交易=> 获取用户合约历史记录
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
    //public void GetUserFuturesHistoryList(string InJson)
export const GetUserFuturesHistoryList = (data) => {
    return axios.request({
        url: '/GetUserFuturesHistoryList',
        data,
        method: 'post'
    })
}

//region 合约交易 => 提交平仓
//[WebMethod(Description = @"合约交易 => 提交平仓(输入参数string InJson)</br>
//UserId:用户ID</br>
//OrderId:持仓订单主键<br/>        
//LanguageVersion: 语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文) <br/>")]
// [WebMethod(Description = "")]
//public void SubmitCloseFutures(string InJson)
export const SubmitCloseFutures = (data) => {
    return axios.request({
        url: '/SubmitCloseFutures',
        data,
        method: 'post'
    })
}
export const GetMiningMachineList = (data) => {
    return axios.request({
        url: '/GetMiningMachineList',
        data,
        method: 'post'
    })
}
export const GetMiningMachineInfo = (data) => {
    return axios.request({
        url: '/GetMiningMachineInfo',
        data,
        method: 'post'
    })
}
export const BuyMiner = (data) => {
    return axios.request({
        url: '/BuyMiner',
        data,
        method: 'post'
    })
}
export const GetMyMiningMachineOrder = (data) => {
    return axios.request({
        url: '/GetMyMiningMachineOrder',
        data,
        method: 'post'
    })
}
export const GetFinancialManagementPageData = (data) => {
    return axios.request({
        url: '/GetFinancialManagementPageData',
        data,
        method: 'post'
    })
}
export const PostTask = (data) => {
    return axios.request({
        url: '/PostTask',
        data,
        method: 'post'
    })
}
export const GetOrderList = (data) => {
    return axios.request({
        url: '/GetOrderList',
        data,
        method: 'post'
    })
}
export const GetMineralProductionRecords = (data) => {
    return axios.request({
        url: '/GetMineralProductionRecords',
        data,
        method: 'post'
    })
}
export const EndOrderTask = (data) => {
    return axios.request({
        url: '/EndOrderTask',
        data,
        method: 'post'
    })
}
export const GetQuicklyBuyCoinsUrl = (data) => {
        return axios.request({
            url: '/GetQuicklyBuyCoinsUrl',
            data,
            method: 'post'
        })
    }
    // 竞猜=》获取竞猜界面初始化数据
export const GetGamePageData = (data) => {
    return axios.request({
        url: '/GetGamePageDataV2',
        data,
        method: 'post'
    })
}
export const GetCurrentProjectData = (data) => {
    return axios.request({
        url: '/GetCurrentProjectDataV2',
        data,
        method: 'post'
    })
}
export const GetWinningBroadcastList = (data) => {
    return axios.request({
        url: '/GetWinningBroadcastListV2',
        data,
        method: 'post'
    })
}
export const GameBetting = (data) => {
        return axios.request({
            url: '/GameBetting',
            data,
            method: 'post'
        })
    }
    // 竞猜 => 获取开奖记录
    // 接口名： GetGameLotteryRecord
export const GetGameLotteryRecord = (data) => {
        return axios.request({
            url: '/GetGameLotteryRecordV2',
            data,
            method: 'post'
        })
    }
    // 竞猜 => 下注记录
    // 接口名： GameBettingRecord
export const GameBettingRecord = (data) => {
        return axios.request({
            url: '/GameBettingRecordV2',
            data,
            method: 'post'
        })
    }
    // 竞猜 => 快速交割
    // 接口名： FastDelivery
export const FastDelivery = (data) => {
        return axios.request({
            url: '/FastDelivery',
            data,
            method: 'post'
        })
    }
    // 获取盲盒界面数据
export const GetBlindBoxPageData = (data) => {
        return axios.request({
            url: '/GetBlindBoxPageData',
            data,
            method: 'post'
        })
    }
    //  获取盲盒抽奖广播列表
export const GetBlindboxWinningList = (data) => {
        return axios.request({
            url: '/GetBlindboxWinningList',
            data,
            method: 'post'
        })
    }
    // 开盲盒
export const OpenBlindBox = (data) => {
        return axios.request({
            url: '/OpenBlindBox',
            data,
            method: 'post'
        })
    }
    // 获取盲盒抽奖记录
export const GameBlindboxWinningRecord = (data) => {
        return axios.request({
            url: '/GameBlindboxWinningRecord',
            data,
            method: 'post'
        })
    }
    // 获取盲盒兑换界面数据
export const GetBlindboxExchangePage = (data) => {
        return axios.request({
            url: '/GetBlindboxExchangePage',
            data,
            method: 'post'
        })
    }
    // 盲盒兑换券兑换
export const BlindBoxExchange = (data) => {
    return axios.request({
        url: '/BlindBoxExchange',
        data,
        method: 'post'
    })
}

// 1===》 获取最新的通知公告(首页弹窗)
export const GetLatestNotice = (data) => {
        return axios.request({
            url: '/GetLatestNotice',
            data,
            method: 'post'
        })
    }
    // 接口名：GetLatestNotice
    // 请求参数：
    // UserId:用户token
    // LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
    // 返回字段说明：
    // NewsId：公告主键
    // NewsContent：公告内容(html编码后的富文本)
    // H5Url:H5链接


// 2===》获取资讯列表
// 接口名：GetInformationPageListV2
// 请求参数：
// PageSize:每页数
// PageIndex:当前页
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯) 
// UserId:用户编号(默认传default，若已登录则传用户token)
// 返回字段说明：
// NewsId：资讯主键
// Category:所属类别主键
// CreateDate：创建时间
// Description:简介
// FullHead：标题
// FilePath：标题图
// ReleaseTime:置顶时间
// SortCode:是否置顶（等于1视为置顶）
// IsRead:是否已读（1-已读、0-未读）


// 3===》标识已读资讯、标识已读通知公告
// 接口名：UpdateUserEntity
// 请求参数：
// UserId:用户编号
// Action:修改信息:标识已读资讯-NewsRead、标识已读通知公告-NoticeRead
// ActionValue:值(标识已读资讯==>传资讯主键、标识已读通知公告==>传公告主键）
// LanguageVersion:语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)

// 4==》获取symbol的详情信息
// 接口名：GetSymbolsEntity
// 请求参数：
// SymbolsId:主键编号
// LanguageVersion: 语言版本(zh-CN：西班牙，zh-hk：葡萄牙，en：英文，jpn：阿拉伯)
// 返回数据说明：
// SymbolsId:主键
// Symbol：交易对
// BaseCurrency：基础币种
// QuoteCurrency：计价币种
// Amount：24小时成交量
// OpenPrice：开盘价
// ClosePrice：当前成交价
// HighPrice：最高价
// LowPrice：最低价
// Vol：24小时成交额
// EnabledMark：是否有效
// SortCode：排序号
// CreateDate：创建时间
// GainRate：涨幅比例((现价-开盘价)/开盘价),此值自行换算成百分比形式
// FilePath:生成K线的参数
// RisingValue：涨幅值
// Unit:是否撮合交易（0-不支持撮合交易、1-支持撮合交易，不支持时行情也详情下面不出现买入卖出按钮）
// IsC2C:是否主流币（0-不是、1-是）
// Buy1Price:当前价换算成美元的值
// -----------------------以下字段是本次多返回的字段-------------------
// Icon：虚拟币图标
// FullName：虚拟币全称
// IssueDate：发行时间(字符串，直接显示)
// TotalAmount：发行总量(字符串，直接显示)
// InCirculation：流通总量(字符串，直接显示)
// WhitePaper：白皮书(如果为空或者空字符串就不显示复制图标)
// OfficialWebsite：官网(如果为空或者空字符串就不显示复制图标)
// Introduction：简介内容(字符串，直接显示)