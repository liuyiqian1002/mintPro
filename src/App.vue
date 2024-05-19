<template>
  <div id="app">
    <keep-alive>
      <!-- 需要缓存的视图组件 -->
      <router-view v-if="$route.meta.keepAlive"></router-view>
    </keep-alive>
    <!-- 不需要缓存的视图组件 -->
    <router-view v-if="!$route.meta.keepAlive"></router-view>
     <img v-show="isLogin"  class="service"  ref="service"
      @mousedown="down" @touchstart.stop="down"
      @mousemove="move" @touchmove.stop="move"
      @mouseup="end" @touchend.stop="end"
      @click="CustomerService"
      :style="{top:defaultPosition?'80%':(position.y+'px'), left:position.x+'px'}" src="./assets/images/home/ic_contract_service.png" alt="">
  </div>
</template>
<script>
var dx,dy
var screenWidth = window.screen.width
var screenHeight = window.screen.height
import {  mapState } from "vuex";
import { isWechat, getQueryString } from "@/libs/tools";
import { setToken, getToken, localSave, localRead } from "@/libs/util";
import { Locale } from 'vant';
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
export default {
  name: "app",
  data() {
    return {
      position: {
        x: screenWidth -60,
        y:  '80%'
      },
      defaultPosition : true
    };
  },
  methods: {
     CustomerService(){
      let url =  this.$config.imgPath+`/News/CustomerService/${this.lang}?token=${this.userInfo.UserId}`;
      location.href = url;
    },
     // 实现移动端拖拽
    down(event){
      this.defaultPosition = false
      this.flags = true;
      var touch ;
      if(event.touches){
          touch = event.touches[0];
      }else {
          touch = event;
      }
       console.log('屏幕大小', screenWidth, screenHeight )
      console.log('鼠标点所在位置', touch.clientX,touch.clientY)
      console.log('div左上角位置', event.target.offsetTop,event.target.offsetLeft)
      dx = touch.clientX - event.target.offsetLeft
      dy = touch.clientY - event.target.offsetTop
       // 定位滑块的位置
        this.position.x = touch.clientX - dx;
        this.position.y = (touch.clientY - dy);
       if (this.position.x < 0) {
          this.position.x = 0
        } else if (this.position.x > screenWidth - touch.target.clientWidth) {
          this.position.x = screenWidth - touch.target.clientWidth
        }
        if (this.position.y < 0) {
          this.position.y = 0
        } else if (this.position.y > screenHeight - touch.target.clientHeight) {
          this.position.y = screenHeight - touch.target.clientHeight
        }
    },
    move() {
      this.defaultPosition = false
      if (this.flags) {
        var touch ;
        if (event.touches) {
          touch = event.touches[0];
        } else {
          touch = event;
        }
        // 定位滑块的位置
        this.position.x = touch.clientX - dx;
        this.position.y = (touch.clientY - dy);
        // 限制滑块超出页面
        // console.log('屏幕大小', screenWidth, screenHeight )
        // console.log('position', this.position.x, this.position.y )
        if (this.position.x < 0) {
          this.position.x = 0
        } else if (this.position.x > screenWidth - touch.target.clientWidth) {
          this.position.x = screenWidth - touch.target.clientWidth
        }
        // let  positionY = Number(this.position.y.replace('px',''))
        if (this.position.y < 0) {
          this.position.y = 0
        } else if (this.position.y > screenHeight - touch.target.clientHeight) {
          this.position.y =screenHeight - touch.target.clientHeight 
        }
        //阻止页面的滑动默认事件
        let service =  this.$refs.service
        service.addEventListener("touchmove",function(event){
            event.preventDefault();
        },false);
      }
    },
    //鼠标释放时候的函数
    end(){
      console.log('end')
      this.flags = false;
    },
  },

  mounted() {
    // (zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)
    // 按我截图的改，中简==》西班牙，中繁==》葡萄牙，日文==》阿拉伯
    // 西班牙语	es-ES
       let lang =''
    if(localStorage.getItem('MINT_lang')){
      lang = localStorage.getItem('MINT_lang')
    }else{
      localStorage.setItem('MINT_lang','en');
      lang = 'en'
    }
    console.log(lang)
    this.$i18n.locale = lang;
    if(lang=='zh-CN'){
      Locale.use('zh-CN', zhCN);
    }else if(lang=='zh-hk'){
      Locale.use('zh-HK', zhHK);
    }else if(lang=='jpn'){
      Locale.use('ja-JP', jaJP);
    }else if(lang=='en'){
      Locale.use('en-US', enUS);
    }
    
  },
   computed: {
     ...mapState({
       isLogin: state => state.base.isLogin,
       userInfo: state => state.base.userInfo,
       lang: state => state.base.lang,
     }),
  },
};
</script>
<style lang="less" scoped>
 .service{
      position: fixed;
      bottom: 200px;
      border-radius: 50%;
      right: 20px;
      // padding:10px;
      width: 100px;
      height: 100px;
      z-index: 1000;
      // img{
      //   width: 100%;
      // }
      overflow: hidden;
      // background-color: @main_color;
    }
</style>
<style lang="less">
  #app {
    font-family: PingFang SC, "Helvetica Neue", Arial, sans-serif;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
  }
</style>
