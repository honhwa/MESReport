<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { reactive, watchEffect, watch, computed } from 'vue';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { DownGradeLabel } from '@/pages/ReportPortal/FS/DownGrade/LCM1/Table';
// import { useVirtualList } from '@vueuse/core';

const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const { DownSecondColumns, DownThridColumns: DownThridColumnsLCM1, DownThridColumnsLCM2 } = require(`@/pages/ReportPortal/FS/DownGrade/${site}/Table`);
    return { DownSecondColumns, DownThridColumnsLCM1, DownThridColumnsLCM2 };
}
let { DownSecondColumns, DownThridColumnsLCM1, DownThridColumnsLCM2 } = getImport(UseParamsAdd.Fab);

let useCommonParams = AddCommonPageParams();
let TableParams = useCommonParams.FirstTableParams;
let FactoryType: string = UseParamsAdd.Fab;
let DownThridColumns;
switch (FactoryType) {
    case "LCM1":
        DownThridColumns = DownThridColumnsLCM1;
        break;
    case "LCM2":
        DownThridColumns = DownThridColumnsLCM2;
        break;

    default:
        break;
}
let PageSetting = reactive({
    moreData: [] as any,
    List: [] as any,
    Columns: DownSecondColumns,
    TopTitle: '',
    BottomTitle: '',
    BottomList: [] as any
});
const [DynamicTable] = useTable();

async function GetDetailData(TableParams: any) {
    let formParams = useCommonParams.FormParams;

    let ColumnsData = {
        Operator: [{ title: '工號', dataIndex: '工號', align: 'center' }, ...DownSecondColumns],
        equip_nbr: [{ title: 'equip_nbr', dataIndex: 'Product', align: 'center' }, ...DownSecondColumns],
        Prod_nbr: [{ title: 'Product', dataIndex: 'Product', align: 'center' }, ...DownSecondColumns]
        // Operator1: DownGradeProdColumns
    };
    PageSetting.Columns = ColumnsData[formParams['ShowMode']];
    // let APIParams: any = useCommonParams.APIMessage
     let Grade = DownGradeLabel.indexOf(TableParams['dataIndex']) <= 16 ? 'lcm_grade' : 'downgrade';
    //let Grade = DownGradeLabel.indexOf(TableParams['dataIndex']) <= 18 ? 'downgrade' : 'lcm_grade';
    let { api01 } = await GetCommon({
        ...formParams,
        ReportType: 'DownGradeDetail1',
        Queryby: Grade,
        Operator: TableParams['人員工號'] ?? TableParams['站别'] ?? TableParams['Product'] ?? '',
        lcm_grade: TableParams['title'].split('*')[0],
        Operator12: '',
        Operator123: '',
        item: '',
        dfct_code: '',
        lcm_grade1: '',
        Queryby1: ''
    });

    let pushdata = api01['Data0']['Table'];
    PageSetting.List = pushdata;
    PageSetting.TopTitle = 'LCMA Down Grade Detail';
}
async function GetSecondTable(TableParams: any, SecondTableParams: any) {
    console.log("输出表格2的tableparams", TableParams);
    console.log("输出表格2的SecondTableParams", SecondTableParams);
    let Operator123 = "";
    let formParams = useCommonParams.FormParams;
    let Operator12: string = '';
    let lcm_grade1 = "";
    let Operator3 = SecondTableParams["工號"];
    if (UseParamsAdd.Fab == "LCM1") {
        Operator123 = TableParams['人員工號'] ?? TableParams['站别'] ?? TableParams['Product'] ?? '';
        lcm_grade1 = TableParams['LCM_GRADE'] ?? ''
    }
    else if (UseParamsAdd.Fab == "LCM2") {
        Operator123 = PageSetting.List[PageSetting.List.length - 3]['工號'] ?? '';
        Operator3 = PageSetting.List[PageSetting.List.length - 3]['工號'] ?? '';
        lcm_grade1 = TableParams['title'].split('*')[0];
    }

    if (SecondTableParams['工號'] == '小計' && formParams['ShowMode'] == 'Operator') {
        Operator12 = TableParams['人員工號'] ?? '';
        if (UseParamsAdd.Fab == "LCM2") {
            Operator123 = "小計"
            Operator12 = PageSetting.List[PageSetting.List.length - 3]['工號'] ?? '';
        }
    } else if (SecondTableParams['工號'] == '小計' && formParams['ShowMode'] == 'equip_nbr') {
        Operator12 = TableParams['站别'] ?? '';
        if (UseParamsAdd.Fab == "LCM2") {
            Operator123 = "小計"
            Operator12 = PageSetting.List[PageSetting.List.length - 3]['工號'] ?? '';
        }
    } else if (SecondTableParams['工號'] == '小計' && formParams['ShowMode'] == 'Prod_nbr') {
        Operator12 = TableParams['Product'] ?? '';
        if (UseParamsAdd.Fab == "LCM2") {
            Operator12 = TableParams['Product'] ?? '';
        }
    } else if (SecondTableParams['工號'] != '小計') {
        if (UseParamsAdd.Fab == "LCM2") {
            if (formParams['ShowMode'] == 'Operator') {
                Operator12 = SecondTableParams["工號"];
            }
            else if (formParams['ShowMode'] == 'equip_nbr') {
                Operator12 = TableParams["站别"];
                Operator3 = TableParams["站别"];
            }
        } else if (UseParamsAdd.Fab == "LCM1") {
            if (formParams['ShowMode'] == 'Operator') {
                Operator12 = "";
            }
        }


    }
    if (UseParamsAdd.Fab == "LCM2") {
        console.log("符合LCM2");
        
        if (formParams['ShowMode'] == 'Prod_nbr') {
            console.log("進來了");
            
            if (SecondTableParams["Product"] != '小計') {
            console.log("不走小計");
                Operator12 = TableParams['Product'];
                Operator123 = TableParams['Product'];
                Operator3 = TableParams["Product"];
            }else if(SecondTableParams["Product"] == '小計'){
            console.log("走小計");
            Operator12 = TableParams['Product'];
            Operator3 = TableParams["Product"];
                Operator123 = "小計"
            }

        }
    }
    // Queryby1處理
    let lcm_gradeclick = ['累計檢驗數量', 'A', 'B', 'C', 'C2', 'C3', 'C4', 'C5', 'C6', 'D', 'E', 'L', 'W',
    'R', 'K', 'G', 'Down Grade數量'];
    let downgradeclick = ['Down Grade比率', '送修數量', '送修比率', '檢驗良品率', 'Down Grade',
    'A GRADE 比率', 'A~C5 GRADE 比率']
    let Queryby1 = "";
    if(lcm_gradeclick.includes(TableParams['dataIndex'])){
        Queryby1 = "lcm_grade"
    }
    if(downgradeclick.includes(TableParams['dataIndex'])){
        Queryby1 = "downgrade"
    }

    let Grade = DownGradeLabel.indexOf(TableParams['dataIndex']) <= 16 ? 'lcm_grade' : 'downgrade';
    //let Grade = DownGradeLabel.indexOf(TableParams['dataIndex']) <= 18 ? 'downgrade' : 'lcm_grade';
    let { api01 } = await GetCommon({
        ...formParams,
        ReportType: 'DownGradeDetail2',
        Queryby: Grade,
        Operator: TableParams['人員工號'] ?? TableParams['站别'] ?? TableParams['Product'] ?? '',
        lcm_grade: TableParams['title'].split('*')[0],
        Operator12,
        Operator123,
        item: SecondTableParams['ITEM'] ?? '',
        dfct_code: SecondTableParams['DEFECT_CODE'] ?? '',
        lcm_grade1,
        Operator3,
        // Queryby1: UseParamsAdd.Fab == "LCM2" ? SecondTableParams["ITEM"].replace(/\s*/g, "").toLowerCase() : ""
        // 如果首頁的一階段點擊的
        Queryby1: Queryby1
    });
    let pushdata = api01['Data0']['Table'];
    PageSetting.BottomList = pushdata;
    PageSetting.BottomTitle = 'LCMA Down Grade Detail';

}
// let onMoreDataLoad = computed(() => {
//     if (PageSetting.moreData.length > 0) return PageSetting.moreData;
//     else return [];
// });
// const { list, containerProps, wrapperProps } = useVirtualList(onMoreDataLoad, {
//     itemHeight: 22
// });
// watch(
//     () => list.value,
//     () => {
//         let pushData: any[] = [];
//         PageSetting.List = [];
//         list.value.forEach((item) => {
//             if (item.index < PageSetting.moreData.length) {
//                 pushData.push(item.data);
//             }
//         });
//         PageSetting.List = pushData;
//     }
// );

watchEffect(() => {
    //一阶表格监控
    TableParams = useCommonParams.FirstTableParams;
    GetDetailData(TableParams);
});
watch(
    //二阶表格监控
    () => useCommonParams.SecondTableParams,
    (newValue) => {
        GetSecondTable(TableParams, newValue);
    },
    {
        deep: true
    }
);
</script>

<template>
    <div>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.TopTitle" :columns="PageSetting.Columns"
            sticky
                :key="PageSetting.TopTitle" bordered size="small" :data-source="PageSetting.List" :pagination="false"
                :data-all="PageSetting.List"
                :flexwidth="1"
                :row-class-name="getRowClassNmae" export-file-name="导出">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
        <a-card>
            <DynamicTable row-key="id" :header-title="PageSetting.BottomTitle" :columns="DownThridColumns"
                :key="PageSetting.BottomTitle" bordered size="small" :data-source="PageSetting.BottomList"
                sticky
                :flexwidth="1"
                :data-all="PageSetting.BottomList"
                :pagination="false" export-file-name="导出">
                <template #export-button>
                    <a-button type="success"> 导出 </a-button>
                </template>
            </DynamicTable>
        </a-card>
    </div>
</template>
<style lang="less" scoped>
:deep(.ant-table) {
    height: 35vh !important;
}

:deep(.ant-table-container) {
    height: 35vh !important;
}

:deep(.ant-table-wrapper) {
    height: 35vh !important;
    // overflow: auto;
}

:deep(.ant-table-body) {
    height: 35vh !important;
    overflow: auto;
}


</style>