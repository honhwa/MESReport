<template>
    <DynamicTable
        header-title="角色管理"
        show-index
        title-tooltip="请谨慎删除角色,这会影响该角色下的所有用户!!"
        :data-request="loadTableData"
        :columns="columns"
    >
        <template #toolbar>
            <a-button type="primary" @click="openUserModal({})"> <PlusOutlined /> 新增 </a-button>
        </template>
    </DynamicTable>
</template>

<script lang="tsx">
export default {
    name: 'RoleManage'
};
</script>
<script setup lang="tsx">
import { PlusOutlined, ExclamationCircleOutlined } from '@ant-design/icons-vue';
import { userSchemas } from './formSchemas';
import { RoleManageColumns, TableListItem, TableColumnItem } from './columns';
import { useTable } from '@/components/core/dynamic-table';
import { getUserInfo } from '@/api/system/user';
import { useFormModal } from '@/hooks/useModal/index';
import { message, Modal } from 'ant-design-vue';
import { GetCommon } from '@/api/pages/common';
import { Storage } from '@/utils/Storage';

let UserParams = Storage.get('UserMessage');
const [showModal] = useFormModal();
const [DynamicTable, dynamicTableInstance] = useTable();

const openUserModal = async (record: Partial<TableListItem> = {}) => {
    const [formRef] = await showModal<any>({
        modalProps: {
            title: `${record.ROLE ? '编辑' : '新增'}角色`,
            width: 700,
            onFinish: async (values) => {
                values.apiName = record.ROLE ? 'UpdateRolePermissions' : 'CreateRolePermissions';
                values.PEOSON = UserParams['MemName'];
                let { api01 } = await GetCommon(values);
                if (api01 == 'Success!') {
                    message.success('角色更新成功!');
                    dynamicTableInstance?.reload();
                } else {
                    message.error('角色更新失敗!');
                }
            }
        },
        formProps: {
            labelWidth: 100,
            schemas: userSchemas
        }
    });
    formRef?.setFieldsValue(record);
};
const loadTableData = async () => {
    const { api01 } = await getUserInfo({ apiName: 'GetRolePermissions' });

    return api01.Table;
};
const columns: any[] = [
    ...RoleManageColumns,
    {
        title: '操作',
        // width: 150,
        dataIndex: '$action',
        align: 'center',
        fixed: 'right',
        actions: ({ record }) => [
            {
                label: '编辑',
                comtype: 'a-button',
                onClick: () => openUserModal(record)
            },
            {
                label: '删除',
                comtype: 'a-button',
                popConfirm: {
                    title: '你确定要删除吗？',
                    onConfirm: () => delRowConfirm(record.ROLE, record.ROLELEVEL)
                }
            }
        ]
    }
];
const delRowConfirm = async (ROLE: string, ROLELEVEL: string) => {
    if(['Admin','user'].includes(ROLE)) return message.error('當前角色不可刪除!')
    // if (Array.isArray(userId)) {
    Modal.confirm({
        title: '最后确认！！是否删除此用户?',
        icon: <ExclamationCircleOutlined />,
        centered: true,
        onOk: async () => {
            let { api01 } = await GetCommon({ ROLE, ROLELEVEL, apiName: 'DeleteRolePermissions' });
            if (api01 == 'Success!') {
                message.success('角色刪除成功!');
                dynamicTableInstance?.reload();
            } else {
                message.error('角色刪除失敗!');
            }
        }
    });
};
</script>

<style></style>
