<template>
  <div class="financial_wrap">
        <van-nav-bar
        v-show="!token"
      :title="$t('financial.title')"
      left-text
      left-arrow
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
    <div class="top">
        <p class="price">{{baseData.FinancialBalance |decimals(2) }}</p>
        <p>{{$t('financial.FinancialBalance')}}(USDT)</p>
        <div >
            <div>
                <p class="mc">{{baseData.Balance |decimals(2) }}</p>
                <p>{{$t('financial.Balance')}}</p>
            </div>
            <div>
                <p class="green">{{baseData.TotalRevenue |decimals(2) }}</p>
                <p>{{$t('financial.TotalRevenue')}}</p>
            </div>
        </div>
    </div>
    <div class="content">
        <p class="title">{{$t('financial.Amount')}}</p>
        <div class="text_wrap">
            <input type="text" v-model="Amount" :placeholder="$t('financial.enterAmount')">
        </div>
        <p class="title">{{$t('financial.standard')}}</p>
        <div class="sel_box">
            <ul>
                <li @click="FinancialIndex = index" :class="{act:index == FinancialIndex}" v-for="( i , index) in baseData.PackageList">
                    <div>
                        <p>+{{i.rate*100 |decimals(2)  }}%/{{$t('financial.day')}}</p>
                        <p>{{i.cycle}}{{$t('mining.Days')}}</p>
                    </div>
                </li>
                
            </ul>
        </div>
        <p class="tips">{{$t('financial.DueTips')+income}}</p>
        <div class="btns">
            <button class="main_btn" @click="showPwd">{{$t('financial.btnChange')}}</button>
            <button class="sub_btn" @click="toManager">{{$t('financial.btnManagement')}}</button>
        </div>
    </div>
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitOpenFutures"
      @close="closeDialog"
      show-cancel-button
    >
      <!-- <p class="pwd_tips" @click="showKeyboard = !showKeyboard"  v-show="!Password"></p> -->
      <ul class="pwd_num" @click="showKeyboard = !showKeyboard"  >
        <li v-for="i in Password"></li>
        <li class="unent" v-for="i in (6-Password.length)"></li>
      </ul>
    </van-dialog>
    <van-number-keyboard
      :show="showKeyboard"
      @input="onInput"
      @delete="onDelete"
      @blur="showKeyboard = false"
    />
  </div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  Tab,
  Tabs,
  PasswordInput,
  Empty,
  List,
  Sticky,
  NumberKeyboard,
} from "vant";
import {checkRates,encryptByDES} from '@/libs/util'
import { Locale } from 'vant';
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
import {GetFinancialManagementPageData ,PostTask} from "@/api"
export default {
    name:'news',
     components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    [Empty.name]: Empty,
    [Tab.name]: Tab,
    [Tabs.name]: Tabs,
    [Popup.name]: Popup,
    [Sticky.name]: Sticky,
     [List.name]:List,
     [NumberKeyboard.name]:NumberKeyboard,
  },
    data(){
        return {
            baseData:{},
            FinancialPackage:{},
            FinancialIndex:0,
            Amount:"",
            income:0,
            MINT_auth:{},
            show:false,
            showKeyboard:false,
            Password:""

        }
    },
      created(){
      let lang=this.$route.query.lang;
      let token=this.$route.query.token;
      this.token = token;
        if(lang){
            localStorage.setItem('MINT_lang',lang);
        }else{
        lang = localStorage.getItem('MINT_lang') ?localStorage.getItem('MINT_lang'):'en'
        }
        this.$i18n.locale = lang;
        if(lang=='zh-CN'){
        Locale.use('zh-CN', zhCN);
        }else if(lang=='zh-hk'){
        Locale.use('zh-HK', zhHK);
        }else if(lang=='jpn'){
        Locale.use('ja-JP', jaJP);
        }else {
        Locale.use('en-US', enUS);
        }
        if(token){
          console.log('url token')
          this.MINT_auth.UserId=token;
          localStorage.setItem('isApp',true);
           let MINT_auth= encryptByDES(JSON.stringify(this.MINT_auth));
          localStorage.setItem('MINT_auth',MINT_auth)
        }
  },
    mounted(){
         if(!this.token){
        console.log('localStorage token')
        let MINT_auth = localStorage.getItem("MINT_auth");
        if(!MINT_auth){
          this.$router.push('/login')
        }
        this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
      }
        //  let MINT_auth = localStorage.getItem("MINT_auth");
        // this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
        this.getData();
        // this.getExpandInfoList()
    },
    methods:{
        getData(){
            GetFinancialManagementPageData({UserId:this.MINT_auth.UserId}).then(r=>{
                if(r.data.Code == 200){
                    console.log(r.data.Data)
                    this.baseData= r.data.Data;
                    this.getIncome()
                }
            })
        },
        showPwd(){
            if(!checkRates(this.Amount)) {
               this.$toast(this.$t('financial.AmountErr'))
                return
           }
            this.show = true;
            this.showKeyboard = true;

        },
        postTask(){
           if(this.Password.length!=6){
               this.$toast(this.$t('transactionPwd.passwordErr'))
                return
           }
           let Amount =  this.Amount.split('.')[0];
           console.log(Amount)
            let data = {
                Password:this.Password,
                UserId:this.MINT_auth.UserId,
                ItemDetailId:this.baseData.PackageList[this.FinancialIndex].itemdetailid,//套餐主键
                Amount:Amount//转入金额
            }
            // if(!this.Amount){
            //     this.$toast(this.$t('financial.AmountErr'))
            //     return
            // }
            this.$toast.loading({
                duration: 0, // 持续展示 toast
                forbidClick: true,
                // message: this.$t('login.loading'),
            });
            PostTask(data).then(r=>{
                if(r.data.Code == 200){
                    console.log(r.data.Data)
                    this.FinancialPackage = r.data.Data;
                    this.$toast(r.data.Message)
                    let query = this.token?{token:this.token}:{}
                     this.$router.push({
                        path:'/success',
                        query
                        // query:{
                            // title:this.$t('financial.success')
                        // }
                    })
                }

            })
        },
        getIncome(){
            let PackageList = this.baseData.PackageList;
            let index = this.FinancialIndex;
            this.income = (this.Amount * (PackageList[index].rate)*(PackageList[index].cycle)).toFixed(2)
        },
        closeDialog(){
            this.Password='';
        },
        submitOpenFutures(action, done) {
        if (!this.Password) {
            return ;
        }
        this.postTask();
        },
        onInput(key) {
                this.Password= (this.Password+ key).slice(0, 6);
                console.log(this.Password);
            },
            onDelete() {
            this.Password= this.Password.slice(0, this.Password.length - 1);
            },
            toManager(){
                let query = this.token?{token:this.token}:{}
                this.$router.push({
                    path:"/financial_admin",
                    query
                })
            }
    },
    watch:{
        '$route'(to, from) {
        // console.log(to, from)
            let lang=this.$route.query.lang;
      let token=this.$route.query.token;
      this.token = token;
        if(lang){
            localStorage.setItem('MINT_lang',lang);
        }else{
        lang = localStorage.getItem('MINT_lang') ?localStorage.getItem('MINT_lang'):'en'
        }
        this.lang =lang
        this.$i18n.locale = lang;
        if(lang=='zh-CN'){
        Locale.use('zh-CN', zhCN);
        }else if(lang=='zh-hk'){
        Locale.use('zh-HK', zhHK);
        }else if(lang=='jpn'){
        Locale.use('ja-JP', jaJP);
        }else {
        Locale.use('en-US', enUS);
        }
        var isLogin = false
        if(token){
          console.log('url token')
          this.MINT_auth.UserId=token;
          localStorage.setItem('isApp',true);
           let MINT_auth= encryptByDES(JSON.stringify(this.MINT_auth));
          localStorage.setItem('MINT_auth',MINT_auth)
          isLogin = false
        }else{
            localStorage.setItem('isApp','');
            isLogin  = !!localStorage.getItem('MINT_auth')

        }
        this.$store.dispatch('setIsLogin', isLogin);
        },
        Amount(){
            this.getIncome()
        },
        FinancialIndex(){
            this.getIncome()
        }
    }

}
</script>
<style lang="less" >
.financial_wrap{
  .van-overlay {
    z-index: 10 !important;
  }

}
</style>
<style lang="less" scoped>
                   .pwd_tips {
                       text-align: center;
                       height: 90px;
                       // margin: 10px;/*  */
                       line-height: 90px;
                   }
.pwd_num {
    display: flex;
    height: 90px;
    justify-content: center;
    align-items: center;
    li {
        border-radius: 50%;
        width: 30px;
        height: 30px;
        background-color: @panel_color;
        margin: 0 10px;
        &.unent{
            margin: 0 10px;
            border: none;
            background-color: transparent;
            position: relative;
            &:before{
                position: absolute;
                content: '';
                display: block;
                width: 30px;
                height: 2px;
                background-color: @panel_color;
                top: 50%;
                left: 0;
                transform: translate(0,-50%);
            }
        }

    }
}
.financial_wrap{
    .top{
        height: 356px;
        background: url('../assets/images/common/information_bg.png') no-repeat;
        background-size: cover;
        text-align: center;
        >p{
            &:nth-of-type(1){
                padding-top: 54px;
                color: @main_color;
                font-size: 76px;
                font-weight: 500;
            }
            &:nth-of-type(2){
                color: #fff;
                margin-top: 10px;
                font-size: 28px;
            }
        }
        >div{
            color: #fff;
            margin-top: 30px;
            display: flex;
            justify-content: center;
            >div{
                flex: 1;
                p{
                    margin-bottom: 10px;
                    &.mc{
                        color: @main_color;
                    }

                    &.green{
                        color: #77FF25;

                    }
                }
                &:nth-of-type(1){
                    position: relative;
                    &::before{
                        position: absolute;
                        content: '';
                        display: block;
                        width: 1px;
                        height: 80%;
                        background: #fff;
                        right: 0;
                        top: 0;
                    }
                }

            }
        }
    }
    .content{
        background-color: @background-color;
        height: 100vh;
        padding: 30px;
        > .title {
            position: relative;
            color: @font_1_color;
            padding-left: 30px;
            font-size: 28px;
            font-weight: 500px;
            margin: 20px 0 ;
            &::before {
                position: absolute;
                content: " ";
                left: 0;
                height: 100%;
                background: @main_color;
                display: block;
                width: 10px;
            }
        }
        .text_wrap{
            background: @item_bg;
            height: 108px;
            border-radius: 10px;
            input{
                background-color: @panel_color;
                padding-left: 20px;
                color: @font_1_color;
                height: 100%;
                width: 100%;
                font-size: 30px ;
            }
        }
        .sel_box{
            ul{

                display: flex;
                flex-wrap: wrap;
                justify-content: space-between;
                li{
                    width:  49%;
                    margin: 10px 0;
                    background-color: @panel_color;
                    border-radius: 10px;
                    height: 110px;
                    display: flex;
                    align-items: center;
                    justify-content: center;
                    border: 1px solid #D7D7D7;
                    >div{
                        >p{
                            color: @sub-font-color;
                            text-align: center;
                            font-size: 30px;
                            &:nth-of-type(2){
                                color: @main_color;
                            }
                        }

                    }
                    &.act{
                        background: #282724;
                        >div{
                            >p{
                                color: #fff;
                                text-align: center;
                                font-size: 30px;
                                &:nth-of-type(2){
                                    color: @main_color;
                                }
                            }
                        }
                    }

                }
            }
        }
        > .tips{
            text-align: center;
            color: @sub-font-color;
            margin: 40px 0;
            font-size: 24px;
        }
        >.btns{
            width: 100%;
            button{
                display: block;
                width: 100%;
                height: 84px;
                margin: 30px 0 ;
                border-radius: 12px;
                font-size: 28px;

            }
            .main_btn{
                background: @main_color;
            }
            .sub_btn{
                background: @bg_color;
                color: @main_color;
            }
        }
    }
}
</style>