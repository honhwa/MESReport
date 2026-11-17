<template>
    <DynamicTable
        header-title="角色成員管理"
        show-index
        title-tooltip="请不要随意删除成員，避免影响成員的使用。"
        :data-request="loadTableData"
        :columns="columns"
        :search="true"
        :scroll="{ y: 500 }"
    >
        <template #toolbar>
            <a-button type="primary" @click="openUserModal({})"> <PlusOutlined /> 新增 </a-button>
        </template>
    </DynamicTable>
</template>

<script lang="tsx">
export default {
    name: 'UserManage'
};
</script>
<script setup lang="tsx">
import {
    // SyncOutlined,
    PlusOutlined,
    // EditOutlined,
    // DeleteOutlined,
    ExclamationCircleOutlined
    // SwapOutlined,
} from '@ant-design/icons-vue';
import { Modal, Alert } from 'ant-design-vue';
import { userSchemas } from './formSchemas';
import { baseColumns, TableListItem, TableColumnItem, RoleList } from './columns';
import { useTable } from '@/components/core/dynamic-table';
import { getUserInfo, createUser, updateUser, deleteUser } from '@/api/system/user';
import { useFormModal } from '@/hooks/useModal/index';
import { GetRoleList } from '@/api/system/menu/role';
import { onMounted } from 'vue';

const [DynamicTable, dynamicTableInstance] = useTable();
const [showModal] = useFormModal();

const loadTableData = async (values) => {
    const { api01 } = await getUserInfo({
        apiName: 'GetRole',
        EMP_ID: values['EMP_ID'] ?? '',
        EMP_NAME: values['EMP_NAME'] ?? '',
        BACKUP: values['BACKUP'] ?? '',
        EMP_PERMS: values['EMP_PERMS'] ?? ''
    });
    api01.Table.forEach((item) => (item['SHOW_TOKEN'] = false));
    return api01.Table;
};
/**
 * @description 打开用户弹窗
 */
const openUserModal = async (record: Partial<TableListItem> = {}) => {
    const [formRef] = await showModal<any>({
        modalProps: {
            title: `${record.EMP_ID ? '编辑' : '新增'}用户`,
            width: 700,
            onFinish: async (values) => {
                // values.id = record.EMP_ID;
                values.apiName = record.EMP_ID ? 'UpdataRole' : 'CreateRole';
                await (record.EMP_ID ? updateUser : createUser)(values);
                dynamicTableInstance?.reload();
            }
        },
        formProps: {
            labelWidth: 100,
            schemas: userSchemas
        }
    });
    formRef?.setFieldsValue(record);
    // if (record?.id) {
    //   const { roles } = await getUserInfo({ userId: record.id });
    //   formRef?.setFieldsValue({ roles });
    // }
};

/**
 * @description 表格删除行
 */
const delRowConfirm = async (EMP_ID: string, EMP_NAME: string) => {
    // if (Array.isArray(userId)) {
    Modal.confirm({
        title: '最后确认！！是否删除此用户?',
        icon: <ExclamationCircleOutlined />,
        centered: true,
        onOk: async () => {
            await deleteUser({ EMP_ID: EMP_ID, EMP_NAME: EMP_NAME, apiName: 'DeleteRole' });
            dynamicTableInstance.reload();
        }
    });
    // }
    // else {
    //   await deleteUser({ EMP_ID: [userId] }).finally(dynamicTableInstance?.reload);
    // }
};

const hiddenToken = async (record: Partial<TableListItem> = {}) => {
    record.SHOW_TOKEN = !record.SHOW_TOKEN;
};

const columns: TableColumnItem[] = [
    ...baseColumns,
    {
        title: '操作',
        width: 150,
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
                label: record.SHOW_TOKEN?'隐藏':'显示',
                comtype: 'a-button',
                onClick: () => hiddenToken(record)
            },
            {
                label: '删除',
                comtype: 'a-button',
                popConfirm: {
                    title: '你确定要删除吗？',
                    onConfirm: () => delRowConfirm(record.EMP_ID, record.EMP_NAME)
                }
            }
        ]
    }
];

onMounted(async () => {
    let { api01 } = await GetRoleList({ apiName: 'GetRolePermissions' });
    if (Array.isArray(api01['Table'])) {
        RoleList.value = api01['Table'].map((item) => {
            return { label: item['ROLE'], value: item['ROLE'] };
        });
    }
});
</script>

<style></style>
