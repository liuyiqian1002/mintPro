<template>
  <div>
      <div id="qrcode" ></div>
  </div>
</template>

<script>
import QRCode from 'qrcodejs2';
export default {
    props:{"codeUrl":String},
    mounted(){
        console.log(this.codeUrl)
    let qrcode = new QRCode('qrcode', {
        width: 132,  
        height: 132,
        text: this.codeUrl, // 二维码地址
        colorDark : "#000",
        colorLight : "#fff",
      })
    }
}
</script>

<style>

</style>>