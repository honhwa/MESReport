import type { FormSchema } from '@/components/core/schema-form/'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { NGBSelectType} from '@/pages/ReportPortal/Static/NGBCommon-Form-schema';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,ReworkedParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CompleteFromDateParams,CompleteToDateParams,LcmOwnerParams} from '@/pages/ReportPortal/Static/NGBCommonParams'

  const useCommonParams = AddCommonPageParams();
  const { getCommonFormParams,getCommonFormParamsNoAll,mountedEDCEquipment } = useRemoteSelect()
  let ifSelectItemall=false// 是否全選標記



  export let GrpOptions = [
    {
      label: 'EDC',
      value: 'EDC'
    },
    {
      label: 'RCP',
      value: 'RCP'
    },
   ]


export const schemas = (FactoryType:string,optionsdata:any)=>{
return [
...CompleteFromDateParams,
...CompleteToDateParams,
...FloorParams,
...new EDCEquipmentParams(optionsdata,FactoryType)._obj,
   
  { 
    label: '是否顯示WIP(RECIPE)',
    component: 'RadioGroup',
    field: 'IsWip',
    componentProps: {
      options: [
        {
          label: 'NO',
          value: 'NO'
        },
        {
          label: 'YES',
          value: "YES"
        },
      ],
    },
    defaultValue: 'NO'

  },  
  {
    label: '線別(eqp_id)',
    component: 'Select',
    field: 'Equipment',
    componentProps: ({ formModel, field }) => {
      //console.log("formModel.Choose",formModel.Choose)
      return {     
        options: optionsdata.LCMEDCEquipmentLabel,
        placeholder: 'Equipment',
        onChange: async (e: any) => {  
          // if (formModel.Choose == 'EDC'){
             //console.log(111,optionsdata.LCMEDCUnit)
             formModel.Unit=[];
             formModel.Item=[];
             optionsdata.LCMEDCUnit = await getCommonFormParamsNoAll('UNIT', {
              name: "Date",
              apiName: "EDCUnit",
              FactoryType: useCommonParams.Fab,
              G_FAC: useCommonParams.G_FAC_COM,
              EDCEquipment: e,
              Choose:formModel.Choose
            })
          // }else{             
          //    optionsdata.LCMEDCUnit = await getCommonFormParams('UNIT', {
          //     name: "Date",
          //     apiName: "EDCUnit",
          //     FactoryType: useCommonParams.Fab,
          //     G_FAC: useCommonParams.G_FAC_COM,
          //     EDCEquipment: e,
          //     Choose:formModel.Choose
          //   })

          // }       
            
          
        },
      };
    },
  },
  {
    label: 'UNIT',
    field: 'Unit',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
          return {
            options: optionsdata.LCMEDCUnit,
            placeholder: 'UNIT',
            onChange: async (e: any) => {   
              formModel.Item=[];        
              optionsdata.LCMEDCItem = await getCommonFormParams('HOSTDATANAME', {
                name: "Date",
                apiName: "EDCItem",
                FactoryType: useCommonParams.Fab,
                G_FAC: useCommonParams.G_FAC_COM,
                EDCUnit: formModel.Unit,
                EDCEquipment:formModel.Equipment
              })
            }
          }

     
    },
  },
  {
    label: 'EDC Item',
    field: 'Item',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
          return {
            mode: 'multiple',
            options: optionsdata.LCMEDCItem,
            maxTagCount:10,
            placeholder: 'Item',
            onChange: async (e: any) => {  
                         
              if (e.indexOf('ALL')!= -1&& ifSelectItemall!=true) {
               
                let SelectData: string[] = []    
                optionsdata.LCMEDCItem.forEach(item => {
                  SelectData.push(item.value)
                });
                formModel[field] = SelectData
                ifSelectItemall=true
              }else if (e.indexOf('ALL')!= -1&& ifSelectItemall===true) {
                
                e.map((val,i)=>{
                  if(val==="ALL"){
                   e.splice(i,1);
                  }
               })
               formModel[field]=e           
               ifSelectItemall=false
              
              }else {
                //console.log(66) 
                if(e.indexOf('ALL')=== -1 && ifSelectItemall===true)
                {
                  formModel[field]=[]
                  ifSelectItemall=false
                  
                  
                }else{
                  //console.log("999")
                  formModel[field]=e
                }
                
              }
            }
          }
    },
    vIf: ({ formModel }) => formModel.Choose == 'EDC'
  },
  {
    label: 'EDC Panel ID 輸入',
    component: 'InputTextArea',
    field: 'PanelId',
    componentProps: ({ formModel, field }) => {
      return{
        placeholder:"最大限制筆數:500",
      }
    },
    vIf: ({ formModel }) => formModel.Choose == 'EDC'
  },

 ];
}   


///**************LCM EDC******************/////
class EDCEquipmentParams{
  optionsdata:NGBSelectType;
  FactoryType: string;
  _obj: FormSchema[];
  constructor(
    optionsdata:NGBSelectType,
    FactoryType: string,
  ){
    this.optionsdata = optionsdata;
    this.FactoryType = FactoryType;
    optionsdata['FactoryType'] = this.FactoryType;
    this.init(optionsdata, this.FactoryType);
    this._obj =[ 
      {
        label: 'Chooce',
        component: 'RadioGroup',
        field: 'Choose',
        componentProps: ({ formModel, field }) => {
          return {
            options: GrpOptions,        
            onChange: async (e: any) => {
              formModel.Equipment=[];
              formModel.Unit=[];
              formModel.Item=[];
              optionsdata.LCMEDCEquipmentLabel = await getCommonFormParamsNoAll('EQP_ID', {
                name: "Date",
                apiName: "EDCEquipment",
                FactoryType: useCommonParams.Fab,
                G_FAC: useCommonParams.G_FAC_COM,
                Choose:formModel.Choose,
              })
           },       
          }
    
        },
        defaultValue: 'EDC',
      
      }, 
    ]; 
    
  }
  // api請求數據
  async init(optionsdata: NGBSelectType, FactoryType: string) {
    await mountedEDCEquipment(optionsdata, useCommonParams.Fab)  
  }

  // 獲取Marterial
  get obj() {
    return this._obj;
   }
   // 更改label
   setLabel(index: number, LabelValue: string) {
       this._obj[index]['label'] = LabelValue;
       return this;
    }
}


