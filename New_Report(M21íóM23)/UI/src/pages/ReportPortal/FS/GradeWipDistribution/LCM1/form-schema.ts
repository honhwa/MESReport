import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil'
import { Size } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
  SizeData: [{ label: 'ALL', value: 'ALL' }],
  ProductData: [],
  BigProductData: [],
  ProductData1: [],
  BigProductData1: [],
})
let OwnerName = [
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

export async function getSizeData(func: Function, params: String, funcparams: String) {
  let ReturnData: any = [];

  let { Date: { Data: { Table: Data } } } = await func({
    G_FAC: "A",
    name: "Date",
    apiName: funcparams,
    FactoryType: "LCM1",
  });

  for (let i in Data) {
    //@ts-ignore
    ReturnData.push({ label: Data[i][params], value: Data[i][params] });
  }
  ReturnData.unshift({ label: 'ALL', value: 'ALL' })
  return ReturnData

}
export const schemas = (Fab: string): FormSchema[] => {
  return [
    {
      field: 'vDate_s',
      component: 'DatePicker',
      label: 'Date',
      componentProps: {
        valueFormat: 'YYYY-MM-DD'
      },
      defaultValue: formatToDate(),
    },
    {
      label: 'Floor',
      component: 'RadioGroup',
      field: 'floor',

      componentProps: {
        options: [
          { label: 'ALL', value: '' },
          {
            label: '1F',
            value: "and floor ='1'"
          },
          {
            label: '2F',
            value: "and floor ='2'"
          },
          {
            label: '3F',
            value: "and floor ='3'"
          }
        ]
      },
      defaultValue: ''
    },
    ...new Size(optionsdata, Fab).getSize(),
    {
      label: 'LCM Owner',
      component: 'Select',
      field: 'owner_code',
      componentProps: ({ formModel, field }) => {
        return {
          mode: 'multiple',
          options: OwnerName.map(item => { return { label: item, value: item } }),
          onSelect: (e: any) => {
            if (e != 'ALL') {
              formModel[field] = formModel[field].filter((item) => item != 'ALL');
            } else {
              formModel[field] = ['ALL'];
            }
          }
        }
      },
      defaultValue: ['ALL']

    },
  ];

}