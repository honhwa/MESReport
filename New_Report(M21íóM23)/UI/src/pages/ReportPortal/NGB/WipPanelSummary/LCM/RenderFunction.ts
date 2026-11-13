
// import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { Console } from 'console';
import { useLoading } from '@/components/feat/Loading';
import { ref} from 'vue';
import { GetCommon } from '@/api/pages/common';
import { message } from 'ant-design-vue';


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

function getImport() {
    const DetailColumsData = require(`@/pages/ReportPortal/NGB/WipPanelSummary/LCM/Table`);
    return DetailColumsData
    // return { OperDetail3Columns, WipColumns,WipDetailCoulmns };
}
let DetailColumsData = getImport();

export function getWipPanelSummaryDetail(tableParams,formParams,APIMessage,PageSetting){      
    openFullLoading()
    getData(tableParams,formParams,APIMessage,PageSetting);

};

async function getData(tableParams,formParams,APIMessage,PageSetting){     
   
    const ParamsData: any = {
    name: "Date",
    apiName: "WipPanelSumDetail",
    FactoryType: formParams["FactoryType"],
    G_FAC: formParams["G_FAC"],
    Colname:formParams["Colname"],

    sql_StartHour:APIMessage['sql_StartHour'],
    sql_ToHour:APIMessage["sql_ToHour"],
    sql_Wo_nbr:APIMessage["sql_Wo_nbr"],
    sql_Floor:APIMessage["sql_Floor"],
    sql_IsHold:APIMessage["sql_IsHold"],
    sql_Prod_nbr:APIMessage["sql_Prod_nbr"],
    sql_Operation:APIMessage["sql_Operation"],
    sql_HoldCode:APIMessage["sql_HoldCode"], 
    
    sel_Prod_nbr:tableParams["PROD_NBR"]|| '',
    sel_Operation:tableParams["WORK_CTR"]|| '',
    sel_Owner:tableParams["LCM_OWNER"]|| '',
    sel_HoldFlag:tableParams["HOLD_FLAG"]|| '',
    sel_HoldCode:tableParams["HOLD_CODE"]|| '',
    sel_Size:tableParams["PANEL_SIZE"]|| '',
    sel_Wo_nbr:tableParams["WO_NBR"]|| '',
    sel_DueDate:tableParams["DUE_DT"]|| '',
    sel_LcdProduct:tableParams["LCD_PROD_NBR"]|| '',
    sel_LcdGrade:tableParams["LCD_GRADE"]|| '',
    sel_LcmGrade:tableParams["LCM_GRADE"]|| '',
    
   };

  
  try{
       const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
       //console.log("WipPanelSumDetail-SQLHidden:",SQL01)
        PageSetting.SQLHidden="WipPanelSumDetail-SQLHidden:"+SQL01;
    
       if (Data.length != 0) {
        
            PageSetting.Columns = DetailColumsData.WipPanSumDetailCol;
            PageSetting.moreData = Data;
            PageSetting.List =PageSetting.moreData;
            PageSetting.x = null;
            if (PageSetting.Columns.length > 7) {
                PageSetting.x = 110 * PageSetting.Columns.length;
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

