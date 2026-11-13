import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import {Size, Owner} from '@/pages/ReportPortal/Static/TNCommonParams'



export const schemas = (FactoryType:string)=>{ 
    let optionsdata = reactive({});
    return [
        {
            field: 'Date',
            component: 'DatePicker',
            label: 'common.commonpages.Date',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.TN_Floor',
            component: 'RadioGroup',
            field: 'floor',

            componentProps: {
                options: [
                    {
                        label: 'ALL',
                        value: 'ALL'
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
            defaultValue: 'ALL'
        },
        {
            label: 'common.commonpages.WorkOrderLike',
            component: 'Input',
            field: 'Wo_nbr'
        },
        ...new Size(optionsdata, FactoryType).obj,
        ...new Owner(optionsdata,FactoryType,'owner_code').obj,
    ];
};