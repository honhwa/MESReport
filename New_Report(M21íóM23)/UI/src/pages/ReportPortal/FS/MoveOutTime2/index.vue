<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';

const { mounted, InfiniteScroll, getOperation } = useRemoteSelect();
const UseParamsAdd = AddCommonPageParams();//储存临时资料,如Fab,表单资料和一阶资料
//获取配置文件以便于生成表单内容和表格栏位
function getImport(site: any) {
    let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/MoveOutTime2/${site}/form-schema`);
    const {
        MoveOutTimeColumns,
        MoveOutTimeDetailColumns
    } = require(`@/pages/ReportPortal/FS/MoveOutTime2/${site}/Table`);
    return { schemas, optionsdata, MoveOutTimeColumns, MoveOutTimeDetailColumns };
}
let { schemas, optionsdata, MoveOutTimeColumns, MoveOutTimeDetailColumns } = getImport(UseParamsAdd.Fab);
//定义生成页面的属性
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
//动态表格实例
const [DynamicTable] = useTable();
//动态表单实例
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
//实现懒加载
const MoveOutTime = ref<HTMLElement | null>(null);
InfiniteScroll(MoveOutTime, PageSetting);
//submit的方法
async function confirm(values) {
    // let dynamicColumns: TableColumn[] = []
    let baseParams = { apiName: 'MoveOutTime', FactoryType: UseParamsAdd.Fab, G_FAC: 'A' };

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    if (values['ReportType'] == 'MoveOutTimeDataSetDetail') PageSetting.Columns = MoveOutTimeDetailColumns;
    else PageSetting.Columns = MoveOutTimeColumns;
    PageSetting.Title = 'Move Out Time';
    let { api01 } = await GetCommon({ ...baseParams, ...values });
    PageSetting.moreData = api01['Data0']['Table'];
    PageSetting.moreData.forEach((item: object) => {
        item['TRANS_DATE1'] = item['TRANS_DATE'].split(' ')[0];
        item['Time'] = item['TRANS_DATE'].split(' ')[1];
    });
    if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
    else PageSetting.List = PageSetting.moreData;

    UseParamsAdd.AddFormParams({ ...baseParams, ...values });
    UseParamsAdd.AddAPIResultMessage(api01['Message']);
}
//由api获取select下拉框的内容
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
    //获取站点
    optionsdata.OperationLabel = await getOperation('Operation_MoveOutTime',UseParamsAdd.Fab);
    let { Data, EQUIP_NBR } = await getEquipment();
    optionsdata.EquipmentBig = Data;
    optionsdata.Equipment1 = optionsdata.Equipment = EQUIP_NBR;
});
//获取机台别
async function getEquipment() {
    let Params = { apiName: 'Equipment', FactoryType: UseParamsAdd.Fab, G_FAC: UseParamsAdd.G_FAC_COM, dept: 2 };
    let { api01 } = await GetCommon(Params);
    let Data = api01.Data.Table.map((item) => {
        return { label: item['CODE'], value: item['CODE'] };
    });
    let EQUIP_NBR = api01.Data1.Table.map((item) => {
        return { label: item['EQUIP_NBR'], value: item['EQUIP_NBR'] };
    });
    Data.unshift({ label: 'ALL', value: 'ALL' });
    return { Data, EQUIP_NBR };
}
</script>

<template>
    <div class="DayliMove">
        <a-card>

            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                    <span>注意: 因資料量過大,為保證系統正常呈現, 選取日期跨度時,請勿超過一個星期</span>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="MoveOutTime">
                        <DynamicTable
                        showFavorite
                        sticky
                        :style="{ display: 'inline-block' }"

                        :PageParams="PageParams"
                            row-key="MoveOutTime"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"
                            :data-source="PageSetting.List"
                            :pagination="false"
                            export-file-name="导出"
                        >
                            <!-- <template #toolbar>
                    <a-button type="primary" @click="openMenuModal({})"> 新增 </a-button>
                </template> -->
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>
