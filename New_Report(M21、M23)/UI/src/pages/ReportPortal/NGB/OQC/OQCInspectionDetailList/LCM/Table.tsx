import {TableCustomCell} from '@/pages/ReportPortal/utils/NGBcommon'

let OQCInspectionDetailListLabel = [
     '日期',
    '機種名', 'Panel Id',
    'serial_no ', '工單類型',
    '不良現象', '不良描述',
    '檢出畫面', '區域',
    'C檢等級', 'OQC等級',
    'OQC-defect code', 'c-defect',
    'MFG 1600', 'MFG 1630',
    'MFG 1700', 'C_EQUIPNBR',
    'OQC 檢驗人員', '最終判定',
    '判定結果', '檢出站點',
    '數量', '其它檢驗',
    '工單號','批退'
]

let OQCInspectionDetailListProps = [
    'TRANS_DATE',
    'PRODUCT_ID', 'SECTION_NO',
    'SERIAL_NO', 'LCM_OWNER',
    'ERRC_DESCR', 'PCOMMENT',
    'OUTCP', 'PPOSITION',
    'CGRADE', 'OQC_GRADE',
    'DEFECT_CODE', 'CDEFECT',
    'C_OPERATOR', 'C2_OPERATOR',
    'D_OPERATOR', 'C_EQUIP_NBR',
    'OPERATOR', 'LEVEL_JUDGEMENT',
    'CODE_2', 'WORK_CTR',
    'QTY', 'COMMENT1',
    'WO_NBR','S0001'
]
let OQCInspectionDetailListWidth= [250,150,150,160,80,150,200,100,100,80,80,150,100,100,100,100,100,110,80,100,80,80,150,150,80]

export let OQCInspectionDetailListColumns = TableCustomCell(OQCInspectionDetailListLabel,OQCInspectionDetailListProps,[],OQCInspectionDetailListWidth)
