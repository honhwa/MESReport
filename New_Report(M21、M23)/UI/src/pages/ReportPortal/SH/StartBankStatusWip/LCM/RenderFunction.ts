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

// import { Object } from 'lodash';
const RightTopPageShow= ref(false);
const RightBottomPageShow = ref(false);


let secondTableParams = {};
let [formParams, APIMessage, firstTableParams, thirdTableParams] = [{}, {}, {}, {}]
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});



export function getStartBankStatusWipDetail(tableParams,formParams,PageSetting){      
    openFullLoading()
    getData(tableParams,formParams,PageSetting);

};

async function getData(tableParams,formParams,PageSetting){    
    const ParamsData: any = {
    name: "Date",
    apiName: "StartBankStatusDetail",
    FactoryType: formParams["FactoryType"],
    G_FAC: formParams["G_FAC"],
    ToDate:formParams["ToDate"],
    WorkCtr:tableParams["title"].replace('*', '').slice(5),
    Lcd_Prod_nbr:tableParams["LCD_PROD"],
    LcdGrade:tableParams["GRADE"],
    LcdOwner:tableParams["title"].slice(0,4),
   };


  try{
       const { Date: { Data: { Table: Data },sql01 } } = await GetCommon(ParamsData); 
       //console.log("StartBankStatusDetail-SQLHidden:",sql01)
       PageSetting.SQLHidden="StartBankStatusDetail-SQLHidden:"+sql01; 
       
       if (Data.length != 0) {
            let ColumsData: any = [];
            let WidthPars=50;
            let classNamePars="";

            for (let item in Data[0]) { 
                ColumsData.push({ title: item, align: 'center', dataIndex: item, className: classNamePars, width:WidthPars});
                
            }
        
            PageSetting.Columns=ColumsData;
            //PageSetting.List = Data;
            PageSetting.moreData= Data;
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


