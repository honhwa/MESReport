import type { FormSchema } from '@/components/core/schema-form/';
import {ReworkedParams} from '@/pages/ReportPortal/Static/CommonParams'
import { formatToDateHour } from '@/utils/dateUtil';
import { reactive } from 'vue';
import { Size } from '@/pages/ReportPortal/Static/common';



export const schemas=(Fab):FormSchema[] =>{
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
    ...new Size(optionsdata, 'LCM2').obj,

    ReworkedParams,
    {
      label: 'common.commonpages.ModelType',
      component: 'RadioGroup',
      field: 'ModelType',
      componentProps: {
        
        options: [
          {
            label: 'ALL',
            value: ''
          },
          {
            label: 'TV MOD-UD(4K2K)',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)='D' "
          },
          {
            label: 'TV MOD-FHD ',
            value: "and substr(a.prod_nbr,1,1)='G' and substr(a.prod_nbr,6,1)<>'D' "
          },
          {
            label: 'TV Open cell-UD(4K2K)',
            value: "and substr(a.prod_nbr,1,1)='2' and substr(a.prod_nbr,6,1)='D' "
          },{
            label:'TV Open cell FHD ',
            value: "and substr(a.prod_nbr,1,1)='2' and substr(a.prod_nbr,6,1)<>'D' "
          }
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
            label: 'ALL',
            value: 'A'
          },
          {
            label: '1700',
            value: "Band work_ctr ='1700'"
          },
          {
            label: '1760',
            value: "Band work_ctr ='1760'"
          },
          {
            label: '1800',
            value: "Band work_ctr ='1800'"
          },
          {
            label: '1910',
            value: "Band work_ctr ='1910'"
          },{
            label: '6500',
            value: "Band work_ctr ='6500'"
          },
        ],
      },
      defaultValue: "Band work_ctr ='1760'"
    
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

