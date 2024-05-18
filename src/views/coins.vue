<template>
  <div class="buy_coin_content" ref="buy_coin_content">
      <div class="top">
            <van-nav-bar
            class="nav"

                @click-left="token?'':$router.go(-1)"
                >
                <!-- @click-right="$router.push('/order')" -->
                <template  #left >
                    <van-icon
                    v-show="!token"
                    :name="require('../assets/images/common/left-arrow.png')"
                    size="18"
                    />
                </template>
            </van-nav-bar>
            <div class="nav_bar">
                <div v-for="(i,j) in navList" @click="changeNav(j)" :key="j" :class="{'act':j == navIndex}">{{i}}</div>
            </div>
            <div class="pic"><img src="../assets/images/common/buy_coin.png" alt=""></div>
      </div>
    <div class="contnet">
       <p>{{$t('Coin.Selectcurrency')}}</p>
       <ul @click="showCurrencyList=true">
           <li>
              <input type="text" v-model="Currency" readonly :placeholder="$t('Coin.SelectcurrencyPl')">
           </li>
           <li ><img src="../assets/images/common/arrow_right.png" alt=""></li>
       </ul>
       <div v-show="navIndex == 0">
        <p>{{$t('base.quantity')}}</p>
        <ul>
            <li>
                <input type="text"  v-model="Amount"  @input="enterAmount"  :placeholder="$t('deal.quantityErr')">
            </li>
            <li style="color: #fff" >{{Currency}}</li>
        </ul>
            <p class="tips" ><span>{{$t('Coin.Actual')}}</span><span>{{Receipt}} USDT</span></p>

       </div>
       <div v-show="navIndex == 1">
        <p>{{$t('base.quantity')}}</p>
        <ul>
            <li>
                <input type="text"  v-model="Amount"  @input="enterAmount"  :placeholder="$t('deal.quantityErr')">
            </li>
            <li style="color: #fff">USDT</li>
        </ul>
            <p class="tips" ><span>{{$t('property.usable')}}</span><span>{{CoinItem.Balance}} USDT</span></p>
            <p class="tips" ><span>{{$t('Coin.Minimum')}}</span><span>{{SellCoinQuantity}} USDT</span></p>
            <p class="tips" ><span>{{$t('Coin.Available')}}</span><span>{{sellReceipt}} {{Currency}}</span></p>

       </div>
    </div>
        <div class="btns" v-show="navIndex==0">
            <button class="main_btn" @click="buyConfirm">{{$t('base.confirm')}}</button>
            <p @click="toRecord(1)">{{$t('Coin.buyCoinRec')}}</p>
        </div>
        <div class="btns" v-show="navIndex==1">
            <button class="main_btn" @click="sellConfirm">{{$t('base.confirm')}}</button>
            <p @click="toRecord(2)"> {{$t('Coin.sellCoinRec')}}</p>
        </div>
    <van-popup v-model="showTips" position="bottom" class="tips_content" round>
        <div class="title">
            <p>{{$t('Coin.Notes')}}</p>
            <img @click="showTips = false" src="../assets/images/common/tips_close.png" alt="">
        </div>
        <div class="con">
            <iframe v-show="navIndex == 0" :src="iframeUrl" ref="iframe" frameborder="0"></iframe>
            <div class="description"  v-html="escape2Html(description) " v-if="navIndex == 1" ></div>
        </div>
        <div class="txt" v-show="navIndex == 1" >
            <p>{{$t('Coin.Payout')}}</p>
            <textarea name="" v-model="ReceiptAccount" :placeholder="$t('Coin.PayoutPlease')"></textarea>
        </div>
        <button v-show="navIndex == 0" :disabled='isdisabled' @click="buy" class="main_btn"> {{isdisabled? t+' s' :$t('Coin.pay')}}</button>
        <button v-show="navIndex == 1"  @click="showPwd" class="main_btn"> {{$t('base.confirm')}}</button>
    </van-popup>

    <van-popup v-model="showCurrencyList" position="bottom" round>
      <van-picker
        show-toolbar
        :columns="CurrencyList"
        @cancel="showCurrencyList = false"
        @confirm="onConfirm"
      />
    </van-popup>

    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitOpenFutures"
      show-cancel-button
    >
      <!-- <p class="pwd_tips" @click="showKeyboard = !showKeyboard"  v-show="!Password">{{$t('login.password')}}</p> -->
      <ul class="pwd_num" @click="showKeyboard = !showKeyboard" >
        <li v-for="i in Password" :key="i"></li>
        <li class="unent" v-for="(i,k) in (6-Password.length)" :key="k"></li>
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
import{
    GetBuyLegalCurrencyList,
    SubmitQuickBuyCoin,
    GetSellLegalCurrencyList,
    GetUserCommodityAccountsInfo,
    SubmitQuickSellCoin,
    GetMinSellCoinQuantity
    }from "@/api";
import {encryptByDES} from "@/libs/util"
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
import {
  Icon,
  NavBar,
  Locale,
  Popup,
  Picker,
    PasswordInput,
  NumberKeyboard,
} from "vant";
export default {
    name:"buyCoins",
     components: {
        [Icon.name]: Icon,
        [NavBar.name]: NavBar,
        [Popup.name]: Popup,
        [Picker.name]: Picker,
        [PasswordInput.name]: PasswordInput,
        [NumberKeyboard.name]: NumberKeyboard,
    },
    data(){
        return {
            navList :[this.$t('Coin.buy'),this.$t('Coin.sell')],
            navIndex:0,
            showTips:false,
            iframeUrl:"",
            lang:"",
            isdisabled:true,
            showCurrencyList:false,
            CurrencyList:[],
            Currency:"",
            MINT_auth:{},
            Amount:"",
            rate:"",
            t:3,
            Receipt:'0.00',
            description:'',
            show:false,
            showKeyboard:false,
            Password:"",
            ReceiptAccount:"",
            CoinItem:{},
            sellReceipt:'0.00',
            SellCoinQuantity:'',
            token:''
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
     this.iframeUrl =  this.$config.imgPath+`/News/AppSynopsis?id=0a4d0a8f-5ee7-4054-8da2-f20cbbcd002a&LanguageVersion=${this.lang}`
      setTimeout(_=>{
        var _clientHeight=document.documentElement.clientHeight;
          let cssLink = document.createElement("link");
          cssLink.href = "style.css";
          cssLink.rel = "stylesheet";
          cssLink.type = "text/css";
//           var doc=document.getElementById("edit").contentWindow.document;
//
// //If you are doing any dynamic writing do that first
//           doc.open();
//           doc.write(myData);
//           doc.close();
//
// //Then append child
//           doc.body.appendChild(cssLink)
        console.log(_clientHeight)
        // this.$refs.iframe.src= this.iframeUrl//this.$config.imgPath+`/News/AppSynopsis/16515ae1-eb5b-4aa3-99cc-fc1ace43ede4?LanguageVersion=${lang}`;
        // this.$refs.iframe.style.height= _clientHeight+'px';
        // this.$refs.iframe.style.width='100%';
      },500)
       var windowHeight = document.documentElement.clientHeight;
        this.$refs.buy_coin_content.style.minHeight= windowHeight + 'px';
this.getBuyLegalCurrencyList()
  },
  methods:{
    getMinSellCoinQuantity(){
        GetMinSellCoinQuantity({}).then(r=>{
             if(r.data.Code == 200 ){
                this.SellCoinQuantity = r.data.Data;
             }
        })
    },
       escape2Html(str) {
         var arrEntities = { lt: "<", gt: ">", nbsp: " ", amp: "&", quot: '"' };
    let s =  str?str.replace(/&(lt|gt|nbsp|amp|quot);/gi, function (all, t) {
        return arrEntities[t];
      }):'';
      s = s?s.replace(/<img/g,"<img style='max-width:100%;height:auto;'"):'';
      s = s?s.replace(/\n/g,"<br/>"):'';
      s=  s?s.replace(/<p/g,"<p style='word-break:break-all;'"):'';
       return s
    },
       showPwd() {
           if(!this.ReceiptAccount){
               this.$toast(this.$t('Coin.PayoutPlease'))
               return
           }
           this.showTips = false;
      this.show = true;
      this.showKeyboard = true;
      this.Password = "";
    },
       onInput(key) {
      this.Password = (this.Password + key).slice(0, 6);
      console.log(this.Password);
    },
    onDelete() {
      this.Password = this.Password.slice(0, this.Password.length - 1);
    },
      submitOpenFutures(action, done) {
          var data = {
              UserId:this.MINT_auth.UserId,//用户token
                LegalCurrency:this.Currency,
                Quantity:this.Amount,
                ReceiptAccount:this.ReceiptAccount,
                SafePassword:this.Password
          }
             this.$toast.loading({
            duration: 0, // 持续展示 toast
            forbidClick: true,
            message: this.$t('login.loading'),
        });
          SubmitQuickSellCoin(data).then(r=>{
              if(r.data.Code == 200 ){
                  this.$toast.clear()
                  this.show = false;
                    this.showKeyboard = false;
                    this.Password = "";
                    this.ReceiptAccount=''
                    this.$t(r.data.Message)
                  this.changeNav(1)
              }
          })
          console.log(data)
      },
      sellConfirm(){
            if(!this.Currency){
              this.$toast(this.$t('Coin.SelectcurrencyPl'))

              return
          }
          if(!this.Amount){
              this.$toast(this.$t('deal.quantityErr'))
              return
          }
          if(this.Amount<this.SellCoinQuantity){
              this.$toast(this.$t('Coin.Minimum')+ this.SellCoinQuantity)
              return
          }
          this.showTips=true;

      },
      getUserCommodityAccountsInfo(){
          GetUserCommodityAccountsInfo({UserId:this.MINT_auth.UserId}).then(r=>{
              if(r.data.Code == 200 ){
                  r.data.Data.DataList.map(_=>{
                      if(_.Coin == 'USDT'){
                          this.CoinItem = _
                      }
                  })
                  console.log(r.data)
              }
          })
      },
      changeNav(j){
          this.navIndex = j;
          this.Currency=''
          this.Amount =''
          this.CurrencyList = [];
          if(j==0){
              this.getBuyLegalCurrencyList()
          }else{
              this.getUserCommodityAccountsInfo()
              this.getSellLegalCurrencyList()
              this.getMinSellCoinQuantity()
          }
      },

      getSellLegalCurrencyList(){
          GetSellLegalCurrencyList({}).then(r=>{
            console.log(r);
            if(r.data.Code==200){
                let arr = [];
                r.data.Data.map(_=>{
                    arr.push({value:_.rate,text:`${_.legalcurrency}`,description:_.description})

                })
                this.CurrencyList =arr
            }
        })

      },
      buy(){
          this.submitQuickBuyCoin()
      },
      submitQuickBuyCoin(){
          var data = {
              UserId:this.MINT_auth.UserId,
              LegalCurrency:this.Currency,
              Quantity:this.Amount,
          }
          console.log(data)
           this.$toast.loading({
            duration: 0, // 持续展示 toast
            forbidClick: true,
            message: this.$t('login.loading'),
        });
            //  var r = {data: {"Code":200,"Message":"Successful operation","Data":"https://m.baidu.com?uid=XV876637&platform=MINT&buy=83.33333333USDT&pay=100USD"}
            //  }
          SubmitQuickBuyCoin(data).then(r=>{
              if(r.data.Code==200){
                    this.$toast.clear();
                    this.showTips = false;
                    this.Currency = '';
                    this.Amount=''
                  console.log(JSON.stringify(r.data))
                  this.$store.dispatch('setIsLogin', false);
                  this.$router.push({
                      path:'/h5',
                      query:{
                          title:this.$t('Coin.pay'),
                          url:r.data.Data
                      }
                  })
                //   {"Code":200,"Message":"Successful operation","Data":"https://m.baidu.com?uid=XV876637&platform=MINT&buy=83.33333333USDT&pay=100USD"}
              }

          })
      },
      buyConfirm(){
          if(!this.Currency){
              this.$toast(this.$t('Coin.SelectcurrencyPl'))
              return
          }
          if(!this.Amount){
              this.$toast(this.$t('deal.quantityErr'))
              return
          }
          this.showTips=true;
          var t= setInterval(_=>{
              if(this.t <= 0){
                  this.isdisabled = false
                  clearInterval(t)
              }else{
                  this.t -=1
              }
          },1000)
      },
          //   <!-- 快捷买币卖币 => 获取买币支持的法币列表 -->
    getBuyLegalCurrencyList(){
        GetBuyLegalCurrencyList({UserId:this.MINT_auth.UserId}).then(r=>{
            console.log(r);
            if(r.data.Code==200){
                let arr = [];
                r.data.Data.map(_=>{
                    arr.push({value:_.rate,text:`${_.legalcurrency}`})

                })
                this.CurrencyList =arr
            }
        })
    },
      onConfirm(value, index) {
          console.log(value.description)
          this.description = value.description
        this.Currency=value.text;
        this.rate = value.value
        this.showCurrencyList = false;
        },
        enterAmount(){
          this.Amount= this.Amount.replace(/[^\d.,]/g, ""); //清除“数字”和“.”以外的字符
		this.Amount = this.Amount.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的
        this.Amount = this.Amount.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
        if(this.navIndex == 0){
            this.Amount = this.Amount.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'); //只能输入两个小数
        }else{
            this.Amount = this.Amount.replace(/^(\-)*(\d+)\.(\d\d\d\d\d\d\d\d).*$/, '$1$2.$3'); //只能输入两个小数
        }
      },
      toRecord(type){
          if(type==1){
              this.$router.push({
                  path:'/buyCoinsRecord',
              })
          }else{
               this.$router.push({
                  path:'/sellCoinsRecord',
              })
          }
      }
    //   buyCoinsRecord
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
      rate(){
          this.Receipt =  Number(this.Amount / this.rate).toFixed(2)
          this.sellReceipt =  Number(this.Amount * this.rate).toFixed(2)
      },
      Amount(){
          this.Receipt =  Number(this.Amount / this.rate).toFixed(2)
          this.sellReceipt =  Number(this.Amount * this.rate).toFixed(2)
      }
  }

}
</script>
<style lang="less">
    .buy_coin_content {
        background-color: @bg_clor;
    }
</style>
<style lang="less" scoped >
.buy_coin_content{
    position: relative;
    .top{
        height: 350px;
        position: relative;
        background-color: @bg_clor;
        .nav{
            background: transparent;
            &.van-hairline--bottom{
                &:after{
                    border:unset;
                }
            }
        }
        .nav_bar{
            padding: 16PX;
            display: flex;
            margin-top: 80px;
            font-size: 36px;
            font-weight: 500;
            color: rgba(255, 255, 255, .5);
            >div{
                margin-right: 60px;
                font-size: 44px;
                &.act{
                    color: #fff;
                }
            }
        }
        .pic{
            position: absolute;
            width: 290px;
            // height: 196px;
            right: 64px;
            bottom: 28px;
            img{
                width: 100%;
                // height: 100%;
            }
        }
    }
       .contnet{
        padding: 40px ;
        border-radius:24px ;
        overflow: hidden;
        margin-top:-24px ;
        position: relative;
        p{
            color:@minor-font-color;
            margin:40px 0 30px;

            &.tips{
                display: flex;
                justify-content: space-between;

            }
        }
            ul{
                display: flex;
                height: 100px;
                align-items: center;
                justify-content: space-between;
                position: relative;
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
                                background-color: @main_color2;
                                color: #fff;
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
                        color:@font_1_color;
                        font-size: 26px;
                        background-color:transparent;
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
                        width: 15px;
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
        // .tips{
        //     padding: 20px;
        //     height: 50px;
        //     line-height: 50px;
        //     font-size: 28px;
        //     color: @sub-font-color;
        // }
        .btns{
            padding: 20px;
            margin-top: 80px;
            // position: absolute;
            bottom: 90px;
            width: 100%;
            button{
                background: @main_color2;
            }
            p{
                text-align: center;
                margin-top: 60px;
                font-size: 28px;
                color: @main_color2;
            }
        }
        .tips_content{
            min-height: 53vh;
            padding: 0 30px 30px;
            .title{
                position: relative;
                padding: 42px;
                display: flex;
                justify-content: center;
                align-items: center;
                font-size: 36px;
                color: @main_color2;
                font-weight: 500;
                img{
                    position: absolute;
                    right:30px;
                    width: 40px;

                }
            }
            iframe{
                width: 100%;
                height: 30vh;

            }
            .description{
                width: 100%;
                max-height:  20vh;
                overflow: scroll;
            }
            .txt{
                p{
                    color:#999999;
                    padding: 24px 0;
                }
                textarea{
                    background-color: #F5F6FA;
                    width: 100%;
                    border-radius: 16px;
                    height: 200px;
                    padding: 30px;
                    color:@font_color;

                }
            }
            button{
                display: block;
                margin: 10px auto;
                width: 100%;
                background-color: @main_color2;
                &:disabled{
                    background-color: rgba(22, 121, 248, .5);
                }

            }
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
    background-color: #000;
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
          background-color: #000;
          top: 50%;
          left: 0;
          transform: translate(0,-50%);
        }
    }
  }
}


}
</style>
<style lang="less">
.buy_coin_content{
      .custom-image .van-empty__image {
         width: 480px;
         height: 298px;
         margin-top: unset;
       }
  .van-nav-bar{
            background-color: transparent;
        }
        .van-icon,
        .van-nav-bar__left{
          color:#fff !important;
        }
        .van-hairline--top-bottom,
        .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
  .van-overlay {
    z-index: 10 !important;
  }

}
</style>