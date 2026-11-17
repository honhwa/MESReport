<script lang="tsx" setup>
import { useTable } from '@/components/core/dynamic-table';
import { baseColumns, TableColumnItem, TableListItem } from './columns';
import { GetCommon } from '@/api/pages/common';
import { explainSchemas } from './formSchma';
import { useFormModal } from '@/hooks/useModal/useFormModal';
import { Storage } from '@/utils/Storage';
import { useModal } from '@/hooks/useModal/';
import { watchEffect } from 'vue';
import { message } from 'ant-design-vue';
import { useRoute } from 'vue-router';
import { ReplaceSpace } from '@/utils/helper/RichTextHelper';

const route = useRoute();
const [fnModal] = useModal();
const [showModal] = useFormModal();
const [DynamicTable, dynamicTableInstance] = useTable();
const UserParams = Storage.get('UserMessage');
const columns: TableColumnItem[] = [
    {
        title: '标题',
        dataIndex: 'NOTICETITLE',
        bodyCell: ({ record }) => (
            <div onClick={() => handleOpenHookModal(record)} class="NoticeTitle">
                {record['NOTICETITLE']}
            </div>
        )
    },
    ...baseColumns,
    {
        title: '操作',
        width: 260,
        dataIndex: '$action',
        hideInSearch: true,
        align: 'center',
        fixed: 'right',
        actions: ({ record, index }) => [
            {
                label: '编辑',
                comtype: 'a-button',
                onClick: () => openExpainModal(record, false)
            },
            {
                label: '新增',
                comtype: 'a-button',
                onClick: () => openExpainModal(record, true)
            },
            {
                label: '删除',
                comtype: 'a-button',
                popConfirm: {
                    title: '你确定要删除吗？',
                    onConfirm: () => delRowConfirm(record)
                }
            }
        ]
    }
];
const openExpainModal = async (record: Partial<TableColumnItem>, isAdd: boolean) => {
    const [formRef] = await showModal({
        modalProps: {
            title: isAdd ? '新增' : '編輯' + '説明文件',
            width: 1200,
            onFinish: async (values) => {
                values.SYSID = process.env.VUE_APP_SYSID;
                values.apiName = isAdd ? 'CreateNoticeData' : 'UpdateNoticeData';
                values.NOTICEPEOPLE = UserParams['MemName'];
                values.NOTICEDATA = ReplaceSpace(values.NOTICEDATA);
                let { api01 } = await GetCommon(values);
                message.warn(api01);
                dynamicTableInstance.reload();
            }
        },
        formProps: {
            labelWidth: 100,
            schemas: explainSchemas
        }
    });
    formRef?.setFieldsValue({
        ...record
    });
};
const delRowConfirm = async (record: TableListItem) => {
    let { api01 } = await GetCommon({ apiName: 'DeleteNoticeData', SYSID: process.env.VUE_APP_SYSID, ...record });
    message.warn(api01);
    dynamicTableInstance.reload();
};
const loadTableData = async () => {
    let GetMenuParams = { apiName: 'GetNotice', SYSID: process.env.VUE_APP_SYSID! };
    const { api01: Data } = await GetCommon(GetMenuParams);
    return { list: Data['Table'] };
};
const handleOpenHookModal = (record) => {
    const ExplainData = <div v-html={record['NOTICEDATA']}></div>;
    fnModal.setProps({
        title: record['NOTICETITLE'],
        width: 1200,
    });
    fnModal.show({
        content: ExplainData,
    });
};

watchEffect(async () => {
    if (route.fullPath == '/announcement') {
        let { list } = await loadTableData();
        if (list.length > 0) handleOpenHookModal(list[0]);
    }
});
</script>

<template>
    <a-card>
        <DynamicTable
            :columns="columns"
            :data-request="loadTableData"
            style="margin-top: 1%"
            :explain="false"
            :OwnerFoldParams="false"
        >
            <template #toolbar>
                <a-button type="primary" @click="openExpainModal({}, true)">
                    {{ $t('component.table.addNewNotice') }}
                </a-button>
            </template>
        </DynamicTable>
    </a-card>
</template>

<style scoped lang="less">
:deep(.NoticeTitle) {
    cursor: pointer;
    color: blue;
}
:deep(.Notice){
    max-height: 90%;
    overflow: auto;
}
</style>
