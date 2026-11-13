import type { TableColumn } from '@/components/core/dynamic-table';

export function MoveOutTimeColumns(){
    return [
        {
            title: 'Group',
            dataIndex: 'MAINEQP',
        }, {
            title: 'Date',
            dataIndex: 'TRANS_DATE1',
        }, {
            title: 'Equipment',
            dataIndex: 'EQUIP_NBR',
        }, {
            title: 'Product',
            dataIndex: 'PROD_NBR',
        }, {
            title: 'Tact Time',
            dataIndex: 'TactTime',
            customRender: ({ record }) => {
                return <a href={record.url}>{record['TactTime']}</a>
              },
        },
    ];
}
// export const MoveOutTimeColumns: TableColumn[] = [
//     {
//         title: 'Group',
//         dataIndex: 'MAINEQP',
//     }, {
//         title: 'Date',
//         dataIndex: 'TRANS_DATE1',
//     }, {
//         title: 'Equipment',
//         dataIndex: 'EQUIP_NBR',
//     }, {
//         title: 'Product',
//         dataIndex: 'PROD_NBR',
//     }, {
//         title: 'Tact Time',
//         dataIndex: 'TactTime',
//         customRender: ({ record }) => {
//             return <a href={record.url}>{{record.TactTime}}</a>
//           },
//     },
// ]
export const MoveOutTimeDetailColumns: TableColumn[] = [
    {
        title: 'Date',
        dataIndex: 'TRANS_DATE1',
    }, {
        title: 'Time',
        dataIndex: 'Time',
    }, {
        title: 'Equipment',
        dataIndex: 'EQUIP_NBR',
    }, {
        title: 'Product',
        dataIndex: 'PROD_NBR',
    }, {
        title: 'Tact Time',
        dataIndex: 'TactTime',
    },
]

// export const MoveOutTimeColumns = TableCustomCell(MoveOutTimeLabel, MoveOutTimeProps, ['TactTime'],false, 'MoveOutTimeLCM1IEDetail')
// export const MoveOutTimeDetailColumns = TableCustomCell(MoveOutTimeDetailLabel, MoveOutTimeDetailProps, [],false, '')