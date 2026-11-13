<template>
    <div>
        <DynamicTable
            sticky
            :title-tooltip="['上下移動菜單后請按保存按鈕才能生效', '添加菜單后請為相關角色分配該菜單否則無法呈現']"
            header-title="菜单管理"
            :data-request="loadTableData"
            :columns="columns"
            :row-selection="{ type: 'radio', onChange: onSelectChange }"
            :scroll="{ x: 2500, y: 500 }"
        >
            <template #toolbar>
                <a-button type="ghost" @click="MoveUp(SelectRow.SelectedRows[SelectRow.RowIndex], SelectRow.RowIndex)">
                    <ArrowUpOutlined class="Move"
                /></a-button>
                <a-button
                    type="ghost"
                    @click="MoveDown(SelectRow.SelectedRows[SelectRow.RowIndex], SelectRow.RowIndex)"
                >
                    <ArrowDownOutlined class="Move"
                /></a-button>
                <a-button type="dashed" @click="SaveMenuChange">{{ $t('common.saveText') }}</a-button>
                <a-button type="primary" @click="openMenuModal({}, true)">
                    {{ $t('component.table.addNewMenu') }}
                </a-button>
            </template>
        </DynamicTable>
    </div>
</template>

<script lang="tsx" setup>
import { onMounted, reactive, ref } from 'vue';
import { cloneDeep, sortBy } from 'lodash-es';
import { baseColumns, TableListItem, TableColumnItem, RoleList } from './columns';
import { menuSchemas, explainSchemas } from './formSchemas';
import { message, TreeSelectProps } from 'ant-design-vue';
// import { TableSummary, Switch } from 'ant-design-vue';
import { permmenu, updateMenu, createMenu, deleteMenu } from '@/api/system/menu';
import { useTable } from '@/components/core/dynamic-table';
import { useFormModal } from '@/hooks/useModal/useFormModal';
import { formatMenu2Tree, TreeDataItem } from '@/utils/file/data2Tree';
// import { IconFont } from '@/components/basic/icon-font';
import { FormStatus } from '@/store/module/form';
import { Storage } from '@/utils/Storage';
import { ArrowDownOutlined, ArrowUpOutlined } from '@ant-design/icons-vue';
import { GetRoleList, UpdateRoleFun } from '@/api/system/menu/role';
import { GetCommon } from '@/api/pages/common';
import { ReplaceSpace } from '@/utils/helper/RichTextHelper';

// const fixedTop = ref(false);
// defineOptions({
//   name: 'SysMenu',
// });
const SelectRow = reactive({
    SelectedRows: [] as any[],
    SelectedRowKeys: [] as string[],
    ListData: [] as TreeDataItem[],
    RowIndex: 0
});
let UserParams = Storage.get('UserMessage');
const useFormStatus = FormStatus();
const menuTree = ref<TreeSelectProps['treeData']>([]);
const [DynamicTable, dynamicTableInstance] = useTable({
    search: false,
    pagination: false,
    size: 'small',
    rowKey: 'id',
    bordered: true,
    scroll: { x: 2100, y: 500 }
});
const [showModal] = useFormModal();
const onSelectChange = (selectedRowKeys: any[], selectedRows: any[]) => {
    GetCurrentArray(SelectRow.ListData, selectedRows[0]['id']);
    useFormStatus.ChangeMoveCount();
    //   SelectRow.selectedRowKeys = selectedRows['id'];
};

const loadTableData = async () => {
    //菜单数据通过此方法获取，每次更改可通过dynamicTableInstance.reload()触发更新;也可使用useUserStore存储的Data(不触发)
    let GetMenuParams = { apiName: 'ShowRoute', SYSID: process.env.VUE_APP_SYSID! };
    const { api01: Data } = await permmenu(GetMenuParams);
    Data.filter((item: Object) => {
        if (item['parentId'] == process.env.VUE_APP_SYSID) {
            item['parentId'] = null;
        }
        item['type'] = parseInt(item['type']);
        item['Owner'] = item['Owner'] ?? '';
        item['orderNum'] = parseInt(item['orderNum']);
        return item;
    });
    menuTree.value = formatMenu2Tree(
        cloneDeep(Data).filter((n) => n.type !== 2),
        null
    );
    let SortData = sortBy(Data, ['orderNum']);
    let MenuList = formatMenu2Tree(cloneDeep(SortData), null);
    SelectRow.ListData = MenuList;

    return { list: MenuList };
};

const openMenuModal = async (record: Partial<TableListItem>, isAdd: boolean = false) => {
    useFormStatus.changeFormStatus(isAdd);
    const [formRef] = await showModal({
        modalProps: {
            title: `${isAdd ? '新增' : '编辑'}菜单`,
            width: 700,
            onFinish: async (values) => {
                // values.isShow = record.isShow=='True'?true:false
                values.routeName = values.name;
                values.menuId = record.id;
                values.perms = values.perms ?? 0;
                values.apiName = isAdd ? 'CreateMenuList' : 'UpdateMenuList';
                values.SYSID = process.env.VUE_APP_SYSID;
                values.PERSON = UserParams['MemName'];
                await (isAdd ? createMenu : updateMenu)(values);
                dynamicTableInstance.reload();
                // AddRole(values) 希望在設定頁面時默認設定權限,暫時不着急寫
            },
            wrapClassName: 'tesdsds'
        },
        formProps: {
            labelWidth: 100,
            schemas: menuSchemas
        }
    });

    formRef?.updateSchema([
        {
            field: 'parentId',
            componentProps: {
                treeDefaultExpandedKeys: [-1].concat(record?.keyPath || []),
                treeData: ref([{ id: 'FSUI', name: process.env.VUE_APP_SYSID, children: menuTree.value }])
            }
        }
    ]);
    formRef?.setFieldsValue({
        ...record,
        icon: record.icon ?? '',
        // perms: record.perms,
        isShow: record.isShow == 'True',
        keepalive: record.keepalive == 'True',
        parentId: record.parentId ?? process.env.VUE_APP_SYSID
    });
};
const openExpainModal = async (record: Partial<TableListItem>) => {
    const [formRef] = await showModal({
        modalProps: {
            title: `编辑说明文件`,
            width: 1200,
            onFinish: async (values) => {
                let ExplainData = ReplaceSpace(values.tinymce ?? record.Explain);
                let params = {
                    type: 2,
                    name: record.name,
                    REMARK: record.REMARK,
                    parentId: record.parentId ?? process.env.VUE_APP_SYSID,
                    id: record.id,
                    orderNum: record.orderNum,
                    Owner: record.Owner ?? '',
                    perms: record.perms,
                    routeName: record.title,
                    menuId: record.id,
                    apiName: 'UpdateMenuList',
                    SYSID: process.env.VUE_APP_SYSID,
                    PERSON: UserParams['MemName'],
                    Explain: ExplainData
                };
                await UpdateRoleFun(params);
                dynamicTableInstance.reload();
            }
        },
        formProps: {
            labelWidth: 100,
            schemas: explainSchemas
        }
    });
    formRef?.updateSchema([
        {
            field: 'parentId',
            componentProps: {
                treeDefaultExpandedKeys: [-1].concat(record?.keyPath || []),
                treeData: ref([{ id: 'FSUI', name: process.env.VUE_APP_SYSID, children: menuTree.value }])
            }
        }
    ]);
    formRef?.setFieldsValue({
        title: record.id,
        tinymce: record.Explain
    });
};
const delRowConfirm = async (record: TableListItem) => {
    let paramsdata = {
        apiName: 'DeleteMenuList',
        id: record.id,
        orderNum: record.orderNum,
        SYSID: process.env.VUE_APP_SYSID,
        perms: record.perms
    };
    await deleteMenu(paramsdata);
    dynamicTableInstance.reload();
};

const columns: TableColumnItem[] = [
    ...baseColumns,
    {
        title: '操作',
        // width: 160,
        dataIndex: '$action',
        hideInSearch: true,
        align: 'center',
        fixed: 'right',
        actions: ({ record, index }) => [
            {
                label: '编辑',
                comtype: 'a-button',
                onClick: () => openMenuModal(record, false)
            },
            {
                label: '编辑说明',
                comtype: 'a-button',
                onClick: () => openExpainModal(record)
            },
            {
                label: '新增',
                comtype: 'a-button',
                onClick: () => openMenuModal(record, true)
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

const MoveUp = async (record, index) => {
    let Data = cloneDeep(SelectRow.SelectedRows);
    // GetCurrentArray(SelectRow.ListData, record['id']);
    if (index > 0) {
        // let Temp = Data[index - 1]['orderNum'];
        SelectRow.SelectedRows[index - 1]['orderNum'] = Data[index]['orderNum'];
        SelectRow.SelectedRows[index]['orderNum'] = Data[index - 1]['orderNum'];
        let UpData = SelectRow.SelectedRows[index - 1];
        SelectRow.SelectedRows.splice(index - 1, 1);
        SelectRow.SelectedRows.splice(index, 0, UpData);
        SelectRow.RowIndex -= 1;
    } else {
        message.warning('已经是第一条，不可上移');
    }
};
const MoveDown = (record, index) => {
    let Data = cloneDeep(SelectRow.SelectedRows);
    // GetCurrentArray(SelectRow.ListData, record['id']);
    if (index + 1 === SelectRow.SelectedRows.length) {
        message.warning('已经是最后一条，不可下移');
    } else {
        // let Temp = SelectRow.SelectedRows[index + 1]['orderNum'];
        SelectRow.SelectedRows[index + 1]['orderNum'] = Data[index]['orderNum'];
        SelectRow.SelectedRows[index]['orderNum'] = Data[index + 1]['orderNum'];
        let downDate = SelectRow.SelectedRows[index + 1];
        SelectRow.SelectedRows.splice(index + 1, 1);
        SelectRow.SelectedRows.splice(index, 0, downDate);
        SelectRow.RowIndex += 1;
    }
};
function SaveMenuChange() {
    let params: any = {
        apiName: 'UpdateMenuList',
        type: [] as any,
        name: [] as any,
        REMARK: [] as any,
        parentId: [] as any,
        id: [] as any,
        orderNum: [] as any,
        Owner: [] as any,
        perms: [] as any,
        routeName: [] as any,
        menuId: [] as any,
        PERSON: [] as any
    };
    SelectRow.SelectedRows.forEach((OriData) => {
        params['type'].push('2');
        params['name'].push(OriData['name']);
        params['REMARK'].push(OriData['REMARK']);
        params['id'].push(OriData['id']);
        params['parentId'].push(OriData['parentId'] ?? process.env.VUE_APP_SYSID);
        params['orderNum'].push(OriData['orderNum']);
        params['Owner'].push(OriData['Owner']);
        params['perms'].push(OriData['perms']);
        params['routeName'].push(OriData['name']);
        params['menuId'].push(OriData['id']);
        params['PERSON'].push(UserParams['MemName']);
    });
    //@ts-ignore
    updateMenu(params);
}
function GetCurrentArray(Data: any[], params: string) {
    Data.forEach((item, index) => {
        if (item.id != params && item.children) {
            GetCurrentArray(item.children, params);
        } else {
            if (item.id == params) {
                SelectRow.SelectedRows = Data;
                SelectRow.RowIndex = index;
            }
        }
    });
}
onMounted(async () => {
    let { api01 } = await GetRoleList({ apiName: 'GetRolePermissions' });
    if (Array.isArray(api01['Table'])) {
        RoleList.value = api01['Table'].map((item) => {
            return { label: item['ROLE'], value: item['ROLE'] };
        });
    }
});
async function AddRole(values) {
    let params: any = {
        apiName: 'UpdateRoleFunction',
        ROLE_ID: [] as any, //values['RoleType']??""
        ROLE_NAME: [] as any,
        NAME: [] as any,
        ROUTER_ID: [] as any,
        ROUTER: [] as any,
        BACKUP: [] as any,
        PERSON: [] as any //SelectRow.SelectData['ROUTER_ID']
    };
    values['role'].forEach((OriData) => {
        params['NAME'].push(OriData['name']);
        params['ROUTER_ID'].push(OriData['id']);
        params['ROUTER'].push(OriData['router']);
        params['BACKUP'].push('Y');
        params['PERSON'].push(UserParams['MemName']);
    });
    console.log(params, values);

    // await UpdateRoleFun(params);
}
</script>

<style lang="less" scoped>
.Move {
    font-size: 18px;
}
</style>
