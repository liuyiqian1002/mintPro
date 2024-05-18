<template>
  <div class="blind_box_record_wrap">
    <van-nav-bar
      :title="$t('blindBoxRecord.title')"
      left-text
       :left-arrow='true'
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
      <div class="content">
        <van-empty v-show="isEmpty" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
        <van-list
          v-model="loading"
          :finished="finished"
          :finished-text="$t('base.noMore')"
          @load="onLoad"
        >
          <div class="card" v-for="i in dataList">
              <p class="title"><span>{{$moment(i.CreateDate).format('YYYY-MM-DD HH:mm:ss')}}</span> <span>{{i.Frequency}}{{$t('blindBox.Times')}}</span></p>
                <ul>
                    <li>
                        <label>{{$t('blindBoxRecord.pay')}}</label>
                        <div>{{i.Total}} USDT </div>
                    </li>
                    <li>
                        <label>{{$t('blindBoxRecord.currency')}}</label>
                        <div><span v-for="k in i.WinningDetail" >{{k.Quantity}} {{k.Coin}}   </span>
                        </div>
                    </li>
                </ul>
          </div>
        </van-list>
      </div>
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
   GameBlindboxWinningRecord
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
        MINT_auth:{},
        loading: false,
        finished: false,
        refreshing: false,
        PageIndex:0,
        dataList:[],
        isEmpty:false,
        emptyImg:''
      }
  },
  mounted(){
       let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        this.emptyImg = require('../../assets/images/common/empty_img.png');
  },
  methods:{
      onLoad() {
        setTimeout(() => {
            this.PageIndex++;
            this.getData();
        }, 1000);
        },
      getData(){
           let data ={
                UserId:this.MINT_auth.UserId,
                PageSize:20,//每页数
                PageIndex:this.PageIndex,//当前页   
          }
          GameBlindboxWinningRecord(data).then(r=>{
              console.log(r)
              if (r.data.Code == 200) {
                this.dataList.push(...r.data.Data);
                this.loading = false;
                console.log(this.dataList)
            } else {
                this.finished = true;
                if (this.dataList.length == 0) {
                this.isEmpty = true;
                } else {
                this.isEmpty = false;
                }
            }
          })
      }
  },
}
</script>
<style lang="less" >
    .blind_box_record_wrap{
        .custom-image .van-empty__image {
            width: 480px;
            height: 298px;
            margin-top: unset;
        }
    }
    .blind_box_record_wrap{
        .van-nav-bar{
            background: #1C2134;
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
    .blind_box_record_wrap{
        min-height: 100vh;
        background: #272A3F;
        padding:40px;
        position: relative;
        .empty{
            height: 120px;
        }
        .content{
            .card{
                background: #343751;
                padding: 30px;
                border-radius: 10px;
                margin-bottom: 20px;
                .title{
                    color: #fff;
                    display: flex;
                    justify-content: space-between;
                    span{
                        font-size: 26px;
                        &:nth-of-type(2){
                            color: #0CC2A4;
                        }
                    }
                }
                ul{
                    margin-top: 36px;
                    li{
                        margin-bottom: 10px;
                        display: flex;
                        label{
                            color:#6D7291;
                            font-size: 26px;
                            width: 33%;
                        }
                        div{
                            color:#FFF;
                            width: 67%;
                            span{
                                display: inline-block;
                                margin-right: 30px;
                            }
                        }
                    }

                }

            }
        }
    }
</style>