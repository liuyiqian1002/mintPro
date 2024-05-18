<template>
  <div class="market_wrap">
    <van-nav-bar
      :title="$t('market.title')"
      :placeholder="true"
      fixed
      @click-left="$router.push('/reference_asset')"
      @click-right="nShow = true"
    >
      <template #right>
        <van-icon
          :name="require('../assets/images/common/ic_search_black.png')"
          size="18"
        />
      </template>
    </van-nav-bar>
    <div>
        <van-tabs @change="changeNav" sticky swipeable animated  offset-top="46"
                  :border="false" color='#E0EDF5' line-width="40" title-active-color='#E0EDF5' >
            <van-tab  v-for="(i, index ) in navListL" :title="i.ItemValue" >
                <div class="content">
                     <!-- <van-list
                        v-model="loading"
                        :finished="finished"
                        finished-text="没有更多了"
                        @load="onLoad"
                    > -->
                      <ul v-for="(i , index )  in SymbolsList" @click="toKline(i)">
                        <li>
                            <div>
                                <p>{{i.BaseCurrency}}<span>/{{i.QuoteCurrency}}</span></p>
                                <p>24H {{i.Amount | formatUnit}}</p>
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
                      <!-- <ul>
                    <li>
                    <div>
                        <p>BTC<span>/USDT</span></p>
                        <p>24H量 27539</p>
                    </div>
                    </li>
                    <li>
                    <div>
                        <p>375.9193</p>
                        <p>$375.9193</p>
                    </div>
                    </li>
                    <li>
                    <div class="down">
                        -1.41%
                    </div>
                    </li>
                </ul> -->
                     <!-- </van-list> -->

                </div>
        </van-tab>
        </van-tabs>
    </div>
    <search @changeSt="closrSearch" Flag='2' @search='search' :nShow="nShow" />
    <mainFooter />
  </div>
</template>

<script>
import mainFooter from "@/components/mainFooter";
import search from "@/components/search";
import {GetSymbolsArea,GetSymbolsPageList} from '@/api'
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
  name: "market",
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
    search,
    mainFooter
  },
  data() {
    return {
      nShow: false,
      navListL:[{}],
      navIndex:0,
       loading: false,
      finished: false,
      refreshing: false,  
      SymbolsList:[],
      Time:'',
      AreaId:'Default',
      PageIndex:1,
      time:'',
      lang:''
    };
  },
  mounted(){
    this.lang  = localStorage.getItem('MINT_lang')
      this.getNav();
      this.getSymbolsPageList();
     this.time = setInterval(_=>{
        this.getSymbolsPageList()
      },6000);
      setTimeout(_=>{
        // 创建文本节点            
        let  html =` <ul class="nav">
                    <li>
                        ${this.$t('market.name')}
                    </li>
                    <li>
                       ${this.$t('market.closePrice')}
                    </li>
                    <li>
                       ${this.$t('market.gainRate')}
                    </li>
                </ul>`
                // $('.van-sticky').append(html)
                var para=document.createElement("div"); 
                para.id='Nav'  
              var element=document.getElementsByClassName("van-sticky");            
              element[0].appendChild(para); 
              document.getElementById('Nav').innerHTML=html;
      },500)

  },
  methods: {
     search(i){
        this.nShow= ! this.nShow;
       this.$toast.loading({
         duration: 0, // 持续展示 toast
          forbidClick: true,
      })
        setTimeout(_=>{
          this.$toast.clear()
          this.toKline(i)
        },2000)
      },
      getNav(){
        GetSymbolsArea({}).then(r=>{
            if(r.data.Code == 200 ){
                console.log(r.data);
                this.navListL = r.data.Data
            }
        })
      },
      changeNav(i){
         let ItemDetailId=  this.navListL[i].ItemDetailId
          console.log(ItemDetailId);
          this.AreaId = ItemDetailId;
          this.SymbolsList=[];
          this.PageIndex = 1;
          this.finished =false;
          clearInterval(this.time);
            this.getSymbolsPageList();
         this.time = setInterval(_=>{
            this.getSymbolsPageList();
          },6000)
      },

      onLoad(){
        setTimeout(() => {
        this.PageIndex ++;
        this.getSymbolsPageList();
      }, 1000);
      },
        //region 行情 => 获取全部symbol的交易行情列表
      getSymbolsPageList(){
        let data = {
          PageIndex:this.PageIndex,
          PageSize:100,
          AreaId:this.AreaId,
          Sidx:'SortCode',
          Sord:'desc',
          KeyWord:'Default',
          Flag:'2'
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
      gainRate(i){
        let str  = i.RisingValue>0?`+`:``;
        let num = (((i.ClosePrice - i.OpenPrice)/i.OpenPrice )*100).toFixed(2);
        return str+num
      },
    closrSearch() {
      this.nShow = !this.nShow;
    },
    toKline(i){
      localStorage.setItem('SymbolsIdTiem',JSON.stringify(i));
          localStorage.setItem('path','deal')
          this.$router.push({
            path:'/trad',
            query:{
              SymbolsId:i.SymbolsId
            }
          })
        },
  },
  destroyed(){
    console.log('定时器关')
    clearInterval(this.time);
  }
};
</script>
<style lang="less" >
    body {
        background-color: @bg_clor;
    }

    .van-hairline--bottom:after {
        border-bottom-width: 0px;
    }

    .market_wrap_main{
        background-color: @bg_clor;
    }

    .market_wrap {
        background-color: @bg_clor;

        .van-tabs__line {
            position: absolute;
            left: 0;
            color: @font_1_color;
            z-index: 1;
        }

        .van-tab__text {
            display: flex;
            justify-content: center;
            flex-direction: column;
            align-items: center;
            color: #E0EDF5;
        }
        .nav{
            background-color: #152439;
            display: flex;
            justify-content: space-between;
            padding: 20px;
            .bot_br;
            margin-top: -1px;
            li{
                font-size: 24px;
                color: #ccc;
            }
        }
        .van-tab--active{
            font-weight: bold;
            font-size: 38px;
        }
    }
</style>
<style lang="less" scoped>
    .market_wrap {
        height: 100vh;

        // .nav{
        //     background-color: #fff;
        //     display: flex;
        //     justify-content: space-between;
        //     padding: 20px;
        //     .bot_br;
        //     margin-top: -1px;
        //     li{
        //         font-size: 24px;
        //         color: @sub-font-color;
        //     }
        // }
        .content {
            padding: 20px 20px 120px;
            background-color: @bg_clor;
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
                                    color:#E0EDF5;
                                    span {
                                        font-size: 18px;
                                        color: #E0EDF5;
                                    }
                                }
                                &:nth-of-type(2) {
                                    font-weight: 400;
                                    // color: @minor-font-color;
                                    color: #1DD368;
                                }
                            }
                        }
                    }
                    &:nth-of-type(2) {
                        p {
                            &:nth-of-type(1) {
                                font-weight: 600;
                                font-weight: 600;
                                font-size: 36px;
                                color:#E0EDF5;
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