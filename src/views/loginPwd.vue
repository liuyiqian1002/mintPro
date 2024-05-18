<template>
<div class="login_pwd_wrap_main">
<van-nav-bar :title="$t('loginPwd.title')" left-text fixed  left-arrow @click-left="$router.go(-1)" />
  <div class="login_pwd_wrap">
    <div class="contnet">
        <p>{{$t('loginPwd.oldPwsLabel')}}</p>
       <ul>
           <li><input type="password" v-model="OldPassWord" :placeholder="$t('loginPwd.oldPws')"></li>
       </ul>
       <p>{{$t('loginPwd.newPwsLabel')}}</p>
       <ul>
           <li><input type="password" v-model="NewPassWord" :placeholder="$t('loginPwd.newPws')"></li>
       </ul>
       <p>{{$t('loginPwd.newPwsLabel')}}</p>
       <ul>
           <li><input type="password" v-model="NewPassWord1" :placeholder="$t('loginPwd.newPws1')"></li>
       </ul>
    </div>
    <p class="tips">{{$t('loginPwd.passwordTips')}}</p>
    <div class="btns">
        <button class="main_btn" @click="updatePassword">{{$t('loginPwd.btn')}}</button>
    </div>
  </div>

</div>
</template>

<script>
import {UpdatePassword} from "@/api"
import {IsEmail,isPwd } from "@/libs/util"
import {
  Icon,
  NavBar,
  IndexBar,
} from "vant";
export default {
    name:'receivingQRCode',
    components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
    },
    data(){
        return{
            OldPassWord:'',
            NewPassWord:"",
            NewPassWord1:"",
            MINT_auth:{}

        }
    },
    methods:{
        updatePassword(){
            let data = {
                UserId :this.MINT_auth.UserId,
                OldPassWord:this.OldPassWord,
                NewPassWord:this.NewPassWord
            };
            if(!this.OldPassWord){
                this.$toast(this.$t('loginPwd.oldPws'))
                return;
                
            }
            if(!this.NewPassWord){
                this.$toast(this.$t('loginPwd.newPws'))
                return;
                
            }
            if(!isPwd(this.OldPassWord) ||!isPwd(this.NewPassWord)  ){
                this.$toast(this.$t('register.passwordErr'));
            return 
            }
            if(this.NewPassWord != this.NewPassWord1){
                this.$toast(this.$t('register.passwordErr1'));
            return 
            }
            UpdatePassword(data).then(r=>{
                if(r.data.Code == 200 ){
                    this.$toast(r.data.Message);
                    setTimeout(_=>{
                        localStorage.removeItem('MINT_auth')
                        localStorage.removeItem('userInfo')
                        this.$router.push('/')
                    },1000)
                }
            })
        }
        // //region [3]用户 => 修改登录密码
//[WebMethod(Description = @"[3]用户 => 修改登录密码(输入参数string InJson)</br>
//UserId:用户编号</br>
//OldPassWord:旧密码</br>
//NewPassWord:新密码</br>
//LanguageVersion:语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
// //public void UpdatePassword(string InJson)
    },
    mounted(){
         let MINT_auth = localStorage.getItem("MINT_auth");
    this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
        document.body.style.background='#f5f5f5';
    },
    destroyed(){
          document.body.style.background='#fff';
    }

}
</script>
<style lang="less" >
    .login_pwd_wrap_main{
        background-color: @background-color;
        .van-hairline--top-bottom,
        .van-hairline--bottom{
            &::after{
                content: '';
                border-bottom-width:0px;
                border: none !important;
            }
        }
    }

</style>
<style lang="less" scoped>
    .login_pwd_wrap{
        height: 100vh;
        padding-top: 46PX;
        .contnet{
            padding: 20px;
            >p{
                color:@font_f_color ;
                margin: 20px 0;
            }
            ul{
                display: flex;
                // height: 80px;
                align-items: center;
                justify-content: space-between;
                position: relative;
                padding:0 30px;
                width: 100%;
                background-color: @panel_color;
                border-radius: 20px;
                margin-bottom: 20px;
                &:not(:last-of-type){
                    .bot_br;
                }
                li{
                    height: 80px;
                    &:nth-of-type(1){
                        width: 100%;
                    }
                    input{
                        height: 100%;
                        width: 100%;
                        font-size: 26px;
                        color: @font_1_color;
                        background-color:transparent;
                    }
                    .vcode_bnt{
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        border: 1px solid @main_color;
                        transform: translate(0,-50%,);
                        background: transparent;
                        color: @main_color;
                        padding:5px 10px;
                        width: 160px;
                        &:disabled{
                            color:  @minor-font-color;;
                            border: 1px solid @minor-font-color;;
                        }
                    }
                }
            }
        }
        .tips{
            padding: 20px;
            height: 50px;
            line-height: 50px;
            font-size: 28px;
            color: @sub-font-color;
        }
        .btns{
            margin-top: 50px;
            padding: 20px;
            button {
                background-color: @main_color;
            }
        }
    }
</style>