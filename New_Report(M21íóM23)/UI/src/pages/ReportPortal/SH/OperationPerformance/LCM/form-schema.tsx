import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams,LcmOwnerParams,LcdOwnerParams,ShopCDParams} from '@/pages/ReportPortal/Static/SHCommonParams'


const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect()
let ifSelectColall=false// 是否全選標記
export let WoTypeOptions = [
  { value: "ALL", label: "ALL" },
  { value: "N", label: "N" },
  { value: "W", label: "W" },
  { value: "E", label: "E" },
  { value: "X", label: "X" },
  { value: "S", label: "S" },
  { value: "D", label: "D" },
  { value: "P", label: "P" },
  { value: "Q", label: "Q" },
  { value: "R", label: "R" },
  { value: "C", label: "C" },
  { value: "M", label: "M" },
  { value: "B", label: "B" },
  { value: "Y", label: "Y" },
  ]


export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [ 
  ...FromDateParams,
  ...ToDateParams,
  ...ShiftParams,
  ...ShopCDParams,
  ...FloorParams,
  ...InputWorkorderParams,
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
  //...new LcmOwnerParams(optionsdata,FactoryType)._obj,
  //...new LcdOwnerParams(optionsdata,FactoryType)._obj,
  {
    label: '工單類型',
    field: 'WoType',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
      return {
        mode: "multiple",
        options: WoTypeOptions,
        maxTagCount:10,
        onChange: async (e: any) => {  
                         
          if (e.indexOf('ALL')!= -1&& ifSelectColall!=true) {
           
            let SelectData: string[] = []    
            WoTypeOptions.forEach(item => {
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
