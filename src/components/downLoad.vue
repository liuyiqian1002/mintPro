<template>
  <div class="down_load_wrap" @click="downloadApp">
    <div class="l" >
        <img src="../assets/images/common/logo.png" alt="">
        <div>
            <p>MINT</p>
            <p>{{$t('base.downloadSubTitle')}}</p>
        </div>
    </div>
    <div class="r">
        <img  src="../assets/images/common/download1.png" class="down_load_ico" alt="">
        <img @click.stop='closeDownLoad' src="../assets/images/common/close_download.png" class="close_ico" alt="">
    </div>
  </div>
</template>

<script>
export default {
    methods:{
        downloadApp(){
            localStorage.setItem('download',Date.now());
           let lang  = localStorage.getItem('MINT_lang');
          location.href= `https://download.mintexc.com`
        },
        closeDownLoad(){
            localStorage.setItem('download',Date.now());
            this.$emit('closeDownLoad',false)
        }
    }

}
</script>
<style lang="less" scoped>
    .down_load_wrap{
        margin: 0;
        background-color:@panel_color ;
        position: fixed;
        z-index: 100;
        width: 100%;
        display: flex;
        justify-content: space-between;
        padding:0 20px ;
        height: 100px;
        >.l{
            display: flex;
            align-items: center;
            >img{
                width: 56px;
            }
            >div{
                color: #fff;
                margin-left: 20px;
                p{

                    &:nth-of-type(1){
                        font-size: 28px;
                        font-weight: 500;

                    }
                    &:nth-of-type(2){
                        font-size: 22px;

                    }
                }
            }
        }
        >.r{
            display: flex;
            align-items: center;
            .down_load_ico{
                width: 56px;
                margin-right: 22px;
            }
            .close_ico{
                width: 26px;
            }
        }

    }
</style>