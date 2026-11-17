import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { reactive } from 'vue';
export let optionsdata: any = reactive({
    Rework: [
        { label: '是', value: 'Y' },
        { label: '否', value: 'N' },
        { label: 'ALL', value: 'ALL' }
    ],
    PlanType:[
        { label: 'MPS', value: 'MPS' },
        { label: 'MPC', value: 'MPC' },
    ],
    Behind:[
        { label: 'ALL', value: 'ALL' },
        { label: '落後(負值) ', value: 'behind' },
    ],
    FGType:[
        { label: 'ALL', value: 'ALL' },
        { label: 'E', value: 'E' },
        { label: 'R', value: 'R' },
        { label: 'P', value: 'P' },
    ],
    TN_LCMOwner: []
});

export const schemas: FormSchema[] = [
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
        label: 'Rework',
        component: 'RadioGroup',
        field: 'Rework',
        componentProps: () => {
            return {
                options: optionsdata.Rework,
            };
        },
        defaultValue: "N"
    },
    {
        label: 'MPS or MPC',
        component: 'RadioGroup',
        field: 'PlanType',
        componentProps: () => {
            return {
                options: optionsdata.PlanType,
            };
        },
        defaultValue: "MPS"
    },
    {
        label: '落後超前',
        component: 'RadioGroup',
        field: 'Behind',
        componentProps: () => {
            return {
                options: optionsdata.Behind,
            };
        },
        defaultValue: "ALL"
    },
    {
        label: '工單型態',
        component: 'CheckboxGroup',
        field: 'FGType',
        componentProps: () => {
            return {
                options: optionsdata.FGType,
            };
        },
        defaultValue: ["ALL"]
    },
    {
        label: 'common.commonpages.LCMOwner',
        component: 'Select',
        field: 'LCMOwner',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_LCMOwner,
                placeholder: 'lcm_owner'
            };
        }
    },
    {
        label: 'include V Order ',
        component: 'Checkbox',
        field: 'IncludeVGrade',
        componentProps: () => {
            return {
            };
        }
    }
];
