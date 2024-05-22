<template>
  <div class="entrust_wrap">
    <van-nav-bar
      :title="$t('entrust.title')"
      left-text
      left-arrow
      :placeholder="true"
      fixed
      @click-left="$router.go(-1)"
    />
    <div style="height: 1000px">
      <div class="cont">
        <van-tabs
          v-model="active"
          swipeable
          animated
          :offset-top="46"
          :sticky="true"
          color="#1989fa"
          @change="changeNav"
          type="card" 
          title-active-color="#fff"
        >
        
          <van-tab :title="$t('entrust.all')">
            <div class="position">
              <!-- <van-list
                    v-model="loading"
                    :finished="finished"
                    finished-text="没有更多了"
                    @load="onLoad"
                > -->
                 <div class="nav" @click="show = true">
                <ul>
                  <li>{{$t('entrust.Trading')}}</li>
                  <li>{{navTitle}} <img src="../assets/images/common/back_right.png" alt="" /></li>
                </ul>
              </div>
              <van-empty
                v-show="isPositionEntityEmpty"
                :description="$t('base.noData')"
                class="custom-image"
                :image="emptyImg"
              />
             <div class="card" v-for="i in PositionEntity">
                <div class="title">
                  <div>
                    <span>{{ i.BaseCurrency }}/USDT</span>
                    <!-- 0-开多、1-开空 -->
                    <span :class="[{'down-color':i.Flag==1},{'up-color':i.Flag==0}]">{{
                      i.Flag == 1 ? $t('contractTrad.empty') :$t('contractTrad.many')
                    }}</span>
                    <span :class="[{'down-color':i.Flag==1},{'up-color':i.Flag==0}]">{{ (i.IncomeRate*100).toFixed(2)+'%' }}</span>
                    <span>{{$t('contractTrad.incomeRate')}}</span>
                  </div>
                </div>
                <div>
                  <ul>
                    <li>
                      <p>{{$t('contractTrad.positionnum1')}}(USDT)</p>
                      <p>{{ i.PositionNum }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.createprice')}}(USDT)</p>
                      <p>{{ i.CreatePrice }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.income')}}(USDT)</p>
                      <p>{{ i.Income }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.positionnum')}}(USDT)</p>
                      <p>{{ i.PositionNum }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.NowPrice')}}(USDT)</p>
                      <p>{{ i.NowPrice }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.fee')}}(USDT)</p>
                      <p>{{ i.Margin }}</p>
                    </li>
                    <li>
                      <p>{{$t('contractTrad.leveragemultiple')}}({{$t('contractTrad.double')}})</p>
                      <p>{{ i.LeverageMultiple }}</p>
                    </li>
                     <li v-show="CountDownTime(i)>0">
                      <p>{{$t("base.CloseDate")}}</p>
                      <p> 
                        <van-count-down :time="CountDownTime(i)">
                          <template #default="timeData">
                          <span >{{ timeData.days >10?timeData.days:'0'+timeData.days}} {{$t('financial.day')}} </span>
                          <span >{{ timeData.hours>10?timeData.hours:'0'+timeData.hours }}</span>
                          <span >:</span>
                          <span >{{ timeData.minutes >10?timeData.minutes :'0'+timeData.minutes }}</span>
                          <span >:</span>
                          <span >{{ timeData.seconds>10?timeData.seconds:'0'+timeData.seconds}}</span>
                        </template>
                       </van-count-down>
                      </p>
                    </li>
                  </ul>
                  <div class="btns">
                    <button
                      class="main_btn"
                      @click="submitCloseFutures(i.OrderId)"
                    >
                      {{$t('contractTrad.create')}}
                    </button>
                  </div>
                </div>
              </div>
              <!-- </van-list> -->
            </div>
          </van-tab>
          <van-tab :title="$t('entrust.history')">
            <div class="position">
               <div class="nav" @click="show = true">
                <ul>
                  <li>{{$t('entrust.Trading')}}</li>
                  <li>{{navTitle}} <img src="../assets/images/common/back_right.png" alt="" /></li>
                </ul>
              </div>
              <van-empty
                v-show="isEmpty"
                description=""
                class="custom-image"
                :image="emptyImg"
              />
              <van-list
                v-model="loading"
                :finished="finished"
                :finished-text="$t('base.noMore')"
                @load="HistoryList"
              >
                <div class="card" v-for="i in CouplingOrdersList">
                  <div class="title">
                    <div>
                      <span>{{ i.basecurrency }}/USDT</span>
                      <span class="down-color">{{
                        i.flag == 1 ? $t('entrust.flag') : $t('entrust.flag1')
                      }}</span>
                      <span
                        >{{ i.closedate | dateTiem }}
                        {{ i.closedate | dateDay }}</span
                      >
                    </div>
                    <span class="state">{{$t('entrust.done')}}</span>
                  </div>
                  <div>
                    <ul>
                      <li>
                        <p>{{$t('entrust.positionnum1')}}(USDT)</p>
                        <p>{{ i.positionnum }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.createprice')}}(USDT)</p>
                        <p>{{ i.createprice }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.income')}}(USDT)</p>
                        <p>{{ i.income }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.positionnum')}}(USDT)</p>
                        <p>{{ i.positionnum }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.closeprice')}}(USDT)</p>
                        <p>{{ i.closeprice }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.fee1')}}(USDT)</p>
                        <p>{{ i.fee }}</p>
                      </li>
                      <li>
                        <p>{{$t('entrust.leveragemultiple')}}({{$t('entrust.double')}})</p>
                        <p>{{ i.leveragemultiple }}</p>
                      </li>
                    </ul>
                  </div>
                </div>
              </van-list>
            </div>
          </van-tab>
        </van-tabs>
      </div>
    </div>
      <van-popup v-model="show" position="bottom" round>
      <van-picker
        show-toolbar
        :columns="Symbols"
        @cancel="show = false"
        @confirm="onConfirm"
      />
    </van-popup>
  </div>
</template>

<script>
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
  Sticky,
  Dialog,
  List,
  CountDown
} from "vant";
import {
  GetUserFuturesHistoryList,
  GetUserPositionEntity,
  SubmitCloseFutures,
  GetSymbolsPageList,
} from "@/api";
export default {
  name: "entrust",
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
    [Dialog.name]: Dialog,
    [CountDown.name]: CountDown,
  },
  data() {
    return {
      active: 0,
      CouplingOrdersList: [],
      SymbolsId: "",
      MINT_auth: {},
      PageIndex: 0,
      loading: false,
      finished: false,
      refreshing: false,
      PositionEntity: [],
      isPositionEntityEmpty: false,
      isEmpty: false,
      emptyImg: "",
      time:"",
      show:false,
      Symbols:[],
      navTitle:''
    };
  },
  mounted() {
    this.emptyImg = require("../assets/images/common/empty_img.png");
    this.SymbolsId = this.$route.query.SymbolsId;
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    this.getUserFuturesHistoryList();
    this.getUserPositionEntity();
    this.getSymbolsPageList();
    this.time =  setInterval(_=>{
       this.getUserPositionEntity();
    },6000)
  },
  methods: {
      //region 行情 => 获取全部symbol的交易行情列表
    getSymbolsPageList() {
      let data = {
        PageIndex: 1,
        PageSize: 100,
        AreaId: "Default",
        Sidx: "SortCode",
        Sord: "desc",
        KeyWord: "Default",
        Flag: "2",
      };
      GetSymbolsPageList(data).then((r) => {
        console.log(r.data);
        if (r.data.Code == 200) {
          let arr = [];
          r.data.Data.DataList.map((_) => {
            arr.push({
              value: _.SymbolsId,
              text: _.BaseCurrency,
              QuoteCurrency: _.QuoteCurrency,
            });
            if(_.SymbolsId==this.SymbolsId){
              this.navTitle = _.BaseCurrency+'/'+_.QuoteCurrency
            }
          });
          this.Symbols = arr;
        }
      });
    },
    onConfirm(value, index) {
      console.log(value);
      this.show = false;
      this.SymbolsId = value.value
       this.navTitle = value.text+'/'+value.QuoteCurrency
      this.getUserPositionEntity()
      //   this.$toast(`当前值：${value}, 当前索引：${index}`);
    },
    //   region 合约交易 => 获取我的单个合约的持仓信息
    getUserPositionEntity() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户Id</br>
        SymbolsId: this.SymbolsId, //交易对主键</br>
      };
      GetUserPositionEntity(data).then((r) => {
        console.log(r.data);
        if (r.data.Code == 200) {
          this.PositionEntity = [r.data.Data];
          this.loading = false;
          this.isPositionEntityEmpty = false;
        } else {
          this.PositionEntity = [];
          this.finished = true;
          if (this.PositionEntity.length == 0) {
            this.isPositionEntityEmpty = true;
          } else {
            this.isPositionEntityEmpty = false;
          }
        }
      });
    },
    getUserFuturesHistoryList() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户主键<br/>
        SymbolsId: this.SymbolsId, //交易对主键<br/>
        PageSize: 10, //每页数</br>
        PageIndex: this.PageIndex, //当前页</br>
      };
      GetUserFuturesHistoryList(data).then((r) => {
        console.log(r.data);
        if (r.data.Code == 200) {
          this.CouplingOrdersList.push(...r.data.Data);
          this.loading = false;
        } else {
          this.finished = true;
          if (this.CouplingOrdersList.length == 0) {
            this.isEmpty = true;
          } else {
            this.isEmpty = false;
          }
        }
      });
    },
    changeNav(i) {
      console.log(i);
      this.PageIndex = 0;
      this.CouplingOrdersList = [];
      this.finished = false;
      if (i == 2) {
      }
    },
    onLoad() {
      setTimeout(() => {
        this.getUserPositionEntity();
      }, 1000);
    },
    HistoryList() {
      setTimeout(() => {
        this.PageIndex++;
        this.getUserFuturesHistoryList();
      }, 1000);
    },
      CountDownTime(i){
        // i.CloseDate i.NowDate 
        let NowDate = i.NowDate? i.NowDate.replace(/-/g,'/').split('.')[0]:'';
        let CloseDate = i.CloseDate? i.CloseDate.replace(/-/g,'/').split('.')[0]:"";
        let leftTimes= new Date(CloseDate) - new Date(NowDate);
        return leftTimes;
      },
    // 平仓
    submitCloseFutures(id) {
      let data = {
        UserId: this.MINT_auth.UserId, //用户ID</br>
        OrderId: id, //持仓订单主键<br/>
      };
      Dialog.confirm({
        title: this.$t('base.confirm'),
        message: this.$t('entrust.confirm'),
      })
        .then(() => {
          // on confirm
          SubmitCloseFutures(data).then((r) => {
            console.log(r.data);
            if (r.data.Code == 200) {
              this.$toast(r.data.Message);
              this.getUserPositionEntity();
            }
          });
        })
        .catch(() => {
          // on cancel
        });
    },
  },
  destroyed(){
    clearInterval(this.time)
  }
};
</script>
<style lang="less" >
.entrust_wrap{
  .van-hairline--top-bottom,
        .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
  .custom-image .van-empty__image {
     width: 480px;
     height: 298px;
     margin-top: unset;
   }
    .van-tabs__wrap{
        padding: 20px 0;
        background: #fff;
    }
    .van-tabs__nav--card{
        border: unset;
    }
    .van-tab{
        width: 190px !important;
        flex: unset;
    }
    .van-tabs__nav--card {
        height: 80px;
        display: flex;
        justify-content: center;
    }
    .van-tabs__nav--card .van-tab{
        line-height: 80px;
        width: 130px;
    }
    .van-tabs--card>.van-tabs__wrap{
        height: 80px;
    }
    .van-tabs__wrap{
          .van-tabs__nav--card .van-tab{
        color: #666;
        border:2px solid #eee !important;
        background-color: #F5F6FA !important;
        color: #909399 !important;
        // margin: 0 10px;
        // border-radius: 8px;
    }
        .van-tabs__nav--card .van-tab.van-tab--active{
            background: unset;
            background-color: #fff !important;
            color: rgb(25, 137, 250) !important;
        }
    }
}

</style>
<style lang="less" >
  body {
    background-color: @bg_color;
  }

  .entrust_wrap{
    background-color: @bg_color;
    .van-hairline--top-bottom,
    .van-hairline--bottom{
      &::after{
        content: '';
        border-bottom-width:0px;
        border: none !important;
      }
    }
    .custom-image .van-empty__image {
      width: 480px;
      height: 298px;
      margin-top: unset;
    }
    .van-tabs__wrap{
      padding: 20px 0;
      background: @background-color;
    }
    .van-tabs__nav--card{
      border: unset;
    }
    .van-tab{
      width: 190px !important;
      flex: unset;
    }
    .van-tabs__nav--card {
      height: 80px;
      display: flex;
      justify-content: center;
    }
    .van-tabs__nav--card .van-tab{
      line-height: 80px;
      width: 130px;
    }
    .van-tabs--card>.van-tabs__wrap{
      height: 80px;
    }
    .van-tabs__wrap{
      .van-tabs__nav--card .van-tab{
        color: #666;
        border:2px solid #eee !important;
        color: #909399 !important;
        background-color: @panel_color !important;
        // margin: 0 10px;
        // border-radius: 8px;
      }
      .van-tabs__nav--card .van-tab.van-tab--active{
        background: unset;
        background-color: @main2_color !important;
        color:@font_3_color !important;
      }
    }
  }

</style>
<style lang="less" scoped>
  .cont {
    // border-top: 10px solid #eee;
    .open_position {
      padding: 20px;
      .open_position_cont {
        display: flex;
        padding: 20px;
        > div {
          // .t;
          &.con_left {
            width: 60%;
            .title {
              display: flex;
              justify-content: space-between;
              height: 80px;
              > div {
                flex: 1;
                text-align: center;
                font-size: 28px;
                line-height: 80px;
                color: @sub-font-color;
                border: 1px solid #eee;
                &:nth-of-type(1) {
                  &.active {
                    background-color: @up-color;
                    color: #fff;
                  }
                }
                &:nth-of-type(2) {
                  &.active {
                    background-color: @down-color;
                    color: #fff;
                  }
                }
              }
            }
            .txt {
              margin-top: 20px;
              p {
                color: @minor-font-color;
                padding: 10px 0;
              }
              > div {
                margin: 20px 0;
                height: 80px;
                width: 100%;
                border-radius: 4px;
                display: flex;
                justify-content: space-between;
                border: 1px solid #eee;
              }
              .in {
                margin: 0 auto;
                input {
                  height: 100%;
                  text-align: center;
                }
              }
              .add,
              .minus {
                width: 80px;
                height: 80px;
              }
              .minus {
                background: url(../assets/images/common/iv_open_minus.png)
                no-repeat center;
                background-size: 40%;
              }
              .add {
                background: url(../assets/images/common/iv_open_plus.png)
                no-repeat center;
                background-size: 40%;
              }
            }
            .bot {
              .tips {
                color: @sub-font-color;
                padding: 10px 0;
              }
              .btns {
                height: 80px;
                line-height: 80px;
                display: flex;
                justify-content: space-between;
                &.bay {
                  .t;
                  .active {
                    background-color: @up-color;
                    color: #fff;
                  }
                }
                &.sell {
                  .active {
                    background-color: @down-color;
                    color: #fff;
                  }
                }
                button {
                  width: 48%;
                  border-radius: 4px;
                  background-color: #eee;
                  color: @sub-font-color;
                }
              }
            }
          }
          &.con_right {
            width: 40%;
            padding: 10px;
            p.bay {
              color: @up-color;
              font-size: 26px;
              font-weight: 500;
            }
            p.sell {
              color: @down-color;
            }
            p.sub-color {
              color: @sub-font-color;
            }
            ul {
              display: flex;
              color: @font_1_color;
              justify-content: space-between;
              &.title {
                color: @sub-font-color;
              }
              .bay {
                color: @up-color;
              }
              .sell {
                color: @down-color;
              }
              li {
                &:nth-of-type(2) {
                  color: @sub-font-color;
                }
              }
            }
          }
        }
      }
      > .btns {
        &.bay {
          button {
            background-color: @up-color;
          }
        }
        &.sell {
          button {
            background-color: @down-color;
          }
        }
      }
    }
    .position {
      padding: 20px;
      .nav{
        ul{
          height: 80px;
          display: flex;
          color: @font_1_color;
          justify-content: space-between;
          align-items: center;
          .bot_br;
          li{
            display: flex;
            align-items: center;
            font-size: 28px;
            img{
              margin-left: 15px;
              width: 20px;
            }
          }
        }

      }
      .card {
        margin-bottom: 20px;
        padding: 20px 0;
        .bot_br;
        > div.title {
          height: 60px;
          line-height: 60px;
          display: flex;
          align-items: center;
          justify-content: space-between;
          > div {
            span {
              &:not(:nth-last-of-type(1)) {
                font-size: 28px;
                margin-right: 10px;
                font-weight: 500;
              }
              &:nth-last-of-type(1),
              &.state {
                color: @sub-font-color;
              }
            }
          }
          .state {
            color: @sub-font-color;
          }
        }
        ul {
          display: flex;
          flex-wrap: wrap;
          li {
            width: 33.33%;
            margin: 10px 0;
            &:nth-of-type(3n) {
              text-align: right;
            }
            p {
              padding: 10px 0;
              font-size: 26px;
              &:nth-of-type(1) {
                color: @sub-font-color;
              }
              &:nth-of-type(2) {
                font-weight: 600;
              }
            }
          }
        }
        .btns {
          margin: 0 auto;
          width: 60%;
          button {
            height: 90px;
            line-height: 90px;
          }
        }
      }
    }
  }
</style>