/*
 * @Author: snake19840
 * @Date: 2023-01-03 15:02:49
 * @LastEditors: snake19840
 * @Description: 
 * 
 * Copyright (c) 2023 by snake19840/www.snake19840.top, All Rights Reserved. 
 */
import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {LcmBySizeByModuleProdParams,CompleteFromDateParams,CompleteToDateParams,InputWorkorderParams,FloorParams,
  OperationParams,ShiftParams,LcmBySizeProdParams,CustomerParams} from '@/pages/ReportPortal/Static/SHCommonParams'

  export let MessageOptions = [
    {value: "Panel_id", label: "Panel_id" },
    {value: "Cart_id", label: "Cart_id" },
    {value: "Cart_ID_cust", label: "Cart_ID_cust" },
    {value: "hold_code", label: "hold_code" },
    ]

    export let ShowTypeOptions = [
        {value: "ALL", label: "ALL" },
        {value: "WIP", label: "WIP" },
        {value: "WMS", label: "WMS" },
        ]
    


export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
  ...CompleteFromDateParams,
  ...CompleteToDateParams,
  ...FloorParams,  
  {
    label: 'Message Type',
    field: 'MessageType',
    component: 'Select',
    componentProps: {
      options: MessageOptions
    },
  },
  {
    label: '請輸入:',
    component: 'InputTextArea',
    field: 'TypeVal',
  },
  {
    label: '顯示方式',
    component: 'RadioGroup',
    field: 'ShowType',
    componentProps: {
      options: ShowTypeOptions
    },
    defaultValue: 'ALL'
  }, 

  ...new LcmBySizeByModuleProdParams(optionsdata,FactoryType).getSize(),
];
}
