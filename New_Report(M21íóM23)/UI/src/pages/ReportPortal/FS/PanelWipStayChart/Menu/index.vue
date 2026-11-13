<script setup lang="ts">
import { computed, onMounted, ref, nextTick } from "vue";
 import {IMenuList}  from './type'
const props = defineProps({
    onClose: { type: Function, default: () => { } },
    MenuList: {
        type: Array as () => Array<IMenuList>, //(string也可以是其他你自定义的接口),
        required: true,
        default: () => []
    }
});
// 创建组件ref
const contextMenu = ref<HTMLElement | null>(null);
onMounted(async () => {
    // 确保组件已经渲染
    await nextTick();
    // 触发组件focus
    if (contextMenu.value)
        contextMenu.value.focus();
});

const clickFunc = (action: Function) => {
    action();
    props.onClose();
};
</script>
<template>
    <div  class="context-menu" ref="contextMenu" @blur="props.onClose" tabindex="-1">
        <div v-for="(item, index) in props.MenuList" :key="index">
            <div class="divide" v-if="!item" />
            <div v-else class="context-menu__item" @click="clickFunc(item.onClick)">
                {{ item.label }}
            </div>
        </div>
    </div>
</template>
<style scoped lang="less">
.context-menu {
    position: fixed;
    padding: 12px 4px;
    border-radius: 6px;
    border: 1px solid rgba(222, 222, 222, 0.5);
    background-color: #ffffff;
    font-size: 14px;
    font-weight: 500;
    user-select: none;

    &:focus {
        outline: none;
    }

    .divide {
        height: 1px;
        background-color: rgba(222, 222, 222, 0.5);
        margin: 8px auto;
        width: calc(100% - 12px);
    }

    &__item {
        padding: 8px 12px;
        cursor: pointer;

        &:hover {
            color: #646cff;
            background-color: rgba(100, 108, 255, 0.1);
            border-radius: 6px;
        }
    }

    &__info {
        display: flex;
        padding: 0 12px;
    }

    &__img {
        width: 36px;
        height: 36px;
        margin-right: 6px;
    }

    &__name {
        font-size: 16px;
    }

    &__subname {
        color: #666;
        font-size: 12px;
    }
}
</style>

