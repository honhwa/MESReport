import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Operation, Size } from '@/pages/ReportPortal/Static/common';
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
        ...new Size(optionsdata, FactoryType, {
            size: 'Size',
            bigproduct: 'Product',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Prod_nbr'
        }).getSize(),
        ...new Operation(optionsdata, 'LCM1',"Oper").obj,
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
