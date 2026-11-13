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



export function GetDetailData(tableParams,APIMessage,formParams,PageSetting){
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
            apiName:"EntityStaSumnewDetail",
            FactoryType:formParams["FactoryType"],
            G_FAC: formParams["G_FAC"],

            Equipment:tableParams["EQUIP_NBR"],
        
            lstr_FromDate:APIMessage['lstr_FromDate'],
            lstr_ToDate:APIMessage["lstr_ToDate"],
        };
   

        const { Date: { Data: { Table: Data },SQL } } = await GetCommon(ParamsData); 
        PageSetting.SQLHidden = "EntityStaSumnewDetail-SQLHidden:"+SQL

    if (Data.length == 0) {
        //加載防護罩結束
        closeFullLoading();
        return message.warn("無數據！")
    }

    let sumdate=getSumData(Data.length, 0, Data, ['STAY_TIME',]);
    sumdate['STAY_TIME']=sumdate['STAY_TIME'].toFixed(2);   
    Data.splice( Data.length, 0, Object.assign(sumdate, { EQUIP_NBR: '總合計' }));

    PageSetting.List =Data;
    
    //加載防護罩結束
    closeFullLoading();

}




function toFixed(arg0: number) {
    throw new Error('Function not implemented.');
}

