<template>
  <div>

    <van-nav-bar
    
  :title="$t('index.login')" 
  :right-text="$t('index.righttxt')"
  @click-right="onreger"
 
>
   <!-- <template #left>
     <span>{{$t('index.lang')}}</span>
     <van-dropdown-menu>
      <van-dropdown-item v-model="value1" :options="option1" @change="onchang" />  
      </van-dropdown-menu>
   </template> -->
    </van-nav-bar>
    <img alt="" src="../assets/55.jpg" style="width:100%;height:230px;" />



      <van-form @submit="onSubmit">
  <van-field
    v-model="login.useremail"
    name="邮箱(Mail)"
    :label="$t('index.email')"
    :placeholder="$t('index.emailpcl')"
    :rules="[{ required: true, message: '请填写邮箱(unesite svoju e-poštu)' }]"
  />
  <van-field
    v-model="login.Password"
    type="password"
    name="密码(zaporka)"
    :label="$t('index.pwd')"
    :placeholder="$t('index.pwdpcl')"
    :rules="[{ required: true, message: '请填写密码(Molimo unesite lozinku)' }]"
  />
   <div style="margin:10px">
     <van-checkbox v-model="checked">{{$t('index.forgetpwd')}}</van-checkbox>
   </div>
  
   

  <div style="margin: 16px;">
    <van-button round block type="info" native-type="submit">{{$t('index.subtxt')}}</van-button>
    
  </div>
</van-form>
  </div>
</template>

<script>
export default {
  name:'index',
  data(){
      return {
        option1:[{icon:'',text:'China',value:'zh-CN'},{icon:'',text:'English',value:'en-US'},{icon:'',text:'Serbia',value:'sr-SR'}],
        login:{
           useremail:'',
           Password:'',
        },       
        checked:false,
        value1:'',
      }
  },
  created(){
     this.$i18n.locale='zh-CN'
    this.value1=localStorage.getItem('lang')
    this.checked=localStorage.getItem('bl')
    this.login.useremail=localStorage.getItem('email')
    if(this.checked) this.login.Password=localStorage.getItem('pwd')
  },
  methods:{
    onchang(val){
      localStorage.setItem('lang',val)
      this.$i18n.locale='zh-CN'
    },
    onSubmit(values) {
      console.log('submit', values);
      this.$post("User/Login",this.login).then(r=>{
          console.log(r)
          if(r.code==0){
              this.$toast.success(r.msg);
              localStorage.setItem("userid",r.data.id)
              localStorage.setItem('email',r.data.useremail)
               localStorage.setItem('user',r.data.name)
              localStorage.setItem('pwd',this.login.Password)
              localStorage.setItem('bl',this.checked)
              sessionStorage.setItem("token",r.strtoken)
              localStorage.setItem('idtype',r.data.identitytype)
              this.$router.push({path : '/shopproduct',query:{shopid:2}})  
          }else
             this.$toast.fail(r.msg)
      })
    },
    onreger(){
      this.$router.push({path:'/userreg'})
    },
    test(){
      var url="https://www.google.com/maps/search/?api=1&query=莲花超市"
      window.open(url)
    }
  }
}
</script>

<style>
.ui-flex {
            display: -webkit-box !important;
            display: -webkit-flex !important;
            display: -ms-flexbox !important;
            display: flex !important;
            -webkit-flex-wrap: wrap;
            -ms-flex-wrap: wrap;
            flex-wrap: wrap
        }

        .ui-flex, .ui-flex *, .ui-flex :after, .ui-flex :before {
            box-sizing: border-box
        }

        .ui-flex.justify-center {
            -webkit-box-pack: center;
            -webkit-justify-content: center;
            -ms-flex-pack: center;
            justify-content: center
        }
        .ui-flex.center {
            -webkit-box-pack: center;
            -webkit-justify-content: center;
            -ms-flex-pack: center;
            justify-content: center;
            -webkit-box-align: center;
            -webkit-align-items: center;
            -ms-flex-align: center;
            align-items: center
        }
</style> 