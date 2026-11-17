import {TableCustomCell} from '@/pages/ReportPortal/utils/NGBcommon'


let WipPanSumLabel = [
    "Product",
    "Ver",
    "Panel ID",
    "Owner",
    "Oper",
    "Operation Desc",
    "Cassette ID",
    "Lcd Product",
    "Exp/RMA/Resorting No",
    "Status",
    "Hold",
    "Rework",
    "Stay Hrs",
    "date",
    "Hold Category",
    "Hold COMMENT1",
    "Priority",  
    ]
let WipPanSumProp = [
    "PROD_NBR",
"VER",
"PNL_ID",
"LCM_OWNER",
"WORK_CTR",
"WORK_NAME",
"CST_ID",
"PANEL_PN",
"WO_NBR",
"STATUS",
"HOLD_FLAG",
"STIMES_TTL",
"STAY_HRS",
"TRANS_DATE",
"HOLD_CODE",
"PRIORITY",
"COMMENT1",
]

let WipPanSumWidth=[80,25,100,35,35,50,35,35,65,35,35,35,35,70,50,50,50]

export let WipPanSumDetailCol = TableCustomCell(WipPanSumLabel,WipPanSumProp,[],WipPanSumWidth)