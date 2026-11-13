
/*
 * @Description: echarts 自适应问题
 */
import { ref } from 'vue'
export default function () {
    const mounted = (tableHeight) => {
        window.onresize = () => {
            return (() => {
              tableHeight.value.y = document.body.clientHeight - 430;
            })();
          };
      }
}
