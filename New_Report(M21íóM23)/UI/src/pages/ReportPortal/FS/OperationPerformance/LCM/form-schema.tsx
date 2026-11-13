import { reactive,computed } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Size} from '@/pages/ReportPortal/Static/common';
import { useI18n } from '@/hooks/useI18n';

const { t } = useI18n();

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
  OwnerName: [],
  LcdGradeLabel:[],
  LcmProdTypeData:[],
})
export async function getSizeData(func: Function, params: String, funcparams: String) {
  let ReturnData: any = [];

  let { Date: { Data: { Table: Data } } } = await func({
    G_FAC: "A",
    name: "Date",
    apiName: funcparams,
  });

  for (let i in Data) {
    //@ts-ignore
    ReturnData.push({ label: Data[i][params], value: Data[i][params] });
  }
  ReturnData.unshift({ label: 'ALL', value: 'ALL' })
  return ReturnData

}
export const schemas  = (FactoryType:string)=>{
  return [
  {
    field: 'DateFrom',
    component: 'DatePicker',
    label: 'DateFrom',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
  {
    field: 'DateTo',
    component: 'DatePicker',
    label: 'DateTo',
    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
    required: true
  },
  {
    label: 'Shift',
    component: 'RadioGroup',
    field: 'Shift',
    componentProps: {
      options: [
        {
         
          value: 'ALL'
        },
        {
          label: '日(A)',
          value: "日"
        },
        {
          label: '夜(B)',
          value: '夜'
        }
      ],
    },
    defaultValue: 'ALL'

  },
  {
    label: 'Floor',
    component: 'RadioGroup',
    field: 'Floor',
    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: '1F',
          value: '1'
        },
        {
          label: '2F',
          value: '2'
        },
        {
          label: '3F',
          value: '3'
        }
      ],
    },
    defaultValue: 'ALL'
  },

  {
    label: 'Work Order',
    component: 'InputTextArea',
    field: 'WorkOrder',

  },
  {
    label: 'LCM Product Type',
    component: 'Select',
    field: 'LcmProductType',
    componentProps: () => {
      return {
        options: optionsdata.LcmProdTypeData
      }
    },
  },
    ...new Size(optionsdata, FactoryType).getSize(),
  /* {
    label: 'size',
    component: 'Select',
    field: 'Size',

    componentProps: () => {
      return {
        options: optionsdata.SizeData,
        placeholder: 'size',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1)
          } else {
            optionsdata.BigProductData1 = optionsdata.BigProductData
          }

        },
      };
    }
  },
  {
    label: 'big product',
    component: 'Select',
    field: 'BigProduct',
    componentProps: () => {
      return {
        options: optionsdata.BigProductData1,
        placeholder: 'big product',
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.ProductData1 = optionsdata.ProductData.filter(item => item?.value.indexOf(e) != -1)
          } else {
            optionsdata.ProductData1 = optionsdata.ProductData
          }
        }
      };
    }

  },
  {
    label: 'Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
      };
    }
  }, */
  {
    label: 'PROD ID',
    component: 'InputTextArea',
    field: 'ProdId',
    helpMessage: '手動輸入',
},
  {
    label: 'common.commonpages.LCMOwner',
    component: 'Select',
    field: 'LcmOwner',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.OwnerName
      }
    }
  },
  {
    label: 'LCD Grade',
    component: 'Select',
    field: 'LcdGrade',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.LcdGradeLabel
      }
    },
  },
]

}
 