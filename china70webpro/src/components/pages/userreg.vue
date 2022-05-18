<template>
   <div>
    <van-nav-bar
  :title="$t('index.righttxt')" 
  left-arrow
  @click-left="onClickLeft"
 
/>


<van-form validate-first @submit="onSubmit">

   <van-field
    v-model="par.name"
    name="用户名"
    :label="$t('index.username')"
    :placeholder="$t('index.username')"
    :rules="[{ required: true, message: '请填写用户名(Molimo unesite svoje korisničko ime)' }]"
  />
 <van-field
    v-model="par.tel"
    name="手机号(Broj telefona)"
    :label="$t('index.tel')"
    :placeholder="$t('index.tel')"
    :rules="[{ required: true, message: '请填写手机号(Molimo unesite svoj telefonski broj)' }]"
  />
  <van-field
    v-model="par.useremail"
    name="邮箱(Mail)"
     :label="$t('index.email')"
    :placeholder="$t('index.emailpcl')"
    :rules="[{ required: true, message: '请填写邮箱(unesite svoju e-poštu)' }]"
  />
  <van-field
    v-model="par.pwd"
    type="password"
    name="密码(zaporka)"
    :label="$t('index.pwd')"
    :placeholder="$t('index.pwdpcl')"
    :rules="[{ required: true, message: '请填写密码(Molimo unesite lozinku)' }]"
  />
   <div style="margin: 16px;">
    <van-button round block type="info" native-type="submit">{{$t('index.subtxt')}}</van-button>
  </div>
</van-form>
   </div>
</template>

<script>
export default {
   name:'userreg',
   data(){
       return{
          par:{
             id:0,
             useremail:'',
             pwd:'',
             Identitytype:'User',
             name:'',
             tel:''
          }
       }
   },
   created(){

   },
   methods:{
       onSubmit(){

          this.$post('User/UserAddorEdit',this.par).then(x=>{
             this.$toast(x.msg)
              this.$router.push({path : '/index'})
          })
       },
       onClickLeft(){
          this.$router.go(-1)
       }
   }
}
</script>

<style>

</style>