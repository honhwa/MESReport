import type { FormSchema } from '@/components/core/schema-form/';
// import { GetProduct, GetBigProduct } from '@/api/pages/daylimove';
// import { message } from 'ant-design-vue';
import { reactive } from 'vue';
import { formatToDate } from '@/utils/dateUtil';
import { LCMProductType, Owner, Size } from '@/pages/ReportPortal/Static/common';

export let optionsdata: any = reactive({
    SizeData: [{ label: 'ALL', value: 'ALL' }],
    ProductData: [],
    BigProductData: [],
    ProductData1: [],
    BigProductData1: [],
    OwnerName: [],
    LcmProdTypeData: []
});

/* export async function getSizeData(func: Function, params: String, funcparams: String) {
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
} */
export const schemas = ()=>{
    let optiondata2 = reactive({});
   return [
        {
            label: 'common.commonpages.Date',
            field: 'FromDate',
            component: 'DatePicker',
            componentProps: {
                valueFormat: 'YYYY-MM-DD'
            },
            defaultValue: formatToDate(),
            required: true
        },
        {
            label: 'common.commonpages.To',
            field: 'ToDate',
            component: 'DatePicker',
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
                        value: ''
                    },
                    {
                        label: '日(A)',
                        value: "and shift_id='A'"
                    },
                    {
                        label: '夜(B)',
                        value: "and shift_id='B'"
                    }
                ]
            },
            defaultValue: ''
        },
        // product type 下拉单选框
        ...new LCMProductType(optiondata2,"LCM1","ProdType").obj,
        /* {
            label: 'LCM Product Type',
            component: 'Select',
            field: 'ProdType',
            componentProps: () => {
                return {
                    options: optionsdata.LcmProdTypeData
                };
            },
            defaultValue: 'ALL'
        }, */
    
        // Product必須要一個ALL才可以請求數據，沒有ALL會失敗
        ...new Size(optiondata2, 'LCM1', {
            size: 'Size',
            bigproduct: 'BigProduct',
            optionproduct: 'OptionProduct',
            selectmenu: '#optionmenu',
            product: 'Product'
        }).obj,
       /*  {
            label: 'size',
            component: 'Select',
            field: 'Size',
            componentProps: () => {
                return {
                    options: optionsdata.SizeData,
                    placeholder: 'size',
                    onChange: async (e: any) => {
                        if (e !== 'ALL') {
                            optionsdata.BigProductData1 = optionsdata.BigProductData.filter(
                                (item) => item?.value.indexOf(e) != -1
                            );
                        } else {
                            optionsdata.BigProductData1 = optionsdata.BigProductData;
                        }
                    }
                };
            },
            defaultValue: ['ALL']
        },
        {
            label: 'big product',
            component: 'Select',
            field: 'BigProduct',
            componentProps: () => {
                return {
                    options: optionsdata.BigProductData1,
                    placeholder: 'big product',
                    onChange: async (e: any) => {
                        if (e !== 'ALL') {
                            optionsdata.ProductData1 = optionsdata.ProductData.filter(
                                (item) => item?.value.indexOf(e) != -1
                            );
                        } else {
                            optionsdata.ProductData1 = optionsdata.ProductData;
                        }
                    }
                };
            },
            defaultValue: ['ALL']
        },
        {
            label: 'Product',
            field: 'Product',
            component: 'Select',
            componentProps: () => {
                return {
                    mode: 'multiple',
                    options: optionsdata.ProductData1,
                    placeholder: 'product'
                };
            },
            defaultValue: ['ALL']
        }, */
        ...new Owner(optiondata2,"LCM1","LcmOwner").obj,
        /* {
            label: 'LCM Owner',
            component: 'Select',
            field: 'LcmOwner',
            componentProps: (model) => {
                return {
                    mode: 'multiple',
                    options: optionsdata.OwnerName,
                    onSelect: (e: any, option) => {
                        if (e != 'ALL') {
                            model.formModel['LcmOwner'] = model.formModel['LcmOwner'].filter((item) => item != 'ALL');
                        }
                    }
                };
            },
            defaultValue: ['ALL']
        } */
    ];
    
}