<template>
  <div>
    <a-button @click="goback" class="backButton">返回上級目錄</a-button>

    <a-card class="routerpage">
      <ol>
        <li v-for="item in Floder" :key="item.name" class="file">
          <a @click="GetFolderData(item.path)"  v-if="item.type == 'folder'">
            <icon-font
            type="wenjian"
            size="22"
            class="fileicon"
          /><span class="filename">{{item.name}}</span></a>
          <a :href="BASE_URL + item.path.replace('/app', '')" v-else>
            <icon-font
            type="wendang"
            size="22"
            class="fileicon"
          /><span class="filename">{{item.name}}</span></a>
        </li>
      </ol>
      <!-- <router-view></router-view> -->
    </a-card>
  </div>
</template>

<script lang="ts" setup>
  import { onMounted, ref } from 'vue';
  import { GetCommon } from '@/api/pages/common';
  import { IconFont } from '@/components/basic/icon-font';
  import { message } from 'ant-design-vue';
  import { useRoute } from 'vue-router';

  interface foldertype {
    name: string;
    path: string;
    type: string;
  }
  const FolderFab = {
    LCM1: [
      {
        name: 'MFG_Share_Folder',
        path: '/share/lcmreport',
        type: 'folder',
      },
      {
        name: 'FSA_Share_Folder',
        path: '/share/NHCMI',
        type: 'folder',
      },
    ],
    BEOL1: [
      {
        name: 'IE_Share_Folder',
        path: '/share/IE',
        type: 'folder',
      },
      {
        name: 'MFG_Report_Folder',
        path: '/share/lcdreport',
        type: 'folder',
      },
      {
        name: 'PM_Share_Folder',
        path: '/share/PM',
        type: 'folder',
      },
      {
        name: 'ENG_Share_Folder',
        path: '/share/ENG',
        type: 'folder',
      },
      {
        name: 'AMHS_Share_Folder',
        path: '/share/AMHS',
        type: 'folder',
      },
      {
        name: 'Test_Share_Folder',
        path: '/share/Test',
        type: 'folder',
      },
    ],
  };
  const route = useRoute();
  let Floder = ref<foldertype[]>([]);
  let FolderHistory = ref<string[]>([]);
  let BASE_URL = process.env.VUE_APP_BASE_API;
  // let count = ref<number>(0);

  async function GetFolderData(directory: string) {
    directory = directory.replace('/app/documents', '');
    let params = {
      apiName: 'ReadFolder',
      directory,
      // directory: '//10.189.204.201/cminlfs$/public/'
    };
    let { api01 } = await GetCommon(params);
    if (!FolderHistory.value.includes(directory)) FolderHistory.value.push(directory);
    // count.value++;
    Floder.value = api01;
  }
  /**
   * 返回上一层目录
   */
  function goback() {
    //第一个保存的为文件根目录
    if (FolderHistory.value.length >= 2) {
      GetFolderData(FolderHistory.value[FolderHistory.value.length - 2]);
      FolderHistory.value.pop();
    } else if (FolderHistory.value.length == 1) {
      Floder.value = FolderFab[route.meta.Owner??'LCM1']
      FolderHistory.value.pop();
    } else {
      message.warning('此文件已無上層目錄');
    }

    // FolderHistory.value[count.value - 2];
  }
  onMounted(async () => {
    Floder.value = FolderFab[route.meta.Owner??'LCM1']
  });
</script>
<script lang="ts">
  export default {
    name: 'sharepage',
  };
</script>
<style scoped lang="less">
  .routerpage {
    padding: 20px;
    font-size: 20px;
    height: calc(100vh - 150px);
    overflow-y: auto;
    .file a {
        text-decoration: none;
        line-height: 34px;
        position: relative;
        padding-top: 2px;
        // display: inline-block;
    }
    .file a::before {
        content: '';
        display: block;
        position: absolute;
        height: 2px;
        background: rgb(92, 91, 184);
        width: 0;
        left: auto; /* left 和 right 的值是关键 */
        right: 0;
        bottom: 0;
        transition: all 0.5s;
    }
   .file a:hover::before {
        width: 100%;
        left: 0; /* left 和 right 的值是关键 */
        right: auto;
    }
    .filename {
      color: rgb(92, 91, 184);
      margin-left: 3%;
    }
    .backButton{
        margin:0 0 1% 3%;
    }
    .fileicon {
      position: absolute;
      margin: 5px 0 0 15px;
    }
  }
</style>
