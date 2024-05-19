<template>
  <div class="setLan_wrap">
    <van-nav-bar :title="$t('setLan.title')" left-text   left-arrow @click-left="$router.go(-1)" />
    <div class="cont">
        <ul >
            <li v-for=" (i ,j) in dataList" :class="{active:selIndex== j}" @click="selIndex = j">
                {{i.label}}
            </li>
        </ul>
        <div class="btns">
            <button @click="changeLan" class="main_btn">{{$t('setLan.btnSave')}}</button>
        </div>
    </div>
  </div>
</template>

<script>
import {
  Icon,
  NavBar,
  IndexBar,
  Sticky,
} from "vant";
import { Locale } from 'vant';
import enUS from '@/assets/i18n/lang/en-US';
import jaJP from '@/assets/i18n/lang/ar-sa';
import zhHK from '@/assets/i18n/lang/es-POR';
import zhCN from '@/assets/i18n/lang/es-ES';
export default {
    name:"setLan",
     components: {
    [Sticky.name]: Sticky,
    [NavBar.name]: NavBar,
    [IndexBar.name]: IndexBar,
  },
  data(){
      return {
          selIndex:0,
        //   语言版本(zh-CN：中简，zh-hk：中繁，en：英文，jpn：日文)<br/>")]
        // 按我截图的改，中简==》西班牙，中繁==》葡萄牙，日文==》阿拉伯
          dataList:[
            {
                label:'Việt Nam',
                val:"zh-CN"
            },
            {
                label:'Indonesia',
                val:"zh-hk"
            },
            {
                label:'English',
                val:"en"
            },
            {
                label:'Malaysia',
                val:"jpn"
            },  
          ]
      }
  },
  mounted(){
    let lang  =  localStorage.getItem('MINT_lang')?localStorage.getItem('MINT_lang'):'zh';
    this.dataList.map(( _ ,i)=>{
        if(_.val == lang){
            console.log(_);
            this.selIndex = i;
        }
    })
  },
  methods:{
      changeLan(){
          let selIndex = this.selIndex;
          let locale = this.dataList[selIndex].val
            this.$i18n.locale = locale;
            localStorage.setItem('MINT_lang',locale);
              if(locale=='zh-CN'){
                Locale.use('zh-CN', zhCN);
                }else if(locale=='zh-hk'){
                Locale.use('zh-HK', zhHK);
                }else if(locale=='jpn'){
                Locale.use('ja-JP', jaJP);
                }else if(locale=='en'){
                Locale.use('en-US', enUS);
                }
            let that = this;
           this.$toast({
            duration: 1500,
            message: this.$t('transactionPwd.seccess'),
            onClose() {
                that.$router.go(-1);
            }
          });
      }
  }

}
</script>
<style lang="less"  >
    .setLan_wrap{
        background-color: @background-color;
        .van-nav-bar{
            background: transparent;
        }
    }

</style>
<style lang="less" scoped>
    .setLan_wrap{
        height: 100vh;
        .cont{
            padding: 0 30px;
            li{

                background: url(../assets/images/common/ic_langue_unselected.png) no-repeat;
                background-color: @panel_color;
                color:@font_color;
                margin-bottom: 20px;
                border-radius: 24px;
                .bot_br;
                padding:10px 40px;
                line-height: 80px;
                background-position: 95% center;
                background-size: 50px 50px;
                &.active{
                    background: url(../assets/images/common/ic_langue_selected.png) no-repeat;
                    background-color: @panel_color;
                    background-position: 95% center;
                    background-size: 50px 50px;
                }
            }
        }
        .btns{
            width: 100%;
            margin:  50px auto;
            button{
                background-color:@main_color2 !important;
            }
        }
        .bot{
            width: 80%;
            margin: 80px auto;
            .main_btn{
                height: 90px;
                font-size: 28px;
            }
            .nav{
                margin: 20px 0;
                justify-content: space-between;
                display: flex;
            }
        }
    }
</style>