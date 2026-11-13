import { defineStore } from 'pinia';
// import darkThemeCss from 'ant-design-vue/dist/antd.dark.css?raw';
import { ConfigProvider } from 'ant-design-vue';
import type { Theme } from 'ant-design-vue/es/config-provider/';
import { store } from '@/store';
import { Storage } from '@/utils/Storage';
import { useUserStoreWithOut } from '@/store/module/user';
import { computed } from 'vue';
import { tabsTheme } from '@/components/LayoutJSX/header/components/setting/constant';

const styleDom = document.createElement('style');
styleDom.dataset.type = 'theme-dark';
document.head.appendChild(styleDom);
const userStore = useUserStoreWithOut();

/**
 * 项目默认配置项
 * primaryColor - 默认主题色, 如果修改颜色不生效，请清理 localStorage
 * navTheme - sidebar theme ['dark', 'light'] 两种主题
 * colorWeak - 色盲模式
 * layout - 整体布局方式 ['sidemenu', 'topmenu'] 两种布局
 * fixedHeader - 固定 Header : boolean
 * fixSiderbar - 固定左侧菜单栏 ： boolean
 * contentWidth - 内容区布局： 流式 |  固定
 */

/** 主题色 */
export type ThemeName = 'light' | 'dark' | 'realDark';
export type { Theme };

export type ThemeState = {
  navTheme: ThemeName; // theme for nav menu
  primaryColor: string; // '#F5222D', // primary color of ant design
  layout: 'sidemenu' | 'topmenu'; // nav menu position: `sidemenu` or `topmenu`
  contentWidth: 'Fluid' | 'Fixed'; // layout of content: `Fluid` or `Fixed`, only works when layout is topmenu
  fixedHeader: false; // sticky header
  fixSiderbar: false; // sticky siderbar
  colorWeak: false;
  menu: {
    locale: true;
  };
  title: string;
  pwa: false;
  iconfontUrl: string;
  // production: process.env.NODE_ENV === 'production' && process.env.VUE_APP_PREVIEW !== 'true'
};
export type TabsTheme = {
  tabBgcolor: string;
  noBorder?: boolean;
  headHeight?: string;
  title?:string;
  key?:string;
  separator?:string;
}
export const defaultConfig: ThemeState = {
  navTheme: 'dark', // theme for nav menu
  primaryColor: 'rgb(24, 144, 255)', // '#F5222D', // primary color of ant design
  layout: 'sidemenu', // nav menu position: `sidemenu` or `topmenu`
  contentWidth: 'Fluid', // layout of content: `Fluid` or `Fixed`, only works when layout is topmenu
  fixedHeader: false, // sticky header
  fixSiderbar: false, // sticky siderbar
  colorWeak: false,
  menu: {
    locale: true,
  },
  title: 'vite-antdv-admin',
  pwa: false,
  iconfontUrl: '',
  // production: process.env.NODE_ENV === 'production' && process.env.VUE_APP_PREVIEW !== 'true',
} as any;
// const noBorder = false;
const noBorder =['FS'].includes(process.env.VUE_APP_SITE) ? userStore.perms == 'Admin' : true;
let tabsDefaultColor = noBorder? 'rgb(255, 255, 255)' : '#FED800'
// const headHeight = pureTabs ? '64px' : '34px'


export let tabsConfig: TabsTheme = tabsTheme.find(item=>item.tabBgcolor == tabsDefaultColor)!

const setRealDarkTheme = (navTheme?: ThemeName) => {
  if (navTheme === 'realDark') {
    document.documentElement.classList.add('dark');
    styleDom.textContent = 'darkThemeCss';
  } else {
    document.documentElement.classList.remove('dark');
    styleDom.textContent = '';
  }
};


let localThemeConfig: Partial<ThemeState>&Partial<TabsTheme> = {};
try {
  localThemeConfig = JSON.parse(Storage.get('THEME__', '{}'));
  const { primaryColor, navTheme,tabBgcolor } = localThemeConfig;
  navTheme && setRealDarkTheme(navTheme);
  if(tabBgcolor) tabsConfig=tabsTheme.find(item=>item.tabBgcolor == tabBgcolor)!;
  primaryColor &&
    ConfigProvider.config({
      theme: {
        primaryColor,
      },
    });
} catch {
  localThemeConfig = {};
}

export const useThemeStore = defineStore({
  id: 'theme',
  state: (): ThemeState&TabsTheme => ({
    ...defaultConfig,
    ...localThemeConfig,
    ...tabsConfig,
  }),
  getters: {
    getNavTheme(): ThemeName {

      return this.navTheme;
    },
  },
  actions: {
    setTheme(theme: Partial<ThemeState>) {
      for (const key in theme) {
        // @ts-ignore
        this[key] = theme[key];
      }
      // document.documentElement.style.setProperty(key, nextTheme[key]);
      setRealDarkTheme(theme.navTheme);
      Storage.set('THEME__', JSON.stringify(this.$state));
    },
    /** antdv自带的改变主题颜色方法，但可以配置的颜色很有限，仅6种 */
    fillColor(theme: Theme) {
      this.primaryColor = theme.primaryColor ?? this.primaryColor;
      ConfigProvider.config({
        theme,
      });
      Storage.set('THEME__', JSON.stringify(this.$state));
    },
    setTabsTheme(tabsTheme: TabsTheme) {
      for (const key in tabsTheme) {
        // @ts-ignore
        this[key] = tabsTheme[key];
      }
      Storage.set('THEME__', JSON.stringify(this.$state));
    }
  },
});

// Need to be used outside the setup
export function useThemeStoreWithOut() {
  return useThemeStore(store);
}
