<template>
<div class="transaction_pwd_wrap_main">
<van-nav-bar :title="$t('transactionPwd.title')" left-text fixed  left-arrow @click-left="$router.go(-1)" />
  <div class="transaction_pwd_wrap" @click="showKeyboard=false">
    <div class="contnet">
        <p>{{$t('loginPwd.vCodeLabel')}}</p>
       <ul>
           <li>
               <input type="text" v-model="Code" :placeholder="$t('transactionPwd.vcode')">
           </li>
           <li>
               <button class="vcode_bnt" :disabled='!isGetCode' @click="getCode">{{btnTips}}</button>
           </li>
       </ul>
       <p>{{$t('transactionPwd.title')}}</p>
       <ul>
           <li><input v-model="pwd" type="password" readonly :placeholder="$t('transactionPwd.password')" @click.stop="pwdInput('pwd')"></li>
       </ul>
       <p>{{$t('transactionPwd.title')}}</p>
       <ul>
           <li><input v-model="pwd1" type="password" readonly @click.stop="pwdInput('pwd1')" :placeholder="$t('transactionPwd.password1')"></li>
       </ul>
    </div>
    <div class="btns">
        <button class="main_btn" @click="upUserEntity">{{$t('transactionPwd.btn')}}</button>
    </div>
  </div>
    <van-number-keyboard
    :show="showKeyboard"
    @input="onInput"
    @delete="onDelete"
    @blur="showKeyboard = false"
    />
</div>
</template>

<script>
import Vue from 'vue';
import { PasswordInput, NumberKeyboard } from 'vant';

Vue.use(PasswordInput);
Vue.use(NumberKeyboard);
import {
  Icon,
  NavBar,
  IndexBar,
} from "vant";
import {GetMobileCode,UpdateUserEntity} from "@/api"
import { mapState } from "vuex"
import userInfoVue from './user/userInfo.vue';
export default {
    name:'receivingQRCode',
    components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
    },
    data(){
        return{
            showKeyboard:false,
            pwd:'',
            pwd1:'',
            txtNow:'',
            Code:'',
            isGetCode:true,
            btnTips:'',
            userInfo:{},
            isPay:true
        }
    },
    mounted(){
        document.body.style.background='#f5f5f5';
      this.btnTips = this.$t("forgetPwd.btnGetCode")
    //   console.log(this.base.userInfo.Email)
        let  userInfo = localStorage.getItem('userInfo')
        this.userInfo = JSON.parse(this.$decryptByDES(userInfo));
        console.log(this.userInfo )
    },
    methods: {
    getCode(){
        let Mobile =''
          if(this.userInfo.VerifCodeAccount){
                Mobile=  this.userInfo.VerifCodeAccount=="Email"?this.userInfo.Email:(this.userInfo.AreaCode+"|"+this.userInfo.Mobile)
                }else{
                    Mobile=  this.userInfo.Email?this.userInfo.Email:(this.userInfo.AreaCode+"|"+this.userInfo.Mobile)
            }
        let data ={
            Title:'交易密码',
            Mobile: Mobile//this.userInfo.Email?this.userInfo.Email:(this.userInfo.AreaCode+"|"+this.userInfo.Mobile)

        };
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
      upUserEntity(){
           if(!this.Code){
                this.$toast(this.$t('register.codeErr'));
                return;
            }
            if(this.pwd != this.pwd1){
                this.$toast(this.$t('transactionPwd.passwordErr1'));
            return 
            }
          let data = {
            UserId:this.userInfo.UserId,//用户编号</br>
            Action:'WithdrawPassword',
            ActionValue:`${this.pwd}#${this.Code}`
          };
          UpdateUserEntity(data).then(r=>{
              if(r.data.Code ==200 ){
                  let that = this;
                     this.$toast({
                        message: this.$t('transactionPwd.seccess'),
                        onClose(){
                            that.$router.push("/mine")
                        }
                     })
              }
          })
      },
        pwdInput(val){
            this.txtNow = val;
            this.showKeyboard= true;
        },
        onInput(key) {
            this[this.txtNow]= (this[this.txtNow]+ key).slice(0, 6);
            console.log(this[this.txtNow]);
        },
    onDelete() {
      this[this.txtNow]= this[this.txtNow].slice(0, this[this.txtNow].length - 1);
    },
    },
    destroyed(){
          document.body.style.background='#fff';
    },
     computed: {
        ...mapState(["base"]),
    },

}
</script>
<style lang="less" >
.transaction_pwd_wrap_main{
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
<style lang="less" >
    .transaction_pwd_wrap_main{
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
    .transaction_pwd_wrap{
        height: 100vh;
        background-color: @background-color;
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
                        width: 70%;
                    }
                    input{
                        height: 100%;
                        color:@font_1_color;
                        width: 100%;
                        font-size: 26px;
                        background-color:transparent;
                    }
                    .vcode_bnt{
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        // border: 1px solid @main_color;
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
                &:not(:nth-of-type(1)){
                    li{
                        width: 100%;
                    }
                }
            }
        }
        .btns{
            margin-top: 50px;
            padding: 20px;
        }
    }
</style>