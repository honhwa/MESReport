<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import { cloneDeep } from 'lodash-es';
import { useLoading } from '@/components/feat/Loading';

const { mounted } = useRemoteSelect();
let repeatData = ['PROD'];
let repeatDataBlu = ['DESCR'];
const UseParamsAdd = AddCommonPageParams();
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
// const To5DefectCode = ref<HTMLElement | null>(null);
function getImport(site: any) {
    let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/Top5DefectCodeReport/${site}/form-schema`);
    const {
        Top5DefectCodeColumns,
        Top5OperationsLabel,
        Top5OperationsProps
    } = require(`@/pages/ReportPortal/FS/Top5DefectCodeReport/${site}/Table`);
    return { schemas, optionsdata, Top5DefectCodeColumns, Top5OperationsLabel, Top5OperationsProps };
}
let Fab: string = '';
switch (UseParamsAdd.Fab) {
    case 'BLU1':
        Fab = 'BLU1';
        break;
    case 'LCM1':
    case 'LCM2':
        Fab = 'LCM1';
        break;
    default:
        Fab = 'LCM1';
        break;
}
let { schemas, optionsdata, Top5DefectCodeColumns, Top5OperationsLabel, Top5OperationsProps } = getImport(Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
    exceljsparams: {
        Header: {
            key: [] as any[],
            title: [] as any[],
        }
    }
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    // let dynamicColumns: TableColumn[] = []
    let baseParams = { apiName: 'Top5DefectCode', FactoryType: UseParamsAdd.Fab, G_FAC: 'A' };
    // let Data1: any[]=[], Data2: any[]=[], Data3: any[]=[], Data4: any[]=[];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    PageSetting.Title = 'Top5 Defect Code';
    try {
        let { api01 } = await GetCommon({ ...baseParams, ...values });
        UseParamsAdd.AddFormParams({ ...baseParams, ...values });
        if (Fab == 'LCM1') handledata(api01);
        else handlebludata(api01);
        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
//实现无限滚动或者说懒加载
// InfiniteScroll(To5DefectCode, PageSetting);
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
    if (Fab == 'LCM1') {
        await getDefectCode();
    } else {
        await getErrorCode();
    }
});
async function getDefectCode() {
    let apiparams = { apiName: 'DefectCode', FactoryType: UseParamsAdd.Fab, G_FAC: UseParamsAdd.G_FAC_COM };
    let { api01 } = await GetCommon(apiparams);
    let Data: Object[] = [{ label: 'ALL', value: 'ALL' }];
    api01['Data']['Table'].forEach((item) => {
        Data.push({ value: item['ERRC_NBR'], label: `${item['ERRC_NBR']} defect` });
    });

    api01['Data1']['Table'].forEach((item) => {
        if (item['ERRC_NBR'].includes('defect')) {
            Data.push({
                value: `substr(dfct_code,1,1) = '${item['ERRC_NBR']}'`,
                label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`
            });
        } else {
            Data.push({
                value: item['ERRC_NBR'],
                label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`
            });
        }
    });
    optionsdata.DefectCode = Data;
}
async function getErrorCode() {
    let apiparams = { apiName: 'TopDefectCodeErrorCode', FactoryType: Fab, G_FAC: UseParamsAdd.G_FAC_COM };
    let { api01 } = await GetCommon(apiparams);
    let Data: Object[] = [{ label: 'ALL', value: 'ALL' }];
    api01['Data']['Table'].forEach((item) => {
        Data.push({ value: item['ERRC_NBR'], label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}` });
    });
    optionsdata.errorcode = Data;
}
function EasyPovit(Data: any[], Operations: any[], PROD: any[]) {
    let alaldata: any[] = [];
    Data.forEach((item) => {
        //添加屬性
        let Yield =
            item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item[item['WORK_CTR']] = item['ERRC_DESCR'];
        item[`${item['WORK_CTR']}-DFCT_CODE`] = `${Yield}(${item['QTY']})`;
        // item['PROD1'] = item['PROD']
    });
    PROD.forEach((Prod) => {
        //將原始數組拆分成多個數組在進行合并
        //根據Prod機種的類別進行循環，合并的數組在此定義
        let ProdData: any[] = [];
        Operations.forEach((Work) => {
            //根據Operations站點的類別進行循環，拆分的數組在此定義
            let Data2: any[] = [];
            Data.forEach((item1) => {
                //循環原始數組,將原始數組中滿足當前機種且當前站點有數據的篩選出來
                if (item1[Work] && item1['PROD'] == Prod) {
                    Data2.push(item1);
                }
            });
            // if (Data2.length > 0) {
            ProdData.push(Data2);
            // }
        });
        for (let j = 0; j < 5; j++) {
            //因爲只需要Top5，所以直接寫死長度
            let ColumnData = {};
            //進行合并的Object在此定義
            if (ProdData.length > 1 && ProdData[0][j]) {
                //合并數組的長度大於一且有值才進行Object多項逐次合并
                ColumnData = ProdData[0][j];
                for (let i = 1; i < ProdData.length; i++) {
                    if (ProdData[i][j]) {
                        //PROD1用以進行排序
                        ProdData[i][j]['PROD1'] = j + 1;
                        ColumnData = Object.assign(ColumnData, ProdData[i][j]);
                    }
                    // else{
                    //     ProdData[i].push({PROD1:j+1})
                    //     ColumnData = Object.assign(ColumnData, ProdData[i][j]);
                    //     console.log(ColumnData,ProdData[i][j]);
                    // }
                }
            } else if (ProdData.length == 1 && ProdData[0][j]) {
                //否則直接賦值(如果有值)
                ProdData[0][j]['PROD1'] = j + 1;
                ColumnData = ProdData[0][j];
            } else {
                 
                let DataColumn = { PROD1: j + 1, PROD: Prod };
                for (let i = 1; i < ProdData.length; i++) {
                    if (ProdData[i][j]) {
                        //PROD1用以進行排序
                        ProdData[i][j]['PROD1'] = j + 1;
                        ColumnData = Object.assign(DataColumn, ProdData[i][j]);
                    }else{
                        ColumnData = DataColumn
                    }
                }
            }
            
            //重新處理的數組
            alaldata.push(ColumnData);
        }

        // alaldata.push(ProdData);
    });
    return alaldata;
}
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (['PROD'].includes(record['PROD1'])) {
            return 'Yellow';
        } else return 'addborder';
        // else if(record['PROD1'].length>1){
        //     return 'Yellow';
        // }
    }
}
function handledata(api01: any) {
    let Operations: any[] = [];
    let PROD: any[] = [];
    let ListData: any[] = [];
    api01['Data2']['Table'].forEach((item) => {
        //Total
        item['PROD'] = 'ALL';
    });
    api01['Data3']['Table'].forEach((item) => {
        //右下角總的total
        item['PROD'] = 'ALL';
        item['PROD1'] = 'ALL';

        let QtyYield =
            item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item['Total'] = item['ERRC_DESCR'];
        item['Total-DFCT_CODE'] = `${QtyYield}(${item['QTY']})`;
        // item['Total-DFCT_CODE'] = item['QTY']
    });
    api01['Data1']['Table'].forEach((item) => {
        //Sum
        let QtyYield =
            item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item['Total-DFCT_CODE'] = `${QtyYield}(${item['QTY']})`;
        item['Total'] = item['ERRC_DESCR'];
        // item['PROD'] = 'ALL';
    });
    let ApiData = cloneDeep([
        ...api01['Data0']['Table'],
        ...api01['Data2']['Table'],
        ...api01['Data1']['Table'],
        ...api01['Data3']['Table']
    ]);
    let AllData = [
        ...api01['Data0']['Table'],
        ...api01['Data2']['Table'],
        ...api01['Data1']['Table'],
        ...api01['Data3']['Table']
    ];
    AllData.forEach((item) => {
        item['PROD1'] = item['PROD'];
        if (!PROD.includes(item['PROD']) && item['PROD']) PROD.push(item['PROD']);
        if (!Operations.includes(item['WORK_CTR'])) Operations.push(item['WORK_CTR']);
        let AllDefectQTY: number = 0,
            AllOutQty: number = 0;
        AllData.forEach((item1) => {
            if (
                item1['PROD'] == item['PROD'] &&
                item['OUT_QTY'] == item1['OUT_QTY'] &&
                item['WORK_CTR'] == item1['WORK_CTR']
            ) {
                let QTYData = isNaN(item1['QTY']) ? 0 : parseInt(item1['QTY']);
                AllDefectQTY += QTYData;
            }
        });
        let Yield =
            AllDefectQTY / item['OUT_QTY'] == Infinity ? '' : ((AllDefectQTY * 100) / item['OUT_QTY']).toFixed(3) + '%';
        let OutYield =
            (item['OUT_QTY'] - AllDefectQTY) / item['OUT_QTY'] == Infinity
                ? ''
                : (((item['OUT_QTY'] - AllDefectQTY) * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item['Yield'] = `${Yield}(${AllDefectQTY})`;
        item['OutYield'] = OutYield;
        // item['Total'] = AllOutQty
    });
    let config: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OUT_QTY'
        },
        changeColumn: Operations,
        repeatColumn: repeatData
    };
    let config1: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OutYield'
        },
        changeColumn: Operations,
        repeatColumn: repeatData
    };
    let config2: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'Yield'
        },
        changeColumn: Operations,
        repeatColumn: repeatData
    };

    let povitData = getPovitData(unref(config));
    let povitData1 = getPovitData(unref(config1));
    let povitData2 = getPovitData(unref(config2));

    let povitData3 = EasyPovit(ApiData, Operations, PROD);

    let InsertColumns = {};
    Top5OperationsLabel.forEach((item: string, index: number) => {
        InsertColumns[Top5OperationsProps[index]] = item;
        InsertColumns[`${Top5OperationsProps[index]}-DFCT_CODE`] = item;
    });
    PageSetting.Columns = Top5DefectCodeColumns;

    povitData.forEach((item, index) => {
        let TotalOutQty: number = 0;
        let  AllDefectQTY: number = 0;
        // item['Total'] = 0;
        Operations.forEach((Oper) => {
            if (Oper) {
                let QtyData = povitData2[index][Oper]
                    ? povitData2[index][Oper].split('%')[1].replace(/[^0-9-]/g, '')
                    : 0;
                let Qty = isNaN(QtyData) ? 0 : parseInt(QtyData);
                let TotalOutQtyData = isNaN(item[Oper]) ? 0 : item[Oper];
                TotalOutQty += TotalOutQtyData;
                AllDefectQTY =AllDefectQTY + Qty;
            }
        });
        item['Total'] = TotalOutQty;
        povitData2[index]['Total'] =
            (AllDefectQTY * 100) / TotalOutQty == Infinity
                ? ''
                : ((AllDefectQTY * 100) / TotalOutQty).toFixed(3) + `%(${AllDefectQTY})`;
        povitData1[index]['Total'] =
            (TotalOutQty - AllDefectQTY) / TotalOutQty == Infinity
                ? ''
                : (((TotalOutQty - AllDefectQTY) * 100) / TotalOutQty).toFixed(3) + '%';
        ListData.push(item);
        ListData.push(povitData1[index]);
        ListData.push(povitData2[index]);
        povitData3.forEach((j) => {
            if (j['PROD'] == item['PROD']) {
                ListData.push(j);
            }
        });
        if (index < povitData.length - 1) ListData.push({ PROD1: 'PROD', ...InsertColumns });
    });

    PageSetting.List = ListData;
    PageSetting.exceljsparams.Header.key= ['PROD1',...Top5OperationsProps];
    PageSetting.exceljsparams.Header.title = ['PROD',...Top5OperationsLabel];
}
function handlebludata(api01: any) {
    let Operations: any[] = [];
    let PROD: any[] = [];
    let ListData: any[] = [];
    api01['Data0']['Table'].forEach((item) => {
        //Total
        item['PROD'] = item['DESCR'];
    });
    let ApiData = cloneDeep(api01['Data0']['Table']);
    let AllData = api01['Data0']['Table'];
    AllData.forEach((item) => {
        item['PROD1'] = item['PROD'];
        if (!PROD.includes(item['PROD']) && item['PROD']) PROD.push(item['PROD']);
        if (!Operations.includes(item['WORK_CTR'])) Operations.push(item['WORK_CTR']);
        let AllDefectQTY: number = 0,
            AllOutQty: number = 0;
        AllData.forEach((item1) => {
            if (
                item1['PROD'] == item['PROD'] &&
                item['OUT_QTY'] == item1['OUT_QTY'] &&
                item['WORK_CTR'] == item1['WORK_CTR']
            ) {
                let QTYData = isNaN(item1['QTY']) ? 0 : parseInt(item1['QTY']);
                AllDefectQTY += QTYData;
            }
        });
        let Yield =
            AllDefectQTY / item['OUT_QTY'] == Infinity ? '' : ((AllDefectQTY * 100) / item['OUT_QTY']).toFixed(3) + '%';
        let OutYield =
            (item['OUT_QTY'] - AllDefectQTY) / item['OUT_QTY'] == Infinity
                ? ''
                : (((item['OUT_QTY'] - AllDefectQTY) * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item['Yield'] = `${Yield}(${AllDefectQTY})`;
        item['OutYield'] = OutYield;
        // item['Total'] = AllOutQty
    });
    let config: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OUT_QTY'
        },
        changeColumn: Operations,
        repeatColumn: repeatDataBlu
    };
    let config1: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OutYield'
        },
        changeColumn: Operations,
        repeatColumn: repeatDataBlu
    };
    let config2: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'Yield'
        },
        changeColumn: Operations,
        repeatColumn: repeatDataBlu
    };

    let povitData = getPovitData(unref(config));
    let povitData1 = getPovitData(unref(config1));
    let povitData2 = getPovitData(unref(config2));

    let povitData3 = EasyPovit(ApiData, Operations, PROD);

    let InsertColumns = {};
    Top5OperationsLabel.forEach((item: string, index: number) => {
        InsertColumns[Top5OperationsProps[index]] = item;
        InsertColumns[`${Top5OperationsProps[index]}-DFCT_CODE`] = item;
    });
    PageSetting.Columns = Top5DefectCodeColumns;
    
    povitData.forEach((item, index) => {
        let TotalOutQty: number = 0,
            AllDefectQTY: number = 0;
        // item['Total'] = 0;
        Operations.forEach((Oper) => {
            if (Oper) {
                let QtyData = povitData2[index][Oper]
                    ? povitData2[index][Oper].split('%')[1].replace(/[^0-9]/g, '')
                    : 0;
                let Qty = isNaN(QtyData) ? 0 : parseInt(QtyData);
                let TotalOutQtyData = isNaN(item[Oper]) ? 0 : item[Oper];
                TotalOutQty += TotalOutQtyData;
                AllDefectQTY += Qty;
            }
        });
        debugger
        
        item['Total'] = TotalOutQty;
        povitData2[index]['Total'] =
            (AllDefectQTY * 100) / TotalOutQty == Infinity
                ? ''
                : ((AllDefectQTY * 100) / TotalOutQty).toFixed(3) + `%(${AllDefectQTY})`;
        povitData1[index]['Total'] =
            (TotalOutQty - AllDefectQTY) / TotalOutQty == Infinity
                ? ''
                : (((TotalOutQty - AllDefectQTY) * 100) / TotalOutQty).toFixed(3) + '%';
        ListData.push(item);
        ListData.push(povitData1[index]);
        ListData.push(povitData2[index]);
        povitData3.forEach((j) => {
            if (j['PROD'] == item['PROD']) {
                ListData.push(j);
            }
        });
        if (index < povitData.length - 1) ListData.push({ PROD1: 'PROD', ...InsertColumns });
    });

    PageSetting.List = ListData;
}
</script>

<template>
    <div>
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div >
                        
                    <!--     :exceljsparams="PageSetting.exceljsparams" -->
                    <DynamicTable
                        showFavorite
                        sticky
                        :id="PageSetting.Title"
                        :PageParams="PageParams"
                        row-key="To5DefectCode"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        export-type="Multi"
                        :data-source="PageSetting.List"
                        :data-all="PageSetting.List"
                        :pagination="false"
                        :export-file-name="PageSetting.Title"
                        :row-class-name="getRowClassNmae"
                        :flexwidth="1"
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
    </div>
</template>
