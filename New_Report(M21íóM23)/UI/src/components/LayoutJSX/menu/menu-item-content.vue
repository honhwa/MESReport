<template>
    <div>
        <icon-font :type="item?.meta?.icon"  :class="{ icon: item?.meta?.icon=='juxing-gengduo' }" />
        <TitleI18n :title="item?.meta?.title" />
    </div>
</template>

<script lang="ts">
export default{
  name:'MenuItemContent'
}
</script>

<script lang="ts" setup>
import type { RouteRecordRaw } from 'vue-router';
import { IconFont } from '@/components/basic/icon-font';
import { TitleI18n } from '@/components/basic/title-i18n';



// defineOptions({
//     name: 'MenuItemContent'
// });

defineProps({
    item: {
        type: Object as PropType<RouteRecordRaw>,
        default: () => ({})
    }
});

</script>

<style lang="less" scoped>
.icon {
  animation: rotate 2s linear infinite; /* 添加旋转动画 */
}

@keyframes rotate {
  from {
    transform: rotate(0deg); /* 从0度开始旋转 */
  }
  to {
    transform: rotate(360deg); /* 旋转一周，即360度 */
  }
}
</style>
