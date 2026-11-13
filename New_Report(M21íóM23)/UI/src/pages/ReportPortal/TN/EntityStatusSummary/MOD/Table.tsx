import type { TableColumn } from '@/components/core/dynamic-table';
export type TableListItem = API.DayliMoveThItem;
export type TableColumnItem = TableColumn<TableListItem>;
export const baseColumns: any[] = [
    {
        title: 'Group',
        dataIndex: 'EQUIP_GRP',
        fixed: 'left',
        className: 'v-center'
    },
    {
        title: 'Entity',
        dataIndex: 'EQUIP_NBR',
        fixed: 'left',
        className: 'v-center'
    },
    {
        title: 'Target',
        children: [
            {
                title: 'UP %',
                width: 80,
                dataIndex: 'UP_RATE_TGT',
            },
            {
                title: 'Yield %',
                width: 80,
                dataIndex: 'YIELD_TGT',
            },
            {
                title: 'UPH %',
                width: 80,
                dataIndex: 'UPH_RATE_TGT',
            },
            {
                title: 'Util %',
                width: 80,
                dataIndex: 'UTIL_RATE_TGT',
            },
            {
                title: 'OEE %',
                width: 80,
                dataIndex: 'OEE_RATE_TGT',
            },
            {
                title: 'OEE %(不含Psetup/E Lot/非廠責)',
                width: 80,
                dataIndex: 'OEE_RATE_FAC_TGT',
            },
            {
                title: 'UPH',
                width: 80,
                dataIndex: 'M_UP',
            },
            {
                title: 'Daily Move',
                width: 80,
                dataIndex: 'DAILY_MOVE',
            }
        ]
    },
    {
        title: 'Throughput',
        children: [
            {
                title: 'PROD AVG Daily Move',
                dataIndex: 'PROD_AVG_DAILY_MOVE',
            },
            {
                title: 'ENG AVG Daily Move',
                dataIndex: 'ENG_AVG_DAILY_MOVE',
            },
            {
                title: 'UPH',
                dataIndex: 'UPH',
            },
            {
                title: 'MAX',
                dataIndex: 'MAX',
            }
        ]
    },
    {
        title: 'Performance',
        children: [
            {
                title: 'UP %',
                dataIndex: 'UP_RATE',
            },
            {
                title: 'UP%(Modify)',
                dataIndex: 'UP_RATE_MODIFY',
            },
            {
                title: 'Run %',
                dataIndex: 'RUN_RATE',
            },
            {
                title: 'Run%(Modify)',
                dataIndex: 'RUN_RATE_MODIFY',
            },
            {
                title: 'PSetup %',
                dataIndex: 'PSETUP_RATE',
            },
            {
                title: 'E Lot %',
                dataIndex: 'ELOT_RATE',
            },
            {
                title: 'UPH %',
                dataIndex: 'UPH_RATE',
            },
            {
                title: 'Yield %',
                dataIndex: 'YIELD_RATE',
            },
            {
                title: 'OEE %',
                dataIndex: 'OEE_RATE',
            },
            {
                title: 'OEE%(Modify)',
                dataIndex: 'OEE_RATE_MODIFY',
            }
        ]
    },
    {
        title: 'RUN',

        children: [
            {
                title: 'M_RUN',
                dataIndex: 'M_RUN',
            },
            {
                title: 'E_RUN',
                dataIndex: 'E_RUN',
            },
            {
                title: 'Sub-RUN',
                dataIndex: 'SUB_RUN',
            }
        ]
    },
    {
        title: 'IDLE(非廠責)',

        children: [
            {
                title: 'PNL_SHT',
                dataIndex: 'PNL_SHT',
            },
            {
                title: 'MTL_SHT',
                dataIndex: 'MTL_SHT',
            },
            {
                title: 'PNL_不良',
                dataIndex: 'PNL_NG',
            },
            {
                title: 'MTL_不良',
                dataIndex: 'MTL_NG',
            },
            {
                title: 'No Plan(計劃超前)',
                dataIndex: 'NO_PLAN_CODE_O',
            },
            {
                title: 'No Plan(無投產計畫)',
                dataIndex: 'NO_PLAN_CODE_N',
            },
            {
                title: '他站WIP異常(非LCM責)',
                dataIndex: 'WIP_ERROR_OTHER',
            },
            {
                title: 'Sub-IDLE(非廠責)',
                dataIndex: 'SUB_IDLE_NON',
            }
        ]
    },
    {
        title: 'IDLE(廠責)',

        children: [
            {
                title: 'EQ Auto Idle',
                dataIndex: 'EQ_AUTO_IDLE',
            },
            {
                title: 'Daily Check',
                dataIndex: 'DAILY_CHECK',
            },
            {
                title: 'NO Operator',
                dataIndex: 'NO_OPERATOR',
            },
            {
                title: '他站WIP異常(LCM責)',
                dataIndex: 'WIP_ERROR',
            },
            {
                title: '他站設備異常',
                dataIndex: 'OTHER_EQP_ERROR',
            },
            {
                title: 'Other',
                dataIndex: 'FACIDLE_OTHER',
            },
            {
                title: 'Sub-IDLE(廠責)',
                dataIndex: 'SUB_IDLE',
            }
        ]
    },
    {
        title: 'CHGMTL',

        children: [
            {
                title: 'CHGMTL',
                dataIndex: 'CHGMTL_TIME',
            }
        ]
    },
    {
        title: 'PSetup',

        children: [
            {
                title: 'PSetup',
                dataIndex: 'P_SETUP_TIME',
            }
        ]
    },
    {
        title: 'UP',

        children: [
            {
                title: 'Sub-UP',
                dataIndex: 'SUB_UP',
            }
        ]
    },
    {
        title: 'DOWN',

        children: [
            {
                title: 'WARM_UP',
                dataIndex: 'WARM_UP_TIME',
            },
            {
                title: 'ENG',
                dataIndex: 'ENG_TIME',
            },
            {
                title: 'PM',
                dataIndex: 'PM_TIME',
            },
            {
                title: 'Repair',
                dataIndex: 'REPAIR_TIME',
            },
            {
                title: 'ProDown',
                dataIndex: 'PRODOWN_TIME',
            },
            {
                title: 'FacDown',
                dataIndex: 'FACDOWN_TIME',
            },
            {
                title: 'Sub-Down',
                dataIndex: 'SUB_DOWN',
            }
        ]
    },
    {
        title: 'OFF',

        children: [
            {
                title: 'OFF',
                dataIndex: 'OFF',
            }
        ]
    }
];

export const detail1: any[] = [
    {
        title: 'EQUIP_NBR',

        dataIndex: 'EQUIP_NBR'
    },
    {
        title: 'START_TIME',

        dataIndex: 'START_TIME'
    },
    {
        title: 'Close_time',

        dataIndex: 'CLOSE_TIME'
    },
    {
        title: 'Entity_status',

        dataIndex: 'ENTITY_STATUS'
    },
    {
        title: 'EQ_status',

        dataIndex: 'EQ_STATUS'
    },
    {
        title: 'stay_time',

        dataIndex: 'STAY_TIME'
    },
    {
        title: 'code',

        dataIndex: 'CODE'
    },
    {
        title: 'status Desc',

        dataIndex: 'C_DESC'
    },
    {
        title: 'Operator',

        dataIndex: 'OPERATOR'
    },
    {
        title: 'Comment',

        dataIndex: 'COMMENT1'
    }
];
