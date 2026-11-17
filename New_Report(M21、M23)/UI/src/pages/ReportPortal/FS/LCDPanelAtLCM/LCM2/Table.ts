import { TableCustomCell } from '@/pages/ReportPortal/utils/common';



export let LCDLabel = ['Lcm_prod', 'Owner', 'Lcd_prod', "ERP_PROD_ID",'Lcd_grade', 'Lcd_owner']
export let LCDProps = ['LCM_PROD', 'LCM_OWNER', 'LCD_PROD', "ERP_PROD_ID",'LCD_GRADE', 'LCD_OWNER']
export let LCDDetail = [
    "PROD_NBR",
    "LCD_PROD_NBR",
    "ENTITY_NO",
    "TRANS_DATE",
    "LCD_ABN_CF",
    "LCD_ABN_LCD",
    "LCD_ABN_TFT",
    "TRANS_TYPE"
]


export const LcdPanelAtLcmColumns = TableCustomCell(LCDLabel, LCDProps, [],false, '')
export const LcdPanelAtLcmDetailColumns = TableCustomCell(LCDDetail, LCDDetail, [],false, '')