import type { TableColumn } from '@/components/core/dynamic-table';
export const baseColumns: any[] = [
    {
        title: 'Size',
        align: 'center',
        dataIndex: 'SUB',
        key: 'SUB',
        className:'v-center'
    },
    {
        title: 'Prod(PFCD/SKD)-CST首碼',
        align: 'center',
        dataIndex: 'LCD_PROD',
        key: 'LCD_PROD',
        className:'v-center'

    },
    {
        title: 'ERP_PROD_ID',
        align: 'center',
        dataIndex: 'ERP_PROD_ID',
        key: 'ERP_PROD_ID',
        className:'v-center'

    },
    {
        title: 'Grade',
        align: 'center',
        dataIndex: 'GRADE',
        key: 'GRADE',
        className:'v-center'

    },
    
];
export const abncolumns: any[] = [
    {
        title: 'ABN Code',
        align: 'center',
        dataIndex: 'ABN_CODE',
        key: 'ABN_CODE',
        className:'v-center'
    }
];
export const sumcolumns:any[] = [
  {
    title: "總計",
    children: [
        {
            title: "總計",
            align: 'center',
            dataIndex: "Sum",
            key: "Sum"
        }
    ]
}
]
