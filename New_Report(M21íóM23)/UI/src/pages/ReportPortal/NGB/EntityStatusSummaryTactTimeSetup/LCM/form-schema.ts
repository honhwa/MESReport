import type { FormSchema } from '@/components/core/schema-form/'
import dayjs from 'dayjs';
import { reactive } from 'vue';
import {LcmBySizeByModuleProdParams,FromDateParams,ToDateParams,InputWorkorderParams,FloorParams,DateMonthParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,LcmEquipmentParams,LcdOwnerParams,LcdGradeParams,LcmOwnerParams,WorkOrderLikeParams} from '@/pages/ReportPortal/Static/NGBCommonParams'



export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
 ...new DateMonthParams(optionsdata)._obj,
 ...new LcmBySizeProdParams(optionsdata,FactoryType).getSize(),
 ...new LcmEquipmentParams(optionsdata,FactoryType)._obj,    
 ];
}


export const schemasdetail = (FactoryType:string,optionsdata:any)=>{
  return [
    ...new DateMonthParams(optionsdata)._obj,
    {
      label: '請選擇要維護的項目',
      component: 'RadioGroup',
      field: 'action',
      componentProps: {
        options: [
          {
            label: '查詢',
            value: 'Query'
          },
          {
              label: '刪除',
              value: 'Delete'
          }
          
        ]
      },
      defaultValue: 'Query',
    },
  ];
}