import type { FormSchema } from '@/components/core/schema-form/';
import { Size } from '@/pages/ReportPortal/Static/common';
import { reactive } from 'vue';

export const schemas = (FactoryType: string) => {
    let optionsdata: any = reactive({});
    return [
        ...new Size(optionsdata, FactoryType).getSize(),
        {
            label: 'common.commonpages.Password',
            component: 'InputPassword',
            field: 'inputpassword',
            componentProps: {
                autocomplete: 'off',
                type: 'password'
            }
        }
    ];
};
