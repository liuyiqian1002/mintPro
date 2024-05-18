<template>
  <div class="mining_wrap">
    <van-nav-bar
      :title="$t('mining.title')"
      left-text
      :left-arrow='!token'
      @click-left="token?'':$router.go(-1)"
      @click-right="$router.push('/order')"
    >
    <template #right>
        <van-icon
          :name="require('../assets/images/common/mining_order.png')"
          size="18"
        />
      </template>
</van-nav-bar>
    <div class="avatar" v-show="userInfo.MiningMachineGrade !='N0'">
      <div class="pic">
        <img :src="avatar" alt="">
      </div>
      <div class="user_name">
        <span>{{userInfo.EnCode}}</span>
        <span class="tag">{{userInfo.MiningMachineGrade}} </span>
        <!-- <img :src="$config.imgPath+avatar" alt=""> -->
      </div>
    </div>
      <div class="section">
      <div class="card_wrap">
        <div class="card" v-for="(i ,index) in MachineList" :key="i.commodityid" @click="toDetail(i.commodityid)">
          <div class="pic">
            <img :src="$config.imgPath+i.filepath" alt="" />
          </div>
          <p class="title">{{i.fullname}}</p>
          <p class="price">{{$t('mining.price')}} <span>{{i.sellingprice}} USDT</span></p>
          <ul>
            <li>
              <span>{{$t('mining.leaseTerm')}}</span>
              <span>{{i.days}}{{$t('mining.Days')}}</span>
            </li>
            <li>
              <span>{{$t('mining.upperLimit')}}</span>
              <span>{{i.inventory}}{{$t('mining.platform')}}</span>
            </li>
            <li>
              <span>{{$t('mining.output')}}</span>
              <span>{{i.dailyoutput}} {{i.coin}}/{{$t('mining.Days')}}</span>
            </li>
            <li>
              <span>{{$t('mining.totalOutput')}}</span>
              <span>{{i.totaloutput}}{{i.coin}}</span>
            </li>
            <!-- <li>
              <span>{{$t('mining.HandlingFee')}}</span>
              <span>{{i.totaloutput}}{{i.coin}}</span>
            </li> -->
          </ul>
          <div class="btns">
            <button >{{$t('mining.RentNow')}}</button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import {encryptByDES} from "@/libs/util"
import { Locale } from 'vant';
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
import {
  GetMiningMachineList,
  GetEntityUser,
} from "@/api";
import {
  Icon,
  NavBar,
} from "vant";
export default {
    name:'mining',
    components: {
    [Icon.name]: Icon,
    [NavBar.name]: NavBar,
  },
    data(){
        return {
            MINT_auth:{},
            MachineList:[],
            userInfo:{UserGradeFilePath:''},
            avatar:'',
            token:'',
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

        this.getEntityUser();
        this.getMiningMachineList();
    },
    methods:{
      //用户信息
      getEntityUser(){
        let data = {
          UserId: this.MINT_auth.UserId,
        };
          GetEntityUser(data).then(r=>{
              if(r.data.Code == 200){
                  console.log(r.data.Data);
                  this.userInfo = r.data.Data;
                   this.avatar = r.data.Data.HeadIcon
            ? this.$config.imgPath + r.data.Data.HeadIcon
            : require("../assets/images/common/logo.png");
                  localStorage.setItem('userInfo',encryptByDES(JSON.stringify(this.userInfo)))
              }
          })
      },
        getMiningMachineList(){
        GetMiningMachineList({UserId :this.MINT_auth.UserId}).then(r=>{
          console.log(r);
            const {data} =r;
            if(data.Code == 200 ){
            this.MachineList = data.Data
            console.log(this.MachineList)
            }
        })
        },
         //去购买
     toDetail(id){
       let query = this.token?{
           cid:id,
           token:this.token
         }:{
           cid:id
         }
       this.$router.push({
         path:'/product_details',
         query
       })
     },

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
  }
}
</script>
<style lang="less" scoped>
  .mining_wrap{
    background-color: @background-color;
    height: 100vh;
    .avatar{
      padding: 20px;
      display: flex;
      align-items: center;
      .pic{
        width: 64px;
        height: 64px;
        border-radius: 50%;
        overflow: hidden;
        margin-right: 10px;
        img{
          width: 100%;
        }
      }
      .user_name{
        display: flex;
        align-items: center;
        color:#272E3F;
        .tag{
          display: block;
          // padding :10px;
          width: 100px;
          height: 50px;
          background:url(../assets/images/common/tag_bg.png) no-repeat;
          background-size: contain;
          color: #FFD791;
          text-align: center;
          line-height: 45px;
          margin-left: 10px;
          padding-right: 5px;
        }

      }
    }
    .section {
      padding: 0px 28px 180px;
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
        .card {
          background-color: @panel_color;
          color: @font_3_color;
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
            span{
              color: @main_color;
              font-size: 36px ;
            }
          }
          ul {
            padding: 0 20px;
            li {
              margin: 10px 0;
              > span {
                &:nth-of-type(2n + 1) {
                  color :@sub_f_color;
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
          .btns{
            text-align: center;
            padding: 20px 0;
            color: #fff;
            button{
              width: 90%;
              height: 50px;
              border-radius: 1rem;
              background: @main_color;
            }
          }
        }
      }
    }

  }
</style>