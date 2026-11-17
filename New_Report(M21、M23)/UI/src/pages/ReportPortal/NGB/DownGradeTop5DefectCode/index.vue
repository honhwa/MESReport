<template>
    <div >
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
                    <div class="NGBScrollForm">
                     <SchemaForm @submit="confirm"> </SchemaForm>
                </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  class="ScrollTable" ref="QualityTable">
                        <DynamicTable sticky
                            showFavorite
                            :PageParams="PageParams"
                            id="DownGradeTop5DefectCode"
                            export-type="Multi"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small"                             
                            :data-source="PageSetting.List"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassName_Sel"
                            :style="{ display: 'table',width:PageSetting.Columns.length > 3 ? '100% !important' : '50% !important'}"
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                     <div hidden>{{ PageSetting.SQLHidden }}</div>
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref ,unref} from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import {getrowsum, Inserttotalrow,Sumtotalrow} from '@/pages/ReportPortal/utils/NGBcommon';
import { message } from 'ant-design-vue';
import type { TableColumn } from '@/components/core/dynamic-table';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter, useRoute } from 'vue-router';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { ReportCommon } from '@/store/module/ReportCommon';
import { PovitParams ,getPovitData} from "@/utils/reportcommon/provit";
import { deRepeat } from '@/utils/helper/povitHelper';


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);


const router = useRouter();
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `DownGradeTop5DefectCode-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);
let Top5OperationsLabel: any = [];
let Top5OperationsProps: any = [];
let Operations: any[] = [];
let InsertColumns = {};
let Message:any;
let TopTableParams:any;
let repeatData = ['PROD'];



// 頁面數據
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Message: {},
    Title: "",
    moreData: [] as any,
    x: '' as any,
    SQLHidden:'',
    APIColumns: [] as any,
});


function getImport(site: any) {    
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/NGB/DownGradeTop5DefectCode/${bigfab}/form-schema`);
    return { schemas,  GrpOptions };
}
let { schemas, GrpOptions } = getImport(useCommonParams.Fab);
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


async function confirm(values) {
    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    Top5OperationsLabel = [];
    Top5OperationsProps = [];
    Operations= [];

    let AllColumn=[{lable:'1400',value: "1400"},
                   {lable:'1590',value: "1590"},
                   {lable:'C KEN',value: "1600"},
                ]

     AllColumn.forEach((item) => {
           if((values.SpecialOper.includes(item['value']) && item['value']!="1600")||(item['value']=="1600")){
                Top5OperationsLabel.push(item['lable'])
                Top5OperationsProps.push(item['value'])
                Operations.push(item['value'])
           };           
        });

   
    TopTableParams =  { name: "Date",apiName: "DownGradeTop5DataSet", FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };

    TopTableParams = { ...TopTableParams, ...values }    
   
    try {
            const { Date: {
                Data: { Table: Data },//main data every prod
                Data_2: { Table: Data_2 },// 1600 defect
                Data_3: { Table: Data_3 },// 1400 defect
                Data_4: { Table: Data_4 },// 1590 defect
                SQL01,
                SQL16,
                SQL14,
                SQL15,
                sql_Transdate,
                sql_dgdfct_where,
                sql_SpecialOper,
                lstr_ProdType,

            } } = await GetCommon(TopTableParams);

            PageSetting.SQLHidden = "DownGradeTop5DefectCode-SQLHidden SQL01:"+SQL01+"; SQL16:"+SQL16+"; SQL14:"+SQL14+"; SQL15:"+SQL15
            
             Message = {                
                sql_Transdate:sql_Transdate,
                sql_dgdfct_where:sql_dgdfct_where,
                sql_SpecialOper:sql_SpecialOper,
                lstr_ProdType:lstr_ProdType,
            };            
            
            const Top5Columns = GetColumns(Top5OperationsLabel, Top5OperationsProps)

            const Top5DefectCodeColumns: any[] = [
            {
                //title: 'PROD',//label
                align: 'center',
                dataIndex: 'PROD1',//prop屬性,
            },
            ...Top5Columns
            ];

         //  console.log("sQL01:"+SQL01)
        //   console.log("sQL16:"+SQL16)
        //   console.log("sQL14:"+SQL14)
        //   console.log("sQL15:"+SQL15)


            if (Data.length == 0 && Data_2.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
            }

            PageSetting.Title=`${useCommonParams.Fab} DownGrade Top 5 Defect Code`

            ReportItem.APIMessage = JSON.stringify(Message);  
            ReportItem.FormParams=TopTableParams;

            InsertColumns = {};
            Top5OperationsLabel.forEach((item: string, index: number) => {                
                  InsertColumns[Top5OperationsProps[index]] = item;
                  InsertColumns[`${Top5OperationsProps[index]}-DFCT_CODE`] = item;
                  InsertColumns[`Title`] = item;
              
            });

            //增加站點給 defect
            Data_2.forEach((item) => {
            item['WORK_CTR'] = '1600';
            });
           
            if(values.SpecialOper.includes("1400")){
                Data_3.forEach((item) => {
                 item['WORK_CTR'] = '1400';
                });
            }

            if(values.SpecialOper.includes("1590")){
                Data_4.forEach((item) => {
                item['WORK_CTR'] = '1590';
                });
            }

            //AllTop5Data 機種為ALL的 top5資料數據整理 （增加 by 站點pass量總量OUT_QTY 和 by 站點prework量總量REWORK_QTY）
            ///-------------↓↓↓↓↓↓↓↓↓↓↓↓------↓↓↓↓↓↓↓↓↓↓↓↓---------------------------
            let AllTop5Data: any = []
            if(values.SpecialOper.includes("1590") && values.SpecialOper.includes("1400")){
                    //總不良 BY站點
                AllTop5Data = cloneDeep([ 
                ...Data_2,//1600       
                ...Data_3,//1400
                ...Data_4,//1590            
                ]
               );  
            }else if(values.SpecialOper.includes("1400")){
                  //總不良 BY站點
                  AllTop5Data = cloneDeep([ 
                    ...Data_2,//1600       
                    ...Data_3,//1400
                    //...Data_4,//1590            
                    ]
               );  
            }else if(values.SpecialOper.includes("1590")){
                //總不良 BY站點
                AllTop5Data = cloneDeep([ 
                    ...Data_2,//1600       
                    //...Data_3,//1400
                    ...Data_4,//1590            
                    ]
               );  
            }else{
                  //總不良 BY站點
                  AllTop5Data = cloneDeep([ 
                ...Data_2,//1600  
                ]
               );    
            }

            //計算各料號 by站點的總不良數
            let TotalData = cloneDeep([ ...Data,]);
            let { insertrowList}=Sumtotalrow(TotalData, ["OUT_QTY","PROD","WORK_CTR"], ["QTY"], "DFCT_CODE")
            //原來Data 增加欄位REWORK_QTY 總不良數
            for (let i = 0; i < insertrowList.length; i++) {
                Data.forEach((item) => {
                    if(item['WORK_CTR']==insertrowList[i]['WORK_CTR']
                       && item['PROD']==insertrowList[i]['PROD']&&item['OUT_QTY']==insertrowList[i]['OUT_QTY'] ){
                        item['REWORK_QTY'] = insertrowList[i]['QTY'];
                    }
                  
                });
            }

            //console.log("Data",Data)

            //去重複計算各站點ALL 的OUT_QTY,REWORK_QTY
            let TempData = cloneDeep([ ...Data,]);
            let TempData2=deRepeat(TempData,['OUT_QTY','PROD','WORK_CTR','REWORK_QTY'])
           
            Operations.forEach((Oper) => {
                let TotalOutQtyData=0;
                let ReworkQtyData=0;
                for (let i = 0; i < TempData2.length; i++) {                
                    if (Oper==TempData2[i]['WORK_CTR']){
                        TotalOutQtyData=TotalOutQtyData+TempData2[i]['OUT_QTY']
                        ReworkQtyData=ReworkQtyData+TempData2[i]['REWORK_QTY']
                    }
                }

                AllTop5Data.forEach((item) => {
                    if (Oper==item['WORK_CTR']){
                        item['OUT_QTY'] = TotalOutQtyData; 
                        item['REWORK_QTY'] =ReworkQtyData; 
                    }
                });
               
            });
            //console.log("AllTop5Data",AllTop5Data)
            /////////-----------↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑---------------------------
             

             PageSetting.moreData=handledata(Data,AllTop5Data,Data_3,Data_4,values.SpecialOper)
             PageSetting.List = PageSetting.moreData;
             PageSetting.Columns = Top5DefectCodeColumns;

            

       
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }

}

function handledata(Data: any,Data_2: any,Data_3: any,Data_4: any,SpecialOper: any){   
        let PROD: any[] = [];
        let ListData: any[] = [];
        let SelectProd: any[] = [];
        let ApiData_2 = cloneDeep([ ...Data_2,]);

        Data.forEach((item) => {
            if (!SelectProd.includes(item['PROD'])) SelectProd.push(item['PROD']);
        });

       //ALL prod 的top5
       ApiData_2.forEach((item) => {
           item['PROD'] = 'ALL';           
        });

        //用於計算top 5
        let Defct5Data = cloneDeep([        
        ...Data,//every prod top5
        ...ApiData_2,//ALL prod 的top5
        ]);
        

       // every prod sum
       Data.forEach((item) => {         
          let Yield= item['REWORK_QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['REWORK_QTY'] * 100) / item['OUT_QTY']).toFixed(2) + '%';
          //item['Yield']=`${Yield}(${item['REWORK_QTY']})`; 
          if(Yield==""&& item['REWORK_QTY']!=""){
            item['Yield']=`0.00%(${item['REWORK_QTY']})`;
          }else{
            item['Yield']=`${Yield}(${item['REWORK_QTY']})`; 
          }
          item['OutYield'] = (item['OUT_QTY']-item['REWORK_QTY']) / item['OUT_QTY'] == Infinity ? '' :((item['OUT_QTY']-item['REWORK_QTY']) * 100 / item['OUT_QTY']).toFixed(2) + '%';
       });
    
       // all prod sum
        Data_2.forEach((item) => {
            //Total
           item['PROD'] = 'ALL';
           let Yield= item['REWORK_QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['REWORK_QTY'] * 100) / item['OUT_QTY']).toFixed(2) + '%';
           //item['Yield']=`${Yield}(${item['REWORK_QTY']})`; 
           if(Yield==""&& item['REWORK_QTY']!=""){
            item['Yield']=`0.00%(${item['REWORK_QTY']})`;
          }else{
            item['Yield']=`${Yield}(${item['REWORK_QTY']})`; 
          }
           item['OutYield'] = (item['OUT_QTY']-item['REWORK_QTY']) / item['OUT_QTY'] == Infinity ? '' :((item['OUT_QTY']-item['REWORK_QTY']) * 100 / item['OUT_QTY']).toFixed(2) + '%';
        });

        //用於計算Ins-Qty，Yield-Rate，Yield-Lossby站點
        let AllData= [
            ...Data,//every prod sum
            ...Data_2,// all prod sum
          ];
   
    SelectProd.push("ALL");
    
   
    //Ins-Qty by站點，料號轉置
    let config: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OUT_QTY'
        },
        changeColumn: Operations,
        repeatColumn: repeatData
    };
    //Yield-Rate by站點，料號轉置
    let config1: PovitParams = {
        Data: AllData,
        povitColumn: {
            WORK_CTR: 'OutYield'
        },
        changeColumn: Operations,
        repeatColumn: repeatData
    };
     //Yield-Loss: by站點，料號轉置
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

    //TOP5計算
    let povitData3 = EasyPovit(Defct5Data, Operations, SelectProd);

    // console.log("povitData",povitData)
    // console.log("povitData1",povitData1)
    // console.log("povitData2",povitData2)
    // console.log("povitData3",povitData3)

    //Ins-Qty 加總
    povitData.forEach((item, index) => {
        //debugger;
        let TotalOutQty: number = 0,
            AllDefectQTY: number = 0;
        let AllYeild:number = 1;
        // item['Total'] = 0;
        Operations.forEach((Oper) => {
            if (Oper) {                            
                if(povitData1[index][Oper]=="NaN%"){
                    povitData1[index][Oper]=''
                }
            }
        });
        
         item['PROD1']="Ins-Qty:"       
         povitData2[index]['PROD1']="Yield-Loss:"
           
         povitData1[index]['PROD1']="Yield-Rate:%"
         povitData1[index]['OutYield']=povitData1[index]['PROD1']        
       
        if (index < povitData.length) ListData.push({ PROD1: item['PROD'], ...InsertColumns });

        ListData.push(item);
        ListData.push(povitData1[index]);
        ListData.push(povitData2[index]);


        povitData3.forEach((j) => {
            if (j['PROD'] == item['PROD']) {               
               ListData.push(j);
            }
        });
        
        
        
    });
    console.log("333ListData",ListData)
     return ListData
}

function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (['C KEN'].includes(record['Title'])) {
            return 'YellowGray';
        }
             
    }
    
}

//top5 資料組合
function EasyPovit(Data: any[], Operations: any[], PROD: any[]) {
    let alaldata: any[] = [];
    Data.forEach((item) => {
        //添加屬性
        let Yield =
            item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(2) + '%';        
            if(item['DFCT_CODE']==""||item['DFCT_CODE']==null){
                item[item['WORK_CTR']] = "";   
            }else{
                item[item['WORK_CTR']] = `${item['DFCT_CODE']}(${item['ERRC_DESCR']})`;
            }
            
            item[`${item['WORK_CTR']}-DFCT_CODE`] = `${Yield}(${item['QTY']})`;
            item[`${item['WORK_CTR']}-DFCT_CODE2`] = item['DFCT_CODE']; //獲取DFCT_CODE for 二階報表傳參數
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
            if (ProdData.length > 1) {
                //合并數組的長度大於一且有值才進行Object多項逐次合并
                if(ProdData[0][j]){
                  ColumnData = ProdData[0][j];
                }
                
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
            //重新處理的數組
            alaldata.push(ColumnData);
        }

        alaldata.push(ProdData);
   });
    return alaldata;
}


function GetColumns(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            //title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            colSpan: 2,
            children:
                [
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}`,
                        colSpan: 0,
                        rowSpan: 0,
                        col:index,
                        customCell: (record: Object, rowIndex: number, column: Object) => {
                            if (!Reflect.has(record, 'OutYield') && record[item] != record[`${item}-DFCT_CODE`]) {
                                return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    // style: {
                                    //     cursor: 'pointer',
                                    // },
                                    // onDblclick: () => {   
                                    //     ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };
                                    //     router.push({ name: `Top5DefectCodeDetail-${useCommonParams.Fab}` });  
                                    // }
                                }
                            }
                            // else if(!record[item]||record[item]==) {
                            //     return {
                            //         colSpan: 0,
                            //         rowSpan: 0
                            //     }
                            // }
                            else {
                                return {
                                    colSpan: 2,
                                    rowSpan: 1

                                }
                            }
                        },
                    },
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}-DFCT_CODE`,
                        colSpan: 0,
                        rowSpan: 0,
                        col:index,
                        customCell: (record: Object, rowIndex: number, column: Object) => {
                            if (Reflect.has(record, 'OutYield') || record[`${item}-DFCT_CODE`] == record[item]) {
                                return {
                                    colSpan: 0,
                                    rowSpan: 0
                                }
                            }
                            else {                               
                                if(`${item}-DFCT_CODE`=="Total-DFCT_CODE"||record['PROD']=="ALL"){
                                    return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    // classname:"LightBlue",
                                    onDblclick: () => {
                                       
                                        ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };
                                        router.push({ name: `DownGradeTop5DefectCodeDetail-${useCommonParams.Fab}` });  
                                    }, style: {
                                        cursor: 'pointer',
                                        // background: '#9088c0'
                                    }
                                 }  
                                }else{
                                    return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    // classname:"LightBlue",
                                    onDblclick: () => {
                                        ReportItem.FirstTableParams={ ...record, ...column['__originColumn__'] };             
                                        router.push({ name: `DownGradeTop5DefectCodeDetail-${useCommonParams.Fab}` });  
                                    }, style: {'background-color':'rgb(176,244,230)',
                                        cursor: 'pointer',
                                        // background: '#9088c0'
                                    }
                                }
                                }
                                
                            }

                        },

                    }
                ]
        }
    })
}




//实现无限滚动或者说懒加载
//InfiniteScroll(QualityTable, PageSetting);
onMounted(async () => {
   
});

</script>

<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}

.swapper {
    display: flex;
    flex-direction: column;
}
</style>
