import { defineComponent } from 'vue';

import { GithubOutlined, CopyrightOutlined } from '@ant-design/icons-vue';
import { Layout } from 'ant-design-vue';
import styles from './index.module.less';

const { Footer: ALayoutFooter } = Layout;

export default defineComponent({
  name: 'PageFooter',
  components: { ALayoutFooter },
  setup() {
    return () => (
      <>
        <a-layout-footer class={styles.page_footer}>
          
          <div class={styles.copyright}>
            Copyright 2023 群丰骏科技（上海）有限公司<CopyrightOutlined />
          </div>
        </a-layout-footer>
      </>
    );
  },
});
