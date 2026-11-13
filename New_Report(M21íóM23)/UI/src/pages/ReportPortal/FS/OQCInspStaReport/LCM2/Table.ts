// import router from '@/router';
import { TableCustomCell } from '@/pages/ReportPortal/utils/common';
import router from '@/router';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';



// export let OQCInspectionLabel = ['Operation', '線別', 'Product', '抽驗頻率', '是否撕膜檢', 'MFG產出數', 'OQC應抽檢數', 'OQC 抽檢數', '差異數量', '判退數量', '不良率(DPPM)']

// export let OQCInspectionProps = [
//     "WORK_CTR",
//     "LINE_NO",
//     "PROD_NBR",
//     "INSP_FREQUENCY",
//     "PROD_FLAG",
//     "PASS_QTY",
//     "CHECK_CNT",
//     "INSPECT_QTY",
//     "DIFF_CNT",
//     "NG_QTY"
// ]



export const OQCInspectionColumns: any[] = [
    {
        title: 'Operation',
        align: 'center',
        dataIndex: 'WORK_CTR',
        customCell: (record: Object) => {
            return {
                rowSpan: record?.['WORK_CTR-span']?.['rowspan'] || 0,
                colSpan: record?.['WORK_CTR-span']?.['colspan'] || 0,
            }
        }
    }, {
        title: 'Floor',
        align: 'center',
        dataIndex: 'FLOOR',
        customCell: (record: Object, rowindex: number, column: Object) => {
            return {
              rowSpan: record['FLOOR-span']['rowspan'],
              colSpan: record['FLOOR-span']['colspan']
            }
          }
    }, {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
    }, {
        title: '抽驗頻率',
        align: 'center',
        dataIndex: 'INSP_FREQUENCY',
    },{
        title: 'MFG產出數',
        align: 'center',
        dataIndex: 'PASS_QTY',
    }, {
        title: 'OQC應抽檢數',
        align: 'center',
        dataIndex: 'CHECK_CNT',
    }, {
        title: 'OQC抽檢數',
        align: 'center',
        dataIndex: 'INSPECT_QTY',
        //@ts-ignore
        customCell: (record: Object, rowindex: number, column: Object) => {
            let Obj = {};
            Obj['style'] = {
                cursor: 'pointer',
            }
            Obj['onDblclick'] = () => {
                const UseParamsAdd = AddCommonPageParamsWithOut()
                UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                router.push({ name: 'OQCInspectionLCM1Detail' })
            }
            return Obj
        }
    }, {
        title: '差異數量',
        align: 'center',
        dataIndex: 'DIFF_CNT'
    }, {
        title: '判退數量',
        align: 'center',
        dataIndex: 'NG_QTY'
    }, {
        title: '不良率(DPPM)',
        align: 'center',
        dataIndex: 'DPPM'
    }
]

let OQCInspectionDetailLabel = ['廠別', 'PNL_ID', 'PROD_NBR', 'WORK_CTR', 'C檢Grade', 'OQC Grade', 'C檢OPERATOR',
    'OQC OPERATOR', 'EQUIP_NBR', 'ERR_CODE', 'RANK1', 'LCM_OWNER', 'TRANS_DATE', 'OQC_INSPECT', 'OQCCMT1'
    , 'OQCCMT2', 'OQCCMT3', 'OQCCMT4']

let OQCInspectionDetailProps = [
    "FAC_ID",
    "PNL_ID",
    "PROD_NBR",
    "WORK_CTR",
    "C_GRADE",
    "GRADE_NEW",
    "C_OPERATOR",
    "OPERATOR",
    "EQUIP_NBR",
    "ERR_CODE",
    "RANK1",
    "LCM_OWNER",
    "TRANS_DATE",
    "OQC_INSPECT",
    "OQCCMT1",
    "OQCCMT2",
    "OQCCMT3",
    "OQCCMT4"
]

export const OQCInspectionDetailColumns = TableCustomCell(OQCInspectionDetailLabel, OQCInspectionDetailProps, [])