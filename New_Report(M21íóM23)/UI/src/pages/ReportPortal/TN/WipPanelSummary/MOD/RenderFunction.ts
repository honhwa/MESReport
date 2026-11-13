
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';



const useCommonParams = AddCommonPageParams();
// let tableParams = useOperation.FirstTableParams;
let secondTableParams = {};
//let [formParams, APIMessage] = [{}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// function getImport() {
//     const DetailColumsData = require(`@/pages/ReportPortal/TN/WipPanelSummary/MOD/Table`);
//     return DetailColumsData
//     // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
// }
// let DetailColumsData = getImport();

// export function getWipPanelSummaryDetail(tableParams,formParams,PageSetting, APIMessage){      
//     openFullLoading()
//     getData(tableParams,formParams,PageSetting, APIMessage);

// };


// async function getData(tableParams,formParams,PageSetting, APIMessage){    
//     const ParamsData: any = {
//     name: "Date",
//     apiName: "TN_WipPanelSumDataSetDetail",
//     FactoryType: useCommonParams.Fab,
//     G_FAC: useCommonParams.G_FAC_COM,
//     OverHours:formParams["StartHour"],
//     SelectColumn:formParams["Colname"],
//     Sqlwhere:APIMessage["sql_Where"],
    
//     ProdNbr:tableParams["PROD_NBR"],
//     Owner:tableParams["LCM_OWNER"],
//     Operation:tableParams["WORK_CTR"],
//     HoldFlag:tableParams["HOLD_FLAG"],
//     HoldCode:tableParams["HOLD_CODE"],
//     Size:tableParams["PANEL_SIZE"],
//     WoNbr:tableParams["WO_NBR"],
//     LcdProduct:tableParams["LCD_PROD_NBR"],
//     LcdGrade:tableParams["LCD_GRADE"],
//     MvinFlag:tableParams["MVIN_FLAG"],
//     LcmGrade:tableParams["LCM_GRADE"],
//     QTime:tableParams["MIN_QRS_DT"],
//     ERP:tableParams["ERP_ORDER_TYPE"],
//    };

//    //console.log("APIMessage2",APIMessage["sql_StartHour"],APIMessage)

//   try{
//        const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
//        console.log("WipPanelSumDetail-SQLHidden:",SQL01)

//         if (Data.length != 0) {
//         let ColumsData: any = [];
//         let WidthPars=50;
//         let classNamePars="pro_auto-center";
        

//         PageSetting.Columns = DetailColumsData.WipPanSumDetailCol;
//         PageSetting.moreData = Data;
//         }else{

//                  message.warning("無數據！")
//          }

//         closeFullLoading();

//     }catch(error: any){ 
//         closeFullLoading();
//         return Promise.reject(error); 
//     }
// }

