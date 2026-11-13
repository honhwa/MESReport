import type { FormSchema } from '@/components/core/schema-form/';
import { reactive } from 'vue';
import { Size, Date, Marterial ,Owner} from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({});

export const schemas = (FactoryType: string) => {
    return [
        ...new Date()._obj,
        ...new Marterial(optionsdata, FactoryType)._obj,
        // BLU2厰使用LCM2的size接口
        ...new Size(optionsdata, "LCM2").getSize(),
        ...new Owner(optionsdata, "LCM2")._obj,
    ];
};
