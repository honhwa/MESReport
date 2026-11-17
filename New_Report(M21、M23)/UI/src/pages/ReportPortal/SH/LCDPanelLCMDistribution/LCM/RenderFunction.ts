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


const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});



export function getDetail(tableParams,formParams,APIMessage,PageSetting){      
    openFullLoading()
    getData(tableParams,formParams,APIMessage,PageSetting);

};

async function getData(tableParams,formParams,APIMessageParams,PageSetting){    
    let APIMessage=JSON.parse(APIMessageParams);
    const ParamsData: any = {
    name: "Date",
    apiName: "LCDSTPNDataSet",
    FactoryType: formParams["FactoryType"],
    G_FAC: formParams["G_FAC"],
    FromDate:formParams["FromDate"],
    ToDate:formParams["ToDate"],
    QueryType:formParams["QueryType"],
    Shift:formParams["Shift"],
    Floor:formParams["Floor"],
    AbnormalCode:formParams["AbnormalCode"],
    LcdProduct:formParams["LcdProduct"],
    LcdGrade:formParams["LcdGrade"],
    Prod_nbr:formParams["Prod_nbr"],
    LcmOwner:formParams["LcmOwner"],

    //lstr_AbnormalCode:APIMessage['lstr_AbnormalCode'],
    sql_Date:APIMessage['sql_Date'],
    //sql_Shift:APIMessage['sql_Shift'],
    //sql_Floor:APIMessage['sql_Floor'],
    //sql_LcdProduct:APIMessage['sql_LcdProduct'],
    //sql_Prod_nbr:APIMessage['sql_Prod_nbr'],
    //sql_LcmOwner:APIMessage['sql_LcmOwner'],
    sel_Product:tableParams["PROD_NBR"],
   };


  try{
       const { Date: { Data: { Table: Data },SQL01 } } = await GetCommon(ParamsData); 
       console.log("LCDSTPNDataSet:",SQL01)
       PageSetting.SQLHidden="LCDSTPNDataSet-SQLHidden:"+SQL01; 
       
       if (Data.length != 0) {
            let ColumsData: any = [];
            let WidthPars=80;
            let classNamePars="";
            let titlePars="";

            for (let item in Data[0]) { 
                if(item.slice(0,1)=="A"){
                    titlePars=item.slice(1).replaceAll('_', '-');
                    WidthPars=80;
                }else{
                    titlePars=item;
                    if (item=="LCM_PROD"||item=="LCD_PROD"||item=="LCD_ABN_LCD") {
                        WidthPars=120;  
                    }else{
                        WidthPars=85; 
                    }
                } 
                ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars});
            }
        
            PageSetting.Columns=ColumsData;
            PageSetting.List = Data;
            //PageSetting.moreData= Data;
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


