import { formatToDate } from '@/utils/dateUtil';

export const schemas1 = () => {
    return [
        {
            field: 'password',
            component: 'InputPassword',
            label: '請輸入密碼：',
            required: true
        }
    ];
};
export const schemas2 = (FactoryType: string) => {
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
            label: 'common.commonpages.Shift',
            component: 'RadioGroup',
            field: 'Shift',
            componentProps: {
                options: [
                    {
                        label: '日(16:30)',
                        value: 'A'
                    },
                    {
                        label: '日',
                        value: 'C'
                    },
                    {
                        label: '夜',
                        value: 'B'
                    }
                ]
            },
            defaultValue: 'A'
        },
        {
            label: 'Select type',
            component: 'RadioGroup',
            field: 'type',
            componentProps: {
                options: [
                    {
                        label: 'LCM SUM',
                        value: 'LCM SUM'
                    },
                    {
                        label: 'LCM LINE',
                        value: 'LCM LINE'
                    },
                    {
                        label: 'BL SUM',
                        value: 'BL SUM'
                    },
                    {
                        label: 'BL LINE',
                        value: 'BL LINE'
                    },
                    {
                        label: 'TV LINE',
                        value: 'TV LINE'
                    },
                ]
            },
            defaultValue: 'LCM SUM'
        }
    ];
};
