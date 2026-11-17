//需要计算的列
export let CalcuList = [
  'TARGET_QTY',
  'ACTUAL_QTY',
  'REMAIN',
  'SHIP_QTY',
  'WIP_ALL',
  'WIP_HOLD',
  'ALL_WIP_1310',
  'ALL_WIP_1320',
  'ALL_WIP_1330',
  'ALL_WIP_1340',
  'ALL_WIP_1345',
  'ALL_WIP_1350',
  'ALL_WIP_1360',
  'ALL_WIP_1370',
  'ALL_WIP_1380',
  'ALL_WIP_1390',
  'ALL_WIP_3500',
];

// 可点击列
export let ClickColumn = [
 
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
        className: 'v-center',
      },
      {
        title: 'WO Comment',
        align: 'center',
        dataIndex: `COMMENT1`,
        key: `COMMENT1`,
      },
      {
        title: 'WO Type',
        align: 'center',
        dataIndex: `WO_TYPE`,
        key: `WO_TYPE`,
      },
      {
        title: 'Owner',
        align: 'center',
        dataIndex: `LCM_OWNER`,
        key: `LCM_OWNER`,
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
        dataIndex: `PROD_SIZE`,
        key: `PROD_SIZE`,
      },
      {
        title: 'Lcm Product',
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
        title: 'Start Date',
        align: 'center',
        dataIndex: `PLAN_ST_DT`,
        key: `PLAN_ST_DT`,
      },
      {
        title: 'Due Day',
        align: 'center',
        dataIndex: `DUE_DT`,
        key: `DUE_DT`,
      },
      {
        title: 'WO Status',
        align: 'center',
        dataIndex: `WO_STATUS`,
        key: `WO_STATUS`,
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
        title: 'Remain',
        align: 'center',
        dataIndex: `REMAIN`,
        key: `REMAIN`,
      },
      {
        title: 'Complete',
        align: 'center',
        dataIndex: `SHIP_QTY`,
        key: `SHIP_QTY`,
      },
      {
        title: 'Total WIP',
        align: 'center',
        dataIndex: `WIP_ALL`,
        key: `WIP_ALL`,
      }
    ],
  },
  {
    title: 'ALL',
    align: 'center',
    className: 'v-center',
    children: [
      { title: '1310', align: 'center', dataIndex: 'ALL_WIP_1310' },
      { title: '1320', align: 'center', dataIndex: 'ALL_WIP_1320' },
      { title: '1330', align: 'center', dataIndex: 'ALL_WIP_1330' },
      { title: '1340', align: 'center', dataIndex: 'ALL_WIP_1340' },
      { title: '1345', align: 'center', dataIndex: 'ALL_WIP_1345' },
      { title: '1350', align: 'center', dataIndex: 'ALL_WIP_1350' },
      { title: '1360', align: 'center', dataIndex: 'ALL_WIP_1360' },
      { title: '1370', align: 'center', dataIndex: 'ALL_WIP_1370' },
      { title: '1380', align: 'center', dataIndex: 'ALL_WIP_1380' },
      { title: '1390', align: 'center', dataIndex: 'ALL_WIP_1390' },
      { title: '3500', align: 'center', dataIndex: 'ALL_WIP_3500' },
    ],
  },
];
// 一级表头
let header = {
  key: [] as any,
  title: [] as any,
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
  },
  {
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
];
