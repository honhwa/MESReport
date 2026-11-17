
// 一阶报表
export let baseColumns: any[] = [
    {
        title: '',
        align: 'center',
        className: 'v-center',
        // fixed: 'left',
        // width: 100,
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
                title: 'WIP(不含8600-8700)',
                align: 'center',
                dataIndex: `WIP0`,
                key: `WIP0`
            },
            {
                title: 'WIP(8600-8700)',
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
                title: '8XXX(NR)',
                align: 'center',
                dataIndex: `ALL_WIP_8XXX_NR`,
                key: `ALL_WIP_8XXX_NR`
            },
            {
                title: '8XXX(R)',
                align: 'center',
                dataIndex: `ALL_WIP_8XXX_R`,
                key: `ALL_WIP_8XXX_R`
            },
            {
                title: '9XXX',
                align: 'center',
                dataIndex: `ALL_WIP_9XXX`,
                key: `ALL_WIP_9XXX`
            },
            {
                title: 'XXXX',
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
                title: '8100',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8100',
                key: 'NON_HOLD_WIP_8100'
            },
            {
                title: '8200',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8200',
                key: 'NON_HOLD_WIP_8200'
            },
            {
                title: '8300',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8300',
                key: 'NON_HOLD_WIP_8300'
            },
            {
                title: '8350',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8350',
                key: 'NON_HOLD_WIP_8350'
            },
            {
                title: '8390',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8390',
                key: 'NON_HOLD_WIP_8390'
            },
            {
                title: '8410',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8410',
                key: 'NON_HOLD_WIP_8410'
            },
            {
                title: '8420',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8420',
                key: 'NON_HOLD_WIP_8420'
            },
            {
                title: '8425',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8425',
                key: 'NON_HOLD_WIP_8425'
            },
            {
                title: '8430',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8430',
                key: 'NON_HOLD_WIP_8430'
            },
            {
                title: '8450',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8450',
                key: 'NON_HOLD_WIP_8450'
            },
            {
                title: '8460',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8460',
                key: 'NON_HOLD_WIP_8460'
            },
            {
                title: '8470',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8470',
                key: 'NON_HOLD_WIP_8470'
            },
            {
                title: '8480',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8480',
                key: 'NON_HOLD_WIP_8480'
            },
            {
                title: '8490',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8490',
                key: 'NON_HOLD_WIP_8490'
            },
            {
                title: '8500',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8500',
                key: 'NON_HOLD_WIP_8500'
            },
            {
                title: '8600',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8600',
                key: 'NON_HOLD_WIP_8600'
            },
            {
                title: '8610',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8610',
                key: 'NON_HOLD_WIP_8610'
            },
            {
                title: '8620',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8620',
                key: 'NON_HOLD_WIP_8620'
            },
            {
                title: '8630',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8630',
                key: 'NON_HOLD_WIP_8630'
            },
            {
                title: '8700',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_8700',
                key: 'NON_HOLD_WIP_8700'
            },
            {
                title: '9101',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_9101',
                key: 'NON_HOLD_WIP_9101'
            },
            {
                title: '9102',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_9102',
                key: 'NON_HOLD_WIP_9102'
            },
            {
                title: '9400',
                align: 'center',
                dataIndex: 'NON_HOLD_WIP_9400',
                key: 'NON_HOLD_WIP_9400'
            }
        ]
    },
    {
        title: 'HOLD',
        align: 'center',
        className: 'v-center',
        children: [
            {
                title: '8100',
                align: 'center',
                dataIndex: 'HOLD_WIP_8100',
                key: 'HOLD_WIP_8100'
            },
            {
                title: '8200',
                align: 'center',
                dataIndex: 'HOLD_WIP_8200',
                key: 'HOLD_WIP_8200'
            },
            {
                title: '8300',
                align: 'center',
                dataIndex: 'HOLD_WIP_8300',
                key: 'HOLD_WIP_8300'
            },
            {
                title: '8350',
                align: 'center',
                dataIndex: 'HOLD_WIP_8350',
                key: 'HOLD_WIP_8350'
            },
            {
                title: '8390',
                align: 'center',
                dataIndex: 'HOLD_WIP_8390',
                key: 'HOLD_WIP_8390'
            },
            {
                title: '8410',
                align: 'center',
                dataIndex: 'HOLD_WIP_8410',
                key: 'HOLD_WIP_8410'
            },
            {
                title: '8420',
                align: 'center',
                dataIndex: 'HOLD_WIP_8420',
                key: 'HOLD_WIP_8420'
            },
            {
                title: '8425',
                align: 'center',
                dataIndex: 'HOLD_WIP_8425',
                key: 'HOLD_WIP_8425'
            },
            {
                title: '8430',
                align: 'center',
                dataIndex: 'HOLD_WIP_8430',
                key: 'HOLD_WIP_8430'
            },
            {
                title: '8450',
                align: 'center',
                dataIndex: 'HOLD_WIP_8450',
                key: 'HOLD_WIP_8450'
            },
            {
                title: '8460',
                align: 'center',
                dataIndex: 'HOLD_WIP_8460',
                key: 'HOLD_WIP_8460'
            },
            {
                title: '8470',
                align: 'center',
                dataIndex: 'HOLD_WIP_8470',
                key: 'HOLD_WIP_8470'
            },
            {
                title: '8480',
                align: 'center',
                dataIndex: 'HOLD_WIP_8480',
                key: 'HOLD_WIP_8480'
            },
            {
                title: '8490',
                align: 'center',
                dataIndex: 'HOLD_WIP_8490',
                key: 'HOLD_WIP_8490'
            },
            {
                title: '8500',
                align: 'center',
                dataIndex: 'HOLD_WIP_8500',
                key: 'HOLD_WIP_8500'
            },
            {
                title: '8600',
                align: 'center',
                dataIndex: 'HOLD_WIP_8600',
                key: 'HOLD_WIP_8600'
            },
            {
                title: '8610',
                align: 'center',
                dataIndex: 'HOLD_WIP_8610',
                key: 'HOLD_WIP_8610'
            },
            {
                title: '8620',
                align: 'center',
                dataIndex: 'HOLD_WIP_8620',
                key: 'HOLD_WIP_8620'
            },
            {
                title: '8630',
                align: 'center',
                dataIndex: 'HOLD_WIP_8630',
                key: 'HOLD_WIP_8630'
            },
            {
                title: '8700',
                align: 'center',
                dataIndex: 'HOLD_WIP_8700',
                key: 'HOLD_WIP_8700'
            },
            {
                title: '9101',
                align: 'center',
                dataIndex: 'HOLD_WIP_9101',
                key: 'HOLD_WIP_9101'
            },
            {
                title: '9102',
                align: 'center',
                dataIndex: 'HOLD_WIP_9102',
                key: 'HOLD_WIP_9102'
            },
            {
                title: '9400',
                align: 'center',
                dataIndex: 'HOLD_WIP_9400',
                key: 'HOLD_WIP_9400'
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
};
let Calclick: string[] = [];
baseColumns.forEach((item, index) => {
    if (index != 0) {
        item.children.forEach((iitem) => {
          Calclick.push(iitem.dataIndex)
        });
    }
});
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
  ...Calclick,
  
];

// 可点击列
export let ClickColumn = [
  'REMAIN',
  'WIP0',
  'WIP1',
  'SCRAP_QTY',
  'WIP_NON_HOLD',
  ...Calclick,
  
];
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
