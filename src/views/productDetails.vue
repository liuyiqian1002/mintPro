<template>
  <div class="product_details_wrap">
    <van-nav-bar
    v-show="!token"
      :title="$t('productDetails.title')"
      left-text
      left-arrow
      :placeholder="true"
      fixed
      @click-left="$router.go(-1)"
    />
    <div class="top">
      <!-- 轮播 -->
      <div class="carousel">
        <div class="swiper-container">
          <div class="swiper-wrapper picslide">
            <div class="swiper-slide index_slide" v-for="i in ImgList">
              <!-- <a :href="i.ObjectValue"> -->
              <img :src="$config.imgPath + i.FilePath" alt="" />
              <!-- <img src="../assets/images/common/banner.png" alt="" /> -->
              <!-- </a> -->
            </div>
          </div>
          <!-- 分页器 -->
          <div class="swiper-pagination"></div>
        </div>
      </div>
    </div>
    <div class="content">
      <p class="title">{{$t('productDetails.details')}}</p>
       <table >
            <tr>
                <td>{{$t('mining.price')}}</td>
                <td class="mc">{{ MachineInfo.SellingPrice }} USDT</td>
            </tr>
            <tr>
                <td>{{$t('mining.leaseTerm')}}</td>
                <td>{{ MachineInfo.Days }}{{$t('mining.Days')}}</td>
            </tr>
            <tr>
                <td>{{$t('mining.upperLimit')}}</td>
                <td>{{ MachineInfo.Inventory }}{{$t('mining.platform')}}</td>
            </tr>
            <tr>
                <td>{{$t('mining.output')}}</td>
                <td>{{ MachineInfo.DailyOutput }} {{MachineInfo.Coin}}/{{$t('mining.Days')}}</td>
            </tr>
            <tr>
                <td>{{$t('mining.totalOutput')}}</td>
                <td>{{ MachineInfo.TotalOutput }}{{MachineInfo.Coin}}</td>
            </tr>
            <tr>
                <td>{{$t('mining.HandlingFee')}}</td>
                <td>{{ MachineInfo.Fee }}{{MachineInfo.Coin}}/{{$t('mining.Days')}}</td>
            </tr>
        </table>
        <p> {{$t('productDetails.FeeTips')}}</p>
      <div class="doc" v-html="escape2Html(MachineInfo.Description)"></div>
    </div>
    <div class="btns">
      <button @click="nShow = true">{{$t('mining.RentNow')}}</button>
    </div>
    <van-popup
      v-model="nShow"
      closeable
      :style="{ height: '46%', width: '100%' }"
      position="bottom"
      round
    >
      <div class="pop_content">
        <p class="pop_title"> {{$t('productDetails.rent')}}</p>
        <div class="con">
          <div class="l">
            <div class="pic">
              <img :src="$config.imgPath + MachineInfo.FilePath" alt="" />
            </div>
          </div>
          <div class="r">
            <p>{{MachineInfo.FullName}}</p>
            <p class="price">
             {{$t('mining.price')}} <span>{{ MachineInfo.SellingPrice }} USDT</span>
            </p>
            <ul>
              <li>
                <span>{{$t('mining.leaseTerm')}}</span>
                <span>{{ MachineInfo.Days }}{{$t('mining.Days')}}</span>
              </li>
              <li>
                <span>{{$t('mining.upperLimit')}}</span>
                <span>{{ MachineInfo.Inventory }}{{$t('mining.platform')}}</span>
              </li>
              <li>
                <span>{{$t('mining.output')}}</span>
                <span>{{ MachineInfo.DailyOutput }} {{MachineInfo.Coin}}/{{$t('mining.Days')}}</span>
              </li>
              <li>
                <span>{{$t('mining.totalOutput')}}</span>
                <span>{{ MachineInfo.TotalOutput }}{{MachineInfo.Coin}}</span>
              </li>
              <li>
                <span>{{$t('mining.HandlingFee')}}</span>
                <span>{{ MachineInfo.Fee }}{{MachineInfo.Coin}}/{{$t('mining.Days')}}</span>
              </li>
            </ul>
          </div>
        </div>
        <div class="btns">
          <div>
            <p>
              {{$t('productDetails.NeedPay')}}<span>{{ MachineInfo.SellingPrice }}</span
              > USDT
            </p>
          </div>
          <div>
            <button @click="showPwd(MachineInfo.CommodityId)">{{$t('mining.RentNow')}}</button>
          </div>
        </div>
      </div>
    </van-popup>
    <van-dialog
      v-model="show"
      :title="$t('login.password')"
      @confirm="submitOpenFutures"
      @close="closeDialog"
      show-cancel-button
    >
      <!-- <p class="pwd_tips" @click="showKeyboard = !showKeyboard"  v-show="!Password">{{$t('login.password')}}</p> -->
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
  NumberKeyboard
} from "vant";

import { GetMiningMachineInfo, BuyMiner } from "@/api";
import Swiper from "swiper";
export default {
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
    [List.name]: List,
    [NumberKeyboard.name]: NumberKeyboard,
  },
  data() {
    return {
      ImgList: ["", ""],
      nShow: !true,
      cid: "",
      MachineInfo: { Description: "" },
      MINT_auth:{},
      showKeyboard:false,
      show:false,
      Password:'',
      CommodityId:'',
      token:""
    };
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
  mounted() {
    this.token=this.$route.query.token;
    let MINT_auth = localStorage.getItem("MINT_auth");
    this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
    this.cid = this.$route.query.cid;
    this.getData();
  },
  methods: {
    getData() {
      GetMiningMachineInfo({ UserId :this.MINT_auth.UserId,CommodityId: this.cid }).then((r) => {
        const { data } = r;
        if (data.Code == 200) {
          console.log(data);
          this.ImgList = data.Data.ImgList;
          this.MachineInfo = data.Data;
          setTimeout(() => {
            var mySwiper = new Swiper(".swiper-container", {
              direction: "horizontal",
              slidesPerView: "auto",
              centeredSlides: true,
              watchSlidesProgress: true,
              paginationClickable: true,
              // slidesPerView: 1.1,
              spaceBetween: 10,
              loop: true,
              autoplay: {
                delay: 2500,
                disableOnInteraction: false,
              },
              // pagination: {
              //   el: ".swiper-pagination",
              // },
            });
          }, 500);
        }
      });
    },
    showPwd(id){
      this.nShow = false;
      this.showKeyboard =true;
      this.show =true;
      this.CommodityId = id;

    },
    buyMiner(id) {
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
        // message: this.$t('login.loading'),
      });
      BuyMiner({ Password:this.Password,UserId :this.MINT_auth.UserId,CommodityId: this.CommodityId }).then((r) => {
        const { data } = r;
        console.log(r);
        console.log(data);
        if (data.Code == 200) {
          this.$toast(data.Message);
          this.nShow = false;
          let query = this.token?{flag:'1',token:this.token}:{flag:'1'}
          this.$router.push({
                        path:'/success',
                        query
                    })
        }
      });
    },
    
    escape2Html(str) {
         var arrEntities = { lt: "<", gt: ">", nbsp: " ", amp: "&", quot: '"' };
    let s =  str.replace(/&(lt|gt|nbsp|amp|quot);/gi, function (all, t) {
        return arrEntities[t];
      });
      s = s.replace(/<img/g,"<img style='max-width:100%;height:auto;'");
      s=  s.replace(/<p/g,"<p style='word-break:break-all;'");
       return s 
      // var arrEntities = { lt: "<", gt: ">", nbsp: " ", amp: "&", quot: '"' };
      // return str.replace(/&(lt|gt|nbsp|amp|quot);/gi, function (all, t) {
      //   return arrEntities[t];
      // });
    },
    closeDialog(){
      this.Password='';
    },
    submitOpenFutures(action, done) {
      if (!this.Password) {
        return ;
      }
      this.buyMiner();
    },
     onInput(key) {
            this.Password= (this.Password+ key).slice(0, 6);
            console.log(this.Password);
        },
        onDelete() {
        this.Password= this.Password.slice(0, this.Password.length - 1);
        },
  },
};
</script>
<style lang="less" >
    .product_details_wrap{
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
    .product_details_wrap {
        background: #f5f5f5;
        min-height: 100vh;
        .top {
            background: #fff;
            border-radius: 0 0 0.6rem 0.6rem;
        }
        .carousel {
            position: relative;
            //   height: 14rem;
            overflow: hidden;

            img {
                width: 100%;
                height: 100%;
            }
            p{
                word-break:break-all;
            }

            .swiper-pagination-bullet-active {
                opacity: 1;
            }
        }
        .content {
            padding: 20px;
            margin-top: 20px;
            padding-bottom:180px ;
            > .title {
                position: relative;
                padding-left: 30px;
                font-size: 28px;
                font-weight: 500px;
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
            table{
                margin-top: 20px;
                width: 100%;
                text-align: center;
                border-collapse: unset;
                border-spacing: 1;
                border-radius: 10px;
                td,th{
                    height: 60px;
                    border: 1px solid #999;
                    text-align: center;
                    font-size: 28px;
                    &:nth-of-type(2){
                        width: 60%;
                        &.mc{
                            color: @main_color;
                        }

                    }
                }
            }
            .doc {
                margin-top: 20px;
                padding: 10px;
                img {
                    width: 100%;
                }
                table {
                    width: 100%;
                }
            }
        }
        .btns {
            position: fixed;
            bottom: 0px;
            width: 100%;
            left: 0;
            right: 0;
            padding: 20px 0;
            background: #fff;
            button {
                .base_btn;
                color: #fff;
                width: 90%;
                margin: auto;
            }
        }
        .pop_content {
            .pop_title {
                text-align: center;
                height: 100px;
                line-height: 100px;
                font-size: 30px;
            }
            .con {
                display: flex;
                padding: 20px;
                > div {
                    &.l {
                        width: 30%;
                        .pic {
                            // height: 160px;
                            width: 160px;
                            img {
                                width: 100%;
                                // height: 100%;
                            }
                        }
                    }
                    &.r {
                        width: 70%;
                        .title {
                            padding: 0 20px;
                            font-size: 30px;
                            font-weight: 500;
                        }
                        .price {
                            padding: 10px 20px;
                            span {
                                color: @main_color;
                                font-size: 36px;
                            }
                        }
                        ul {
                            padding: 0 20px;
                            li {
                                margin: 10px 0;
                                > span {
                                    margin-right: 50px;
                                    &:nth-of-type(2n + 1) {
                                        color: @sub_f_color;
                                        width: 120px;
                                        // text-align: justify;
                                        // text-justify: inter-ideograph;
                                        // &:after {
                                        //   content: "";
                                        //   display: inline-block;
                                        //   width: 100%;
                                        //   height: 0px;
                                        // }
                                    }
                                }
                                display: flex;
                                // justify-content: space-between;
                            }
                        }
                    }
                }
            }
            > .btns {
                display: flex;
                justify-content: space-between;
                align-items: center;
                > div {
                    &:nth-of-type(1) {
                        padding: 0 20px;
                        font-size: 28px;
                        span {
                            font-size: 48px;
                            margin: 0 10px;
                            font-weight: 500;
                        }
                    }
                    &:nth-of-type(2) {
                        width: 40%;
                        text-align: right;
                    }
                }
            }
        }
    }
</style>