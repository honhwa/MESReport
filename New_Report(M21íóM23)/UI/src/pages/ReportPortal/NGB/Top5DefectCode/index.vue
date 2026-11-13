<template>
    <div class="Top5DefectCode">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" Source-show="PageSetting.showmenu">
                    <div class="NGBScrollForm">
                     <SchemaForm @submit="confirm"> </SchemaForm>
                </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  class="ScrollTable" ref="Top5DefectCodeTable">
                        <DynamicTable sticky
                            showFavorite
                            :PageParams="PageParams"
                            id="Top5DefectCode"
                            export-type="Multi"
                            :header-title="PageSetting.Title"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small" :flexwidth='-5'
                             :scroll="{ x: PageSetting.x,}"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassName_Sel"
                            
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
import { reactive, onMounted, ref, unref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import {getrowsum, Inserttotalrow} from '@/pages/ReportPortal/utils/NGBcommon';
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


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
const Top5DefectCodeTable = ref<HTMLElement | null>(null);


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
let ReportNameParams = `Top5DefectCode-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);
let repeatData = ['PROD'];
let Top5OperationsLabel: any = [];
let Top5OperationsProps: any = [];
let Operations: any[] = [];
let InsertColumns = {};
let Message:any;
let TopTableParams:any;



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
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas, GrpOptions } = require(`@/pages/ReportPortal/NGB/Top5DefectCode/${bigfab}/form-schema`);
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
const {  InfiniteScroll, } = useRemoteSelect()



async function confirm(values) {
    openFullLoading();
    PageSetting.List = [];
    PageSetting.Columns = [];

    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
        if (["WorkOrderLike", "Operator"].includes(key)) {
            values[key] = values[key].split(',')
        }
    });

    if (values.SpecialOper==1){
        Top5OperationsLabel=['PCB INSPECTION', 'ASSEMBLY','AGING','C INSPECTION','D INSPECTION', 'Total']
        Top5OperationsProps=[1300,1400,1500,1600,1700,'Total']
        Operations=[1300,1400,1500,1600,1700]

    }else if(values.SpecialOper==2){
        Top5OperationsLabel=['PCB INSPECTION', 'ASSEMBLY','AGING','C INSPECTION','C KEN 2','D INSPECTION', 'Total']
        Top5OperationsProps=[1300,1400,1500,1600,1630,1700,'Total']
        Operations=[1300,1400,1500,1600,1630,1700]

    }else if(values.SpecialOper==3){
        Top5OperationsLabel=['PCB INSPECTION', 'ASSEMBLY','ASSEMBRW','AGING','C INSPECTION','D INSPECTION', 'Total']
        Top5OperationsProps=[1300,1400,1410,1500,1600,1700,'Total']
        Operations=[1300,1400,1410,1500,1600,1700]

    }else if(values.SpecialOper==4){
        Top5OperationsLabel=['PCB INSPECTION', 'ASSEMBLY','ASSEMBLY-1430','AGING','C INSPECTION','D INSPECTION', 'Total']
        Top5OperationsProps=[1300,1400,1430,1500,1600,1700,'Total']
        Operations=[1300,1400,1430,1500,1600,1700]

    }else{
        Top5OperationsLabel=['PCB INSPECTION', 'ASSEMBLY','AGING','C 1590','C INSPECTION','D INSPECTION', 'Total']
        Top5OperationsProps=[1300,1400,1500,1590,1600,1700,'Total']
        Operations=[1300,1400,1500,1590,1600,1700]
    }

   
    TopTableParams =  { name: "Date",apiName: "Top5DfctDataSet", FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };

    TopTableParams = { ...TopTableParams, ...values }    
   
    try {
            const { Date: {
                Data: { Table: Data },//main data every prod
                Data_2: { Table: Data_2 },// all prod  for bottom 匯總數據
                Data_3: { Table: Data_3 },// every prod defect
                Data_4: { Table: Data_4 },// all prod  defect for bottom 匯總數據
                SQL01,
                SQL02,
                SQL03,
                SQL04,
                sql_Date,
                rework,
                StrSql_w1,
                StrSql_wf,

            } } = await GetCommon(TopTableParams);

            PageSetting.SQLHidden = "Top5DfctDataSet-SQLHidden SQL01:"+SQL01+"; SQL02:"+SQL02+"; SQL03:"+SQL03+"; SQL04:"+SQL04
            
             Message = {                
                sql_Date:sql_Date,
                rework:rework,
                StrSql_w1:StrSql_w1,
                StrSql_wf:StrSql_wf,
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
        //   console.log("sQL01:"+SQL01)
        //   console.log("sQL02:"+SQL02)
        //   console.log("sQL03:"+SQL03)
        //   console.log("sQL04:"+SQL04)


            if (Data.length == 0 && Data_2.length == 0) {
                closeFullLoading();
                return message.warn("无数据！")
            }

            PageSetting.Title=`${useCommonParams.Fab} Top 5 Defect Code Report`

            ReportItem.APIMessage = JSON.stringify(Message);  
            ReportItem.FormParams=TopTableParams;

            InsertColumns = {};
            Top5OperationsLabel.forEach((item: string, index: number) => {                
                  InsertColumns[Top5OperationsProps[index]] = item;
                  InsertColumns[`${Top5OperationsProps[index]}-DFCT_CODE`] = item;
                  InsertColumns[`Title`] = item;
              
            });
           
            
            PageSetting.moreData=handledata(Data,Data_2,Data_3,Data_4)
            PageSetting.List = PageSetting.moreData;
            PageSetting.Columns = Top5DefectCodeColumns;
         
        // if (PageSetting.moreData.length > 100) {
        //     //预先加载40条数据
        //     PageSetting.List = PageSetting.moreData.slice(0, 30);
        // }
        // else {
        //     PageSetting.List = PageSetting.moreData;
        // }

        PageSetting.x = null;
        if (PageSetting.Columns.length > 7) {
            PageSetting.x = 90 * PageSetting.Columns.length;
        }
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }

}

function handledata(Data: any,Data_2: any,Data_3: any,Data_4: any){     
       
        let NewOperations: any[] = [];
        let PROD: any[] = [];
        let ListData: any[] = [];
        let SelectProd: any[] = [];
        let ApiData_2 = cloneDeep([ ...Data_2,]);

        //右側by 料號 total
        Data_3.forEach((item) => {      
        item['WORK_CTR'] = 'Total';
       });

       //ALL prod 的top5
       ApiData_2.forEach((item) => {
           item['PROD'] = 'ALL';           
        });

        Data_4.forEach((item) => {
            //右下角總的total
            item['PROD'] = 'ALL';
            item['WORK_CTR'] = 'Total';
        });


        let ApiData = cloneDeep([        
        ...Data,//every prod top5
        ...ApiData_2,//ALL prod 的top5
        ...Data_3, //every prod 右側total
        ...Data_4, //ALL prod 右側total
        ]);


        Data_3.forEach((item) => {
            if (!SelectProd.includes(item['PROD'])) SelectProd.push(item['PROD']);
        });
        

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
    
       //all prod sum
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
       

      let AllData = [
        ...Data,
        ...Data_2,
    ];
   
      //按defect 對應機種過濾 AllData中無defect的機種站點
      let SelectAllData: any[] = [];
      SelectProd.push("ALL");
      for(let i=0; i <AllData.length-1; i++){
        if (Operations.includes(AllData[i]['WORK_CTR'])&& SelectProd.includes(AllData[i]['PROD'])){
            if (!PROD.includes(AllData[i]['PROD']) && AllData[i]['PROD']) PROD.push(AllData[i]['PROD']);
            if (!NewOperations.includes(AllData[i]['WORK_CTR'])) NewOperations.push(AllData[i]['WORK_CTR']);
            SelectAllData.push(AllData[i]);
        } 

      }
   
    //Ins-Qty by站點，料號轉置
    let config: PovitParams = {
        Data: SelectAllData,
        povitColumn: {
            WORK_CTR: 'OUT_QTY'
        },
        changeColumn: NewOperations,
        repeatColumn: repeatData
    };
    //Yield-Rate by站點，料號轉置
    let config1: PovitParams = {
        Data: SelectAllData,
        povitColumn: {
            WORK_CTR: 'OutYield'
        },
        changeColumn: NewOperations,
        repeatColumn: repeatData
    };
     //Yield-Loss: by站點，料號轉置
    let config2: PovitParams = {
        Data: SelectAllData,
        povitColumn: {
            WORK_CTR: 'Yield'
        },
        changeColumn: NewOperations,
        repeatColumn: repeatData
    };

    let povitData = getPovitData(unref(config));
    let povitData1 = getPovitData(unref(config1));
    let povitData2 = getPovitData(unref(config2));
    //TOP5計算
    let povitData3 = EasyPovit(ApiData, NewOperations, SelectProd);
    console.log("povitData2",povitData2)
    console.log("povitData1",povitData1)

    try {

        //Ins-Qty 加總
        povitData.forEach((item, index) => {
            debugger;
            let TotalOutQty: number = 0,
                AllDefectQTY: number = 0;
            let AllYeild:number = 1;
            // item['Total'] = 0;
            NewOperations.forEach((Oper) => {
                if (Oper) {
                    debugger;
                    let QtyData = povitData2[index][Oper]
                        ? povitData2[index][Oper].split('%')[1].replace(/[^0-9]/g, '')
                        : 0;                   
                    let Qty = isNaN(QtyData) ? 0 : parseInt(QtyData);
                    let TotalOutQtyData = isNaN(item[Oper]) ? 0 : item[Oper];
                    TotalOutQty += TotalOutQtyData;
                    AllDefectQTY += Qty;
                    AllYeild=Number(povitData1[index][Oper]?povitData1[index][Oper].replace('%', ''):100)/100*AllYeild               
                    if(povitData1[index][Oper]=="NaN%"){
                        povitData1[index][Oper]=''
                    }
                }
            });

            //LCM1 1500,1700 站點只顯示ALL的數據
            if(useCommonParams.Fab=="LCM1"){
            if(item['PROD']!=="ALL"){
                item['1700']=""
                item['1500']=""
            }

                if(povitData1[index]['PROD']!=="ALL"){
                    povitData1[index]['1700']=""
                    item['1500']=""
                }

                if(povitData2[index]['PROD']!=="ALL"){
                    povitData2[index]['1700']=""
                    item['1500']=""
                }
            }
            
            
            item['PROD1']="Ins-Qty:"
            //item['Total'] = TotalOutQty;        
            povitData1[index]['Total'] =(AllYeild*100).toFixed(2) + '%'
            if(isNaN(AllYeild)){
                povitData1[index]['Total']='100%'
            }else{
                povitData1[index]['Total'] =(AllYeild*100).toFixed(2) + '%'
            }

            povitData2[index]['Total'] =(100-povitData1[index]['Total'].replace('%', '')).toFixed(2) + `%(${AllDefectQTY})`;
            povitData2[index]['PROD1']="Yield-Loss:"
            
            povitData1[index]['PROD1']="Yield-Rate:%"
            povitData1[index]['OutYield']=povitData1[index]['Total']
            
            
            if (index < povitData.length) ListData.push({ PROD1: item['PROD'], ...InsertColumns });

            ListData.push(item);
            ListData.push(povitData1[index]);
            ListData.push(povitData2[index]);


            povitData3.forEach((j) => {
                if (j['PROD'] == item['PROD']) {
                    if(100-povitData1[index]['Total'].replace('%', '')==0){
                        j['Total-DFCT_CODE']="0.00%"+ `%(${j['Total-DFCT_CODE'].split('%')[1].replace(/[^0-9]/g, '')})`;
                    }else{
                        j['Total-DFCT_CODE']=((j['Total-DFCT_CODE'].split('%')[1].replace(/[^0-9]/g, '')/AllDefectQTY)*(100-povitData1[index]['Total'].replace('%', ''))).toFixed(2) + `%(${j['Total-DFCT_CODE'].split('%')[1].replace(/[^0-9]/g, '')})`;
                    
                    }
                ListData.push(j);
                }
            });
        
         });
    } catch (error) {
        debugger;
        console.log("error",error)
    }

    console.log("ListData",ListData)
     return ListData
}

function getRowClassName_Sel(record: Object) {
    if (record && Object.keys(record).length != 0) {
        if (['Total'].includes(record['Title'])) {
            return 'YellowGray';
        }
        // else if([1,2,3,4,5].includes(record['PROD1'])) {
        //     console.log("record",record)
        //     Operations.forEach((Work) => {
        //         console.log("record[`${Work}-DFCT_CODE`]",record[`${Work}-DFCT_CODE`],Work)
        //        if (record[`${Work}-DFCT_CODE`]){
        //         console.log(123)
        //         return 'GrayBlue';
        //        }
        //   });
        // }        
    }
    
}

//top5 資料組合
function EasyPovit(Data: any[], Operations: any[], PROD: any[]) {
    let alaldata: any[] = [];
    Data.forEach((item) => {
        //添加屬性
        let Yield =
            item['QTY'] / item['OUT_QTY'] == Infinity ? '' : ((item['QTY'] * 100) / item['OUT_QTY']).toFixed(2) + '%';
        item[item['WORK_CTR']] = item['ERRC_DESCR'];
        item[`${item['WORK_CTR']}-DFCT_CODE`] = `${Yield}(${item['QTY']})`;
        item[`${item['WORK_CTR']}-DFCT_CODE2`] = item['DFCT_CODE']; //獲取DFCT_CODE for 二階報表傳參數
    });
    Operations.push("Total");

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
                                        router.push({ name: `Top5DefectCodeDetail-${useCommonParams.Fab}` });  
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
                                        router.push({ name: `Top5DefectCodeDetail-${useCommonParams.Fab}` });  
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
InfiniteScroll(Top5DefectCodeTable, PageSetting);
onMounted(async () => {
    // if (bigfab==="LCM"){
    //     // await mounted(optionsdata,useCommonParams.Fab)    
    //     // await mountedSourceFab(optionsdata, useCommonParams.Fab);
    //     // await mountedWoTypeBySap(optionsdata, useCommonParams.Fab);
    //     // await mountedLCMowner(optionsdata, useCommonParams.Fab);
    //     //await mountedLCMDefectCode(optionsdata, useCommonParams.Fab);
    //     //await mountedLCDGrade(optionsdata, useCommonParams.Fab);
    //     //await mountedModuleData(optionsdata, useCommonParams.Fab);
    // }else{
        
    // }
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
