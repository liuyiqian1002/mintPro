<template>
  <div class="register_wrap">
    <!-- :title="$t('login.title')" -->
    <van-nav-bar
      :right-text="$t('login.title')"
      left-arrow
      @click-left="$router.go(-1)"
      @click-right="$router.push('/login')"
      :placeholder="true"
      fixed
    >
      <template #right>
        <span class="cf">{{ $t("login.title") }}</span>
      </template>
    </van-nav-bar>
    <downLoad @closeDownLoad="closeDownLoad" v-show="showDownLoad" />
    <div :class="['logo', , { show_download: showDownLoad }]">
      <!-- <img src="../assets/images/common/logo.png" alt=""> -->
      <p class="welcome">{{ $t("register.title") }}</p>
    </div>
    <!-- <div class="nav">
          <div @click="navIndex = index" :class="{'active':navIndex==index}"  v-for="(i ,index ) in navList">
              {{i}}
          </div>
      </div> -->
    <div class="nav" v-show="navIndex == 0">
      <div @click="navIndex = 0" :class="{ active: navIndex == 0 }">
        {{ navList[0] }}
      </div>
      <div @click="navIndex = 1" :class="{ active: navIndex == 1 }">
        {{ navList[1] }}
      </div>
    </div>

    <div class="nav" v-show="navIndex == 1">
      <div @click="navIndex = 1" :class="{ active: navIndex == 1 }">
        {{ navList[1] }}
      </div>
      <div @click="navIndex = 0" :class="{ active: navIndex == 0 }">
        {{ navList[0] }}
      </div>
    </div>
    <div class="txt_box">
      <div class="item phone" v-show="navIndex == 0">
        <span @click="showSelPhoneAre = true">+{{ PhoneAre }}</span
        ><input
          v-model="Phone"
          :placeholder="$t('login.phone')"
          ref="Phone"
        /><b v-show="Phone" @click="delTxt('Phone')"></b>
      </div>
      <div class="item" v-show="navIndex == 1">
        <input
          type="email"
          v-model="Email"
          :placeholder="$t('register.email')"
          ref="Email"
        /><b v-show="Email" @click="delTxt('Email')"></b>
      </div>
      <div class="item vcode">
        <input
          type="text"
          v-model="Code"
          :placeholder="$t('register.vcode')"
        /><button :disabled="!isGetCode" class="vcode_bnt" @click="getCode">
          {{ btnTips }}
        </button>
      </div>
      <div class="item pwd">
        <input
          type="password"
          v-model="Password"
          :placeholder="$t('register.password')"
          ref="Password"
        /><b v-show="Password" @click="delTxt('Password')"></b>
      </div>
      <div class="item pwd">
        <input
          type="password"
          :placeholder="$t('register.password1')"
          v-model="Password1"
          ref="Password1"
        /><b v-show="Password1" @click="delTxt('Password1')"></b>
      </div>
      <div class="item initcode">
        <input
          type="text"
          :readonly="isShare"
          :placeholder="$t('register.recommend')"
          v-model="RecommendEnCode"
          ref="RecommendEnCode"
        /><b
          v-show="RecommendEnCode && !isShare"
          @click="delTxt('RecommendEnCode')"
        ></b>
      </div>
    </div>
    <div class="bot">
      <div class="btns">
        <button class="main_btn" :disabled="!isReg" @click="sign">
          {{ $t("register.btn") }}
        </button>
        <div class="agree">
          <label @click="checked = !checked">
            <b :class="{ active: checked }"> </b> {{ $t("register.agree") }}
          </label>
          <p @click="toH5">{{ $t("register.agreement") }}</p>
        </div>
        <!-- <div class="login">
                {{$t('register.hasAcc')}} <span @click="$router.push('/login')">{{$t('register.loginNow')}} </span>
            </div> -->
      </div>
    </div>
    <div class="tips">
      <p class="safety_tips">
        <img src="../assets/images/common/safety_tips.png" alt="" />
        {{ $t("base.safetyTips") }}
      </p>
    </div>
    <van-popup v-model="showSelPhoneAre" position="bottom" round>
      <van-picker
        show-toolbar
        :columns="phoneAreList"
        @cancel="showSelPhoneAre = false"
        @confirm="onConfirm"
      />
    </van-popup>
  </div>
</template>

<script>
import {
  Icon,
  Image,
  List,
  CountDown,
  Popup,
  NavBar,
  IndexBar,
  IndexAnchor,
  Cell,
  Empty,
  DropdownMenu,
  Sticky,
  DropdownItem,
  Field,
  Picker,
} from "vant";
import { Registered, GetMobileCode, GetMobileAreaCodeList } from "@/api";
import downLoad from "@/components/downLoad";
import { IsEmail, isPwd } from "@/libs/util";
import { Locale } from "vant";
import enUS from "@/assets/i18n/lang/en-US";
import jaJP from "@/assets/i18n/lang/ar-sa";
import zhHK from "@/assets/i18n/lang/es-POR";
import zhCN from "@/assets/i18n/lang/es-ES";
export default {
  name: "forget",
  components: {
    [Sticky.name]: Sticky,
    [Icon.name]: Icon,
    [List.name]: List,
    [CountDown.name]: CountDown,
    [Popup.name]: Popup,
    [NavBar.name]: NavBar,
    [IndexBar.name]: IndexBar,
    [IndexAnchor.name]: IndexAnchor,
    [Cell.name]: Cell,
    [Empty.name]: Empty,
    [DropdownMenu.name]: DropdownMenu,
    [DropdownItem.name]: DropdownItem,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    downLoad,
  },
  data() {
    return {
      checked: true,
      isReg: true,
      Account: "",
      Password: "",
      Password1: "",
      RecommendEnCode: "", //邀请码（选填）</br>
      Code: "", //验证码<br/>
      isGetCode: true,
      btnTips: "",
      navList: [],
      navIndex: 0,
      showSelPhoneAre: false, //区号选择
      phoneAreList: [], //区号
      PhoneAre: "1",
      Email: "",
      Phone: "",
      isShare: false,
      showDownLoad: false,
    };
  },
  created() {
    let lang = this.$route.query.lang;
    if (lang) {
      localStorage.setItem("MINT_lang", lang);
    } else {
      lang = localStorage.getItem("MINT_lang")
        ? localStorage.getItem("MINT_lang")
        : "en";
      // localStorage.setItem('MINT_lang','en');
      // lang = 'en'
    }
    this.$i18n.locale = lang;
    if (lang == "zh-CN") {
      Locale.use("zh-CN", zhCN);
    } else if (lang == "zh-hk") {
      Locale.use("zh-HK", zhHK);
    } else if (lang == "jpn") {
      Locale.use("ja-JP", jaJP);
    } else {
      Locale.use("en-US", enUS);
    }
  },
  mounted() {
    this.RecommendEnCode = this.$route.query.inviteCode;

    if (this.$route.query.inviteCode) {
      this.isShare = true;
    }

    this.btnTips = this.$t("forgetPwd.btnGetCode");
    this.selPhoneAre();
    this.navList = [this.$t("login.navPhone"), this.$t("login.navEmail")];
    this.showDownLoadPop();
  },
  methods: {
    onConfirm(value, index) {
      this.PhoneAre = value.value;
      this.showSelPhoneAre = false;
    },
    //获取手机区号列表
    selPhoneAre() {
      GetMobileAreaCodeList({}).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
          let arr = [];
          r.data.Data.map((_) => {
            arr.push({
              value: _.itemvalue,
              text: `+${_.itemvalue}   ${_.description}`,
            });
          });
          this.phoneAreList = arr;
        }
      });
    },
    getCode() {
      //[WebMethod(Description = @"[88]系统 =>发送验证码(输入参数string Json)</br>
      //Mobile:邮箱</br>
      //Title:发送类别(用户注册、密码找回、交易密码、转账、提币)</br>
      let data = {
        Title: "用户注册",
        Mobile:
          this.navIndex == 1
            ? this.Email.trim()
            : (this.PhoneAre + "|" + this.Phone).trim(),
      };
      if (this.navIndex == 1) {
        if (!IsEmail(this.Email.trim())) {
          this.$toast(this.$t("register.emailErr"));
          return;
        }
      } else {
        if (!this.Phone) {
          this.$toast(this.$t("login.phone"));
          return;
        }
      }
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      this.isGetCode = false;
      GetMobileCode(data).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
          this.$toast(this.$t("register.sendSeccess"));
          let wait = 60;
          let time = setInterval((_) => {
            wait--;
            if (wait == 0) {
              clearInterval(time);
              this.btnTips = this.$t("forgetPwd.btnGetCode");
              this.isGetCode = true;
            } else {
              this.btnTips = wait + "S";
            }
          }, 1000);
        } else {
          this.isGetCode = true;
        }
      });
    },
    sign() {
      let data = {
        RegisterMark: this.navIndex == 1 ? "Email" : "Mobile",
        RegisterType: "Web",
        Account: this.navIndex == 1 ? this.Email : this.Phone,
        Password: this.Password,
        OpenId: this.navIndex == 1 ? "Default" : this.PhoneAre, //第三方快捷返回值说明：不是快捷注册传default)</br>
        RecommendEnCode: this.RecommendEnCode
          ? this.RecommendEnCode
          : "Default", //this.RecommendEnCode,//邀请码（选填）</br>
        Code: this.Code, //验证码<br/>
      };
      if (!this.Code) {
        this.$toast(this.$t("register.codeErr"));
        return;
      }
      if (this.navIndex == 1) {
        if (!IsEmail(this.Email.trim())) {
          this.$toast(this.$t("register.emailErr"));
          return;
        }
      }
      if (!isPwd(this.Password)) {
        this.$toast(this.$t("register.passwordErr"));
        return;
      }
      if (this.Password != this.Password1) {
        this.$toast(this.$t("register.passwordErr1"));
        return;
      }
      if (!this.checked) {
        this.$toast(this.$t("register.isAgree"));
        return;
      }
      this.isReg = false;
      console.log(data);
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      Registered(data).then((r) => {
        this.isReg = true;
        console.log(r);
        const { data } = r;

        if (data.Code == 200) {
          let that = this;
          this.$toast({
            message: this.$t("register.secces"),
            onClose() {
              that.$router.push("/login");
            },
          });
        }
      });
    },
    // 清除
    delTxt(val) {
      this[val] = "";
      this.$refs[val].focus();
    },
    toH5() {
      let lang = localStorage.getItem("MINT_lang");
      let url =
        this.$config.imgPath +
        `/News/AppSynopsis/8f14ea47-5fce-4741-aee8-4d85eeb7a69c?LanguageVersion=${lang}`;
      this.$router.push({
        path: "/H5",
        query: {
          url: url,
          title: this.$t("register.agreement"),
        },
      });
    },
    showDownLoadPop() {
      let downloadTime = localStorage.getItem("download");
      if (!downloadTime) {
        this.showDownLoad = true;
      } else {
        console.log(Date.now() - Number(downloadTime) > 1000 * 60 * 60 * 12);
        this.showDownLoad =
          Date.now() - Number(downloadTime) > 1000 * 60 * 60 * 12;
      }
    },
    closeDownLoad(e) {
      this.showDownLoad = false;
    },
  },
};
</script>
<style lang="less" >
.register_wrap {
  .van-nav-bar .van-nav-bar__left .van-icon {
    color: #fff;
  }
  .van-hairline--bottom::after {
    border-bottom: unset;
  }
}
</style>
<style lang="less" scoped>
.register_wrap {
  background-color: @background-color;
  min-height: 100vh;
  padding-bottom: 120px;
  position: relative;
  color: @font_color;
  // .logo{
  //     margin-top: 60px;
  //     text-align: center;
  //     img{
  //         width:100px ;
  //     }
  // }
  .logo {
    padding: 0 48px;
    //    width: 80%;
    padding-top: 60px;
    &.show_download {
      padding-top: 160px;
    }
    text-align: left;
    .pic {
      img {
        border-radius: 20px;
        width: 158px;
      }
      padding-bottom: 40px;
    }
    .welcome {
      color: @font_color;
      font-size: 56px;
      font-family: PingFang SC;
      font-weight: 500;
    }
  }
  > .nav {
    padding: 0 48px;
    margin: 60px auto 0;
    display: flex;
    justify-content: space-between;
    > div {
      color: @font_color;
      font-size: 29px;
      padding: 10px 0;
      &.active {
        color: @font_text_main_color;
      }
    }
  }
  .txt_box {
    padding: 0 48px;
    // width: 80%;
    margin: 50px auto 0;
    .item {
      margin: 30px 0 0;
      height: 112px;
      line-height: 112px;
      position: relative;
      background-color: @panel_color;
      background-size: contain;
      border-radius: 8px;
      b {
        height: 112px;
        width: 70px;
        display: block;
        position: absolute;
        right: 0;
        top: 0;
        background: url(../assets/images/common/ic_edit_delete.png) no-repeat
          center;
        background-size: 40px 40px;
      }
      input {
        width: 100%;
        height: 70px;
        padding-left: 30px;
        font-size: 26px;
        background-color: @panel_color;
        background-color: transparent;
        &::-ms-input-placeholder {
          /* Internet Explorer 10-11 */
          .input-placeholder;
        }
        &::-webkit-input-placeholder {
            .input-placeholder;
        }
        &::-moz-placeholder {
          /* Mozilla Firefox 19+ */
          .input-placeholder;
        }
        &::-moz-placeholder {
          /* Mozilla Firefox 4 to 18 */
          .input-placeholder;
        }
      }
      &.email {
        input {
          background: url(../assets/images/common/ic_user.png) no-repeat;
          background-size: 30px;
        }
      }
      &.phone {
        display: flex;
        align-items: center;
        span {
          position: relative;
          width: 130px;
          text-align: center;
          margin-right: 10px;
          color: @font_color;
          font-size: 29px;
          &::before {
            content: "";
            width: 20px;
            height: 15px;
            background-color: @panel_color;
            background-size: contain;
            // border-left: 10px solid transparent;
            // border-right: 10px solid transparent;
            // border-top: 10px solid #fff;
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            margin: auto;
          }
        }
      }
      &.vcode {
        input {
          // background: url(../assets/images/common/ic_yzm.png) no-repeat  left center ;
          background-size: 30px;
          width: 100%;
          // white-space: nowrap;
          // overflow : hidden;
          // text-overflow: ellipsis;
          // display: -webkit-box;
          // -webkit-line-clamp: 1;
          // -webkit-box-orient: vertical;
        }
        .vcode_bnt {
          position: absolute;
          top: 50%;
          right: 10px;
          // border: 1px solid @main_color;
          transform: translate(0, -50%);
          background-color: @btn_main_color;
          border-radius: 8px;
          color: @font_3_color;
          font-size: 26px;
          padding: 5px 10px;
          width: 200px;
          &:disabled {
            color: @minor-font-color;
            border: 1px solid @minor-font-color;
          }
        }
      }
      &.pwd {
        input {
          // background: url(../assets/images/common/ic_pass.png) no-repeat  ;
          background-size: 30px;
          background-position: 0 8px;
        }
      }
    }
  }
  .bot {
    padding: 0 48px;
    margin: 40px auto 40px;
    .main_btn {
      height: 90px;
      font-size: 28px;
      background-color: @main2_color;
      color: @font_3_color;
    }
    .agree {
      margin-top: 10px;
    }
    .login {
      margin-top: 20px;
      text-align: center;
      span {
        color: @main_color;
      }
    }
  }
  .tips {
    position: absolute;
    bottom: 90px;
    left: 0;
    right: 0;
    padding: 0 48px;

    margin: 0px auto;

    font-size: 29px;
    span {
      font-size: 29px;
      font-weight: 500;
    }
    .safety_tips {
      // display: flex;
      // align-items: center;
      font-size: 24px;
      margin-top: 15px;
      color: #fff;
      img {
        width: 20px;
        margin-right: 5px;
      }
    }
  }
}
</style>