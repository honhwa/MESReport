<template>
    <div class="logo">
        <img :src="logsrc" alt="" />
        <h2 v-show="!props.collapsed" class="title">{{systemname}}</h2>
    </div>
</template>

<script setup>
import { useUserStore } from '@/store/module/user';

// defineProps({
//     collapsed: {
//         type: Boolean
//     }
// });
const userStore = useUserStore();
const logsrc = `/${userStore.avatar}.ico`
const props = defineProps({
    collapsed: Boolean
});
// const systemname = process.env.VUE_APP_SYSID
const systemname = userStore.avatar;
// const turn = computed(() => `${props.collapsed ? 20 : 95}px`);
</script>

<style lang="less" scoped>
.logo {
    @apply flex overflow-hidden whitespace-nowrap items-center;

    //
    height: 64px;
    line-height: 64px;

    img {
        height: 32px;
        margin-right: 8px;
        padding-left: 25px;
    }
    .title {
        @apply mb-0 text-xl;
        color: rgb(24, 144, 255);
        // text-align: center;
        display: inline-block;
        letter-spacing: 5px;
    }
}
</style>
