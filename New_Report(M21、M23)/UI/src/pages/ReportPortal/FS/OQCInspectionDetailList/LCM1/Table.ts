// import router from '@/router';
import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
import router from '@/router';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';



let OQCInspectionDetailListLabel = [
     'work_ctr',
    'Panel Id', 'serial_no',
    'product_id ', 'judgement',
    'OQC_grade', 'defect_code',
    'Defcode desc.', '異常單號',
    '現象位置', '現象說明',
    '檢出畫面', 'Process',
    'c-grade', 'c-defect',
    'c-operator', 'operator',
    'level_judgement'
]
let OQCInspectionDetailListProps = [
     'WORK_CTR',
    'SECTION_NO', 'SERIAL_NO',
    'PRODUCT_ID', 'JUDGEMENT',
    'OQC_GRADE', 'DEFECT_CODE',
    'OQCCMT', 'OQCCMT1',
    'OQCCMT2', 'OQCCMT3',
    'OQCCMT4', 'OQC_PROCESS',
    'CGRADE', 'CDEFECT',
    'C_OPERATOR', 'OPERATOR',
    'LEVEL_JUDGEMENT'
]

export const OQCInspectionDetailListColumns = TableCustomCell(OQCInspectionDetailListLabel,OQCInspectionDetailListProps,[])