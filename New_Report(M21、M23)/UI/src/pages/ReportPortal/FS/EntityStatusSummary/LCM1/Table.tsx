import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns1: any[] = [
    {
        title: 'Group',
        dataIndex: 'Group',
        fixed: 'left',
        className: 'v-center',
    },
    {
        title: 'Entity',
        dataIndex: 'Entity',
        fixed: 'left',
        className: 'v-center',
    },
    {
        title: '',
        children: [
            {
                title: 'PROD Daily Move',
                width: 80,
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.',
                    },
                ],
            },
            {
                title: 'ENG Daily Move',
                width: 80,
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'Act.1',
                    },
                ],
            },
            {
                title: 'UPH',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.1',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.2',
                    },
                ],
            },
            {
                title: 'OEE%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.2',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.3',
                    },
                ],
            },
            {
                title: 'UPH%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.3',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.4',
                    },
                ],
            },
            {
                title: 'UP%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.4',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.5',
                    },
                ],
            },
            {
                title: 'RUN%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'Tgt.5',
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'Act.6',
                    },
                ],
            },
            {
                title: 'PSETUP%',
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'Act.7',
                    },
                ],
            },
            {
                title: 'E Lot%',
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'Act.8',
                    },
                ],
            },
        ],
    },
    {
        title: 'RUN',
        children: [
            {
                title: 'M_RUN',
                children: [
                    {
                        title: '',
                        dataIndex: ' 1',
                    },
                ],
            },
            {
                title: 'E_RUN',
                children: [
                    {
                        title: '',
                        dataIndex: ' 2',
                    },
                ],
            },
            {
                title: 'Sub_RUN',
                children: [
                    {
                        title: 'SUM',
                        dataIndex: 'SUM',
                    },
                ],
            },
        ],
    },
    {
        title: 'IDLE',
        children: [
            { title: '設備停置', children: [{ title: '', dataIndex: ' 3' }] },
            {
                title: 'PNL_SHT',
                width: 80,
                children: [{ title: '', dataIndex: ' 4' }],
            },
            {
                title: 'MTL_SHT',
                width: 80,

                children: [{ title: '', dataIndex: ' 5' }],
            },
            { title: 'PNL_不良', children: [{ title: '', dataIndex: ' 6' }] },
            {
                title: 'MTL_不良',
                width: 80,

                children: [{ title: '', dataIndex: ' 7' }],
            },
            {
                title: '非LCM 責WIP 異常',
                width: 80,

                children: [{ title: '', dataIndex: ' 8' }],
            },
            {
                title: '財會盤點',
                width: 80,

                children: [{ title: '', dataIndex: ' 9' }],
            },
            { title: 'No_Plan', children: [{ title: '', dataIndex: ' 10' }] },
            { title: 'ChgMTL', children: [{ title: '', dataIndex: ' 11' }] },
            { title: 'P_setup', children: [{ title: '', dataIndex: ' 12' }] },
            {
                title: 'E_setup',
                width: 80,

                children: [{ title: '', dataIndex: ' 13' }],
            },
            {
                title: 'Sub-IDLE',

                children: [{ title: 'SUM', dataIndex: 'SUM1' }],
            },
        ],
    },
    {
        title: 'Down',
        children: [
            {
                title: 'EQ_Modify',
                width: 80,
                children: [{ title: '', dataIndex: ' 14' }],
            },
            { title: 'PM', children: [{ title: '', dataIndex: ' 15' }] },
            {
                title: 'WARM_UP',
                width: 80,
                children: [{ title: '', dataIndex: ' 16' }],
            },
            { title: 'ENG', width: 80, children: [{ title: '', dataIndex: ' 17' }] },
            { title: 'Repair', children: [{ title: '', dataIndex: ' 19' }] },
            { title: 'ProDown', children: [{ title: '', dataIndex: ' 19' }] },
            {
                title: 'FacDown',
                width: 80,
                children: [{ title: '', dataIndex: ' 20' }],
            },
            {
                title: 'Sub-Down',
                children: [{ title: 'SUM', dataIndex: 'SUM2' }],
            },
        ],
    },
    {
        title: 'PSETUP',
        children: [
            {
                title: 'P_setup #',
                width: 80,
                children: [{ title: '次數', dataIndex: '次數' }],
            },
            { title: 'A1', children: [{ title: '次數', dataIndex: '次數1' }] },
            { title: 'A2', children: [{ title: '次數', dataIndex: '次數2' }] },
            { title: 'B', children: [{ title: '次數', dataIndex: '次數3' }] },
            { title: 'C', children: [{ title: '次數', dataIndex: '次數4' }] },
            { title: 'D', children: [{ title: '次數', dataIndex: '次數5' }] },
            {
                title: 'OTHER',
                width: 80,
                children: [{ title: '次數', dataIndex: '次數6' }],
            },
        ],
    },
    {
        title: 'CHMT',

        children: [
            {
                title: 'ChgMTL #',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數7' }],
            },
            {
                title: 'G TCP/Chip IC',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數8' }],
            },
            {
                title: 'S TCP/Chip IC',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數9' }],
            },
            {
                title: 'TEFLON',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數10' }],
            },
            {
                title: 'SILICONE',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數11' }],
            },
            { title: 'ACF', children: [{ title: '次數', dataIndex: '次數12' }] },
            {
                title: '不織布',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數13' }],
            },
            { title: 'TAPE', children: [{ title: '次數', dataIndex: '次數14' }] },
            {
                title: 'OTHER',
                width: 80,

                children: [{ title: '次數', dataIndex: '次數15' }],
            },
        ],
    },
    {
        title: 'PM',

        children: [
            {
                title: 'PM#',

                children: [
                    {
                        title: '次數',

                        dataIndex: '次數16',
                    },
                ],
            },
        ],
    },
];
export let excelparams1 = {
    Header: {
        title: [
            'Group',
            'Entity',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            'RUN',
            'RUN',
            'RUN',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'IDLE',
            'Down',
            'Down',
            'Down',
            'Down',
            'Down',
            'Down',
            'Down',
            'Down',
            'PSETUP',
            'PSETUP',
            'PSETUP',
            'PSETUP',
            'PSETUP',
            'PSETUP',
            'PSETUP',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'CHMT',
            'PM',
        ] as any[],
        key: [
            'Group',
            'Entity',
            'Tgt.',
            'Act.',
            'Act.1',
            'Tgt.1',
            'Act.2',
            'Tgt.2',
            'Act.3',
            'Tgt.3',
            'Act.4',
            'Tgt.4',
            'Act.5',
            'Tgt.5',
            'Act.6',
            'Act.7',
            'Act.8',
            ' 1',
            ' 2',
            'SUM',
            ' 3',
            ' 4',
            ' 5',
            ' 6',
            ' 7',
            ' 8',
            ' 9',
            ' 10',
            ' 11',
            ' 12',
            ' 13',
            'SUM1',
            ' 14',
            ' 15',
            ' 16',
            ' 17',
            ' 19',
            ' 19',
            ' 20',
            'SUM2',
            '次數',
            '次數1',
            '次數2',
            '次數3',
            '次數4',
            '次數5',
            '次數6',
            '次數7',
            '次數8',
            '次數9',
            '次數10',
            '次數11',
            '次數12',
            '次數13',
            '次數14',
            '次數15',
            '次數16',
        ] as any[],
    },
    MutiHeader: [
        [
            'Group',
            'Entity',
            'PROD Daily Move',
            'PROD Daily Move',
            'ENG Daily Move',
            'UPH',
            'UPH',
            'OEE%',
            'OEE%',
            'UPH%',
            'UPH%',
            'UP%',
            'UP%',
            'RUN%',
            'RUN%',
            'PSETUP%',
            'E Lot%',
            'M_RUN',
            'E_RUN',
            'Sub_RUN',
            '設備停置',
            'PNL_SHT',
            'MTL_SHT',
            'PNL_不良',
            'MTL_不良',
            '非LCM 責WIP 異常',
            '財會盤點',
            'No_Plan',
            'ChgMTL',
            'P_setup',
            'E_setup',
            'Sub-IDLE',
            'EQ_Modify',
            'PM',
            'WARM_UP',
            'ENG',
            'Repair',
            'ProDown',
            'FacDown',
            'Sub-Down',
            'P_setup #',
            'A1',
            'A2',
            'B',
            'C',
            'D',
            'OTHER',
            'ChgMTL #',
            'G TCP/Chip IC',
            'S TCP/Chip IC',
            'TEFLON',
            'SILICONE',
            'ACF',
            '不織布',
            'TAPE',
            'OTHER',
            'PM#',
        ],
        [
            'Group',
            'Entity',
            'Tgt.',
            'Act.',
            'Act.',
            'Tgt.',
            'Act.',
            'Tgt.',
            'Act.',
            'Tgt.',
            'Act.',
            'Tgt.',
            'Act.',
            'Tgt.',
            'Act.',
            'Act.',
            'Act.',
            '',
            '',
            'SUM',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            'SUM',
            '',
            '',
            '',
            '',
            '',
            '',
            '',
            'SUM',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
            '次數',
        ],
    ] as any[],
};
export const baseColumns2: any[] = [
    {
        title: 'Group',
        dataIndex: 'Group',
        fixed: 'left',
        className: 'v-center',
    },
    {
        title: 'Entity',
        fixed: 'left',
        className: 'v-center',
        dataIndex: 'Entity',
    },
    {
        title: 'Target',
        children: [
            {
                title: 'UPH',

                dataIndex: 'UPH0',
            },
            {
                title: 'UPH達成率',
                dataIndex: 'UPH達成率',
            },
        ],
    },
    {
        title: 'Throughput',

        children: [
            {
                title: 'PROD AVG Daily Move',

                dataIndex: 'PROD AVG Daily Move',
            },
            {
                title: 'ENG AVG Daily Move',

                dataIndex: 'ENG AVG Daily Move',
            },
            {
                title: 'UPH',

                dataIndex: 'UPH1',
            },
            {
                title: 'MAX',

                dataIndex: 'MAX',
            },
        ],
    },
    {
        title: 'Performance',

        children: [
            { title: 'UP-%', dataIndex: 'UP-%', key: 'UP-%' },
            { title: 'Run rate', dataIndex: 'Run rate', key: 'Run rate' },
            { title: 'UPH達成率', dataIndex: 'UPH達成率', key: 'UPH達成率1' },
            { title: 'Util.%', dataIndex: 'Util.%', key: 'Util.%' },
            { title: 'OEE.%', dataIndex: 'OEE.%', key: 'OEE.%' },
            { title: '能力OEE.%', dataIndex: '能力OEE.%', key: '能力OEE.%' },
        ],
    },
    {
        title: 'RUN',

        children: [
            { title: 'M_RUN', dataIndex: 'M_RUN' },
            { title: 'E_RUN', dataIndex: 'E_RUN' },
            { title: 'Sub-RUN', dataIndex: 'Sub-RUN' },
        ],
    },
    {
        title: 'IDLE',

        children: [
            { title: '設備停置', dataIndex: '設備停置' },
            { title: 'Daily Check', dataIndex: 'Daily Check' },
            { title: 'Panel停線', dataIndex: 'Panel停線' },
            { title: '部材停線', dataIndex: '部材停線' },
            { title: 'No_plan', dataIndex: 'No_plan' },
            { title: 'EQ AUTO IDLE', dataIndex: 'EQ AUTO IDLE' },
            { title: 'MTL_SHT', dataIndex: 'MTL_SHT' },
            { title: 'P_setup', dataIndex: 'P_setup' },
            { title: 'E_setup', dataIndex: 'E_setup' },
            { title: 'ChgMTL', dataIndex: 'ChgMTL' },
            { title: 'Sub-IDLE', dataIndex: 'Sub-IDLE' },
        ],
    },
    {
        title: 'DOWN',

        children: [
            { title: 'EQ_MODIFY', dataIndex: 'EQ_MODIFY' },
            { title: 'ENG', dataIndex: 'ENG' },
            { title: 'PM', dataIndex: 'PM' },
            { title: 'Repair', dataIndex: 'Repair' },
            { title: 'ProDown', dataIndex: 'ProDown' },
            { title: 'FacDown', dataIndex: 'FacDown' },
            { title: 'Sub-Down', dataIndex: 'Sub-Down' },
        ],
    },
];
export let excelparams2 = {
    Header: {
        title: [
            'Group',       'Entity',      'Target',
            'Target',      'Throughput',  'Throughput',
            'Throughput',  'Throughput',  'Performance',
            'Performance', 'Performance', 'Performance',
            'Performance', 'Performance', 'RUN',
            'RUN',         'RUN',         'IDLE',
            'IDLE',        'IDLE',        'IDLE',
            'IDLE',        'IDLE',        'IDLE',
            'IDLE',        'IDLE',        'IDLE',
            'IDLE',        'DOWN',        'DOWN',
            'DOWN',        'DOWN',        'DOWN',
            'DOWN',        'DOWN'
          ]
          ,
        key:[
            'Group',               'Entity',
            'UPH0',                'UPH達成率',
            'PROD AVG Daily Move', 'ENG AVG Daily Move',
            'UPH1',                'MAX',
            'UP-%',                'Run rate',
            'UPH達成率1',           'Util.%',
            'OEE.%',               '能力OEE.%',
            'M_RUN',               'E_RUN',
            'Sub-RUN',             '設備停置',
            'Daily Check',         'Panel停線',
            '部材停線',            'No_plan',
            'EQ AUTO IDLE',        'MTL_SHT',
            'P_setup',             'E_setup',
            'ChgMTL',              'Sub-IDLE',
            'EQ_MODIFY',           'ENG',
            'PM',                  'Repair',
            'ProDown',             'FacDown',
            'Sub-Down'
          ]
    },
    MutiHeader: [
        [
            'Group',               'Entity',
            'UPH',                 'UPH達成率',
            'PROD AVG Daily Move', 'ENG AVG Daily Move',
            'UPH',                 'MAX',
            'UP-%',                'Run rate',
            'UPH達成率',           'Util.%',
            'OEE.%',               '能力OEE.%',
            'M_RUN',               'E_RUN',
            'Sub-RUN',             '設備停置',
            'Daily Check',         'Panel停線',
            '部材停線',            'No_plan',
            'EQ AUTO IDLE',        'MTL_SHT',
            'P_setup',             'E_setup',
            'ChgMTL',              'Sub-IDLE',
            'EQ_MODIFY',           'ENG',
            'PM',                  'Repair',
            'ProDown',             'FacDown',
            'Sub-Down'
          ]
    ]
}
export const detail1: any[] = [
    {
        title: 'EQUIP_NBR',

        dataIndex: 'EQUIP_NBR',
    },
    {
        title: 'START_TIME',

        dataIndex: 'START_TIME',
    },
    {
        title: 'Close_time',

        dataIndex: 'CLOSE_TIME',
    },
    {
        title: 'Entity_status',

        dataIndex: 'ENTITY_STATUS',
    },
    {
        title: 'EQ_status',

        dataIndex: 'EQ_STATUS',
    },
    {
        title: 'stay_time',

        dataIndex: 'STAY_TIME',
    },
    {
        title: 'code',

        dataIndex: 'CODE',
    },
    {
        title: 'status Desc',

        dataIndex: 'C_DESC',
    },
    {
        title: 'Operator',

        dataIndex: 'OPERATOR',
    },
    {
        title: 'Comment',

        dataIndex: 'COMMENT1',
    },
];
