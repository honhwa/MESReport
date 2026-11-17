<template>
    <!-- <svg class="svg-icon" :style="getStyle" aria-hidden="true">
        <use :xlink:href="symbolId" />
    </svg> -->
    <!--在项目中配置svg-icon 组件太麻烦了，发现用img也是一样的效果我就写成这样了-->
    <img :src="menu[props.name]"/>
</template>
<script lang="ts">
export default {
    name: 'SvgIcon'
};
</script>
<script lang="ts" setup>
import { computed, CSSProperties } from 'vue';

const importAll = (context) => {
    const map = {};
    for (const key of context.keys()) {
        const keyArr = key.split('/');
        keyArr.shift(); // 移除.
        map[keyArr.join('.').replace(/\.svg$/g, '')] = context(key);
    }
    return map;
};
// const importAll = (requireContext: __WebpackModuleApi.RequireContext) =>
//     requireContext.keys().forEach(requireContext);
//   try {
//     importAll(require.context('@/assets/icons', true, /\.svg$/));
//   } catch (error) {
//     console.log(error);
//   }
const menu = importAll(require.context('@/assets/icons', true, /\.svg$/));
//vue脚手架版本不对？我没全局安装？tsconfig配置错了？全部是正确的，能正常运行就是显示这个错误TT
//搞得我以为以为这里有问题弄了好久，真坑



const props = defineProps({
    prefix: {
        type: String,
        default: 'SvgIcon'
    },
    name: {
        type: String,
        required: true
    },
    size: {
        type: [Number, String],
        default: 16
    }
});
// const symbolId = computed(() => `#${props.prefix}-${props.name}`);

const getStyle = computed((): CSSProperties => {
    const { size } = props;
    const s = `${size}`.replace('px', '').concat('px');
    return {
        width: s,
        height: s
    };
});
</script>

<style scoped>
.svg-icon {
    overflow: hidden;
    vertical-align: -0.15em;
    fill: currentColor;
}
</style>
