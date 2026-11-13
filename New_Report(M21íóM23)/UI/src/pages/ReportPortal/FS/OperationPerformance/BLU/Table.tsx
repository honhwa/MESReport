import type { TableColumn } from '@/components/core/dynamic-table';
import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { message } from 'ant-design-vue';
import router from '@/router';
// 首页
// 上表格
let ToptableColumns = [
  {
    title: 'Operation',
    align: 'center',
    dataIndex: `WORK_CTR`,
    key: `WORK_CTR`,
  },
  {
    title: 'IN',
    align: 'center',
    dataIndex: `IN_QTY`,
    key: `IN_QTY`,
  },
  {
    title: "Pass Q'ty",
    align: 'center',
    dataIndex: `PASS_QTY`,
    key: `PASS_QTY`,
  },
  {
    title: 'To Rwk',
    align: 'center',
    dataIndex: `TOTALRWK_QTY`,
    key: `TOTALRWK_QTY`,
  },
  {
    title: 'rwk/pass Rate',
    align: 'center',
    dataIndex: `rwkpassRate`,
    key: `rwkpassRate`,
  },
  {
    title: 'to be scrap',
    align: 'center',
    dataIndex: `TOBESCRAP_QTY`,
    key: `TOBESCRAP_QTY`,
  },
  {
    title: 'WIP',
    align: 'center',
    dataIndex: `WIP_QTY`,
    key: `WIP_QTY`,
  },
  {
    title: "1'st yield",
    align: 'center',
    dataIndex: `firstyield`,
    key: `firstyield`,
  },
  {
    title: 'Total yield',
    align: 'center',
    dataIndex: `totalyield`,
    key: `totalyield`,
  },
  {
    title: '修理再發率',
    align: 'center',
    dataIndex: `repairrate`,
    key: `repairrate`,
  }
]
let ToptableClickColumn = ["PASS_QTY","TOTALRWK_QTY", "WIP_QTY"];
export let TopTable = {
  ToptableColumns,
  ClickColumn:ToptableClickColumn,

}
// 下表格
let BottomTableColums = [
  {
    title: 'Operation',
    align: 'center',
    children: [
      {
        title: 'Operation',
        align: 'center',
        dataIndex: `WORK_CTR`,
        key: `WORK_CTR`,
      },
    ]
  },
  {
    title: 'In',
    align: 'center',
    children: [
      {
        title: 'In',
        align: 'center',
        dataIndex: `IN_QTY`,
        key: `IN_QTY`,
      },
    ]
  },
  {
    title: 'Out',
    align: 'center',
    children: [
      {
        title: 'rework',
        align: 'center',
        dataIndex: `PASS_QTY`,
        key: `PASS_QTY`,
      },
      {
        title: 'rework(1)以上',
        align: 'center',
        dataIndex: `PASS2_QTY`,
        key: `PASS2_QTY`,
      },
      {
        title: 'rate',
        align: 'center',
        dataIndex: `RATE`,
        key: `RATE`,
      },
    ]
  },
  {
    title: 'To be scrap',
    align: 'center',
    children: [
      {
        title: 'To be scrap',
        align: 'center',
        dataIndex: `TOBESCRAP_QTY`,
        key: `TOBESCRAP_QTY`,
      },
    ]
  },
  {
    title: 'Wip',
    align: 'center',
    children: [
      {
        title: 'Wip',
        align: 'center',
        dataIndex: `WIP_QTY`,
        key: `WIP_QTY`,
      },
    ]
  },
]
let BottomtableClickColumn = ["IN_QTY", "PASS_QTY", "PASS2_QTY", "RATE", "TOBESCRAP_QTY"];
export let BottomTable = {
  BottomTableColums,
  ClickColumn:BottomtableClickColumn,
}
// 一阶
// 上表格點擊pass'qty
let PassqtyColumnsLabel = ['機種名稱', '版本', '機台', '班别', '投產數', '良品數', 'Rework'];
let PassqtyColumnsProp = ['PROD_NBR', 'VER', 'EQUIP_NBR', 'SHIFT_ID', 'PASS_QTY', 'GOOD_PASS', 'RWK_QTY'];
let PassQtyClickColumn = ['PASS_QTY']
export let Toppassqty = {
  Label:PassqtyColumnsLabel,
  Props:PassqtyColumnsProp,
  ClickColumn:PassQtyClickColumn,
  Title: "BLA Operation Performance Pass Q'ty",
  MergetList: ['PROD_NBR'],
  CalcuList: ['PASS_QTY', 'GOOD_PASS', 'RWK_QTY']
}
// 上表格点击 to rwk,暂时无数据测试不了




// 上表格点击Wip
let WipQtyLabel = ['product', 'Reversion', 'Qty'];
let WipQtyProp = ['PROD_NBR', 'VER', 'COUNT'];
export let TopWip = {
  WipQtyLabel,
  WipQtyProp,
  CalcuList: ['COUNT'],
  Title: "BLA Wip Q'ty",
  ClickColumn:["COUNT"]

}

// 下表格点击In,rework,rework（1以上），rate
let InLabel = ['Operation', 'Qty'];
let InProp = ['WORK_CTR', 'QTY'];
export let BottomIn = {
  Label:InLabel,
  Props:InProp,
  ClickColumn: ["QTY"],
  Title: "BLA Rework ",
  CalcuList: ['QTY'],
}
// 下表格的tobeScrap
let TobescrapLabel = ['Scrap Code', 'Description', "Qty"];
let TobescrapProp = ['CODE', 'CODE_DESC', 'COUNT'];
export let BottomTobeScrap = {
  Label:TobescrapLabel,
  Props:TobescrapProp,
  ClickColumn: ["COUNT"],
  Title: "BLA Operation Performance Scrap Q'ty ",
  CalcuList: ['COUNT'],
}
// 下表格的wip，使用二阶的BottomInsecond






// 二阶
// 上表格點擊pass'qty
let PassqtyColumnssecondLabel = ['prod_nbr', 'entity_no', 'trans_date', 'trans_type', 'equip_nbr', 'operator','work_ctr', 'output_fg',  'to_wc', 'defect_code','comment', 'rework次数', 'dfct次数', '合计片数'];
let PassqtyColumnssecondProp = ['PROD_NBR', 'ENTITY_NO', 'TRANS_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR','WORK_CTR', 'OUTPUT_FG',  'TO_WC', 'DFCT_CODE', 'COMMENT1','STIMES_TTL', 'ITEM_NBR', 'PASS_QTY'];
export let Toppassqtysecond = {
  Label:PassqtyColumnssecondLabel,
  Props:PassqtyColumnssecondProp,
  Title: "LCMA Operation Performance Detail",
  MergetList: ['PROD_NBR'],
  CalcuList: ['PASS_QTY'],
  insertrow: {
    insertList: ["PASS_QTY"],
    condition: ["PROD_NBR"]
  }
}
// 上表格的wip

let wipsecondLabel = ['prod_nbr','acct_date','pnl_id','last_eqp_nbr','work_ctr', 'rework次数', '合计片数'];
let wipsecondprop = ['PROD_NBR','ACCT_DATE',"PNL_ID","LAST_EQP_NBR", 'WORK_CTR','STIMES_TTL', 'QTY'];
export let Wipsecond = {
  Label:wipsecondLabel,
  Props:wipsecondprop,
  Title: "LCMA Operation Performance Detail",
  MergetList: ['PROD_NBR','ACCT_DATE'],
  CalcuList: ['QTY'],
  insertrow: {
    insertList: ["QTY"],
    condition: ["PROD_NBR"]
  }
}
// 下表格点击in,rework,rework（1以上），rate，tobeScrap
let insecondLabel = [ 'prod_nbr', 'acct_date', 'trans_type', 'equip_nbr', 'operator', 'output_fg', 'work_ctr', 'to_wc', 'rework次数', '合计片数'];
let insecondProp = [ 'PROD_NBR', 'ACCT_DATE', 'TRANS_TYPE', 'EQUIP_NBR', 'OPERATOR', 'OUTPUT_FG', 'WORK_CTR', 'TO_WC', 'STIMES_TTL', 'QTY'];

export let BottomInsecond = {
  Label:insecondLabel,
  Props:insecondProp,
  MergetList: ['PROD_NBR'],
  CalcuList: ['QTY'],
  insertrow: {
    insertList: ["QTY"],
    condition: ["PROD_NBR"]
  },
  Title:"BLA Operation Performance Detail"
}



let FactoryType = ""
export function pushFactoryType(sFactoryType:string) {
  FactoryType  = sFactoryType;
}

// 三阶



let OperationColumsData = { Toppassqty, BottomInsecond, Toppassqtysecond, BottomTobeScrap, BottomIn, TopWip }
export default OperationColumsData