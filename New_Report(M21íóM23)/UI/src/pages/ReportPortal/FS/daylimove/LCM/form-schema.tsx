import type { FormSchema } from '@/components/core/schema-form/';
import { reactive, ref } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { Operation, Owner, Size, Size2 } from '@/pages/ReportPortal/Static/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const { mountedowner, mountedOper } = useRemoteSelect();
// 每个组件都会生成一个optiondata
/* function create(params:type) {
    
} */
/* export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    OwnerName: [],
    OperationLabel: []
}); */
/* await mountedowner(optionsdata, UseParamsAdd.Fab);
await mountedOper(optionsdata, UseParamsAdd.Fab); */
class getmount {
    constructor(FactoryType: string, optionsdata: Object) {
        this.init(FactoryType, optionsdata);
        console.log("输出optionsdata",optionsdata);
        
    }
    async init(FactoryType: string, optionsdata: Object) {
        await mountedowner(optionsdata, FactoryType);
        await mountedOper(optionsdata, FactoryType);
    }
}
function handlesize(FactoryType: string, optionsdata: Object) {
    if (['BLU1', 'BLU2'].includes(FactoryType)) {
        return new Size2(optionsdata, FactoryType).getSize();
    } else {
        return new Size(optionsdata, FactoryType,{
            size: 'Size',
            bigproduct: 'Product',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'prod_nbr'
        }).getSize();
    }
}
function handlelabel(FactoryType: string) {
    let label = '';
    switch (FactoryType) {
        case 'LCM1':
        case 'LCM2':
        case 'SET1':
        case 'SET2':
            label = 'common.commonpages.LCMOwner';
            break;
        case 'BLU1':
        case 'BLU2':
            label = 'Owner';
            break;
        default:
            label = 'common.commonpages.LCMOwner';
            break;
    }
    return label;
}
export const schemas = (FactoryType: string) => {
    let optionsdata = reactive({});
    // new getmount(FactoryType,optionsdata);
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
            label: 'common.commonpages.WorkOrderLike',
            component: 'Input',
            field: 'Wo_nbr'
        },
        {
            label: 'common.commonpages.WorkOrder',
            component: 'InputTextArea',
            field: 'wo_num'
            // helpMessage: '使用逗號分割',
        },
        ...handlesize(FactoryType, optionsdata),
        ...new Owner(optionsdata,FactoryType,'owner_code').obj,
       /*  {
            label: handlelabel(FactoryType),
            component: 'Select',
            field: 'owner_code',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata['OwnerName']
                };
            }
        }, */
        /* {
            label: 'common.commonpages.Operation',
            component: 'Select',
            field: 'workGroup',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata['OperationLabel'],
                    dropdownMatchSelectWidth:false
                };
            }
        }, */
        ...new Operation(optionsdata,FactoryType,'workGroup').obj
    ];
};
