import type { FormSchema } from '@/components/core/schema-form/';
import {
    ShiftParams, LcmProductParams,ReworkedParams,FloorParams,
  LCMOwnerParams, LCDGradeParams, LcdProductParams
  
} from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';




export const schemas = (Fab)=>{
  let optionsdata =reactive({}) 
  return [
    { label: 'common.commonpages.Date',
    component: 'DatePicker',
    field: 'CompleteFromDate',
  
    componentProps: {
      format: 'YYYY-MM-DD HH:mm',
      showTime:true,
      minuteStep:30,
      valueFormat:'YYYY-MM-DD HH:mm'
    },
    defaultValue:formatToDateHour()
  },
  {
    label: 'common.commonpages.To',
    component: 'DatePicker',
    field: 'CompleteToDate',
  
    componentProps: {
      format: 'YYYY-MM-DD HH:mm',
      showTime:true,
      minuteStep:30,
      valueFormat:'YYYY-MM-DD HH:mm'
    },
    defaultValue: formatToDateHour(),
  },
  {
    label: 'common.commonpages.Shift',
    component: 'RadioGroup',
    field: 'Shift',
  
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: '日(A)',
          value: "and shift_id='A'"
        },
        {
          label: '夜(B)',
          value: "and shift_id='B'"
        }
      ],
    },
    defaultValue: ''
  },
  {
    label: 'common.commonpages.Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [{ label: 'ALL', value: '' }, { label: '1F', value: " and floor='1'" },
      { label: '2F', value: " and floor='2'" }, { label: '3F', value: " and floor='3'" },]
    },
    defaultValue: ''
  },
    {
      field: 'LCMDivider',
      component: 'Divider',
      label: 'common.commonpages.LCMProduct'
    },
    ...new Size(optionsdata, 'SET1').obj,
    ReworkedParams,
    LCMOwnerParams,
    {
      label: 'common.commonpages.ModelType',
      component: 'RadioGroup',
      field: 'ModelType',
      componentProps: {
        // console.log(field);
        
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: 'TV MOD-UD(4K2K)',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)='D'  "
          },
          {
            label: 'TV MOD-FHD ',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)<>'D'  "
          },
        ],
      },
      defaultValue: ''
    
    },
    {
      label: 'common.commonpages.InspectionSite',
      component: 'RadioGroup',
      field: 'InspectionSite',
      componentProps: {
        options: [
          {
            label: '8550(OQC InLine檢)',
            value: "Aand work_ctr='8550'"
          },
          {
            label: '8650(OBA1 OQC檢) ',
            value: "Band work_ctr='8650'"
          },
          {
            label: '8656(OBA2 客驗) ',
            value: "Band work_ctr='8656'"
          }
        ],
      },
      defaultValue: "Band work_ctr='8650'"
    
    },
    {
      label: 'common.commonpages.OBAInspectionType',
      component: 'RadioGroup',
      field: 'OBAInspectionType',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: 'JQC檢',
            value: "JQC"
          },
          {
            label: 'VOE檢',
            value: 'VOE'
          },
          {
            label: 'OBA檢',
            value: 'OBA'
          }
        ],
      },
      defaultValue: ''
    
    }
  ];
}

