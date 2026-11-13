import router from '@/router';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommon } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParamsWithOut();
let useReportCommon = ReportCommon();

let OnlineSpcChartLabel = [
    'ONCHID', 'ONCHTYPE', 'COWNER', 'CDESCRIPTION', 'CTOOLID', 'MES_ID', 'DATA_GROUP', 'REP_UNIT', 'DATA_PAT', 'ACTIVE', 'OFLNSPC', 'CLSEQ', 
    'SPECSEQ', 'CLVER', 'UCL1', 'LCL1', 'CL1', 'UCL2', 'LCL2', 'CL2', 'UCL3', 'LCL3', 'CL3', 'SPECVER', 'USPEC', 'LSPEC', 'TARGET', 'PPROC_ID', 
    'PEQPT_ID', 'PSUBENTITY', 'PRECIPE', 'PROC_ID', 'EQPT_ID', 'SUBENTITY', 'RECIPE', 'P2PTOOL', 'SHOLDLOT', 'SHOLDEQP', 'SALARM', 'HOLDLOT', 
    'HOLDEQP', 'ALARM', 'HOLDLOT3', 'HOLDEQP3', 'ALARM3', 'TARTOOL', 'WER01', 'WER02', 'WER03', 'WER04', 'WER05', 'WER06', 'WER07', 'WER08', 'WER09',
    'WER10', 'WER11', 'WER12', 'WER13', 'WER14', 'WER15', 'CHARTGRADE', 'AVGTYPE', 'SIGMATYPE', 'SAMPLECNT', 'PROD', 'CREATE_USER', 'CREATE_TIME', 'EDIT_USER', 
    'EDIT_TIME', 'ENVSTAT', 'EC_CODE', 'SPEC_ACT', 'CL_ACT',' RUN_MODE', 'WER16', 'MHOLDLOT', 'MHOLDEQP', 'MALARM', 'UML2', 'LML2', 'ML2', 'T_STAMP', 'OOR3', 'OOR4'
]

let OnlineSpcChartProps = [
    'ONCHID', 'ONCHTYPE', 'COWNER', 'CDESCRIPTION', 'CTOOLID', 'MES_ID', 'DATA_GROUP', 'REP_UNIT', 'DATA_PAT', 'ACTIVE', 'OFLNSPC', 'CLSEQ', 
    'SPECSEQ', 'CLVER', 'UCL1', 'LCL1', 'CL1', 'UCL2', 'LCL2', 'CL2', 'UCL3', 'LCL3', 'CL3', 'SPECVER', 'USPEC', 'LSPEC', 'TARGET', 'PPROC_ID', 
    'PEQPT_ID', 'PSUBENTITY', 'PRECIPE', 'PROC_ID', 'EQPT_ID', 'SUBENTITY', 'RECIPE', 'P2PTOOL', 'SHOLDLOT', 'SHOLDEQP', 'SALARM', 'HOLDLOT', 
    'HOLDEQP', 'ALARM', 'HOLDLOT3', 'HOLDEQP3', 'ALARM3', 'TARTOOL', 'WER01', 'WER02', 'WER03', 'WER04', 'WER05', 'WER06', 'WER07', 'WER08', 'WER09',
    'WER10', 'WER11', 'WER12', 'WER13', 'WER14', 'WER15', 'CHARTGRADE', 'AVGTYPE', 'SIGMATYPE', 'SAMPLECNT', 'PROD', 'CREATE_USER', 'CREATE_TIME', 'EDIT_USER', 
    'EDIT_TIME', 'ENVSTAT', 'EC_CODE', 'SPEC_ACT', 'CL_ACT',' RUN_MODE', 'WER16', 'MHOLDLOT', 'MHOLDEQP', 'MALARM', 'UML2', 'LML2', 'ML2', 'T_STAMP', 'OOR3', 'OOR4'
    ]
let OnlineSpcChartClick =[
    'ONCHID', 'ONCHTYPE', 'COWNER', 'CDESCRIPTION', 'CTOOLID', 'MES_ID', 'DATA_GROUP', 'REP_UNIT', 'DATA_PAT', 'ACTIVE', 'OFLNSPC', 'CLSEQ', 
    'SPECSEQ', 'CLVER', 'UCL1', 'LCL1', 'CL1', 'UCL2', 'LCL2', 'CL2', 'UCL3', 'LCL3', 'CL3', 'SPECVER', 'USPEC', 'LSPEC', 'TARGET', 'PPROC_ID', 
    'PEQPT_ID', 'PSUBENTITY', 'PRECIPE', 'PROC_ID', 'EQPT_ID', 'SUBENTITY', 'RECIPE', 'P2PTOOL', 'SHOLDLOT', 'SHOLDEQP', 'SALARM', 'HOLDLOT', 
    'HOLDEQP', 'ALARM', 'HOLDLOT3', 'HOLDEQP3', 'ALARM3', 'TARTOOL', 'WER01', 'WER02', 'WER03', 'WER04', 'WER05', 'WER06', 'WER07', 'WER08', 'WER09',
    'WER10', 'WER11', 'WER12', 'WER13', 'WER14', 'WER15', 'CHARTGRADE', 'AVGTYPE', 'SIGMATYPE', 'SAMPLECNT', 'PROD', 'CREATE_USER', 'CREATE_TIME', 'EDIT_USER', 
    'EDIT_TIME', 'ENVSTAT', 'EC_CODE', 'SPEC_ACT', 'CL_ACT',' RUN_MODE', 'WER16', 'MHOLDLOT', 'MHOLDEQP', 'MALARM', 'UML2', 'LML2', 'ML2', 'T_STAMP', 'OOR3', 'OOR4'
    ]

let OnlineSpcChartWidth= [280,100,60,280,80,200,180,60,80,60,60,280,280,60,60,60,60,60,60,60,60,60,60,80,80,80,80,80,80,80,80,80,80,80,80,
    80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,80,
    80,80,80,80,80,80,80,80,80,80,80,80,200,80,200,80,80,80,80,80,80,80,80,80,80,80,80,200,80,80]

export let OnlineSpcChartColumns = TableCustomCell(OnlineSpcChartLabel,OnlineSpcChartProps,OnlineSpcChartClick,OnlineSpcChartWidth,false,'OnlineSpcChartDetail')

let OnlineSpcChartSecondLabel = [ 'USER_ID', 'CLDATE', 'CLTIME', 'LOTID', 'GLASSID', 'AVERAGE', 'RANGE', 'DTMAX', 
                                'DTMIN', 'USPEC', 'TARGET', 'LSPEC', 'UCL1', 'CL1', 'LCL1', 'UCL2', 'CL2', 'LCL2', 
                                'OOC1','OOC2', 'OC31', 'OC32', 'OOS', 'SEQ', 'MES_ID', 
                                'GROUPID', 'CHARTID', 'CHARTTYPE', 'T_STAMP', 'LD_ID']
let OnlineSpcChartSecondProps = [ 'USER_ID', 'CLDATE', 'CLTIME', 'LOTID', 'GLASSID', 'DTAVG', 'DTRNG', 'DTMAX', 
                                'DTMIN', 'USPEC', 'TARGET', 'LSPEC', 'UCL1', 'CL1', 'LCL1', 'UCL2', 'CL2', 'LCL2', 
                                'OOC1','OOC2', 'OC31', 'OC32', 'OOS', 'SEQ', 'MES_ID', 
                                'GROUPID', 'ONCHID', 'CHARTTYPE', 'T_STAMP', 'LD_ID']
let OnlineSpcChartSecondWidth= [80,80,80,160,100,80,80,80,80,80,80,50,80,80,80,50,50,50,80,80,80,80,80,200,280,130,280,80,200,80]

export let OnlineSpcChartSecondColumns = TableCustomCell(OnlineSpcChartSecondLabel,OnlineSpcChartSecondProps,[],OnlineSpcChartSecondWidth)


function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[],isMergeColumns: boolean = false, ToOtherpage?: string) {
    return Prop.map((item: string, index: number) => {
        console.log("22")
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            width:Width[index],
            customCell: (record:Object, rowIndex:number, column:Object) => {
                return {
                    onDblclick: () => {          
                        
                        //加參數
                        useReportCommon.AddFirstTableParams(`OnlineSpcChart-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'], rowindex:rowIndex})
                       
                        //加路由
                        router.push({name: ToOtherpage+`-${useCommonParams.Fab}` });

                    }, style: { cursor: 'pointer' }
                }
            }
        }
    })
}