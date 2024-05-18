<template>
  <div class="forget_wrap">
      <!-- :title="$t('forgetPwd.title')"  -->
    <van-nav-bar left-text   left-arrow @click-left="$router.go(-1)" />
      <div class="logo">
          <!-- <img src="../assets/images/common/logo.png" alt=""> -->
          <p class="welcome">{{$t('forgetPwd.title')}}</p>
      </div>
       <div class="nav" v-show="navIndex ==0">
          <div @click="navIndex = 0" :class="{'active':navIndex==1}" >
              {{navList[0]}}
          </div>
          <div @click="navIndex = 1" :class="{'active':navIndex==0}" >
              {{navList[1]}}
          </div>
      </div>


      <div class="nav" v-show="navIndex ==1">
          <div @click="navIndex = 1" :class="{'active':navIndex==1}"  >
              {{navList[1]}}
          </div>
          <div @click="navIndex = 0" :class="{'active':navIndex==0}"  >
              {{navList[0]}}
          </div>
      </div>
      <div class="txt_box">
         <div class="item phone" v-show="navIndex==0">
            <span @click="showSelPhoneAre=true">+{{PhoneAre}}</span><input   v-model="Phone" :placeholder="$t('login.phone')" ref="Phone"><b v-show="Phone" @click="delTxt('Phone')"></b>
         </div>
         <div class="item " v-show="navIndex==1">
            <input type="email"  v-model="Email" :placeholder="$t('forgetPwd.email')" ref="Email"><b v-show="Email" @click="delTxt('Email')"></b>
         </div>
         <div class="item vcode">
           <input type="text" v-model="Code" :placeholder="$t('register.vcode')"><button :disabled='!isGetCode' class="vcode_bnt" @click="getCode">{{btnTips}}</button>
             <!-- <input type="text" :placeholder="$t('forgetPwd.vcode')"><button disabled class="vcode_bnt">{{$t("forgetPwd.btnGetCode")}}</button> -->
         </div>
         <div class="item pwd">
           <input type="password"  v-model="Password" :placeholder="$t('register.password')" ref="Password"><b v-show="Password" @click="delTxt('Password')"></b>
             <!-- <input type="password" :placeholder="$t('forgetPwd.password')"><b></b> -->
         </div>
         <div class="item pwd">
            <input type="password"  v-model="Password1" :placeholder="$t('register.password')" ref="Password1"><b v-show="Password1" @click="delTxt('Password1')"></b>
             <!-- <input type="password" :placeholder="$t('forgetPwd.password1')"><b></b> -->
         </div>
      </div>
      <div class="bot">
        <div class="btns">
            <button  class="main_btn" :disabled='!isReg'  @click="resetPwd">
                {{$t('forgetPwd.btn')}}
            </button>
        </div>
      </div>
       <div class="tips">
          <!-- <p>{{$t('login.hasAcc')}}？ <span class="mc"  @click="$router.push('/register')" >{{$t("login.Registernow")}}</span></p> -->
          <p class="safety_tips"><img src="../assets/images/common/safety_tips.png" alt=""> {{$t('base.safetyTips')}}</p>
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
  Picker
} from "vant";
import {ResetPassword,GetMobileCode,GetMobileAreaCodeList} from "@/api"
import {IsEmail,isPwd } from "@/libs/util"
export default {
    name:"forget",
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
    [Field.name]: Field,
     [Picker.name]: Picker,
    [Popup.name]: Popup,
  },
  data(){
      return {
        isReg:true,
        Account:'',
        Password:'',
        Password1:'',
        Code:'',//验证码<br/>
        isGetCode:true,
        btnTips:'',
        navList:[],
        navIndex:0,
        showSelPhoneAre:false,//区号选择
        phoneAreList:[],//区号
        PhoneAre:'1',
        Phone:"",
        Email:"",

      }
  },
   mounted(){
           this.navList=[this.$t('login.navPhone'),this.$t("login.navEmail")];
      this.btnTips = this.$t("forgetPwd.btnGetCode");
      this.selPhoneAre();
  },
   methods:{
       onConfirm(value, index) {
        this.PhoneAre=value.value;
        this.showSelPhoneAre = false;
        },
             //获取手机区号列表
    selPhoneAre(){
        GetMobileAreaCodeList({}).then(r=>{
            console.log(r);
            if(r.data.Code==200){
                let arr = [];
                r.data.Data.map(_=>{
                    arr.push({value:_.itemvalue,text:`+${_.itemvalue}   ${_.description}`})
                      
                })
                this.phoneAreList =arr
            }
        })
    },
      getCode(){
          //[WebMethod(Description = @"[88]系统 =>发送验证码(输入参数string Json)</br>
        //Mobile:邮箱</br>
        //Title:发送类别(用户注册、密码找回、交易密码、转账、提币)</br>
        let data ={
            Title:'密码找回',
            Mobile:this.navIndex==1?this.Email.trim():(this.PhoneAre+"|"+this.Phone).trim()
        };
        if(this.navIndex==1){
            if(!IsEmail(this.Email.trim())){
                    this.$toast(this.$t('register.emailErr'));
                return 
                }
        }else{
            if(!this.Phone){
                this.$toast(this.$t('login.phone'));
            return 
            }
        }
        this.$toast.loading({
            duration: 0, // 持续展示 toast
            forbidClick: true,
        });
        this.isGetCode = false;
        GetMobileCode(data).then(r=>{
            console.log(r);
            if(r.data.Code == 200 ){
               this.$toast( this.$t('register.sendSeccess'));
                let wait = 60;
                let time = setInterval(_=>{
                    wait --;
                    if(wait==0){
                        clearInterval(time);
                        this.btnTips= this.$t("forgetPwd.btnGetCode");
                        this.isGetCode = true;
                    }else{

                        this.btnTips=wait+'S';
                    }
                    
                },1000)
            }else{
                this.isGetCode = true;
            }
        })
      },
        resetPwd(){
            // Account:登录账户(邮箱、手机号(区号|手机号))
            let data = {
                Account:this.navIndex==1?this.Email:(this.PhoneAre+'|'+this.Phone),
                NewPassword:this.Password,
                Code:this.Code//验证码<br/>
            };
            if(!this.Code){
                this.$toast(this.$t('register.codeErr'));
                return;
            }
            if(this.navIndex==1){
                if(!IsEmail(this.Email.trim())){
                    this.$toast(this.$t('register.emailErr'));
                return 
                }
            }
            if(!isPwd(this.Password)){
                this.$toast(this.$t('register.passwordErr'));
            return 
            }
            if(this.Password != this.Password1){
                this.$toast(this.$t('register.passwordErr1'));
            return 
            }
            this.isReg = false;
            console.log(data);
            this.$toast.loading({
                duration: 0, // 持续展示 toast
                forbidClick: true,
                message: this.$t('register.loading'),
            });
            ResetPassword(data).then(r=>{
                this.isReg = true
                console.log(r)
                const{data} = r;

                if(data.Code == 200){
                    let that = this;
                     this.$toast({
                        message: this.$t('forgetPwd.secces'),
                        onClose(){
                            that.$router.push("/login")
                        }
                     })
                }

            })
        },
        // 清除
        delTxt(val) {
            this[val] = '';
            this.$refs[val].focus();
        },
  }
}
</script>

<style lang="less" >
    .forget_wrap{
        .van-nav-bar{
            background: transparent;
        }
        .van-nav-bar .van-nav-bar__left .van-icon{
            color:#fff;
        }
        .van-hairline--bottom::after{
            border-bottom: unset;
        }

    }

</style>
<style lang="less" scoped>

    .forget_wrap{
        height: 100vh;
        color: #fff;
        position: relative;
        // .logo{
        //     margin-top: 60px;
        //     text-align: center;
        //     img{
        //         width:100px ;
        //     }
        // }
        .logo{
            //    width: 80%;
            //    margin: 100px auto 0;
            text-align: left;
            .pic{

                img{
                    border-radius: 20px;
                    width:158px ;
                }
                padding-bottom: 40px;
            }
            .welcome{
                padding:0 48px;
                padding-top: 98px;
                font-size: 56px;
                font-family: PingFang SC;
                font-weight: 500;
            }
        }
        > .nav{
            padding: 0 48px;
            margin: 60px auto 0;
            display: flex;
            justify-content: space-between;
            >div{
                color: #FFFFFF;
                font-size: 29px;
                padding: 10px 0 ;
                &.active{
                    color: rgba(147, 161, 168, 1);
                }
            }
        }
        .txt_box{
            padding:0 48px;
            // width: 80%;
            margin: 50px auto 0;
            .item{
                margin: 30px 0 0;
                height: 112px;
                line-height: 112px;
                position: relative;
                border-radius: 8px;
                background-color: @panel_color;
                // background: url(../assets/images/common/inpup_bg.png) no-repeat;
                background-size: contain;
                b{
                    height: 112px;
                    width: 70px;
                    display: block;
                    position: absolute;
                    right: 0;
                    top: 0;
                    background: url(../assets/images/common/ic_edit_delete.png) no-repeat  center ;
                    background-size: 40px 40px;

                }
                input{
                    width: 100%;
                    height: 70px;
                    padding-left: 30px;
                    font-size: 26px;
                    background-color:transparent;
                    &::-ms-input-placeholder {
                        /* Internet Explorer 10-11 */
                        .cf;
                    }
                    &::-webkit-input-placeholder {
                        .cf;
                    }
                    &::-moz-placeholder {
                        /* Mozilla Firefox 19+ */
                        .cf;
                    }
                    &::-moz-placeholder {
                        /* Mozilla Firefox 4 to 18 */
                        .cf;
                    }
                }
                &.email{
                    input{
                        background-color: @panel_color;
                        background-size:30px ;
                    }
                }
                &.phone{
                    display: flex;
                    align-items: center;
                    span{
                        position: relative;
                        width: 130px;
                        text-align: center;
                        margin-right: 10px;
                        color: #fff;
                        font-size: 29px;
                        &::before{
                            content: '';
                            width: 20px;
                            height: 15px;
                            background:url(../assets/images/common/drop.png) no-repeat;
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
                &.vcode{
                    input{
                        // background: url(../assets/images/common/ic_yzm.png) no-repeat  left center ;
                        background-size:30px ;
                        width: 100%;
                        // white-space: nowrap;
                        // overflow : hidden;
                        // text-overflow: ellipsis;
                        // display: -webkit-box;
                        // -webkit-line-clamp: 1;
                        // -webkit-box-orient: vertical;
                    }
                    .vcode_bnt{
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        // border: 1px solid @main_color;
                        background-color: @background-color;
                        color: #fff;
                        font-size: 26px;
                        padding:5px 10px;
                        width: 200px;
                        &:disabled{
                            color:  @minor-font-color;;
                            border: 1px solid @minor-font-color;;
                        }
                    }
                }
                &.pwd{
                    input{
                        // background: url(../assets/images/common/ic_pass.png) no-repeat  ;
                        background-size:30px ;
                        background-position: 0 8px ;
                    }
                }
            }
        }
        // .txt_box{
        //     width: 80%;
        //     margin: 50px auto 0;
        //     .item{
        //         display: flex;
        //         margin: 10px 0 0;
        //         height: 80px;
        //         line-height: 80px;
        //         position: relative;
        //         .bot_br;
        //         align-items: center;
        //         label{
        //             width: 120px;
        //             padding: 0 15px;
        //             text-align: center;
        //              text-align-last: justify;
        //              position: relative;
        //             &::after{
        //                 content: '';
        //                 display: block;
        //                 height: 30px;
        //                 width: 2px;
        //                 background-color: #eee;
        //                 position: absolute;
        //                 right: 0;
        //                 top: 50%;
        //                 transform: translate(-50%,-50%);

        //             }
        //         }
        //         b{
        //             height: 70px;
        //             width: 70px;
        //             display: block;
        //             position: absolute;
        //             right: 0;
        //             top: 0;
        //             background: url(../assets/images/common/ic_edit_delete.png) no-repeat  center ;
        //             background-size: 40px 40px;

        //         }
        //         input{
        //             width: 80%;
        //             height: 70px;
        //             font-size: 26px;
        //             padding: 0 10px;
        //         }
        //         &.phone{
        //             label{
        //                 position: relative;
        //                 text-align: center;
        //                 color: #333333;
        //                 font-size: 29px;
        //                 &::before{
        //                     content: '';
        //                       width: 0;
        //                     height: 0;
        //                     border-left: 10px solid transparent;
        //                     border-right: 10px solid transparent;
        //                     border-top: 10px solid #333;
        //                     position: absolute;
        //                     top: 0;
        //                     right: 10px;
        //                     bottom: 0;
        //                     margin: auto;
        //                 }
        //             }
        //         }
        //         &.email{
        //             input{
        //                 // background: url(../assets/images/common/ic_user.png) no-repeat  ;
        //                 background-size:30px ;
        //             }
        //         }
        //         &.vcode{
        //             input{
        //                 // background: url(../assets/images/common/ic_yzm.png) no-repeat  left center ;
        //                 background-size:30px ;
        //                   width: 80%;
        //                 white-space: nowrap;
        //                 overflow : hidden;
        //                 text-overflow: ellipsis;
        //                 display: -webkit-box;
        //                 -webkit-line-clamp: 1;
        //                 -webkit-box-orient: vertical;
        //             }
        //           .vcode_bnt{
        //             position: absolute;
        //             top: 50%;
        //             right: 10px;
        //             // border: 1px solid @main_color;
        //             transform: translate(0,-50%,);
        //             background: transparent;
        //             color: @main_color;
        //             padding:5px 10px;
        //              width: 200px;
        //             &:disabled{
        //               color:  @minor-font-color;;
        //              border: 1px solid @minor-font-color;;
        //             }
        //           }
        //         }
        //         &.pwd{
        //             input{
        //                 // background: url(../assets/images/common/ic_pass.png) no-repeat  ;
        //                 background-size:30px ;
        //                 background-position: 0 8px ;
        //             }
        //         }
        //     }
        // }
        .bot{
            padding: 0 48px;
            margin: 80px auto;
            .main_btn{
                height: 90px;
                font-size: 28px;
                background-color: @main_color2;
                color: @font_3_color;//@main_color;
            }
            .nav{
                margin: 20px 0;
                justify-content: space-between;
                display: flex;
            }
        }
        .tips{
            width: 80%;
            margin: 80px auto;
            font-size: 29px;
            position: absolute;
            bottom: 90px;
            left: 0;
            right: 0;
            width: 80%;
            margin: 0px auto;
            span{
                font-size: 29px;
                font-weight: 500;
            }
            .safety_tips{
                // display: flex;
                // align-items: center;
                font-size: 24px;
                margin-top: 15px;
                color: @font_f_color;
                img{
                    width:20px;
                    margin-right: 5px;
                }
            }
        }
    }
</style>