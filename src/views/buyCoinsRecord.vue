<template>
  <div class="buy_coin_record">
        <van-nav-bar :title="$t('Coin.buyCoinRec')" fixed placeholder left-arrow @click-left="$router.go(-1)" />
        <div class="content">
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
            <div class="card" v-for="i in dataList" :key="i.payid" @click="toDetail(i.payid)">
                <div class="title">
                    <div> <span :class="[{'buy':type==1}]"> {{type==1?$t('Coin.buy'):this.$t('Coin.sell')}}</span>USDT</div>
                    <div :class="[{'cr':i.status==2},{'c6':i.status==1},{'cb':i.status==0}]"> {{setStatus(i.status) }}</div>
                </div>
                <div class="con">
                    <p><span>{{$t('Coin.Payable')}}</span><span>{{i.pay}} {{i.legalcurrency}}</span></p>
                    <p><span>{{$t('Coin.receipt')}}</span><span>{{i.buy}} USDT</span></p>
                </div>
                <p class="time">{{$moment(i.createdate).format('YYYY-MM-DD HH:mm:ss')}}</p>
            </div>
             </van-list>

        </div>
  </div>
</template>

<script>
import {GetPageBuyCoinOrder} from "@/api"
import { Popup,NavBar ,List,empty} from "vant";
export default {
    name:'buyCoinRecord',
    components: {
        [NavBar.name]: NavBar,
        [Popup.name]: Popup,
        [List.name]: List,
        [empty.name]: empty,
    },
    data(){
        return {
            MINT_auth:{},
            dataList:[],
            type:"1",
            PageIndex:0,
            loading: false,
            finished: false,
            refreshing: false,  
            emptyImg:"",
            isEmpty:false
        }
    },
    mounted(){
               this.emptyImg = require("../assets/images/common/empty_img.png");
          let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
            this.onLoad()
    },
    methods:{
        toDetail(id){
            this.$router.push({
                path:'/coinsOrder',
                query:{
                    id:id,
                    type:this.type
                }
            })
        },
        // 已到账 订单状态(0-待打款、1-已到账、2-已取消)
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
        onLoad(){
            this.finished = true;
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
            }
            GetPageBuyCoinOrder(data).then(r=>{
                console.log(r.data);
                if(r.data.Code ==200){
                    this.dataList.push(...r.data.Data);
                 this.loading = false;
                 this.finished = false
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
    }

}
</script>
<style lang="less" >
.buy_coin_record{
   .custom-image .van-empty__image {
         width: 480px;
         height: 298px;
         margin-top: unset;
       }
}
</style>
<style lang="less" scoped >
.buy_coin_record{
    min-height: 100vh;
    background-color: @bg_clor;
    .content{
        padding:24px 30px ;
        .card{
            background: #fff;
            padding: 30px;
            border-radius: 12px;
            margin-bottom: 20px;
            .title{
                display: flex;
                justify-content: space-between;
                >div{
                    &:nth-of-type(1){
                        font-size: 32px;
                        span{
                            margin-right: 10px;
                            font-weight: 500;
                        }
                        .buy{
                            color: rgba(12, 194, 164, 1);
                        }
                    }
                }
                    .c6{
                        color: #666666;

                    }
                    .cr{
                        color: #F25959;

                    }
                    .cb{
                        color: #1679F8;

                    }
            }
            .con{
                padding: 15px 0 ;
                border-bottom: 1PX solid #D9D9D9;
                p{
                    padding: 12px 0;
                    span{
                        &:nth-of-type(1){
                            color: #828282;
                            width: 200px;
                            display: inline-block;
                        }
                    }
                }
                &+p{
                    color: #828282;
                    padding: 28px 0;
                }
            }
        }

    }

}

</style>