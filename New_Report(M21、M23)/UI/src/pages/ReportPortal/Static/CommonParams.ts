import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { optionsdata } from '@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema';
//LCM Owner
export const OwnerName = [
  'ALL',
  'GRN0',
  'DGDB',
  'DGDY',
  'INT0',
  'LCD0',
  'LCM0',
  'PQOY',
  'PRMA',
  'PROD',
  'PROY',
  'OTAP',
  'RES0',
  'RMA0',
  'RMB0',
  'RMC0',
  'RSA0',
  'RTV0',
  'RWDG',
  'RWK0',
  'RWKY',
  'RWPY',
  'TEST',
  'TVP0'
];
export const LCMOwner = OwnerName.map(item => { return { label: item, value: item } })
//LCDGrade
export const LCDGradeOptions = ['ALL', '1B', '1Y', 'B', 'C6', 'D0', 'G0', 'L', 'N', 'D', 'F', 'M', 'A',
  'C',
  'C1',
  'K',
  'E',
  '3N',
  'NA',
  'NC',
  'NM',
  'NN',]
export const LCDGrade301 = LCDGradeOptions.map(item => { return { label: item, value: item } })
//PanelSourceOptions
export let PanelSourceOptions = ['ALL',
  'LCD1',
  'LCD2',
  'LCD3',
  'LCD4',
  'LCD5',
  'LCD6',
  'LCD7',
  'LCD8',
  'LCDM',
  'HDA1',
  'NHAP',
  'NHAL',
  'NLC3',
  'NHBD',]
//PanelSource
export let PanelSourceParams: FormSchema = {
  label: 'common.commonpages.PanelSource',
  component: 'Select',
  field: 'PanelSource',
  componentProps: {
    options: PanelSourceOptions.map(item => { return { label: item, value: item } }),
  },
  defaultValue: 'ALL'
}

//LCDGrade
export let LCDGradeParams: FormSchema = {
  field: 'LCDGRADE',
  label: 'LCD Grade',
  component: 'Select',
  componentProps: ({ formModel, field }) => {
    return {
      mode: 'multiple',
      options: LCDGrade301,
      onSelect: (e: any) => {
        if (e != 'ALL') {
          formModel[field] = formModel[field].filter((item) => item != 'ALL');
        } else {
          formModel[field] = LCDGrade301.filter((item) => item.label != 'ALL');
        }
      }
    }
  },
  defaultValue: ['ALL']
}
export let LCDGrade1Params: FormSchema = {
  field: 'LCDGrade',//大小寫後續由api定義統一
  label: 'LCD Grade',
  component: 'Select',
  componentProps: ({ formModel, field }) => {
    return {
      mode: 'multiple',
      options: LCDGrade301,
      onSelect: (e: any) => {
        if (e != 'ALL') {
          formModel[field] = formModel[field].filter((item) => item != 'ALL');
        } else {
          formModel[field] = LCDGrade301.filter((item) => item.label != 'ALL');
        }
      }
    }

  },
  defaultValue: ['ALL']
}

export const LCMOwnerParams: FormSchema = {
  label: 'common.commonpages.LCMOwner',
  component: 'Select',
  field: 'LCMOwner',
  componentProps: ({ formModel, field }) => {
    return {
      mode: 'multiple',
      options: LCMOwner,
      onSelect: (e: any) => {
        if (e != 'ALL') {
          formModel[field] = formModel[field].filter((item) => item != 'ALL');
        } else {
          formModel[field] = LCMOwner.filter((item) => item.label != 'ALL');
        }
      }
    }

  },
}

export const ShiftParams: FormSchema = {
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
        value: "A"
      },
      {
        label: '夜(B)',
        value: 'B'
      }
    ],
  },
  defaultValue: ''

}
export const ShiftParams1: FormSchema = {
  label: 'common.commonpages.Shift',
  component: 'RadioGroup',
  field: 'Shift',

  componentProps: {
    options: [
      {
        label: 'ALL',
        value: 'ALL'
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
  defaultValue: 'ALL'

}
export const ShiftParams2: FormSchema = {
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

}

export const StartDateParams: FormSchema = {
  label: 'common.commonpages.Date',
  component: 'DatePicker',
  field: 'FromDate',

  componentProps: {
    valueFormat: 'YYYY-MM-DD'
  },
  defaultValue: formatToDate(),
}
export const EndDateParams: FormSchema = {
  label: 'common.commonpages.To',
  component: 'DatePicker',
  field: 'ToDate',

  componentProps: {
    valueFormat: 'YYYY-MM-DD'
  },
  defaultValue: formatToDate(),
}

export const LcmProductParams: FormSchema[] = [
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
        },
      };
    },
    LabelStyle: { color: 'blue' },
  },
  {
    label: 'common.commonpages.bigproduct',
    component: 'Select',
    field: 'bigproduct',


    componentProps: () => {
      return {
        options: optionsdata.BigProductData1,
        placeholder: 'bigproduct',
        onChange: async (e: any) => {
          let Data: string[] = []
          if (!e.includes('ALL')) {
            e.forEach((product: string) => {
              let ProductData = optionsdata.ProductData.filter(item => item?.value.indexOf(product) != -1)
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
    defaultValue: [],
    LabelStyle: { color: 'blue' },

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
    defaultValue: ['ALL'],
    LabelStyle: { color: 'blue' },
  },
]
export const LcdProductParams: FormSchema[] = [
  {
    label: 'common.commonpages.size',
    component: 'Select',
    field: 'LCDSize',
    LabelStyle: { color: 'blue' },

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
    LabelStyle: { color: 'blue' },
    componentProps: () => {
      return {
        mode: 'multiple',
        options: optionsdata.LCDProductData1,
        placeholder: 'product',
        maxTagCount: 8,
      };
    },
    defaultValue: ['ALL']
  },
]
export const OperationParams: FormSchema = {
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
export const EquipmentParams: FormSchema = {
  label: 'common.commonpages.Equipment',
  component: 'Select',
  field: 'Equipment',
  componentProps: () => {
    return {
      mode: 'multiple',
      options: optionsdata.Equipment
    }
  },
  defaultValue: ['ALL']
}
export const LCMProductSize1Params: FormSchema = {
  label: 'common.commonpages.LCMProductSize',
  component: 'Select',
  field: 'LCMProductSize1',
  componentProps: () => {
    return {
      options: optionsdata.LCMProductSize
    }
  },
  defaultValue: 'ALL'
}
export const LCMProductSizeParams: FormSchema = {
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
export const LCMProductTypeParams: FormSchema = {
  label: 'common.commonpages.LCMProductType',
  component: 'Select',
  field: 'LCMProductType',
  componentProps: () => {
    return {
      options: optionsdata.LCMProductType
    }
  },
  defaultValue: 'ALL'
}
export const FloorParams: FormSchema = {
  label: 'common.commonpages.Floor',
  component: 'RadioGroup',
  field: 'Floor',
  componentProps: {
    options: [{ label: 'ALL', value: 'ALL' }, { label: '1F', value: '1' },
    { label: '2F', value: '2' }, { label: '3F', value: '3' },]
  },
  defaultValue: 'ALL'
}
export const FloorParams1: FormSchema = {//後端應統一
  label: 'common.commonpages.Floor',
  component: 'RadioGroup',
  field: 'Floor',
  componentProps: {
    options: [{ label: 'ALL', value: '' }, { label: '1F', value: '1' },
    { label: '2F', value: '2' }, { label: '3F', value: '3' },]
  },
  defaultValue: ''
}
export const FloorParams2: FormSchema = {
  label: 'Floor',
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
        value: " and floor='1'"
      },
      {
        label: '2F',
        value: " and floor='2'"
      },
      {
        label: '3F',
        value: " and floor='3'"
      }
    ],
  },
  defaultValue: ''
}

export const ReworkedParams: FormSchema = {
  label: 'common.commonpages.Reworked',
  component: 'Select',
  field: 'Reworked',
  componentProps: {
    options: [
      {
        label: 'ALL',
        value: ''
      },
      {
        label: '1次',
        value: "1"
      },
      {
        label: '1次以上',
        value: "over_1"
      },
      {
        label: '2次',
        value: "2"
      },
      {
        label: '2次以上',
        value: "over_2"
      },
      {
        label: '3次',
        value: "3"
      },
      {
        label: 'No',
        value: "N"
      },
    ],
  },
  defaultValue: 'ALL'
}