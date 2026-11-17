import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { GetCommon } from '@/api/pages/common';
import { Operation, Size } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: []
});
export let OperationLabel: any = [];
export async function getOperaction(FactoryType: Object) {
    const {
        Date: { Table: Data }
    } = await GetCommon({
        name: 'Date',
        apiName: 'Operation',
        FactoryType: FactoryType,
        G_FAC: 'A'
    });
    OperationLabel = Data;
    let temp = OperationLabel.map((item) => {
        return {
            label: `${item['WORK_CTR']}-${item['WORK_NAME']}`,
            value: item['WORK_CTR']
        };
    });
    OperationLabel = [{ label: 'ALL', value: 'ALL' }].concat(temp);
    console.log('输出operaction:', OperationLabel);
}
export async function getSizeData(func: Function, params: String, funcparams: String, FactoryType: Object) {
    let ReturnData: any = [];

    let {
        Date: {
            Data: { Table: Data }
        }
    } = await func({
        G_FAC: 'A',
        name: 'Date',
        apiName: funcparams,
        FactoryType: FactoryType
    });

    for (let i in Data) {
        //@ts-ignore
        ReturnData.push({ label: Data[i][params], value: Data[i][params] });
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}
export const schemas = (FactoryType: string) => {
    let optionsdata: any = reactive({});
    return [
        {
            label: 'Facility',
            component: 'RadioGroup',
            field: 'oshop',
            componentProps: {
                options: [
                    {
                        label: 'LCMA',
                        value: 'A'
                    }
                ]
            },
            defaultValue: 'A'
        },
        {
            label: 'Panel Search List(MAX:1000)',
            component: 'InputTextArea',
            field: 'pnl_ids',
            labelWidth: 180,
            span: 16
        },
        {
            label: 'HIS Query Function',
            component: 'RadioGroup',
            field: 'query_func',
            componentProps: {
                options: [
                    {
                        label: 'oper(one/all)+panel ID',
                        value: 'panel'
                    },
                    {
                        label: 'oper(one)+prod+time',
                        value: 'prod'
                    }
                ]
            },
            labelWidth: 120,
            defaultValue: 'panel'
        },
        {
            label: 'common.commonpages.Date',
            field: 'vDate_s',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.To',
            field: 'vDate_e',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
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
              optionsdata.BigProductData1 = optionsdata.BigProductData.filter(item => item?.value.indexOf(e) != -1);
            } else {
              optionsdata.BigProductData1 = optionsdata.BigProductData
            }
  
          },
        };
      }
    },
    defaultValue: ["ALL"],
  },
  {
    label: 'Search Type',
    component: 'RadioGroup',
    field: 'query_type',
    componentProps: {
      options: [
        {
          label: 'WIP',
          value: 1
        },
        {
          label: 'HIS',
          value: 2
        },
      ]
    },
    {
      label: 'Product',
      field: 'Prod_nbr',
      component: 'Select',
      componentProps: () => {
        return {
          mode: 'multiple',
          options: optionsdata.ProductData1,
          placeholder: 'product',
        };
      },
      defaultValue: ["ALL"]
    }, */
        ...new Size(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'Product',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Prod_nbr'
        }).getSize(),
        ...new Operation(optionsdata, 'LCM1',"Oper").obj,
       /*  {
            label: 'Operation',
            component: 'Select',
            field: 'Oper',
            componentProps: () => {
                return {
                    options: OperationLabel,
                    mode: 'multiple',
                    placeholder: 'Operation'
                };
            },
            defaultValue: ['ALL']
        }, */
        {
            label: 'Search Type',
            component: 'RadioGroup',
            field: 'query_type',
            componentProps: {
                options: [
                    {
                        label: 'WIP',
                        value: 1
                    },
                    {
                        label: 'HIS',
                        value: 2
                    }
                ]
            },
            defaultValue: 1
        }
    ];
};
