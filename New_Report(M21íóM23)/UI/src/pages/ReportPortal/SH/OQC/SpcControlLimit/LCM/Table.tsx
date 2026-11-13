import {TableCustomCell} from '@/pages/ReportPortal/utils/SHcommon'

let ControlLimitLabel = [
    'Machine' , 'ONCHID' ,'Including Chart','USL','Target','LSL' , 'UCL', 'CL', 'LCL', 'UCL', 'CL','LCL' ,
    'cpk1','cpk2','cpk3','開始日期','截止日期','組數','sample_cnt1', 'sample_cnt2', 'sample_cnt3'
]

let ControlLimitProps = [
    'MESSEQNO', 'ONCHID', 'DEFAULTCHARTTYPE', 'USPEC', 'TARGET', 'LSPEC', 
    'UCL', 'CL', 'LCL', 'CALUCL', 'CALCL', 'CALLCL', 'CPK1', 'CPK2', 'CPK3', 
    'CREATEB_TIME', 'CREATE_TIME', 'CNT_SAMPLE', 'SAMPLE_CNT1', 'SAMPLE_CNT2', 'SAMPLE_CNT3'
    
]
let ControlLimitWidth= [250,150,150,160,80,150,200,100,100,80,80,150,100,100,100,100,100,110,80,100,80]

export let ControlLimitColumns = TableCustomCell(ControlLimitLabel,ControlLimitProps,[],ControlLimitWidth)
