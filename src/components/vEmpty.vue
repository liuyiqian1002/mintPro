<template>
  <div class="hello">
   <van-empty
  class="custom-image"
  image="https://img.yzcdn.cn/vant/custom-empty-image.png"
  :description="emptyTxt"
/>
  </div>
</template>

<script>
import {
  Empty
} from "vant";
export default {
  name: 'empty',
  props: {
    emptyTxt:String,
    emptyUrl:String
  }
}
</script>

<style scoped lang="less">

</style>
