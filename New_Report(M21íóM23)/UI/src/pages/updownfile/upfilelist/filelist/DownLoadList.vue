<script lang="ts" setup>
import { DraggableModal } from '@/components/core/draggable-modal';
import { useTable } from '@/components/core/dynamic-table';
import { ref } from '@vue/reactivity';
import { createPreviewColumns } from '../filelist/data';
import { GetFileList, RemoveFile } from '@/api/pages/common';
import { message } from 'ant-design-vue';

const downcolumns = createPreviewColumns({ handleRemove, handleDownload });
const downFileListRef = ref<any[]>([]);
const [DynamicTable, dynamicTableInstance] = useTable();
let downfilevisible = ref(false);

async function GetDownLoadList() {
    downfilevisible.value = true;
    let Data = await GetFileList({ parentId: '' });   
    Data.forEach((item) => {
        item.url = process.env.VUE_APP_BASE_API + `api/Interface/DownloadFile?DownloadTicket=${item.name}`;
        //如果圖片不能顯示會自動拼接nginx轉發的url
    });
    console.log
    downFileListRef.value = Data;
}
async function handleRemove(record) {
    let formdata = new FormData();
    formdata.append('FileID', record['name']);
    let data = await RemoveFile(formdata);
    if (data == 'success') GetDownLoadList();
    else message.error('删除失败');
}
function handleDownload(record) {
    window.location.href = record['url'];
}
</script>

<template>
    <div class="ml-6">
        <a-button type="primary" @click="GetDownLoadList" style="margin-left: 5px">文件列表</a-button>
        <DraggableModal
            v-model:visible="downfilevisible"
            title="文件下载"
            :keyboard="false"
            @cancel="() => (downfilevisible = false)"
            :width="1000"
        >
            <DynamicTable
                :dataSource="downFileListRef"
                :columns="downcolumns"
                
                :showToolBar="false"
            ></DynamicTable>
            <template #footer>
                <a-button @click="() => (downfilevisible = false)" danger>取消</a-button>
            </template>
        </DraggableModal>
    </div>
</template>

<script lang="ts">
export default {
    name: 'DownLoadList'
};
</script>

<style lang="less" scoped>
.upload-modal-toolbar {
    display: flex;
    // align-items: center;
    justify-content: space-between;
    margin-bottom: 8px;
}
.ml-6 {
    position: absolute;
    top: 85px;
    right: 50px;
    z-index: 20;
}

// .ml-6 {

//     position: fixed;
//     z-index:1000
// }
</style>
