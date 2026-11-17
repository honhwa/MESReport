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
    "Hold Comments",
    "Priority",
    "start over Hrs",
    "Serial NO",
    "Lcm Grade",
    "rma_nbr",

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
"HOLD_COMMENT",
"PRIORITY",
"START_HOURS",
"SERIAL_NO",
"LCM_GRADE",
"MVIN_FLAG",

]

let WipPanSumWidth=[80,25,75,40,35,55,50,75,35,30,45,50,85,50,90,40,60,90,50,50]

export let WipPanSumDetailCol = TableCustomCell(WipPanSumLabel,WipPanSumProp,[],WipPanSumWidth)