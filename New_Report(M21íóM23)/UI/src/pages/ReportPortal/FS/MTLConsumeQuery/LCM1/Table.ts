import { TableCustomCell } from '@/pages/ReportPortal/utils/common';



export let MoveOutTimeLabel = ['Group', 'Date', 'Equipment', 'Product', 'Tact Time']
export let MoveOutTimeProps = ['Group', 'Date', 'Equipment', 'Product', 'Tact Time']
export let MoveOutTimeDetail = [
    "PROD_NBR",
    "LCD_PROD_NBR",
    "ENTITY_NO",
    "TRANS_DATE",
    "LCD_ABN_CF",
    "LCD_ABN_LCD",
    "LCD_ABN_TFT",
    "TRANS_TYPE"
]


export const MoveOutTimeColumns = TableCustomCell(MoveOutTimeLabel, MoveOutTimeProps, [],false, 'MoveOutTimeDetail')
export const MoveOutTimeDetailColumns = TableCustomCell(MoveOutTimeDetail, MoveOutTimeDetail, [],false, '')