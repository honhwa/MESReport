import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';

export const schemas: FormSchema[] = [
    {
        field: 'sDate',
        component: 'DatePicker',
        label: 'common.commonpages.Date',
        componentProps: {
            valueFormat: 'YYYY-MM-DD'
        },
        defaultValue: formatToDate(),
        required: true
    },
    {
        field: 'eDate',
        component: 'DatePicker',
        label: 'common.commonpages.Date',
        componentProps: {
            valueFormat: 'YYYY-MM-DD'
        },
        defaultValue: formatToDate(),
        required: true
    }
];
