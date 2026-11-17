import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import dayjs from 'dayjs';
import { Size, Operation } from '@/pages/ReportPortal/Static/common';

// 定义从后端返回的下拉选择框的数据
export let optionsdata: any = reactive({
  LcmProdTypeData: [],
  ProductData: [],
  ProductData1: [],
  SizeData: [],
  BigProductData: [],
  BigProductData1: [],
  OperationLabel: [],
});

export const schemas = (FactoryType: string) => {
  let optiondata = reactive({});
  return [
    {
      label: 'Date/Time',
      field: 'FromDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: '开始时间',
      field: 'FromTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm',
      },
      defaultValue: dayjs('08:00', 'HH:mm'),
      required: true,
    },
    {
      label: 'To',
      field: 'ToDate',
      component: 'DatePicker',
      componentProps: {
        valueFormat: 'YYYY-MM-DD',
      },
      defaultValue: formatToDate(),
      required: true,
    },
    {
      label: '结束时间',
      field: 'ToTime',
      component: 'TimePicker',
      componentProps: {
        minuteStep: 30,
        format: 'HH:mm',
      },
      defaultValue: dayjs('08:00', 'HH:mm'),
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
            value: "and shift_id='A'",
          },
          {
            label: '夜(B)',
            value: "and shift_id='B'",
          },
        ],
      },
      defaultValue: '',
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
      label: 'common.commonpages.Interval',
      component: 'RadioGroup',
      field: 'Interval',
      componentProps: {
        options: [
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
      defaultValue: 'day',
    },
    {
      label: 'Owner',
      component: 'RadioGroup',
      field: 'Owner',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '',
          },
          {
            label: 'P',
            value: '1',
          },
          {
            label: 'E',
            value: '2',
          },
          {
            label: 'P->E',
            value: '3',
          },
          {
            label: 'E->P',
            value: '4',
          },
        ],
      },
      defaultValue: '',
    },
    {
      label: '轉換形式',
      component: 'RadioGroup',
      field: 'ConversionForm',
      componentProps: {
        options: [
          {
            label: 'ALL',
            value: '',
          },
          {
            label: 'MOD→Open Cell',
            value: " and substr(prod_nbr,1,1)='G'   and substr(to_prod_nbr,1,1)='2' ",
          },
          {
            label: 'MOD→MOD',
            value: " and substr(prod_nbr,1,1)='G'   and substr(to_prod_nbr,1,1)='G' ",
          },
          {
            label: 'Open Cell→MOD',
            value: " and substr(prod_nbr,1,1)='2'   and substr(to_prod_nbr,1,1)='G' ",
          },
          {
            label: 'Open Cell→Open Cell',
            value: " and substr(prod_nbr,1,1)='2'   and substr(to_prod_nbr,1,1)='2' ",
          },
        ],
      },
      defaultValue: '',
    },
    ...new Size(optiondata, FactoryType, {
      size: 'Size',
      bigproduct: 'bigProduct',
      optionproduct: 'OptionProduct',
      selectmenu: '#optionmenu',
      product: 'Product',
    }).obj,
   /*  {
      label: 'size',
      component: 'Select',
      field: 'Size',
      componentProps: () => {
        return {
          options: optionsdata.SizeData,
          placeholder: 'size',
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.BigProductData1 = optionsdata.BigProductData.filter(
                (item) => item?.value.indexOf(e) != -1,
              );
            } else {
              optionsdata.BigProductData1 = optionsdata.BigProductData;
            }
          },
        };
      },
      defaultValue: 'ALL',
    },
    {
      label: 'big product',
      component: 'Select',
      field: 'bigProduct',
      componentProps: () => {
        return {
          options: optionsdata.BigProductData1,
          placeholder: 'big product',
          onChange: async (e: any) => {
            if (e !== 'ALL') {
              optionsdata.ProductData1 = optionsdata.ProductData.filter(
                (item) => item?.value.indexOf(e) != -1,
              );
            } else {
              optionsdata.ProductData1 = optionsdata.ProductData;
            }
          },
        };
      },
      defaultValue: 'ALL',
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
      },
      defaultValue: ['ALL'],
    }, */
    ...new Operation(optiondata, FactoryType,"Operation").obj,
   /*  {
      label: 'Operation',
      component: 'Select',
      field: 'Operation',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.OperationLabel,
        };
      },
      defaultValue: ['ALL'],
    }, */
    {
      label: '顯示欄位',
      component: 'Select',
      field: 'ShowFields',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: [
            /* {
              label: 'Operation',
              value: 'WORK_CTR',
            }, */
            {
              label: '轉工單時間',
              value: 'TRANS_DATE',
            },
            {
              label: '線別',
              value: 'EQUIP_NBR',
            },
            {
              label: '樓層',
              value: 'FLOOR',
            },
            {
              label: 'Old owner',
              value: 'lcm_owner',
            },
            {
              label: 'New owner',
              value: 'to_lcm_owner',
            },
            {
              label: 'Old prod',
              value: 'prod_nbr',
            },
            {
              label: 'New prod',
              value: 'to_prod_nbr',
            },
            {
              label: 'Old work nbr',
              value: 'wo_nbr',
            },
            {
              label: 'New work nbr',
              value: 'to_wo_nbr',
            },
            {
              label: '等級',
              value: 'LCM_GRADE',
            },
          ],
        };
      },
    },
  ];
};
