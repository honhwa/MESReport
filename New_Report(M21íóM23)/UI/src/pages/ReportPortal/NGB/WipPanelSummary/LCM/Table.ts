import {TableCustomCell} from '@/pages/ReportPortal/utils/NGBcommon'


let WipPanSumLabel = [
    "Product",
    "Ver",
    "Panel ID",
    "Owner",
    "Oper",
    "Cassette ID",
    "Lcd Product",
    "Exp/RMA/Resorting No",
    "Status",
    "Hold",
    "Rework",
    "Stay Hrs",
    "date",
    "Hold Category",
    "Hold Code",
    "Hold Comments",
    "Priority",
    "start over Hrs",
    "Serial NO",
    "Lcm Grade",
    "Mvin_flag",
    "Dfct code",
    "Modify Date"
    ]
let WipPanSumProp = ["PROD_NBR",
"VER",
"PNL_ID",
"LCM_OWNER",
"WORK_CTR",
"CST_ID",
"PANEL_PN",
"WO_NBR",
"STATUS",
"HOLD_FLAG",
"STIMES_TTL",
"STAY_HRS",
"TRANS_DATE",
"HOLD_FLAG",
"HOLD_CODE",
"HOLD_COMMENT",
"PRIORITY",
"START_HOURS",
"SERIAL_NO",
"LCM_GRADE",
"MVIN_FLAG",
"DFCT_CODE",
"MODIFY_DATE",
]

let WipPanSumWidth=[70,20,70,30,30,55,50,75,30,30,40,50,100,50,45,90,40,60,90,40,40,40,60]

export let WipPanSumDetailCol = TableCustomCell(WipPanSumLabel,WipPanSumProp,[],WipPanSumWidth)