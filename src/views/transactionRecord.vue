<template>
  <div class="transaction_record_wrap">
    <van-nav-bar
      :title="$t('transactionRecord.title')"
      left-text
      left-arrow
      @click-left="$router.go(-1)"
    />
    <van-empty
      v-show="isEmpty"
      :description="$t('base.noData')"
      class="custom-image"
      :image="emptyImg"
    />
      <van-list v-model="loading" :finished="finished" @load="onLoad">
        <div class="card" v-for="i in dataList">
          <ul>
            <li>{{ i.coin }}</li>
            <li>
              <span
                :class="[
                  { back: i.operatdirection == 0 },
                  { done: i.operatdirection == 1 },
                ]"
                >{{  transactionState(i.operatdirection) }}</span
              >
            </li>
          </ul>
        <ul>
          <li>{{ $t("withdrawRecord.code") }}</li>
          <li>{{i.code}} </li>
        </ul>
        <ul>
          <li>{{ $t("withdrawRecord.account") }}</li>
          <li>{{i.account}} </li>
        </ul>
          <ul>
            <li>
              <label> {{ $t("withdrawRecord.amount") }}</label>
              {{ i.quantity }}
            </li>
            <li>
              <label>{{ $t("withdrawRecord.createdtae") }}</label>
              {{ $moment(i.createdate).format("YYYY/MM/DD") }}
            </li>
          </ul>
          <!-- <ul>
            <li>
              <label>{{ $t("withdrawRecord.txhash") }}</label>
              {{ i.txhash }}
            </li>
          </ul> -->
        </div>
      </van-list>
  </div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
  Picker,
  Popup,
  PasswordInput,
  NumberKeyboard,
  empty,
  List,
} from "vant";
import { GetPageUserTransfer ,GetPageUserTakeOutCoins} from "@/api";
export default {
  name: "transactionRecord",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    [empty.name]: empty,
    [List.name]: List,
  },
  data() {
    return {
      emptyImg: "",
      isEmpty: false,
      MINT_auth: {},
      dataList:[],
       MINT_auth: {},
      PageIndex: 0,
      loading: false,
      finished: false,
      refreshing: false,
    };
  },
  mounted() {
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    this.emptyImg = require("../assets/images/common/empty_img.png");
  },
  methods: {
    getData() {
      let data = {
        UserId:this.MINT_auth.UserId,//用户编号</br>
        PageSize:10,//每页数</br>
        PageIndex:this.PageIndex//当前页</br>
      };
      GetPageUserTransfer(data).then((r) => {
        console.log(r.data);
        if (r.data.Code == 200) {
          console.log(r.data);
          this.dataList.push(...r.data.Data.rows);
          this.loading = false;
        } else {
          this.finished = true;
          if (this.dataList.length == 0) {
            this.isEmpty = true;
          } else {
            this.isEmpty = false;
          }
        }
      });
    },
     onLoad() {
      setTimeout(() => {
        this.PageIndex++;
        this.getData();
      }, 1000);
    },
    transactionState(val)  {
    let str = ''
        // 收支方向(1-支出，0-收入)
    switch (val) {
        case 1:
            str = this.$t('base.expenditure')
            break
        case 0:
            str = this.$t('base.income')
            break
    }
    return str;
}
  },
};
</script>
<style lang="less" >
  body {
    background-color: @background-color;
  }
  .custom-image .van-empty__image {
    width: 480px;
    height: 298px;
    margin-top: unset;
  }
</style>
<style lang="less" scoped>
  .transaction_record_wrap{
    background-color: @bg_color;

    .card {
      padding: 20px;
      .bot_br;
      ul {
        display: flex;
        justify-content: space-between;
        height: 50px;
        line-height: 50px;
        &:nth-of-type(1) {
          font-weight: 500;
        }
        li {
          font-size: 28px;
          .done {
            color: @sub-font-color;
          }
          .back {
            color: #f56767;
          }
          label {
            color: @sub-font-color;
          }
        }
      }
    }

  }
</style>