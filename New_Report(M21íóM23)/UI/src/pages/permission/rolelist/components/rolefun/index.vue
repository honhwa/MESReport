<template>
    <div>
        <SchemaForm @submit="confirm" :submitButtonOptions="{ text: '更新' }" :showTextPosition="'right'">
            <template #remoteSearch="{ formModel, field }">
                <ApiSelect
                    v-model:value="formModel[field]"
                    show-search
                    option-filter-prop="label"
                    result-field="Table"
                    label-field="ROLE"
                    value-field="ROLE"
                    :api="GetRoleList"
                    @callchange="GetRoleFunction"
                    :params="{ apiName: 'GetRolePermissions' }"
                />
            </template>
        </SchemaForm>
        <DynamicTable
        sticky
            :data-request="LoadRoleFunData"
            :columns="RoleFunColumns"
            :row-selection="{ onChange: onSelectChange, selectedRowKeys: SelectRow.selectedRowKeys }"
            :scroll="{ y: 500 }"
            :pagination="false"
        >
        </DynamicTable>
    </div>
</template>

<script lang="tsx">
export default {
    name: 'RoleFun'
};
</script>
<script setup lang="tsx">
import { userSchemas } from './formSchemas';
import { GetCommon } from '@/api/pages/common';
import { RoleFunColumns } from './columns';
import { useTable } from '@/components/core/dynamic-table';
import { UpdateRoleFun } from '@/api/system/menu/role';
import { Storage } from '@/utils/Storage';
import { permmenu } from '@/api/system/menu';
import { cloneDeep } from 'lodash-es';
import { formatMenu2Tree } from '@/utils/file/data2Tree';
import { useForm, ApiSelect } from '@/components/core/schema-form';
import { onMounted, reactive } from 'vue';
import { GetRoleList } from '@/api/system/menu/role';

let UserParams = Storage.get('UserMessage');
const [DynamicTable] = useTable();
let SelectRow = reactive({
    SelectData: [] as any,
    selectedRowKeys: [] as any
});

async function confirm(values) {
    let params: any = {
        apiName: 'UpdateRoleFunction',
        ROLE_ID: values['RoleType'] ?? '', //values['RoleType']??""
        ROLE_NAME: values['RoleType'] ?? '',
        NAME: [] as any,
        ROUTER_ID: [] as any,
        ROUTER: [] as any,
        BACKUP: [] as any,
        PERSON: [] as any //SelectRow.SelectData['ROUTER_ID']
    };
    SelectRow.SelectData.forEach((OriData) => {
        params['NAME'].push(OriData['name']);
        params['ROUTER_ID'].push(OriData['id']);
        params['ROUTER'].push(OriData['router']);
        params['BACKUP'].push('Y');
        params['PERSON'].push(UserParams['MemName']);
    });
    await UpdateRoleFun(params);
}
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: userSchemas,
    actionColOptions: {
        span: 24
    },
    layout: 'horizontal'
});
function onSelectChange(selectedRowKeys, selectedRows) {
    SelectRow.selectedRowKeys = selectedRowKeys;
    SelectRow.SelectData = selectedRows;
}
const LoadRoleFunData = async () => {
    let GetMenuParams = {
        apiName: 'ShowRoute',
        SYSID: process.env.VUE_APP_SYSID!
        // EMP_ID: UserParams['MemID'],
        // EMP_NAME: UserParams['MemName']
    };
    const { api01: Data } = await permmenu(GetMenuParams);
    Data.filter((item: Object) => {
        if (item['parentId'] == process.env.VUE_APP_SYSID) {
            item['parentId'] = null;
        }
        return item;
    });
    let MenuList = formatMenu2Tree(cloneDeep(Data), null);
    return { list: MenuList };
};
// async function optionsListApi() {
//     const { api01 } = await GetCommon({
//         apiName: 'GetRolePermissions',
//         // SysName: `${process.env.VUE_APP_SITE}Report`
//     });
//     return api01;
// }
async function GetRoleFunction(value: any) {
    SelectRow.selectedRowKeys = [];
    let params = {
        apiName: 'GetRoleFunction',
        ROLE_ID: value
    };
    let { api01 } = await GetCommon(params);
    if (api01['Table'].length > 0) {
        SelectRow.selectedRowKeys = api01['Table'].map((item) => {
            return (item = item['ROUTER_ID']);
        });
    }
    // SelectRow.selectedRowKeys = ['DashBoard']
}
// function handleChange(value){
//   a  GetRoleFunction(value);
// }
onMounted(async () => {
    await GetRoleFunction('Admin');
});
</script>

<style></style>
