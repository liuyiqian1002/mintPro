<template>
  <div class="guess_wrap">
    <van-nav-bar
      :title="$t('guess.title')"
      left-text
       :left-arrow='!token'
      :placeholder="true" 
      fixed
      :right-text="$t('guess.rule')"
      @click-left="$router.go(-1)"
      @click-right='showRule'
    />
    <van-sticky :offset-top="46" >
        <div class="game_type">
            <ul>
                <li v-for="(i,index) in gameTypeList" :key="i" @click="changeGameType(index)" :class="{'active':gameTypeIndex==index}">
                    {{i+$t('guess.min')}}
                </li>
            </ul>

        </div>
    </van-sticky>
    <div class="top_panel" ref="iframeBox">
        <iframe  ref="iframe" frameborder="0"></iframe>
        <!-- <div class="swipe_wrap">
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 90 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 901 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 901 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 901 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>
            <div class="item">
                <span>恭喜131****4201在竞猜中赢得 901 USDT奖励</span>
                <span class="time">20:40:10</span>
            </div>

        </div> -->
    </div>
    <van-notice-bar class="notice" v-show="notice" color="#FFFFFF" background="#1C2134"
        :text="notice"
    />
    <div class="guess_panel">
        <div class="title">
            <div class="left">
                <p>NO:{{CurrentProjectData.Period}}</p>
            </div>
            <div class="time">
                <van-count-down :time="time" millisecond @finish='finish' format="HH:mm:ss:SS">
                    <template #default="timeData">
                        <span class="block">{{ fetch(timeData.minutes,10)  }}</span>
                        <span class="block">{{ fetch(timeData.minutes,1)  }}</span>
                        <span class="colon">:</span>
                        <span class="block">{{ fetch(timeData.seconds,10)  }}</span>
                        <span class="block">{{ fetch(timeData.seconds,1)  }}</span>
                        <span class="colon">:</span>
                        <span class="block">{{ fetch(timeData.milliseconds ,100)}}</span>
                        <span class="block">{{ fetch(timeData.milliseconds ,10)}}</span>
                    </template>
                </van-count-down>

            </div>
        </div>
        <div class="sel">
            <div @click="BettingTarget=1" :class="['up',{'active':BettingTarget==1} ]">
                <div class="pic">
                </div>
                <p>{{Target(1)}}</p>

            </div>
            <div  @click="BettingTarget=2" :class="['down',{'active':BettingTarget==2} ]">
                <div class="pic">
                </div>
                <p>{{Target(2)}}</p>
            </div>
        </div>
        <div class="txt">
            <div class="label">
                {{$t('guess.GuessAmount')}}
            </div>
            <div >
                <input type="text" v-model="Amount" :placeholder="$t('guess.enterAmount')" >
               <span>USDT</span> 
            </div>
        </div>
        <div class="quick">
            <ul>
                <li v-for="(i ,index) in numList" :class="{'active':index==numIndex}" :key="i" @click="changeNum(index)">
                    {{i}}
                </li>
            </ul>
        </div>

        <div class="bot_tips">
            <div>
                <p>{{$t('guess.selection')}}<span>{{Target(BettingTarget )}}</span></p>
            </div>
            <div>
                <p> {{$t('guess.Payable')}}<span><span class="mc">{{Amount?Amount:'-'}}</span>USDT</span></p>
            </div>
        </div>
    </div>
    <div class="guess_btn">
        <button  @click="showPwd" :disabled='BettingTarget==0'>{{$t('guess.GuessBtn')}}</button>
    </div>
    <p class="tips"><span>{{$t('guess.Available')}}:</span><span>{{Balance>=0?Balance:'-'}} USDT</span></p>
      <div class="record">
          <div class="cont">
        <van-tabs
          v-model="active"
          swipeable
          animated
          type="card" 
          :offset-top="106"
          :sticky="true"
          @change="changeNav"
          title-active-color="#fff"
        >
          <van-tab :title="i" v-for="i in NavBar">
               <div class="open">
                <van-list
                    v-model="loading"
                    :finished="finished"
                    :finished-text="isEmpty?'':$t('base.noMore')"
                    @load="onLoad"
                >
                <van-empty v-show="active==0 && (gameLotteryRecord.length==0)&&isRes" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
                <van-empty v-show="active==1 && (BettingRecord.length==0)&&isRes" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />

                <div class="card" >
                    <p class="title" v-show="active==0 && (gameLotteryRecord.length!=0) ">{{$t('guess.LotteryRecord')}}</p>
                    <p class="title" v-show="active==1 && (BettingRecord.length!=0)">{{$t('guess.BettingRecord')}}</p>
                    <!-- 开奖记录 -->
                    <ul v-show="active==0" v-for="(i,index) in gameLotteryRecord" :key="index">
                        <li><span>NO:{{i.period}}</span> <span :class="[{up:i.lotteryresult==1},{down:i.lotteryresult==2}]">{{ Target(i.lotteryresult)}}</span></li>
                        <li><span><label>{{$t('guess.startprice')}}</label>{{i.startprice}}</span> <span><label>{{$t('guess.endprice')}}</label>{{i.endprice}}</span></li>
                    </ul>
                    <!-- 下注 -->
                    <ul v-show="active==1" class="betting_record" v-for="i in BettingRecord" :key="i.recordid">
                        <li><span><label>NO:{{i.period}}</label> <img v-show="i.deliverytype == 2" src="../assets/images/common/guess/FastDelivery.png" alt=""></span> <span ><label :class="{end:i.status==4}">{{status(i.status )}}</label></span></li>
                        <li>
                            <div>
                                <span><label>{{$t('guess.Guess')}}</label></span> 
                                <span :class="[{up:i.bettingtarget==1},{down:i.bettingtarget==2}]">{{ Target(i.bettingtarget)}}</span>
                            </div>
                            <div>
                                <span><label>{{$t('guess.Profit')}}</label></span> 
                                <span :class="{mc:i.profit>0}">{{i.profit?i.profit:'-'}}</span>
                            </div>
                        </li>
                        <li>
                            <div>
                                <span><label>{{$t('guess.amount')}}</label></span> 
                                <span>{{i.amount}} USDT</span>
                            </div>
                            <div>
                                <span><label>{{$t('guess.Results')}}</label></span> 
                                <span :class="[{up:i.lotteryresult==1},{down:i.lotteryresult==2}]">{{Target(i.lotteryresult) }}</span>
                            </div>
                        </li>
                        <li>
                            <div>
                                <span><label>{{$t('guess.startprice')}}</label></span> 
                                <span>{{i.startprice}}</span>
                            </div>
                            <div>
                                <span><label>{{$t('guess.endprice')}}</label></span> 
                                <span>{{i.endprice}}</span>
                            </div>
                        </li>
                        <li>
                            <div>
                                <span><label>{{$t('guess.time')}}</label></span> 
                                <span>{{$moment(i.createdate).format('YYYY-MM-DD HH:mm:ss')}}</span>
                            </div>
                        </li>
                        <div class="btns" v-show="i.status == 3">
                            <button @click="showFastDelivery(i.recordid)">{{$t('guess.Fast')}}</button>
                        </div>
                    </ul>
                </div>
                </van-list>
            </div>
          </van-tab>
        </van-tabs>
      </div>
    </div>
    <!-- 密码 -->
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitFuturesTransfer"
      show-cancel-button
    >
      <ul
        class="pwd_num"
        @click="showKeyboard = !showKeyboard"
      >
        <li v-for="i in Password"></li>
        <!-- <li class="unent" v-for="i in (6-Password.length)"></li> -->

      </ul>
    </van-dialog>
    <van-number-keyboard
      :show="showKeyboard"
      @input="onInput"
      @delete="onDelete"
      @blur="showKeyboard = false"
    />
    <!-- 游戏说明 -->
      <van-dialog
      v-model="isShowRule"
      :title="$t('guess.ruleTitle')"
    >
    <div class="rule" v-html="escape2Html(GamePageData.GameRule)">

    </div>
    </van-dialog>
      <van-dialog
      v-model="isSucces"
      :showConfirmButton='false'
      :closeOnClickOverlay='true'
      width='220'
    >
    <div class="succes">
        <img src="../assets/images/common/success.png" alt="">
        <p>{{$t('guess.successful')}}</p>
    </div>
    </van-dialog>
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
    GetGamePageData,
    GetCurrentProjectData,
    GetWinningBroadcastList,
    GameBetting,
    GetGameLotteryRecord,
    GameBettingRecord,
    FastDelivery
    } from "@/api"
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
export default {
    name:'guess',
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
        return {
            time:0,
            MINT_auth:{},
            GamePageData:{GameRule:""},
            CurrentProjectData:{},
            showKeyboard: false,
            show: false,
            Password: "",
            Amount:'',
            Balance:"",
            isShowRule:false,
            BettingTarget:0,
            NavBar:[],
            finished:false,
            gameLotteryRecord:[],
            BettingRecord:[],
            active:0,
            isEmpty:false,
            PageIndex:0,
            loading: false,
            isSucces: false,
            isInit:false,
            notice:'',
            token:'',
            isRes:false,
            numList:[
                1,5,20,50,100,200
            ],
            numIndex:-1,
            gameTypeList:[1,3,10],
            gameTypeIndex:0

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
        this.emptyImg = require('../assets/images/common/empty_img.png');
        this.NavBar= [this.$t('guess.LotteryRecord'),this.$t('guess.BettingRecord')];
        let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        let lang  = localStorage.getItem('MINT_lang')?localStorage.getItem('MINT_lang'):'zh-CN';
         let _clientHeight=document.documentElement.clientHeight;
        let height = (_clientHeight )
        this.$refs.iframe.style.width='100%';
        this.$refs.iframe.style.height='530px';
        this.$refs.iframe.src= this.$config.imgPath+`/Product/Kline?LanguageVersion=${lang}`;
        document.body.style.background = "#0D0F1E";
        this.getData()
        this.getCurrentProjectData()
        this.getWinningBroadcastList()
        this.getGameLotteryRecord()
    },
    methods:{
        changeGameType(i){
        this.gameTypeIndex=i 
        this.$toast.loading({
            duration: 0, // 持续展示 toast
            forbidClick: true,
        });
        this.gameLotteryRecord= [];
        this.BettingRecord = []
        this.getData()
        this.getCurrentProjectData()
        this.gameBettingRecord()
        this.getWinningBroadcastList()
        // this.getWinningBroadcastList()
        this.changeNav()
        // this.getGameLotteryRecord()
            setTimeout(_=>{
                this.$toast.clear()
            },1000)
        },
        changeNum(index){
            this.numIndex = index;
            this.Amount  = Number(this.Amount)+ Number(this.numList[index]) ;
        },
        //初始数据
        getData(){
            let data = {
                UserId:this.MINT_auth.UserId,
                GameType:this.gameTypeList[this.gameTypeIndex]
                }
            GetGamePageData(data).then(r=>{
                if(r.data.Code == 200 ){
                    this.GamePageData = r.data.Data;
                    console.log('初始数据')
                    console.log(this.GamePageData )
                    this.Balance =this.GamePageData.Balance
                    this.isInit =true;
                    this.time = this.GamePageData.TotalSecond*1000;
                }
            })
        },
        //获取游戏当前开奖期数相关数据
        getCurrentProjectData(){
            let data = {UserId:this.MINT_auth.UserId, GameType:this.gameTypeList[this.gameTypeIndex]}
            GetCurrentProjectData(data).then(r=>{
                console.log('获取游戏当前开奖期数相关数据')
                console.log(r)
                if(r.data.Code == 200 ){
                    this.CurrentProjectData = r.data.Data;
                }
            })
        },
           // 竞猜 => 获取中奖广播列表
        getWinningBroadcastList(){
            this.notice = ''
            let data ={UserId:this.MINT_auth.UserId,GameType:this.gameTypeList[this.gameTypeIndex]}
            GetWinningBroadcastList(data).then(r=>{
                if(r.data.Code == 200 ){
                    let noticeList = [];
                    let str =''
                    // alert(JSON.stringify(r.data.Data))
                    r.data.Data.map(_=>{
                       str = _.description
                        noticeList.push(str)
                    })
                    this.notice = noticeList.join('    ')
                }
                console.log(r)
            })
        },
     

        seconds(val){
            if(val){
              let  v =  val<10?('0'+val):val
               return v
            }
        },
         showPwd() {
      if (!checkRates(this.Amount)) {
        this.$toast(this.$t("exchaneg.AmountText"));
        return;
      }
       Dialog.confirm({
        title: this.$t("guess.GuessBtn"),
        confirmButtonText: this.$t("base.confirm"),
        cancelButtonText: this.$t("base.cancel"),
      })
        .then(() => {
           this.Password = "default";
            this.submitFuturesTransfer()
        })
        .catch(() => {
          // on cancel
        });
    //   this.show = true;
    //   this.showKeyboard = true;
     
    },
         submitFuturesTransfer(action, done) {
      if (!this.Password) {
        // this.$toast('密码不格式不正确');w
        return;
      }
      let data = {
        UserId: this.MINT_auth.UserId, //用户ID</br>
        GameProjectId: this.GamePageData.GameProjectId, //交易对主键编号<br/>
        BettingTarget: this.BettingTarget,//操作方向(Into-币币账户划入到合约账户、DrawOut-合约账户划出到币币账户)</br>
        Amount: this.Amount, //数量<br/>
        WithdrawPassword: this.Password, //支付密码<br/>
      };
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      console.log(data)
      GameBetting(data).then((r) => {
        this.$toast.clear();
        console.log(r)
        if (r.data.Code == 200) {
            this.Balance = r.data.Data.Balance;
            this.Amount ='';
            this.isSucces = true;
        }else{
            this.$toast(r.data.Message)
        }
      });
    },
    //下注记录
    gameBettingRecord(){
        let data = {
            UserId:this.MINT_auth.UserId,
            PageSize:10,//每页数
            PageIndex:this.PageIndex,//当前页
            GameType:this.gameTypeList[this.gameTypeIndex]
        }
        GameBettingRecord(data).then(r=>{
            console.log(r)
            this.finished = true;
             if(r.data.Code == 200 ){
                this.BettingRecord.push(...r.data.Data);
                this.isRes=true
                this.loading = false;
                    this.finished = false;
                }else{
                    this.isRes=true
                    this.finished = true;
                    if(this.BettingRecord.length == 0 ){
                        this.isEmpty = true;
                    }else{
                        this.isEmpty = false;
                    }
                }
        })
    },
    finish(){
        // alert(11)
        if(this.isInit){
            // alert(1)
            this.getData();
            this.getCurrentProjectData();
        }
    },
     changeNav(i){
      this.isEmpty= false
        this.gameLotteryRecord=[];
        this.BettingRecord=[];
        this.PageIndex=1;
        this.isRes = false
         this.finished =false;
         if(this.active==0){
            this.getGameLotteryRecord();
        }else{
            this.gameBettingRecord()
        }
      },
    //开奖记录
    getGameLotteryRecord(){
         let data = {
            UserId:this.MINT_auth.UserId,
            PageSize:10,//每页数
            PageIndex:this.PageIndex,//当前页
            GameType:this.gameTypeList[this.gameTypeIndex]
            
        }
        GetGameLotteryRecord(data).then(r=>{
            console.log('开奖记录')
            console.log(r)
            
            this.finished = true;
            if(r.data.Code == 200 ){
                this.gameLotteryRecord.push(...r.data.Data);
                this.loading = false;
                this.isRes=true
                this.finished = false;
                }else{
                    this.finished = true;
                    this.isRes=true
                    if(this.gameLotteryRecord.length == 0 ){
                        this.isEmpty = true;
                    }else{
                        this.isEmpty = false;
                    }
                }
        })
    },
    showFastDelivery(id){
        Dialog.confirm({
        title: this.$t("guess.FastTips"),
        confirmButtonText: this.$t("base.confirm"),
        cancelButtonText: this.$t("base.cancel"),
      })
        .then(() => {
            this.FastDelivery(id);
        })
        .catch(() => {
          // on cancel
        });
    },
    //快速交割
    FastDelivery(id){
         let data = {
            UserId:this.MINT_auth.UserId,
            RecordId:id,
        }
        FastDelivery(data).then(r=>{
            console.log('快速交割')
            console.log(r)
            if(r.data.Code == 200 ){
                this.PageIndex=1;
                this.BettingRecord = [];
                this.finish = true
                this.gameBettingRecord()
                this.$toast(r.data.Message)
            }
        })
    },
    onLoad(){
        setTimeout(() => {
        this.PageIndex ++;
        if(this.active==0){
            this.getGameLotteryRecord();
        }else{
            this.gameBettingRecord()
        }
      }, 2000);
      },
    showRule(){
        this.isShowRule = true;
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
        onInput(key) {
            this.Password = (this.Password + key).slice(0, 6);
            console.log(this.Password);
        },
        onDelete() {
        // this.Password = this.Password.slice(0, this.Password.length - 1);
        },
        fetch(val,m){
            // if(val){
               let n =  parseInt( (val/m)%10);
               return n
            // }
        },
        Target(val){
            let str = '';
            val==0?str='-':
                val==1?(str=this.$t('guess.Bullish')):
                val==2?(str= this.$t('guess.Bearish')):str='-';
                return str
        },
         status(val){
            // (1-投注中、2-待开奖、3-待结算、4-已结束)
            let str = '';
            val==0?str='-':
                val==1?(str=this.$t('guess.Betting')):
                    val==2?(str=this.$t('guess.drawn')):
                    val==3?(str=this.$t('guess.settled')):
                    val==4?(str=this.$t('guess.over')):'';
                return str
        }
    },
    watch:{
        // Amount(){
        //     let index = -1
        //     this.numList.map((_,i)=>{
        //         if(this.Amount == _){
        //             index= i;
        //         }
        //     })
        //     this.numIndex = index;
        // }
    },
    destroyed(){
         document.body.style.background = "#fff";
    }

}
</script>
<style lang="less" >
    .guess_wrap{
        .van-sticky--fixed{
            padding:0 30px;
        }
        .custom-image .van-empty__image {
            width: 480px;
            height: 298px;
            margin-top: unset;
        }
        .van-tabs__wrap{
            padding: 20px 0;
            background: @panel_color;
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
                background-color: @btn_main_color !important;
                color: @font_3_color !important;
                // margin: 0 10px;
                border-radius: 8px;
                border: none;
            }
            .van-tabs__nav--card .van-tab.van-tab--active{
                background: unset;
                background-color: @bg2_color !important;//@main_color;
                border:1px solid @main_color;

                color: @font_color !important;
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
    .guess_wrap{
        .van-nav-bar{
            background: @background-linear;
        }
        .van-nav-bar__right,.van-nav-bar__text,
        .van-ellipsis,.van-nav-bar .van-nav-bar__left .van-icon{
            color:#fff  !important;
        }
        .van-hairline--bottom::after{
            border-bottom: unset;
        }
    }
</style>
<style lang="less" scoped>
    .rule{
        padding: 20px;
        max-height: 300px;
        overflow: scroll;
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
    .guess_wrap{
        padding:0 30px 0;
        background: @background-color;
        .game_type{
            // background: rgb(13, 15, 30);
            ul{

                display: flex;
                flex-wrap: wrap;
                justify-content: space-between;
                li{

                    margin: 10px 0;
                    background: @bg2_color;
                    width: 33%;
                    height: 90px;
                    font-size: 34px;
                    display: flex;
                    align-items: center;
                    color: #6D7291;
                    justify-content: center;
                    &:nth-of-type(1){
                        &.active{
                            // background: #1679F8;
                            background: @btn_main_color;
                            background-size: 100% 100%;
                            color: @font_3_color;
                        }
                    }
                    &:nth-of-type(2){
                        &.active{
                            &.active{
                                background: url(../assets/images/common/guess/type_btn2.png) no-repeat;
                                background-size: 100% 100%;
                                color: @font_3_color;
                            }
                        }
                    }
                    &:nth-of-type(3){
                        &.active{
                            background: url(../assets/images/common/guess/type_btn3.png) no-repeat;
                            background-size: 100% 100%;
                            color: @font_3_color;
                        }
                    }
                    &:nth-of-type(1){
                        border-radius: 10px 0 0 10px;
                        overflow: hidden;
                    }
                    &:nth-of-type(3){
                        border-radius:  0 10px 10px 0;
                        overflow: hidden;
                    }

                }
            }
        }
        .top_panel{
            overflow:hidden;
            height: 530px;
            overflow-y:hidden;
            margin-bottom: 20px;
            padding: 20px ;
            // background: url(../assets/images/common/guess/notice_bg.png) no-repeat;
            // background-size: 100% 100%;
            // .swipe_wrap{
            //     max-height: 240px;
            //     overflow:scroll;
            // }
            // .item{
            //     display: flex;
            //     justify-content: space-between;
            //     padding: 5px 0;
            //     span{
            //         font-size: 26px;
            //         color:#6D7291 ;
            //     }
            // }
        }
        .notice{
            border-radius: 11px;
        }
        .guess_panel{
            margin-top: 20px;
            border-radius: 20px;
            background: @panel_color;
            overflow: hidden;
            .title{
                // background-color: #1C2134;
                border-bottom: 1px solid @main_color;
                display: flex;
                justify-content: space-between;
                padding: 20px;
                .left{
                    color:  @font_color;
                }
                .time{
                    .colon {
                        display: inline-block;
                        margin: 0 4px;
                        color: @font_color;
                    }
                    .block {
                        margin-left: 10px;
                        display: inline-block;
                        width: 40px;
                        color: #0CC2A4;
                        font-size: 26px;
                        text-align: center;
                        border: 2px solid @main_color;
                        // border-image: linear-gradient(180deg, #393f52 0%, #8991a7 52%, #393f52) 1 1;
                    }
                }

            }
            .sel{
                padding: 20px;
                background-color: @panel_color;
                display: flex;
                justify-content: center;
                >div{
                    // border: 1px solid #f00;
                    flex: 1;
                    text-align: center;
                    &.up{
                        text-align: center;
                        position: relative;
                        &::before{
                            content: '';
                            display: block;
                            width: 2px;
                            height: 55%;
                            // background-color: #363D55;
                            position: absolute;
                            right: 0;
                            top: 50%;
                            transform: translate(-50%,-50%);
                        }
                        .pic{
                            margin: 0 auto;
                            background: url(../assets/images/common/guess/up_un_sel.png) no-repeat;
                            background-size: contain;
                        }
                        p{
                            font-size: 26px;
                            color: #555767;
                        }
                        &.active{
                            .pic{
                                margin: 0 auto;
                                background: url(../assets/images/common/guess/up_sel.png) no-repeat;
                                background-size: contain;
                            }
                            p{
                                color: @up-color;
                            }

                        }
                    }
                    &.down{
                        text-align: center;
                        .pic{
                            margin: 0 auto;
                            background: url(../assets/images/common/guess/down_un_sel.png) no-repeat;
                            background-size: contain;
                        }
                        p{
                            font-size: 26px;
                            color: #555767;
                        }
                        &.active{
                            .pic{
                                margin: 0 auto;
                                background: url(../assets/images/common/guess/down_sel.png) no-repeat;
                                background-size: contain;
                            }
                            p{
                                color: @down-color;
                            }

                        }
                    }
                    .pic{
                        height: 150px;
                        width: 136px;

                    }
                }
            }
            .txt{
                padding:10px 20px;
                display: flex;
                justify-content: space-between;
                color: @font_text_main_color;
                height: 100px;
                align-items: center;
                background: @main3_color;
                >div{
                    &:nth-of-type(2){
                        width: 70%;
                        height: 60px;
                        display: flex;
                        justify-content: space-between;
                        align-items: center;
                        padding :0 10px;
                        input{
                            width: 100%;
                            height: 100%;
                            background-color: transparent;
                        }
                        span{
                            right: 10px;
                        }
                        background: @panel_color;
                        border-radius: 4px;
                        // border: 1px solid @main_color;
                    }
                }

            }
            .quick{
                padding:10px;
                ul{
                    display: flex;
                    flex-wrap: wrap;
                    justify-content: space-between;
                    li{
                        margin: 10px 0;
                        background: @bg2_color;
                        width: 31.5%;
                        height: 90px;
                        font-size: 34px;
                        display: flex;
                        align-items: center;
                        color: @font_color;
                        justify-content: center;
                        border-radius: 8px;
                        &.active{
                            background: @btn_main_color;
                            color: @font_3_color;
                        }

                    }
                }
            }
            .bot_tips{
                padding: 20px;
                // background-color: #1C2134;
                display: flex;
                justify-content: space-between;
                p{
                    color:#6D7291 ;
                    span{
                        color:@main2_color;
                        margin:0 10px;
                        &.mc{
                            .mc;
                            // margin:0 10px;
                        }
                    }
                }
            }
        }
        .guess_btn{
            margin-top: 20px;
            width: 100%;
            height: 90px;
            button{
                width: 100%;
                height: 100%;
                background: @btn_main_color;
                background-size:100% 100%;
                font-size: 28px;
                border-radius: 10px;
                color: @font_3_color;
                &:disabled{
                    color: @font_color;
                    background: @panel_color;
                    background-size:100% 100%;
                }
            }
        }
        .tips{
            margin-top: 20px;
            color: @font_color;
            display: flex;
            justify-content: space-between;
        }
        .record{

            margin-top: 40px;
            .open{
                color: @font_color;
                border-radius: 8px 8px 0 0;
                overflow: hidden;
                .card{
                    min-height: 200px;
                    .title{
                        padding: 30px;
                        font-size: 24px;
                        // background-color:#1C2134;
                    }
                    ul{
                        padding: 10px 30px;
                        &:nth-of-type(2n){
                            background-color: @bg_color;
                        }
                        &:nth-of-type(2n+1){
                            background-color: @bg2_color;
                        }
                        li{
                            display: flex;
                            justify-content: space-between;
                            padding: 10px 0;
                            span{
                                font-size: 26px;
                                color: @font_color;
                                display: flex;
                                align-items: center;
                                &.up{
                                    color: @up-color;
                                }
                                &.down{
                                    color: @down-color;
                                }
                                label{
                                    margin-right: 15px;
                                    font-size: 26px;
                                    color: @font_color;
                                }
                            }

                        }
                    }
                    ul.betting_record{
                        li:nth-last-of-type(1){
                            >div{
                                width: 100%;
                                display: flex;
                                span{
                                    display: block;
                                    width: 25%;
                                    white-space: nowrap;
                                }
                            }
                        }
                        li:not(:nth-last-of-type(1)){
                            .end{
                                // color: #F25959;
                            }
                            .mc{
                                color: @main_color;
                            }
                            // border: 1px solid #f00;
                            img{
                                width: 26px;
                                margin-left: 10px;
                            }
                            >div{
                                display: flex;
                                justify-content: space-between;
                                flex: 1;
                                >span{
                                    display: block;
                                    width: 50%;
                                    white-space: nowrap;
                                    // border: 1px solid #f00;
                                }

                            }
                        }
                        .btns{
                            text-align: center;
                            padding: 10px 0;
                            button{
                                background-color:@main_color;
                                height: 60px;
                                width: 220px;
                                color: @font_color;
                                border-radius: 8px;
                                font-size: 28px;
                            }
                        }

                    }
                }
            }
        }
        .succes{
            text-align: center;
            padding:80px;
            img{
                display: block;
                margin: 0 auto;
                width: 134px;
            }
            p{
                color :  #303133;
                font-size: 32px;
                padding-top:40px;
            }
        }
    }
</style>