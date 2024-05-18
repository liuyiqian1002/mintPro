<template>
  <div class="bind_acc_wrap">
    <van-nav-bar
      :title="type=='mobile'?$t('bindAccount.phone'):$t('bindAccount.email')"
      left-text
      left-arrow
      @click-left="$router.push(redirect)"
    />
    <div class="content">
      <div v-show="type == 'email'">
        <label>{{$t('login.navEmail')}}</label>
        <div class="item">
          <input
            v-model="Email"
            :placeholder="$t('login.email')"
            ref="Email"
          /><b v-show="Phone" @click="delTxt('Email')"></b>
        </div>
      </div>
      <div v-show="type == 'mobile'">
        <label>{{$t('login.navPhone')}}</label>
        <div class="item phone">
          <span @click="showSelPhoneAre = true">+{{ PhoneAre }}</span
          ><input
            v-model="Phone"
            :placeholder="$t('login.phone')"
            ref="Phone"
          /><b v-show="Phone" @click="delTxt('Phone')"></b>
        </div>
      </div>
      <label>{{$t('withdraw.Code')}}</label>
      <div class="item vcode">
        <input
          type="text"
          v-model="Code"
          :placeholder="$t('register.vcode')"
        /><button :disabled="!isGetCode" class="vcode_bnt" @click="getCode">
          {{ btnTips }}
        </button>
      </div>
    </div>
    <div class="btns">
      <button class="main_btn" @click="userTransfer">
        {{ $t("base.confirm") }}
      </button>
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
import { Icon, NavBar, IndexBar, Sticky, Popup, Picker } from "vant";
import {
  GetMobileAreaCodeList,
  UpdateUserEntity,
  GetEntityUser,
  GetMobileCode,
} from "@/api";
import { IsEmail, isPwd, encryptByDES } from "@/libs/util";
export default {
  name: "bindAccount",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Popup.name]: Popup,
    [Picker.name]: Picker,
  },
  data() {
    return {
      type: "",
      Phone: "",
      Email: "",
      showSelPhoneAre: false, //区号选择
      phoneAreList: [], //区号
      navIndex: 0,
      PhoneAre: "1",
      Code: "",
      isGetCode: true,
      btnTips: "",
      MINT_auth: {},
      redirect:''
    };
  },
  mounted() {
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    console.log(this.MINT_auth);
    this.btnTips = this.$t("forgetPwd.btnGetCode");
    this.type = this.$route.query.type;
    this.redirect = this.$route.query.redirect?this.$route.query.redirect:"/safety";
    if (this.type == "mobile") {
      this.selPhoneAre();
    }
  },
  methods: {
    // 清除
    delTxt(val) {
      this[val] = "";
      this.$refs[val].focus();
    },
    getCode() {
      //[WebMethod(Description = @"[88]系统 =>发送验证码(输入参数string Json)</br>
      //Mobile:邮箱</br>
      //Title:发送类别(用户注册、密码找回、交易密码、转账、提币)</br>
      let data = {
        Title: "绑定账号",
        Mobile:
          this.type == "email"
            ? this.Email.trim()
            : (this.PhoneAre + "|" + this.Phone).trim(),
      };
      if (this.type == "email") {
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
      console.log(data);
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
    getEntityUser() {
      let data = {
        UserId: this.MINT_auth.UserId,
      };
      GetEntityUser(data).then((r) => {
        // console.log(JSON.stringify(r.data.Data) );
        if (r.data.Code == 200) {
          this.UserInfo = r.data.Data;
          const { UserGrade } = r.data.Data;
          console.log(this.UserInfo);
          this.$store.commit("setUserInfo", this.UserInfo);
          this.selIndex = this.UserInfo.VerifCodeAccount == "Mobile" ? 0 : 1;
        }
      });
    },
    userTransfer() {
      if (this.type == "email") {
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
      if (!this.Code) {
        this.$toast(this.$t("register.codeErr"));
        return;
      }
      // 邮箱#验证码
      // 区号|手机号#验证码
      let ActionValue =
        this.type == "email"
          ? `${this.Email}#${this.Code}`
          : `${this.PhoneAre}|${this.Phone}#${this.Code}`;
      let data = {
        UserId: this.MINT_auth.UserId, //用户编号</br>?
        Action: this.type == "email" ? "BindEmailAccount" : "BindMobileAccount", //修改信息:<br/>
        ActionValue: ActionValue,
      };
      console.log(data);
      UpdateUserEntity(data).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
          this.getEntityUser();
          this.$toast({
            message: r.data.Message,
          });
          this.$router.push({
            path: "/success",
            query: {
              type: "bind",
              redirect:this.redirect
            },
          });
          // console.log(r.Data)
        }
      });
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
    onConfirm(value, index) {
      this.PhoneAre = value.value;
      this.showSelPhoneAre = false;
    },
  },
};
</script>
<style lang="less" scoped>
  .bind_acc_wrap {
    .content {
      padding: 0 48px;
      // width: 80%;
      margin: 50px auto 0;
      label {
        color: #999999;
        font-size: 28px;
      }
      .item {
        margin: 30px 0;
        height: 112px;
        line-height: 112px;
        position: relative;
        //   background: url(../assets/images/common/inpup_bg.png) no-repeat;
        background-size: contain;
        background-color: #f5f6f7;
        border-radius: 16px;
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
          background-color: transparent;
          // &::-ms-input-placeholder {
          //   /* Internet Explorer 10-11 */
          //   .cf;
          // }
          // &::-webkit-input-placeholder {
          //   .cf;
          // }
          // &::-moz-placeholder {
          //   /* Mozilla Firefox 19+ */
          //   .cf;
          // }
          // &::-moz-placeholder {
          //   /* Mozilla Firefox 4 to 18 */
          //   .cf;
          // }
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
            color: #272e3f;
            font-size: 29px;
            &::before {
              content: "";
              width: 20px;
              height: 15px;
              background: url(../assets/images/common/drop1.png) no-repeat;
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
            //   border: 1px solid @main_color;
            transform: translate(0, -50%);
            background: #f5f6f7;
            color: @main_color;
            font-size: 26px;
            padding: 5px 10px;
            height: 100%;
            width: 200px;
            &:disabled {
              color: @minor-font-color;
              // border: 1px solid @minor-font-color;
            }
          }
        }
      }
    }
    .btns {
      margin-top: 50px;
      padding: 0 48px;
      text-align: center;
    }
  }
</style>