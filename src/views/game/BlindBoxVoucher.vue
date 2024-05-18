<template>
  <div class="blind_box_record_wrap">
    <van-nav-bar
      :title="$t('blindBoxVoucher.title')"
      left-text
       :left-arrow='true'
      :placeholder="true" 
      fixed
      @click-left="$router.go(-1)"
    />
      <div class="content">
        <div class="top">
          <div>
            <p>{{$t('blindBox.Voucher')}}</p>
            <p> {{$t('blindBoxVoucher.Current')}}*{{baseData.Voucher}}</p>
          </div>
        </div>
        <van-empty v-show="isEmpty" :description="$t('base.noData')"    class="custom-image" :image="emptyImg" />
          <div class="card" v-for="i in dataList">
              <div class="l">
                <p>{{i.itemvalue}} {{i.itemname}}</p>
                <p>≈{{i.usdttotal}} USD</p>
              </div>
              <div class="r">
                <button @click="Exchange(i.itemdetailid)" :disabled='!(baseData.Voucher>0)' >{{$t('blindBox.exchange')}}</button>
              </div>
          </div>
      </div>
      </van-dialog>
      <van-dialog
      v-model="isSucces"
      :showConfirmButton='false'
      :closeOnClickOverlay='true'
      width='220'
    >
    <div class="succes">
        <img src="../../assets/images/common/success.png" alt="">
        <p>{{$t('blindBoxVoucher.successful')}}</p>
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
   GetBlindboxExchangePage,
   BlindBoxExchange
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
        emptyImg:'',
        baseData:{},
        isSucces:false
      }
  },
  mounted(){
       let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
        this.emptyImg = require('../../assets/images/common/empty_img.png');
         this.getData();
  },
  methods:{
    Exchange(id){
      Dialog.confirm({
        title: this.$t('blindBox.exchange'),
        message: this.$t('blindBoxVoucher.tips'),
      })
        .then(() => {
          // on confirm
           let data = {
              UserId:this.MINT_auth.UserId,
              ItemDetailId:id
            }
            this.$toast.loading({
                      duration: 0, // 持续展示 toast
                      forbidClick: true,
                  });
            BlindBoxExchange(data).then(r=>{
              this.$toast.clear();
              if(r.data.Code == 200 ){
                this.isSucces = true;
                this.getData()
              }
            })
        })
        .catch(() => {
          // on cancel
        });
     
    },
      getData(){
           let data ={
                UserId:this.MINT_auth.UserId,
          }
          GetBlindboxExchangePage(data).then(r=>{
              console.log(r)
              if (r.data.Code == 200) {
                this.baseData = r.data.Data
                this.dataList = r.data.Data.DataList;
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
            .top{
                height: 170px;
                background: url('../../assets/images/common/guess/voucher.png') no-repeat;
                background-size: 100% 100%;
                margin-bottom: 20px;
                display: flex;
                justify-content: center;
                align-items: center;
                color: #fff;
                >div{
                    text-align: center;
                    p{
                        &:nth-of-type(1){
                            color: #04A3F2;
                            font-size: 32px;
                            font-weight: 600;
                        }
                        &:nth-of-type(2){
                            margin-top: 10px;
                            font-size: 24px;
                        }
                    }
                }

            }
            .card{
                background: #fff;
                padding: 30px;
                border-radius: 10px;
                margin-bottom: 20px;
                display: flex;
                justify-content: space-between;
                align-items: center;
                >div{
                    p:nth-of-type(1){
                        color: #202021;
                        font-size: 36px;
                        font-weight: 500;
                    }
                    p:nth-of-type(2){
                        color: #828282;
                        font-size: 24px;
                    }
                    button{
                        width: 152px;
                        height: 64px;
                        opacity: 1;
                        background: #1679f8;
                        border-radius: 32px;
                        color: #fff;
                        font-size: 24px;
                        font-weight: 500;
                        &:disabled{
                            background: #D0E4FE;
                            color: #fff;
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