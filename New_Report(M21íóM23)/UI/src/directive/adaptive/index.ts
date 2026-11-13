
// import Vue from '*.vue'
import adaptive from './adaptive'
 
const install = function(Vue) {
  // 绑定v-adaptive指令
  Vue.directive('adaptive', adaptive)
}
if (Reflect.has(window,'Vue')) {
  window['adaptive'] = adaptive
  // eslint-disable-next-line no-undef
  //@ts-ignore
  Vue.use(install)
}
 //@ts-ignore
adaptive.install = install
 
export default adaptive
