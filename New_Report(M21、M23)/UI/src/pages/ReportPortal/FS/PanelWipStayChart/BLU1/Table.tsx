export const PanelOperWipLabel = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelOperWipProp = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelProdWipLabel = [
  'Product',
  '0~1 h',
  '1~2 h',
  '2~4 h',
  '4~8 h',
  '8~12 h',
  '12~24 h',
  '1~2 d',
  '2~3 d',
  '3~5 d',
  '大於5d',
  '大於7d',
  '大於30d',
  '大於60d',
  'sum'];
export const PanelProdWipProp = [
  'PROD_NBR',
  'SUM(QTY_0_1H)',
  "SUM(QTY_1_2H)",
  "SUM(QTY_2_4H)",
  "SUM(QTY_4_8H)",
  "SUM(QTY_8_12H)",
  "SUM(QTY_12_24H)",
  "SUM(QTY_1_2D)",
  "SUM(QTY_2_3D)",
  'SUM(QTY_3_5D)',
  "SUM(QTY_5_7D)",
  "SUM(QTY_7_30D)",
  "SUM(QTY_30_60D)",
  "SUM(QTY_60D)",
  'SUM(QTY_ALL)'
];
export const PanelWipStayDetailOperProps = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'WO_NBR',
  'LCM_OWNER',
  'WORK_CTR',
  'WORK_NAME',
  'ISSUE_DATE',
  'HOLD_CODE',
  'CUR_HOLD_COMMENT'
]
const PanelWipStayDetailOperLabel = [
  'Product',
  'Revision',
  'BL ID',
  'Wo_Nbr',
  'Owner',
  'Operation',
  'Operation Desc',
  'STPN Time',
  'Hold Code',
  'Hold Comment'

]
export const PanelWipStayDetailProdProps = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'WO_NBR',
  'LCM_OWNER',
  'TRANS_DATE',
  'TRANS_TYPE',
]
const PanelWipStayDetailProdLabel = [
  'Product',
  'Revision',
  'BL ID',
  'Wo_Nbr',
  'Owner',
  'STPN Time',
  'Hold Code',
]
function TableCustomCell(Label: string[], Prop: string[]) {
  return Prop.map((item: string, index: number) => {
    return {
      title: Label[index],
      align: 'center' as any,
      dataIndex: item,
      className: ['PROD_NBR', 'Operation'].includes(item) ? 'v-center' : ''
    }
  })
}
export const PanelWipOperColumns = TableCustomCell(PanelOperWipLabel, PanelOperWipProp);
export const PanelWipProdColumns = TableCustomCell(PanelProdWipLabel, PanelProdWipProp);
export const PanelWipDetailOperColumns = TableCustomCell(PanelWipStayDetailOperLabel, PanelWipStayDetailOperProps)
export const PanelWipDetailProdColumns = TableCustomCell(PanelWipStayDetailProdLabel, PanelWipStayDetailProdProps)
