import type { FormSchema } from '@/components/core/schema-form/';
import { formatToDate } from '@/utils/dateUtil';
import { reactive } from 'vue';
export let optionsdata: any = reactive({
    Rework: [
        { label: '是', value: 'Y' },
        { label: '否', value: 'N' },
        { label: 'ALL', value: 'ALL' }
    ],
    Shift: [
        {
            label: 'ALL',
            value: 'ALL'
        },
        {
            label: '日(A)',
            value: 'A'
        },
        {
            label: '夜(B)',
            value: 'B'
        }
    ],
    Operation: [
        { label: 'ASSY', value: 'ASSY' },
        { label: 'CD-', value: 'CD-' },
        { label: 'BONDING(Input)', value: 'BOND' },
        { label: 'FOG', value: 'FOG' },
        { label: 'LAM', value: 'LAM' },
        { label: 'SHIPPING(Output)', value: 'SHIP' }
    ],
    PlanType: [
        { label: 'ALL', value: 'ALL' },
        { label: 'MPS', value: 'MPS' },
        { label: 'MPC', value: 'MPC' }
    ],
    Behind: [
        { label: 'ALL', value: 'ALL' },
        { label: '落後(負值) ', value: 'behind' }
    ],
    FGType: [
        { label: 'ALL', value: 'ALL' },
        { label: 'E', value: 'E' },
        { label: 'R', value: 'R' },
        { label: 'P', value: 'P' }
    ],
    TN_PlanVersion: '',
    TN_SizeData: [{ label: 'ALL', value: 'ALL' }],
    TN_ProductData: [],
    TN_BigProductData: [],
    TN_ProductData1: [],
    TN_BigProductData1: [],
    TN_LCMOwner: []
});

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
    },
    {
        label: 'common.commonpages.Shift',
        component: 'RadioGroup',
        field: 'Shift',
        componentProps: () => {
            return {
                options: optionsdata.Shift
            };
        },
        defaultValue: 'ALL'
    },
    {
        label: 'Rework',
        component: 'RadioGroup',
        field: 'Rework',
        componentProps: () => {
            return {
                options: optionsdata.Rework
            };
        },
        defaultValue: 'ALL'
    },
    {
        label: 'Operation',
        component: 'RadioGroup',
        field: 'Operation',
        componentProps: () => {
            return {
                options: optionsdata.Operation
            };
        },
        defaultValue: 'ASSY'
    },
    {
        label: 'MPS or MPC',
        component: 'RadioGroup',
        field: 'PlanType',
        componentProps: () => {
            return {
                options: optionsdata.PlanType
            };
        },
        defaultValue: 'MPS'
    },
    {
        label: '落後超前',
        component: 'RadioGroup',
        field: 'Behind',
        componentProps: () => {
            return {
                options: optionsdata.Behind
            };
        },
        defaultValue: 'ALL'
    },
    {
        label: '工單型態',
        component: 'CheckboxGroup',
        field: 'FGType',
        componentProps: () => {
            return {
                options: optionsdata.FGType
            };
        },
        defaultValue: ['ALL']
    },
    {
        label: 'Plan版本',
        component: 'Input',
        field: 'PlanVersion',
        componentProps: () => {
            return {
                disabled: true,
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
                placeholder: 'size',
                onChange: async (e: any) => {
                    if (e !== 'ALL') {
                        optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData.filter(
                            (item) => item?.value.indexOf(e) != -1
                        );
                    } else {
                        optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData;
                    }
                }
            };
        }
    },
    {
        label: 'common.commonpages.bigproduct',
        component: 'Select',
        field: 'Product',
        componentProps: () => {
            return {
                options: optionsdata.TN_BigProductData1,
                placeholder: 'big product',
                onChange: async (e: any) => {
                    if (e !== 'ALL') {
                        optionsdata.TN_ProductData1 = optionsdata.TN_ProductData.filter(
                            (item) => item?.value.indexOf(e) != -1
                        );
                    } else {
                        optionsdata.TN_ProductData1 = optionsdata.TN_ProductData;
                    }
                }
            };
        }
    },
    {
        label: 'common.commonpages.Product',
        field: 'prod_nbr',
        component: 'Select',
        componentProps: () => {
            return {
                mode: 'multiple',
                options: optionsdata.TN_ProductData1,
                placeholder: 'product'
            };
        }
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
            return {};
        }
    }
];
