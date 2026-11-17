<template>
    <a-modal v-model:visible="props.visible" :title="props.title" @ok="handleOk" @cancel="handleCancel">
        <div class="clearfix">
            <Upload :file-list="fileList" :before-upload="beforeUpload" @remove="handleRemove" :max-count="1"
                @change="FileListChange">
                <a-button>
                    <upload-outlined></upload-outlined>
                    選擇文件
                </a-button>
            </Upload>
            <a-button type="primary" :disabled="fileList.length === 0" :loading="uploading" style="margin-top: 16px"
                @click="handleUpload">
                點擊上傳
            </a-button><div :style="{ color: 'red', 'font-size': '16px' }">注意：點擊上傳后會即時覆蓋原資料，請慎重點擊</div>
        </div>
    </a-modal>
</template>
<script lang='tsx' setup>
import { UploadOutlined } from '@ant-design/icons-vue';
import { message, Upload } from 'ant-design-vue';
import { ref, defineProps, defineEmits } from 'vue';
const props = defineProps({
    visible: {
        type: Boolean,
        default: false
    },
    title: {
        type: String,
    },
})
const emits = defineEmits(["closemodal", "uploadfile"])
// 關閉ok對話框
const handleOk = () => {
    // 清空上傳的文件
    fileList.value = [];
    // 發送事件給父組件
    emits('closemodal', false);
    // props.visible = false;

};
// 關閉cancel對話框
const handleCancel = () => {
    emits('closemodal', false);
}
// 已经上传的文件列表（受控）
const fileList = ref<any>([]);
const uploading = ref(false);
// 移除上傳文件
const handleRemove = (file: any) => {
    const index = fileList.value.indexOf(file);
    const newFileList = fileList.value.slice();
    newFileList.splice(index, 1);
    fileList.value = newFileList;
};
// 上傳之前的限制
const beforeUpload = (file:any) => {
    /* fileList.value = [...fileList.value, file];
    return false; */
    let fileType = file.name.replace(/.+\./, "");
    let flag = ["xlsx","xls","csv"].includes(fileType);
    if(!flag){
        message.error('请选择xlsx/xls/csv类型文件')
        fileList.value= [];
        return true;
    }else{
        fileList.value = [...fileList.value, file];
        return false;
    }
};
// 文件上傳
const handleUpload = () => {
    const formData = new FormData();
    fileList.value.forEach((file, index) => {
        if (index == 0) {
            formData.append('file', file);
            formData.append('測試', "測試");
        }
    });
    uploading.value = true;
    emits("uploadfile", fileList, formData)
    uploading.value = false;
};
// 上傳的文件发生改变时
function FileListChange() {
    // 限制只上传一个文件，再次上传时则替换(覆盖)以前的文件
    if (fileList.value.length > 1) {
        fileList.value.splice(0, 1)
    }

}



</script>