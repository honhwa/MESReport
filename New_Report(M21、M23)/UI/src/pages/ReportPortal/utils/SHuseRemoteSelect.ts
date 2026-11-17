/*
 * @Description: echarts 自适应问题
 */
import { reactive, Ref } from 'vue'
import { GetCommon } from '@/api/pages/common';
import { useInfiniteScroll } from '@vueuse/core';
import { RouteLocationNormalizedLoaded } from 'vue-router';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { number } from '@intlify/core-base';



const UseCommonParams = AddCommonPageParamsWithOut()
export default function () {

    //公用獲取API數據的方法1
    async function getCommonFormParams(extractparams: string, apiparams: Object) {
        let ReturnData: any = [];
        // let ApiData = await GetCommon(apiparams);
        // let Data = ApiData.Date.Data.Table ?? ApiData.api01.Data.Table
        // let SQLHidden=ApiData.Date.Data.sql ?? ApiData.api01.Data.sql

        let { Date: { Data: { Table: Data},sql } }=await GetCommon(apiparams);
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][extractparams], value: Data[i][extractparams] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' })

       //console.log(apiparams["apiName"]+"-common-SQLHidden:",sql)
        return ReturnData
    }

    // 公用獲取API數據的方法2 (含all)
    async function getCommonFormData(params: String, funcparams: String, FactoryType: String) {
        let ReturnData: object[] = [];
        let { Date: { Data: { Table: Data},sql } } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: "Date",
            apiName: funcparams,
            FactoryType: FactoryType,
        });
        
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params], value: Data[i][params] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' })
        //console.log(funcparams+"-common-SQLHidden:",sql)
        return ReturnData
    }

    // 公用獲取API數據的方法3 (不含all)
    async function getCommonFormDataNoAll(params: String, funcparams: String, FactoryType: String) {
        let ReturnData: object[] = [];
        let { Date: { Data: { Table: Data },sql } } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: "Date",
            apiName: funcparams,
            FactoryType: FactoryType,
        });
        
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params], value: Data[i][params] });
        }
        //console.log(funcparams+"-common-SQLHidden:",sql)
        return ReturnData
    }

       //公用獲取API數據的方法4 (不含all)
       async function getCommonFormParamsNoAll(extractparams: string, apiparams: Object) {
        let ReturnData: any = [];
        // let ApiData = await GetCommon(apiparams);
        // let Data = ApiData.Date.Data.Table ?? ApiData.api01.Data.Table
        // let SQLHidden=ApiData.Date.Data.sql ?? ApiData.api01.Data.sql

        let { Date: { Data: { Table: Data},sql } }=await GetCommon(apiparams);
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][extractparams], value: Data[i][extractparams] });
        }
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' })

       //console.log(apiparams["apiName"]+"-common-SQLHidden:",sql)
        return ReturnData
    }


  /////////***************LCM 左側Query 部分公用數據************↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓************************* */
    async function getOper(params: String, funcparams: String, FactoryType) {
        let ReturnData: any = [];
        let { Date: { Data: { Table: Data },sql } } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: "Date",
            apiName: funcparams,
            FactoryType: FactoryType,
        });

       
        for (let i in Data) {
            //@ts-ignore
            // let a=Data[i]["params"].split('-')[0];

            ReturnData.push({ label: Data[i][params], value: Data[i][params].split('-')[0]});
        }
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' })
        //console.log(funcparams+"-common-SQLHidden:",sql)
        return ReturnData

    }
   
    async function getLcmProdType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let { Date: { Data: { Table: Data } } } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: "Date",
            apiName: funcparams,
            FactoryType: FactoryType,

        });
        for (let i in Data) {
            //@ts-ignore
            if (i == 0) {
                ReturnData.push({ label: "Normal", value: "Normal" });
            } else {
                ReturnData.push({ label: Data[i]["SOPTION"], value: Data[i]["SOPTION"] });
            }
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' })
        return ReturnData;
    }

    //PROD SIZE 查詢
    const mounted = async (optionsdata, FactoryType) => {
        //optionsdata.SizeData = await getCommonFormDataNoAll('SIZE1', 'Size', FactoryType);
        //optionsdata.ProductData1 = optionsdata.ProductData = await getCommonFormData('SOPTION', 'Product', FactoryType);
        optionsdata.BigProductData1 = optionsdata.BigProductData = await getCommonFormData('PART', 'BigProduct', FactoryType);
        return optionsdata
    }

    //LCMowner 查詢
    const mountedLCMowner = async (optionsdata, FactoryType, optionsdataprop: string = "OwnerName") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('LCMOWNER', 'LcmOwner', FactoryType);        
    }

    //LCDowner 查詢
    const mountedLCDowner = async (optionsdata, FactoryType, optionsdataprop: string = "LcdOwnerLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('LCDOWNER', 'LcdOwner', FactoryType);        
    }

    const mountedModuleData = async (optionsdata, FactoryType, optionsdataprop: string = "ModuleData") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('PROD_TYPE', 'Module', FactoryType);        
    }
     //Customer 查詢
    const mountedCustomer = async (optionsdata, FactoryType, optionsdataprop: string = "CustomData") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('CUSTOMER', 'Customer', FactoryType);        
    }
    //oper 查詢
    const mountedOper = async (optionsdata, FactoryType, optionsdataprop: string = "OperationLabel") => {
        optionsdata[optionsdataprop] = await getOper('OPER', 'Operation',FactoryType);
    }
    //Hold Code 查詢
    const mountedHoldCode = async (optionsdata, FactoryType, optionsdataprop: string = "HoldCodeLabel") => {
        optionsdata[optionsdataprop] = await getOper('HOLD_CODE', 'HoldCode', FactoryType);        
    }
    //LCM PROD TYPE 查詢
    const mountedLcmProdType = async (optionsdata: object, FactoryType: string, optionsdataprop: string = "LcmProdTypeData") => {
        optionsdata[optionsdataprop] = await getLcmProdType("LCMProductType", FactoryType);
    }

    //LCDGrade 查詢
    const mountedLCDGrade = async  (optionsdata, FactoryType, optionsdataprop: string = "LcdGradeLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('SGRADE', 'LcdGrade', FactoryType); 
    }

    //LCMGrade 查詢
    const mountedLCMGrade = async  (optionsdata, FactoryType, optionsdataprop: string = "LcmGradeLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('LCM_GRADE', 'LcmGrade', FactoryType); 
    }

     //SourceFab 查詢
     const mountedSourceFab = async (optionsdata, FactoryType, optionsdataprop: string = "SourceFabLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('SOURCEFAB', 'SourceFab', FactoryType);       
    }

    //WoTypeBySap 工單屬性查詢
    const mountedWoTypeBySap = async (optionsdata, FactoryType, optionsdataprop: string = "WoTypeBySapLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('SAP_TYPE_GROUP', 'WoTypeBySap', FactoryType);        
    }
     
    //LCMBigEquipment 查詢
    const mountedLCMBigEquipment = async (optionsdata, FactoryType,optionsdataprop: string = "LCMBigEquipmentLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('CODE', 'BigEquipment', FactoryType);
    }  
    
    
     //LCM OEEBigEquipment 查詢
     const mountedLCMOEEBigEquipment = async (optionsdata, FactoryType,optionsdataprop: string = "LCMBigEquipmentLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('CODE', 'OEEBigEquipment', FactoryType);
    } 

    //Equipment 查詢
    const mountedEquipment = async (optionsdata, FactoryType,optionsdataprop: string = "EquipmentLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('Equipment', 'Equipment', FactoryType);
    }  

    //Defect Code 查詢
    const mountedLCMDefectCode = async (optionsdata, FactoryType,optionsdataprop: string = "LCMDefectCodeLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('ERRC_NBR', 'DefectCode', FactoryType);
    } 

    //Defect Code 查詢
    const mountedDefectCode = async (optionsdata, FactoryType,optionsdataprop: string = "DefectCodeLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('DefectCode', 'DefectCode', FactoryType);
    } 

    //lcm edc EDCEquipment
    const mountedEDCEquipment = async (optionsdata, FactoryType,optionsdataprop: string = "LCMEDCEquipmentLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormParamsNoAll('EQP_ID', {
            name: "Date",
            apiName: "EDCEquipment",
            FactoryType: FactoryType,
            G_FAC: UseCommonParams.G_FAC_COM,
            Choose:"EDC",
          }) ;
    } 


    //LcdSize 查詢
    const mountedLcdSize = async (optionsdata, FactoryType,optionsdataprop: string = "LcdSizeLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('SIZE1', 'LcdSize', FactoryType);
    } 

    
   ///////////////**********************************************↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ */ 
    /////////////// /////////////// //////*********/ SPC/*********/////////////// /////////////// /////////////// /////////////// 

      //SPC EQPTID 查詢
      const mountedEqptId = async (optionsdata, FactoryType, optionsdataprop: string = "SpcEqptIdLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('EQPT_ID', 'SpcEqptId',FactoryType);
    }

    //SPC PROD 查詢
    const mountedSpcProd = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProdLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROD', 'SpcProd',FactoryType);
    }
    
    //SPC DATA GROUP 查詢
    const mountedDataGroup = async (optionsdata, FactoryType, optionsdataprop: string = "SpcGroupLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('DATA_GROUP', 'SpcGroup',FactoryType);
    }

    //SPC Module Name 查詢
    const mountedSpcModuleName = async (optionsdata, FactoryType, optionsdataprop: string = "SpcModuleNameLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('MODULE_NAME', 'SpcModuleName',FactoryType);
    }

    //SPC Meas Tool 查詢
    const mountedSpcMeasTool = async (optionsdata, FactoryType, optionsdataprop: string = "SpcMeasToolLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('EQPT_ID', 'SpcMeasTool',FactoryType);
    }

    //SPC Proc Tool 查詢
    const mountedSpcProcTool = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProcToolLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROC_ID', 'SpcProcTool',FactoryType);
    }

    //SPC Line 查詢
    const mountedSpcLine = async (optionsdata, FactoryType, optionsdataprop: string = "SpcLineLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('LINE', 'SpcLine',FactoryType);
    }

    //SPC Before Hours 查詢
    const mountedSpcBeforeHours = async (optionsdata, FactoryType, optionsdataprop: string = "SpcBeforeHoursLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('HOUR', 'SpcBeforeHours',FactoryType);
    }

    const mountedSpcProdNum = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProdNumLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROD_NBR', 'SpcProdNum',FactoryType);
    }
   
    const mountedSpcPreLine = async (optionsdata, FactoryType, optionsdataprop: string = "SpcPreLineLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('EQPT_ID', 'SpcPreLine',FactoryType);
    }
    
    const mountedSpcProdID = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProdIDLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROD_ID', 'SpcProdID',FactoryType);
    }
    //SPC Cpk Performance Report
    const mountedSpcOper = async (optionsdata, FactoryType, optionsdataprop: string = "SpcOperLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('OPER', 'SpcOper',FactoryType);
    }
    
    const mountedSpcProduct = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProductLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROD', 'SpcProduct',FactoryType);
    }

    const mountedSpcProcess = async (optionsdata, FactoryType, optionsdataprop: string = "SpcProcessLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('PROCESS', 'SpcProcess',FactoryType);
    }

    const mountedSpcPerLine = async (optionsdata, FactoryType, optionsdataprop: string = "SpcPerLineLabel") => {
        optionsdata[optionsdataprop] = await getCommonFormData('LINE', 'SpcPerLine',FactoryType);
    }


    //MOD 站點2900默認選擇
    const mountedOperationForINS = async (optionsdata, FactoryType, optionsdataprop: string = "OperationForINS") => {
        optionsdata[optionsdataprop] = await getCommonFormDataNoAll('OPERATIONFORINS', 'OperationForINS', FactoryType);        
    }
    // MOD 站點
    const mountedOperationForRWK = async (optionsdata, FactoryType, optionsdataprop: string = "OperationForRWK") => {
        optionsdata[optionsdataprop] = await getOper('OPER', 'OperationForRWK',FactoryType);
    }

    /**
     * 
     * @param dom 設定的ref節點
     * @param PageSetting 全部數據
     * @distance 距離底部的距離
     */
    function InfiniteScroll(dom: Ref<HTMLElement | null>, PageSetting: BasePageParams.BasePageSetting) {
        useInfiniteScroll(
            dom,
            () => {
                // load more
                if (PageSetting.moreData.length > 100) {
                    let start = PageSetting.List.length
                    let end = PageSetting.List.length + 20
                    let newData = PageSetting.moreData.slice(start, end)
                    PageSetting.List.push(...newData)
                }
            },
            { distance: 10 }
        )
    }


    return {
        /////***************LCM  部分******************/////
        mounted,
        getCommonFormData,
        getCommonFormDataNoAll,
        InfiniteScroll,
        mountedLCMowner,
        mountedOper,
        getCommonFormParams,
        getCommonFormParamsNoAll,
        mountedLCMBigEquipment,
        mountedLcmProdType,
        mountedLCDGrade,
        mountedCustomer,
        mountedLCDowner,
        mountedModuleData,
        mountedHoldCode,
        mountedSourceFab,
        mountedWoTypeBySap,
        mountedEquipment,
        mountedLCMDefectCode,
        mountedLcdSize,
        mountedLCMGrade,
        mountedDefectCode,
        mountedLCMOEEBigEquipment,
        ///////////LCM EDC  部分 ////////
        mountedEDCEquipment,

       ////////////////////////////////////////////////// 
      /////***************SPC  部分******************/////
      mountedEqptId, 
      mountedSpcProd, 
      mountedDataGroup,
      mountedSpcModuleName,
      mountedSpcMeasTool,
      mountedSpcProcTool,
      mountedSpcLine,
      mountedSpcBeforeHours,
      mountedSpcProdNum,
      mountedSpcPreLine,
      mountedSpcProdID,
      mountedSpcOper,
      mountedSpcProduct,
      mountedSpcProcess,
      mountedSpcPerLine,


    /////***************MOD  部分******************/////
      mountedOperationForINS,
      mountedOperationForRWK,
       
  

      /////***************BEOL  部分******************/////
    }
}
