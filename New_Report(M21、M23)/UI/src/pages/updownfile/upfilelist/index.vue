<script lang="ts" setup>
import { DraggableModal } from '@/components/core/draggable-modal';
import { useTable } from '@/components/core/dynamic-table';
import { Upload, Alert, message } from 'ant-design-vue';
import { useUploadType } from './UseUpload';
import { reactive, unref, ref } from '@vue/reactivity';
import { checkImgType, getBase64WithFile } from './helper';
import { FileItem, UploadResultStatus } from './filelist/typing';
import { createTableColumns } from './filelist/data';
import { computed } from '@vue/runtime-core';
import { toRefs } from 'vue';
import { buildUUID } from '@/utils/uuid';
import { UploadFile } from '@/api/pages/common';
import { FileType } from 'ant-design-vue/es/upload/interface';
import { basicProps } from './type';

const columns = createTableColumns(handleRemove);
const isUploadingRef = ref(false);
const fileListRef = ref<FileItem[]>([]);
const [DynamicTable, dynamicTableInstance] = useTable();
const props = defineProps(basicProps);
let { accept, helpText, maxNumber, maxSize } = toRefs(props);
let visible = ref(false);
const { getStringAccept, getHelpText } = useUploadType({
    acceptRef: accept,
    helpTextRef: helpText,
    maxNumberRef: maxNumber,
    maxSizeRef: maxSize
});

function beforeUpload(file: FileType) {
    const { size, name } = file;
    // 设置最大值，则判断
    if (maxSize.value && file.size / 1024 / 1024 >= maxSize.value) {
        message.error('文件超出限制');
        return false;
    }

    const commonItem = {
        uuid: buildUUID(),
        file,
        size,
        name,
        percent: 0,
        type: name.split('.').pop()
    };
    // 生成图片缩略图
    if (checkImgType(file)) {
        // beforeUpload，如果异步会调用自带上传方法
        // file.thumbUrl = await getBase64(file);
        getBase64WithFile(file).then(({ result: thumbUrl }) => {
            fileListRef.value = [
                ...unref(fileListRef),
                {
                    thumbUrl,
                    ...commonItem
                }
            ];
        });
    } else {
        fileListRef.value = [...unref(fileListRef), commonItem];
    }
    return false;
}
async function handleClose() {
    if (!isUploadingRef.value) {
        fileListRef.value = [];
        visible.value = false;
    } else {
        message.warning('请等待文件上传后执行操作');
        visible.value = true;
    }
}
function handleRemove(record: FileItem) {
    const index = fileListRef.value.findIndex((item) => item.uuid === record.uuid);
    index !== -1 && fileListRef.value.splice(index, 1);
    // emit('delete', record);
}

async function handleStartUpload() {
    if ((fileListRef.value.length ?? 0) > maxNumber.value) {
        return message.warning(`最多只能上传${maxNumber.value}个文件`);
    }
    try {
        isUploadingRef.value = true;
        // 只上传不是成功状态的
        const uploadFileList = fileListRef.value.filter((item) => item.status !== UploadResultStatus.SUCCESS) || [];
        // UploadFileApiByList(uploadFileList);
        const data = await Promise.all(
            uploadFileList.map((item) => {
                let isInnoDrive = process.env.VUE_APP_SITE=='TN'
                return isInnoDrive?uploadApiByInnoDrive(item):uploadApiByItem(item);
            })
        );

        isUploadingRef.value = false;
    } catch (e) {
        isUploadingRef.value = false;
        throw e;
    }
}
const getIsSelectFile = computed(() => {
    return (
        fileListRef.value.length > 0 && !fileListRef.value.every((item) => item.status === UploadResultStatus.SUCCESS)
    );
});

async function uploadApiByItem(item: FileItem) {
    item.status = UploadResultStatus.UPLOADING;
    try {
        const { data } = await UploadFile(
            {
                data: {
                    overwrite: 'true',
                    parentId: '',
                    files: item.file
                },
                file: item.file,
                name: item.name,
                filename: item.name,
                ticket:'Yes',
            },
            function onUploadProgress(progressEvent: ProgressEvent) {
                const complete = ((progressEvent.loaded / progressEvent.total) * 100) | 0;
                item.percent = complete;
            }
        );
        if (data && Array.isArray(data) && data.length > 0) {
            item.status = UploadResultStatus.SUCCESS;
            return {
                success: true,
                error: null
            };
        } else {
            item.status = UploadResultStatus.ERROR;
            return {
                success: false,
                error: '上传失败'
            };
        }
    } catch {
        item.status = UploadResultStatus.ERROR;
    }
}
async function uploadApiByInnoDrive(item: FileItem) {
    item.status = UploadResultStatus.UPLOADING;
    try {
        const { data } = await UploadFile(
            {
                data: {
                    overwrite: 'true',
                    parentId: '37bb91d1-7dd9-4563-a916-b3f60df13d29',
                    files: item.file
                },
                file: item.file,
                name: item.name,
                filename: item.name,
                ticket:'mxdNpNYBD3gypTZNsd+RnVo89I+gl6lDVTcs/a5Z6Uo7MyB84GHagp9eoC8ayIlt',
                url:'http://pinnodrvapattachtn.cminl.oa/api/Interface/UploadFiles'
            },
            function onUploadProgress(progressEvent: ProgressEvent) {
                const complete = ((progressEvent.loaded / progressEvent.total) * 100) | 0;
                item.percent = complete;
            }
        );
        if (data && Array.isArray(data) && data.length > 0) {
            item.status = UploadResultStatus.SUCCESS;
            return {
                success: true,
                error: null
            };
        } else {
            item.status = UploadResultStatus.ERROR;
            return {
                success: false,
                error: '上传失败'
            };
        }
    } catch {
        item.status = UploadResultStatus.ERROR;
    }
}

const getUploadBtnText = computed(() => {
    const someError = fileListRef.value.some((item) => item.status === UploadResultStatus.ERROR);

    return isUploadingRef.value ? '上传中' : someError ? '重新上传' : '开始上传';
});

// async function UploadFileApiByList(List: FileItem[]) {
//     let formData = new window.FormData();
//     let fileList:Flie[]=[];
//     try {
//         List.forEach(item=>fileList.push(item.file))
//         formData.append('parentId', 'overwrite');
//         formData.append('files', fileList[0]);
//         formData.append('overwrite', 'true');
//         const data = await UploadFile(formData);
//         if (Array.isArray(data) && data.length > 0) {
//             return {
//                 success: true,
//                 error: null
//             };
//         }
//         return {
//                 success: false,
//                 error: null
//             };
//     } catch (e) {
//         console.log(e);
//         return {
//             success: false,
//             error: e
//         };
//     }
// }
</script>

<template>
    <div class="ml-6">
        <!-- <Alert message="文件上传" type="info" show-icon>
            <template #description> 未接入API</template>
        </Alert> -->
        <!-- <Alert :message="getHelpText" type="info" banner class="upload-modal-toolbar__text" /> -->
        <a-button type="success" @click="() => (visible = true)">上传文件</a-button>
        <DraggableModal
            v-model:visible="visible"
            title="文件上传"
            :keyboard="false"
            @cancel="handleClose"
            :width="1000"
        >
            <div class="upload-modal-toolbar">
                <Alert :message="getHelpText" type="info" banner class="upload-modal-toolbar__text" />

                <Upload
                    :accept="getStringAccept"
                    :multiple="true"
                    :before-upload="beforeUpload"
                    :show-upload-list="false"
                    class="upload-modal-toolbar__btn"
                >
                    <a-button type="primary"> 选择 </a-button>
                </Upload>
            </div>

            <DynamicTable :dataSource="fileListRef" :columns="columns" :showToolBar="false"></DynamicTable>
            <template #footer>
                <a-button @click="handleClose" danger>取消</a-button>
                <a-button
                    @click="handleStartUpload"
                    type="primary"
                    :disabled="!getIsSelectFile"
                    :loading="isUploadingRef"
                >
                    {{ getUploadBtnText }}
                </a-button>
            </template>
        </DraggableModal>
    </div>
</template>
<script lang="ts">
export default {
    name: 'UpFile'
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
    right: 150px;
    z-index: 20;
}

// .ml-6 {

//     position: fixed;
//     z-index:1000
// }
</style>
