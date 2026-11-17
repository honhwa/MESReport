// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { getfinalsum, Inserttotalrow, dclick,mergeTableRow,getSumData } from '@/pages/ReportPortal/utils/SHcommon';
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
    const { detail1,detail2
    } = require(`@/pages/ReportPortal/SH/EquipmentUtilization/${bigfab}/table`);    
    return { detail1,detail2};
}

let {detail1,detail2} = getImport();


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
    let ParamsData = {
            name: "Date",
            apiName:"DownGradeTop5Detail",
            FactoryType:formParams["FactoryType"],
            G_FAC: formParams["G_FAC"],

            prod_nbr:tableParams["PROD"]=="ALL" ?"Total":tableParams["PROD"],
            defect_code:tableParams[tableParams["dataIndex"].replace('-DFCT_CODE', '-DFCT_CODE2')],
            main_wc:tableParams["dataIndex"].replace('-DFCT_CODE', ''),
        
            sql_Transdate:APIMessage['sql_Transdate'],
            sql_dgdfct_where:APIMessage["sql_dgdfct_where"],
            sql_SpecialOper:APIMessage["sql_SpecialOper"],
            lstr_ProdType:APIMessage["lstr_ProdType"],
        };
   

        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
        console.log("DownGradeTop5Detail-SQLHidden:",SQL01)   
        PageSetting.SQLHidden = "DownGradeTop5Detail-SQLHidden:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }

    let sumdate=getSumData(Data.length, 0, Data, ['PASS_QTY',]);
  
    PageSetting.FirstColumns =detail1;
    let { insertIndexList, insertrowList } = Inserttotalrow(Data, ["PROD_NBR"],['PASS_QTY',], "PROD_NBR"); 
    Data.splice( Data.length, 0, Object.assign(sumdate, { PROD_NBR: '總合計' }));

    PageSetting.FirstList =Data;
    tempdate=Data;
    PageSetting.y= null
    PageSetting.y = 15 * (Data.length+6);
    if(PageSetting.y>450){
        PageSetting.y=450;
    }

    
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
        apiName: "DownGradeTop5Detail2",
        FactoryType: NewFactoryType,
        G_FAC: NewG_FAC,
        entity_no:strENTITY_NO,
       };

    const { Date: { Data: { Table: Data },SQL01 } } = await  GetCommon(ParamsData2);
    NewPageSetting.SQLHidden2 = "DownGradeTop5Detail2-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }
    NewPageSetting.SecondColumns = detail2;
    NewPageSetting.SecondList=Data;
     //加載防護罩結束
     closeFullLoading();
}


