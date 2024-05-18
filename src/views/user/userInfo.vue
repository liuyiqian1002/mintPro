<template>
  <div class="user_info_wrap">
    <van-nav-bar
      :title="$t('userInfo.title')"
      left-text
      :placeholder="true"
      fixed
      left-arrow
      @click-left="$router.go(-1)"
    />
    <div class="content">
      <div class="avatar">
        <div class="pic">
              <input
                type="file"
                @change="upimg($event)"
                name=""
                class="upimg"
                id=""
              />
              <img :src="avatar" ref="img" alt="" />
        </div>
        <p>{{$t('mine.avatar')}}</p>
      </div>
      <ul >
        <li>UID <span>{{ UserInfo.EnCode }}</span></li>
        <li>
          <div>
            
          </div>
        </li>
      </ul>
      <ul @click="$router.push('/nickname')">
        <li>{{ $t("userInfo.nickname") }} <span>{{ UserInfo.NickName || $t('userInfo.unset') }}</span></li>
        <li>
          <div class="">
            <img
              class="arrow_right"
              src="../../assets/images/common/back_right.png"
              alt=""
            />
          </div>
        </li>
      </ul>
      <ul @click="show = true">
        <li>{{ $t("userInfo.sex") }} <span>{{  setGender(UserInfo.Gender)  }}</span></li>
        <li>
          <div>
             <img
              class="arrow_right"
              src="../../assets/images/common/back_right.png"
              alt=""
            />
          </div>
        </li>
      </ul>
      <ul @click="showDatetime = true">
        <li>{{ $t("userInfo.birthday") }} <span>{{ UserInfo.Birthday || $t('userInfo.unset') }}</span></li>
        <li>
          <div>
             <img
              class="arrow_right"
              src="../../assets/images/common/back_right.png"
              alt=""
            />
          </div>
        </li>
      </ul>
      
    </div>
    <van-popup v-model="show" position="bottom" round>
      <van-picker
        show-toolbar
        :columns="Gender"
        @cancel="show = false"
        @confirm="onConfirm"
      />
    </van-popup>
    <van-popup v-model="showDatetime" position="bottom" round>
    <van-datetime-picker
    v-model="currentDate"
    type="date"
    title=""
    :min-date="minDate"
    :max-date="maxDate"
    @cancel="showDatetime = false"
    @confirm="confirmDatetime"
    />
    </van-popup>
  </div>
</template>

<script>
import { Icon, NavBar, IndexBar, Picker, Popup ,DatetimePicker} from "vant";
import { UpdateUserEntity, GetEntityUser, UpLoadImg } from "@/api";
import { encryptByDES, decryptByDES } from "@/libs/util";
export default {
  name: "userInfo",
  components: {
    [Icon.name]: Icon,
    [IndexBar.name]: IndexBar,
    [NavBar.name]: NavBar,
    [Picker.name]: Picker,
    [Popup.name]: Popup,
    [DatetimePicker.name]: DatetimePicker,
  },
  data() {
    return {
      MINT_auth: {},
      avatar: "",
      UserInfo: {},
      Gender:[],
      show:false,
      showDatetime:false,
      currentDate: new Date(),
      minDate: new Date(1949, 0, 1),
      maxDate: new Date(),
      lang:""
    };
  },
  mounted() {
    this.Gender = [
          {value:1,text:this.$t('userInfo.m')},
          {value:2,text:this.$t('userInfo.w')},
      ]
    let MINT_auth = localStorage.getItem("MINT_auth");
    MINT_auth = this.$decryptByDES(MINT_auth);
    this.MINT_auth = JSON.parse(MINT_auth);
    this.lang  = localStorage.getItem('MINT_lang')
    this.getEntityUser();
    this.avatar = this.MINT_auth.HeadIcon
      ? this.$config.imgPath + this.MINT_auth.HeadIcon
      : require("../../assets/images/common/logo.png");
  },
  methods: {
      onConfirm(value, index) {
      console.log(value.value);
      this.updateUserEntity('Gender',value.value).then(r=>{
          if(r.Code == 200 ){
              this.getEntityUser();
          }
      })
      this.show = false;
    },
    confirmDatetime() {
      console.log(this.currentDate);
      let value =  this.$moment(this.currentDate).format('YYYY/MM/DD');
      this.updateUserEntity('Birthday',value).then(r=>{
          if(r.Code == 200 ){
              this.showDatetime = false
              this.getEntityUser();
          }
      })
      this.show = false;
    },
    upimg(e) {
      let file = e.target.files[0];
      // var exp = /png|gif|jpg|jpeg/;
      // if (!exp.test(file.name)) {
      //   this.$toast(this.$t('base.imgErr'))
      //   return;
      // }
      let param = new FormData(); //创建form对象
      param.append("file", file); //通过append向form对象添加数据
      let EnCode = `${encodeURIComponent(
        encryptByDES(JSON.stringify({ EnCode: this.MINT_auth.EnCode }))
      )}`;
      param.append("EnCode", this.MINT_auth.EnCode); //通过append向form对象添加数据
      param.append("LanguageVersion", this.lang); //通过append向form对象添加数据
      // console.log(param.get("file")); //FormData私有类对象，访问不到，可以通过get判断值是否传进去
      let config = {
        headers: {
          "Content-Type": "multipart/form-data",
        },
      }; //添加请求头
      this.$toast.loading({
        duration: 0, // 持续展示 toast
        forbidClick: true,
      });
      this.axios.post(this.$config.baseUrl.pro+"/Index.asmx/UpLoadImg", param, config).then((res) => {
        console.log(JSON.parse(this.$decryptByDES(res.data)));
        let r = JSON.parse(this.$decryptByDES(res.data));
        // {"Code":200,"Message":"添加成功","Data":{"FilePath":"/Resource/WebUserImage/HJ231057/20200810005058500.png"}}
        if (r.Code == 200) {
          let FilePath = r.Data.FilePath;
          this.$toast(this.$t('base.upSeccess'));
          this.updateUserEntity("HeadIcon", FilePath).then((r) => {
            console.log(r);
          });
        }else{
          this.$toast(r.Message);
        }
      });

      var objUrl = getObjectURL(file);
      console.log("objUrl = " + objUrl);
      if (objUrl) {
        this.$refs.img.src = objUrl;
      }

      function getObjectURL(file) {
        var url = null;
        if (window.createObjectURL != undefined) {
          // basic
          url = window.createObjectURL(file);
        } else if (window.URL != undefined) {
          // mozilla(firefox)
          url = window.URL.createObjectURL(file);
        } else if (window.webkitURL != undefined) {
          // webkit or chrome
          url = window.webkitURL.createObjectURL(file);
        }
        return url;
      }
    },
    getEntityUser() {
      let data = {
        UserId: this.MINT_auth.UserId,
      };
      GetEntityUser(data).then((r) => {
        // console.log(JSON.stringify(r.data.Data) );
        if (r.data.Code == 200) {
          this.UserInfo = r.data.Data;
          const { UserGrade } = r.data.Data;
          console.log(this.UserInfo);
          this.$store.commit("setUserInfo", this.UserInfo);
          this.avatar = r.data.Data.HeadIcon
            ? this.$config.imgPath + r.data.Data.HeadIcon
            : require("../../assets/images/common/logo.png");
        }
      });
    },
    async updateUserEntity(Action, ActionValue) {
      let data = {
        UserId: this.MINT_auth.UserId,
        Action,
        ActionValue,
      };
      let res = await UpdateUserEntity(data).then((r) => {
        return r.data;
      });
      return res;
    },
    setGender(val)  {
    let str = ''
        // （0-待到账、1-已到账、2-已退回）
    switch (val) {
        case '1':
            str = this.$t('userInfo.m')
            break
        case '2':
            str = this.$t('userInfo.w')
            break
            default:
            str = this.$t('userInfo.unset')
            break
    }
    return str;
}
  },
};
</script>
<style lang="less" scoped>
  /*.van-picker{*/
  /*  background-color: #1CED34;*/
  /*}*/
  /*.van-picker__columns{*/
  /*  background-color: red;*/
  /*}*/

  .user_info_wrap {
    background-color: @background-color;
    height: 100vh;
    min-height: 100vh;
    .content {
      >.avatar{
        padding: 30px 0 0 ;
        margin-bottom: 20px;
        .pic{
          width: 200px;
          height: 200px;
          margin: 0 auto;
          position: relative;
          border-radius: 50%;
          overflow: hidden;
          .upimg {
            position: absolute;
            top: 0;
            right: 0;
            bottom: 0;
            border: 1px solid;
            opacity: 0;
            z-index: 1;
          }
          img{
            width: 100%;
            height: 100%;
          }
        }
        p{
          font-size: 24px;
          color: @font_f_color;
          text-align: center;
          margin-top: 20px;
        }

      }
      ul {
        background-color: @panel_color;
        display: flex;
        color: @font_1_color;
        align-items: center;
        padding: 30px 20px;
        margin-bottom: 10px;
        margin:0 20px;
        justify-content: space-between;
        .bot_br;
        &:not(:nth-last-of-type(1)) {
        }
        li {
          span{
            margin-left: 40px;
          }
          > div {
            display: flex;
            align-items: center;
            position: relative;
            .upimg {
              position: absolute;
              top: 0;
              right: 0;
              bottom: 0;
              border: 1px solid;
              opacity: 0;
              z-index: 1;
            }
          }
          .avatar {
            height: 100px;
            width: 100px;
            border-radius: 50%;
            overflow: hidden;
            img {
              width: 100%;
              height: 100%;
            }
          }
          img.arrow_right {
            height: 30px;
            margin-left: 20px;
          }
        }
      }
    }
  }
</style>