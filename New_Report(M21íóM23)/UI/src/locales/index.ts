import { createI18n } from 'vue-i18n';
import { localeMap } from './config';
import { setHtmlPageLang, setLoadLocalePool } from './helper';
import type { App } from 'vue';
import ZH_CN from './lang/zh_CN';
import EN_US from './lang/en_US';
import { useLocaleStoreWithOut } from '@/store/module/locale';

 function createI18nOptions() {
  const localeStore = useLocaleStoreWithOut();
  const locale = localeStore.getLocale;
  // const defaultLocal = await import(`./lang/${locale}.ts`);
  // const message = EN_US.default?.message ?? {};

  setHtmlPageLang(locale);
  setLoadLocalePool((loadLocalePool) => {
    loadLocalePool.push(locale);
  });

  return {
    locale,
    // legacy: false,
    fallbackLocale: localeMap.zh_CN, // set fallback locale
    messages: {
      zh_CN: ZH_CN?.message ?? {},
      en_US: EN_US?.message
    },
    globalInjection: true,
    silentTranslationWarn: true, // true - warning off
    missingWarn: false,
    silentFallbackWarn: true,
  };
}

const options =  createI18nOptions();//因为无法开启顶层await,所以直接全部添加语言
export const i18n = createI18n(options);

// setup i18n instance with global
export async function setupI18n(app: App) {
  app.use(i18n);
}
