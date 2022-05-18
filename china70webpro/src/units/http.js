import axios from 'axios';
axios.defaults.timeout = 50000;
//axios.defaults.baseURL ='/Api';
axios.defaults.baseURL ='https://lotus.srbcnshop.com';
axios.interceptors.request.use(
    config => {
        config.headers={
            'Content-Type':'application/json',
            'token':sessionStorage.getItem('token')
          }
        return config;
    },
    error => {
        console.log(error);
        return Promise.reject();
    }
);

axios.interceptors.response.use(
    response => {
        if (response.status === 200) {
            return response.data;
        } else {
            Promise.reject();
        }
    },
    error => {
        console.log(error);
        return Promise.reject();
    }
);

export function post(url,data = {}){
    return new Promise((resolve,reject) => {
      axios.post(url,data)
           .then(response => {
             resolve(response);
           }).catch(err => {
             reject(err)
           })
    })
  }



  export function fetch(url,params={}){
    return new Promise((resolve,reject) => {
      axios.get(url,{
        params:params
      })
      .then(response => {
        resolve(response);
      })
      .catch(err => {
        reject(err)
      })
    })
  }
