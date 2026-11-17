// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { getfinalsum, Inserttotalrow, dclick,mergeTableRow } from '@/pages/ReportPortal/utils/NGBcommon';
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

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});



export function getDetail(tableParams,APIMessage,formParams,PageSetting,detail1_1){
    openFullLoading(); 
    NewPageSetting=PageSetting;
    NewFactoryType=formParams["FactoryType"];
    NewG_FAC=formParams["G_FAC"];
    GetFirstData(tableParams,APIMessage,formParams,PageSetting,detail1_1);
};


async function GetFirstData(tableParams,APIMessage,formParams,PageSetting,detail1_1) {
    PageSetting.FirstColumns = [];
    PageSetting.SecondColumns = [];
    PageSetting.ThirdList = [];
    let ParamsData: any;
    let ApiNamePara="";

    if (formParams["ShowMode"]!="operators"){
        ApiNamePara="DGQualityDetail"
    }else{
        ApiNamePara="DGQualityDetail2"
    }

    ParamsData = {
        name: "Date",
        apiName:ApiNamePara,
        FactoryType: formParams["FactoryType"],
        G_FAC: formParams["G_FAC"],

        FromDate:APIMessage["FromDate"],
        ToDate:APIMessage["ToDate"],
        ShowMode:APIMessage["ShowMode"],
        StrSql_w1:APIMessage["StrSql_w1"],
        sql_LcmGrade:APIMessage["sql_LcmGrade"],

        sel_prod:tableParams["title"].replace('*', ''),
        sel_operator:tableParams["title"].replace('*', ''),
        sel_dfctcode:tableParams['DFCT_CODE'],
        
    };

    const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData);   
    PageSetting.SQLHidden =ApiNamePara+"-SQLHidden SQL01:"+SQL01

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }
    PageSetting.FirstColumns = detail1_1
    PageSetting.FirstList=Data
    
    
    //加載防護罩結束
    closeFullLoading();

}




