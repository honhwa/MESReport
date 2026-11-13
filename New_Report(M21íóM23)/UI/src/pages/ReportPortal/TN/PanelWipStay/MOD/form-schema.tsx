import type { FormSchema } from '@/components/core/schema-form/'
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { AddCommonPageParams } from '@/store/module/commonpage';
import {FromDateParams,ToDateParams,FloorParams,ReworkedParams} from '@/pages/ReportPortal/Static/TNCommonParams'
import {Size, Owner, Operation} from '@/pages/ReportPortal/Static/TNCommonParams'

  const useCommonParams = AddCommonPageParams();

  export let WotypeOptions = [
    {value: "ALL", label: "ALL" },
    {value: "1", label: "MP" },
    {value: "2", label: "EN" },
    {value: "3", label: "RY" },
    {value: "4", label: "RK" },
    {value: "5", label: "RM" },
    {value: "6", label: "ME" },
    {value: "7", label: "TR" },
  ];


  export const schemas = (FactoryType:string)=>{
    let optionsdata = reactive({});
  return [
    {
      label: 'common.commonpages.QueryType',
      component: 'RadioGroup',
      field: 'Queryby',

      componentProps: {
        options: [
          {
            label: '各站停留時間',
            value: 'last_trans_dt'
          },
          {
            label: 'FAB停留時間',
            value: 'start_time'
          }
        ]
      },
      defaultValue: 'last_trans_dt'

    }, 
    { 
      label: 'common.commonpages.ReportType',
      component: 'RadioGroup',
      field: 'ReportType',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },        
          {
            label: '站點',
            value: "Oper"
          },
          {
            label: '機種',
            value: 'Prod'
          },
          
        ],
      },
      defaultValue: 'ALL'

    }, 
    ...FloorParams,
    { 
      label: 'common.commonpages.HoldFlag',
      component: 'RadioGroup',
      field: 'Hold',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL'
          },        
          {
            label: '是',
            value: "Y"
          },
          {
            label: '否',
            value: 'N'
          },
          
        ],
      },
      defaultValue: 'ALL'

    },  
    ReworkedParams,
    { 
      label: 'common.commonpages.WorkOrderLike',
      component: 'Input',
      field: 'Wo_nbr',
    },
    {
      label: 'common.commonpages.WorkType',
      component: 'CheckboxGroup',   
      field: 'Wotype',
      componentProps: () => {
        return {
          options: WotypeOptions,
        };
      },
      defaultValue: ['ALL']
    },
    { 
      label: 'common.commonpages.SelectWorkDueDay',
      component: 'RadioGroup',
      field: 'Iswodueday',
      componentProps: {
        options: [
          {
            label: '否',
            value: 'N'
          },
          {
            label: '是',
            value: "Y"
          },
          
        ],
      },
      defaultValue: 'NO'

    },
    ...FromDateParams,
    ...ToDateParams,
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata,FactoryType,'owner_code').obj,
    ...new Operation(optionsdata, FactoryType,'workGroup').obj,
  ];
};        


