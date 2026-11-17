import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,LcmOwnerParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  let ifSelectColall=false// 是否全選標記

  export let GrpOptions = [
  {value: "ALL", label: "ALL" },
  {value: "A.WO_NBR", label: "WO_NBR" },
  { value: "A.CUST_GRP", label: "CUST_GRP" },
  { value: "A.FLOOR", label: "FLOOR" },
  { value: "A.LCM_OWNER", label: "LCM_OWNER" },
  { value: "A.VER", label: "VER" },
  { value: "A.PROD_NBR", label: "PROD_NBR" },
  { value: "A.TARGET_QTY", label: "TARGET_QTY" },
  { value: "A.ACTUAL_QTY", label: "ACTUAL_QTY" },
  { value: "(A.TARGET_QTY-A.ACTUAL_QTY) DIFF", label: "DIFF" },
  { value: "A.RELEASE_DT", label: "RELEASE_DT" },
  { value: "A.PLAN_ST_DT", label: "LAN_ST_DT" },
  { value: "A.DUE_DT", label: "DUE_DT" },
  { value: "A.OPER", label: "OPER" },
  { value: "A.COMMENTS", label: "COMMENTS" },
  { value: "A.LCD_ABN_CF", label: "LCD_ABN_CF" },
  { value: "A.LCD_ABN_LCD", label: "LCD_ABN_LCD" },
  { value: "B.TYPES", label: "TYPES" },
  { value: "B.LCD_PRODUCT_ID", label: "LCD_PRODUCT_ID" },
  { value: "B.LCD_OWNER", label: "LCD_OWNER" },
  { value: "B.LCD_GRADE_GRP", label: "LCD_GRADE_GRP" },
  { value: "B.GRADE_MAX_QTY", label: "GRADE_MAX_QTY" },
  { value: "B.GRADE_ACTUAL_QTY", label: "GRADE_ACTUAL_QTY" },
  { value: "A.AERB_NBR", label: "AERB_NBR" },
  { value: "A.STATUS", label: "STATUS" },
  ]


  export const schemas = (FactoryType:string,optionsdata:any)=>{
  return[
  ...FromDateParams,
  ...ToDateParams,
  ...FloorParams, 
  ...InputWorkorderParams,
  ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
  ...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  ...new OperationParams(optionsdata,FactoryType)._obj,
  {
    label: '顯示欄位',
    field: 'Colname',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
      return {
        mode: "multiple",
        options: GrpOptions,
        maxTagCount:10,
        onChange: async (e: any) => {  
                         
          if (e.indexOf('ALL')!= -1&& ifSelectColall!=true) {
           
            let SelectData: string[] = []    
             GrpOptions.forEach(item => {
              SelectData.push(item.value)
            });
            formModel[field] = SelectData
            ifSelectColall=true
          }else if (e.indexOf('ALL')!= -1&& ifSelectColall===true) {
            
            e.map((val,i)=>{
              if(val==="ALL"){
               e.splice(i,1);
              }
           })
           formModel[field]=e           
           ifSelectColall=false
          
          }else {
            //console.log(66) 
            if(e.indexOf('ALL')=== -1 && ifSelectColall===true)
            {
              formModel[field]=[]
              ifSelectColall=false
              
              
            }else{
              //console.log("999")
              formModel[field]=e
            }
            
          }
        }
     }
   }
  }, 
  
];
}
