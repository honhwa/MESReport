import type { FormSchema } from '@/components/core/schema-form/';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { FSType, LCMProductType, EDCLineid, EDCUnit } from '@/pages/ReportPortal/Static/common';
import dayjs from 'dayjs';
import { Equipment } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
    EDCLineid: [],
    EDCUnit: [],
    LCMProductType: [],
    EquipmentData: [],
    BigEquipmentData: [],
    BigEquipmentData1: []
});

let optiondata2 = reactive({});
export const schemas = (FactoryType:string) => {
    return [
        {
            label: 'Date/Time',
            field: 'FromDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: '开始时间',
            field: 'FromTime',
            component: 'TimePicker',
            componentProps: {
                minuteStep: 30,
                format: 'HH:mm'
            },
            defaultValue: dayjs('08:00', 'HH:mm'),
            required: true
        },
        {
            label: 'To',
            field: 'ToDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: '结束时间',
            field: 'ToTime',
            component: 'TimePicker',
            componentProps: {
                minuteStep: 30,
                format: 'HH:mm'
            },
            defaultValue: dayjs('08:00', 'HH:mm'),
            required: true
        },

        //...new LCMProductType(optionsdata, 'LCM1', 'ProdType').obj,
        ...new EDCLineid(optionsdata, 'LCM1', 'EDCLineid').obj,
        ...new EDCUnit(optionsdata, 'LCM1', 'EDCUnit').obj,
        {
            //label: '請輸入計劃版本',
            label: 'PANEL ID:',
            component: 'InputTextArea',
            field: 'wo_nbr'
        }
    ];
};
let FactoryType = 'LCM1';
export function pushFactoryType(sFactoryType: string) {
    FactoryType = sFactoryType;
}
