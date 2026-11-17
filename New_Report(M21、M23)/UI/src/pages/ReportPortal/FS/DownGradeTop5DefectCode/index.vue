<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import { cloneDeep } from 'lodash-es';
import { useLoading } from '@/components/feat/Loading';


  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
let repeatData = ['PROD'];
const UseParamsAdd = AddCommonPageParams();
const FactoryType = UseParamsAdd.Fab;
const To5DefectCode = ref<HTMLElement | null>(null);
function getImport(site: any) {
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/DownGradeTop5DefectCode/${site}/form-schema`);
    const { ChangeColumns } = require(`@/pages/ReportPortal/FS/DownGradeTop5DefectCode/${site}/Table`);
    return { schemas, optionsdata, ChangeColumns };
}

let { schemas, optionsdata, ChangeColumns } = getImport(UseParamsAdd.Fab);
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
    exceljsparams: {
        Header: {}
    }
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    openFullLoading();
    let { Top5OperationsLabel, Top5OperationsProps, Top5DefectCodeColumns } = ChangeColumns(values.Operation);
    // let dynamicColumns: TableColumn[] = []
    let baseParams = {
        apiName: "DownGradeTop5DefectCode",
        ReportType: "DownGradeTop5DfctDataSet",
        FactoryType: UseParamsAdd.Fab,
        G_FAC: 'A'
    };
    // let Data1: any[]=[], Data2: any[]=[], Data3: any[]=[], Data4: any[]=[];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    PageSetting.Title = 'LCMA DownGrade Top 5 Defect Code ';
    let { api01 } = await GetCommon({ ...baseParams, ...values });
    UseParamsAdd.AddFormParams({ ...baseParams, ...values })

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

        let QtyYield = item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
        item['Total'] = item['ERRC_DESCR'];
        item['Total-DFCT_CODE'] = `${QtyYield}(${item['QTY']})`;
        // item['Total-DFCT_CODE'] = item['QTY'] 
    });
    api01['Data1']['Table'].forEach((item) => {
        //Sum
        let QtyYield = item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
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

        // let Yield =
        //     item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(3) + '%';
        if (!PROD.includes(item['PROD']) && item['PROD']) PROD.push(item['PROD']);
        if (!Operations.includes(item['WORK_CTR'])) Operations.push(item['WORK_CTR']);
        let AllDefectQTY: number = 0,
            AllOutQty: number = 0;
        AllData.forEach((item1) => {
            if (item1['PROD'] == item['PROD'] && item['OUT_QTY'] == item1['OUT_QTY'] && item['WORK_CTR'] == item1['WORK_CTR']) {
                let QTYData = isNaN(item1['QTY']) ? 0 : parseInt(item1['QTY']);
                AllDefectQTY += QTYData;
            }
            // if(item1['PROD']==item['PROD']&&item1['WORK_CTR']==item['WORK_CTR']){
            //     let OutQtyData = isNaN(item1['OUT_QTY'])?0:item1['OUT_QTY']
            //     AllOutQty+=OutQtyData
            // }
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


    povitData.forEach(item => {
        Top5OperationsProps.forEach(titem => {
            if (titem != "Total") {
                item[`${titem}-DFCT_CODE`] = item[`${titem}`]
            }
        })

    })
    povitData1.forEach(item => {
        Top5OperationsProps.forEach(titem => {
            if (titem != "Total") {
                item[`${titem}-DFCT_CODE`] = item[`${titem}`]
            }
        })
    })
    povitData2.forEach(item => {
        Top5OperationsProps.forEach(titem => {
            if (titem != "Total") {
                item[`${titem}-DFCT_CODE`] = item[`${titem}`]
            }
        })
    })
    

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
                let QtyData = povitData2[index][Oper] ? povitData2[index][Oper].split('%')[1].replace(/[^0-9]/g, '') : 0;
                let Qty = isNaN(QtyData) ? 0 : parseInt(QtyData);
                let TotalOutQtyData = isNaN(item[Oper]) ? 0 : item[Oper];
                TotalOutQty += TotalOutQtyData;
                AllDefectQTY += Qty;
            }
        });
        item['Total'] = TotalOutQty;
        item['Total-DFCT_CODE'] = TotalOutQty;



        povitData2[index]['Total'] =
            (AllDefectQTY * 100) / TotalOutQty == Infinity
                ? ''
                : ((AllDefectQTY * 100) / TotalOutQty).toFixed(3) + `%(${AllDefectQTY})`;
        povitData2[index]['Total-DFCT_CODE'] = povitData2[index]['Total'];

        povitData1[index]['Total'] =
            (TotalOutQty - AllDefectQTY) / TotalOutQty == Infinity
                ? ''
                : (((TotalOutQty - AllDefectQTY) * 100) / TotalOutQty).toFixed(3) + '%';

        povitData1[index]['Total-DFCT_CODE'] = povitData1[index]['Total'];
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
    // exceljs的表頭
    let header = {
        key: [] as any,
        title: [] as any
    };
    PageSetting.Columns.forEach(item => {
        if (item["children"]) {
            item["children"].forEach(citem => {
                header.key.push(citem["dataIndex"]);
                header.title.push(citem["title"]);
            })
        } else {
            header.key.push(item["dataIndex"]);
            header.title.push(item["title"]);
        }
    })
    PageSetting.exceljsparams.Header = header
    closeFullLoading();

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
            if (Data2.length > 0) ProdData.push(Data2);
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
                }
            } else if (ProdData.length == 1 && ProdData[0][j]) {
                //否則直接賦值(如果有值)
                ProdData[0][j]['PROD1'] = j + 1;
                ColumnData = ProdData[0][j];
            }
            else {

                let DataColumn = { PROD1: j + 1, PROD: Prod };
                for (let i = 1; i < ProdData.length; i++) {
                    if (ProdData[i][j]) {
                        //PROD1用以進行排序
                        ProdData[i][j]['PROD1'] = j + 1;
                        ColumnData = Object.assign(DataColumn, ProdData[i][j]);
                    } else {
                        ColumnData = DataColumn
                    }
                }
            }
            //重新處理的數組
            alaldata.push(ColumnData);
            // console.log(ProdData[i]);
        }

        // alaldata.push(ProdData);
    });

    return alaldata;
}
function getRowClassNmae(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (['PROD'].includes(record['PROD1'])) {
            return 'Green';
        }
        // else if(record['PROD1'].length>1){
        //     return 'Yellow';
        // }

    }
}
</script>

<template>
    <div>
        <a-card>
       
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="To5DefectCode">
                        <DynamicTable row-key="To5DefectCode" id="To5DefectCode" :header-title="PageSetting.Title"
                            showFavorite :PageParams="PageParams" sticky :columns="PageSetting.Columns"
                            :key="PageSetting.Columns" bordered export-type="Exceljs"
                            :exceljsparams="PageSetting.exceljsparams" :flexwidth="1" :data-source="PageSetting.List"
                            :data-all="PageSetting.List" :pagination="false" :export-file-name="PageSetting.Title"
                            :row-class-name="getRowClassNmae">
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
