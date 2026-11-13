import type { TableColumn } from '@/components/core/dynamic-table';

export const baseColumns: TableColumn[] = [
    {
        dataIndex: 'NO',
        title: 'NO'
    },
    {
        dataIndex: 'event',
        title: 'event'
    },
    {
        dataIndex: 'event1',
        title: 'event1',
        hideInTable:true
    },
     {
        dataIndex: 'EQUIP_NBR',
        title: 'EQUIP_NBR'
    }, {
        dataIndex: 'START_TIME',
        title: 'START_TIME'
    }, {
        dataIndex: 'CLOSE_TIME',
        title: 'CLOSE_TIME'
    }, {
        dataIndex: 'SUBTIME',
        title: 'SUBTIME'
    },
    {
        dataIndex: 'OPERATORS',
        title: 'OPERATORS'
    },

    {
        dataIndex: 'TRANS_OPERATION',
        title: 'TRANS_OPERATION'
    }, {
        dataIndex: 'NEXT_CODE',
        title: 'NEXT_CODE'
    },
    {
        dataIndex: 'SUBCOUNT',
        title: '數量'
    },{
        dataIndex: 'SUBALLTIME',
        title: '時間'
    },
]
