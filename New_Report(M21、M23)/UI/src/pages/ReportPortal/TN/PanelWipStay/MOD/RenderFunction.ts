
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';



const useCommonParams = AddCommonPageParams();
// let tableParams = useOperation.FirstTableParams;
let secondTableParams = {};
//let [formParams, APIMessage] = [{}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//  function getImport() {
//     const PanelWipDetailColumns = require(`@/pages/ReportPortal/TN/PanelWipStay/MOD/Table`);
//     return PanelWipDetailColumns
//     // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
// }
// let PanelWipDetailColumns = getImport();

// export function getPanelWipStayDetail(tableParams,PageSetting){      
//     openFullLoading()
//     getData(tableParams,PageSetting);

// };

// async function getData(tableParams: any,PageSetting) {
//      openFullLoading();
//      const ParamsData: any = {
//         name: "Date",
//         apiName: 'TN_PanelWipStayDataSetDetail',
//         FactoryType: useCommonParams.Fab,
//         G_FAC: useCommonParams.G_FAC_COM,
        
//         Sqlwhere:APIMessage["sql_Where"],
//         Prod_nbr:tableParams["PROD_NBR"]|| '',
//         Work_ctr:tableParams["WORK_CTR1"]|| '',
//         Dates:tableParams["title"].replace('*', ''),
//         Queryby:formParams["Queryby"],
//     };
    
//     try{
//         const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
//         console.log("PanelWipStayDetail-SQLHidden:",SQL01)
//         PageSetting.Columns = PanelWipDetailColumns;
			
//         PageSetting.moreData = Data;
//          closeFullLoading();

//     }catch(error: any){ 
//         closeFullLoading();
//         return Promise.reject(error); 
//     } 

// }
