<template>
  <div class="login-box">
    <div class="bg-page">
      <!-- <span class="originspan">INNOLUX</span> -->
      <div class="CompanyLogo">
        <template v-for="item in state.logData">
          <span>{{ item }}</span>
        </template>
        <span>{{ state.companyTitle }}</span>
        <!-- <div :title="CompanyLogo"></div> -->
      </div>
      <!-- <img src="~@/assets/images/Inx.png" class="image" /> -->
    </div>
    <div class="login-container">
      <div class="login-logo">
        <!-- <svg-icon name="logo" :size="45" /> -->
        <!-- <img src="~@/assets/images/logo.png" width="45" /> -->
        <h1 class="SystemTitle">{{ state.systemTitle }}</h1>
        <!--windicss挺好用的但是作者已经不再维护更新-->
      </div>

      <a-form layout="horizontal" :model="state.formInline" @submit.prevent="handleSubmit">
        <a-form-item>
          <a-input
            v-model:value="state.formInline.UserID"
            size="large"
            placeholder="rootadmin"
            autofocus
          >
            <template #prefix><user-outlined type="user" /></template>
          </a-input>
        </a-form-item>
        <a-form-item>
          <a-input-password
            v-model:value="state.formInline.Password"
            size="large"
            placeholder="123456"
            autocomplete="new-password"
          >
            <template #prefix><lock-outlined type="user" /></template>
          </a-input-password>
        </a-form-item>
        <a-form-item>
          <a-select v-model:value="state.company" :options="CompanyMod" @change="GetCompanyLog">
            <!-- <template #prefix><safety-outlined type="user" /></template> -->
          </a-select>
        </a-form-item>
        <a-form-item>
          <a-button type="primary" html-type="submit" size="large" :loading="state.loading" block>
            登录
          </a-button>
        </a-form-item>
      </a-form>
      <PageFooter />
    </div>
    <!--  -->
  </div>
</template>

<script setup lang="tsx">
  import { reactive, onMounted } from 'vue';
  import { UserOutlined, LockOutlined, SafetyOutlined } from '@ant-design/icons-vue';
  import { useRoute, useRouter } from 'vue-router';
  import { message, Modal } from 'ant-design-vue';
  import { useUserStore } from '@/store/module/user';
  import PageFooter from '@/components/LayoutJSX/footer/index';
  import { CompanyData, CompanyMod } from './index';
  import { Storage } from '@/utils/Storage';

  const state = reactive({
    loading: false,
    captcha: '',
    formInline: {
      UserID: '',
      Password: '',
      UserType: 'Auto',
      SysID: 'FS_TABLEAU_MCF',
    },
    company: 'CARUX',
    companyTitle: '',
    systemTitle: '',
    logData: [] as any,
  });

  const route = useRoute();
  const router = useRouter();
  const userStore = useUserStore();

  const GetCompanyLog = () => {
    CompanyData.forEach((item) => {
      if (item.CompanyLog != state.company || !item.CompanyLog) {
        userStore.setAvatar('INNOLUX');
      } else {
        state.companyTitle = item.CompanyName;
        state.systemTitle = item.SystemTitle;
        state.logData = item.LogData;
        userStore.setAvatar(state.company);
      }
    });
  };
  const handleSubmit = async () => {
    const { UserID, Password } = state.formInline;
    let UserParams = Storage.get('IsSSO');
    if (UserID.trim() == '' || Password.trim() == '') {
      return message.warning('用户名或密码不能为空！');
    }
    if (UserParams) return SkipSSOLogin(UserID, Password);
    try {
      message.loading('登录中...', 0);
      state.loading = true;
      await userStore.login(state.formInline).finally(() => {
        state.loading = false;
        message.destroy();
      });
      message.success('登录成功！');
      setTimeout(() => router.replace((route.query.redirect as string) ?? '/'), 100);
    } catch (error: any) {
      Modal.error({
        title: () => '提示',
        content: () => error.message ?? error.Message ?? error,
      });
    }
  };
  const SkipSSOLogin = async (User: string, Password: string) => {
    if (
      (User == 'rootadmin' && Password == 'rootadmin') ||
      (User == 'admin' && Password == 'admin')
    ) {
      Storage.set('UserMessage', { MemID: User, MemName: User }, null);
      Storage.set('ACCESS_TOKEN', User, null);
      try {
        message.loading('登录中...', 0);
        state.loading = true;
        await userStore.afterLogin().finally(() => {
          state.loading = false;
          message.destroy();
        });
        message.success('登录成功！');
        setTimeout(() => router.replace((route.query.redirect as string) ?? '/'), 100);
      } catch (error: any) {
        Modal.error({
          title: () => '提示',
          content: () => error.message ?? error.Message ?? error,
        });
      }
    } else {
      Modal.error({
        title: () => '提示',
        content: () => '账号密码错误!',
      });
    }
  };

  onMounted(() => {
    Storage.set('IsSSO', false, null);
    GetCompanyLog();
  });
</script>
<script lang="tsx">
  export default {
    name: 'Login',
  };
</script>
<style lang="less" scoped>
  .login-box {
    display: flex;
    width: 100vw;
    height: 100vh;
    // padding-top: 240px;
    // background: url('~@/assets/images/login.svg');
    background-size: 100%;
    flex-direction: column-reverse;
    align-items: center;
    overflow: hidden;

    &::before {
      position: absolute;
      top: 0;
      left: 0;
      width: 100%;
      height: 100%;
      margin-left: -48%;
      background: url('~@/assets/images/login.svg');
      background-position: 100%;
      background-repeat: no-repeat;
      background-size: auto 100%;
      z-index: 1;
      content: '';
    }
    .bg-page {
      position: absolute;
      top: 35%;
      left: 3%;
      width: 50%;
      z-index: 2;
      content: '';
      .CompanyLogo {
        color: rgba(27, 122, 186, 1);
        font-style: italic;
        font-weight: 900;
        font-size: 70px;
        span:nth-child(1) {
          font-size: 90px;
        }
        span:nth-child(3) {
          color: rgba(192, 38, 46, 1);
          font-size: 90px;
        }
        span:nth-child(4) {
          color: rgba(105, 165, 69, 1);
        }
        span:nth-child(6) {
          margin-left: 2%;
          font-weight: 500;
          color: rgba(44, 33, 5, 1);
          font-size: 60px;
        }
      }
    }
    .login-page {
      display: flex;
      width: 50%;
      .login-logo {
        display: flex;
        margin-bottom: 30px;
        align-items: center;

        .svg-icon {
          font-size: 48px;
        }
      }
    }

    :deep(.ant-form) {
      width: 350px;
      padding-top: 50px;
      // background: rgba(233, 240, 243, 0.5);
      // .ant-col {
      //     width: 90%;
      // }

      .ant-form-item-label {
        padding-right: 6px;
      }
      .a-form-item {
        padding-top: 20px;
      }
    }
    .login-container {
      position: absolute;
      top: 20%;
      right: 250px;
      width: 450px;
      height: 550px;
      padding: 50px;
      display: flex;
      flex-flow: column;
      align-items: center;
      border-radius: 20px;
      background-color: rgba(255, 255, 255, 0.5);
      box-shadow: 4px 4px 4px rgba(0, 0, 0, 0.25), -3px -3px 4px rgba(255, 255, 255, 0.15);
      backdrop-filter: blur(10px);
      border-bottom: 2px solid blueviolet;
      border-right: 2px solid blueviolet;
      border-top: 2px solid white;
      border-left: 2px solid white;
      transition-duration: 1s;
      transition-property: border-top, border-left, border-bottom, border-right, box-shadow;

      &:hover {
        border-top: 2px solid blueviolet;
        border-left: 2px solid blueviolet;
        border-bottom: 2px solid rgb(238, 103, 238);
        border-right: 2px solid rgb(238, 103, 238);
        box-shadow: rgba(240, 46, 170, 0.4) 5px 5px, rgba(240, 46, 170, 0.3) 10px 10px,
          rgba(240, 46, 170, 0.2) 15px 15px, rgba(240, 46, 170, 0.1) 20px 20px,
          rgba(240, 46, 170, 0.05) 25px 25px;
      }
    }
  }
  .SystemTitle {
    font-size: 25px;
    text-align: center;
    padding-top: 10px;
    // letter-spacing: 1px;
    font-weight: bold;
    color: rgb(15, 16, 16);
    font-family: 'Lucida Calligraphy', cursive, serif, sans-serif;
  }
</style>
