<template>
    <i18n-t tag="span" :keypath="getTitle" scope="global" ></i18n-t>
    <!-- <span>{{$t(getTitle)}}</span> -->
</template>

<script lang="ts">
export default {
    name: 'TitleI18n'
};
</script>

<script setup lang="ts">
import {  computed } from 'vue';
import { useLocaleStore } from '@/store/module/locale';

let props = defineProps({
    title: {
        type: [String, Object] as PropType<string | Title18n>,
        required: true,
        default: ''
    }
});

let localeStore = useLocaleStore();

let getTitle = computed(() => {
    let { title = '' } = props;
    if (typeof title === 'object') {
        return title?.[localeStore.locale] ?? title;
    }
    return title;
});
</script>
