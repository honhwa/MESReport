// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { getfinalsum, Inserttotalrow, dclick,mergeTableRow,getSumData } from '@/pages/ReportPortal/utils/NGBcommon';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';



const useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);
let secondTableParams = {};
let [formParams, APIMessage] = [{}, {}]
let NewPageSetting: any;
let NewFactoryType="";
let NewG_FAC="";
let tempdate: any;

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport() {
    const {        
        Top5SecondColumns,Top5DetailColumns
    } = require(`@/pages/ReportPortal/NGB/Top5DefectCode/${bigfab}/table`);    
    return { Top5SecondColumns, Top5DetailColumns};
}

let {Top5SecondColumns,Top5DetailColumns} = getImport();


export function getDetail(tableParams,APIMessage,formParams,PageSetting){
    openFullLoading(); 
    NewPageSetting=PageSetting;
    NewFactoryType=formParams["FactoryType"];
    NewG_FAC=formParams["G_FAC"];
    GetFirstData(tableParams,APIMessage,formParams,PageSetting);
   
};


async function GetFirstData(tableParams,APIMessage,formParams,PageSetting) {
    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    PageSetting.ThirdList = [];
   
     let  ParamsData = {
            name: "Date",
            apiName:"Top5DfctDetail",
            FactoryType: formParams["FactoryType"],
            G_FAC: formParams["G_FAC"],
            StrSql_wf:APIMessage['StrSql_wf'],
            StrSql_w1:APIMessage["StrSql_w1"],
            sql_Date:APIMessage["sql_Date"],

            rework:APIMessage["rework"],
            ProdType:formParams["ProdType"],
            SpecialOper:formParams["SpecialOper"],
            Prod_nbr:tableParams["PROD"]=="ALL" ?"Total":tableParams["PROD"],
            dfct_code:tableParams[tableParams["dataIndex"].replace('-DFCT_CODE', '-DFCT_CODE2')],
            work_ctr:tableParams["dataIndex"].replace('-DFCT_CODE', ''),
        
            
        };

   const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
   console.log("Top5DfctDetail-SQLHidden:",SQL01)   
   PageSetting.SQLHidden = "Top5DfctDetail-SQLHidden:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }

    // let TopList: any = [];
    // let TopList2: any = [];
    Data.forEach((item) => {
        if (item.OUTPUT_FG != " "){
            item['PASS_QTY2']=item['PASS_QTY']*(-1);
        }else{
            item['PASS_QTY2']=item['PASS_QTY'];
        }
        //TopList.push(item);
    })

   
    // let tempsum = getfinalsum(TopList, ["PASS_QTY2"], "PROD_NBR");
    
    // Inserttotalrow(TopList, ["PROD_NBR"], ["PASS_QTY2"], "PROD_NBR");
    // TopList.push(tempsum);

    // TopList.forEach((item) => {
    //     if (item.PROD_NBR == "合計"||item.PROD_NBR == "總計"){
    //         item['PASS_QTY']=item['PASS_QTY2']
    //     }else{
    //         item['PASS_QTY']=item['PASS_QTY'];
    //     }
    //     TopList2.push(item);
    // })

    // console.log("TopList",TopList)
    // console.log("Data",Data)
    // console.log("TopList2",TopList2)



    let sumdate=getSumData(Data.length, 0, Data, ['PASS_QTY2',]);
  
     PageSetting.FirstColumns =Top5DetailColumns;
    let { insertIndexList, insertrowList } = Inserttotalrow(Data, ["PROD_NBR"],['PASS_QTY2',], "PROD_NBR"); 
    Data.splice( Data.length, 0, Object.assign(sumdate, { PROD_NBR: '總合計' }));

    Data.forEach((item) => {
        if (item.PROD_NBR == "合計"||item.PROD_NBR == "總合計"){
            item['PASS_QTY']=item['PASS_QTY2']
        }else{
            item['PASS_QTY']=item['PASS_QTY'];
        }
    })

   
    
    PageSetting.FirstList =Data;
    tempdate=Data;
    PageSetting.y= null
    PageSetting.y = 15 * (Data.length+6);
    if(PageSetting.y>450){
        PageSetting.y=450;
    }

    // PageSetting.x= null
    // if (PageSetting.FirstColumns.length > 7) {
    //     PageSetting.x = 110 * PageSetting.FirstColumns.length;
    // }

    //加載防護罩結束
    closeFullLoading();

}


export function getDetail2(secondTableParams){
    openFullLoading();    
    GetSecondTable(secondTableParams);
   
};
async function GetSecondTable(secondTableParams) {

    let strENTITY_NO="";
    if(secondTableParams["PROD_NBR"]=="合計"){
       let rownum=secondTableParams["rowindex"]-1
       for(let i=0; i <tempdate.length; i++){
         if(tempdate[i]["PROD_NBR"]==tempdate[rownum]["PROD_NBR"]){
            strENTITY_NO=tempdate[i]["ENTITY_NO"]+","+strENTITY_NO
         }
     }
     if(strENTITY_NO!=""){
        strENTITY_NO=strENTITY_NO.slice(0,strENTITY_NO.length-1)
     }

    }else if(secondTableParams["PROD_NBR"]=="總合計"){
        for(let i=0; i <tempdate.length; i++){
            if(tempdate[i]["ENTITY_NO"]&&tempdate[i]["ENTITY_NO"]!=""){
               strENTITY_NO=tempdate[i]["ENTITY_NO"]+","+strENTITY_NO
            }
        }
        if(strENTITY_NO!=""){
            strENTITY_NO=strENTITY_NO.slice(0,strENTITY_NO.length-1)
         }
    }
    else{
        strENTITY_NO=secondTableParams["ENTITY_NO"]
    }
    //console.log("strENTITY_NO",strENTITY_NO)
    
    const ParamsData2: any = {
        name: "Date",
        apiName: "Top5DfctDetail2",
        FactoryType: NewFactoryType,
        G_FAC: NewG_FAC,
        entity_no:strENTITY_NO,
       };

    const { Date: { Data: { Table: Data },SQL01 } } = await  GetCommon(ParamsData2);
    NewPageSetting.SQLHidden2 = "Top5DfctDetail2-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }
    NewPageSetting.SecondColumns = Top5SecondColumns;
    NewPageSetting.SecondList=Data;
     //加載防護罩結束
     closeFullLoading();
}





