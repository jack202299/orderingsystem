// The Vue build version to load with the `import` command
// (runtime-only or standalone) has been set in webpack.base.conf with an alias.
import Vue from 'vue'
import App from './App'
import router from './router'
import Vant from 'vant';
import 'vant/lib/index.css';
import VueI18n from 'vue-i18n'
import {post,fetch} from './units/http'
import tools from './units/tools';


Vue.config.productionTip = false
Vue.use(Vant);
Vue.use(VueI18n)
Vue.prototype.$tools=tools;
Vue.prototype.$post=post;
Vue.prototype.$fetch=fetch;

const i18n = new VueI18n({
  locale: 'zh-CN',    // 语言标识
  //this.$i18n.locale // 通过切换locale的值来实现语言切换
  messages: {
    'zh-CN': require('../lang/zh'),   // 中文语言包
    'en-US': require('../lang/en'),    // 英文语言包
    'sr-SR':require('../lang/sr')
  }
})

/* eslint-disable no-new */
new Vue({
  el: '#app',
  router,
  i18n,
  components: { App },
  template: '<App/>'
})
// router.beforeEach((to, from, next) => {
//   if (to.path === '/index') {
//     next();
//   } else {
//     let token = sessionStorage.getItem('token');
 
//     if (token === null || token === '') {
//       next('/index');
//     } else {
//       next();
//     }
//   }
// });