import type { App } from 'vue';

import { SvgIcon } from '@/components/LayoutJSX/svg-icon';
import { IconFont } from '@/components/basic/icon-font';
import { TitleI18n } from '@/components/basic/title-i18n';

/**
 * 全局注册自定义组件
 * @param app
 */
export function setupCustomComponents(app: App) {
  app.component(SvgIcon.name, SvgIcon);
  app.component(IconFont.name, IconFont);
  app.component(TitleI18n.name, TitleI18n);
}
