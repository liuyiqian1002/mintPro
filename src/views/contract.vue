<template>
  <div class="contract_wrap">
    <header>
      <van-nav-bar
        :placeholder="true"
        fixed
      >
        <template #left>
         <span style="color:#fff">{{ $t('contract.title')}}</span>
        </template>
        <template #right>
          <div class="nav_property" >
            <van-icon
            @click="$router.push('/reference_asset')"
              :name="require('../assets/images/common/contract_property.png')"
              size="18"
              
            />
<!--            <p>{{$t('contract.property')}}</p>-->
          </div>
          
          <van-icon
          @click="nShow= true"
            :name="require('../assets/images/common/ic_search_black.png')"
            size="16"
          />
        </template>
      </van-nav-bar>
      <div class="nav">
        <ul>
          <li>
            {{$t('contract.name')}}
          </li>
          <li>
            {{$t('contract.closePrice')}}
          </li>
          <li>
             {{$t('contract.gainRate')}}
          </li>
        </ul>
      </div>
    </header>
    <div class="content">
      <!-- <ul > -->
          <!-- <van-list
                    v-model="loading"
                    :finished="finished"
                    finished-text="没有更多了"
                    @load="onLoad"
                > -->
        <ul v-for="(i , index )  in SymbolsList" @click="toDetails(i)">
          <li>
            <div>
                <p>{{i.BaseCurrency}}<span>/{{i.QuoteCurrency}}</span></p>
                <p>24H   {{i.Amount | formatUnit}}</p>
            </div>
          </li>
          <li>
            <div>
              <p>{{i.ClosePrice}}</p>
              <p>≈${{i.Buy1Price | decimals(2)}}</p>
            </div>
          </li>
          <li>
            <div :class="[{'down':i.RisingValue<0},{'up':i.RisingValue>=0}]">
                {{gainRate(i)}}%
            </div>
          </li>
        </ul>
          <!-- </van-list> -->
    </div>
    <mainFooter />
        <search  @changeSt='closrSearch' Flag='3' @search='search' :nShow = nShow />
        <userInstructions @closeH5='closeH5' :nShow = showH5 />
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, Tab, Tabs ,Popup,List} from "vant";
import mainFooter from '@/components/mainFooter';
import search from '@/components/search';
import userInstructions from '@/components/userInstructions';
import {
  GetSymbolsPageList,
  GetEntityUser,
} from "@/api";
export default {
  name: "contract",
  components: {
    [NavBar.name]: NavBar,
    [Icon.name]: Icon,
    mainFooter,
    search,
    userInstructions,
    [Popup.name]: Popup,
    [List.name]:List,
  },
  data() {
    return {
        nShow:false,
        SymbolsList:[],
        showH5:false,
        MINT_auth:{},
        UserInfo:{},
        PageIndex:1,
        finished:false,
        loading:false,
        time:"",
        lang:""
    };
  },
  mounted(){
    this.lang  = localStorage.getItem('MINT_lang')
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    console.log(this.MINT_auth);
    this.getSymbolsPageList();
    this.time =  setInterval(_=>{
      this.getSymbolsPageList();
    },6000)
    this.getEntityUser();
  },
  methods:{
       getEntityUser() {
      let data = {
        UserId: this.MINT_auth.UserId,
      };
      GetEntityUser(data).then((r) => {
        console.log(r.data)
        // console.log(JSON.stringify(r.data.Data) );
        if (r.data.Code == 200) {
          this.UserInfo = r.data.Data;
          const { UserGrade } = r.data.Data;
          if(this.UserInfo.IsOpenFutures != 1){
            this.showH5 = true
          }
        }
      });
    },
      closrSearch(){
          this.nShow= ! this.nShow
      },
      search(i){
        this.nShow= ! this.nShow;
       this.$toast.loading({
         duration: 0, // 持续展示 toast
          forbidClick: true,
      })
        setTimeout(_=>{
          this.$toast.clear()
          this.toDetails(i)
        },2000)
      },
      closeH5(){
        this.$router.push({
          path:'/'
        })
          this.showH5= ! this.showH5
      },
         //region 行情 => 获取全部symbol的交易行情列表
      getSymbolsPageList(){
        let data = {
          PageIndex:this.PageIndex,
          PageSize:100,
          AreaId:'Default',
          Sidx:'SortCode',
          Sord:'desc',
          KeyWord:'Default',
          Flag:'3'
        };
        GetSymbolsPageList(data).then(r=>{
          console.log(r.data)
          if(r.data.Code == 200 ){
            this.SymbolsList = r.data.Data.DataList;
            let Symbols =  btoa(JSON.stringify(this.SymbolsList ))
            localStorage.setItem('Symbols',Symbols);
                this.loading = false;
                }else{
                    this.finished = true;
                    if(this.SymbolsList.length == 0 ){
                        this.isEmpty = true;
                    }else{
                        this.isEmpty = false;
                    }
                }
        })
      },
      toDetails(i){
        localStorage.setItem('contractSymbolsItem',JSON.stringify(i))
        this.$router.push({
          path:'/contract_trad',
          query:{
            id:i.SymbolsId
          }
        })
      },
       gainRate(i){
        let str  = i.RisingValue>0?`+`:``;
        let num = (((i.ClosePrice - i.OpenPrice)/i.OpenPrice )*100).toFixed(2);
        return str+num
      },
      // onLoad(){
      //   setTimeout(() => {
      //   this.PageIndex ++;
      //   this.getSymbolsPageList();
      // }, 1000);
      // },
  },
  destroyed(){
    console.log('定时器关')
    clearInterval(this.time )
  }
};
</script>
<style lang="less" >
  .contract_wrap{
    .van-nav-bar{
      line-height: unset;
    }
    .nav_property{
      margin-right:20px;
      text-align: center;
      color: @font_1_color;
      >p{
        padding: 0;
        margin: 0;
        line-height:unset;

      }
    }
    .van-nav-bar__text{
      font-size: 38px;
      font-weight: 500;
    }

  }

</style>
<style lang="less" scoped>
  .contract_wrap {
    background-color: @background-color;
    height: 100vh;
    header {
      position: fixed;
      top: 0;
      width: 100%;
      left: 0;
      .bot_br;
      .nav {
        padding: 20px;
        background-color: @panel_color;
        ul {
          display: flex;
          height: 60px;
          background-color: @panel_color;
          justify-content: space-between;
          align-items: center;
          li {
            width: 33.33%;
            font-size: 28px;
            color: @default-color;
            &:nth-of-type(2) {
              text-align: center;
            }
            &:nth-of-type(3) {
              text-align: right;
            }
          }
        }
      }
    }
    .content {
      background-color: @bg_clor;
      padding: 220px 20px 120px;
      ul {
        display: flex;
        justify-content: space-between;
        margin-bottom: 10px;
        padding: 10px 0;
        align-items: center;
        .bot_br;
        li {
          width: 33.33%;
          &:nth-of-type(1) {
            div {
              p {
                &:nth-of-type(1) {
                  font-weight: 500;
                  font-size: 32px;
                  color:@sub-font-color2;
                  span {
                    font-size: 18px;
                    color: @sub-font-color2;
                  }
                }
                &:nth-of-type(2) {
                  font-weight: 400;
                  // color: @minor-font-color;
                  color: @minor-font-color;
                }
              }
            }
          }
          &:nth-of-type(2) {
            p {
              &:nth-of-type(1) {
                font-weight: 600;
                font-size: 32px;
                color:@sub-font-color2;
              }
              &:nth-of-type(2) {
                font-weight: 600;
                color: @minor-font-color;
                font-size: 20px;
              }
            }
            text-align: center;
          }
          &:nth-of-type(3) {
            div {
              width: 180px;
              height: 60px;
              display: flex;
              align-items: center;
              justify-content: center;
              color: #fff;
              border-radius: 4px;
              &.up {
                background-color: @up-color;
              }
              &.down {
                background-color: @down-color;
              }
            }
            display: flex;
            justify-content: flex-end;
            text-align: right;
          }
        }
      }
    }
  }
</style>