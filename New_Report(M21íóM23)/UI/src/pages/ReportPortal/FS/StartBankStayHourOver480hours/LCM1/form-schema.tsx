import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { LCDOwner } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    OwnerName: [],
    LcmProdTypeData: []
});

export async function getSizeData(func: Function, params: String, funcparams: String) {
    let ReturnData: any = [];

    let {
        Date: {
            Data: { Table: Data }
        }
    } = await func({
        G_FAC: 'A',
        name: 'Date',
        apiName: funcparams
    });

    for (let i in Data) {
        //@ts-ignore
        ReturnData.push({ label: Data[i][params], value: Data[i][params] });
    }
    ReturnData.unshift({ label: 'ALL', value: 'ALL' });
    return ReturnData;
}
export const schemas = (FactoryType: string) => {
    return [
        {
            label: 'Start Hours',
            field: 'StartHour',
            component: 'Input',
            componentProps: () => {
                return {
                    // placeholder: '不能超過4個字符',
                    // maxlength:4,
                    // showCount:true
                };
            },
            componentSlots: () => {
                return {
                    prefix: () => 'over',
                    suffix: () => '小時'
                };
            },
            defaultValue: '480'
        },
        {
            label: '顯示Abnormal code:',
            component: 'RadioGroup',
            field: 'show_abn',
            componentProps: {
                options: [
                    {
                        label: 'NO',
                        value: 'N'
                    },
                    {
                        label: 'Yes',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            label: '查詢異常CST:',
            component: 'RadioGroup',
            field: 'show_uncst',
            componentProps: {
                options: [
                    {
                        label: 'NO',
                        value: 'N'
                    },
                    {
                        label: 'Yes',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        {
            label: 'OVER Q TIME:',
            component: 'RadioGroup',
            field: 'show_qtime',
            componentProps: {
                options: [
                    {
                        label: 'NO',
                        value: 'N'
                    },
                    {
                        label: 'Yes',
                        value: 'Y'
                    }
                ]
            },
            defaultValue: 'N'
        },
        ...new LCDOwner(optionsdata, FactoryType).obj,
        {
            label: 'Cassette Id',
            component: 'Input',
            field: 'cst_id'
        },
        {
            label: '實驗單申請人',
            component: 'Input',
            field: 'exp_engineer'
        },
        {
            label: '實驗申請單號',
            component: 'Input',
            field: 'exp_nbr'
        }
    ];
};
