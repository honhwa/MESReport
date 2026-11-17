import type { FormSchema } from '@/components/core/schema-form/';
import {
    ShiftParams2, FloorParams2
} from '@/pages/ReportPortal/Static/CommonParams'
import { Size, Equipment } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue'
import { formatToDateHour } from '@/utils/dateUtil';


export let optionsdata: any = reactive({
});
export const schemas: FormSchema[] = [
    {
        label: 'common.commonpages.Password',
        component: 'InputPassword',
        field: 'PassWord',
        componentProps: {
            autocomplete: 'off',
            type: 'password'
        }
    },
    {
        label: 'common.commonpages.Date',
        component: 'DatePicker',
        field: 'CompleteFromDate',

        componentProps: {
            format: 'YYYY-MM-DD HH:mm',
            showTime: true,
            minuteStep: 30,
            valueFormat: 'YYYY-MM-DD HH:mm'
        },
        defaultValue: formatToDateHour()
    },
    {
        label: 'common.commonpages.To',
        component: 'DatePicker',
        field: 'CompleteToDate',

        componentProps: {
            format: 'YYYY-MM-DD HH:mm',
            showTime: true,
            minuteStep: 30,
            valueFormat: 'YYYY-MM-DD HH:mm'
        },
        defaultValue: formatToDateHour(),
    },
    {
        label: 'common.commonpages.QueryType',
        component: 'RadioGroup',
        field: 'QType',
        componentProps: {
            options: [{ value: 'PROD_NBR', label: '機臺' }, { value: 'equip_nbr', label: '機種' }]
        },
        defaultValue: 'PROD_NBR'

    },
    {
        label: 'Operator',
        component: 'Input',
        field: 'Operator',

    },
    ShiftParams2,
    FloorParams2,

    {
        field: 'LCMDivider',
        component: 'Divider',
        label: 'LCM Product'
    },
    ...new Size(optionsdata, 'LCM1').getSize(),
    ...new Equipment(optionsdata, 'LCM1', 0).obj,
];



