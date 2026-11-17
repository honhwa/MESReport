import type { FormSchema } from '@/components/core/schema-form/';
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
import { formatToDate } from '@/utils/dateUtil';


export const schemas: FormSchema[] = [
  {
    label: 'common.commonpages.Date',
    component: 'DatePicker',
    field: 'FromDate',

    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
  },
  {
    label: 'common.commonpages.To',
    component: 'DatePicker',
    field: 'ToDate',

    componentProps: {
      valueFormat: 'YYYY-MM-DD'
    },
    defaultValue: formatToDate(),
  },
  {
    label: 'common.commonpages.Shift',//i18n
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
          value: "A"
        },
        {
          label: '夜(B)',
          value: 'B'
        }
      ],
    },
    defaultValue: ''
  },
  {
    label: 'common.commonpages.ReportType',
    component: 'RadioGroup',
    field: 'ReportType',
    componentProps: {
      options: [
        {
          label: 'By Total',
          value: ''
        },
        {
          label: 'By Detail',
          value: "MoveOutTimeDataSetDetail"
        },
      ],
    },
    defaultValue: ''
  },
  {label: 'common.commonpages.Operation',
  component: 'Select',
  field: 'Operation',
  componentProps: () => {
    return {
      options: optionsdata.OperationLabel
    }
  },
  defaultValue: '1100'},
  {
    label: 'common.commonpages.Equipment',
    component: 'Select',
    field: 'BigEquipment',
    componentProps: () => {
      return {
        options: optionsdata.EquipmentBig,
        onChange: async (e: any) => {
          if (e !== 'ALL') {
            optionsdata.Equipment1 = optionsdata.Equipment.filter(item => item?.value.indexOf(e) != -1)
          } else {
            optionsdata.Equipment1 = optionsdata.Equipment
          }
        }
      }
    },
    defaultValue: 'ALL'
  },
  {
    // label: '23ds',
    component: 'Select',
    field: 'Equipment',
    componentProps: () => {
      return {
        options: optionsdata.Equipment1,

      }
    },
    defaultValue: 'ALL'
  },
  {
    label: 'common.commonpages.size',
    component: 'Select',
    field: 'Size',


    componentProps: (model) => {
      return {
        options: optionsdata.SizeData,
        placeholder: 'size',
        onChange: async (e: any) => {

          if (e !== 'ALL') {
            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1);
          } else {
            optionsdata.BigProductData1 = optionsdata.BigProductData
          }

        },
        onSelect: async (e: any) => {
          model.formModel.bigproduct = Array.from(new Set(optionsdata.BigProductData1.map(item => item.value)))[0]
        }
      };
    }
  },
  {
    label: 'common.commonpages.bigproduct',
    component: 'Select',
    field: 'bigproduct',


    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.BigProductData1,
        placeholder: 'bigproduct',
        onChange: async (e: any) => {
          let Data:string[] = []
          if (!e.includes('ALL')) {
             e.forEach((product: string) => {
              let ProductData  =  optionsdata.ProductData.filter(item => item?.value.indexOf(product) != -1)
              Data.push(...ProductData)
            })
            optionsdata.ProductData1 = Data
          } else {
            optionsdata.ProductData1 = optionsdata.ProductData
          }
        },
        // onSelect: async (e: any) => {
        //   model.formModel.Product = Array.from(new Set(optionsdata.ProductData1.map(item => item.value)))
        // }
      };
    },
    defaultValue: []


  },
  {
    label: 'common.commonpages.Product',
    field: 'Product',
    component: 'Select',
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.ProductData1,
        placeholder: 'product',
        maxTagCount: 8,
      };
    },
    defaultValue: ['ALL']
  },
];

