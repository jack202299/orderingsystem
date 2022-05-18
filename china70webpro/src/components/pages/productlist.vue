<template>
  <div>
      <van-nav-bar fixed
      
    :title="$t('pl.title')"
    :left-text="$t('pl.backtl')"
    :right-text="$t('pl.add')"
    left-arrow
    @click-left="onClickLeft"
    @click-right="onClickRight"
    />
    <van-search v-model="par.productpar" :placeholder="$t('pl.plc')"    @search="onSearch"   />
     <van-pull-refresh v-model="refreshing" @refresh="onRefresh">
      <van-list
        v-model="loading"
        :finished="finished"
        finished-text="没有更多了"
        @load="onload"
    >
    
      <van-card v-for="(item,index) in list" :key="index"
        currency="RSD"      
        
        :title="item.productName"
        :thumb="item.images"
        >
   <template #price>
       <span>RSD{{item.price}}/{{item.unit}}</span>
  </template> 
  
   <template #desc>
     <span>{{item.alias}}</span>
  </template> 
   <template #num>
     <span>库存：{{item.reserve}}</span>
  </template> 
  <template #footer>
    <van-button size="mini" @click="edrow(item.id)">{{$t('pl.edit')}}</van-button>
    <van-button  size="mini" @click="delproduct(item.id,index)">{{$t('pl.del')}}</van-button>
  </template>
   </van-card>
  </van-list>      
</van-pull-refresh>
   <van-empty v-if="showempty" description="暂时没有记录" />

  </div>
</template>

<script>
export default {
    name:'productlist',
    data(){
        return{  
          showempty:false,
            loading: false,
            finished: false,
             refreshing: false,      
           irows:0,
           list:[],
           show1:true,
           par:{
               productpar:'',
               shopid:'',
               page:1,
               pagesize:10
           }
        }
    },
   created(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
       this.par.shopid=this.$route.query.shopid 
       var uid=localStorage.getItem("userid")
       this.$fetch('User/QueryUserInfo',{Id:uid}).then(r=>{
       console.log(r.result)
       if(r.code==0){
         if(r.result.identitytype!="admin")  this.show1=false
       }
     })
    },
    methods:{
      getdata(){
         
              if (this.refreshing) {
                this.list = [];
                this.refreshing = false;
              }      
        
        
          this.$post('Product/QueryProductShopId',this.par).then(x=>{
              console.log(x);
                  this.irows=x.totalPage;
                  if(x.data.length==0) this.showempty=true
                  this.list=this.list.concat(x.data);
                 
                  this.loading=false                 
              
               if(this.list.length>=this.irows) 
                {
                  this.finished=true
                   console.log("finished:"+this.finished)
                }else{
                  this.par.page++
                }
              
          })      
          

          
      },
      delproduct(id,index){
           
           this.$fetch('Product/DelProduct',{Id:id}).then(x=>{
               if(x.code==0){
                  this.list.splice(index,1)
               }
               this.$toast(x.msg)
           })
      },
      onSearch(val){
      this.par.productpar=val
      this.par.page=1
      this.list=[]
      this.loading=true
      this.getdata() 
      },
      onload(){
        console.log("onload   "+this.loading)
        this.getdata()
      },
    
      onRefresh(){    
        this.finished = false;
         this.par.page=1
      // 重新加载数据
      // 将 loading 设置为 true，表示处于加载状态
      this.loading = true;
      this.irows=0
      this.getdata();     
         
      },
      onClickLeft(){
         this.$router.go(-1)
      },
      onClickRight(){
        this.$router.push({path : '/editproduct', query : { b:"add",shopid:this.$route.query.shopid  }})
      },
      edrow(id){
          this.$router.push({path : '/editproduct', query : { b:"edit",rowid:id,shopid:this.$route.query.shopid }})
      }
    }
}
</script>

<style>

</style>