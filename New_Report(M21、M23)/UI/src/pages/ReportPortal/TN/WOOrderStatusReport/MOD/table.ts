//需要计算的列
export let CalcuList = [
  'TARGET_QTY', 'ACTUAL_QTY',
  'QTY1', 'AT_QTY', 'REMAIN',
  'WIP0', 'WIP1', 'SHIP_QTY',
  'SCRAP_QTY', 'WIP_ALL', 'WIP_NON_HOLD',
  'WIP_HOLD', 'ALL_WIP_1XXX_NR', 'ALL_WIP_1XXX_R',
  'ALL_WIP_2XXX', 'ALL_WIP_3XXX', 'ALL_WIP_4XXX',
  'NON_HOLD_WIP_1100', 'NON_HOLD_WIP_1200', 'NON_HOLD_WIP_1300',
  'NON_HOLD_WIP_1350', 'NON_HOLD_WIP_1400', 'NON_HOLD_WIP_1450',
  'NON_HOLD_WIP_1500', 'NON_HOLD_WIP_1600', 'NON_HOLD_WIP_1700',
  'NON_HOLD_WIP_1800', 'NON_HOLD_WIP_1850', 'NON_HOLD_WIP_1910',
  'NON_HOLD_WIP_2401', 'NON_HOLD_WIP_2402', 'NON_HOLD_WIP_2403',
  'NON_HOLD_WIP_2404', 'NON_HOLD_WIP_2405', 'NON_HOLD_WIP_2406',
  'NON_HOLD_WIP_2407', 'NON_HOLD_WIP_2408', 'NON_HOLD_WIP_2409',
  'NON_HOLD_WIP_2410', 'NON_HOLD_WIP_2420', 'NON_HOLD_WIP_3000',
  'NON_HOLD_WIP_4000', 'HOLD_WIP_1100', 'HOLD_WIP_1200',
  'HOLD_WIP_1300', 'HOLD_WIP_1350', 'HOLD_WIP_1400',
  'HOLD_WIP_1450', 'HOLD_WIP_1500', 'HOLD_WIP_1600',
  'HOLD_WIP_1700', 'HOLD_WIP_1800', 'HOLD_WIP_1850',
  'HOLD_WIP_1910', 'HOLD_WIP_2401', 'HOLD_WIP_2402',
  'HOLD_WIP_2403', 'HOLD_WIP_2404', 'HOLD_WIP_2405',
  'HOLD_WIP_2406', 'HOLD_WIP_2407', 'HOLD_WIP_2408',
  'HOLD_WIP_2409', 'HOLD_WIP_2410', 'HOLD_WIP_2420',
  'HOLD_WIP_3000', 'HOLD_WIP_4000'
]


// 可点击列
export let ClickColumn = ['WO_NBR'];


export let baseColumns: any[] = [
  {
    title: 'Work Order',
    align: 'center',
    dataIndex: `WO_NBR`,
    key: `WO_NBR`,
    fixed: 'left',
    className: 'v-center'
    // width: 100,
  },
  {
    title: 'WO Status',
    align: 'center',
    dataIndex: `WO_STATUS`,
    key: `WO_STATUS`,
  },
  {
    title: 'LCM Produst',
    align: 'center',
    dataIndex: `PROD_NBR`,
    key: `PROD_NBR`,
  },
  {
    title: 'Ver',
    align: 'center',
    dataIndex: `VER`,
    key: `VER`,
  },
  {
    title: 'Owner',
    align: 'center',
    dataIndex: `LCM_OWNER`,
    key: `LCM_OWNER`,
  },
  {
    title: 'WO Type',
    align: 'center',
    dataIndex: `WO_TYPE`,
    key: `WO_TYPE`,
  },
  {
    title: 'Release Date',
    align: 'center',
    dataIndex: `RELEASE_DT`,
    key: `RELEASE_DT`,
  },
  {
    title: 'Start Date',
    align: 'center',
    dataIndex: `PLAN_ST_DT`,
    key: `PLAN_ST_DT`,
  },
  {
    title: 'Due Date',
    align: 'center',
    dataIndex: `DUE_DT`,
    key: `DUE_DT`,
  },
  {
    title: 'Enginner',
    align: 'center',
    dataIndex: `ENGINNER`,
    key: `ENGINNER`,
  },
  {
    title: 'Size',
    align: 'center',
    dataIndex: `PROD_SIZE`,
    key: `PROD_SIZE`,
  },
  {
    title: 'Prod Type',
    align: 'center',
    dataIndex: `PROD_TYPE`,
    key: `PROD_TYPE`,
  },
  {
    title: 'WO Comment',
    align: 'center',
    dataIndex: `COMMENT1`,
    key: `COMMENT1`,
  },
  {
    title: '修量',
    align: 'center',
    dataIndex: `NEW_QTY`,
    key: `NEW_QTY`,
  },
  {
    title: 'Target Qty',
    align: 'center',
    dataIndex: `TARGET_QTY`,
    key: `TARGET_QTY`,
  },
  {
    title: 'Actual Qty',
    align: 'center',
    dataIndex: `ACTUAL_QTY`,
    key: `ACTUAL_QTY`,
  },
  {
    title: 'Change',
    align: 'center',
    dataIndex: `CHANGE_QTY`,
    key: `CHANGE_QTY`,
  },
  {
    title: 'Remain',
    align: 'center',
    dataIndex: `REMAIN`,
    key: `REMAIN`,
  },
  {
    title: '投入率',
    align: 'center',
    dataIndex: `MVIN_RATE`,
    key: `MVIN_RATE`,
  },
  {
    title: '完成率',
    align: 'center',
    dataIndex: `MVOU_RATE`,
    key: `MVOU_RATE`,
  },
  {
    title: 'Bonding wip(1XXX-13XX)',
    align: 'center',
    dataIndex: `WIP_BOND`,
    key: `WIP_BOND`,
  },
  {
    title: 'LAM wip',
    align: 'center',
    dataIndex: `WIP_LAM`,
    key: `WIP_LAM`,
  },
  {
    title: 'ASSY wip(14XX)',
    align: 'center',
    dataIndex: `WIP_ASSY`,
    key: `WIP_ASSY`,
  },
  {
    title: 'CD wip(15XX-17XX)',
    align: 'center',
    dataIndex: `WIP_CD1`,
    key: `WIP_CD1`,
  },
  {
    title: 'CD wip(18XX-19XX)',
    align: 'center',
    dataIndex: `WIP_CD2`,
    key: `WIP_CD2`,
  },
  {
    title: 'RW wip(2XXX)',
    align: 'center',
    dataIndex: `WIP_RWK`,
    key: `WIP_RWK`,
  },
  {
    title: 'Ship',
    align: 'center',
    dataIndex: `SHIP_QTY`,
    key: `SHIP_QTY`,
  },
  {
    title: 'Scrap',
    align: 'center',
    dataIndex: `SCRAP_QTY`,
    key: `SCRAP_QTY`,
  },
  {
    title: 'Total WIP',
    align: 'center',
    dataIndex: `WIP_ALL`,
    key: `WIP_ALL`,
  },
  {
    title: 'Non-Hold',
    align: 'center',
    dataIndex: `WIP_NON_HOLD`,
    key: `WIP_NON_HOLD`,
  },
  {
    title: 'Hold',
    align: 'center',
    dataIndex: `WIP_HOLD`,
    key: `WIP_HOLD`,
  },
  {
    title: 'NON-HOLD',
    align: 'center',
    className: "v-center"
  },
  {
    title: 'HOLD',
    align: 'center',
    className: "v-center"
  }
];
export let detailColumns: any[] = [
  {
    title: 'Product',
    align: 'center',
    dataIndex: `PROD_NBR`,
    key: `PROD_NBR`,
  },
  {
    title: 'Revision',
    align: 'center',
    dataIndex: `VER`,
    key: `VER`,
  },
  {
    title: 'Panel ID',
    align: 'center',
    dataIndex: `PNL_ID`,
    key: `PNL_ID`,
  },
  {
    title: 'Owner',
    align: 'center',
    dataIndex: `LCM_OWNER`,
    key: `LCM_OWNER`,
  },
  {
    title: 'Operation',
    align: 'center',
    dataIndex: `WORK_CTR`,
    key: `WORK_CTR`,
  },

  {
    title: 'Operation Desc',
    align: 'center',
    dataIndex: `WORK_NAME`,
    key: `WORK_NAME`,
  }, {
    title: 'Cassette ID',
    align: 'center',
    dataIndex: `CST_ID`,
    key: `CST_ID`,
  },
  {
    title: 'LCD Product',
    align: 'center',
    dataIndex: `PANEL_PN`,
    key: `PANEL_PN`,
  },
  {
    title: 'LCD Grade',
    align: 'center',
    dataIndex: `LCD_GRADE`,
    key: `LCD_GRADE`,
  },
  {
    title: 'LCM_GRADE',
    align: 'center',
    dataIndex: `LCD_GRADE`,
    key: `LCD_GRADE`,
  },
  {
    title: 'SERIAL_NO',
    align: 'center',
    dataIndex: `SERIAL_NO`,
    key: `SERIAL_NO`,
  },
  {
    title: 'CART_ID',
    align: 'center',
    dataIndex: `CART_ID`,
    key: `CART_ID`,
  },
  {
    title: 'PALLET_ID',
    align: 'center',
    dataIndex: `PALLET_ID`,
    key: `PALLET_ID`,
  },
  {
    title: 'Exp/RMA/Resorting No',
    align: 'center',
    dataIndex: `WO_NBR`,
    key: `WO_NBR`,
  },
  {
    title: 'Status',
    align: 'center',
    dataIndex: `STATUS`,
    key: `STATUS`,
  },

  {
    title: 'Hold',
    align: 'center',
    dataIndex: `HOLD_FLAG`,
    key: `HOLD_FLAG`,
  },
  {
    title: 'Rework',
    align: 'center',
    dataIndex: `STIMES_TTL`,
    key: `STIMES_TTL`,
  },
  {
    title: 'Stay Hrs',
    align: 'center',
    dataIndex: `stayhrs`,
    key: `stayhrs`,
  },
  {
    title: 'date',
    align: 'center',
    dataIndex: `TRANS_DATE`,
    key: `TRANS_DATE`,
  },
  {
    title: 'Hold Category',
    align: 'center',
    dataIndex: `HOLD_CODE`,
    key: `HOLD_CODE`,
  },
  {
    title: 'Hold Comments',
    align: 'center',
    dataIndex: `COMMENT1`,
    key: `COMMENT1`,
  },

  {
    title: 'Priority',
    align: 'center',
    dataIndex: `PRIORITY`,
    key: `PRIORITY`,
  },
  {
    title: 'start Hrs',
    align: 'center',
    dataIndex: `starthrs`,
    key: `starthrs`,
  },

  {
    title: 'STPN date',
    align: 'center',
    dataIndex: `ISSUE_DATE`,
    key: `ISSUE_DATE`,
  },
  {
    title: 'WO Cust',
    align: 'center',
    dataIndex: `WO_CUST`,
    key: `WO_CUST`,
  },
]
