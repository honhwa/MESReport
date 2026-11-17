<template>
    <div class="DayliMove">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="DaylimoveTable" >
                    <!--  
                        v-Infinitescroll:40="{ PageSetting }"
                    -->
                    <DynamicTable  showFavorite :PageParams="PageParams" row-key="id"
                        export-type="Api"
                        :excellparams="PageSetting.excellparams" 
                        :header-title="PageSetting.Title" :columns="PageSetting.Columns" :key="PageSetting.Columns" sticky
                        :data-source="PageSetting.moreData" :data-all="PageSetting.moreData" :pagination="false"
                        :export-file-name="PageSetting.Title" :row-class-name="getRowClassNmae" :flexwidth="1">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
            <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted } from 'vue';
import { GetDailyMove } from '@/api/pages/daylimove';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import {
    getRowClassNmae,
    getdiffdate,
    getrowsum,
    Inserttotalrow,
    getfinalsum,
    megercell
} from '@/pages/ReportPortal/utils/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
import dayjs from 'dayjs';

const router = useRouter();
const UseParamsAdd = ReportCommonWithOut();
const DaylimoveTable = ref<HTMLElement | null>(null);
//vue3的mixin
const { mounted, InfiniteScroll, mountedowner, mountedOper } = useRemoteSelect();

function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/daylimove/${site}/form-schema`);
    const { LCM1Columns,
        LCM2Columns,
        BLUColumns,
        SET1PCColumns,
        SET1TestColumns,
        SET1TestNewColumns,
        SET2PCColumns,
        SET2TestColumns,
        SET2TestNewColumns, } = require(`@/pages/ReportPortal/FS/daylimove/${site}/Table`);
    return {
        schemas,
        LCM1Columns,
        LCM2Columns,
        BLUColumns,
        SET1PCColumns,
        SET1TestColumns,
        SET1TestNewColumns,
        SET2PCColumns,
        SET2TestColumns,
        SET2TestNewColumns,
        optionsdata
    };
}
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
let importurl = ref("");
switch (UseParamsAdd.Fab) {
    case "BLU1":
    case "BLU2":
    importurl.value = "BLU"
        break;
    default:
    importurl.value = "LCM"
        break;
}
let repeatData = ['WORK_CTR', 'WO_NBR', 'LCM_OWNER', 'SIZE1', 'PROD_NBR', 'CUSTOMER'];
let { schemas, LCM1Columns,
    LCM2Columns,
    BLUColumns,
    SET1PCColumns,
    SET1TestColumns,
    SET1TestNewColumns,
    SET2PCColumns,
    SET2TestColumns,
    SET2TestNewColumns, optionsdata } = getImport(importurl.value);

let PageSetting = reactive({
    List: [] as any,
    APIColumns: [] as any,
    Columns: [] as any,
    moreData: [] as any,
    SQLHidden: '',
    Title: '',
    baseColumns: [],
    excellparams:{},
});

const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
    /* submitButtonOptions:{
        text: '查询333'
    } */
});
/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {
    let calcondition = ['SIZE1'];
    openFullLoading();
    let baseParams = { name: 'Date', apiName: 'DailyMove', FactoryType: UseParamsAdd.Fab, G_FAC: 'A', building: '' };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        } /* else if (key == 'bigProduct') {
            values['Product'] = values[key];
        } else if (key == 'Product') {
            values['prod_nbr'] = values[key];
        } */ else if (key == "wo_num") {
            values[key] = values[key].split(/[\r\n\t]/).filter(item => item);
        }
    });
    switch (UseParamsAdd.Fab) {
        case 'LCM2':
            PageSetting.Title = 'LCMA Daily Move ';
            calcondition = ['CUSTOMER', 'WORK_CTR', 'SIZE1'];
            break;
        case 'BLU1':
        case 'BLU2':
            if (UseParamsAdd.Fab == 'BLU2') {
                calcondition = ['WORK_CTR', 'SIZE1', 'SIZE1', 'PROD_NBR', 'LCM_OWNER'];
            } else {
                calcondition = ['SIZE1'];
            }
            switch (values['floor']) {
                case '1':
                    values['floor'] = " and floor='1'";
                    break;
                case '2':
                    values['floor'] = " and floor='2'";
                    break;
                case '3':
                    values['floor'] = " and floor='3'";
                    break;
                default:
                    break;
            }
            if (UseParamsAdd.Fab == 'BLU1') {
                PageSetting.Title = 'BLA Daily Move ';
            } else {
                PageSetting.Title = 'BL2 Daily Move ';
            }
            break;
        case 'SET1':
            let routertitle = router.currentRoute.value.meta.title;
            if (routertitle == '(MV202) Daily Move For TW') {
                baseParams.FactoryType = 'SET1_TEST';
            } else if (routertitle == '(MV202) Daily Move For TW new') {
                baseParams.FactoryType = 'SET1_NEW_TEST';
            } else if (routertitle == '(MV202) Daily Move') {
                baseParams.FactoryType = 'SET1';
            }
            PageSetting.Title = 'LCMA_TV Daily Move ';
            calcondition = ['WORK_CTR'];
            break;
        case 'SET2':
            let routertitle2 = router.currentRoute.value.meta.title;
            if (routertitle2 == '(MV202) Daily Move For TW') {
                baseParams.FactoryType = 'SET2_TEST';
            } else if (routertitle2 == '(MV202) Daily Move For TW new') {
                baseParams.FactoryType = 'SET2_NEW_TEST';
            } else if (routertitle2 == '(MV202) Daily Move') {
                baseParams.FactoryType = 'SET2';
            }
            PageSetting.Title = 'LCM2_TV Daily Move ';

            calcondition = ['WORK_CTR'];
            break;
        default:
            calcondition = ['SIZE1'];
            PageSetting.Title = 'LCMA Daily Move ';
            break;
    }
    dynamicFormRef.validate().then(async () => {
        try {
            const {
                Date: {
                    Data: { Table: Data, sql: Message },
                }
            } = await GetDailyMove({ ...baseParams, ...values });
            PageSetting.excellparams = { ...baseParams, ...values };
            if (Data.length == 0) {
                closeFullLoading();
                return message.warn('無數據!');
            }
            PageSetting.APIColumns = getdiffdate(values.vDate_s, values.vDate_e1);
            // 設置導出的時間格式
           /*  Data.forEach((item) => {
                item['PD'] = dayjs(item['PD']).format('YYYY/M/D');
            });
            for (let i = 0; i < PageSetting.APIColumns.length; i++) {
                PageSetting.APIColumns[i] = dayjs(PageSetting.APIColumns[i]).format('YYYY/M/D');
            }
            PageSetting.SQLHidden = Message;
            let DateColumns: any = [];
            PageSetting.APIColumns.map((item: any) => {
                DateColumns.push({
                    title: item,
                    align: 'center',
                    dataIndex: item,
                    resizable: true
                });
            }); */
            let showattr = '';
            switch (UseParamsAdd.Fab) {
                case 'BLU1':
                case 'BLU2':
                    PageSetting.baseColumns = BLUColumns;
                    showattr = 'PROD_NBR';
                    break;
                case 'SET1':
                case 'SET2':
                    let routertitle = router.currentRoute.value.meta.title;
                    if (routertitle === '(MV202) Daily Move For TW') {
                        PageSetting.baseColumns = SET1TestColumns;

                        showattr = 'CUSTOMER';
                    } else if (routertitle === '(MV202) Daily Move For TW new') {
                        PageSetting.baseColumns = SET1TestNewColumns;
                        showattr = 'EQUIP_NBR';
                    } else {
                        PageSetting.baseColumns = SET1PCColumns;
                        showattr = 'CUSTOMER';
                    }
                    break;
                case "LCM1":
                case "LCM2":
                    PageSetting.baseColumns = LCM1Columns;
                    showattr = 'CUSTOMER';
                default:
                    showattr = 'CUSTOMER';
                    break;
            }
            /* PageSetting.Columns = [
                ...PageSetting.baseColumns,
                ...DateColumns,
                {
                    title: '總計',
                    align: 'center',
                    // dataIndex: 'Total',
                    dataIndex: 'SUM',
                    resizable: true
                }
            ]; */
            PageSetting.Columns = Object.keys(Data[0]).map(item=>{
                return {
                    title: item,
                    align: 'center',
                    dataIndex: item,
                    resizable: true
                }
            })

           /*  //vue3或者adv无法监测push,shift等方法
            // debugger
            let config: PovitParams = {
                Data,
                povitColumn: {
                    PD: 'QTY'
                },
                changeColumn: PageSetting.APIColumns,
                repeatColumn: repeatData
            };
            let povitData = getPovitData(unref(config)); //进行类行列转置的动作
            // 添加總計行
            getrowsum(povitData, PageSetting.APIColumns, 'Total');
            // 最後總計
            let totalsum = getfinalsum(povitData, [...PageSetting.APIColumns, 'Total'], showattr, '合計');
            Inserttotalrow(
                povitData,
                calcondition,
                [...PageSetting.APIColumns, 'Total'],
                showattr,
                '小計',
                false,
                true,
                ['WORK_CTR']
            );
            povitData.push(totalsum);
            PageSetting.moreData = povitData; //进行表格合并
            megercell(PageSetting.Columns, ['WORK_CTR', 'SIZE1', 'LCM_OWNER', 'PROD_NBR', showattr], povitData); */
            PageSetting.moreData = Data; //进行表格合并
            console.log(PageSetting.Columns);
            
            // megercell(PageSetting.Columns, ['WORK_CTR', 'SIZE1', 'LCM_OWNER', 'PROD_NBR', showattr], PageSetting.moreData ); 
            megercell(PageSetting.Columns, ['Operation', 'Work Order', 'Size', 'Owner', 'Customer',showattr], PageSetting.moreData ); 
            if (PageSetting.moreData.length > 100) {
                PageSetting.List = PageSetting.moreData.slice(0, 40);
            } //预先加载40条数据
            else {
                PageSetting.List = PageSetting.moreData;
            }

            closeFullLoading();
            // console.log('輸出最終的columns', PageSetting.Columns);
        } catch (error) {
            closeFullLoading();
        }
    });
}

InfiniteScroll(DaylimoveTable, PageSetting);
let TableHeight = ref(0);
onMounted(async () => {
    /*  await mountedowner(optionsdata, UseParamsAdd.Fab);
     await mountedOper(optionsdata, UseParamsAdd.Fab); 
     await mounted(optionsdata, UseParamsAdd.Fab);
     */
    TableHeight.value = Math.round(document.documentElement.clientHeight) - 200;
});
</script>

