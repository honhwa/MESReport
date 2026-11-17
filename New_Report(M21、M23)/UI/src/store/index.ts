import { createPinia } from 'pinia';
import type { App } from 'vue';

const store = createPinia();

export function setupStore(app: App<Element>) {
  app.use(store);
}

export { store };
//没有模块的嵌套结构 
//Pinia 通过设计提供扁平结构，就是说每个 store 都是互相独立的，谁也不属于谁，也就是扁平化了
//更好的代码分割且没有命名空间。当然你也可以通过在一个模块中导入另一个模块来隐式嵌套 store，甚至可以拥有 store 的循环依赖关系

