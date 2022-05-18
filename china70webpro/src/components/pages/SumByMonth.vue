<template>
  <div>
       <van-nav-bar fixed
  :title="$t('sumbymonth.title')"
  :left-text="$t('sumbymonth.lefttext')"
  :right-text="$t('sumbymonth.righttext')"
  left-arrow
  @click-left="onClickLeft"
  @click-right="onClickRight"
/>
<van-cell :title="$t('sumbymonth.choosemonth')" :value="text" @click="show = true" />
<van-calendar v-model="
show" type="range" @confirm="onConfirm" :min-date="mindate" />
<van-cell-group>
  <van-cell v-for="(item,index) in list" :key="index" :title="item.date"  >
  <template #default>
      <span>{{$t('sumbymonth.sales')}}}{{item.m}}</span>
  </template>
   <template #label>
      <span>{{$t('sumbymonth.Numberoftransactions')}}{{item.icount}}</span>
   </template>
  </van-cell>
</van-cell-group>
  <van-empty v-if="showempty" :description="$t('sumbymonth.emptip')" />
  </div>
</template>

<script>
export default {
   name:'sumbymonth',
   data(){
     return {
        showempty:false,
        mindate:new Date(2010, 0, 1),
        text:'',
        show:false,
        list:[],
        par:{
           shopid:0,
           querydate:[]
        }
     }
   },
   created(){
       this.par.shopid=this.$route.query.shopid
   },
   methods:{
      onConfirm(date) {
      const [start, end] = date;
      this.show = false;
      this.text=`${this.formatDate(start)} - ${this.formatDate(end)}`;
      this.par.querydate.push(this.formatDate(start))
       this.par.querydate.push(this.formatDate(end));
    },
    onClickLeft() {
       this.$router.go(-1)
    },
    onClickRight() {
       this.getdata()
    },
     formatDate(date) {
      return `${date.getFullYear()}-${date.getMonth() + 1}-${date.getDate()}`;
    },
     getdata(){
      
        this.$post('Order/QuerySumByMonth',this.par).then(r=>{         
          console.log(r)         
          this.list=r
         if(r.length==0) this.showempty=true
        })
     },
   }
}
</script>

<style>

</style>