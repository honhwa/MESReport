<template>
    <Tooltip :title="getTitle" placement="bottom" :mouse-enter-delay="0.5">
        <span @click="toggle">
            <FullscreenOutlined v-if="!isFullscreen" />
            <FullscreenExitOutlined v-else />
        </span>
    </Tooltip>
</template>
<script lang="ts" setup>
import { computed, unref, inject, defineProps } from 'vue';
import { useFullscreen } from '@vueuse/core';
import { Tooltip } from 'ant-design-vue';
import { FullscreenExitOutlined, FullscreenOutlined } from '@ant-design/icons-vue';

const domdata = inject('domRef') as HTMLElement;



const { toggle, isFullscreen } = useFullscreen(domdata);
const getTitle = computed(() => {
    return unref(isFullscreen) ? '退出全屏' : '进入全屏';
});
</script>
