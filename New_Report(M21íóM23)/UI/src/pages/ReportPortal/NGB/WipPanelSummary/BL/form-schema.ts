import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,BLBySizeProdParams,
  OperationParams,HoldCodeParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

const useCommonParams = AddCommonPageParams();
const { getCommonFormParams } = useRemoteSelect()

// export let optionsdata: any = reactive({  
//   OwnerName: [{ label: 'ALL', value: '' }],
//   OperationLabel: [],  
// })
let ifSelectColall=false// 是否全選標記

export let GrpOptions = [
{value: "ALL", label: "ALL" },
{value: "a.prod_nbr", label: "Lcm Product" },
{ value: "a.lcm_owner", label: "Owner" },
{ value: "a.work_ctr", label: "Oper" },
{ value: "a.hold_flag", label: "Hold" },
{ value: "a.hold_code", label: "Hold_code" },
{ value: "a.panel_size", label: "Size" },
{ value: "a.wo_nbr", label: "Work Order" },
{ value: "b.due_dt", label: "Due day" },

]



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return[
 
  
   ...InputWorkorderParams,
   ...FloorParams,
   
   ...new BLBySizeProdParams(optionsdata,FactoryType).getSize(),
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
  ///...HoldCodeParams,
];
}

