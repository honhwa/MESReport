import { formatToDate } from "@/utils/dateUtil";



export const schemas = (FactoryType: string) => {
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
                        label: '日',
                        value: 'A'
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
            label: 'Type',
            component: 'RadioGroup',
            field: 'type',
            componentProps: {
                options: [
                    {
                        label: 'SUM',
                        value: 'SUM'
                    },
                    {
                        label: 'LINE',
                        value: 'LINE'
                    }
                ]
            },
            defaultValue: 'SUM'
        },
        
    ];
};