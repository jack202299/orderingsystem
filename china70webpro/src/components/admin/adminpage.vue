<template>
  <div>
 <van-nav-bar fixed
  title="管理功能"
  left-text="返回"  
  left-arrow
  @click-left="onClickLeft"  
/>
   <van-grid :column-num="3">
   <van-grid-item  icon="photo-o" text="店铺管理" to="/shopAdmin" />
   <van-grid-item  icon="photo-o" text="行业分类" to="/kindsadmin" />
   <van-grid-item  icon="photo-o" text="文字" />
   </van-grid>
  </div>
</template>

<script>
export default {
   name:'adminpage',
   data(){
      return {

      }
   },
   created(){
       
   },
   methods:{
      onClickLeft(){
         this.$router.go(-1)
      },

   }
}
</script>

<style>

</style>