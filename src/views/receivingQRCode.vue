<template>
  <div class="receiving_qr_code_wrap">
     <van-nav-bar :title="$t('receivingQRCode.title')" left-text   left-arrow @click-left="$router.go(-1)" />
    <div class="contnet">
        <img :src="imgUrl" alt="" v-show="showImg">
        <div class="qr_box" ref="imageWrapper" v-show="!showImg">
            <p>{{$t('receivingQRCode.conTitle')}}</p>
            <div class="qr_code">
                <div id="qrcode" ref="qrCodeUrl"></div>
            </div>
            <div class="btns">
            <button class="main_btn" @click="saveImg">{{$t('receivingQRCode.btn')}}</button>
        </div>
        </div>
       
    </div>
  </div>
</template>

<script>
import QRCode from 'qrcodejs2';
import html2canvas from 'html2canvas'
import {
  Icon,
  NavBar,
  IndexBar,
} from "vant";
export default {
    name:'receivingQRCode',
    components:{
        [Icon.name]:Icon,
        [IndexBar.name]:IndexBar,
        [NavBar.name]:NavBar,
    },
    data(){
        return{
            text:'',
            imgUrl:'',
            showImg:false
        }
    },
    mounted(){
       this.text =  this.$route.query.code;
            let qrcode = new QRCode('qrcode', {
            width: 132,  
            height: 132,
            text: this.text, // 二维码地址
            colorDark : "#000",
            colorLight : "#fff",
        })
        // this.toImage();
    },
    methods:{
        toImage() {
            html2canvas(this.$refs.imageWrapper).then(canvas => {
            let dataURL = canvas.toDataURL("image/png");
            this.imgUrl = dataURL;
            if (this.imgUrl !== "") {
                this.showImg = true;
            }
        });
      },
      saveImg(){

          this.$toast(this.$t('receivingQRCode.saveImfTips'))
      }

    }

}
</script>
<style lang="less" >
.receiving_qr_code_wrap{
    .van-icon,
    .van-nav-bar__title{
    color: #fff !important;
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
    #qrcode{
        margin: 70px auto;
        text-align: center;
        img{
            margin: 0 auto !important;
        }
    }

}
</style>
<style lang="less" scoped>
    .receiving_qr_code_wrap{
        background-color: @bg_clor;
        height: 100vh;
        .contnet{
            margin-top: 50px;
            >img{

                width: 500px;
                margin: 0 auto;
                display: block;
            }
            .qr_box{
                border-radius: 20px;
                height: 650px;
                width: 500px;
                margin: 0 auto;
                background:url(../assets/images/common/ic_collection_code.png) no-repeat;
                background-size: 100%;
                background-color: @panel_color;
                p{
                    text-align: center;
                    color: #fff;
                    height: 110px;
                    font-size: 35px;
                    line-height: 110px;
                }
            }
            .btns{
                width: 300px;
                margin: 30px auto;
                button{
                    height: 80px;
                }
            }
        }
    }
</style>