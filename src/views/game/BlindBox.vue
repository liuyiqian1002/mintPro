<template>
  <div class="blind_box_wrap">
    <van-nav-bar
      :title="$t('blindBox.title')"
      left-text
       :left-arrow='!token'
      :placeholder="true" 
      fixed
      :right-text="$t('blindBox.rule')"
      @click-left="$router.go(-1)"
      @click-right='showRule'
    />
      <div class="l_bg">

      </div>
      <div class="r_bg">

      </div>
    <div class="title">
        {{$t('blindBox.goodLuck')}}
    </div>
     <van-notice-bar class="notice"  color="#03C1A7" background="#FFFFFF" 
     :text="notice" 
    >
        <span v-for="i,index in noticeList" style="margin-right:20px" :key="index">{{i}}</span>
     </van-notice-bar>
    <div class="panel">
        <p class="title">{{$t('blindBox.choose')}}</p>
        <div class="wrap">
            <ul>
                <li :class="{active:index==BoxtIndex}" @click="BoxtIndex =index" v-for="(i,index) in BlindBoxPageData.ItemList" :key="i.ItemDetailId">
                    <div>
                    <div class="pic">
                        <img src="" alt="">
                    </div>
                    <p>{{i.Amount}}USDT</p>
                    </div>
                </li>
            </ul>
            <p class="tips"><b>-</b>{{replaceQuantity($t('blindBox.couponTips')) }} <b>-</b></p>
            <p class="tips"><b>-</b>{{replaceQuantity1($t('blindBox.couponTips1')) }} <b>-</b></p>
        </div>
    </div>
    <div class="limit">
        <span>{{$t('blindBox.limit')}}</span>
        <span>{{BlindBoxPageData.TodayRemaining}} {{$t('blindBox.Times')}}</span>
    </div>
    <div class="sel">
        <div class="f">
            <div @click="Quantity = 1" :class="{active:Quantity==1}">
                1{{$t('blindBox.Times')}}
            </div>
            <div @click="Quantity = 5" :class="{active:Quantity==5}">
                <p>5{{$t('blindBox.Draw')}}</p>
                <p >{{BoxtIndex>=0?((BlindBoxPageData.ItemList[BoxtIndex].Discount)*100):'-'}}{{$t('blindBox.discount')}}</p>
            </div>
        </div>
        <div class="r">
                <span>{{$t('blindBox.Payable')}}</span>
                <span>{{pay}}USDT</span>
        </div>

    </div>
    <div class="take_btn">
        <button @click="openBlindBox">{{$t('blindBox.Blind')}}</button>
    </div>
    <div class="acc">
        <span>{{$t('blindBox.Available')}}:</span>
        <span>{{Balance}} USDT</span>
    </div>
    <div class="btns">
        <button @click="toLink('/blind_box_voucher')">
            <img src="../../assets/images/common/guess/exchange.png" alt="">
            {{$t('blindBox.exchange')}}
        </button>
        <button @click="toLink('/blind_box_record')">
            <img src="../../assets/images/common/guess/record.png" alt="">
            {{$t('blindBox.record')}}
        </button>
    </div>
    <div class="bot_bg_start">
        <img src="../../assets/images/common/guess/bot_bg_start.png" alt="">
    </div>
    <div class="empty"></div>
    <!-- <div class="bot_bg">
        <img src="../../assets/images/common/guess/blind_box_bot_bg.png" alt="">
    </div> -->
     <!-- 游戏说明 -->
      <van-dialog
      v-model="isShowRule"
      :title="$t('guess.ruleTitle')"
    >
    <div class="rule" v-html="escape2Html(BlindBoxPageData.BlindBoxRule)">

    </div>
    </van-dialog>
      <van-dialog
      v-model="showVoucher"
      :showConfirmButton='false'
      :closeOnClickOverlay='true'
      width='80%'
    >
    <div class="succes">
        <p>{{$t('blindBox.obtain')}}</p>
        <div class="con">
            <p>{{$t('blindBox.Voucher')}}</p>
        </div>
        <div class="line">
            <img src="../../assets/images/common/guess/voucher_bot.png" alt="">
        </div> 
        <div class="tips">
            <p>{{$t('blindBox.Voucher')}}</p>
        </div>
        <div class="btn">
            <button @click="toLink('/blind_box_voucher') ; showVoucher = false">{{$t('blindBox.redeem')}}</button>
        </div>
    </div>
    </van-dialog>
    <!--  -->
      <van-dialog
      v-model="showWinning"
      :showConfirmButton='false'
      :closeOnClickOverlay='true'
      width='80%'
    >
    <div :class="['succes',{'obtainbg':Quantity==5}]">
        <p>{{$t('blindBox.obtain')}}</p>
        <div class="coin">
            <div class="obtain" v-show="Quantity==5">
                <ul v-for="(i,index) in WinningItem" :key="index">
                    <li >
                        <img :src="i.Icon?($config.imgPath+ i.Icon):require('../../assets/images/common/guess/coin.png') "alt="">
                    </li>
                    <li>{{i.Quantity}} {{i.Coin}}</li>
                </ul>
            </div>
            <div  v-show="Quantity==1">
                <ul>
                    <li v-for="(i,index) in WinningItem" :key="index">
                        <img :src="i.Icon?($config.imgPath+ i.Icon):require('../../assets/images/common/guess/coin.png') "alt="">
                    </li>
                </ul>
            </div>
        </div>
        <div class="line" v-show="Quantity==1">
            <img src="../../assets/images/common/guess/voucher_bot.png" alt="">
        </div> 
        <div class="tips" v-show="Quantity==1">
            <ul>
                <li v-for="(i,index) in WinningItem" :key="index">{{i.Quantity}} {{i.Coin}}</li>
            </ul>
        </div>
        <div class="btn">
            <button @click="showWinning = false">{{$t('blindBox.Accept')}}</button>
        </div>
    </div>
    </van-dialog>
    <!-- 密码 -->
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="openBlindBox"
      show-cancel-button
    >
      <ul
        class="pwd_num"
        @click="showKeyboard = !showKeyboard"
      >
        <li v-for="i,index in Password" :key="index"></li>
        <!-- <li class="unent" v-for="i in (6-Password.length)"></li> -->

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
import {encryptByDES,checkRates} from "@/libs/util"
import { Locale } from 'vant';
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
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
  Calendar,
  CountDown,
  NumberKeyboard,
  Dialog,
  NoticeBar,
} from "vant";
import {GetBlindBoxPageData,GetBlindboxWinningList,OpenBlindBox}  from "@/api"
export default {
     name:'BlindBox',
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
     [CountDown.name]:CountDown,
     [PasswordInput.name]:PasswordInput,
     [NumberKeyboard.name]:NumberKeyboard,
     [Dialog.name]:Dialog,
     [NoticeBar.name]:NoticeBar,
  },
  data(){
      return{
        token:'',
        isShowRule:false,
        notice:'',
        noticeList:'',
        MINT_auth:{},
        BlindBoxPageData:{BlindBoxRule:"",ItemList:[{},{},{}]},
        BoxtIndex:-1,
        Quantity:1,
        show:false,
        showKeyboard: false,
        Password: "default",
        Balance:'-',
        showVoucher:false,
        showWinning:false,
        WinningItem:[
            // {
            //     ActivityWinningId:"2f4bc938-db1f-4faf-8477-66e9f06b0019",
            //     Coin: "NF",
            //     Icon: "/Resource/Coin/20210624/e4a17030963b418db15bd59838759b3f.png",
            //     Quantity: 20,
            // },
            // {
            //     ActivityWinningId:"2f4bc938-db1f-4faf-8477-66e9f06b0019",
            //     Coin: "DOGE",
            //     Icon: "/Resource/Coin/20210623/ae104538edd64193be3a71c1f8f72f5b.png",
            //     Quantity: 10
            // },
            // {
            //     ActivityWinningId:"2f4bc938-db1f-4faf-8477-66e9f06b0019",
            //     Coin: "NF",
            //     Icon: "/Resource/Coin/20210624/e4a17030963b418db15bd59838759b3f.png",
            //     Quantity: 15
            // },
            // {
            //     ActivityWinningId:"2f4bc938-db1f-4faf-8477-66e9f06b0019",
            //     Coin: "NF",
            //     Icon: "/Resource/Coin/20210624/e4a17030963b418db15bd59838759b3f.png",
            //     Quantity: 15
            // },
            // {
            //     ActivityWinningId:"2f4bc938-db1f-4faf-8477-66e9f06b0019",
            //     Coin: "NF",
            //     Icon: "/Resource/Coin/20210624/e4a17030963b418db15bd59838759b3f.png",
            //     Quantity: 15
            // },
        ],
        pay:0,
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
       let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
    //   document.body.style.background = "#272A3F";
      this.getData();
      this.getBlindboxWinningList();
  },
  methods:{
      getData(){
          let data ={
              UserId:this.MINT_auth.UserId
          }
        GetBlindBoxPageData(data).then(r=>{
            console.log('获取盲盒界面数据')
            if(r.data.Code == 200 ){
                this.BlindBoxPageData = r.data.Data;
                this.Balance = this.BlindBoxPageData.Balance;
                this.BoxtIndex = 0
            }
            console.log(r)
        })
      },
      showPwd(){
          this.Password = '';
          if(this.Quantity>this.BlindBoxPageData.TodayRemaining){
              this.$toast(this.$t('blindBox.notBoxes'))
              return
          }
          this.showKeyboard = true;
          this.show = true
      },
      onInput(key) {
            this.Password = (this.Password + key).slice(0, 6);
            console.log(this.Password);
        },
        onDelete() {
        this.Password = this.Password.slice(0, this.Password.length - 1);
        },
      openBlindBox(){
           Dialog.confirm({
        title: this.$t('blindBox.Blind'),
        message: this.$t('blindBox.tips'),
      })
        .then(() => {
          // on confirm
           this.$toast.loading({
                duration: 0, // 持续展示 toast
                forbidClick: true,
            });
          let BoxtId = this.BlindBoxPageData.ItemList[this.BoxtIndex].ItemDetailId
          let data = {
            UserId:this.MINT_auth.UserId,//用户token
            BoxtId,//盲盒主键
            Quantity:this.Quantity,//抽奖次数(1或者5)
            WithdrawPassword:this.Password//交易密码
          }
          OpenBlindBox(data).then(r=>{
              console.log(r.data.Data)
              if(r.data.Code == 200 ){
                   this.$toast.clear();
                   this.getData()
                   const {Balance,IsVoucher,WinningItem}=r.data.Data
                   this.Balance  = Balance;
                   if(IsVoucher==1){
                       this.showVoucher = true
                   }else{
                       this.WinningItem = WinningItem;
                       this.showWinning =true;
                   }
              }
          })
            
        })
        .catch(() => {
          // on cancel
        });
      },
      toLink(url){
          this.$router.push({
              path:url
          })
      },
      getBlindboxWinningList(){
           let data ={
              UserId:this.MINT_auth.UserId
          }
          GetBlindboxWinningList(data).then(r=>{
              console.log("获取盲盒抽奖广播列表")
              console.log(r)
              if(r.data.Code == 200 ){
                  let arr =[];
                  r.data.Data.map(i=>{
                      arr.push(i.description+'      ')
                  })
                  this.notice = arr.join(' ')
                  this.noticeList = arr
              }
          })
      },
      
      showRule(){
        this.isShowRule = true;
    },
    replaceQuantity(str){
        let v = this.BoxtIndex>=0?this.BlindBoxPageData.ItemList[this.BoxtIndex].Quantity:'-'
        return str.replace(/{}/g,v)
    },
    replaceQuantity1(str){
        let v = this.BoxtIndex>=0?this.BlindBoxPageData.ItemList[this.BoxtIndex].Differences:'-'
        return str.replace(/{}/g,v)
    },
    escape2Html(str) {
         var arrEntities = { lt: "<", gt: ">", nbsp: " ", amp: "&", quot: '"' };
    let s =  str?str.replace(/&(lt|gt|nbsp|amp|quot);/gi, function (all, t) {
        return arrEntities[t];
      }):'';
      s = s?s.replace(/<img/g,"<img style='max-width:100%;height:auto;'"):'';
      s=  s?s.replace(/<p/g,"<p style='word-break:break-all;'"):'';
       return s 
    },
  },
  watch:{
      BoxtIndex(){
          let Amount =this.BlindBoxPageData.ItemList[this.BoxtIndex].Amount;
          let Discount =this.BlindBoxPageData.ItemList[this.BoxtIndex].Discount;
          if(this.Quantity==5){
              this.pay = Number(this.Quantity* Amount*Discount).toFixed(2) 
          }else{
             this.pay =Number(this.Quantity*Amount).toFixed(2) 
          }
      },
      Quantity(){
           let Amount =this.BlindBoxPageData.ItemList[this.BoxtIndex].Amount;
          let Discount =this.BlindBoxPageData.ItemList[this.BoxtIndex].Discount;
          if(this.Quantity==5){
              this.pay = Number(this.Quantity* Amount*Discount).toFixed(2) 
          }else{
              this.pay =Number(this.Quantity*Amount).toFixed(2) 
          }
      }
  }


}
</script>
<style lang="less" >
    .blind_box_wrap{
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
        .custom-image .van-empty__image {
            width: 480px;
            height: 298px;
            margin-top: unset;
        }
        .van-tabs__wrap{
            padding: 20px 0;
            // background: #0D0F1E;
        }
        .van-tabs__nav--card{
            border: unset;
        }
        .van-tab{
            width: 220px !important;
            flex: unset;
        }
        .van-tabs__nav--card {
            height: 80px;
            display: flex;
            justify-content: center;
        }
        .van-tabs__nav--card .van-tab{
            line-height: 80px;
            width: 130px;
        }
        .van-tabs--card>.van-tabs__wrap{
            height: 80px;
        }
        .van-tabs__nav{
            background-color: transparent;
        }
        .van-tabs__wrap{
            .van-tabs__nav--card .van-tab{
                color: #666;
                // border:2px solid #eee;
                background-color: #202135 !important;
                color: #fff !important;
                // margin: 0 10px;
                border-radius: 8px;
            }
            .van-tabs__nav--card .van-tab.van-tab--active{
                background: unset;
                background-color: #202135 !important;//@main_color;
                border:1px solid @main_color;

                color: #fff !important;
            }
        }
        .pic{
            display: flex;
            align-items: center;
            justify-content: center;
            #qrcode{
                img{
                    width: 100%;
                    height: 100%;
                }
            }
        }

    }
    .van-overlay {
        z-index: 10 !important;
    }
    .blind_box_wrap{
        .van-nav-bar{
            // background: #1C2134;
        }
        .van-nav-bar__right,.van-nav-bar__text,
        .van-ellipsis,.van-nav-bar .van-nav-bar__left .van-icon{
            color:#fff  !important;
        }
        .van-hairline--bottom::after{
            border-bottom: unset;
        }
        .van-notice-bar__wrap{
            //
        }
    }
</style>
<style lang="less" scoped>
    .rule{
        padding: 20px;
        max-height: 300px;
        overflow: scroll;
    }
    .blind_box_wrap{
        min-height: 100vh;
        background: linear-gradient( 145deg, #03C1A7 0%, #30AED4 100%);;
        padding:40px;
        position: relative;
        .r_bg ,.l_bg{
            position: absolute;
            top: 0;
            width: 30px;
            height: 800px;
        }
        // .r_bg{
        //     right: 0;
        //     background:  url(../../assets/images/common/guess/blind_box_top_r_bg.png) no-repeat ;
        //     background-size: 100% 100%;
        // }
        // .l_bg{
        //     left: 0;
        //     background:  url(../../assets/images/common/guess/blind_box_top_l_bg.png) no-repeat ;
        //     background-size: 100% 100%;
        // }
        >div.title{
            height: 150px;
            background: @panel_color;//url(../../assets/images/common/guess/blind_box_title.png) no-repeat ;
            background-size: cover;
            display: flex;
            align-items: center;
            justify-content: center;
            font-weight: bold;
            color: @main2_color;
            font-size: 38px;
            border-radius: 8px;
            // padding-bottom: 30px;
        }
        .notice{
            margin-top:20px;
            border-radius: 11px;
        }
        .panel{
            // height: 360px;
            padding:30px 40px;
            margin-top: 28px;
            border-radius: 8px;
            background: @panel_color;// url('../../assets/images/common/guess/blind_box_panel.png')no-repeat;
            background-size: 100% 100%;
            >.title{
                padding-bottom:20px;
                text-align: center;
                font-size: 28px;
                color: @font_text_main_color;
                font-weight: 600;
            }
            ul{
                display: flex;
                justify-content: space-between;
                li{
                    text-align: center;
                    display: flex;
                    align-items: center;
                    padding: 30px 20px;
                    justify-content: center;
                    position: relative;
                    border-radius: 10px;
                    color: @btn_main_color;
                    border: 6px solid @btn_main_color;
                    &.active{
                        background: @btn_main_color;
                        background-size: 100% 100%;
                        .pic{
                            background: url('../../assets/images/common/guess/sel_bg.png') no-repeat;
                            background-size: 100% 100%;
                            &+p{
                                color: @font_3_color;
                            }
                        }

                    }
                    // &::before{
                    //     content: "";
                    //     display: block;
                    //     width: 4px;
                    //     height: 100%;
                    //     position: absolute;
                    //     left: 0;
                    //     background: url('../../assets/images/common/guess/line.png')no-repeat;
                    //     background-size: 100% 100%;
                    // }
                    // &::after{
                    //     content: "";
                    //     display: block;
                    //     width: 4px;
                    //     height: 100%;
                    //     position: absolute;
                    //     right: 0;
                    //     background: url('../../assets/images/common/guess/line.png')no-repeat;
                    //     background-size: 100% 100%;
                    // }
                    width: 30%;
                    .pic{
                        width: 124px;
                        height: 124px;
                        margin: 30px auto 20px;
                        background: url('../../assets/images/common/guess/blind_box_sel.png')no-repeat;
                        background-size: 100% 100%;
                        &+p{
                            // margin-top: 10px;
                            font-size:28px ;
                            color: @btn_main_color;
                        }
                    }


                }
            }
            .tips{
                text-align: center;
                color: @font_text_main_color;
                font-size: 18px;
                position: relative;
                width: 100%;
                display: flex;
                align-items: center;
                justify-content: center;
                margin-top: 10px;
                b{
                    margin: 0 5px;
                }

            }
        }
        .limit{
            height: 80px;
            margin-top: 20px;
            display: flex;
            justify-content: space-between;
            padding:0 30px;
            align-items: center;
            background: rgba(255,255,255,0.1);
            // opacity: 0.1;// url('../../assets/images/common/guess/limit.png')no-repeat;
            background-size: 100% 100%;
            border-radius: 10px;
            >span{
                font-size: 26px;
                opacity: 1;
                &:nth-of-type(1){
                    color: @main4_color;
                }
                &:nth-of-type(2){
                    color: @font_3_color;
                }
            }

        }
        .sel{
            margin-top: 20px;
            display: flex;
            justify-content: space-between;
            flex: 0.8;
            >div{
                width: 48%;
                &.f{

                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    >div{
                        width: 50%;
                        text-align: center;
                        color: @font_3_color;
                        background: rgba(255, 255, 255, .2);
                        border-radius: 10px;
                        height: 64px;
                        // display: flex;
                        // justify-content: center;
                        // align-items: center;
                        // word-break: break-all;
                        // line-height: 64px;
                        // &.active{
                        //     &:nth-of-type(1){
                        //         display: flex;
                        //         align-items: center;
                        //         justify-content: center;

                        //     }
                        //     &:nth-of-type(2){
                        //         background: url('../../assets/images/common/guess/r_btn_unsel.png')no-repeat;
                        //         background-size: 100% 100%;
                        //     }

                        // }
                        &:nth-of-type(1){
                            display: flex;
                            align-items: center;
                            justify-content: center;
                            background: rgba(255, 255, 255, .2);//url('../../assets/images/common/guess/l_btn_unsel.png')no-repeat;
                            background-size: 100% 100%;
                            &.active{
                                // background: url('../../assets/images/common/guess/l_btn_sel.png')no-repeat;
                                // background-size: 100% 100%;
                            }
                        }
                        &:nth-of-type(2){
                            line-height: 32px;
                            margin-left: 10px;
                            // background: url('../../assets/images/common/guess/r_btn_unsel.png')no-repeat;
                            // background-size: 100% 100%;
                            // &.active{
                            //     background: url('../../assets/images/common/guess/r_btn_sel.png')no-repeat;
                            //     background-size: 100% 100%;
                            // }
                        }
                        &.active{
                            background: @panel_color;
                            color: @font_text_main_color;
                        }

                    }

                }
                &.r{
                    // background: #212437;
                    display: flex;
                    justify-content: space-between;
                    align-items: center;
                    padding: 0  10px;
                    >span{
                        font-size: 26px;
                        &:nth-of-type(1){
                            color: @main4_color;
                        }
                        &:nth-of-type(2){
                            color: @font_3_color;
                        }
                    }

                }
            }
        }
        .take_btn{
            margin-top: 28px;
            height: 156px;
            opacity: 1;
            // url('../../assets/images/common/guess/btn_wrap_bg.png')no-repeat;
            background-size: 100% 100%;
            display: flex;
            align-items: center;
            button{
                width: 80%;
                height: 100px;
                line-height: 100px;
                margin: 0 auto;
                background: @panel_color;
                background-size: 100% 100%;
                font-weight: 600;
                color: @btn_main_color;
                font-size: 28px;
                padding-bottom: 26px;


            }
        }
        .acc{
            height: 80px;
            margin-top: 20px;
            display: flex;
            justify-content: space-between;
            padding:0 30px;
            align-items: center;
            // background:#1D1F31;
            // background-size: 100% 100%;
            >span{
                font-size: 26px;
                color: @main4_color;
            }
        }
        >.btns{
            margin-top: 30px;
            height: 100px;
            line-height: 100px;
            display: flex;
            justify-content: space-between;
            >button{
                display: flex;
                align-items: center;
                justify-content: center;
                padding-bottom: 10px;
                color:@font_3_color;
                background: rgba(255, 255, 255, .5);
                border-radius: 20px;
                font-size: 28px;
                img{
                    width: 28px;
                    margin-right: 10px;
                }
                width: 45%;
                // color:#fff;
                &:nth-of-type(1){
                    // background: url('../../assets/images/common/guess/btn_l_bg.png')no-repeat;
                    // background-size: 100% 100%;
                }
                &:nth-of-type(2){
                    // background: url('../../assets/images/common/guess/btn_r_bg.png')no-repeat;
                    // background-size: 100% 100%;
                }

            }

        }
        .empty{
            height: 120px;
        }
        .bot_bg_start{
            z-index: -1;
            position: absolute;
            bottom: 0;
            left: 0;
            right: 0;
            width: 100%;
            height: 500px;
            img{
                width: 100%;
            }
        }
        .bot_bg{
            width: 100%;
            position: absolute;
            width: 100%;
            bottom: 0;
            right: 0;
            // height: 160px;
            margin-top: 30px;
            left: 0;
            img{
                position: absolute;
                bottom: 0;
                height: 128px;
                width: 100%;

            }
        }
        .succes{
            text-align: center;
            background: #50556F;
            padding: 30px;
            background: url('../../assets/images/common/guess/success_bg.png')no-repeat;
            background-size: 100% 100%;
            &.obtainbg{
                background: url('../../assets/images/common/guess/success_bg1.png')no-repeat;
                background-size: 100% 100%;
            }

            p{
                color :  #fff;
                font-size: 32px;
            }
            .coin{
                margin:40px 0 0;
                // height: 100px;
                ul{
                    display: flex;
                    justify-content: center;
                    li{
                        margin:0 10px;
                        img{
                            width: 90px;
                        }
                    }
                }
                .obtain{
                    ul{
                        display: flex;
                        margin: 10px;
                        justify-content: unset;
                        align-items: center;
                        font-size: 28px;
                        margin-left:25%;
                        li{
                            margin:0 10px;
                            color: #fff;
                            img{
                                width: 90px;
                            }
                        }
                    }

                }
            }
            .con{
                margin:50px 0 20px;
                height: 100px;
                display: flex;
                align-items: center;
                justify-content: center;
                background: url('../../assets/images/common/guess/voucher.png')no-repeat;
                background-size: 100% 100%;
                p{
                    color:#04A3F2;
                    font-weight: 600;
                    font-size: 30px;
                }
            }
            .line{
                img{
                    width: 100%;
                }
            }
            .tips{
                ul{
                    display: flex;
                    justify-content: center;
                    flex-wrap: wrap;
                    li{
                        margin:0 30px 10px 0;
                        // min-width: 33.33%;
                        color: #fff;
                    }
                }
                p{
                    height: 90px;
                    line-height: 90px;
                    width: 60%;
                    font-weight: 600;
                    margin: 0 auto;
                    font-size: 34px;
                    // border-bottom: 1px solid #363D55;
                }
            }
            .btn{
                margin-top: 40px;
                button{
                    width: 330px;
                    height: 84px;
                    background: #1679f8;
                    border-radius: 12px;
                    color: #fff;
                }
            }
        }
    }
</style>