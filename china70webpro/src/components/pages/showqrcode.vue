<template>
  <div>
           <van-nav-bar fixed
  title="店铺二维码"
  left-text="back"  
  left-arrow
  @click-left="onClickLeft"      
/>
  <div align="center">
     <van-image
     width="300"
     height="300"
     :src="img"
   /><br>
 <span>{{shop}}</span>
  </div>
   
  </div>
</template>

<script>
export default {
  name:'showqrcode',
  data(){
      return {
         img:"",
         shop:''
      }
  },
  created(){
    this.showqrcode()
  },
  methods:{
    onClickLeft(){
      this.$router.go(-1)
    },
     showqrcode(){
            
            var userid=localStorage.getItem('userid')
          this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)

          if(x.code==1){
              var sid=x.data.id
              this.shop=x.data.shopname
              this.$fetch('Shop/qrcodeget',{id:sid}).then(r=>{
                  
                  this.img=r.image
              })
              
          }else
          {
              this.$toast(x.msg)
          }
        })
      }, 
  }
}
</script>

<style>
   .dd{
       text-align: center;
   }
</style>