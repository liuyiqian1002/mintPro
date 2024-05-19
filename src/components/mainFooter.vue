<template>
<div>
  <div class="footer">
      <van-tabbar v-model="active"  active-color="#65BD8D" inactive-color="#666">
      <van-tabbar-item v-for="(item,index) in bar" :key="index" @click="barClick(index,item)">
        <span>{{item.barTitle}}</span>
        <template #icon="props">
          <img :src="props.active ? item.icon.active : item.icon.inactive" :class="{'img-active': props.active}" />
        </template>
      </van-tabbar-item>
    </van-tabbar>
  </div>
    <!-- <search  @changeSt='closrSearch' :nShow = nShow /> -->
</div>
</template>

<script>
import { Tabbar, TabbarItem ,Popup} from "vant";
import search from '@/views/login';
export default {
name: "tabBar",
  components: {
    [Tabbar.name]: Tabbar,
    [TabbarItem.name]: TabbarItem,
    [Popup.name]: Popup,
    search
  },
    data(){
        return {
             bar: [],
             active: 0,
             nShow:false,
           }

    },
    mounted(){
      this.nShow=true;
      this.bar=[
               {
                 barTitle: this.$t('base.home'),
                 icon: {
                   active: require("../assets/images/tab-bar/home_1.png"),
                   inactive: require("../assets/images/tab-bar/home_0.png")
                 },
                 link: "/"
               },
               {
                 barTitle:this.$t('base.market'),
                 icon: {
                   active: require("../assets/images/tab-bar/market_1.png"),
                   inactive: require("../assets/images/tab-bar/market_0.png")
                 },
                 link: "/market"
               },
               {
                 barTitle: this.$t('base.deal'),
                 icon: {
                   active: require("../assets/images/tab-bar/trade_1.png"),
                   inactive: require("../assets/images/tab-bar/trade_0.png")
                 },
                 link: "/deal"
               },
               {
                 barTitle: this.$t('base.contract'),
                 icon: {
                   active: require("../assets/images/tab-bar/futures_1.png"),
                   inactive: require("../assets/images/tab-bar/futures_0.png")
                 },
                 link: "/contract"
               },
               {
                 barTitle: this.$t('base.property'),
                 icon: {
                   active: require("../assets/images/tab-bar/assets_1.png"),
                   inactive: require("../assets/images/tab-bar/assets_0.png")
                 },
                 link: "/property"
               },
             ];
        let _this = this;
        _this.bar.forEach((item, index) => {
          if (item.link === _this.$route.path) {
                console.log(_this.$route.path)
            _this.active = index;
          }
        });
    },
    methods:{

        barClick(index, item) {
          this.active = index;
          this.$router.push(item.link);
          window.scrollTo(0, 0);
        },
        closrSearch(){
          this.nShow= ! this.nShow
      }
    },
  activated() {
    // let _this = this;
    // _this.bar.forEach((item, index) => {
    //   if (item.link === _this.$route.path) {
    //       console.log(_this.$route.path)
    //     _this.active = index;
    //   }
    // });
  }
}
</script>
<style lang="less" >
  .footer{
      .van-tabbar{
          background-color: #ffffff;
      }
      .img-active{
        transform: scale(1.5);
        // margin-top: 10px;
      }
  }
</style>