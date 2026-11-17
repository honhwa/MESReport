import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { reactive } from 'vue';
export let optionsdata: any = reactive({
    Category: [
        {
            label: '產品資訊 ',
            value: 'PRODINFO'
        },
        {
            label: '可出貨GRADE',
            value: 'PRODGRADE'
        },
        {
            label: '每日Target',
            value: 'DAILYTART'
        }
    ]
});
export const schemas: FormSchema[] = [
    {
        label: 'Category',
        component: 'RadioGroup',
        field: 'Category',
        componentProps: () => {
            return {
                options: optionsdata.Category,
            };
        },
        defaultValue: 'PRODINFO'
    }
];
