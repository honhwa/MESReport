import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { Grade, LCMGrade, LCMProductType, Owner, Size } from '@/pages/ReportPortal/Static/common';
const { mountedWorkOrderInformation } = useRemoteSelect();

export async function getFactoryType(FactoryType: string) {
  await mountedWorkOrderInformation(
    optionsdata,
    FactoryType,
    optionsdata['FromDate'],
    optionsdata['ToDate'],
  );
}
export let optionsdata: any = reactive({
  FactoryType: '',
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  LcmProdTypeData: [],
  OwnerName: [],
  LcdGradeLabel: [],
  LCMGradeData: [],
  WorkOrderInformation: [],
  FromDate: formatToDate(),
  ToDate: formatToDate(),
});
let optinondata = reactive({});
let optinondata2 = reactive({});
let optinondata3 = reactive({});
export const schemas = (FactoryType: string) => {
  return [
    {
      label: 'common.commonpages.Date',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
        onChange: async (e: any) => {
          optionsdata['FromDate'] = e;
        },
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'common.commonpages.To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
        onChange: async (e: any) => {
          optionsdata['ToDate'] = e;
        },
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: 'common.commonpages.Shift',
      component: 'RadioGroup',
      field: 'Shift',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '',
          },
          {
            label: '日(A)',
            value: 'A',
          },
          {
            label: '夜(B)',
            value: 'B',
          },
        ],
      },
      defaultValue: '',
    },
    {
      label: 'common.commonpages.Interval',
      component: 'RadioGroup',
      field: 'Interval',
      componentProps: {
        options: [
          {
            label: 'Shift',
            value: 'shift',
          },
          {
            label: 'Day',
            value: 'day',
          },
          {
            label: 'Week',
            value: 'week',
          },
          {
            label: 'Month',
            value: 'month',
          },
        ],
      },
      defaultValue: 'shift',
    },
    {
      label: 'common.commonpages.Floor',
      component: 'RadioGroup',
      field: 'Floor',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '',
          },
          {
            label: '1F',
            value: " and floor='1'",
          },
          {
            label: '2F',
            value: " and floor='2'",
          },
          {
            label: '3F',
            value: " and floor='3'",
          },
        ],
      },
      defaultValue: '',
    },
    {
      label: '查詢方式',
      component: 'RadioGroup',
      field: 'QueryMethod',
      componentProps: {
        options: [
          {
            label: 'By工單',
            value: '1',
          },
        ],
      },
      defaultValue: '1',
    },
    {
      label: 'LCM Product Area',
      component: 'RadioGroup',
      field: 'LCMProductArea',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: 'ALL',
          },
          {
            label: 'Moudle',
            value: 'Moudle',
          },
          {
            label: 'SKD',
            value: 'SKD',
          },
          {
            label: 'Down Grade',
            value: '',
          },
        ],
      },
      defaultValue: 'ALL',
    },
    {
      label: '產品型態',
      component: 'Select',
      field: 'ProductType',
      componentProps: () => {
        return {
          options: [
            {
              label: 'ALL',
              value: '',
            },
            {
              label: '量產品',
              value: " and fg_type='P'",
            },
            {
              label: '工程品',
              value: " and fg_type='E'",
            },
            {
              label: 'Resorting',
              value: " and fg_type='R'",
            },
            {
              label: 'RMA',
              value: " and fg_type='M'",
            },
          ],
        };
      },
      defaultValue: '',
    },
    ...new LCMProductType(optinondata, FactoryType, 'LCMProductType').obj,
    {
      label: '工單型態',
      component: 'CheckboxGroup',
      field: 'WorkOrderType',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '1=1',
          },
          {
            label: 'HA',
            value: 'HA',
          },
          {
            label: 'MP',
            value: 'MP',
          },
          {
            label: 'ME',
            value: 'ME',
          },
          {
            label: 'TR',
            value: 'TR',
          },
          {
            label: 'RK',
            value: 'RK',
          },
          {
            label: 'RY',
            value: 'RY',
          },
          {
            label: 'OA',
            value: 'OA',
          },
          {
            label: 'TV',
            value: 'TV',
          },
          {
            label: 'RA',
            value: 'RA',
          },
        ],
      },
      defaultValue: ['1=1'],
    },
    {
      label: '工單信息 ',
      component: 'Select',
      field: 'WorkOrderInformation',
      componentProps: () => {
        return {
          options: optionsdata.WorkOrderInformation,
          mode: 'multiple',
          onDropdownVisibleChange: async (e: any) => {
            getFactoryType(FactoryType);
          },
        };
      },
      defaultValue: [],
    },
    ...new Size(optinondata, FactoryType, {
      size: 'Size',
      bigproduct: 'bigProduct',
      optionproduct: 'OptionProduct',
      selectmenu: '#optionmenu',
      product: 'Product',
    }).obj,
   
    ...new Owner(optinondata, FactoryType, 'LCMOwner').obj,
    
    ...new Grade(optinondata2, FactoryType, 'LCDGrade').obj,
   
    ...new LCMGrade(optinondata3, FactoryType, 'LCMGrade',"LCMGradeData").obj,
    {
      label: 'Pallet ID',
      component: 'InputTextArea',
      field: 'PalletID',
    },
  ];
};
