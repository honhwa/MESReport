import { App } from "vue";
// 导入指令

import Infinitescroll from './Infinitescroll';
//封装全局指令
export default function registerDirective(app:App) {
    Infinitescroll(app);
}
