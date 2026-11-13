<template>
    <transition name="slide-up">
        <!--这里把LockScreen包一层单纯为了实现过度效果-->
        <LockScreen v-if="isLock" />
    </transition>
</template>

<script setup>
import LockScreen from './LoockScreen.vue';
import { useLockscreenStore } from '@/store/module/LockScreen';
import { computed, onMounted, reactive, ref } from '@vue/runtime-core';
import { Storage } from '@/utils/Storage';
// import { VerifyToken } from '@/api/account';

let params = Storage.get('ACCESS_TOKEN', null);

const userlock = useLockscreenStore();
const isLock = computed(() => userlock.isLock);
</script>
<style scoped>
.slide-up-enter-active {
    animation: slide-up 0.5s;
}

.slide-up-leave-active {
    animation: slide-up 0.5s reverse;
}
@keyframes slide-up {
    0% {
        transform: translateY(-100%);
    }

    100% {
        transform: translateY(0);
    }
}
</style>