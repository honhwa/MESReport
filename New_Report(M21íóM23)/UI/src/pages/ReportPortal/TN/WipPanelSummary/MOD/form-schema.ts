import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
import {InputWorkorderParams,FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'

import {Size, Operation, HoldCode} from '@/pages/ReportPortal/Static/TNCommonParams'

const useCommonParams = AddCommonPageParams();

// export let optionsdata: any = reactive({  
//   OwnerName: [{ label: 'ALL', value: '' }],
//   OperationLabel: [],  
// })
let ifSelectColall=false// 是否全選標記

export let GrpOptions = [
{value: "ALL", label: "ALL" },
{value: "a.prod_nbr", label: "Lcm Product" },
{ value: "a.lcm_owner", label: "Lcm Owner" },
{ value: "a.work_ctr", label: "Oper" },
{ value: "a.hold_flag", label: "Hold" },
{ value: "a.hold_code", label: "Hold Code" },
{ value: "panel_size", label: "Size" },
{ value: "a.wo_nbr", label: "Work Order" },
{ value: "a.LCD_PROD_NBR", label: "Lcd Product" },
{ value: "a.lcd_grade", label: "Lcd Grade" },
{ value: "a.MVIN_FLAG", label: "Mvin Flag" },
{ value: "a.lcm_grade", label: "Lcm Grade" },
{ value: "a.MIN_QRS_DT", label: "Q-Time Flag" },
]

export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
  return [
    {
      label: 'Start Hours',    
      field: 'StartHour',  
      component: 'Input',    
      componentProps: () => {
        return {
          placeholder: '大於多少',
          onChange: (e: any) => {
            console.log(e);
          },
        };
      },
      componentSlots: () => {
        return {
          prefix: () => 'over',
          suffix: () => '小時',
        };
      },
    },  
    ...FloorParams,
    ...InputWorkorderParams,
    ...new Size(optionsdata, FactoryType).obj,
    ...new Operation(optionsdata, FactoryType,'workGroup').obj,
    ...new HoldCode(optionsdata, FactoryType,'HoldCode').obj,
    {
      label: 'common.commonpages.DisplayColumn',
      field: 'Colname',
      component: 'Select',
      componentProps: ({ formModel, field }) => {
        return {
          mode: "multiple",
          options: GrpOptions,
          onChange: async (e: any) => {  
                          
            if (e.indexOf('ALL')!= -1&& ifSelectColall!=true) {
            
              let SelectData: string[] = []    
              GrpOptions.forEach(item => {
                if(item.value == 'ALL')
                {}
                else
                {
                  SelectData.push(item.value)
                }
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
              console.log(66) 
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
    }
  ];
};
