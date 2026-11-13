<template>
    <div>
        <SettingOutlined @click="showDrawer" />
        <Drawer v-model:visible="visible" placement="right" :closable="false">
            <Descriptions title="整体风格" :column="5">
                <Descriptions.Item v-for="theme in themeStyle" :key="theme.value">
                    <Tooltip :title="theme.label">
                        <div
                            class="style-checbox-item"
                            :class="{ active: themeStore.navTheme === theme.value }"
                            @click="setNavTheme(theme.value)"
                        >
                            <SvgIcon :name="theme.value" size="50"></SvgIcon>
                        </div>
                    </Tooltip>
                </Descriptions.Item>
            </Descriptions>
            <!-- <Descriptions title="主题色" :column="9">
                <Descriptions.Item v-for="item in themeColors" :key="item.key">
                    <div class="style-checbox-item">
                        <Tooltip :title="item.title">
                            <Tag :color="item.value" @click="setThemeColor({ primaryColor: item.value })">
                                <span :style="{ visibility: getThemeColorVisible(item.value) }"> ✔ </span>
                            </Tag>
                        </Tooltip>
                    </div>
                </Descriptions.Item>
                <Descriptions.Item key="custom">
                    <div class="style-checbox-item">
                        <Tooltip title="自定义">
                            <Tag :color="customColor" class="relative overflow-hidden">
                                <input
                                    v-model="customColor"
                                    type="color"
                                    class="absolute inset-0"
                                    :style="colorPickerStyle"
                                    @input="setThemeColor({ primaryColor: customColor })"
                                />
                                <span :style="{ visibility: getThemeColorVisible(customColor) }"> ✔ </span>
                            </Tag>
                        </Tooltip>
                    </div>
                </Descriptions.Item>
            </Descriptions> -->
            <Descriptions title="导航模式" :column="5">
                <Descriptions.Item v-for="item in layouts" :key="item.value">
                    <div class="style-checbox-item" :class="{ active: themeStore.layout === item.value }" @click="setLayout(item.value)">
                        <svg-icon :name="item.value" size="50"></svg-icon>
                    </div>
                </Descriptions.Item>
            </Descriptions>
            <Descriptions title="页签模式" :column="9">
                <Descriptions.Item v-for="item in tabsTheme" :key="item.key">
                    <div class="style-checbox-item">
                        <Tooltip :title="item.title">
                            <Tag :color="item.tabBgcolor" @click="setTabsTheme({ tabBgcolor: item.tabBgcolor })">
                                <span :style="{ visibility: getTabsThemeVisible(item.tabBgcolor) }"> ✔ </span>
                            </Tag>
                        </Tooltip>
                    </div>
                </Descriptions.Item>
            </Descriptions>
        </Drawer>
    </div>
</template>
<script lang="ts">
export default {
    name: 'ProjectSetting'
};
</script>
<script lang="ts" setup>
import { ref, computed, StyleValue } from 'vue';
import { SettingOutlined } from '@ant-design/icons-vue';
import { Drawer, Descriptions, Tag, Tooltip } from 'ant-design-vue';
import { layouts, themeColors, themeStyle,tabsTheme } from './constant';
import type { ThemeName, Theme, ThemeState,TabsTheme } from '@/store/module/projectConfig';
import { useThemeStore } from '@/store/module/projectConfig';
import { SvgIcon } from '../../../svg-icon/index';

// defineOptions({
//   name: 'ProjectSetting',
// });

const themeStore = useThemeStore();
const customColor = ref(themeStore.primaryColor);
const visible = ref(false);

const colorPickerStyle = computed(() => ({ '--custom-color': customColor.value } as StyleValue));

const setNavTheme = (theme: ThemeName) => {
    themeStore.setTheme({ navTheme: theme });
};
const setLayout = (layout: ThemeState['layout']) => {
    themeStore.setTheme({ layout });
};

const setThemeColor = ({ primaryColor }: Theme) => {
    themeStore.fillColor({ primaryColor });
};
const setTabsTheme = ({ tabBgcolor }: TabsTheme) => {
    let data = tabsTheme.find(item=>item.tabBgcolor == tabBgcolor)
    themeStore.setTabsTheme(data!);
};

const getThemeColorVisible = (color: string) => (themeStore.primaryColor === color ? 'visible' : 'hidden');

const getTabsThemeVisible = (color: string) => (themeStore.tabBgcolor === color ? 'visible' : 'hidden');
// const getImageUrl = (theme: ThemeName) => {
//   return new URL(`/src/assets/icons/${theme}.svg`, import.meta.url).href;
// };

const showDrawer = () => {
    visible.value = true;
};
</script>

<style lang="less" scoped>
.style-checbox-item {
    position: relative;
    cursor: pointer;

    &.active:after {
        content: '✔';
        position: absolute;
        bottom: 10px;
        right: 12px;
        color: rgb(24, 144, 255);
    }
}

input[type='color'] {
    width: 40px;
    height: 40px;
    border: 0;
    padding: 0;
    outline: none;
    -webkit-appearance: none;

    &::-webkit-color-swatch-wrapper {
        background: var(--custom-color);
    }

    &::-webkit-color-swatch {
        display: none;
    }
}
</style>
