import {TableCustomCell} from '@/pages/ReportPortal/utils/SHcommon'

let CpCpkReportLabel = [
    'Machine',
    'Macro ID', 'Including Chart',
    'USL', 'Target',
    'LSL', 'UCL',
    'CL', 'LCL',
    'Owner', 'N',
    '樣本組數', 'OOS Count',
    'OOS%', 'OOC Count',
    'OOC%', '違反WER Rule件數',
    'X-Bar', 'Sigma',
    'Ca', 'Cp',
    'Cpk', 'Update Date'
]

let CpCpkReportProps = [
    'MESSEQNO',
    'ONCHID', 'DEFAULTCHARTTYPE1',
    'USPEC', 'TARGET',
    'LSPEC', 'UCL1',
    'CL1', 'LCL1',
    'OWNER', 'N',
    'CNT_SAMPLE', 'CNT_OOS',
    'PCT_OOS', 'CNT_OOC',
    'PCT_OOC', 'CNT_RULES',
    'XBAR', 'SIGMA',
    'CA', 'CP',
    'CPK', 'CREATE_TIME'
]
let CpCpkReportWidth= [250,150,150,160,80,150,200,100,100,80,80,150,100,100,100,100,100,110,80,100,80,80,150]

export let CpCpkReportColumns = TableCustomCell(CpCpkReportLabel,CpCpkReportProps,[],CpCpkReportWidth)
