
// 一阶报表
let FirstparentTitle = '';
let FirstchildrenTitle =
  'Work Order,WO Cust,WO Comment,WO Type,Owner,Engineer,Size,Prod Type,Lcm Product,Ver,Start Date,Due Day,Actual Start Date,Actual Comp Date,Ship Comp Date,WO Status,投入率,完成率,未入庫比率,Target Qty,Actual Qty,轉BL Recycle,實際投入,Remain,WIP(不含1800-1910),WIP(1800-1910),Complete,Scrap,Total WIP,Non-Hold,Hold';
let FirstchildrenKey =
'WO_NBR,CUSTOMER_GROUP,COMMENT1,WO_TYPE,LCM_OWNER,ENGINEER,PROD_SIZE,PROD_TYPE,PROD_NBR,VER,PLAN_ST_DT,DUE_DT,ACTUAL_START_DT,ACTUAL_CMPL_DT,SHIPPING_DATE,WO_STATUS,MVIN_RATE,MVOU_RATE,RECY_SCRAP_SHIP,TARGET_QTY,ACTUAL_QTY,QTY1,AT_QTY,REMAIN,WIP0,WIP1,SHIP_QTY,SCRAP_QTY,WIP_ALL,WIP_NON_HOLD,WIP_HOLD';
let FirstColumns = FirstchildrenKey.split(',').map((item, index) => {
    return {
      title: FirstchildrenTitle.split(',')[index].toString(),
      align: 'center',
      dataIndex: item,
      key: item,
    };
  });
let AllparantTitle = 'ALL';
let AllchildrenTitle = '1XXX(NR),1XXX(R),2XXX,3XXX,4XXX';
let Allchildrenkey = 'ALL_WIP_1XXX_NR,ALL_WIP_1XXX_R,ALL_WIP_2XXX,ALL_WIP_3XXX,ALL_WIP_4XXX';
let Allcolumn = Allchildrenkey.split(',').map((item, index) => {
  return {
    title: AllchildrenTitle.split(',')[index].toString(),
    align: 'center',
    dataIndex: item,
    key: item,
  };
});

let holdparantTitle = 'HOLD';
let holdchildrenTitle =
  '7900,8200,8380,8390,8410,8425,8430,8460,8480,8490,8610,8620,8630,8650,8700,9101,9102,9400';
let holdchildrenkey =
  'HOLD_WIP_7900,HOLD_WIP_8200,HOLD_WIP_8380,HOLD_WIP_8390,HOLD_WIP_8410,HOLD_WIP_8425,HOLD_WIP_8430,HOLD_WIP_8460,HOLD_WIP_8480,HOLD_WIP_8490,HOLD_WIP_8610,HOLD_WIP_8620,HOLD_WIP_8630,HOLD_WIP_8650,HOLD_WIP_8700,HOLD_WIP_9101,HOLD_WIP_9102,HOLD_WIP_9400';

// 合并hold
let holdcolumns = holdchildrenkey.split(',').map((item, index) => {
  return {
    title: holdchildrenTitle.split(',')[index].toString(),
    align: 'center',
    dataIndex: item,
    key: item,
  };
});
// 合并no-hold
let nonholdparamtTitle =
  'NON-HOLD';
let nonholdchildTitle =
  '7900,8200,8380,8390,8410,8425,8430,8460,8480,8490,8610,8620,8630,8650,8700,9101,9102,9400';
let nonholdchildkey =
  'NON_HOLD_WIP_7900,NON_HOLD_WIP_8200,NON_HOLD_WIP_8380,NON_HOLD_WIP_8390,NON_HOLD_WIP_8410,NON_HOLD_WIP_8425,NON_HOLD_WIP_8430,NON_HOLD_WIP_8460,NON_HOLD_WIP_8480,NON_HOLD_WIP_8490,NON_HOLD_WIP_8610,NON_HOLD_WIP_8620,NON_HOLD_WIP_8630,NON_HOLD_WIP_8650,NON_HOLD_WIP_8700,NON_HOLD_WIP_9101,NON_HOLD_WIP_9102,NON_HOLD_WIP_9400';
let nonholdcolumns = nonholdchildkey.split(',').map((item, index) => {
  return {
    title: nonholdchildTitle.split(',')[index].toString(),
    align: 'center',
    dataIndex: item,
    key: item,
  };
});

export let baseColumns: any[] = [
    {
        title: '',
        align: 'center',
        className: 'v-center',
       
        children: FirstColumns
    },
    {
        title: 'ALL',
        align: 'center',
        className: 'v-center',
        children: Allcolumn
    },
    {
        title: 'NON-HOLD',
        align: 'center',
        className: 'v-center',
        children: nonholdcolumns
    },
    {
        title: 'HOLD',
        align: 'center',
        className: 'v-center',
        children: holdcolumns
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
