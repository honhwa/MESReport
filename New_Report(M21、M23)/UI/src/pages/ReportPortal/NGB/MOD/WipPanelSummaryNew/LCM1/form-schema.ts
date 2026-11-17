import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,HoldCodeParams,StartHoursParams,ToHourParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

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
{ value: "panel_size", label: "Size" },
{ value: "a.wo_nbr", label: "Work Order" },
{ value: "b.due_dt", label: "Due Date" },
{ value: "a.LCD_PROD_NBR", label: "Lcd Product" },
{ value: "a.lcd_grade", label: "Lcd Grade" },
{ value: "a.lcm_grade", label: "Lcm Grade" },
{ value: "b.cust_grp", label: "Cust Grp" },
]



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
    {
      label: 'FAB Source',
      component: 'RadioGroup',
      field: 'FAC',
      componentProps: ({formModel,formInstance}) => {
        return {          
            options: [
         {
                label: 'NC2',
                value: "LCM1"
        },       
       {
        label: 'NB2',
        value: "LCM2"
       },                 
          ],    
          onChange: async (e: any) => {
            FactoryType=formModel.FAC;
         
        if (FactoryType=="LCM1" )
          {
          useCommonParams.G_FAC_COM='C'           
          }
         else if (FactoryType=="LCM2" )
         {
          useCommonParams.Fab="LCM2";
         useCommonParams.G_FAC_COM='B';          
       }
      // console.log("pre optiondata",optionsdata);  
      formModel.Operation = undefined; 
    
      const { updateSchema } = formInstance;
         updateSchema(
         ...new OperationParams(optionsdata,FactoryType)._obj,
         ...new HoldCodeParams(optionsdata,FactoryType)._obj,
         ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
        );
                  
   
    
         },       
        }
  
      },
      defaultValue: 'LCM1',
    
    }, 
  ...StartHoursParams,
  ...ToHourParams,  
   ...InputWorkorderParams,
   ...FloorParams,
   {
    label: 'Hold狀態',
    component: 'RadioGroup',
    field: 'IsHold',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'HOLD',
          value: 'Y'
        },
        {
          label: 'NO HOLD',
          value: 'N'
        }
      ]
    },
    defaultValue: 'ALL'

  }, 
  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
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
  ...new HoldCodeParams(optionsdata,FactoryType)._obj,
 ];
}
