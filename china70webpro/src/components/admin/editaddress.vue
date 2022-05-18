<template>
   <div>
      <van-nav-bar fixed
  title="我是买家"
  left-text="返回"  
  
  left-arrow
  @click-left="onClickLeft"   
 
/>
    <van-form @submit="onSubmit">
  <van-field
    v-model="par.shipaddress"
    name="收货地址"
    label="收货地址"
    placeholder="收货地址"
    :rules="[{ required: true, message: '请填写收货地址' }]"
  />
  <van-field
    v-model="par.shipperson"    
    name="收货人"
    label="收货人"
    placeholder="收货人"
    :rules="[{ required: true, message: '请填写收货人' }]"
  />
   <van-field
    v-model="par.shipTel"    
    name="电话"
    label="电话"
    placeholder="电话"
    :rules="[{ required: true, message: '请填写电话' }]"
  />
   <van-field
    v-model="par.company"    
    name="公司名称"
    label="公司名称"
    placeholder="公司名称"
    :rules="[{ required: true, message: '请填写公司名称' }]"
  />
   <van-field
    v-model="par.taxID"    
    name="税号"
    label="税号"
    placeholder="税号"
    :rules="[{ required: true, message: '请填写税号' }]"
  />
  <van-checkbox v-model="par.isdefault">设为默认</van-checkbox>
  <div style="margin: 16px;">
    <van-button round block type="info" native-type="submit">提交</van-button>
  </div>
</van-form>


   </div>
</template>

<script>
export default {
   name:'editaddress',
   data(){
     return {
       par:{
           id:0,
           userid:0,
           shipaddress:'',
           shipperson:'',
           shipTel:'',
           company:'',
           taxID:'',
           isdefault:false
       }
     }
   },
   
   activated(){
     this.getdata()
   },
   methods:{
    onClickLeft(){
         this.$router.go(-1)
     },
  
      onSubmit(values) {
          this.$post('Ships/AddorEditShips',this.par).then(r=>{
              this.$toast(r.msg)
              this.$router.go(-1)
          })
      },
      getdata(){
          var bb=this.$route.query.b
     if(bb=='add'){
         this.par.userid=this.$route.query.userid
         this.par.id=0,
            this.par.shipaddress=''
             this.par.shipperson=''
             this.par.shipTel=""
             this.par.company=""
             this.par.taxID=""
             this.par.isdefault=false
     }else{
         this.par.userid=this.$route.query.userid
         this.par.id=this.$route.query.id
         this.$fetch('Ships/QueryShipsinfo',{Id:this.par.id}).then(r=>{
             console.log(r)
             this.par.id=r.id
             this.par.userid=r.userid
             this.par.shipaddress=r.shipaddress
             this.par.shipperson=r.shipperson
             this.par.shipTel=r.shipTel
             this.par.company=r.company
             this.par.taxID=r.taxID
             this.par.isdefault=r.isdefault
         })
     }
      }

   }

}
</script>

<style>

</style>