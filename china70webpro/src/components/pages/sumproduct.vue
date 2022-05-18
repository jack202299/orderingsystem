<template>
  <div>
    <van-nav-bar fixed
  title="拍产品统计"
  left-text="返回"
  right-text="提交"
  left-arrow
  @click-left="onClickLeft"
  @click-right="onClickRight"
/>
<van-cell title="选择多个日期" :value="text" @click="show = true" />
<van-calendar v-model="show" type="range" @confirm="onConfirm" :min-date="mindate" />
    <van-cell-group v-for="(item,index) in list" :key="index" :title="item.date">
     <van-cell v-for="(it,index) in item.sublist" :key="index" :title="it.name" :value="it.count" />
     
    </van-cell-group>
     <van-cell title="合计" :value="tol"  />
  </div>
</template>

<script>
export default {
   name:"sumproduct",
   data(){
      return {
        mindate:new Date(2010, 0, 1),
        text:'',
        show:false,
        list:[],
        tol:'',
        par:{
           shopid:0,
           querydate:[]
        }
      }
   },
   created(){
        
   },
   methods:{
     getdata(){
        this.par. shopid=this.$route.query.shopid
        this.$post('Product/QueryProductUpload',this.par).then(r=>{
          this.tol=r.tol
          this.list=r.list
        })
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
     onConfirm(date) {
      const [start, end] = date;
      this.show = false;
      this.text=`${this.formatDate(start)} - ${this.formatDate(end)}`;
      this.par.querydate.push(this.formatDate(start))
       this.par.querydate.push(this.formatDate(end));
    },
   }
}
</script>

<style>

</style>