<template>
    <ConfigProvider :locale="getAntdLocale">
        <router-view #="{ Component }">
            <component :is="Component" name="BaerRoot" />
        </router-view>

        <LockScreen />
        <!-- <div>{{ LockScreen.setup.params }}123</div> -->
    </ConfigProvider>
</template>

<script setup lang="ts">
import { LockScreen } from '@/components/Weather/index';
import { ConfigProvider } from 'ant-design-vue';
import { useLocale } from '@/locales/useLocale';


const { getAntdLocale } = useLocale();

</script>


