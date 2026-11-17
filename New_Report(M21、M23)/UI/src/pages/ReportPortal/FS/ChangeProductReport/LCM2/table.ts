export let middle: any[] = [
  {
    label: "Operation",
    value: "WORK_CTR",
  },
  {
    label: "轉工單時間",
    value: "TRANS_DATE",
  },
  {
    label: "線別",
    value: "EQUIP_NBR",
  },
  {
    label: "樓層",
    value: "FLOOR",
  },
  {
    label: "Old owner",
    value: "lcm_owner",
  },
  {
    label: "New owner",
    value: "to_lcm_owner",
  },
  {
    label: "Old prod",
    value: "prod_nbr",
  },
  {
    label: "New prod",
    value: "to_prod_nbr",
  },
  {
    label: "Old work nbr",
    value: "wo_nbr",
  },
  {
    label: "New work nbr",
    value: "to_wo_nbr",
  },
  {
    label: "等級",
    value: "LCM_GRADE",
  },
];
export let leftColumns = {
  title: 'Change Type',
  align: 'center',
  dataIndex: 'P2E',
  className: "v-center",
}
export let detailColumns = [
  {
    title: "Panel ID",
    align: 'center',
    dataIndex: 'ENTITY_NO',
    key:"ENTITY_NO",
  },
  {
    title: 'Operation',
    align: 'center',
    dataIndex: 'WORK_CTR',
    key:"WORK_CTR",
  },
  {
    title: 'Old Owner',
    align: 'center',
    dataIndex: 'LCM_OWNER',
    key:"LCM_OWNER",

  },
  {
    title: 'New Owner',
    align: 'center',
    dataIndex: 'TO_LCM_OWNER',
    key:"TO_LCM_OWNER",

  },
  {
    title: 'Old Prod',
    align: 'center',
    dataIndex: 'PROD_NBR',
    key:"PROD_NBR",

  },
  {
    title: 'New Prod',
    align: 'center',
    dataIndex: 'TO_PROD_NBR',
    key:"TO_PROD_NBR",

  },
  {
    title: 'Old Work Order',
    align: 'center',
    dataIndex: 'WO_NBR',
    key:"WO_NBR",

  },
  {
    title: 'New Work Order',
    align: 'center',
    dataIndex: 'TO_WO_NBR',
    key:"TO_WO_NBR",

  },
  {
    title: 'Trans Date',
    align: 'center',
    dataIndex: 'TRANS_DATE',
    key:"TRANS_DATE",
  },

]
