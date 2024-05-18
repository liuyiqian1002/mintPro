<template>
  <div class="acc_record_wrap">
      <van-nav-bar left-text  :title="$t('property.title')" left-arrow @click-left="$router.push('/property')" />
      <div class="top">
        <p class="title">{{coin}}</p>
        <div class="acc_info" >
         <div class="main_l">
              <p class="label">{{$t('property.usable')}}</p>
              <p class="">{{coinInfo.Balance| decimals(8)}}</p>
              <p class="label">≈{{coinInfo.CNYBalance | decimals(2)}}USD</p>
             
         </div>
         <div class="main_r">
              <p class="label">{{$t('property.Freeze')}}</p>
              <p>{{coinInfo.Freeze| decimals(8)}}</p>
            <!-- <button @click="$router.push('/recharge')">{{$t('property.recharge')}}</button> -->
            <!-- <button @click="$router.push('/withdraw_coin')">{{$t('property.withdraw')}}</button> -->
         </div>
        </div>
      </div>
      <div class="contnet">
          <p class="title">{{$t('property.record')}}</p>
           <van-empty
            v-show="isEmpty"
            :description="$t('base.noData')"
            class="custom-image"
            :image="emptyImg"
          />
            <van-list
                        v-model="loading"
                        :finished="finished"
                        :finished-text="isEmpty?'':$t('base.noData')"
                        @load="onLoad"
                    >
            <div class="card" v-for="(i ) in dataList">
                <ul>
                    <li>
                        <p class="">{{i.description}}</p>
                        <!-- <p>{{$t('property.quantity')}}</p> -->
                        <p>{{i.createdate}}</p>
                    </li>
                    <li></li>
                    <li>
                        <!-- <p>{{$t('property.createdate')}}</p> -->
                        <p>{{i.operatevalue}}</p>
                    </li>
                </ul>
            </div>
            </van-list>
         
      </div>
       <div class="btns">
                <button @click="toRecharge" class="main_btn">{{$t('property.recharge')}}</button>
                <button :disabled='coinInfo.IsWithdraw!=1' @click="$router.push('/withdraw_coin')" class="main_btn">{{$t('property.withdraw')}}</button>
            </div>
  </div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
  List,
  empty,
  Image,
} from "vant";
import { mapState } from "vuex"
import {GetPageUserCommodityAccountsDetail} from "@/api"
export default {
    name:"property",
    components:{
    [Icon.name]: Icon,
    [NavBar.name]: NavBar,
    [IndexBar.name]: IndexBar,
    [empty.name]: empty,
    [Image.name]: Image,
    [List.name]:List,
    },
    data(){
        return{
            coin:'',
            dataList:[],
            MINT_auth:{},
            PageIndex:0,
            loading: false,
            finished: false,
            refreshing: false,  
            coinInfo:{},
            emptyImg:'',
            isEmpty:false
        }
    },
    mounted(){
        this.emptyImg = require("../assets/images/common/empty_img.png");
         console.log(this.base.coinInfo)
         this.coinInfo = this.base.coinInfo;
         console.log(this.coinInfo)
        console.log(this.$route.query);
        this.coin=this.$route.query.coin;
          let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
          let coinInfo = localStorage.getItem("coinInfo");
        coinInfo = this.$decryptByDES(coinInfo);
        this.coinInfo = JSON.parse(coinInfo);
        this.MINT_auth = JSON.parse(MINT_auth);
    },
    methods:{
          onLoad(){
            setTimeout(() => {
                this.PageIndex ++;
                this.getData();
            }, 1000);
        },
        getData(){
            let data = {
                UserId:this.MINT_auth.UserId,//用户编号</br>
                PageSize:10,//每页数</br>
                PageIndex:this.PageIndex,//当前页</br>
                Coin:this.coin,//币种Code<br/>
            }
            GetPageUserCommodityAccountsDetail(data).then(r=>{
                console.log(r.data);
                if(r.data.Code ==200){
                    this.dataList.push(...r.data.Data.rows);
                 this.loading = false;
                }else{
                    this.finished = true;
                    if(this.dataList.length == 0 ){
                        this.isEmpty = true;
                    }else{
                        this.isEmpty = false;
                    }
                }
            })
        },
        toRecharge(){
            if(this.coinInfo.IsOnlineRecharge==1){
                this.$router.push({
                    path:'/recharge_qrcode',
                    query:{
                        coin:this.coin
                    }
                })
            }else{
                this.$router.push('/recharge')
            }

        }
    },
     computed: {
        ...mapState(["base"]),
    },

}
</script>
<style lang="less">
    .acc_record_wrap{
        background-color: @background-color;
        .van-nav-bar__title{
            color: #fff;
        }
        .van-hairline--bottom{
            &::after{
                content: '';
                border-bottom-width:0px;
                border: none !important;
            }
        }
        .van-nav-bar .van-icon{
            color: #fff !important;
        }
    }
</style>
<style lang="less" scoped>
    .acc_record_wrap{
        height: 1700px;
        .top{
            //    background-color: @main_color;
            height: 360px;
            position: relative;
            background: url(../assets/images/common/acc_record_bg.png) no-repeat;
            background-size: cover;
            padding-top: 50px;
            color: #fff;
            width: 95%;
            margin-left: 20px;
            z-index: 10;
            > .title{
                font-size: 48px;
                font-weight: 500;
                text-align: center;
            }
            .acc_info{
                margin-top: 20px;
                >div{
                    flex: 1;
                    text-align: center;
                }
                display: flex;
                justify-content: space-between;
                // align-items: center;
                .main_l{
                    position: relative;
                    font-size: 30px;
                    &::before{
                        position: absolute;
                        right: 0;
                        content: '';
                        display: block;
                        height: 80%;
                        width: 1px;
                        background-color: #D8D8D8;
                    }
                    .label{
                        color:@font_1_color ;
                    }

                }
                .main_r{
                    font-size: 30px;
                    button{
                        display: block;
                        width: 120px;
                        height: 60px;
                        border-radius: 4px;
                        border: 2px solid #fff;/* px */
                        font-weight: 600;/* px */
                        margin: 30px 0;
                        background-color:transparent;
                    }
                }
                padding: 20px ;
                p{
                    padding: 5px 0;

                }
            }
        }
        .contnet{
            .title{
                font-size: 28px;
                color: @font_1_color;
                padding: 10px 0;
            }
            margin:20px;
            position: relative;
            top: 20px;
            z-index: 1;
            padding-bottom: 120px;

            .card{
                background-color: #fff;
                margin-bottom: 30px;
                border-bottom: 1px solid #eee;/* px */
                // padding: 20px;
                // box-shadow: 0px -2px 2px 2px #eee,   /*上边阴影  红色*/
                // -2px 0px 2px 2px #eee,   /*左边阴影  绿色*/
                // 2px 0px 2px 2px #eee,    /*右边阴影  蓝色*/
                // 0px 2px 2px 2px #eee;    /*下边阴影  黄色*/

                ul{
                    display: flex;
                    align-items: center;
                    background-color: @panel_color;
                    padding: 10px 0;
                    li{
                        width: 33.33%;
                        p{
                            &:nth-of-type(1){
                                font-weight: 500;
                                margin-bottom: 10px;
                                color: @font_1_color;
                                // color: @minor-font-color;
                            }
                            &:nth-of-type(2){
                                color:@font_f_color;
                            }
                        }
                        &:nth-of-type(2){
                            text-align: center;
                            justify-content: center;
                        }
                        &:nth-of-type(3){
                            text-align: right;
                            justify-content: center;
                        }
                    }
                }
            }
        }
        .btns{
            position: fixed;
            bottom: 0;
            width: 100%;
            display: flex;
            margin: 0;
            padding:20px;
            z-index: 10;
            >button{
                margin: 10px;
                width: 50%;

                &:nth-of-type(1){
                    background-color: @up-color;
                }
                &:nth-of-type(2){
                    background-color: @down-color;
                }
                &:disabled{
                    background-color: #666;
                }
            }
        }
    }
</style>