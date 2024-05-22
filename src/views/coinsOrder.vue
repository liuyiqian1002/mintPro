<template>
  <div class="buy_coin_order" ref="buy_coin_order">
      <div class="top">
            <van-nav-bar
            class="nav"
                :left-arrow='true'
                @click-left="$router.go(-1)"
                >
                <template #title>
                   <span class="title">{{$t('Coin.PurchaseDet')}}</span>
                </template>
                <template #left>
                    <van-icon
                    :name="require('../assets/images/common/left-arrow.png')"
                    size="18"
                    />
                </template>
            </van-nav-bar>
            <div class="nav_bar" v-show="type==1">
                <div class="title">
                    <img v-if="dataItem.Status == 0" src="../assets/images/common/buy_coin_order_time.png" alt=""> 
                    <img v-if="dataItem.Status == 1" src="../assets/images/common/buy_coin_order_finish.png" alt=""> 
                    <img v-if="dataItem.Status == 2" src="../assets/images/common/buy_coin_order_close.png" alt=""> 
                    {{setStatus(dataItem.Status)}}</div>
                <div class="sub_title" v-show="dataItem.Status == 0">{{$t('Coin.surplus')}} <div class="countdown">  <van-count-down :time="dataItem.TotalSeconds*1000"   format="mm:ss" /> </div> {{$t("Coin.surplus1")}}</div>
                <div class="sub_title" v-show="dataItem.Status == 1">{{$t('Coin.Ordercompleted')}}</div>
                <div class="sub_title" v-show="dataItem.Status == 2">{{$t('Coin.Ordercanceled')}}</div>
            </div>
            <div class="nav_bar" v-show="type==2">
                <div class="title">
                    <img v-if="dataItem.Status == 0" src="../assets/images/common/buy_coin_order_time.png" alt=""> 
                    <img v-if="dataItem.Status == 1" src="../assets/images/common/buy_coin_order_finish.png" alt=""> 
                    <img v-if="dataItem.Status == 2" src="../assets/images/common/buy_coin_order_return.png" alt=""> 
                    {{setSellStatus(dataItem.Status)}}</div>
                <div class="sub_title" v-show="dataItem.Status == 0">{{$t('Coin.Orderwait')}}</div>
                <div class="sub_title" v-show="dataItem.Status == 1">{{$t('Coin.Ordercompleted')}}</div>
                <div class="sub_title" v-show="dataItem.Status == 2">{{$t('Coin.Orderreturned')}}</div>
            </div>
      </div>
    <div class="contnet" v-show="type==1">
       <p class="title">{{$t('Coin.PurchaseDet')}}</p>
       <p><span>{{$t('Coin.Payable')}}</span> <span>{{dataItem.Pay}} {{dataItem.LegalCurrency}}</span></p>
       <p><span>{{$t('Coin.receipt')}}</span> <span>{{dataItem.Buy}} {{dataItem.Coin}}</span></p>
       <p><span>{{$t('Coin.PurchaseTime')}}</span> <span>{{$moment(dataItem.CreateDate).format('YYYY-MM-DD HH:mm:ss')}}</span></p>
       <p><span>{{$t('Coin.Orderstatus')}}</span> <span :class="[{'cr':dataItem.Status==2},{'c6':dataItem.Status==1},{'cb':dataItem.Status==0}]">{{setStatus(dataItem.Status)}}</span></p>
    </div>
    <div class="contnet" v-show="type==2">
       <p class="title">{{$t('Coin.Saledetails')}}</p>
       <p><span>{{$t('Coin.number')}}</span> <span>{{dataItem.SerialNumber}} </span></p>
       <p><span>{{$t('Coin.Deduction')}}</span> <span>{{dataItem.Sell}} {{dataItem.Coin}}</span></p>
       <p><span>{{$t('Coin.Deserved')}}</span> <span>{{dataItem.Deserved}} {{dataItem.LegalCurrency}}</span></p>
       <p><span>{{$t('Coin.Orderstatus')}}</span> <span :class="[{'cr':dataItem.Status==2},{'c6':dataItem.Status==1},{'cb':dataItem.Status==0}]">{{setSellStatus(dataItem.Status)}}</span></p>
       <p><span>{{$t('Coin.PurchaseTime')}}</span> <span>{{$moment(dataItem.CreateDate).format('YYYY-MM-DD HH:mm:ss')}}</span></p>
       <p><span>{{$t('Coin.Collection')}}</span> <span v-html="escape2Html(dataItem.ReceiptAccount)"></span></p>
    </div>
        <div class="btns" v-show="dataItem.Status == 0 && type==1">
            <button class="cancel_btn" @click="cancelQuickBuyCoin">{{$t('Coin.Cancel')}}</button>
            <button class="main_btn" @click="toServe">{{$t('Coin.pay')}}</button>
        </div>
  </div>
</template>

<script>
import {GetBuyCoinOrderDetail,CancelQuickBuyCoin,GetSellCoinOrderDetail} from "@/api"
import {encryptByDES} from "@/libs/util"
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
import {
  Icon,
  NavBar,
  Locale,
  Dialog,
  Popup,CountDown
} from "vant";
export default {
    name:"buyCoins",
     components: {
        [Icon.name]: Icon,
        [NavBar.name]: NavBar,
        [Popup.name]: Popup,
        [Dialog.name]: Dialog,
        [CountDown.name]: CountDown,
    },
    data(){
        return {
            MINT_auth:{},
            type:'',
            id:"",
            dataItem:{}
        }
    },
  mounted(){
         let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        this.type = this.$route.query.type;
        this.id = this.$route.query.id;
        this.getData()
        

  },
  methods:{
      getData(){
          if(this.type == 1){
            this.getBuyCoinOrderDetail()
        }else{
            this.getSellCoinOrderDetail()
        }

      },
      getSellCoinOrderDetail(){
          var data = {
              PayId:this.id,
              UserId:this.MINT_auth.UserId
          }
          GetSellCoinOrderDetail(data).then(r=>{
              console.log(r.data)
              if(r.data.Code == 200 ){
                  this.dataItem = r.data.Data;
                  console.log(r.data.Data)
              }
          })
      },
      toServe(){
          this.$store.dispatch('setIsLogin', false);
           this.$router.push({
                      path:'/h5',
                      query:{
                          title:this.$t('Coin.pay'),
                          url:this.dataItem.PayUrl
                      }
                  })
      },
      cancelQuickBuyCoin(){
          var data = {
              UserId:this.MINT_auth.UserId,
              PayId:this.id
          }
           Dialog.confirm({
            title: this.$t("base.tips"),
            message: this.$t('Coin.cancellation'),
            confirmButtonText: this.$t("base.confirm"),
            cancelButtonText: this.$t("base.cancel"),
          })
            .then(() => {
                 this.$toast.loading({
                    duration: 0, // 持续展示 toast
                    forbidClick: true,
                    message: this.$t('login.loading'),
                });
                CancelQuickBuyCoin(data).then(r=>{
                    if(r.data.Code == 200 ){
                        this.$toast.clear();
                        this.getData()
                    }
                })
            })
            .catch(() => {
              // on cancel
            });
         
      },
      getBuyCoinOrderDetail(){
          var data = {
              PayId:this.id,
              UserId:this.MINT_auth.UserId
          }
          GetBuyCoinOrderDetail(data).then(r=>{
              console.log(r.data)
              if(r.data.Code == 200 ){
                  this.dataItem = r.data.Data;
                  console.log(r.data.Data)
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
      setSellStatus(val){
            var str = ''
            if(val == 0){
                str =this.$t('Coin.orderreviewed')
            }
            if(val == 1){
                str =this.$t('Coin.Received')
            }
            if(val == 2){
                str =this.$t('Coin.Returned')
            }
            return str

        },
      setStatus(val){
            var str = ''
            if(val == 0){
                str =this.$t('Coin.payment')
            }
            if(val == 1){
                str =this.$t('Coin.received')
            }
            if(val == 2){
                str =this.$t('Coin.canceled')
            }
            return str

        },
  }

}
</script>
<style lang="less">
    .buy_coin_order {
        background-color: @bg_color;
    }
</style>
<style lang="less" scoped >
.buy_coin_order{
    position: relative;
    .top{
        height: 400px;
        position: relative;
        background-color: @background-color;
        .nav{
            background: transparent;
            &.van-hairline--bottom{
                &:after{
                    border:unset;
                }
            }
            .title{
                color: #fff;
            }
        }
        .nav_bar{
            padding: 16PX;
            margin-top: 80px;
            font-size: 36px;
            font-weight: 500;
            color: rgba(255, 255, 255, .5);
            margin-right: 60px;
                .title{
                    font-size: 44px;
                    color: #fff;
                    display: flex;
                    align-items: center;
                    img{
                        width: 44px;
                        margin-right: 15px;
                    }
                }
                .sub_title{
                    font-size: 28px;
                    margin-top: 28px;
                    color: rgba(255, 255, 255, .5);
                    display: flex;
                    div{
                        padding: 0 10px;
                        color: #fff;
                    }
                }


        }
        .pic{
            position: absolute;
            width: 242px;
            right: 64px;
            bottom: 28px;
            img{
                width: 100%;
            }
        }
    }
       .contnet{
        padding: 40px ;
        background-color: #fff;
        border-radius:24px ;
        overflow: hidden;
        margin-top:-24px ;
        position: relative;
        >p{
            color:#333333;
            margin:40px 0 30px;
            &.title{
                font-size: 28px;
                font-weight: 500;
            }
            display: flex;
            justify-content: space-between;
            span{
                font-size: 30px;
                display: inline-block;
                &:nth-of-type(1){
                    color: #6D7278;
                    width: 40%;
                    
                }
                &:nth-of-type(2){
                    color: #272E3F;
                    width: 55%;
                    text-align:right;
                    &.c6{
                        color: #666666;

                    }
                   &.cr{
                        color: #F25959;

                    }
                   &.cb{
                        color: #1679F8;

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
            position: fixed;
            bottom: 90px;
            width: 100%;
            display: flex;

            button{
                flex: 1;
                margin: 10px;
                font-size: 36px;
                background-color: #1679F8;
                &.cancel_btn{
                    background-color: rgba(234, 240, 248, 1);
                    color: rgba(102, 102, 102, 1);
                }
            }
        }
        .tips_content{
            height: 53vh;
            padding: 0 30px;
            .title{
                position: relative;
                padding: 42px;
                display: flex;
                justify-content: center;
                align-items: center;
                font-size: 36px;
                color: #333333;
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
            button{
                display: block;
                margin: 10px auto;
                width: 100%;
                &:disabled{
                    background: rgba(22, 121, 248, .5);
                }

            }
        }


}
</style>