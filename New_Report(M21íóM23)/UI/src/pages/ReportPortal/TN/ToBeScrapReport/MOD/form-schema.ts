import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import {
  FromDateParams,ToDateParams, ShiftParams, FloorParams} from '@/pages/ReportPortal/Static/TNCommonParams'
import {Size, Owner, PanelSource} from '@/pages/ReportPortal/Static/TNCommonParams'

export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
  return [
    ...FromDateParams,
    ...ToDateParams,
    ...ShiftParams,
    ...FloorParams,
    ...new PanelSource(optionsdata,FactoryType,'PanelSource').obj,
    ...new Size(optionsdata, FactoryType).obj,
    ...new Owner(optionsdata,FactoryType,'owner_code').obj,
  ];
};

