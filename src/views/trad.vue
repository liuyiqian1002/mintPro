<template>
  <div class="trad_wrap">
     <van-nav-bar
      :title="title"
      left-text
      left-arrow
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
    <div class="ifrem_wrap" ref="iframeBox">
        <iframe ref="iframe" src=""  frameborder="0"></iframe>
        <div class="coins_info">
          <div class="title">
            <div class="pic"><img :src="$config.imgPath+ SymbolsData.Icon" alt=""></div>
            <div>
              {{SymbolsData.FullName}}
            </div>
          </div>
          <ul>
            <li>
              <label>{{$t('base.issuance')}}</label>
              <p>{{SymbolsData.TotalAmount||"-"}}</p>
            </li>
            <li>
              <label>{{$t('base.circulation')}}</label>
              <p>{{SymbolsData.InCirculation||"-"}}</p>
            </li>
            <li>
              <label>{{$t('base.RelTime')}}</label>
              <p>{{SymbolsData.IssueDate||"-"}}</p>
            </li>
            <li>
              <label>{{$t('base.paper')}}</label>
              <div>
                <p>
                  {{SymbolsData.WhitePaper||"-"}}
                  </p>
                  <img 
                   v-clipboard:copy="SymbolsData.WhitePaper"
                  v-clipboard:success="onCopy"
                  v-clipboard:error="onError"
                  v-show="SymbolsData.WhitePaper" src="../assets/images/common/copy_ico.png" alt="">
              </div>
            </li>
            <li>
              <label>{{$t('base.Website')}}</label>
              <div>
                <p>
                  {{SymbolsData.OfficialWebsite||"-"}}
                  </p>
                  <img 
                   v-clipboard:copy="SymbolsData.OfficialWebsite"
                  v-clipboard:success="onCopy"
                  v-clipboard:error="onError"
                  v-show="SymbolsData.OfficialWebsite" src="../assets/images/common/copy_ico.png" alt="">
              </div>
            </li>
          </ul>
          <div class="tips">
            <p class="title">{{$t('base.Intr')}}</p>
            <div class="cont">
              {{SymbolsData.Introduction}}
            </div>

          </div>

        </div>
    </div>
    <div class="btns" v-show="showBtn">
        <button class="main_btn bay" @click="toDeal(1)">{{$t("base.bay")}}</button>
        <button class="main_btn sell" @click="toDeal(2)">{{$t("base.sell")}}</button>
    </div>
  </div>
</template>

<script>
import{GetSymbolsEntity} from '@/api'
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
  Sticky
} from "vant";
export default {
    name:'trad',
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
  },
  data(){
      return {
          title:'',
          SymbolsId:'',
          SymbolsIdTiem:{},
          showBtn:false,
          SymbolsData:{Icon:''}
      }
  },
  mounted(){
       let fromPath =  localStorage.getItem('path');
       console.log(fromPath)
      // document.body.style.background='rgb(15, 21, 36)'
      this.title = '';
      this.SymbolsIdTiem =  localStorage.getItem('SymbolsIdTiem')? JSON.parse(localStorage.getItem('SymbolsIdTiem')):'';
        console.log(this.SymbolsIdTiem )
      let SymbolsId = this.$route.query.SymbolsId
    //    let search =  localStorage.getItem('search')?localStorage.getItem('search'):'';
    //    search = JSON.parse(atob(search) );
    //    search.map(_=>{
    //        if(_.SymbolsId == SymbolsId){
               this.title = this.SymbolsIdTiem.BaseCurrency+'/USDT'
    //        }
    //    })
    this.SymbolsId =SymbolsId;
    console.log(this.SymbolsIdTiem)
        setTimeout(_=>{
        let _clientHeight=document.documentElement.clientHeight;
        console.log(_clientHeight)
        let lang  = localStorage.getItem('MINT_lang')?localStorage.getItem('MINT_lang'):'zh-CN';
        let height = (_clientHeight )
        this.$refs.iframe.src= this.$config.imgPath+`/Product/Index/${this.SymbolsId}?LanguageVersion=${lang}`;
        this.$refs.iframe.style.height= (height-150)+'px';
        // this.$refs.iframeBox.style.height= (height-400)+'px';
        this.$refs.iframe.style.width='100%';
      },500)
       setTimeout(_=>{
         this.showBtn = (fromPath == 'deal');
       },1000)
       this.getSymbolsEntity();

  },
  methods:{
    getSymbolsEntity(){
      let data = {SymbolsId:this.SymbolsId}
      GetSymbolsEntity(data).then(r=>{
        console.log('>>>>>>>>>>>>')
        if(r.data.Code == 200){
          console.log(r)
          this.SymbolsData = r.data.Data;
        }
      })
    },
     onCopy(e) {
            this.$toast(this.$t('recommendFriend.copySeccess'))
        },
        onError(e) {
            this.$toast(this.$t('recommendFriend.copyErr'))
        },
      toDeal(type){
          this.$router.push({
              path:'/deal',
              query:{
                type , //委托方向(1-买入、2-卖出)
                SymbolsId:this.SymbolsIdTiem.SymbolsId
              }
          })
      }
  },
  destroyed(){
      document.body.style.background='#fff'
  },
  beforeRouteEnter(to, from, next) {
   next(vm=>{          //  这里的vm指的就是vue实例，可以用来当做this使用
      console.log(to)
      console.log(from)
    })
  }
}
</script>
<style lang="less" >
  .trad_wrap{

  }
</style>
<style lang="less" scoped>
  .trad_wrap{
    min-height: 100vh;

    .btns{
      position: fixed;
      display: flex !important;
      width: 100%;
      bottom: 0px;
      background-color:#0d1625; //rgb(15, 21, 36);
      padding: 20px 10px;
      left: 0;
      right: 0;
      margin: 0 auto;
      .main_btn{
        margin: 10px;
        font-size: 28px;
        &.bay{
          background-color: @up-color;
        }
        &.sell{
          background-color: @down-color;
        }
      }
    }
    .ifrem_wrap{
      &::-webkit-scrollbar {
        display: none;
      }
      iframe{

        &::-webkit-scrollbar {
          display: none;
        }

      }
    }
    .coins_info{
      min-height: 400px;
      padding:0 30px;
      background-color:#0d1625;
      padding-bottom: 200px;
      >.title{
        display: flex;
        align-items: center;
        >div{
          &:nth-of-type(1){
            width: 64px;
            height: 64px;
            overflow: hidden;
            margin-right: 28px;
            img{
              width: 100%;
            }
          }
          &:nth-of-type(2){
            font-size: 36px;
            color: #E0EDF5;
            font-weight: 500;
          }
        }
      }
      ul{
        margin-top: 40px;
        li{
          display: flex;
          padding-bottom: 30px;
          align-items: center;
          justify-content: space-between;
          label{
            color: @font_f_color;
            font-size: 28px;
            width: 30%;
          }
          >div{
            width: 60%;
            display: flex;
            align-items: center;
            img{
              margin-left: 15px;
              width: 24px;
              height: 24px;
            }
            p{
              text-align: right;
              font-size: 28px;
              color: @font_f_color;
              width: 100%;
              white-space: nowrap;
              overflow : hidden;
              text-overflow: ellipsis;
              // display: -webkit-box;
              -webkit-line-clamp: 1;
            }
          }

        }


      }
      .tips{
        .title{
          font-size: 36px;
          color: @font_1_color;
          font-weight: 500;

        }
        .cont{
          margin-top: 28px;
          font-size: 28px;
          font-weight: 400;
          color: @font_f_color;
        }
      }

    }
  }
</style>