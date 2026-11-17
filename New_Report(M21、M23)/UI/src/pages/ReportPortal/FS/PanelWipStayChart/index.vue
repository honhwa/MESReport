<template>
    <div class="PanelWipStayChart">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="tableref">
                    <DynamicTable row-key="id" :header-title="PageSetting.Title" :columns="PageSetting.columns" showFavorite
                        :flexwidth="1" :data-all="PageSetting.moreData" sticky :PageParams="PageParams"
                        :key="PageSetting.columns" bordered :data-source="PageSetting.List" :pagination="false"
                        export-file-name="导出" :row-class-name="getRowClassNmae">
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
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { getSumData, dclick, antcustomCell, antdclick, getObjArrayCol, getLastColumns } from '@/pages/ReportPortal/utils/common';
import { message } from 'ant-design-vue';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { useLoading } from '@/components/feat/Loading';
import router from '@/router';
import { mousemodal } from './Right';
// console.log("輸出路由信息",router.currentRoute.value.meta.title);


const UseParamsAdd = ReportCommonWithOut();
const tableref = ref<HTMLElement | null>(null);
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
let importurl = ref('LCM');
let PageSetting = reactive({
    List: [] as any,
    Datedata: [] as any,
    columns: [] as any,
    moreData: [] as any,
    Title: "",
    test: "",
});


switch (UseParamsAdd.Fab) {
    case 'LCM1':
        importurl.value = UseParamsAdd.Fab;
        PageSetting.Title = "LCMA Panel Wip Stay Chart ";
        break;
    case 'LCM2':
        importurl.value = 'LCM1';
        PageSetting.Title = "LCM2 Panel Wip Stay Chart ";
        break;
    case 'SET1':
        importurl.value = 'LCM1';
        PageSetting.Title = "LCMA Panel Wip Stay Chart ";
        break;
    case 'SET2':
        importurl.value = 'LCM1';
        PageSetting.Title = "LCM2_TV Panel Wip Stay Chart ";
        break;
    case 'BLU1':
        importurl.value = UseParamsAdd.Fab;
        PageSetting.Title = "LCMA Panel Wip Stay Chart ";
        break;
    default:
        importurl.value = UseParamsAdd.Fab;
        break;
}
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/FS/PanelWipStayChart/${site}/form-schema`);
    const {
        PanelWipOperColumns,
        PanelWipProdColumns,
        PanelOperWipProp,
        PanelProdWipProp,
        LCM2indexPanelOperWipProp,
        LCM2indexcolumns,
    } = require(`@/pages/ReportPortal/FS/PanelWipStayChart/${site}/Table`);
    return {
        schemas, PanelWipOperColumns, PanelWipProdColumns, PanelOperWipProp, PanelProdWipProp, LCM2indexPanelOperWipProp,
        LCM2indexcolumns
    };
}
let { schemas, PanelWipOperColumns, PanelWipProdColumns, PanelOperWipProp, PanelProdWipProp, LCM2indexPanelOperWipProp,
    LCM2indexcolumns } = getImport(
        importurl.value
    );
const { InfiniteScroll } = useRemoteSelect();


const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});




let bottomparams = {
    apiName: '',
    FactoryType: UseParamsAdd.Fab,
    G_FAC: 'A',
    StrSql_w: '',
    work_ctr: '',
    sql_qcck: '',
    stayhrs: '',
    sqlwodueday: '',
    category: '',
    panel_size: '',

};
let PanelWipLabel;
let PanelWipColumns;
async function confirm(values) {
    openFullLoading();
    /**values为所设定的所有参数,将values里为undefined的值赋为空*/
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }

        // 更換了form-schema的size，bigprod，prod
        if (key == "EntyGroup" && values["EntyGroup"]) {
            //    不需要傳遞bigprod
            // values["bigproduct"] = "ALL";
            values["Size"] = "ALL"
        }
        if (key == "pord") {
            values[key] = values[key].split(/[\r\n\t]/).filter(item => item);
        }
    });
    let apiName = '';
    switch (values['category']) {
        case '1':
        case '2':
            apiName = 'PnlWipStayDataSet';
            bottomparams['apiName'] = 'PnlWipStayDetail';
            break;
        case '3':
            apiName = 'PnlWipStayDataSet1';
            bottomparams['apiName'] = 'PnlWipStayDetail1';
        default:
            break;
    }
    switch (UseParamsAdd.Fab) {
        case 'LCM1':
        case 'LCM2':
        case 'SET1':
        case 'SET2':
            if (values['category'] == '1' || values['category'] == '2') {
                PanelWipLabel = PanelOperWipProp;
                PanelWipColumns = PanelWipOperColumns;
            } else if (values['category'] == '3') {
                PanelWipLabel = PanelProdWipProp;
                PanelWipColumns = PanelWipProdColumns;
            }
            break;
        case 'BLU1':
            if (values['category'] == '1') {
                PanelWipLabel = PanelOperWipProp;
                PanelWipColumns = PanelWipOperColumns;
            } else if (values['category'] == '2') {
                PanelWipLabel = PanelProdWipProp;
                PanelWipColumns = PanelWipProdColumns;
            }
            break;
        default:
            break;
    }
    let baseParams = { apiName: apiName, FactoryType: UseParamsAdd.Fab, G_FAC: 'A' };
    try {
        /**组装api所需参数并发起請求并解构赋值 */
        let { api01 } = await GetCommon({ ...baseParams, ...values });
        let Data = api01['Data']['Table'];
        if (Data.length <= 0) {
            closeFullLoading();
            return message.warning('當前條件下無數據!');
        }
        switch (UseParamsAdd.Fab) {
            // 後端返回格式不統一，有時候是大寫有時候是小寫
            case 'LCM2':
                Data.forEach((item) => {
                    item['Operation'] = item['OPERATION'];
                });
                break;
            default:
                break;
        }
        switch (values['category']) {
            // 舊版原本就有問題，現在顯示新的方式
            case '3':
                let middleprops: string[] = [];
                let column = [
                    {
                        title: 'Operation',
                        align: 'center',
                        dataIndex: 'WORK_CTR',
                        key: 'WORK_CTR'
                    }
                ];

                for (let i in Data[0]) {
                    if (!['SUM(QTY_ALL)', 'WORK_CTR', 'Operation'].includes(i)) {
                        column.push({
                            title: i.split('_')[1].replace(')', ''),
                            align: 'center',
                            dataIndex: i,
                            key: i
                        });
                        if (!middleprops.includes(i)) {
                            middleprops.push(i);
                        }
                    }
                }
                column.push({
                    title: 'sum',
                    align: 'center',
                    dataIndex: 'SUM(QTY_ALL)',
                    key: 'SUM(QTY_ALL)'
                });
                dclick(column, Object.keys(Data[0]), clickfunc);
                PageSetting.columns = column;
                if (Data.length <= 0) {
                    return message.warning('當前條件下無數據!');
                }
                let pushdata = cloneDeep(Data);
                let BANK = pushdata.filter((item) => parseInt(item.WORK_CTR) <= 1000);
                let count = BANK.length;
                let BANK_Sub = getSumData(BANK.length, 0, Data, [...middleprops, 'SUM(QTY_ALL)']);
                let WIPSUM_Sub = getSumData(Data.length, BANK.length, Data, [...middleprops, 'SUM(QTY_ALL)']);
                let ALLSUM_Sub = getSumData(Data.length, 0, Data, [...middleprops, 'SUM(QTY_ALL)']);
                pushdata.splice(count, 0, Object.assign(BANK_Sub, { WORK_CTR: 'BANK_SUM' }));
                pushdata.splice(pushdata.length, 0, Object.assign(WIPSUM_Sub, { WORK_CTR: 'WIP_SUM' }));
                pushdata.splice(pushdata.length, 0, Object.assign(ALLSUM_Sub, { WORK_CTR: 'SUM' }));
                let keyarr = Object.keys(pushdata[0]);
                pushdata.forEach((item, index) => {
                    keyarr.forEach((kitem) => {
                        if ([0, '0(0)'].includes(item[kitem])) {
                            item[kitem] = '';
                        }
                    });
                });
                PageSetting.moreData = pushdata;
                if (PageSetting.moreData.length > 100) {
                    PageSetting.List = PageSetting.moreData.slice(0, 40);
                } //预先加载40条数据
                else {
                    PageSetting.List = PageSetting.moreData;
                }

                bottomparams['category'] = api01['Message']['category'];
                break;
            default:
                let PanelFormParams = api01['Message'];
                bottomparams['StrSql_w'] = PanelFormParams['StrSql_w'];
                (bottomparams['sql_qcck'] = PanelFormParams['sql_qcck']),
                    (bottomparams['sqlwodueday'] = PanelFormParams['sqlwodueday']),
                    (bottomparams['category'] = PanelFormParams['category']);
                getSubData(Data, { ...baseParams, ...values });
                break;
        }
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }

}
function clickfunc(record, rowindex, column, item, index) {
    /*  console.log("record", record);
     console.log("rowindex", rowindex);
     console.log("column", column);
     console.log("item", item);
     console.log("index", index); */
    if (['LCM1', 'LCM2', 'SET1', 'SET2'].includes(bottomparams.FactoryType)) {
        switch (record['Operation']) {
            case 'SUM':
                bottomparams['work_ctr'] = '';
                break;
            case 'BANK_SUM':
                bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                break;
            case 'WIP_SUM':
                bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                break;
            default:
                bottomparams['work_ctr'] = ` and a.work_ctr= ${record['Operation']}`;
                break;
        }
    } else if (['BLU1'].includes(bottomparams.FactoryType)) {
        if (bottomparams['category'] == '1') {
            switch (record['Operation']) {
                case 'SUM':
                    bottomparams['work_ctr'] = '';
                    break;
                case 'BANK_SUM':
                    bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                    break;
                case 'WIP_SUM':
                    bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                    break;
                default:
                    bottomparams['work_ctr'] = ` and a.work_ctr= ${record['Operation']}`;
                    break;
            }
        } else if (bottomparams['category'] == '2') {
            switch (record['PROD_NBR']) {
                case 'SUM':
                case '-':
                    bottomparams['work_ctr'] = ' ';
                    break;
                case 'BANK_SUM':
                    bottomparams['work_ctr'] = " and a.prod_nbr like '%46' ";
                    break;
                default:
                    bottomparams['work_ctr'] = ` and a.prod_nbr= '${record['PROD_NBR']}'`;
                    break;
            }
        }
    }

    if (bottomparams['category'] == '3') {
        switch (record['WORK_CTR']) {
            case 'SUM':
                bottomparams['work_ctr'] = '';
                break;
            case 'BANK_SUM':
                bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                break;
            case 'WIP_SUM':
                bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                break;
            default:
                bottomparams['work_ctr'] = ` and a.work_ctr= ${record['WORK_CTR']}`;
                break;
        }
        bottomparams['panel_size'] = item['title'].replace('*', '');
    } else {
        // console.log("輸出operaction的值",record['Operation']);
        if (isNaN(Number(record["Operation"]))) {
            bottomparams['Operation'] = ""
        } else {
            bottomparams['Operation'] = Number(record['Operation']);
        }
        bottomparams['stayhrs'] = item['title'].replace('*', '');
    }
    const UseParamsAdd = ReportCommonWithOut();
    UseParamsAdd.AddFirstTableParams("PanelWipStayChart" + UseParamsAdd.Fab, bottomparams);
    router.push({ name: 'PanelWipStayDetail' + UseParamsAdd.Fab });
}
function getSubData(Data: any[], apiparams) {
    let pushdata = cloneDeep(Data);
    let BANK = pushdata.filter((item) => parseInt(item.Operation) <= 1000);
    let count = BANK.length;
    let BANK_Sub = getSumData(BANK.length, 0, Data, PanelWipLabel);
    let WIPSUM_Sub = getSumData(Data.length, BANK.length, Data, PanelWipLabel);
    let ALLSUM_Sub = getSumData(Data.length, 0, Data, PanelWipLabel);
    if (UseParamsAdd.Fab == 'BLU1' && bottomparams['category'] == '2') {
        pushdata.splice(count, 0, Object.assign(BANK_Sub, { [PanelWipLabel[0]]: '-' }));
        pushdata.splice(pushdata.length, 0, Object.assign(WIPSUM_Sub, { [PanelWipLabel[0]]: 'BANK_SUM' }));
        pushdata.splice(pushdata.length, 0, Object.assign(ALLSUM_Sub, { [PanelWipLabel[0]]: 'SUM' }));
    } else {
        pushdata.splice(count, 0, Object.assign(BANK_Sub, { [PanelWipLabel[0]]: 'BANK_SUM' }));
        pushdata.splice(pushdata.length, 0, Object.assign(WIPSUM_Sub, { [PanelWipLabel[0]]: 'WIP_SUM' }));
        pushdata.splice(pushdata.length, 0, Object.assign(ALLSUM_Sub, { [PanelWipLabel[0]]: 'SUM' }));
    }
    let tempcolumns = cloneDeep(PanelWipColumns);
    let clickcolumns = cloneDeep(PanelWipLabel);
    clickcolumns.shift();
    antcustomCell(tempcolumns, ({ record, rowindex, column, item, index }) => {
        return {
            // 合并功能
            ...antdclick(column, clickcolumns, () => {
                /*  console.log("输出record", record);
                 console.log("输出rowindex", rowindex);
                 console.log("输出column", column); */
                if (['LCM1', 'LCM2', 'SET1', 'SET2'].includes(bottomparams.FactoryType)) {
                    switch (record['Operation']) {
                        case 'SUM':
                            bottomparams['work_ctr'] = '';
                            break;
                        case 'BANK_SUM':
                            bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                            break;
                        case 'WIP_SUM':
                            bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                            break;
                        default:
                            bottomparams['work_ctr'] = ` and a.work_ctr= ${record['Operation']}`;
                            break;
                    }
                } else if (['BLU1'].includes(bottomparams.FactoryType)) {
                    if (bottomparams['category'] == '1') {
                        switch (record['Operation']) {
                            case 'SUM':
                                bottomparams['work_ctr'] = '';
                                break;
                            case 'BANK_SUM':
                                bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                                break;
                            case 'WIP_SUM':
                                bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                                break;
                            default:
                                bottomparams['work_ctr'] = ` and a.work_ctr= ${record['Operation']}`;
                                break;
                        }
                    } else if (bottomparams['category'] == '2') {
                        switch (record['PROD_NBR']) {
                            case 'SUM':
                            case '-':
                                bottomparams['work_ctr'] = ' ';
                                break;
                            case 'BANK_SUM':
                                bottomparams['work_ctr'] = " and a.prod_nbr like '%46' ";
                                break;
                            default:
                                bottomparams['work_ctr'] = ` and a.prod_nbr= '${record['PROD_NBR']}'`;
                                break;
                        }
                    }
                }

                if (bottomparams['category'] == '3') {
                    switch (record['WORK_CTR']) {
                        case 'SUM':
                            bottomparams['work_ctr'] = '';
                            break;
                        case 'BANK_SUM':
                            bottomparams['work_ctr'] = ' and a.work_ctr <=1000 ';
                            break;
                        case 'WIP_SUM':
                            bottomparams['work_ctr'] = ' and a.work_ctr > 1000 ';
                            break;
                        default:
                            bottomparams['work_ctr'] = ` and a.work_ctr= ${record['WORK_CTR']}`;
                            break;
                    }
                    bottomparams['panel_size'] = item['title'].replace('*', '');
                } else {
                    // console.log("輸出operaction的值",record['Operation']);
                    if (isNaN(Number(record["Operation"]))) {
                        bottomparams['Operation'] = ""
                    } else {
                        bottomparams['Operation'] = Number(record['Operation']);
                    }


                    bottomparams['stayhrs'] = item['title'].replace('*', '');
                }
                const UseParamsAdd = ReportCommonWithOut();
                // UseParamsAdd.AddFirstTableParams(bottomparams);
                UseParamsAdd.AddFirstTableParams("PanelWipStayChart" + UseParamsAdd.Fab, bottomparams);
                router.push({ name: 'PanelWipStayDetail' + UseParamsAdd.Fab });
            }),
            // 鼠标右键功能
            ...mousemodal({
                record,
                column,
                item,
                Allcolumns: tempcolumns,
                reportname: "Panel Wip Stay Chart",
                apiparams: apiparams,
                DateArray: ["vDate_s_wodueday", "vDate_e_wodueday"],
                ObservedColumnXOptions: getLastColumns(tempcolumns,["Operation","sum"]),
                ObservedColumnY: {label:"Operation",value:"Operation"},
                ObservedColumnYValueOptions: getObjArrayCol(PageSetting.moreData,"Operation",['BANK_SUM','WIP_SUM','SUM' ]),
            })
        }
    });
    // dclick(tempcolumns, clickcolumns, clickfunc);

    PageSetting.columns = tempcolumns;


    // 清除為0或者0(0)的數據
    let keyarr = Object.keys(pushdata[0]);
    pushdata.forEach((item, index) => {
        keyarr.forEach((kitem) => {
            if ([0, '0(0)'].includes(item[kitem])) {
                item[kitem] = '';
            }
        });
    });
    PageSetting.moreData = pushdata;
    console.log("输出",PageSetting.moreData);
    
    if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 40);
    } //预先加载40条数据
    else {
        PageSetting.List = PageSetting.moreData;
    }
}
InfiniteScroll(tableref, PageSetting as any);


</script>
<style scoped ></style>
