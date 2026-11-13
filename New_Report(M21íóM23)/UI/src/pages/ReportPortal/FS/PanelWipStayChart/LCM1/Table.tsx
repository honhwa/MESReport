export const PanelOperWipLabel = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelOperWipProp = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelProdWipLabel = ['Product', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelProdWipProp = ['PROD_NBR', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'];
export const PanelWipStayDetailProps = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'WO_NBR',
  'WO_CUST',
  'CUSTOMER_GROUP',
  'LCM_OWNER',
  'WORK_CTR',
  'WORK_NAME',
  'ISSUE_DATE',
  'HOLD_CODE',
  'CUR_HOLD_COMMENT'
]
const PanelWipStayDetailLabel = [
  'Product',
  'Revision',
  'Panel ID',
  'Wo_Nbr',
  'Wo_Cust',
  'Packing_Cust',
  'Owner',
  'Operation',
  'Operation Desc',
  'STPN Time',
  'Hold Code',
  'Hold Comment'
]

const SET2DetailLabel = [
  'Product',
  'Revision',
  'Panel ID',
  '模組機種',
  '模組等級',
  'LCM Owner',
  '時間戳',
  'Wo_Nbr',
  'Wo_Cust',
  'Packing_Cust',
  'Owner',
  'Operation',
  'Operation Desc',
  'STPN Time',
  'Hold Code',
  'Hold Comment'
];
export const SET2DetailProp = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'LCD_PROD_NBR',
  "LCD_GRADE",
  "LCD_OWNER",
  "TRANS_DATE",
  'WO_NBR',
  'WO_CUST',
  'CUSTOMER_GROUP',
  'LCM_OWNER',
  'WORK_CTR',
  'WORK_NAME',
  'ISSUE_DATE',
  'HOLD_CODE',
  'CUR_HOLD_COMMENT'
];
function TableCustomCell(Label: string[], Prop: string[]) {
  return Prop.map((item: string, index: number) => {
    return {
      title: Label[index],
      // align: ['1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '大於5d', '大於7d', 'sum'].includes(item) ? 'right':'center' ,
      dataIndex: item,
      className: ['PROD_NBR', 'Operation'].includes(item) ? 'v-center' : ''
    }
  })
}
export const PanelWipOperColumns = TableCustomCell(PanelOperWipLabel, PanelOperWipProp);
export const PanelWipProdColumns = TableCustomCell(PanelProdWipLabel, PanelProdWipProp);
export const PanelWipDetailColumns = TableCustomCell(PanelWipStayDetailLabel, PanelWipStayDetailProps)
export const SET2DetailColumns = TableCustomCell(SET2DetailLabel, SET2DetailProp)
const LCM2Label = [
  " 箱號",
  "棧板號",
  "Panel ID",
  "LCD Grade",
  "LCD Owner",
  "ERP 料號",
  "人員",
  "Operation",
  "Operation Desc",
  "STPN Time",
  "作業時間",
  "Hold Comment"
];
const LCM2Prop = [
  "PROD_NBR",
  "VER",
  "PNL_ID",
  "WO_NBR",
  "WO_CUST",
  "CUSTOMER_GROUP",
  "LCM_OWNER",
  "WORK_CTR",
  "WORK_NAME",
  "LAST_TRANS_DT",
  "HOLD_CODE",
  "CUR_HOLD_COMMENT",
];
export const LCM2DetailColumns = TableCustomCell(LCM2Label, LCM2Prop)
const LCM2Label2 = [
  'Product',
  'Revision',
  'Panel ID',
  'Wo_Nbr',
  'Wo_Cust',
  'Packing_Cust',
  'Owner',
  'Operation',
  'Operation Desc',
  "下綫時間",
  "入站時間",
  'Hold Code',
  'Hold Comment'
]
 const LCM2Prop2 = [
  'PROD_NBR',
  'VER',
  'PNL_ID',
  'WO_NBR',
  'WO_CUST',
  'CUSTOMER_GROUP',
  'LCM_OWNER',
  'WORK_CTR',
  'WORK_NAME',
  "ISSUE_DATE",
  "LAST_TRANS_DT",
  'HOLD_CODE',
  'CUR_HOLD_COMMENT'
]

export const LCM2DetailColumns2 = TableCustomCell(LCM2Label2, LCM2Prop2);
/* const LCM2sizeLabel  = ["Operation","Q0","V5","W0","1A","4A","6A","AA","BA","IG","sum" ];
const LCM2sizeProp  = [];
const SET1sizeLabel  = ["Operation","Q0","V5","W0","1A","4A","6A","AA","BA","IG","sum" ];
const SET1sizeProp  = ["WORK_CTR","SUM(QTY11_V5)","SUM(QTY_ALL)"]; */
/* export const LCM2DetailColumns2 = TableCustomCell(LCM2Label2, LCM2Prop2)

export const LCM2indexPanelOperWipLabel = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '5~7 d','7~15 d', '15~30 d','大於30d', 'sum'];
export const LCM2indexPanelOperWipProp = ['Operation', '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d', '3~5 d', '5~7 d','7~15 d', '15~30 d','大於30d', 'sum'];
export const LCM2indexcolumns = TableCustomCell(LCM2indexPanelOperWipLabel, LCM2indexPanelOperWipProp) */