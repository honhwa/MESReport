
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';



// let tableParams = useOperation.FirstTableParams;
let secondTableParams = {};
let [formParams, APIMessage] = [{}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});



export function getPanelWipStayDetail(tableParams,APIMessage,formParams,PageSetting,PanelWipDetailColumns){      
    openFullLoading()
    getData(tableParams,APIMessage,formParams,PageSetting,PanelWipDetailColumns);

};

async function getData(tableParams,APIMessage,formParams,PageSetting,PanelWipDetailColumns) {
    //openFullLoading(); 
    
    const ParamsData: any = {
       name: "Date",
       apiName: 'PanelWipStayNewDetail',
       FactoryType: formParams["FactoryType"],
       G_FAC: formParams["G_FAC"],
       Queryby:formParams["Queryby"],

       Sqlwhere:APIMessage["sql_Where"],
       Prod_nbr:tableParams["PROD_NBR"]|| '',
       Work_ctr:tableParams["WORK_CTR"]|| '',
       Dates:tableParams["title"].replace('*', ''),
   };

   try{
      const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
      //console.log("PanelWipStayDetail-SQLHidden:",SQL01)

      PageSetting.SQLHidden="PanelWipStayDetail-SQLHidden:"+SQL01;
       
       PageSetting.Title=`${formParams["FactoryType"]} Wip Panel Detail (Real-Time Refresh)`
       if (Data.length != 0) { 
           PageSetting.Columns = PanelWipDetailColumns;
           PageSetting.moreData = Data;
           PageSetting.x = null;
            if (PageSetting.Columns.length > 7) {
                PageSetting.x = 120 * PageSetting.Columns.length;
            } 
       }else{
            message.warning("無數據！")
       }
       closeFullLoading();

   }catch(error: any){ 
       closeFullLoading();
       return Promise.reject(error); 
   } 

}
