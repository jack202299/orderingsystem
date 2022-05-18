<template>
    <div>
   <van-nav-bar fixed
  :title="shoptitle"
  left-text="返回" 
  left-arrow
  @click-left="onClickLeft" 
  />

   <form action="/">
  <van-search
    show-action
    v-model="par.strname"    
    placeholder="请输入搜索关键词"
    @search="onSearch"  
  />
</form>

   <van-cell v-for="(i,index) in list" :key="index"  :title="i.name" :label="i.tel" icon="shop-o">
  <!-- 使用 right-icon 插槽来自定义右侧图标 -->
   <template #label>
      <span>手机号：{{i.tel}}</span> <span>已经交易了{{i.icount}}笔</span>
   </template>
  
  <template #right-icon>
   <van-icon name="browsing-history-o" @click="showimage(i.image)" />
  </template>
  </van-cell>
   <van-empty v-if="showem" description="暂无记录" />
   <van-image-preview v-model="show" :images="images" closeable/>

    </div>
</template>

<script>
export default {
   name:'clientage',
   data(){
      return {
         par:{
            shopid:0,
            strname:''
         },
         list:[],
         images:[],
         show:false,
         showem:false,
      }
   },
   created(){
       this.getdata()
   },
   methods:{
    onClickLeft(){
       this.$router.go(-1)
    },
    onSearch(val){
      this.par.strname=val
      this.getdata()
    },
    getdata(){
        var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              this.par.shopid=sid
              this.$post('Clientage/QueryMyClientage',this.par).then(x=>{
                  if(x.data.length==0) this.showem=true
                  this.list=x.data
              })
          }else
          {
             this.$toast(x.msg)
          }
        })
    },
    showimage(img){
       this.images=[]
       this.images.push(img)
       this.show=true
    }
   }
}
</script>

<style>

</style>