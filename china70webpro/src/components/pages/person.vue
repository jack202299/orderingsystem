<template>
  <div>
      
<van-card 
  
  desc="描述信息"
  :title="user"
  thumb="https://pixabay.com/static/img/profile_image_dummy.svg"
/>
       <van-grid :column-num="3">
        <van-grid-item v-if="show1" icon="add-o"  text="管理" @click="goshopadmin" />
        <van-grid-item icon="orders-o" :text="$t('ps.buyer')"  to='/custmoter' />
        <van-grid-item icon="photo-o" :text="$t('ps.seller')" @click="gomyshop" />

        <van-grid-item icon="photo-o" :text="$t('ps.qrcode')" to='/showqrcode' />
         <van-grid-item icon="photo-o" :text="$t('ps.editpwd')" @click="openeditpwd" />
          <van-grid-item icon="photo-o" :text="$t('ps.about')" to="/about" />
        <van-grid-item icon="close" :text="$t('ps.out')" @click="onclose" />
        </van-grid>
  
     
   
        <van-dialog v-model="show3" :title="$t('editpwd.title')" 
        :confirmButtonText="$t('editpwd.save')"
        @confirm="onSubmitt"
        show-cancel-button>
              <van-form @submit="onSubmit" ref="form">
              <van-field
                v-model="par.oldpwd"
                 type="password"
                name="oldpwd"
                :label="$t('editpwd.old')"
                :placeholder="$t('editpwd.oldtip')"
                :rules="[{ required: true, message: '请填写旧密码(Molimo unesite staru lozinku)' }]"
              />
              <van-field
                v-model="par.newpwd"
                type="password"
                name="oldpwd"
                :label="$t('editpwd.newpwd')"
                :placeholder="$t('editpwd.newtip')"
                :rules="[{ required: true, message: '请填写新密码(Molimo unesite novu lozinku)' }]"
              />
           
           </van-form>
      
      </van-dialog>
  
  </div>



</template>

<script>
export default {
  name:'person',
  data(){
      return{
         show:false,
         user:'',
        
         show1:false,
         show2:false,
         show3:false,
         par:{
           UserId:0,
           oldpwd:'',
           newpwd:''
         }
      }
  },
  created(){
     
   
  },
  mounted(){
   // if(!sessionStorage.getItem("token")) this.$router.replace({path:'/index'})
    var uid=localStorage.getItem("userid")
     this.$fetch('User/QueryUserInfo',{Id:uid}).then(r=>{
       console.log(r.result)
       if(r.code==0){
         if(r.result.identitytype=="admin")  this.show1=true
       }
     })
    this.user=localStorage.getItem('user')
  },
  methods:{
      goshopadmin(){

         this.$router.push({path:'/adminpage'})
      },
           
      gomyshop()
      {
        var userid=localStorage.getItem('userid')
        this.$fetch('Shop/QueryShopInfo',{UserId:userid}).then(x=>{
          console.log(x)
          if(x.code==1){
              var sid=x.data.id
              //this.$router.push({path:'/shoporder',query:{ shopid:sid }})
               this.$router.push({path:'/seller'})
          }else
          {
              this.$toast(x.msg)
          }
        })
      },
      onclose(){
        localStorage.removeItem("userid")
        localStorage.removeItem("email")
        localStorage.removeItem("user")
        localStorage.removeItem("pwd")
        localStorage.removeItem("bl")
        localStorage.removeItem("idtype")          
        sessionStorage.clear();
        this.$router.push({path:'/index'})
      },
      openeditpwd(){
         this.par.UserId=localStorage.getItem('userid')
         this.show3=true
        
      },
      onSubmitt(){
         this.$refs.form.submit();
      },
      onSubmit(){
         this.$post('User/EditUpdatePwd',this.par).then(x=>{
           this.$toast(x.msg)
         })   
      },

      downloadFile(content, fileName) {
    var base64ToBlob = function(code) {
        let parts = code.split(';base64,');
        let contentType = parts[0].split(':')[1];
        let raw = window.atob(parts[1]);
        let rawLength = raw.length;
        let uInt8Array = new Uint8Array(rawLength);
        for(let i = 0; i < rawLength; ++i) {
            uInt8Array[i] = raw.charCodeAt(i);
        }
        return new Blob([uInt8Array], {
            type: contentType
        });
    };
    let aLink = document.createElement('a');
    let blob = base64ToBlob(content); //new Blob([content]);
    let evt = document.createEvent("HTMLEvents");
    evt.initEvent("click", true, true); //initEvent 不加后两个参数在FF下会报错  事件类型，是否冒泡，是否阻止浏览器的默认行为
    aLink.download = fileName;
    aLink.href = URL.createObjectURL(blob);
    aLink.click();
      }

  }
 
}
</script>

<style>

</style>