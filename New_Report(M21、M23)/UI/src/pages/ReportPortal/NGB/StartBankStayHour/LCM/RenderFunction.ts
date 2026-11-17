import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';

// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { watch, reactive, onMounted, watchEffect ,computed} from 'vue';
import { useVirtualList } from '@vueuse/core';
import { message } from 'ant-design-vue';
import { getRowClassName, mergeTableRow,megercell } from '@/pages/ReportPortal/utils/NGBcommon';


let [formParams, APIMessage, firstTableParams,] = [{}, {}, {}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// let useCommonParams = AddCommonPageParams();
// let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

// function getImport(site: any) {    
   
//     return { SecondLabel};
// }
// let { SecondLabel} = getImport("NGB");


export function getStartBankDetail(tableParams,formParams,PageSetting,SecondLabel){      
    openFullLoading()
    getData(tableParams,formParams,PageSetting,SecondLabel);

};

async function getData(tableParams,formParams,PageSetting,SecondLabel){    
    const ParamsData: any = {
    name: "Date",
    apiName: "StartBankDetail",
    FactoryType: formParams["FactoryType"],
    G_FAC: formParams["G_FAC"],
    StayHours:tableParams["STAY_HOUR"],
    CstId:tableParams["CST_ID"],
    Oper:tableParams["OPER"],
   };


  try{
       const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
       //console.log("StartBankStatusDetail-SQLHidden:",sql01)
       PageSetting.SQLHidden="StartBankDetail-SQLHidden:"+SQL01; 
       
       if (Data.length != 0) {
        
            PageSetting.Columns=SecondLabel;
            //PageSetting.List = Data;
            let combineCol1: any = {
                data: Data,
                mergeColNames: ['FAC_ID','FLOOR','CST_ID'],
                firstMergeColNames: ['FAC_ID','FLOOR'],
                firstMerge: 'FAC_ID'
            };
            /**进行表格合并 */
            let Mergedata = mergeTableRow(combineCol1);
            PageSetting.moreData = Mergedata;  
      }else{

        message.warning("無數據！")
      }
        //PageSetting.moreData= Data;
        closeFullLoading();

    }catch(error: any){ 
        closeFullLoading();
        return Promise.reject(error); 
    }
}


