import router from '@/router';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

const UseParamsAdd = ReportCommonWithOut();
export let ZMFGOperatorHColumns = {
    'operators': [{
        title: 'Operators',
        align: 'center',
        dataIndex: 'QITEM',
        fixed: 'left',
        className: 'v-center',
    },
    ],
    'equip_nbr': [
        {
            title: 'Equipment',
            align: 'center',
            dataIndex: 'QITEM',
            fixed: 'left',
            className: 'v-center',
        },
    ]
}
export let ZMFGOperatorColumns = [
    {
        title: 'OQC检验数量',
        align: 'center',
        dataIndex: 'INSP_CNT',
        fixed: 'left',
    },
    {
        title: '判退数量',
        align: 'center',
        dataIndex: 'QPAN',
        fixed: 'left',
        customCell: (record: Object, rowIndex: number, column: Object) => {
            return {
                onDblclick: () => {
                    UseParamsAdd.AddFirstTableParams('ZMFGOperatorLCM1', { ...record, ...column['__originColumn__'] });
                    router.push({ name: 'ZMFGOperatorLCM1Detail' });
                    // router.push({ name: 'GradeWipDisDetail' });
                },
                style: {
                    cursor: 'pointer'
                }
            };
        }
    },
]

export const ZMFGOpDetailColumns = [
{
    title: 'PROD_NBR',
    dataIndex: 'PROD_NBR',
    fixed: 'left',
    className: 'v-center',
},
{
    title: 'PNL_ID',
    dataIndex: 'PNL_ID',
}, {
    title: 'ERR_CODE',
    dataIndex: 'ERR_CODE',
}, {
    title: 'TRANS_DATE',
    dataIndex: 'TRANS_DATE',
}, {
    title: 'OPERATOR',
    dataIndex: 'OPERATOR',
}, {
    title: 'OQCCMT2',
    dataIndex: 'OQCCMT2',
}, {
    title: 'OQCCMT3',
    dataIndex: 'OQCCMT3',
}, {
    title: 'OQCCMT4',
    dataIndex: 'OQCCMT4',
},
]

