//需要计算的列
export let CalcuList = [
    'TARGET_QTY',
    'ACTUAL_QTY',
    'QTY1',
    'AT_QTY',
    'REMAIN',
    'WIP0',
    'WIP1',
    'SHIP_QTY',
    'SCRAP_QTY',
    'WIP_ALL',
    'WIP_NON_HOLD',
    'WIP_HOLD',
    'ALL_WIP_1XXX_NR',
    'ALL_WIP_1XXX_R',
    'ALL_WIP_2XXX',
    'ALL_WIP_3XXX',
    'ALL_WIP_4XXX',
    'NON_HOLD_WIP_1100',
    'NON_HOLD_WIP_1200',
    'NON_HOLD_WIP_1300',
    'NON_HOLD_WIP_1350',
    'NON_HOLD_WIP_1400',
    'NON_HOLD_WIP_1450',
    'NON_HOLD_WIP_1500',
    'NON_HOLD_WIP_1600',
    'NON_HOLD_WIP_1700',
    'NON_HOLD_WIP_1800',
    'NON_HOLD_WIP_1850',
    'NON_HOLD_WIP_1910',
    'NON_HOLD_WIP_2401',
    'NON_HOLD_WIP_2402',
    'NON_HOLD_WIP_2403',
    'NON_HOLD_WIP_2404',
    'NON_HOLD_WIP_2405',
    'NON_HOLD_WIP_2406',
    'NON_HOLD_WIP_2407',
    'NON_HOLD_WIP_2408',
    'NON_HOLD_WIP_2409',
    'NON_HOLD_WIP_2410',
    'NON_HOLD_WIP_2420',
    'NON_HOLD_WIP_3000',
    'NON_HOLD_WIP_4000',
    'HOLD_WIP_1100',
    'HOLD_WIP_1200',
    'HOLD_WIP_1300',
    'HOLD_WIP_1350',
    'HOLD_WIP_1400',
    'HOLD_WIP_1450',
    'HOLD_WIP_1500',
    'HOLD_WIP_1600',
    'HOLD_WIP_1700',
    'HOLD_WIP_1800',
    'HOLD_WIP_1850',
    'HOLD_WIP_1910',
    'HOLD_WIP_2401',
    'HOLD_WIP_2402',
    'HOLD_WIP_2403',
    'HOLD_WIP_2404',
    'HOLD_WIP_2405',
    'HOLD_WIP_2406',
    'HOLD_WIP_2407',
    'HOLD_WIP_2408',
    'HOLD_WIP_2409',
    'HOLD_WIP_2410',
    'HOLD_WIP_2420',
    'HOLD_WIP_3000',
    'HOLD_WIP_4000'
];

// 可点击列
export let ClickColumn = [
    'REMAIN',
    'WIP0',
    'WIP1',
    'SCRAP_QTY',
    'WIP_NON_HOLD',
    'WIP_HOLD',
    'ALL_WIP_1XXX_NR',
    'ALL_WIP_1XXX_R',
    'ALL_WIP_2XXX',
    'ALL_WIP_3XXX',
    'ALL_WIP_4XXX',
    'NON_HOLD_WIP_1100',
    'NON_HOLD_WIP_1200',
    'NON_HOLD_WIP_1300',
    'NON_HOLD_WIP_1350',
    'NON_HOLD_WIP_1400',
    'NON_HOLD_WIP_1450',
    'NON_HOLD_WIP_1500',
    'NON_HOLD_WIP_1600',
    'NON_HOLD_WIP_1700',
    'NON_HOLD_WIP_1800',
    'NON_HOLD_WIP_1850',
    'NON_HOLD_WIP_1910',
    'NON_HOLD_WIP_2401',
    'NON_HOLD_WIP_2402',
    'NON_HOLD_WIP_2403',
    'NON_HOLD_WIP_2404',
    'NON_HOLD_WIP_2405',
    'NON_HOLD_WIP_2406',
    'NON_HOLD_WIP_2407',
    'NON_HOLD_WIP_2408',
    'NON_HOLD_WIP_2409',
    'NON_HOLD_WIP_2410',
    'NON_HOLD_WIP_2420',
    'NON_HOLD_WIP_3000',
    'NON_HOLD_WIP_4000',
    'HOLD_WIP_1100',
    'HOLD_WIP_1200',
    'HOLD_WIP_1300',
    'HOLD_WIP_1350',
    'HOLD_WIP_1400',
    'HOLD_WIP_1450',
    'HOLD_WIP_1500',
    'HOLD_WIP_1600',
    'HOLD_WIP_1700',
    'HOLD_WIP_1800',
    'HOLD_WIP_1850',
    'HOLD_WIP_1910',
    'HOLD_WIP_2401',
    'HOLD_WIP_2402',
    'HOLD_WIP_2403',
    'HOLD_WIP_2404',
    'HOLD_WIP_2405',
    'HOLD_WIP_2406',
    'HOLD_WIP_2407',
    'HOLD_WIP_2408',
    'HOLD_WIP_2409',
    'HOLD_WIP_2410',
    'HOLD_WIP_2420',
    'HOLD_WIP_3000',
    'HOLD_WIP_4000'
];

export let baseColumns: any[] = [
    {
        title: '',
        align: 'center',
        children: [
            {
                title: 'Work Order',
                align: 'center',
                dataIndex: `WO_NBR`,
                key: `WO_NBR`,
                fixed: 'left',
                className: 'v-center'
            },
            {
                title: 'WO Cust',
                align: 'center',
                dataIndex: `CUSTOMER_GROUP`,
                key: `CUSTOMER_GROUP`
            },
            {
                title: 'WO Comment',
                align: 'center',
                dataIndex: `COMMENT1`,
                key: `COMMENT1`
            },
            {
                title: 'WO Type',
                align: 'center',
                dataIndex: `WO_TYPE`,
                key: `WO_TYPE`
            },
            {
                title: 'Owner',
                align: 'center',
                dataIndex: `LCM_OWNER`,
                key: `LCM_OWNER`
            },
            {
                title: 'Engineer',
                align: 'center',
                dataIndex: `ENGINEER`,
                key: `ENGINEER`
            },
            {
                title: 'Size',
                align: 'center',
                dataIndex: `PROD_SIZE`,
                key: `PROD_SIZE`
            },
            {
                title: 'Prod Type',
                align: 'center',
                dataIndex: `PROD_TYPE`,
                key: `PROD_TYPE`
            },
            {
                title: 'Lcm Product',
                align: 'center',
                dataIndex: `PROD_NBR`,
                key: `PROD_NBR`
            },
            {
                title: 'Ver',
                align: 'center',
                dataIndex: `VER`,
                key: `VER`
            },
            {
                title: 'Start Date',
                align: 'center',
                dataIndex: `PLAN_ST_DT`,
                key: `PLAN_ST_DT`
            },
            {
                title: 'Due Day',
                align: 'center',
                dataIndex: `DUE_DT`,
                key: `DUE_DT`
            },
            {
                title: 'Actual Start Date',
                align: 'center',
                dataIndex: `ACTUAL_START_DT`,
                key: `ACTUAL_START_DT`
            },
            {
                title: 'Actual Comp Date',
                align: 'center',
                dataIndex: `ACTUAL_CMPL_DT`,
                key: `ACTUAL_CMPL_DT`
            },
            {
                title: 'Ship Comp Date',
                align: 'center',
                dataIndex: `SHIPPING_DATE`,
                key: `SHIPPING_DATE`
            },
            {
                title: 'WO Status',
                align: 'center',
                dataIndex: `WO_STATUS`,
                key: `WO_STATUS`
            },
            {
                title: '投入率',
                align: 'center',
                dataIndex: `MVIN_RATE`,
                key: `MVIN_RATE`
            },
            {
                title: '完成率',
                align: 'center',
                dataIndex: `MVOU_RATE`,
                key: `MVOU_RATE`
            },
            {
                title: '未入庫比率',
                align: 'center',
                dataIndex: `RECY_SCRAP_SHIP`,
                key: `RECY_SCRAP_SHIP`
            },
            {
                title: 'Target Qty',
                align: 'center',
                dataIndex: `TARGET_QTY`,
                key: `TARGET_QTY`
            },
            {
                title: 'Actual Qty',
                align: 'center',
                dataIndex: `ACTUAL_QTY`,
                key: `ACTUAL_QTY`
            },
            {
                title: '轉BL Recycle',
                align: 'center',
                dataIndex: `QTY1`,
                key: `QTY1`
            },
            {
                title: '實際投入',
                align: 'center',
                dataIndex: `AT_QTY`,
                key: `AT_QTY`
            },
            {
                title: 'Remain',
                align: 'center',
                dataIndex: `REMAIN`,
                key: `REMAIN`
            },
            {
                title: 'WIP(不含1800-1910)',
                align: 'center',
                dataIndex: `WIP0`,
                key: `WIP0`
            },
            {
                title: 'WIP(1800-1910)',
                align: 'center',
                dataIndex: `WIP1`,
                key: `WIP1`
            },
            {
                title: 'Complete',
                align: 'center',
                dataIndex: `SHIP_QTY`,
                key: `SHIP_QTY`
            },
            {
                title: 'Scrap',
                align: 'center',
                dataIndex: `SCRAP_QTY`,
                key: `SCRAP_QTY`
            },
            {
                title: 'Total WIP',
                align: 'center',
                dataIndex: `WIP_ALL`,
                key: `WIP_ALL`
            },
            {
                title: 'Non-Hold',
                align: 'center',
                dataIndex: `WIP_NON_HOLD`,
                key: `WIP_NON_HOLD`
            }
        ]
    },
    {
        title: 'ALL',
        align: 'center',
        className: 'v-center',
        children: [
            {
                title: 'Hold',
                align: 'center',
                dataIndex: `WIP_HOLD`,
                key: `WIP_HOLD`
            },
            {
                title: '1XXX(NR)',
                align: 'center',
                dataIndex: `ALL_WIP_1XXX_NR`,
                key: `ALL_WIP_1XXX_NR`
            },
            {
                title: '1XXX(R)',
                align: 'center',
                dataIndex: `ALL_WIP_1XXX_R`,
                key: `ALL_WIP_1XXX_R`
            },
            {
                title: '2XXX',
                align: 'center',
                dataIndex: `ALL_WIP_2XXX`,
                key: `ALL_WIP_2XXX`
            },
            {
                title: '3XXX',
                align: 'center',
                dataIndex: `ALL_WIP_3XXX`,
                key: `ALL_WIP_3XXX`
            }
        ]
    },
    {
        title: 'NON-HOLD',
        align: 'center',
        className: 'v-center',
        children: [
            {
                title: '4XXX',
                align: 'center',
                dataIndex: 'ALL_WIP_4XXX',
                key: 'ALL_WIP_4XXX'
            },
            {
                title: '1100',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1100',
                key: 'NON_HOLD_WIP_1100'
            },
            {
                title: '1200',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1200',
                key: 'NON_HOLD_WIP_1200'
            },
            {
                title: '1300',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1300',
                key: 'NON_HOLD_WIP_1300'
            },
            {
                title: '1350',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1350',
                key: 'NON_HOLD_WIP_1350'
            },
            {
                title: '1400',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1400',
                key: 'NON_HOLD_WIP_1400'
            },
            {
                title: '1450',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1450',
                key: 'NON_HOLD_WIP_1450'
            },
            {
                title: '1500',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1500',
                key: 'NON_HOLD_WIP_1500'
            },
            {
                title: '1600',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1600',
                key: 'NON_HOLD_WIP_1600'
            },
            {
                title: '1700',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1700',
                key: 'NON_HOLD_WIP_1700'
            },
            {
                title: '1800',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1800',
                key: 'NON_HOLD_WIP_1800'
            },
            {
                title: '1850',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1850',
                key: 'NON_HOLD_WIP_1850'
            },
            {
                title: '1910',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_1910',
                key: 'NON_HOLD_WIP_1910'
            },
            {
                title: '2401',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2401',
                key: 'NON_HOLD_WIP_2401'
            },
            {
                title: '2402',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2402',
                key: 'NON_HOLD_WIP_2402'
            },
            {
                title: '2403',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2403',
                key: 'NON_HOLD_WIP_2403'
            },
            {
                title: '2404',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2404',
                key: 'NON_HOLD_WIP_2404'
            },
            {
                title: '2405',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2405',
                key: 'NON_HOLD_WIP_2405'
            },
            {
                title: '2406',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2406',
                key: 'NON_HOLD_WIP_2406'
            },
            {
                title: '2407',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2407',
                key: 'NON_HOLD_WIP_2407'
            },
            {
                title: '2408',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2408',
                key: 'NON_HOLD_WIP_2408'
            },
            {
                title: '2409',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2409',
                key: 'NON_HOLD_WIP_2409'
            },
            {
                title: '2410',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2410',
                key: 'NON_HOLD_WIP_2410'
            },
            {
                title: '2420',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_2420',
                key: 'NON_HOLD_WIP_2420'
            },
            {
                title: '3000',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_3000',
                key: 'NON_HOLD_WIP_3000'
            }
        ]
    },
    {
        title: 'HOLD',
        align: 'center',
        className: 'v-center',
        children: [
            {
                title: '4000',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_4000',
                key: 'NON_HOLD_WIP_4000'
            },
            {
                title: '1100',
                align: 'center',
                dataIndex: 'HOLD_WIP_1100',
                key: 'HOLD_WIP_1100'
            },
            {
                title: '1200',
                align: 'center',
                dataIndex: 'HOLD_WIP_1200',
                key: 'HOLD_WIP_1200'
            },
            {
                title: '1300',
                align: 'center',
                dataIndex: 'HOLD_WIP_1300',
                key: 'HOLD_WIP_1300'
            },
            {
                title: '1350',
                align: 'center',
                dataIndex: 'HOLD_WIP_1350',
                key: 'HOLD_WIP_1350'
            },
            {
                title: '1400',
                align: 'center',
                dataIndex: 'HOLD_WIP_1400',
                key: 'HOLD_WIP_1400'
            },
            {
                title: '1450',
                align: 'center',
                dataIndex: 'HOLD_WIP_1450',
                key: 'HOLD_WIP_1450'
            },
            {
                title: '1500',
                align: 'center',
                dataIndex: 'HOLD_WIP_1500',
                key: 'HOLD_WIP_1500'
            },
            {
                title: '1600',
                align: 'center',
                dataIndex: 'HOLD_WIP_1600',
                key: 'HOLD_WIP_1600'
            },
            {
                title: '1700',
                align: 'center',
                dataIndex: 'HOLD_WIP_1700',
                key: 'HOLD_WIP_1700'
            },
            {
                title: '1800',
                align: 'center',
                dataIndex: 'HOLD_WIP_1800',
                key: 'HOLD_WIP_1800'
            },
            {
                title: '1850',
                align: 'center',
                dataIndex: 'HOLD_WIP_1850',
                key: 'HOLD_WIP_1850'
            },
            {
                title: '1910',
                align: 'center',
                dataIndex: 'HOLD_WIP_1910',
                key: 'HOLD_WIP_1910'
            },
            {
                title: '2401',
                align: 'center',
                dataIndex: 'HOLD_WIP_2401',
                key: 'HOLD_WIP_2401'
            },
            {
                title: '2402',
                align: 'center',
                dataIndex: 'HOLD_WIP_2402',
                key: 'HOLD_WIP_2402'
            },
            {
                title: '2403',
                align: 'center',
                dataIndex: 'HOLD_WIP_2403',
                key: 'HOLD_WIP_2403'
            },
            {
                title: '2404',
                align: 'center',
                dataIndex: 'HOLD_WIP_2404',
                key: 'HOLD_WIP_2404'
            },
            {
                title: '2405',
                align: 'center',
                dataIndex: 'HOLD_WIP_2405',
                key: 'HOLD_WIP_2405'
            },
            {
                title: '2406',
                align: 'center',
                dataIndex: 'HOLD_WIP_2406',
                key: 'HOLD_WIP_2406'
            },
            {
                title: '2407',
                align: 'center',
                dataIndex: 'HOLD_WIP_2407',
                key: 'HOLD_WIP_2407'
            },
            {
                title: '2408',
                align: 'center',
                dataIndex: 'HOLD_WIP_2408',
                key: 'HOLD_WIP_2408'
            },
            {
                title: '2409',
                align: 'center',
                dataIndex: 'HOLD_WIP_2409',
                key: 'HOLD_WIP_2409'
            },
            {
                title: '2410',
                align: 'center',
                dataIndex: 'HOLD_WIP_2410',
                key: 'HOLD_WIP_2410'
            },
            {
                title: '2420',
                align: 'center',
                dataIndex: 'HOLD_WIP_2420',
                key: 'HOLD_WIP_2420'
            },
            {
                title: '3000',
                align: 'center',
                dataIndex: 'HOLD_WIP_3000',
                key: 'HOLD_WIP_3000'
            },
            {
                title: '4000',
                align: 'center',
                dataIndex: 'HOLD_WIP_4000',
                key: 'HOLD_WIP_4000'
            }
        ]
    }
];
// 一级表头
let header = {
    key: [] as any,
    title: [] as any
};
let mutiheader = [] as any;

for (let i = 0; i <= baseColumns.length; i++) {
    if (baseColumns[i] && baseColumns[i].children && baseColumns[i].children.length) {
        baseColumns[i].children.forEach((item) => {
            header.key.push(item.dataIndex);
            header.title.push(baseColumns[i].title);
            mutiheader.push(item.title);
        });
    } else {
    }
}
export let exceljsparams = {
    Header: header,
    MutiHeader: [mutiheader],
    /* style: [
        {
            key: 'DUE_DT',
            style: {
                numFmt: 'yyyy/m/d'
            }
        },
        {
            key: 'PLAN_ST_DT',
            style: {
                numFmt: 'yyyy/m/d'
            }
        }
    ] */
};
export let detailColumns: any[] = [
    {
        title: 'Product',
        align: 'center',
        dataIndex: `PROD_NBR`,
        key: `PROD_NBR`
    },
    {
        title: 'Revision',
        align: 'center',
        dataIndex: `VER`,
        key: `VER`
    },
    {
        title: 'Panel ID',
        align: 'center',
        dataIndex: `PNL_ID`,
        key: `PNL_ID`
    },
    {
        title: 'Owner',
        align: 'center',
        dataIndex: `LCM_OWNER`,
        key: `LCM_OWNER`
    },
    {
        title: 'Operation',
        align: 'center',
        dataIndex: `WORK_CTR`,
        key: `WORK_CTR`
    },

    {
        title: 'Operation Desc',
        align: 'center',
        dataIndex: `WORK_NAME`,
        key: `WORK_NAME`
    },
    {
        title: 'Cassette ID',
        align: 'center',
        dataIndex: `CST_ID`,
        key: `CST_ID`
    },
    {
        title: 'LCD Product',
        align: 'center',
        dataIndex: `PANEL_PN`,
        key: `PANEL_PN`
    },
    {
        title: 'LCD Grade',
        align: 'center',
        dataIndex: `LCD_GRADE`,
        key: `LCD_GRADE`
    },
    {
        title: 'LCM_GRADE',
        align: 'center',
        dataIndex: `LCD_GRADE`,
        key: `LCD_GRADE`
    },
    {
        title: 'SERIAL_NO',
        align: 'center',
        dataIndex: `SERIAL_NO`,
        key: `SERIAL_NO`
    },
    {
        title: 'CART_ID',
        align: 'center',
        dataIndex: `CART_ID`,
        key: `CART_ID`
    },
    {
        title: 'PALLET_ID',
        align: 'center',
        dataIndex: `PALLET_ID`,
        key: `PALLET_ID`
    },
    {
        title: 'Exp/RMA/Resorting No',
        align: 'center',
        dataIndex: `WO_NBR`,
        key: `WO_NBR`
    },
    {
        title: 'Status',
        align: 'center',
        dataIndex: `STATUS`,
        key: `STATUS`
    },

    {
        title: 'Hold',
        align: 'center',
        dataIndex: `HOLD_FLAG`,
        key: `HOLD_FLAG`
    },
    {
        title: 'Rework',
        align: 'center',
        dataIndex: `STIMES_TTL`,
        key: `STIMES_TTL`
    },
    {
        title: 'Stay Hrs',
        align: 'center',
        dataIndex: `stayhrs`,
        key: `stayhrs`
    },
    {
        title: 'date',
        align: 'center',
        dataIndex: `TRANS_DATE`,
        key: `TRANS_DATE`
    },
    {
        title: 'Hold Category',
        align: 'center',
        dataIndex: `HOLD_CODE`,
        key: `HOLD_CODE`
    },
    {
        title: 'Hold Comments',
        align: 'center',
        dataIndex: `COMMENT1`,
        key: `COMMENT1`
    },

    {
        title: 'Priority',
        align: 'center',
        dataIndex: `PRIORITY`,
        key: `PRIORITY`
    },
    {
        title: 'start Hrs',
        align: 'center',
        dataIndex: `starthrs`,
        key: `starthrs`
    },

    {
        title: 'STPN date',
        align: 'center',
        dataIndex: `ISSUE_DATE`,
        key: `ISSUE_DATE`
    }
];
