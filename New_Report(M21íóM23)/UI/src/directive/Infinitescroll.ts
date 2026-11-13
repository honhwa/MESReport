import { App } from 'vue';
const loadMore = (
    PageSetting: {
        List: any[];
        moreData: any[];
    },
    size: number,
    controller: AbortController
) => {
    PageSetting.List = PageSetting.moreData.slice(0, size);
    //   滚动到底的时候移除scroll监听器
    if (size >= PageSetting.moreData.length) {
        // 移除监听器
        console.log("取消監聽");
        
        controller.abort();
    }
};
export default function showbutton(app: App) {
    app.directive('Infinitescroll', {
        mounted(el: HTMLInputElement, binding, vnode, prevVnode) {
            // 一次加载条数
            let initsize = Number(binding.arg) ?? 40;
            let pagesize: number = Number(binding.arg) ?? 40;
            let PageSetting: any = binding.value.PageSetting;
            //ant design vue 外层滚动元素是ant-table-body
            let div = document.querySelector('.ant-table') as HTMLElement;
            // 取消监听
            const controller = new AbortController();
            loadMore(PageSetting, pagesize, controller);
            if (div) {
                div.addEventListener(
                    'scroll',
                    () => {
                        if (
                            // clientHeight： 在页面上返回内容的可视高度（不包括边框，边距或滚动条）
                            // scrollHeight： 返回整个元素的高度（包括带滚动条的隐蔽的地方）
                            // scrollHeight与scrollTop与clientHeight的关系如下：
                            // scrollHeight = scrollTop + clientHeight
                            Math.abs(div.scrollHeight - div.clientHeight - div.scrollTop) < 1
                        ) {
                            loadMore(PageSetting, (pagesize += initsize), controller);
                            console.log('当前的pagesize是', pagesize);
                        }
                    },
                    {
                        passive: false,
                        //@ts-ignore
                        signal: controller.signal
                    }
                );
            }
        }
    });
}
