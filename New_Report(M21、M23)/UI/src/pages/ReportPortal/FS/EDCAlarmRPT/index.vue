<template>
    <div class="LCM2_AopsEntityStatusDateset">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div class="ScrollTable" ref="LCM2_AopsEntityStatusDateset">
                    <DynamicTable row-key="LCM2_AopsEntityStatusDateset" header-title="LCMB AOPS Entity Status Report "
                        showFavorite sticky :style="{ display: 'inline-block' }" :PageParams="PageParams"
                        :columns="PageSetting.TopColumns" :key="PageSetting.TopColumns" bordered size="small"
                        export-type="Exceljs"
                        :data-source="PageSetting.TopList" :pagination="false" :row-class-name="getRowClassNmae"
                        :exceljsparams="PageSetting.exceljsparams"
                        :scroll="{ x: PageSetting.x, y: 650 }" export-file-name="导出">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { cloneDeep } from 'lodash';
// import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';

import { getfinalsum, getRowClassNmae, megercell } from '@/pages/ReportPortal/utils/common';
// import { megercell, sortArray } from '@/pages/ReportPortal/utils/common';
import dayjs from 'dayjs';
import { message} from 'ant-design-vue';
const UseParamsAdd = AddCommonPageParams();
let FactoryType: any = UseParamsAdd.Fab;
const LCM2_AopsEntityStatusDateset = ref<HTMLElement | null>(null);

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/EDCAlarmRPT/LCM2/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/EDCAlarmRPT/LCM2/table`);
    return { schemas, optionsdata, baseColumns };
}
const { schemas, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    BottomColumns: [] as any,
    BottomList: [] as any,
    Message: {},
    x: 300,
    exceljsparams: {
        Header: {
            title: [] as any[],
            key: ['UNIT_ID', 'ITEM_TYPE', 'ITEMGROUP', 'SPEC', 'VALUE_SETTING', 'VALUE_ACTUAL'] as any[],
        },
        MutiHeader: [
            ["項目", "項目", "項目", "RECIPE ID", "", ""],
            ["項目", "項目", "項目", "公差", "幾臺設定", "實際"]
        ] as any[]
    }
});
const [DynamicTable] = useTable();

const { mounted, InfiniteScroll, mountedowner, mountedLCDGrade } = useRemoteSelect();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});
let TopTableParams = reactive({
    name: 'Date',
    apiName: 'LCM2_AopsEntityStatusDateset', //"StartBankStatusDataSet",
    FactoryType: FactoryType,
    G_FAC: 'A',
    FromDate: '',
    Equipment: []
});


async function confirm() {
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];
    let tempkeys = Object.keys(TopTableParams);
    let tempdynamicFormRef = cloneDeep(dynamicFormRef.formModel);
    console.log('顯示提示信息', Object.values(tempdynamicFormRef));
    if (
        Object.values(tempdynamicFormRef)[1] == 'ALL' ||
        Object.values(tempdynamicFormRef)[2] == 'ALL' ||
        Object.values(tempdynamicFormRef)[4] == 'ALL' ||
        Object.values(tempdynamicFormRef)[1] == '' ||
        Object.values(tempdynamicFormRef)[2] == '' ||
        Object.values(tempdynamicFormRef)[4] == ''
    ) {
        message.success('請選設備及線別');
        //return;
    } else if (Object.values(tempdynamicFormRef)[4].length > 1) {
        message.success('只能選擇一個線別,請重新選擇');

    } else {
        for (let i in tempdynamicFormRef) {
            if (tempkeys.includes(i)) {
                if (['FromTime', 'ToTime'].includes(i)) {
                    TopTableParams[i] = ` ${dayjs(tempdynamicFormRef[i]['$d']).format('HH:mm')}`;
                } else {
                    TopTableParams[i] = tempdynamicFormRef[i];
                }
            }
        }

        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon(TopTableParams);
        let tempaops = '';
        Data.forEach((item) => {
            tempaops = item['LINE_ID'];
        });

        let tempColumns = cloneDeep(baseColumns);
        PageSetting.TopList = Data;
        PageSetting.TopColumns = baseColumns(tempaops);
        PageSetting.exceljsparams.Header.title = [];
        PageSetting.exceljsparams.Header.title = ['幾臺別','幾臺別','幾臺別','幾臺別',tempaops,tempaops];

        PageSetting.TopColumns.forEach((item, index) => {
            if (index == 0) {
                item.children.forEach((citem) => {
                    if (['UNIT_ID', 'ITEM_TYPE'].includes(citem.dataIndex)) {
                        megercell(item.children, ['UNIT_ID', 'ITEM_TYPE'], PageSetting.TopList);
                    }
                });
            }
        });
        
        console.log('PageSetting:TopColumns', tempColumns);
        console.log('PageSetting.Data', Data);
    }
}
InfiniteScroll(LCM2_AopsEntityStatusDateset, PageSetting as any);
onMounted(async () => {
    await mounted(optionsdata, FactoryType);
    await mountedowner(optionsdata, FactoryType);
    await mountedLCDGrade(optionsdata, FactoryType);
});
</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.LCM2_AopsEntityStatusDateset :deep(.sub-total) {
    background-color: #ffb468 !important;
}

.LCM2_AopsEntityStatusDateset :deep(.total) {
    background-color: #ff80c0 !important;
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
