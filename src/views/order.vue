<template>
  <div class="order_wrap">
    <van-nav-bar
      :title="$t('Order.title')"
      left-text
      left-arrow
      fixed
      placeholder
      @click-left="$router.go(-1)"
    >
  </van-nav-bar>
    <div class="content">
       <van-empty v-show="isEmpty" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
      <div class="section" v-show="!isEmpty">
        <van-list
          v-model="loading"
          :finished="finished"
          :finished-text="$t('base.noMore')"
          @load="onLoad"
        >
          <div class="card_wrap">
            <div class="card" v-for="(i, index) in MachineList" @click="Record(i.capitalpoolid)" :key="index">
              <div class="pic">
                <div :class="['tag',{'dis':i.remainingdays<0}]">{{i.remainingdays>0?( $t('Order.Due')+i.remainingdays+ $t('mining.Days')):$t('Order.Expired')}}</div>
                <img :src="$config.imgPath + i.filepath" alt="" />
              </div>
              <p class="title">{{ i.fullname }}</p>
              <p class="price">
                {{$t('mining.price')}} <span>{{ i.sellingprice }} USDT</span>
              </p>
              <ul>
                 <li>
              <span>{{$t('mining.leaseTerm')}}</span>
              <span>{{i.days}}{{$t('mining.Days')}}</span>
            </li>
            <li>
              <span>{{$t('mining.Produced')}}</span>
              <span>{{i.operatevalue}}{{i.coin}}</span>
            </li>
            <li>
              <span>{{$t('mining.output')}}</span>
              <span>{{i.dailyoutput}} {{i.coin}}/{{$t('mining.Days')}}</span>
            </li>
            <li>
              <span>{{$t('mining.totalOutput')}}</span>
              <span>{{i.totaloutput}}{{i.coin}}</span>
            </li>
            <li>
              <span>{{$t('mining.HandlingFee')}}</span>
              <span>{{i.fee}}{{i.coin}}</span>
            </li>
                <!-- <li>
                  <span>{{$t('mining.leaseTerm')}}</span>
                  <span>{{ i.days }}{{$t('mining.Days')}}</span>
                </li>
                <li>
                  <span>Produced</span>
                  <span>{{ i.operatevalue }}(ASIC)</span>
                </li>
                <li>
                  <span>yield</span>
                  <span>{{ i.dailyoutput }} ASIC/day</span>
                </li>
                <li>
                  <span>Total output</span>
                  <span>{{ i.totaloutput }}ASIC</span>
                </li> -->
              </ul>
              <div class="btns">
                <button @click.stop="toDetail(i.commodityid)"> {{$t('Order.Buyagain')}}</button>
              </div>
            </div>
          </div>
        </van-list>
      </div>
    </div>
    <!-- <mainFooter active="order"></mainFooter> -->
  </div>
</template>

<script>
// import mainFooter from "@/views/Footer";
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
} from "vant";
import {encryptByDES} from "@/libs/util"
import { GetMyMiningMachineOrder } from "@/api";
export default {
  name: "news",
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
  },
  data() {
    return {
      MachineList: [],
      PageIndex: 0,
      loading: false,
      finished: false,
      refreshing: false,
      isEmpty: false,
      MINT_auth:{},
      emptyImg:'',
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
     this.emptyImg = require('../assets/images/common/empty_img.png');
    let MINT_auth = localStorage.getItem("MINT_auth");
    this.MINT_auth = JSON.parse(this.$decryptByDES(MINT_auth));
  },
  methods: {
    getData() {
      GetMyMiningMachineOrder({UserId :this.MINT_auth.UserId, PageSize: 10, PageIndex: this.PageIndex }).then(
        (r) => {
          console.log(r);
          if (r.data.Code == 200) {
            this.MachineList.push(...r.data.Data);
            this.loading = false;
          } else {
            this.finished = true;
            if (this.MachineList.length == 0) {
              this.isEmpty = true;
            } else {
              this.isEmpty = false;
            }
          }
        }
      );
    },
    onLoad() {
      setTimeout(() => {
        this.PageIndex++;
        this.getData();
      }, 1000);
    },
    toDetail(id) {
      let query = this.token?{token:this.token,cid: id}:{cid: id}
      this.$router.push({
        path: "/product_details",
        query
      });
    },
    Record(id){
      let query = this.token?{token:this.token,cid: id}:{cid: id}
      this.$router.push({
        path: "/mineral_record",
        query
      });
    }
  },
};
</script>
<style lang="less" >
.order_wrap{
   .custom-image .van-empty__image {
         width: 480px;
         height: 298px;
         margin-top: unset;
       }
}
</style>
<style lang="less" scoped>
  .order_wrap {
    background: #f5f5f5;
    min-height: 100vh;
    .section {
      padding: 0 38px 180px;
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
      .card_wrap {
        display: flex;
        justify-content: space-between;
        flex-wrap: wrap;
        margin-top: 20px;
        .card {
          position: relative;
          .tag {
            width: 160px;
            height: 48px;
            line-height: 48px;
            position: absolute;
            right: 0;
            top: 0;
            margin: 0 auto;
            background: url(../assets/images/common/order_teg.png) no-repeat;
            background-size: cover;
            color: #fff;
            &.dis {
              background: url(../assets/images/common/order_teg_dis.png) no-repeat;
              background-size: cover;
            }
          }
          background-color: #fff;
          width: 48.5%;
          margin: 10px 0;
          //  padding: 10px;
          border-radius: 20px;
          .pic {
            text-align: center;
            padding-bottom: 20px;
            height: 200px;
            overflow: hidden;
            img {
              //  height: 100%;
              width: 100%;
            }
          }
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
                &:nth-of-type(2n + 1) {
                  color: @sub_f_color;
                  width: 120px;
                  // height: 35px;
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
              justify-content: space-between;
            }
          }
          .btns {
            text-align: center;
            padding: 20px 0;
            button {
              width: 90%;
              height: 50px;
              border-radius: 1rem;
              color: #fff;
              background: @main_color;
            }
          }
        }
      }
    }
  }
</style>