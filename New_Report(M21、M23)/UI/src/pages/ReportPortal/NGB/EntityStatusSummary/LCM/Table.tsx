import { flexWidth } from '@/pages/ReportPortal/utils/common';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';
import { reactive } from 'vue';


const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

const customCellRender = (record: Object, rowIndex: number, column: Object) => {
    return {
        ondblclick: () => { 
           useReportCommon.AddFirstTableParams(`EntityStatusSummary-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
           router.push({ name: `EntityStatusSummaryDetail-${useCommonParams.Fab}` });  
        },
        style: {
            cursor: 'pointer'
        }
    };
  };



export const baseColumns1: any[] = [
    {
        title: 'Group',
        dataIndex: 'EQUIP_GRP',
        fixed: 'left',
        className: 'v-center',
        customCell: (record,column: Object) => {
            let text = record['EQUIP_GRP']
            let obj = {
              text,
              style: {},
              
            }
            obj['rowSpan'] = record?.['EQUIP_GRP-span']?.['rowspan'] || 0
            return obj
          }
    },
    {
        title: 'Entity',
        dataIndex: 'EQUIP_NBR',
        fixed: 'left',
        className: 'v-center',
        customCell: (record,column: Object) => {
            let text = record['EQUIP_NBR']
            let obj = {
              text,
              style: {cursor: 'pointer'},
              ondblclick: () => { 
                useReportCommon.AddFirstTableParams(`EntityStatusSummary-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] }) 
                router.push({ name: `EntityStatusSummaryDetail-${useCommonParams.Fab}` });  
             },
            }
            obj['rowSpan'] = record?.['EQUIP_NBR-span']?.['rowspan'] || 0
            return obj
          }
    },
    {
        title: '',
        children: [
            {
                title: 'Prod Daily Move',
                children: [
                    {
                        title: 'Tgt.',                        
                        dataIndex: 'PROD_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'PROD_MOVE',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'ENG Daily Move',
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'ENG_MOVE',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'UPH',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'UPH_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'UPH_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'OEE%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'OEE_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'OEE_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'UPH%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'UPHRATE_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'UPHRATE_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'UP%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'UP_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'UP_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'RUN%',
                children: [
                    {
                        title: 'Tgt.',
                        dataIndex: 'RUN_TAR',
                        customCell: customCellRender,
                    },
                    {
                        title: 'Act.',
                        dataIndex: 'RUN_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'PSETUP%',
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'PSETUP_ACT',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'E Lot%',
                children: [
                    {
                        title: 'Act.',
                        dataIndex: 'ELOT_ACT',
                        customCell: customCellRender,
                    }
                ]
            }
        ]
    },
    {
        title: 'RUN',
        children: [
            {
                title: 'M_RUN',
                children: [
                    {
                        title: '',
                        dataIndex: 'M_RUN',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'E_RUN',
                children: [
                    {
                        title: '',
                        dataIndex: 'E_RUN',
                        customCell: customCellRender,
                    }
                ]
            },
            {
                title: 'Sub_RUN',
                children: [
                    {
                        title: 'SUM',
                        dataIndex: 'RUN_SUM',
                        customCell: customCellRender,
                    }
                ]
            }
        ]
    },
    {
        title: 'IDLE',
        children: [
            { title: '設備停置', children: [{ title: '', dataIndex: 'IDLE_TIME',customCell: customCellRender, }] },
            {
                title: 'PNL_SHT',
                children: [{ title: '', dataIndex: 'PNL_SHT',customCell: customCellRender,}]
            },
            {
                title: 'MTL_SHT',

                children: [{ title: '', dataIndex: 'MTL_SHT_TIME',customCell: customCellRender, }]
            },
            { title: 'PNL_不良', children: [{ title: '', dataIndex: 'IDLE_TIME1',customCell: customCellRender, }] },
            {
                title: 'MTL_不良',

                children: [{ title: '', dataIndex: 'IDLE_TIME2',customCell: customCellRender, }]
            },
            {
                title: '非LCM 責WIP 異常',

                children: [{ title: '', dataIndex: 'WIP_ERR',customCell: customCellRender, }]
            },
            {
                title: '財會盤點',

                children: [{ title: '', dataIndex: 'ERP_IVE',customCell: customCellRender, }]
            },
            { title: 'No_Plan', children: [{ title: '', dataIndex: 'IDLE_TIME3',customCell: customCellRender, }] },
            { title: 'ChgMTL', children: [{ title: '', dataIndex: 'CHGMTL_TIME',customCell: customCellRender, }] },
            { title: 'P_setup', children: [{ title: '', dataIndex: 'P_SETUP_TIME',customCell: customCellRender, }] },
            {
                title: 'E_setup',

                children: [{ title: '', dataIndex: 'E_SETUP_TIME',customCell: customCellRender, }]
            },
            {
                title: 'Sub-IDLE',

                children: [{ title: 'SUM', dataIndex: 'IDLE_SUM',customCell: customCellRender, }]
            }
        ]
    },
    {
        title: 'Down',

        children: [
            {
                title: 'EQ_Modify',

                children: [{ title: '', dataIndex: 'EQ_MODIFY',customCell: customCellRender, }]
            },
            { title: 'PM', children: [{ title: '', dataIndex: 'PM_TIME',customCell: customCellRender, }] },
            {
                title: 'WARM_UP',

                children: [{ title: '', dataIndex: 'WARM_UP_TIME',customCell: customCellRender, }]
            },
            { title: 'ENG', width: 80, children: [{ title: '', dataIndex: 'ENG_TIME',customCell: customCellRender, }] },
            { title: 'Repair', children: [{ title: '', dataIndex: 'REPAIR_TIME',customCell: customCellRender, }] },
            { title: 'ProDown', children: [{ title: '', dataIndex: 'PRODOWN_TIME',customCell: customCellRender, }] },
            {
                title: 'FacDown',

                children: [{ title: '', dataIndex: 'FACDOWN_TIME',customCell: customCellRender, }]
            },
            {
                title: 'Sub-Down',

                children: [{ title: 'SUM', dataIndex: 'DOWN_SUM' ,customCell: customCellRender,}]
            }
        ]
    },
    {
        title: 'PSETUP',

        children: [
            {
                title: 'P_setup #',

                children: [{ title: '次數', dataIndex: 'PSETUP_CNT',customCell: customCellRender, }]
            },
            { title: 'A1', children: [{ title: '次數', dataIndex: 'PSETUP_A1',customCell: customCellRender, }] },
            { title: 'A2', children: [{ title: '次數', dataIndex: 'PSETUP_A2' ,customCell: customCellRender,}] },
            { title: 'B', children: [{ title: '次數', dataIndex: 'PSETUP_B' ,customCell: customCellRender,}] },
            { title: 'C', children: [{ title: '次數', dataIndex: 'PSETUP_C',customCell: customCellRender, }] },
            { title: 'D', children: [{ title: '次數', dataIndex: 'PSETUP_D',customCell: customCellRender, }] },
            {
                title: 'OTHER',

                children: [{ title: '次數', dataIndex: 'PSETUP_O',customCell: customCellRender, }]
            }
        ]
    },
    {
        title: 'CHMT',

        children: [
            {
                title: 'ChgMTL #',

                children: [{ title: '次數', dataIndex: 'CHMT_CNT',customCell: customCellRender, }]
            },
            {
                title: 'G TCP/Chip IC',
                children: [{ title: '次數', dataIndex: 'CHMT_A' ,customCell: customCellRender,}]
            },
            {
                title: 'S TCP/Chip IC',

                children: [{ title: '次數', dataIndex: 'CHMT_B',customCell: customCellRender, }]
            },
            {
                title: 'TEFLON',

                children: [{ title: '次數', dataIndex: 'CHMT_C',customCell: customCellRender, }]
            },
            {
                title: 'SILICONE',

                children: [{ title: '次數', dataIndex: 'CHMT_D',customCell: customCellRender, }]
            },
            { title: 'ACF', children: [{ title: '次數', dataIndex: 'CHMT_E' ,customCell: customCellRender,}] },
            {
                title: '不織布',

                children: [{ title: '次數', dataIndex: 'CHMT_G',customCell: customCellRender, }]
            },
            { title: 'TAPE', children: [{ title: '次數', dataIndex: 'CHMT_H',customCell: customCellRender, }] },
            {
                title: 'OTHER',

                children: [{ title: '次數', dataIndex: 'CHMT_O',customCell: customCellRender, }]
            }
        ]
    },
    {
        title: 'PM',

        children: [
            {
                title: 'PM#',

                children: [
                    {
                        title: '次數',
                        dataIndex: 'PM_CNT',
                        customCell: customCellRender,
                    }
                ]
            }
        ]
    }
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
            'EQUIP_GRP',
            'EQUIP_NBR',
            'PROD_TAR',
            'PROD_MOVE',
            'ENG_MOVE',
            'UPH_TAR',
            'UPH_ACT',
            'OEE_TAR',
            'OEE_ACT',
            'UPHRATE_TAR',
            'UPHRATE_ACT',
            'UP_TAR',
            'UP_ACT',
            'RUN_TAR',
            'RUN_ACT',
            'PSETUP_ACT',
            'ELOT_ACT',
            'M_RUN',
            'E_RUN',
            'RUN_SUM',
            'IDLE_TIME',
            'PNL_SHT',
            'MTL_SHT_TIME',
            'IDLE_TIME1',
            'IDLE_TIME2',
            'WIP_ERR',
            'ERP_IVE',
            'IDLE_TIME3',
            'CHGMTL_TIME',
            'P_SETUP_TIME',
            'E_SETUP_TIME',
            'IDLE_SUM',
            'EQ_MODIFY',
            'PM_TIME',
            'WARM_UP_TIME',
            'ENG_TIME',
            'REPAIR_TIME',
            'PRODOWN_TIME',
            'FACDOWN_TIME',
            'DOWN_SUM',
            'PSETUP_CNT',
            'PSETUP_A1',
            'PSETUP_A2',
            'PSETUP_B',
            'PSETUP_C',
            'PSETUP_D',
            'PSETUP_O',
            'CHMT_CNT',
            'CHMT_A',
            'CHMT_B',
            'CHMT_C',
            'CHMT_D',
            'CHMT_E',
            'CHMT_G',
            'CHMT_H',
            'CHMT_O',
            'PM_CNT',
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
// export const baseColumns2: any[] = [
//     {
//         title: 'Group',

//         dataIndex: 'Group',
//         fixed: 'left',
//         className: 'v-center'
//     },
//     {
//         title: 'Entity',
//         fixed: 'left',
//         className: 'v-center',
//         dataIndex: 'Entity'
//     },
//     {
//         title: 'Target',
//         children: [
//             {
//                 title: 'UPH',

//                 dataIndex: 'UPH0'
//             },
//             {
//                 title: 'UPH達成率',

//                 dataIndex: 'UPH達成率'
//             }
//         ]
//     },
//     {
//         title: 'Throughput',

//         children: [
//             {
//                 title: 'PROD AVG Daily Move',

//                 dataIndex: 'PROD AVG Daily Move'
//             },
//             {
//                 title: 'ENG AVG Daily Move',

//                 dataIndex: 'ENG AVG Daily Move'
//             },
//             {
//                 title: 'UPH',

//                 dataIndex: 'UPH1'
//             },
//             {
//                 title: 'MAX',

//                 dataIndex: 'MAX'
//             }
//         ]
//     },
//     {
//         title: 'Performance',

//         children: [
//             { title: 'UP-%', dataIndex: 'UP-%', key: 'UP-%' },
//             { title: 'Run rate', dataIndex: 'Run rate', key: 'Run rate' },
//             { title: 'UPH達成率', dataIndex: 'UPH達成率', key: 'UPH達成率' },
//             { title: 'Util.%', dataIndex: 'Util.%', key: 'Util.%' },
//             { title: 'OEE.%', dataIndex: 'OEE.%', key: 'OEE.%' },
//             { title: '能力OEE.%', dataIndex: '能力OEE.%', key: '能力OEE.%' }
//         ]
//     },
//     {
//         title: 'RUN',

//         children: [
//             { title: 'M_RUN', dataIndex: 'M_RUN' },
//             { title: 'E_RUN', dataIndex: 'E_RUN' },
//             { title: 'Sub-RUN', dataIndex: 'Sub-RUN' }
//         ]
//     },
//     {
//         title: 'IDLE',

//         children: [
//             { title: '設備停置', dataIndex: '設備停置' },
//             { title: 'Daily Check', dataIndex: 'Daily Check' },
//             { title: 'Panel停線', dataIndex: 'Panel停線' },
//             { title: '部材停線', dataIndex: '部材停線' },
//             { title: 'No_plan', dataIndex: 'No_plan' },
//             { title: 'EQ AUTO IDLE', dataIndex: 'EQ AUTO IDLE' },
//             { title: 'MTL_SHT', dataIndex: 'MTL_SHT' },
//             { title: 'P_setup', dataIndex: 'P_setup' },
//             { title: 'E_setup', dataIndex: 'E_setup' },
//             { title: 'ChgMTL', dataIndex: 'ChgMTL' },
//             { title: 'Sub-IDLE', dataIndex: 'Sub-IDLE' }
//         ]
//     },
//     {
//         title: 'DOWN',

//         children: [
//             { title: 'EQ_MODIFY', dataIndex: 'EQ_MODIFY' },
//             { title: 'ENG', dataIndex: 'ENG' },
//             { title: 'PM', dataIndex: 'PM' },
//             { title: 'Repair', dataIndex: 'Repair' },
//             { title: 'ProDown', dataIndex: 'ProDown' },
//             { title: 'FacDown', dataIndex: 'FacDown' },
//             { title: 'Sub-Down', dataIndex: 'Sub-Down' }
//         ]
//     }
// ];
export const detail1: any[] = [
    {
        title: 'EQUIP_NBR',
        dataIndex: 'EQUIP_NBR',
        width:70,
    },
    {
        title: 'START_TIME',
        dataIndex: 'START_TIME',
        width:110,
    },
    {
        title: 'Close_time',
        dataIndex: 'CLOSE_TIME',
        width:110,
    },
    {
        title: 'Entity_status',
        dataIndex: 'ENTITY_STATUS',
        width:70,
    },
    {
        title: 'EQ_status',
        dataIndex: 'EQ_STATUS',
        width:70,
    },
    {
        title: 'stay_time',
        dataIndex: 'STAY_TIME',
        width:60,
    },
    {
        title: 'code',
        dataIndex: 'CODE',
        width:50,
    },
    {
        title: 'status Desc',
        dataIndex: 'C_DESC',
        width:170,
    },
    {
        title: 'Operator',
        dataIndex: 'OPERATOR',
        width:70,
    },
    {
        title: 'Comment',
        dataIndex: 'COMMENT1',
        width:150,
    }
];
