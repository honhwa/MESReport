import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';

import {FromDateParams,ToDateParams,FloorParams,SourceFabParams,ShiftParams} from '@/pages/ReportPortal/Static/TNCommonParams'

import {Size, Owner, LCD_Grade, DownGrade, PanelSource} from '@/pages/ReportPortal/Static/TNCommonParams'

export const schemas = (FactoryType:string)=>{ 
  let optionsdata = reactive({});
    return [
      ...FromDateParams,
      ...ToDateParams,
      ...ShiftParams,
      ...FloorParams,
      ...SourceFabParams,
      ...new PanelSource(optionsdata,FactoryType,'PanelSource').obj,
      {
        label: 'common.commonpages.DisplayModel',
        component: 'RadioGroup',
        field: 'ShowMode',
        componentProps: {
          options: [
            {
              label: '人員別',
              value: 'Operator'
            },
            {
              label: '機種別',
              value: "Prod_nbr"
            }
          ],
        },
        defaultValue: 'Operator'
      },
      {
        field: 'dgrate',
        component: 'Input',
        label: 'common.commonpages.Ratio',
        componentSlots: () => {
          return {
            prefix: () => 'Downgrage >',
            suffix: () => '%',
          };
        },
        defaultValue:'-100'
      },
      {
        field: 'rwrate',
        component: 'Input',
        label: '',
        componentSlots: () => {
          return {
            prefix: () => '送修 >',
            suffix: () => '%',
          };
        },
        defaultValue:'-100'
      },
      { 
        label: 'common.commonpages.WorkOrderLike',
        component: 'Input',
        field: 'Wo_nbr',
      },
      ...new Size(optionsdata, FactoryType).obj,
      ...new Owner(optionsdata,FactoryType,'owner_code').obj,
      ...new LCD_Grade(optionsdata,FactoryType,'LcdGrade').obj,
      ...new DownGrade(optionsdata,FactoryType,'workGroup').obj,
      { 
        label: 'common.commonpages.ReworkFlag',
        component: 'RadioGroup',
        field: 'Reworked',
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
    ];
};
