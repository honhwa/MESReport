
export let detail1_1: any[] = [
  {
    title: 'Prod Nbr',
    align: 'center',
    dataIndex: `PROD_NBR`,
    key: `PROD_NBR`,
    className: "v-center",
  },
  {
    title: 'Entity No',
    align: 'center',
    dataIndex: `ENTITY_NO`,
    key: `ENTITY_NO`,
  },
  {
    title: 'Trans Date',
    align: 'center',
    dataIndex: `TRANS_DATE`,
    key: `TRANS_DATE`,
  },
  {
    title: 'Trans Type',
    align: 'center',
    dataIndex: `TRANS_TYPE`,
    key: `TRANS_TYPE`,
  },
  {
    title: 'Equip Nbr',
    align: 'center',
    dataIndex: `EQUIP_NBR`,
    key: `EQUIP_NBR`,
  },
  {
    title: 'Operator',
    align: 'center',
    dataIndex: `OPERATOR`,
    key: `OPERATOR`,
  },
  {
    title: 'Output FG',
    align: 'center',
    dataIndex: `OUTPUT_FG`,
    key: `OUTPUT_FG`,
  },
  {
    title: 'Work Ctr',
    align: 'center',
    dataIndex: `WORK_CTR`,
    key: `WORK_CTR`,
  },
  {
    title: 'To WC',
    align: 'center',
    dataIndex: `TO_WC`,
    key: `TO_WC`,
  },
  {
    title: 'Defect Code',
    align: 'center',
    dataIndex: `DFCT_CODE`,
    key: `DFCT_CODE`,
  },
  {
    title: 'Pre Dfct Code',
    align: 'center',
    dataIndex: `PRE_DFCTCODE`,
    key: `PRE_DFCTCODE`,
  },
  {
    title: 'WO Nbr',
    align: 'center',
    dataIndex: `WO_NBR`,
    key: `WO_NBR`,
  },
  {
    title: 'PC Cust',
    align: 'center',
    dataIndex: `PC_CUST`,
    key: `PC_CUST`,
  },
  {
    title: 'Rework次數',
    align: 'center',
    dataIndex: `STIMES_TTL`,
    key: `STIMES_TTL`,
  },
  {
    title: 'Dfct次數',
    align: 'center',
    dataIndex: `ITEM_NBR`,
    key: `ITEM_NBR`,
  },
  {
    title: '合計片數',
    align: 'center',
    dataIndex: `PASS_QTY`,
    key: `PASS_QTY`,
  },
]
export let detail1_2: any[] = [
  {
    title: 'Dfct Code',
    align: 'center',
    dataIndex: `DFCT_CODE`,
    key: `DFCT_CODE`,
    className: "v-center",
  },
  {
    title: 'Dfct Desc',
    align: 'center',
    dataIndex: `ERRC_DESCR`,
    key: `ERRC_DESCR`,
  },
  {
    title: 'Reason Code',
    align: 'center',
    dataIndex: `DFCT_REASON`,
    key: `DFCT_REASON`,
  },
  {
    title: 'Reason Desc',
    align: 'center',
    dataIndex: `ERRM_DESCR`,
    key: `ERRM_DESCR`,
  },
  {
    title: 'Qty',
    align: 'center',
    dataIndex: `QTY`,
    key: `QTY`,
  },
  {
    title: '%',
    align: 'center',
    dataIndex: `Rate`,
    key: `Rate`,
  },
]
export let detail1_3_left: any[] = [
  {
    title: 'Panel ID',
    align: 'center',
    dataIndex: `ENTITY_NO`,
    key: `ENTITY_NO`,
  },
  {
    title: 'Prod Nbr',
    align: 'center',
    dataIndex: `PROD_NBR`,
    key: `PROD_NBR`,
    className: "v-center",
  }
]
// COG的索引为1050
// OLB的索引为1100
// PCB的索引为1200
// PCBI的索引为1300
// ASSEMBLY 索引为 1400
// AGING索引为 1500
// C-索引 1600
// D-索引 1700
// OQC索引为 1900
// 合并数据原理，从上往下遍历，通过1100是否出现设置为一轮

export let datakey = [1050, 1100, 1200, 1300, 1400, 1500, 1600, 1700, 1900];
let parentLabel = ["COG", "OLB", "PCB", "PCBI", "ASSEMBLY", "AGING", "C -", "D -", "OQC"];
let ChildLabel = ["Eqp", "TnsDT", "TA", "trans_type", "comment"];
export let ChildProps = ["EQUIP_NBR", "TRANS_DATE", "OPERATOR", "TRANS_TYPE", "COMMENT1"];
function handlecolumns(datakey: number[], parentLabel: string[], ChildLabel: string[], ChildProps: string[]) {
  let columns: object[] = [];
  parentLabel.forEach((item, index) => {
    let childcolumns:object[] = [];
    ChildLabel.forEach((citem,cindex) => {
      childcolumns.push({
        title: citem,
        align: 'center',
        dataIndex: `${datakey[index]}-${ChildProps[cindex]}`,
        key: `${datakey[index]}-${ChildProps[cindex]}`,
      })
    });
    columns.push({
      title: item,
      align: 'center',
      key: `${datakey[index]}`,
      children: childcolumns,
    })
  })
  return columns;
}
let detail1_3_right = handlecolumns(datakey,parentLabel,ChildLabel,ChildProps)
export let detail1_3 = [...detail1_3_left,...detail1_3_right]
