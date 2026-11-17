import {TableCustomCell} from '@/pages/ReportPortal/utils/SHcommon'

let OnlineSpcLogLabel = [
    'CHARTID', 'USER_ID', 'BOARDID', 'DELFLG', 'ROOT_CAUSE', 'ISSUE_TYPE', 
    'CORRECTIVE_ACTION', 'CUT_IN_DAY', 'OWNER', 'OOC/OOS/Others', 'GROUPID', 'MSRTYPE', 
    'VALUE(%)', 'UL(%)', 'LL(%)', 'CHKTYPE', 'OOC', 'OOC3', 'UNITID', 'PUNITID', 'CRR_ID', 
    'PROC_ID', 'PRE_PROC_ID', 'CLDATE', 'CLTIME', 'PCLDATE', 'PCLTIME', 'HOLDLOT', 
    'HOLDEQP', 'HOLDLOT3', 'HOLDEQP3', 'ONCHTYPE', 'T_STAMP', 'SEQ'
]

let OnlineSpcLogProps = [
    'CHARTID', 'USER_ID', 'BOARDID', 'DELFLG', 'ROOT_CAUSE', 'ISSUE_TYPE', 
    'CORRECTIVE_ACTION', 'CUT_IN_DAY', 'OWNER', 'OOCS', 'GROUPID', 'MSRTYPE', 
    'VALUE', 'UCL', 'LCL', 'CHKTYPE', 'OOC', 'OOC3', 'UNITID', 'PUNITID', 'CRR_ID', 
    'PROC_ID', 'PRE_PROC_ID', 'CLDATE', 'CLTIME', 'PCLDATE', 'PCLTIME', 'HOLDLOT', 
    'HOLDEQP', 'HOLDLOT3', 'HOLDEQP3', 'ONCHTYPE', 'T_STAMP', 'SEQ'
    
]
let OnlineSpcLogWidth= [250,150,150,160,80,150,200,100,100,80,80,150,100,100,100,100,100,110,80,100,80,100,100,80,80,150,100,100,100,100,100,110,80,100,80]

export let OnlineSpcLogColumns = TableCustomCell(OnlineSpcLogLabel,OnlineSpcLogProps,[],OnlineSpcLogWidth)

let OnlineSpcHisLogLabel = [
    'USER_ID', 'CA_ID', 'OWNER', 'BOARDID', 'LOTID', 'ROOT_CAUSE', 'ISSUE_TYPE', 
    'CORRECTIVE_ACTION', 'DUE_DAY', 'OOCS', 'GROUPID', 'MSRTYPE', 'VALUE', 'UL', 'LL', 
    'CHKTYPE', 'OOC', 'OOC3', 'UNITID', 'PUNITID', 'CRR_ID', 'PROC_ID', 'PRE_PROC_ID',
    'CLDATE', 'CLTIME', 'PCLDATE', 'PCLTIME', 'HOLDLOT', 'HOLDEQP', 'HOLDLOT3', 'HOLDEQP3',
    'CHARTID', 'ONCHTYPE', 'T_STAMP', 'SEQ', 'STATUS', 'OOS_FLAG'
]

let OnlineSpcHisLogProps = [
    'USER_ID', 'CA_ID', 'OWNER', 'BOARDID', 'LOTID', 'ROOT_CAUSE', 'ISSUE_TYPE', 
    'CORRECTIVE_ACTION', 'DUE_DAY', 'ooc/oos/others', 'GROUPID', 'MSRTYPE', 'VALUE', 'UL', 'LL', 
    'CHKTYPE', 'OOC', 'OOC3', 'UNITID', 'PUNITID', 'CRR_ID', 'PROC_ID', 'PRE_PROC_ID',
    'CLDATE', 'CLTIME', 'PCLDATE', 'PCLTIME', 'HOLDLOT', 'HOLDEQP', 'HOLDLOT3', 'HOLDEQP3',
    'CHARTID', 'ONCHTYPE', 'T_STAMP', 'SEQ', 'STATUS', 'OOS_FLAG'
]
let OnlineSpcHisLogWidth= [100,130,200,160,80,200,80,300,100,80,120,50,80,80,80,50,50,50,80,100,80,100,100,80,80,150,100,50,50,50,50,300,50,150,150,50,50]

export let OnlineSpcHisLogColumns = TableCustomCell(OnlineSpcHisLogLabel,OnlineSpcHisLogProps,[],OnlineSpcHisLogWidth)
