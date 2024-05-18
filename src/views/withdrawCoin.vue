<template>
<div class="transaction_wrap_main">
<van-nav-bar :title="$t('withdraw.title')" left-text  left-arrow @click-right="$router.push('/withdraw_record')" @click-left="bank" >
<template #right>
        <van-icon
          :name="require('../assets/images/common/ic_withdrawl_coin_details.png')"
          size="18"
        />
      </template>
</van-nav-bar>
  <div class="transaction_wrap">    
    <div class="contnet">
        <p>{{$t('withdraw.Balance')}}</p>
       <ul>
           <li>
              <input type="text" readonly v-model="coinInfo.Balance" >
           </li>
       </ul>
       <p>{{$t('withdraw.Freeze')}}</p>
       <ul>
           <li>
              <input type="text" readonly v-model="coinInfo.Freeze" >
           </li>
       </ul>
       <p>{{$t('withdraw.ToAddress')}}</p>
       <ul>
           <li>
              <textarea type="text" class="address" v-model="ToAddress"  :placeholder="$t('withdraw.ToAddressTxt')"/>
           </li>
           <li @click="selAdd"><img src="../assets/images/common/add_address.png" alt=""></li>
       </ul>
       <p>{{$t('withdraw.ChainName')}}</p>
       <ul v-show="coinInfo.Coin == 'USDT'" class="chainName">
           <li>
               <div class="btns">
                   <button @click="btnIndex = index" :class="['main_btn',{'active':btnIndex == index}]" v-for="(i,index) in btns">{{i}}</button>
               </div>
           </li>
           <li>
           </li>
       </ul>
       <p>{{$t('withdraw.Amount')}}</p>
       <ul>
           <li>
              <input type="text" v-model="Amount" @input="enterAmount" :placeholder="$t('withdraw.AmountTxt')">
           </li>
       </ul>
       <p>{{$t('withdraw.WithdrawPassword')}}</p>
       <ul @click="showKeyboard = true">
           <li>
               <input type="password" readonly v-model="WithdrawPassword" :placeholder="$t('withdraw.pwdTxt')">
           </li>
       </ul>
       <!-- <p>{{$t('withdraw.PhomeCode')}}</p>
        <ul>
           <li>
               <input type="text" v-model="Code" :placeholder="$t('withdraw.plPhomeCode')">
           </li>
           <li>
               <button class="vcode_bnt" :disabled='!isGetCode' @click="getCode">{{btnTips}}</button>
           </li>
       </ul> -->
       <!-- <p>{{$t('withdraw.EmailCode')}}</p>
        <ul>
           <li>
               <input type="text" v-model="Code2" :placeholder="$t('withdraw.plEmailCode')">
           </li>
           <li>
               <button class="vcode_bnt" :disabled='!isGetCode2' @click="getCode('Email')">{{btnTips2}}</button>
           </li>
       </ul> -->
    </div>
     <p class="tips" v-show="coinInfo.Coin != 'USDT'" >{{$t('withdraw.SetValueTips')}} : {{coinInfo.SetValue | coinSetValue(1)}} {{coinInfo.Coin}}</p>
     <p class="tips" v-show="coinInfo.Coin == 'USDT'" >{{$t('withdraw.SetValueTips')}} : {{ USDTFeeSetItem[btnIndex]}} {{coinInfo.Coin}}</p>
     <p class="tips">{{$t('withdraw.minValueTips')}} : {{coinInfo.SetValue | coinSetValue(0)}} {{coinInfo.Coin}}</p>
    <div class="btns">
        <button :disabled='coinInfo.IsWithdraw!=1' class="main_btn" @click="submitUserTakeOutCoins">{{$t('withdraw.btn')}}</button>
    </div>
      <div style="height: 500px"></div>
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
import QRCode from 'qrcodejs2';
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  PasswordInput,
   NumberKeyboard
} from "vant";
import Vue from 'vue';
import{GetMobileCode ,SubmitUserTakeOutCoins} from '@/api'
Vue.use(PasswordInput);
Vue.use(NumberKeyboard);
import{mapState} from 'vuex'
export default {
    name:'transaction',
    components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
        [Picker.name]:Picker,
        [Popup.name]:Popup,
    },
    data(){
        return{
             userCoinAddress:{},
             coinInfo:{},
             WithdrawPassword:'',
             showKeyboard:false,
             MINT_auth:{},
             btns:['ERC20','TRC20','OMNI'],
             btnIndex:0,
            isGetCode:true,
            btnTips:'',
            isGetCode2:true,
            btnTips2:'',
            Code:'default',
            Code2:'default',
            Amount:'',
            ToAddress:'',
            USDTFeeSetItem:[],
        }
    },
    mounted(){
        this.btnTips = this.$t("forgetPwd.btnGetCode")
        this.btnTips2 = this.$t("forgetPwd.btnGetCode")
        console.log(this.base.userCoinAddress)
        this.ToAddress = this.base.userCoinAddress.coinaddress;
          let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
          let coinInfo = localStorage.getItem("coinInfo");
        coinInfo = this.$decryptByDES(coinInfo);
        this.coinInfo = JSON.parse(coinInfo);
        if(this.coinInfo.Coin == 'USDT'){
            this.USDTFeeSetItem = this.coinInfo.USDTFeeSetItem ?this.coinInfo.USDTFeeSetItem.split("#"):[]
        
        }
        console.log(coinInfo)
        document.body.style.background='#f5f5f5';
    },
    methods:{
        selAdd(){
            console.log(this.$route.fullPath)
            // this.$router.push()
            this.$router.replace({
                path:'/address',
                query:{
                   redirect: this.$route.fullPath
                },
            })
        },
        submitUserTakeOutCoins(){
            let ChainName = this.btns[this.btnIndex];
            let data = {
                UserId:this.MINT_auth.UserId,
                ChainName:this.coinInfo.Coin =='USDT'?ChainName: 'defailt',//链名称(只有USDT有ERC20,OMNI，其他的默认传defailt)
                Amount:this.Amount,
                ToAddress:this.ToAddress,
                WithdrawPassword:this.WithdrawPassword,
                Code:this.Code,
                Code2:this.Code2,
                Coin:this.coinInfo.Coin,

            };
            if(!this.ToAddress){
                this.$toast(this.$t('withdraw.ToAddressTxt'));
                return
            }
            if(!this.Amount){
                 this.$toast(this.$t('withdraw.AmountTxt'));
                return
            }
            if(!this.WithdrawPassword){
                this.$toast(this.$t('withdraw.pwdTxt'));
                return
            }
            if(!this.Code){
                 this.$toast(this.$t('withdraw.plPhomeCode'));
                return
            }
            if(!this.Code2){
                 this.$toast(this.$t('withdraw.plEmailCode'));
                return
            }
            console.log(data);
            this.$toast.loading({
                duration: 0, // 持续展示 toast
                message: this.$t('register.loading'),
                forbidClick: true,
            });
            SubmitUserTakeOutCoins(data).then(r=>{
                if(r.data.Code == 200 ){
                    console.log(r.data);
                    this.$toast(r.data.Message);
                    this.$router.replace('/property');
                    this.$toast.clear();
                }else{
                    // this.$toast.clear();
                }
            })
        },
         getCode(type){
             let Mobile = ''
             if(type=='Email'){
                 console.log(!this.MINT_auth.Email)
                 if(!this.MINT_auth.Email){
                    this.$router.push({
                        path:'/bind_account',
                        query:{
                            type:"email",
                            redirect:'/withdraw_coin'
                        }
                    })
                    return
                 }
                 if(!this.MINT_auth.Mobile){
                    this.$router.push({
                        path:'/bind_account',
                        query:{
                            type:"mobile",
                            redirect:'/withdraw_coin'
                        }
                    })
                    return
                 }
                     Mobile=this.MINT_auth.Email
                 
             }else{
                 if(!this.MINT_auth.Mobile){
                    this.$router.push({
                        path:'/bind_account',
                        query:{
                            type:"mobile",
                            redirect:'/withdraw_coin'
                        }
                    })
                    return
                 }
                 if(!this.MINT_auth.Email){
                    this.$router.push({
                        path:'/bind_account',
                        query:{
                            type:"email",
                            redirect:'/withdraw_coin'
                        }
                    })
                    return
                 }
                    Mobile = (this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
             }
            //  let Mobile = this.VerifCodeAccount=="Email">
            // if(this.MINT_auth.VerifCodeAccount){
            //     Mobile=  this.MINT_auth.VerifCodeAccount=="Email"?this.MINT_auth.Email:(this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
            //     }else{
            //         Mobile=  this.MINT_auth.Email?this.MINT_auth.Email:(this.MINT_auth.AreaCode+"|"+this.MINT_auth.Mobile)
            // }
        let data ={
            Title:'提币',
            Mobile: Mobile
        };
        this.$toast.loading({
            duration: 0, // 持续展示 toast
            forbidClick: true,
        });
        type=='Email'?(this.isGetCode2= false):(this.isGetCode= false);
        GetMobileCode(data).then(r=>{
            console.log(r);
            if(r.data.Code == 200 ){
               this.$toast( this.$t('register.sendSeccess'));
                let wait = 60;
                let time = setInterval(_=>{
                    wait --;
                    if(wait==0){
                        clearInterval(time);
                        if(type=='Email'){
                            this.btnTips2= this.$t("forgetPwd.btnGetCode");
                            this.isGetCode2 = true;
                        }else{
                            this.btnTips= this.$t("forgetPwd.btnGetCode");
                            this.isGetCode = true;
                        }
                    }else{
                        type=='Email'?(this.btnTips2=wait+'S'):(this.btnTips=wait+'S');
                        // this.btnTips=wait+'S';
                    }
                    
                },1000)
            }else{
                // this.isGetCode = true;
                type=='Email'?(this.isGetCode2= true):(this.isGetCode= true);
            }
        })
      },
      bank(){
          this.$router.push({
              path:'/acc_record',
              query:{
                  coin:this.coinInfo.Coin
              }
          })
      },
      enterAmount(){
          this.Amount= this.Amount.replace(/[^\d.,]/g, ""); //清除“数字”和“.”以外的字符   
		// this.Amount = this.Amount.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的   
      },
        showPopup() {
         this.show = true;
        },
         onInput(key) {
            this.WithdrawPassword= (this.WithdrawPassword+ key).slice(0, 6);
            console.log(this.WithdrawPassword);
        },
        onDelete() {
        this.WithdrawPassword= this.WithdrawPassword.slice(0, this.WithdrawPassword.length - 1);
        },
        onConfirm(value, index) {
      this.$toast(`当前值：${value}, 当前索引：${index}`);
    },
    onChange(picker, value, index) {
      this.$toast(`当前值：${value}, 当前索引：${index}`);
    },
    onCancel() {
      this.$toast('取消');
    },
    },
    destroyed(){
        document.body.style.background = '#fff';
    },
     computed: {
        ...mapState(["base"]),
    },

}
</script>
<style lang="less" >
    .transaction_wrap_main{
        background-color: @background-color;
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
    .transaction_wrap{
        // min-height: 100vh;
        .contnet{
            padding: 40px ;
            background-color: @background-color;
            >p{
                color:@minor-font-color;
                margin:15px 0;
            }
            ul{
                display: flex;
                height: 100px;
                align-items: center;
                justify-content: space-between;
                position: relative;
                color: @font_1_color;
                padding:0 30px;
                width: 100%;
                background-color: @panel_color;
                border-radius: 20px;
                &:not(:last-of-type){
                    .bot_br;
                }
                &.chainName{
                    background-color:transparent;
                    border-bottom: none;
                }
                li{
                    height: 80px;
                    display: flex;
                    align-items: center;
                    label{
                        display: block;
                        font-size: 28px;
                        color: @important-font-color;
                        margin-right: 20px;
                        width: 160px;
                        text-align-last: justify;
                    }
                    &:nth-of-type(1){
                        width: 100%;
                        >div{
                            display: flex;
                            padding: 0;
                            align-items: center;
                            margin: 0;
                            width: 100%;
                            height: 100%;

                            .main_btn{
                                width: 50%;
                                height: 100%;
                                // width: 120px;
                                margin: 0 10px;
                                background-color: @panel_color;
                                color: @font_1_color;
                                &.active{
                                    color :#fff;
                                    background-color: @main_color;
                                }
                            }

                        }
                    }
                    input{
                        height: 100%;
                        width: 100%;
                        font-size: 26px;
                        background-color:@panel_color;
                    }
                    text-area{
                        color: #1CED34;
                    }
                    .address{
                        width: 100%;
                        background-color: transparent;
                        resize: none;
                        height: 80px;
                        padding-top: 20px;
                        display: table-cell;
                        vertical-align:middle
                        // line-height: 80px;
                    }
                    img{
                        height: 35px;
                    }
                    .vcode_bnt{
                        position: absolute;
                        top: 50%;
                        right: 10px;
                        padding: 10px;
                        // border: 1px solid @main_color;
                        transform: translate(0,-50%,);
                        background: transparent;
                        color: @main_color;
                        width: 160px;
                        border-radius:8px;
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
        }
    }
</style>