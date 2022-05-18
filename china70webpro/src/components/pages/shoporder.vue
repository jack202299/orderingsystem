<template>
   <div>
     
        <van-nav-bar fixed
  :title="$t('shporder.title')"
  :left-text="$t('sumbymonth.lefttext')"  
  left-arrow
  @click-left="onClickLeft"
  
/>
        <van-field
        readonly
        clickable       
        name="calendar"
        :value="par.orderDate"
        :label="$t('shporder.ordate')"
        :placeholder="$t('shporder.plc')"
        @click="showCalendar = true"
        />
        <van-calendar :min-date="mind" v-model="showCalendar" @confirm="onConfirm" />
        

        <van-dropdown-menu>
            <van-dropdown-item v-model="par.blfalge" :options="option1" @change="onchange" />       
        </van-dropdown-menu>
         
            <van-cell-group inset>
            
              <van-card v-for="(item,index) in tabledata" :key="index" 
               
                
                :desc="item.blfalge"
                :title="item.ordername"
                thumb="http://pic.616pic.com/ys_bnew_img/00/42/63/m4Ban0864Y.jpg"
                >
                
                 <template #price>
                    <van-tag plain type="danger">RSD{{item.meney}}</van-tag>
                    
                </template>
                <template #footer>
                     <van-button size="small"  @click="ondeleteOrder(item.id)">{{$t('shporder.del')}}</van-button>
                    <van-button v-if="item.blfalge=='等出货'" size="small" @click="onfh(item.id)">发货</van-button>
                    <van-button size="small"  @click="ongodetail(item.id)">{{$t('shporder.Details')}}</van-button>
                </template>
                </van-card>

            
            </van-cell-group>
              <van-empty v-if="showempty" :description="$t('sumbymonth.emptip')" />

   </div>
</template>

<script>
export default {
   name:'shoporder',
   data(){
       return{
           showempty:false,
            mind:new Date(2020,1,1),
          tabledata:[],
          showCalendar: false,
          par:{
              orderDate:'',
              shopid:0,
              blfalge:0              
          },
          option1:[
               { text: this.$t('shporder.dfh'), value: 0 },
               { text: this.$t('shporder.yfh'), value: 1 },
                ],
          ppar:{
              OrderId:0,
              blfage:0
          }
       }
   },
  mounted(){
       if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
     this.getdata()
   },
   methods:{
       getdata(){
     var date=new Date()
     var y=date.getFullYear()
     var m=date.getMonth()+1
     var d=date.getDate()
     var nowdate=y+'-'+m+'-'+d
     if(!sessionStorage.getItem('ord')) 
        this.par.orderDate=nowdate
     else this.par.orderDate=sessionStorage.getItem('ord')
     
     this.par.shopid=this.$route.query.shopid 
      this.$post('Order/QueryOrderList',this.par).then(x=>{
          console.log(x.data)
         if(x.data.length>0){
             this.tabledata=x.data             
         }else {
           this.showempty=true
           this.tabledata=[]    
         }      

      })
      },

      onConfirm(date) {
      this.par.orderDate = `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
      sessionStorage.setItem('ord',this.par.orderDate)
      this.showCalendar = false;
      this.par.shopid=this.$route.query.shopid 
      
      this.$post('Order/QueryOrderList',this.par).then(x=>{
          console.log(x.data)
         if(x.data.length>0){
             this.tabledata=x.data             
         }else this.showempty=true       

      })
    },
     ongodetail(id){
         console.log(id)
         this.$router.push({path : '/orderdetail', query : { orderid:id }})
     },
     onchange(value){
        console.log(value)
        this.par.blfalge=value
        this.getdata()
     },
     onClickLeft(){
        this.$router.go(-1)
     },
     onfh(id){
        this.ppar.orderid=id;
        this.ppar.blfage=1
        this.$post('Order/UpdateOrderStatus',this.ppar).then(r=>{
            console.log(r)
            this.getdata()
        })
     },
     ondeleteOrder(id){
         this.$dialog.confirm({
             title: '提示',
             message: this.$t('shporder.con'),
         }).then(()=>{
            this.$fetch('Order/DelOrder',{Id:id}).then(r=>{
                this.$toast(r.msg)
                this.getdata()
            })
         })
     }
   }
}
</script>

<style>

</style>