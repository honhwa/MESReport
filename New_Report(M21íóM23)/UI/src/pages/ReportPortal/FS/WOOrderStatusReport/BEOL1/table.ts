//需要计算的列
export let CalcuList = [
  "TOTAL_WIP",
  "TARGET_QTY",
  "START_QTY",
  "BOND_QTY",
  "STB_QTY",
  "COMPLETE_QTY",
  "SCRAP",
]
// 可点击列
export let ClickColumn = [
  "TOTAL_WIP"
]

export let baseColumns: any[] = [
  {
    title: 'Work Order',
    align: 'center',
    dataIndex: `WORK_ORDER_ID`,
    key: `WORK_ORDER_ID`,
    fixed: 'left',
    className: 'v-center'
  },
  {
    title: 'Total WIP',
    align: 'center',
    dataIndex: `TOTAL_WIP`,
    key: `TOTAL_WIP`,
  },
  {
    title: 'Wo Comment',
    align: 'center',
    dataIndex: `WO_COMMENT`,
    key: `WO_COMMENT`,
  },
  {
    title: 'Type',
    align: 'center',
    dataIndex: `TYPE1`,
    key: `TYPE1`,
  },
  {
    title: 'PFCD',
    align: 'center',
    dataIndex: `PFCD`,
    key: `PFCD`,
  },
  {
    title: 'Owner',
    align: 'center',
    dataIndex: `OWNER`,
    key: `OWNER`,
  },
  {
    title: 'Engineer',
    align: 'center',
    dataIndex: `ENGINEER`,
    key: `ENGINEER`,
  },
  {
    title: 'Size',
    align: 'center',
    dataIndex: `SIZE1`,
    key: `SIZE1`,
  },
  {
    title: 'WO Status',
    align: 'center',
    dataIndex: `WO_STATUS`,
    key: `WO_STATUS`,
  },
  {
    title: 'Release Date',
    align: 'center',
    dataIndex: `RELEASE_DATE`,
    key: `RELEASE_DATE`,
  },
  {
    title: 'Start Date',
    align: 'center',
    dataIndex: `START_DATE`,
    key: `START_DATE`,
  },
  {
    title: 'Due Day',
    align: 'center',
    dataIndex: `DUE_DATE`,
    key: `DUE_DATE`,
  },
  {
    title: '目標量',
    align: 'center',
    dataIndex: `TARGET_QTY`,
    key: `TARGET_QTY`,
  },
  {
    title: '下線量',
    align: 'center',
    dataIndex: `START_QTY`,
    key: `START_QTY`,
  },
  {
    title: '已綁量',
    align: 'center',
    dataIndex: `BOND_QTY`,
    key: `BOND_QTY`,
  },
  {
    title: 'STB量',
    align: 'center',
    dataIndex: `STB_QTY`,
    key: `STB_QTY`,
  },
  {
    title: 'Route ID',
    align: 'center',
    dataIndex: `ROUTE_ID`,
    key: `ROUTE_ID`,
  },
  {
    title: '投入率',
    align: 'center',
    dataIndex: `START_PER`,
    key: `START_PER`,
  },
  {
    title: '完成率',
    align: 'center',
    dataIndex: `COMPLETE_PER`,
    key: `COMPLETE_PER`,
  },
  {
    title: 'Complete',
    align: 'center',
    dataIndex: `COMPLETE_QTY`,
    key: `COMPLETE_QTY`,
  },
  {
    title: 'Scrap',
    align: 'center',
    dataIndex: `SCRAP`,
    key: `SCRAP`,
  },
]
export let exceljsparams = {
  
};
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
]
