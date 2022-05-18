<template>
   <div>
        <van-nav-bar fixed
  title="产品分类"
  left-text="返回"  
  right-text="新增"
  left-arrow
  @click-left="onClickLeft"
  @click-right="onClickRight"  
/>
   <div :style="{height: contentHeight}">
    <van-cell v-for="(item,index) in list" :key="index" :title="item.kindsname" icon="shop-o">
  <!-- 使用 right-icon 插槽来自定义右侧图标 -->
  <template #right-icon>
    <van-button type="primary" size="small" @click="onedit(index)">编辑</van-button>
    <van-button type="primary" size="small" @click="ongoedit(item.id)">产品标签</van-button>
  </template>
    </van-cell>
   </div>
       <van-dialog v-model="show" title="编辑" show-cancel-button @confirm="onconfirm">
           <van-field v-model="par.kindsname" label="行业分类" right-icon="exchange" @click-right-icon="ontraleg" />
      </van-dialog>
   </div> 
</template>

<script>
export default {
    name:'shoptypeadmin',
    data(){
        return{
             b:'add',
             show:false,
             contentHeight: 0, 
             list:[], 
             par:{
               id:0,
               kindsname:''
             }
        }
    },
    created(){
        this.getdata()
    },
    mounted(){
       this.contentHeight = document.documentElement.clientHeight - 66 - 40 + "px";
     },
    methods:{
       onClickLeft(){
         this.$router.go(-1)
      },
      onClickRight(){
        this.b='add'
        this.show=true
        this.par.id=0
        this.par.kindsname=''
      },
      getdata(){
       this.$fetch('Kinds/QuerKindsList').then(x=>{
           this.list=x;
       })
     },
     onconfirm(){
         if(this.b=='add'){
           this.par=0
         }
         this.$post('Kinds/AddOrEditKind',this.par).then(x=>{
           this.show=false
           this.$toast(x.msg)
           this.getdata()
         })
     },
     onedit(index){
       this.b='edit'
       this.par=this.list[index]      
       this.show=true
     },
    ongoedit(id){
      this.$router.push({path:'/protype',query:{kindsid:id}})
    },
     ontraleg(){
      if(this.par.kindsname==''){
        this.$toast('没有内容可翻译')
        return
      }

      this.$fetch('Product/CNtoHR',{txt:this.par.kindsname}).then(x=>{
         if(x.code==0) this.par.kindsname=this.par.kindsname+'('+x.result+')'
      })
    }
  }
}
</script>

<style>
 
</style>