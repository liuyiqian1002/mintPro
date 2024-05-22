<template>
  <div class="reference_asset_wrap">
      <van-nav-bar

        :title="$t('referenceAsset.title')"
        :placeholder="true"
        left-arrow 
        fixed
        @click-right="nShow= true"
        @click-left="$router.go(-1)" 
      />
      <div class="content">
          <div class="card" v-for=" i in dataList " :key="i.symbolsid" @click="contractTrad(i)">
            <p class="title"> <span>{{i.basecurrency}}</span>   <img src="../assets/images/common/reference_back_right.png" alt=""></p>
                <ul>
                    <li>
                        <div>
                            <p>{{$t('referenceAsset.balance')}}(USDT)</p>
                            <p>{{i.balance?i.balance:'--'}}</p>
                        </div>
                    </li>
                    <li>
                        <div>
                            <p>{{$t('referenceAsset.stoplossprice')}}(USDT)</p>
                            <p>{{i.stoplossprice?i.stoplossprice:'--'}}</p>
                        </div>
                    </li>
                    <li>
                        <div>
                            <p>{{$t('referenceAsset.rofitlossratio')}}</p>
                            <p>{{i.rofitlossratio?i.rofitlossratio:'--'}}</p>
                        </div>
                    </li>
                </ul>
          </div>
          
      </div>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, } from 'vant'
import{GetUserFuturesAccountsList} from '@/api'
export default {
    name:'referenceAsset',
    components:{
    [NavBar.name]: NavBar,
    [Icon.name]: Icon,
    },
    data(){
        return{
            MINT_auth:{},
            dataList:[],
            time:""
        }
    },
    mounted(){
     let MINT_auth = localStorage.getItem("MINT_auth");
        MINT_auth = this.$decryptByDES(MINT_auth);
        this.MINT_auth = JSON.parse(MINT_auth);
            this.getData()
       this.time =   setInterval(_=>{
            this.getData()
        },6000)
    },
    methods:{
        getData(){
            let data ={
                UserId:this.MINT_auth.UserId,//用户Id</br>
            };
            GetUserFuturesAccountsList(data).then(r=>{
                console.log(r.data);
                if(r.data.Code == 200 ){
                    console.log(r.data);
                    this.dataList = r.data.Data;
                }
            })
        },
        contractTrad(i){
            this.$router.push({
                path:'/contract_trad',
                query:{
                    SymbolsId:i.symbolsid
                }
            })
        }
    },
    destroyed(){
        clearInterval(this.time)
    }
}
</script>
<style lang="less" >

</style>
<style lang="less" scoped>
    .reference_asset_wrap{
        background-color: @bg_color;
        height: 100vh;
        .content{
            padding: 20px;
            .card{
                background-color: @panel_color;
                border-radius: 24px;
                margin-bottom: 20px;
                padding: 20px;
                .title{
                    color: @black;
                    display: flex;
                    align-items: center;
                    justify-content: space-between;
                    padding: 10px 0;
                    font-weight: 500;
                    font-size: 28px;
                    img{
                        height: 32px;
                    }
                }
                ul{
                    display: flex;
                    justify-content: space-between;
                    li{
                        width: 33.33%;
                        p{
                            margin: 10px 0;
                            color: @font_color ;
                            font-size: 24px;
                        }
                        &:nth-of-type(3){
                            text-align: right;
                        }
                        &:nth-of-type(2){
                            text-align: center;
                        }
                    }
                }
            }
        }
    }
</style>