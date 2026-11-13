<script lang="ts" setup>
  import { DraggableModal } from '@/components/core/draggable-modal';
  import { useTable } from '@/components/core/dynamic-table';
  import { Upload, Alert, message } from 'ant-design-vue';
  import { useUploadType } from '../upfilelist/UseUpload';
  import { checkImgType, getBase64WithFile } from '../upfilelist/helper';
  import { FileItem, UploadResultStatus } from '../upfilelist/filelist/typing';
  import { createTableColumns } from '../upfilelist/filelist/data';
  import { computed } from '@vue/runtime-core';
  import { toRefs, watchEffect, reactive, unref, ref, watch } from 'vue';
  import { buildUUID } from '@/utils/uuid';
  import { UploadExcel } from '@/api/pages/common';
  import { basicProps } from '../upfilelist/type';
  import { useVModel } from '@vueuse/core';
  import { downloadByUrl } from '@/utils/file/download';
  import { FileType } from 'ant-design-vue/es/upload/interface';

  const columns = createTableColumns(handleRemove);
  const isUploadingRef = ref(false);
  const fileListRef = ref<FileItem[]>([]);
  const [DynamicTable, dynamicTableInstance] = useTable();
  const props = defineProps(basicProps);
  let { accept, helpText, maxNumber, maxSize, exampfilename, examplefileurl } = toRefs(props);
  let uploadParams = reactive({});
  watchEffect(() => {
    uploadParams = props.uploadParams;
  });

  const { getStringAccept, getHelpText } = useUploadType({
    acceptRef: accept,
    helpTextRef: helpText,
    maxNumberRef: maxNumber,
    maxSizeRef: maxSize,
  });
  const emit = defineEmits(['update:visible']);
  const modelValue = useVModel(props, 'visible', emit);

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
      type: name.split('.').pop(),
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
            ...commonItem,
          },
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
      modelValue.value = false;
    } else {
      message.warning('请等待文件上传后执行操作');
      modelValue.value = true;
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
      const uploadFileList =
        fileListRef.value.filter((item) => item.status !== UploadResultStatus.SUCCESS) || [];
      // UploadFileApiByList(uploadFileList);
      const data = await Promise.all(
        uploadFileList.map((item) => {
          return uploadApiByItem(item);
        }),
      );
      // console.log(data);

      isUploadingRef.value = false;
    } catch (e) {
      isUploadingRef.value = false;
      throw e;
    }
  }
  const getIsSelectFile = computed(() => {
    return (
      fileListRef.value.length > 0 &&
      !fileListRef.value.every((item) => item.status === UploadResultStatus.SUCCESS)
    );
  });

  async function uploadApiByItem(item: FileItem) {
    item.status = UploadResultStatus.UPLOADING;
    const FileParams = {
      file: item.file,
      ...uploadParams,
    };

    const { data } = await UploadExcel(
      {
        data: FileParams,
        file: item.file,
        name: item.name,
        filename: item.name,

        // ticket:'Yes',
        url: process.env.VUE_APP_BASE_API + 'SQLAgent/ApiWork',
      },
      function onUploadProgress(progressEvent: ProgressEvent) {
        const complete = ((progressEvent.loaded / progressEvent.total) * 100) | 0;
        item.percent = complete;
      },
    );

    if (data && Array.isArray(data) && data.length > 0) {
      item.status = UploadResultStatus.SUCCESS;
      return {
        success: true,
        error: null,
      };
    } else {
      item.status = UploadResultStatus.ERROR;
      return {
        success: false,
        error: '上传失败',
      };
    }
  }

  const getUploadBtnText = computed(() => {
    const someError = fileListRef.value.some((item) => item.status === UploadResultStatus.ERROR);

    return isUploadingRef.value ? '上传中' : someError ? '重新上传' : '开始上传';
  });

  const downLoadExcelExamp = () => {
    downloadByUrl({
      url: examplefileurl.value,
      target: '_self',
    });
  };
</script>

<template>
  <div class="ml-6">
    <DraggableModal
      v-model:visible="modelValue"
      title="Excel上传"
      :keyboard="false"
      @cancel="handleClose"
      :width="1000"
    >
      <div class="upload-modal-toolbar">
        <Alert :message="getHelpText" type="info" banner class="upload-modal-toolbar__text" />
        <a @click="downLoadExcelExamp" class="upload-modal-example" v-if="exampfilename"
          >{{ exampfilename }}模板</a
        >
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

      <DynamicTable
        :dataSource="fileListRef"
        :columns="columns"
        :showToolBar="false"
      ></DynamicTable>
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
    name: 'UploadExcel',
  };
</script>

<style lang="less" scoped>
  .upload-modal-toolbar {
    display: flex;
    // align-items: center;
    justify-content: space-between;
    margin-bottom: 8px;
    .upload-modal-example {
      float: right;
    }
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
