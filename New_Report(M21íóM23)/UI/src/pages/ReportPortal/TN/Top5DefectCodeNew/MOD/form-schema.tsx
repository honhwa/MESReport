import type { FormSchema } from '@/components/core/schema-form/';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil';

export let optionsdata: any = reactive({
    Interval: [
        { label: 'Date', value: 'Date' },
        { label: 'Week', value: 'Week' },
        { label: 'Month', value: 'Month' }
    ],
    TN_SizeData: [{ label: 'ALL', value: 'ALL' }],
    TN_LCMOwner: []
});

export const schemas: FormSchema[] = [
    {
        label: 'Interval',
        component: 'RadioGroup',
        field: 'Interval',
        componentProps: () => {
            return {
                options: optionsdata.Interval
            };
        },
        defaultValue: 'Date'
    },
    {
        field: 'sDate',
        component: 'DatePicker',
        label: 'common.commonpages.Date',
        componentProps: {
            valueFormat: 'YYYY-MM-DD'
        },
        defaultValue: formatToDate(),
        required: true,
        vShow: ({ formModel }) => {
            return formModel.Interval != 'Date';
        }
    },
    {
        field: 'eDate',
        component: 'DatePicker',
        label: 'common.commonpages.To',
        componentProps: {
            valueFormat: 'YYYY-MM-DD'
        },
        defaultValue: formatToDate(),
        required: true
    },
    {
        label: 'common.commonpages.LCMOwner',
        component: 'TreeSelect',
        field: 'LCMOwner',
        componentProps: () => {
            return {
                fieldNames: {
                    label: 'title',
                    value: 'value'
                },
                treeData: optionsdata.TN_LCMOwner,
                multiple: true,
                placeholder: 'lcm_owner'
            };
        }
    },
    {
        label: 'common.commonpages.size',
        component: 'Select',
        field: 'Size',
        componentProps: () => {
            return {
                options: optionsdata.TN_SizeData,
                placeholder: 'size'
            };
        },
        defaultValue: 'ALL'
    }
];
