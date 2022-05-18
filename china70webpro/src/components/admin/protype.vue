<template>
  <div>
        <van-nav-bar fixed
  title="行业分类"
  left-text="返回"  
  right-text="新增"
  left-arrow
  @click-left="onClickLeft"
  @click-right="onClickRight"  
/>

    <div :style="{height: contentHeight}">
    <van-cell v-for="(item,index) in list" :key="index" :title="item.protype" icon="shop-o">
  <!-- 使用 right-icon 插槽来自定义右侧图标 -->
  <template #right-icon>
    <van-button type="primary" size="small" @click="onedit(index)">编辑</van-button>
    <van-button type="primary" size="small" @click="ongoedit(item.id)">删除</van-button>
  </template>
    </van-cell>
   </div>
       <van-dialog v-model="show" title="编辑" show-cancel-button @confirm="onconfirm">
           <van-field v-model="par.protype" label="产品分类" right-icon="exchange" @click-right-icon="ontraleg" />
      </van-dialog>
   </div> 
</template>

<script>
export default {
   name:'protype',
   data(){
       return{
           b:'add',
             show:false,
             contentHeight: 0, 
             list:[], 
             par:{
               id:0,
               protype:'',
               kindsid:0
             }
       }
   },
    mounted(){
        this.contentHeight = document.documentElement.clientHeight - 166 - 40 + "px";
        this.getdata()
     },
   created(){
      this.getdata()
   },
   activated(){
         this.contentHeight = document.documentElement.clientHeight - 166 - 40 + "px";
        this.getdata()
   },
   methods:{
      
      onClickLeft(){
         this.$router.go(-1)
      },
      onClickRight(){
        this.b='add'
        this.show=true
        this.par.id=0
        this.par.protype=''
        this.par.kindsid=this.$route.query.kindsid
      },
      getdata(){
          this.par.kindsid=this.$route.query.kindsid
       this.$fetch('ProductType/QueryProductTypeList',{kindsid:this.par.kindsid}).then(x=>{
           this.list=x.data;
       })
     },
     onconfirm(){
         if(this.b=='add'){
           this.par.id=0
         }
         this.$post('ProductType/AddorEditProductType',this.par).then(x=>{
           this.show=false
           this.$toast(x.msg)
           this.getdata()
         })
     },
     onedit(index){
       this.b='edit'
       this.par=this.list[index]
       this.par.kindsid=this.$route.query.kindsid
       this.show=true
     },
      ontraleg(){
      if(this.par.protype==''){
        this.$toast('没有内容可翻译')
        return
      }

      this.$fetch('Product/CNtoHR',{txt:this.par.protype}).then(x=>{
         if(x.code==0) this.par.protype=this.par.protype+'('+x.result+')'
      })
    }

   }
}
</script>

<style>

</style>