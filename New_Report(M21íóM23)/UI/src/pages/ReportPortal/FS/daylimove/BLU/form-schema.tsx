import type { FormSchema } from '@/components/core/schema-form/';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Operation, Owner, Size, Size2 } from '@/pages/ReportPortal/Static/common';

function handlesize(FactoryType: string, optionsdata: Object) {
    if (['BLU1', 'BLU2'].includes(FactoryType)) {
        return new Size2(optionsdata, FactoryType).getSize();
    } else {
        return new Size(optionsdata, FactoryType).getSize();
    }
}

export const schemas = (FactoryType: string) => {
    let optionsdata = reactive({});
    return [
        {
            field: 'vDate_s',
            component: 'DatePicker',
            label: 'common.commonpages.Date',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            field: 'vDate_e1',
            component: 'DatePicker',
            label: 'common.commonpages.To',

            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
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
                        value: '日'
                    },
                    {
                        label: '夜(B)',
                        value: '夜'
                    }
                ]
            },
            defaultValue: 'ALL'
        },
       /*  {
            label: 'common.commonpages.Floor',
            component: 'RadioGroup',
            field: 'floor',
            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: ''
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
                ]
            },
            defaultValue: ''
        }, */
        {
            label: 'common.commonpages.Interval',
            component: 'RadioGroup',
            field: 'intercond',

            componentProps: {
                options: [
                    {
                        label: 'hour',
                        value: 'hour'
                    },
                    {
                        label: 'day',
                        value: 'day'
                    }
                ]
            },
            defaultValue: 'day'
        },
        {
            label: 'common.commonpages.WorkOrder',
            component: 'Input',
            field: 'Wo_nbr'
        },
        ...handlesize(FactoryType, optionsdata),
        ...new Owner(optionsdata,FactoryType,'owner_code').obj,
        ...new Operation(optionsdata,FactoryType,'workGroup').obj
    ];
};
