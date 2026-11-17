<template>
    <a-modal v-model:visible="props.visible" :title="props.title" @ok="handleOk" @cancel="handleCancel"
        :footer="props.footer">
        <div class="clearfix">
            <Upload :file-list="fileList" :before-upload="beforeUpload" @remove="handleRemove" :max-count="maxCount"
                :multiple="multiple" :accept="accept" :progress="progress" @change="FileListChange">
                <a-button>
                    <upload-outlined></upload-outlined>
                    選擇文件
                </a-button>
            </Upload>
            <a-button type="primary" :disabled="fileList.length === 0" :loading="uploading" style="margin-top: 16px"
                @click="handleUpload">
                點擊上傳
            </a-button>
        </div>
    </a-modal>
</template>
<script lang="tsx" setup>
import { UploadOutlined } from '@ant-design/icons-vue';
import { message, Upload } from 'ant-design-vue';
import { ref, defineProps, defineEmits } from 'vue';
import type { UploadChangeParam, UploadProps } from 'ant-design-vue';
const props = defineProps({
    visible: {
        type: Boolean,
        default: false
    },
    title: {
        type: String
    },
    // 接受的文件類型
    accept: {
        type: String,
        default: 'xlsx,xls,csv'
    },
    // 接受的文件最大的size
    maxFileSize: {
        type: Number,
        default: 50
    },
    // 接受的最多的文件個數為
    maxCount: {
        type: Number,
        default: 1
    },
    multiple: {
        type: Boolean,
        default: false
    },
    // model插槽
    footer: {
        type: Object
    }
});
const progress: UploadProps['progress'] = {
    strokeColor: {
        '0%': '#108ee9',
        '100%': '#87d068',
    },
    strokeWidth: 3,
    format: (percent: number) => {
        return `${parseFloat(percent.toFixed(2))}%`
    },
    class: 'test',
};
const emits = defineEmits(['closemodal', 'uploadfile']);
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
    // 清空上傳的文件
    fileList.value = [];
    emits('closemodal', false);
};
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
const beforeUpload = (file: any) => {
    let fileType = file.name.replace(/.+\./, '');
    let filesize = file.size;
    let allowUpload = true; //经过校验之后是否允许上传
    let errorMessage = ''; //校验文件之后，文件不符合要求的提示信息
    let accepttype = props.accept?.replace('.', '')?.split(',');
    let flag = accepttype.includes(fileType);
    if (filesize > props.maxFileSize * 1024 * 1024) {
        allowUpload = false;
        errorMessage = `文件大小超過了${props.maxFileSize}M限制!`;
    }
    if (!flag) {
        allowUpload = false;
        errorMessage = `请选择${accepttype.join('/')}类型文件`;
    }
    // 文件數量限制
    if (fileList.value.length > props.maxCount) {
        allowUpload = false;
        errorMessage = `只能上傳${props.maxCount}個文件`;
    }
    if (allowUpload) {
        if (fileList.value.length > props.maxCount) {
            fileList.value = [...fileList.value, file].splice(props.maxCount);
        } else {
            fileList.value = [...fileList.value, file];
        }
    } else {
        fileList.value = [];
        message.error(errorMessage);
    }
    return allowUpload;
};
// 文件上傳
const handleUpload = () => {
    const formData = new FormData();
    fileList.value.forEach((file, index) => {
        //多文件上传，formData的append方法，对于同一个key会将新添加的key/value添加到后面，不会覆盖已经存在的key
        /* if (index == 0) {
            formData.append('file', file);
        } */
        formData.append('file', file);
    });
    uploading.value = true;
    emits('uploadfile', fileList, formData);
    uploading.value = false;
};
// 上傳的文件发生改变时
function FileListChange(file) {
    // 多文件上傳在beforeUpload處理了
    /* if (fileList.value.length > props.maxCount) {
        fileList.value = [...fileList.value, file].splice(props.maxCount);
    } else {
        fileList.value = [...fileList.value, file];
    } */
    // 限制只上传一个文件，再次上传时则替换(覆盖)以前的文件
    if (props.maxCount == 1 && fileList.value.length > 1) {
        fileList.value.splice(0, 1);
    }
}

window.onpopstate = function () {
    emits('closemodal', false);
}
</script>
