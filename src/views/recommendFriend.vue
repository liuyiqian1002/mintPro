<template>
  <div class="recommend_friend_wrap">
    <van-nav-bar
      :title="$t('recommendFriend.title')"
      :placeholder="true"
      fixed
      left-arrow
      @click-left="$router.go(-1)"
    />
    <div>
      <!-- <div id="qrcode" ></div> -->
      <van-tabs
        v-model="active"
        @change="changeTab"
        swipeable
        animated
        type="card"
        :offset-top="46"
        :sticky="true"
      >
        <van-tab :title="$t('recommendFriend.nav')">
          <div class="invitation_code">
            <div class="main">
              <p class="title">{{$t('recommendFriend.codeTips')}}</p>
              <p class="code_num">{{ userInfo.EnCode }}</p>
              <p class="tips">{{$t('recommendFriend.tips')}}</p>
              <div class="pic">
              <qrCode :codeUrl="codeUrl" />
            </div>
            </div>
            <div class="btns">
                <button
                class="main_btn"
                v-clipboard:copy="codeUrl"
                v-clipboard:success="onCopy"
                v-clipboard:error="onError"
              >
                {{ $t("recommendFriend.copy") }}
              </button>
            </div>
            <!-- <p class="title">{{ $t("recommendFriend.QRDownload") }}</p>
            <div class="pic">
              <qrCode :codeUrl="codeUrl" />
            </div>
            <span>{{ userInfo.EnCode }}</span> -->
            <!-- <div class="btns">
              <button
                class="main_btn"
                v-clipboard:copy="codeUrl"
                v-clipboard:success="onCopy"
                v-clipboard:error="onError"
              >
                {{ $t("recommendFriend.copy") }}
              </button>
            </div> -->
          </div>
        </van-tab>
        <van-tab :title="$t('recommendFriend.nav1')">
          <van-empty
            v-show="isEmpty"
            :description="$t('base.noData')"
            class="custom-image"
            :image="emptyImg"
          />
          <div class="my_friend">
            <!-- <van-pull-refresh v-model="refreshing" @refresh="onRefresh"> -->
            <van-list
              v-model="loading"
              :finished="finished"
              :finished-text="isEmpty?'':$t('base.noMore')"
              @load="onLoad"
            >
              <ul v-for="(i, index) in friendList" :key="i.id">
                <li>
                  <div class="avatar">
                    <div class="pic">
                      <img
                        :src="
                          i.HeadIcon
                            ? $config.imgPath + i.HeadIcon
                            : defaultAvatar
                        "
                        alt=""
                      />
                    </div>
                    <div>
                      <p>{{ i.NickName }}</p>
                      <p>{{ i.CreateDate }}</p>
                    </div>
                  </div>
                </li>
                <li>
                  <p>{{ i.Mobile }}</p>
                </li>
              </ul>
            </van-list>
            <!-- </van-pull-refresh> -->
            <!--  -->
          </div>
        </van-tab>
      </van-tabs>
    </div>
  </div>
</template>

<script>
import QRCode from "qrcodejs2";
import qrCode from "@/components/qrCode";
import { GetUserTeamInfo } from "@/api";
import {
  Icon,
  NavBar,
  IndexBar,
  Tab,
  Tabs,
  empty,
  Image,
  List,
  Cell,
  CellGroup,
  PullRefresh,
} from "vant";
export default {
  name: "recommend_friend",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Tab.name]: Tab,
    [Tabs.name]: Tabs,
    [empty.name]: empty,
    [Image.name]: Image,
    [List.name]: List,
    [CellGroup.name]: CellGroup,
    [Cell.name]: Cell,
    [PullRefresh.name]: PullRefresh,
    qrCode,
  },
  data() {
    return {
      active: 0,
      codeUrl: "",
      userInfo: { EnCode: "" },
      PageIndex: 0,
      friendList: [],
      emptyImg: "",
      isEmpty: false,
      defaultAvatar: "",

      list: [],
      loading: false,
      finished: false,
      refreshing: false,
    };
  },
  mounted() {
    this.emptyImg = require("../assets/images/common/empty_img.png");
    let lang = localStorage.getItem('MINT_lang')
      ? localStorage.getItem('MINT_lang')
      : "zh-CN";
      let origin = location.origin
    let MINT_auth = localStorage.getItem("MINT_auth");
    this.userInfo = JSON.parse(this.$decryptByDES(MINT_auth));
    this.codeUrl = `${origin}/#/register?inviteCode=${this.userInfo.EnCode}&lang=${lang}`;
    this.defaultAvatar = require("../assets/images/common/logo.png");
  },
  methods: {
    // //region 用户 => 获取我的推荐列表信息
    getUserTeamInfo() {
      let data = {
        UserId: this.userInfo.UserId, //用户Id<br/>
        PageSize: 10, //每页数<br/>
        PageIndex: this.PageIndex, //当前页<br/>
      };
      GetUserTeamInfo(data).then((r) => {
        console.log(r);
        if (r.data.Code == 200) {
          this.friendList.push(...r.data.Data);
          this.loading = false;
        } else {
          this.finished = true;
          if (this.friendList.length == 0) {
            this.isEmpty = true;
          } else {
            this.isEmpty = false;
          }
        }
      });
    },
    // 切换
    changeTab(i, title) {
      // 首次
      if (this.friendList.length == 0 && i == 1) {
        //    console.log(111)
        //    this.PageIndex = 0;
        //    this.getUserTeamInfo();
      }
    },
    onLoad() {
      setTimeout(() => {
        if (this.refreshing) {
          this.friendList = [];
          this.refreshing = false;
        }
        this.PageIndex++;
        this.getUserTeamInfo();
      }, 1000);
    },
    onRefresh() {
      // 清空列表数据
      this.finished = false;

      // 重新加载数据
      // 将 loading 设置为 true，表示处于加载状态
      this.loading = true;
      this.onLoad();
    },
    onCopy(e) {
        this.$toast(this.$t('recommendFriend.copySeccess'))
    },
    onError(e) {
        this.$toast(this.$t('recommendFriend.copyErr'))
    },
  },
};
</script>
<style lang="less" >
.recommend_friend_wrap {
  .van-list__finished-text,
  .van-empty__description{
    color:@font_color;
  }
  .custom-image .van-empty__image {
         width: 480px;
         height: 298px;
         margin-top: unset;
       }
   .van-icon,
    .van-nav-bar__title{
    color: @font_color !important;
  }
    .van-nav-bar{
    background-color:transparent;
  }
  .van-hairline--top-bottom,
        .van-hairline--bottom{
          &::after{
            content: '';
            border-bottom-width:0px;
            border: none !important;
          }
        }
        .van-tabs__nav{
          background:transparent;
        }
  .van-tabs__wrap {
    padding: 20px 0;
    background:transparent;
  }
  .van-tabs__nav--card {
    border: unset;
  }
  .van-tab {
    width: 240px !important;
    flex: unset;
  }
  .van-tabs__nav--card {
    height: 80px;
    display: flex;
    justify-content: center;
  }
  .van-tabs__nav--card .van-tab {
    line-height: 80px;
    width: 130px;
  }
  .van-tabs--card > .van-tabs__wrap {
    height: 80px;
  }
  .van-tabs__wrap {
    .van-tabs__nav--card .van-tab {
      color: #fff;
      background-color: rgba(255, 255, 255, .2);;
      // margin: 0 10px;
      // border-radius: 8px;
      border: none;
    }
    .van-tabs__nav--card .van-tab.van-tab--active {
      background: unset;
      background-color: #fff;
      color:@main_color;
    }
  }
  .pic {
    display: flex;
    align-items: center;
    justify-content: center;
    #qrcode {
      img {
        width: 100%;
        height: 100%;
      }
    }
  }
}
</style>
<style lang="less" >
  .recommend_friend_wrap {
    background-color: @background-color;
    .van-list__finished-text,
    .van-empty__description{
      color:@font_color;
    }
    .custom-image .van-empty__image {
      width: 480px;
      height: 298px;
      margin-top: unset;
    }
    .van-icon,
    .van-nav-bar__title{
      color: @font_color !important;
    }
    .van-nav-bar{
      background-color:transparent;
    }
    .van-hairline--top-bottom,
    .van-hairline--bottom{
      &::after{
        content: '';
        border-bottom-width:0px;
        border: none !important;
      }
    }

    .van-tabs__wrap {
      padding: 20px 0;
    }
    .van-tabs__nav--card {
      border: unset;
    }
    .van-tab {
      width: 190px !important;
      flex: unset;
    }
    .van-tabs__nav--card {
      height: 80px;
      display: flex;
      justify-content: center;
    }
    .van-tabs__nav--card .van-tab {
      line-height: 80px;
      width: 130px;
    }
    .van-tabs--card > .van-tabs__wrap {
      height: 80px;
    }
    .van-tabs__wrap {
      .van-tabs__nav--card .van-tab {
        color: @font_1_color;
        border: 1px solid #ccc;
        // margin: 0 10px;
        // border-radius: 8px;
      }
      .van-tabs__nav--card .van-tab.van-tab--active {
        background: unset;
        background-color: @main2_color;
        color:@font_3_color;
        border: none;
      }
    }
    .pic {
      display: flex;
      align-items: center;
      justify-content: center;
      #qrcode {
        img {
          width: 100%;
          height: 100%;
        }
      }
    }
  }
</style>
<style lang="less" scoped>
  .recommend_friend_wrap {
    height: 100vh;
    .invitation_code {
      .main{
        width: 528px;
        height: 786px;
        margin: 0 auto;
        background-color:@main2_color;
        background-size: contain;
        color:@font_3_color;
        border-radius: 40px;
        position: relative;
        &::before{
          content: "";
          display: block;
          width: 100%;
          height: 120px;
          position: absolute;
          bottom: -5px;
          background-size: 100%;
        }
        padding: 40px 0 0 ;
        .title{
          font-size: 30px;
          font-weight: 400;
        }
        .code_num{
          color: @font_3_color;
          font-size: 64px;
          font-weight: 500;
        }
        .tips{
          font-size: 30px;
          width: 500px;
          text-align: center;
        }
      }
      margin: 20px;
      text-align: center;
      p.title {
        text-align: center;
        height: 60px;
        line-height: 60px;
        font-size: 34px;
        font-weight: 400;
      }
      .pic {
        width: 320px;
        height: 320px;
        margin: 25px auto;
        position: relative;
        border-top :1px solid #eee;
        // background: url(../assets/images/common/ic_kuang.png) no-repeat;
        background-size: contain;

        & + span {
          text-align: center;
          margin: 0 auto;
          display: inline-block;
          font-size: 28px;
          padding: 10px;
          .bot_br;
        }
      }
      .btns {
        width: 260px;
        margin: 40px auto;
        button{
          height: 80px;
          background-color: #fff;
          border-radius: 50px;
          color:@main_color;

        }
      }
    }
    .my_friend {
      padding: 20px;
      ul {
        display: flex;
        justify-content: space-between;
        background-color: #fff;
        border-radius: 24px;
        padding: 30px 20px;
        margin-bottom: 20px;
        li {
          display: flex;
          align-items: center;
          .avatar {
            display: flex;
            align-items: center;
            .pic {
              width: 90px;
              height: 90px;
              border-radius: 50%;
              overflow: hidden;
              margin-right: 15px;
              img {
                width: 100%;
              }
              & + div {
                p {
                  font-size: 26px;
                  &:nth-of-type(1) {
                    font-size: 28px;
                    color: #666;
                  }
                  &:nth-of-type(2) {
                    color: #999;
                  }
                }
              }
            }
          }
        }
      }
    }
  }
</style>