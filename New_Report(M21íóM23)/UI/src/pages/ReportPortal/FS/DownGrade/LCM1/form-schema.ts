import type { FormSchema } from '@/components/core/schema-form/';
import {
    ShiftParams, LcmProductParams,LcdProductParams,
  LCMOwnerParams, StartDateParams, EndDateParams,PanelSourceParams,LCDGrade1Params,LCMProductSizeParams, LCMProductTypeParams
  
} from '@/pages/ReportPortal/Static/CommonParams'
import { reactive } from 'vue';
import {  Size } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  LCMOwner: [],
  OperationLabel: [],
  LCDSizeData: [{ label: 'ALL', value: 'ALL' }],
  LCDProductData: [],
  LCDProductData1: [],
});
export let optionsdata2: any = reactive({
  LcdGradeLabel:[],
  OperationOptionLCM1:[
    {
      label: '1600 CKEN',
      value: '1600'
    },
    {
      label: '1700 DKEN',
      value: "1700"
    },
    {
      label: '1800 PAKING',
      value: "1800"
    },{
      label: '1910 SHIPPING',
      value: "1910"
    },{
      label: '1350 BOND CKEN',
      value: "1350"
    },
  ],
  OperationOptionLCM2:[
    {
      label: '1600 CKEN',
      value: '1600'
    },
    {
      label: '1700 DKEN',
      value: "1700"
    },
    {
      label: '1800 PAKING',
      value: "1800"
    },{
      label: '1910 SHIPPING',
      value: "1910"
    },{
      label: '1300 BOND CKEN',
      value: "1300"
    },
  ],
  OperationOption:[]
})



export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  {
    label: 'Shift',
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
          value: " and shift_id='A'"
        },
        {
          label: '夜(B)',
          value: " and shift_id='B'"
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
    label: 'common.commonpages.LCMProductArea',
    component: 'CheckboxGroup',
    field: 'LCMProductArea',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'Moudle',
          value: "Moudle"
        },
        {
          label: 'SKD',
          value: "SKD"
        },
        {
          label: 'LED',
          value: "LED"
        },
        {
          label: 'Down Grade ',
          value: "Down Grade "
        },
      ],
    },
    defaultValue: ['ALL']
  },
  {
    label: 'common.commonpages.SourceFab',
    component: 'RadioGroup',
    field: 'SourceFab',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: 'CMO',
          value: "and source_fab<>'IDT'"
        },
        {
          label: 'IDT',
          value: "and source_fab='IDT'"
        },
      ],
    },
    defaultValue: ''
  },
  PanelSourceParams,
  {
    label: 'common.commonpages.ShowMode',
    component: 'RadioGroup',
    field: 'ShowMode',
    componentProps: {
      options: [
        {
          label: '人員別',
          value: 'Operator'
        },
        {
          label: '機種別良率',
          value: "Operator1"
        },
        {
          label: '機台別',
          value: "equip_nbr"
        },{
          label: '機種別',
          value: "Prod_nbr"
        },
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
    field: 'WorkOrderLike',
    component: 'Input',
    label: 'common.commonpages.WorkOrderLike'
  },
  // LCMProductTypeParams,
  {
    label: 'common.commonpages.LCMProductType',
    component: 'Select',
    field: 'LCMProductType',
    componentProps: () => {
      return {
        options: optionsdata2.LcmProdTypeData,
      };
    },
    defaultValue:"ALL"
  },
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'common.commonpages.LCMProduct'
  },
  ...new Size(optionsdata, 'LCM1').getSize(),
  LCMOwnerParams,
  // LCDGrade1Params,
  {
    label: 'LCD Grade',
    component: 'Select',
    field: 'LCDGrade',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata2.LcdGradeLabel
      }
    },
  },
  {
    field: '#LCDDivider',
    component: 'Divider',
    label: 'LCD Product'
  },
  {
    label: 'common.commonpages.size',
    component: 'Select',
    field: 'LCDSize',
    LabelStyle:{color:'blue'},

    componentProps: (model) => {
      return {
        options: optionsdata.LCDSizeData,
        placeholder: 'LCDSize',
        onChange: async (e: any) => {

          if (e !== 'ALL') {
            // console.log(e);

            optionsdata.LCDProductData1 = optionsdata.LCDProductData.filter(item => item?.SIZE1 == e);
          } else {
            optionsdata.LCDProductData1 = optionsdata.LCDProductData
          }

        },
        onSelect: async (e: any) => {
          model.formModel.LCDProduct = Array.from(new Set(optionsdata.LCDProductData1.map(item => item.value)))
        },
      };
    }
  },
  {
    label: 'common.commonpages.Product',
    field: 'LCDProduct',
    component: 'Select',
    LabelStyle:{color:'blue'},
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.LCDProductData1,
        placeholder: 'product',
        maxTagCount: 8,
      };
    },
  },
  {
    label: 'common.commonpages.Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: ()=>{
    return  {
      options: optionsdata2.OperationOption,
    }
  },
    defaultValue: '1600'
  },
  {
    label: 'common.commonpages.LCMProductSize',
    component: 'Select',
    field: 'LCMProductSize',
    componentProps: () => {
      return {
        options: optionsdata.LCMProductSize
      }
    },
    defaultValue: 'ALL'
  }
];

