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
        <van-calendar type="range" :min-date="mind" v-model="showCalendar"  @confirm="onConfirm" />
        

        <van-dropdown-menu>
            <van-dropdown-item v-model="par.blfalge" :options="option1" @change="onchange" />       
        </van-dropdown-menu>
         
            <van-cell-group inset>
            
            <van-cell v-for="(item,index) in tabledata" :key="index" 
            
             :label="item.person"              
             @click="ongodetail(item.id)"
             >
                
                <template #title>
                 <span>{{$t('ord.orno')}}:{{item.ordername}}</span>
              </template>
              <template #default>
                 <span>{{$t('ord.oramount')}}:{{item.meney}}</span>
              </template>  
            </van-cell>
             
            </van-cell-group>
            <van-empty v-if="showempty" :description="$t('sumbymonth.emptip')" />
    </div>
</template>

<script>
export default {
   name:'order',
   data(){
       return {
           showempty:false,
           mind:new Date(2020,1,1),
          tabledata:[],
          showCalendar: false,
          par:{
              orderDate:'',
              userId:0,
              blfalge:0              
          },
          option1:[
               { text: this.$t('shporder.dfh'), value: 0 },
               { text: this.$t('shporder.yfh'), value: 1 },
          ]

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
     var sd=date.getDate()-3
      if(m<10) m="0"+m
       if(d<10) d="0"+d
       if(sd<10) sd="0"+sd
     var nowdate=y+'-'+m+'-'+d
     var sdate=y+'-'+m+'-'+sd
     if(!sessionStorage.getItem('ord')) 
        this.par.orderDate= sdate+"|"+nowdate
     else this.par.orderDate=sessionStorage.getItem('ord')
     
     this.par.userId=localStorage.getItem('userid') 
      this.$post('Order/QueryOrderListforUser',this.par).then(x=>{
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
      console.log(date)
      const [star,end]=date
    
      this.par.orderDate= this.formatter(star)+"|"+this.formatter(end)
      
      sessionStorage.setItem('ord',this.par.orderDate)
      this.showCalendar = false;
      this.par.userId=localStorage.getItem('userid')  
      
      this.$post('Order/QueryOrderListforUser',this.par).then(x=>{
          console.log(x.data)
         if(x.data.length>0){
             this.tabledata=x.data             
         }else this.showempty =true     

      })
    },
    formatter(date){
       let y=date.getFullYear()
       let m=date.getMonth()+1
       let d=date.getDate()
       if(m<10) m="0"+m
       if(d<10) d="0"+d
       //date=new Date(y+"-"+m+"-"+d)
       return y+"-"+m+"-"+d
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
     }
   }
}
</script>

<style>

</style>