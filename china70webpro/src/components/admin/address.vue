<template>
  <div>
 <van-nav-bar fixed
  title="我是买家"
  left-text="返回"  
  right-text="新增地址"
  left-arrow
  @click-left="onClickLeft"   
   @click-right="onClickRight" 
/>

   <van-radio-group v-model="chosenAddressId" @change="onrowchang">
   <van-cell v-for="(item,index) in list" :key="index">
  <!-- 使用 right-icon 插槽来自定义右侧图标 -->
   <template #icon>
    <van-radio :name="item.id">

    </van-radio>
  </template>

  <template #right-icon>
    <van-icon name="search" class="search-icon" @click="editaddress(item.id)" />
  </template>
  <template #title>
    <span class="custom-title">{{item.name}}</span>
    <span class="custom-title">{{item.tel}}</span><br>
    <span class="custom-title">{{item.address}}</span>
    <span class="custom-title">{{item.taxID}}</span><br>
    <span class="custom-title">{{item.company}}</span><br>
  </template>
</van-cell>
</van-radio-group>
 <van-empty v-if="showempty" description="暂时没有记录" />
  </div>
</template>

<script>
export default {
   name:'address',
   data(){
      return {
        showempty:false,
        chosenAddressId:'',  
        list:[],
        par:{
          id:0,
          userid:0,
          bl:false
        }
      }
   },
   mounted(){
     this.getdata()
   },
   activated(){
      this.getdata()
   },
   methods:{
     onClickLeft(){
         this.$router.go(-1)
     },
     onClickRight(){
          this.$router.push({path:'/editaddress',query:{b:'add',userid:this.$route.query.uid}})
     },
     
     onEdit(item, index){
       console.log(item)
     },
     getdata(){
        var uid=this.$route.query.uid
        this.$fetch('Ships/QueryShipsList',{UserId:uid}).then(r=>{
          if(r.length==0) this.showempty=true
           this.list=r
           this.list.forEach(x=>{
             if(x.isdefault) this.chosenAddressId=x.id
           })
        })
     },
     editaddress(val){
       this.$router.push({path:'/editaddress',query:{b:'edit',userid:this.$route.query.uid,id:val}})
     },
     onrowchang(name){
       this.$dialog.confirm({title:"提示",message:'确定切换默认地址吗'}).then(()=>{
            this.par.id=parseInt(name)
            this.par.userid=this.$route.query.uid
            this.par.bl=true
            this.$post('Ships/UpdateAddress',this.par).then(r=>{
              if(this.$route.query.soure) this.$router.push({path:this.$route.query.soure})
          })
       })


       
     }
   }
}
</script>

<style>

</style>