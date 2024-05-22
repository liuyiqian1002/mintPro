<template>
  <div class="withdraw_record_wrap_main">
    <van-nav-bar
      left-text
      :title="$t('withdrawRecord.title')"
      left-arrow
      @click-right="$router.push('/withdraw_record.vue')"
      @click-left="$router.go(-1)"
    />
    <div class="withdraw_record_wrap">
      <van-empty
        v-show="isEmpty"
        :description="$t('withdrawRecord.noData')"
        class="custom-image"
        :image="emptyImg"
      />
      <van-list v-model="loading" :finished="finished" @load="onLoad">
        <div class="card" v-for="i in dataList">
          <ul>
            <li>{{ i.coin }}</li>
            <li>
              <!-- // （0-待到账、1-已到账、2-已退回） -->
              <span
                :class="[
                  { back: i.mark == 2 },
                  { done: i.mark == 1 },
                  { mc: i.mark == 0 },
                ]"
                >{{   withdrawState(i.mark) }}</span
              >
            </li>
          </ul>
          <ul>
            <li>
              <label> {{ $t("withdrawRecord.amount") }}</label>
              {{ i.amount }}
            </li>
            <li>
              <label>{{ $t("withdrawRecord.createdtae") }}</label>
              {{ $moment(i.createdtae).format("YYYY/MM/DD") }}
            </li>
          </ul>
          <ul>
            <li>
              <label>{{ $t("withdrawRecord.txhash") }}</label>
              {{ i.txhash }}
            </li>
          </ul>
        </div>
      </van-list>

      <!-- <div class="card">
                <ul>
                    <li>USDR</li>
                    <li >
                        <span class="back">已退回</span>

                    </li>
                </ul>
                <ul>
                    <li>
                        <label >数量</label>
                        20
                    </li>
                    <li>
                        <label >交易时间</label>
                        2020-03-27 19:37
                    </li>
                </ul>
                <ul>
                    <li>
                        <label >交易哈希</label>
                        20
                    </li>
                    
                </ul>
            </div>
            <div class="card">
                <ul>
                    <li>USDR</li>
                    <li >
                        <span class="done">已完成</span>

                    </li>
                </ul>
                <ul>
                    <li>
                        <label >数量</label>
                        20
                    </li>
                    <li>
                        <label >交易时间</label>
                        2020-03-27 19:37
                    </li>
                </ul>
                <ul>
                    <li>
                        <label >交易哈希</label>
                        20
                    </li>
                    
                </ul>
            </div>
            <div class="card">
                <ul>
                    <li>USDR</li>
                    <li >
                        <span class="mc">待到账</span>

                    </li>
                </ul>
                <ul>
                    <li>
                        <label >数量</label>
                        20
                    </li>
                    <li>
                        <label >交易时间</label>
                        2020-03-27 19:37
                    </li>
                </ul>
                <ul>
                    <li>
                        <label >交易哈希</label>
                        20
                    </li>
                    
                </ul>
            </div> -->
    </div>
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
  List,
  empty,
} from "vant";
import { GetPageUserTakeOutCoins } from "@/api";
export default {
  name: "withdrawRecord",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    [List.name]: List,
    [empty.name]: empty,
  },
  data() {
    return {
      MINT_auth: {},
      PageIndex: 0,
      dataList: [],
      loading: false,
      finished: false,
      refreshing: false,
      isEmpty: false,
      emptyImg: "",
    };
  },
  mounted() {
    this.emptyImg = require("../assets/images/common/empty_img.png");
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    // this.getData()
  },
  methods: {
    getData() {
      let data = {
        UserId: this.MINT_auth.UserId, //用户编号</br>
        PageSize: 10, //每页数</br>
        PageIndex: this.PageIndex, //当前页</br>
      };
      // dataList
      GetPageUserTakeOutCoins(data).then((r) => {
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
      withdrawState(val)  {
    let str = ''
        // （0-待到账、1-已到账、2-已退回）
    switch (val) {
        case 0:
            str = this.$t('base.accounted')
            break
        case 1:
            str = this.$t('base.arrived')
            break
        case 2:
            str =  this.$t('base.Returned')
            break
    }
    return str;
}
  },
};
</script>
<style lang="less" >
  .withdraw_record_wrap_main {
    background-color: @background-color;
  }

  .withdraw_record_wrap{
    .custom-image .van-empty__image {
      width: 480px;
      height: 298px;
      margin-top: unset;
    }
  }

</style>
<style lang="less" scoped>
  .withdraw_record_wrap {
    background-color: @bg_color;
    height: 100vh;
    padding: 20px;
    .card {
      background-color: @panel_color;
      color: @font_1_color;
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
            color: @font_f_color;
          }
          .back {
            color: #f56767;
          }
          label {
            color: @font_f_color;
          }
        }
      }
    }
  }
</style>