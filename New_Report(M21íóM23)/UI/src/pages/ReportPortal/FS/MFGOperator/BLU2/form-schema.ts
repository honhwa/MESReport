import type { FormSchema } from '@/components/core/schema-form/';
import {
  ShiftParams2, LcmProductParams, FloorParams1, StartDateParams, EndDateParams, OperationParams,
} from '@/pages/ReportPortal/Static/CommonParams';
import { reactive } from 'vue'

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
  LCMProductSize: [],
  LCMProductType: [],
  DefectCode: [],
  LcmProdTypeData: [],
  LCMGradeData: [],
  OwnerName: [],
  errorcode: []
});


export const schemas: FormSchema[] = [
  StartDateParams,
  EndDateParams,
  ShiftParams2,
  {
    label: 'common.commonpages.Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: ''
        },
        {
          label: '1F',
          value: " and m.floor='1'"
        },
        {
          label: '2F',
          value: " and m.floor='2'"
        },
        {
          label: '3F',
          value: " and m.floor='3'"
        }
      ]
    },
    defaultValue: '',
  },

  {
    label: 'common.commonpages.QueryType',
    component: 'RadioGroup',
    field: 'QType',
    componentProps: {
      options: [{ value: 'operators', label: '人員別' }, { value: 'equip_nbr', label: '機台別' }]
    },
    defaultValue: 'operators'

  },
  {
    field: 'LCMDivider',
    component: 'Divider',
    label: 'BLProduct'
  },

  {
    label: 'common.commonpages.size',
    component: 'Select',
    field: 'Size',


    componentProps: ({ formModel }) => {
      return {
        options: optionsdata.SizeData,
        placeholder: 'size',
        onChange: async (e: any) => {

          if (e !== 'ALL') {
            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.split('(')[1].replace(/[\(\)\-]+/g, '') == e);
          } else {
            optionsdata.BigProductData1 = optionsdata.BigProductData
          }

        },
        onSelect: async (e: any) => {
          // model.formModel.bigproduct = Array.from(new Set(optionsdata.BigProductData1.map(item => item.value)))[0]
          formModel.bigproduct = optionsdata.BigProductData1!.map(
            (item) => item.value
          )[0];
          changeProduct(formModel.bigproduct)
        }
      };
    }
  },
  {
    label: 'common.commonpages.bigproduct',
    component: 'Select',
    field: 'bigproduct',


    componentProps: ({ formModel, field }) => {
      return {
        options: optionsdata.BigProductData1,
        placeholder: 'bigproduct',
        onChange: async (e: any) => {
          changeProduct(e)

        },

      };
    },
    defaultValue: []


  },
  {
    label: 'common.commonpages.Product',
    field: 'Product',
    component: 'Select',
    componentProps: ({ formModel, field }) => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
        maxTagCount: 8,
        onSelect: (e: any) => {
          if (e != 'ALL') {
            formModel[field] = formModel[field].filter((item) => item != 'ALL');
          } else {
            formModel[field] = ['ALL'];
          }
        }
      };
    },
    defaultValue: ['ALL']
  },
  {
    label: '',
    // field: '#optionequipmenu',
    field: '#productselectmenu',
    component: 'RadioGroup',
    componentProps: ({ formModel }) => {
      return {
        options: [
          {
            label: '全選',
            value: 'selectALL'
          },
          {
            label: '取消全選',
            value: 'cancelselect'
          }
        ],
        onChange: (e) => {
          // 全選，全部添加
          if (e.target.value == 'selectALL') {
            // 上一個選項的所有值
            formModel['Product'] = optionsdata.ProductData1.map((item) => item.label)
          } else if (e.target.value == 'cancelselect') {
            // 全消全選
            formModel['Product'] = []
          }
        }
      };
    },
    defaultValue: ''
  },

  {
    label: 'common.commonpages.Operation',
    component: 'Select',
    field: 'Operation',
    componentProps: () => {
      return {
        options: optionsdata.OperationLabel
      }
    },
    defaultValue: 'ALL'
  }
];

function changeProduct(e) {
  let Data: string[] = []
  if (e.includes('ALL')) {
    optionsdata.ProductData1 = optionsdata.ProductData
  } else {
    // e.forEach((product: string) => {
    let size = e.split('(')[1].replace(/[\(\)\-]+/g, '')
    let bigp = e.split('(')[0]
    Data = optionsdata.ProductData.filter(item => item.value.includes(bigp) && item.value.includes(size))
    // })
    optionsdata.ProductData1 = Data;
    // console.log(Data,e);

  }
}