import  { createApp } from 'vue'
import App from './App.vue'
import { setupRouter } from './router'
import { setupStore } from './store'
import { setupAntd,setupAssets,setupCustomComponents,setupVexTable } from '@/plugins';
import registerDirective from '@/directive/index';
import { setupI18n } from '@/locales';

// createApp(App).use(store).use(router).mount('#app')
const app = createApp(App);
function setupPlugins() {
  // 注册全局常用的ant-design-vue组件
  setupAntd(app);
  setupVexTable(app);
  // 引入静态资源
  setupAssets();
  setupCustomComponents(app);
  registerDirective(app)
}
async function setupApp() {
    // 挂载vuex状态管理
    // app.use(ElementPlus);
    setupStore(app);
    setupI18n(app);
    await setupRouter(app);//路由挂載要在app節點挂載之前
    app.mount('#app');
  }
setupPlugins();
setupApp();
