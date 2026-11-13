let topTableLabels = ['Plan', 'Act', 'Diff'];
let topTableProps_MTD = ['MTD_PLAN_QTY', 'MTD_QTY', 'MTD_DIFF'];
let topTableProps_D = ['D_PLAN_QTY', 'D_QTY', 'D_DIFF'];
// 首頁上面可點擊列

let bottomTableLabels = ['Plan', 'Act','Diff','Comment','Recover Plan'];
let bottomTableProps_IN = ['IN_QTY','IN_PLAN','IN_DIFF','IN_MPC_COMMENT','IN_REC_PLAN'];
let bottomTableProps_OU = ['OU_QTY','OU_PLAN','OU_DIFF','OU_MPC_COMMENT','OU_REC_PLAN'];


export const topBaseColumns = [
    {
        title: '',
        align: 'center',
        dataIndex: '',
        children: [
            {
                title: 'max(version)',
                align: 'center',
                dataIndex: 'OPER',
                className: 'pro_auto-center'
            }
        ],
        className: 'pro_auto-center'
    },
    {
        title: 'MTD',
        align: 'center',
        dataIndex: 'MTD',
        children: [
            ...topTableLabels.map((item, index) => {
                return {
                    title: item,
                    align: 'center',
                    dataIndex: topTableProps_MTD[index],
                    className: 'pro_auto-center'
                };
            })
        ],
        className: 'pro_auto-center'
    },
    {
        title: 'D',
        align: 'center',
        dataIndex: 'D',
        children: [
            ...topTableLabels.map((item, index) => {
                return {
                    title: item,
                    align: 'center',
                    dataIndex: topTableProps_D[index],
                    className: 'pro_auto-center'
                };
            })
        ],
        className: 'pro_auto-center'
    }
];

export const bottomBaseColumns = [
    {
        title: '機種',
        align: 'center',
        dataIndex: 'PROD_NBR',
        className: 'pro_auto-center',
        customCell: (record) => {
          let text = record['PROD_NBR']
          let obj = {
            text,
            style: {}
          }
          obj['rowSpan'] = record?.['PROD_NBR-span']?.['rowspan'] || 0
          return obj
        }
    },
    {
        title: '客戶',
        align: 'center',
        dataIndex: 'CUSTOMER',
        className: 'pro_auto-center',
        customCell: (record) => {
          let text = record['CUSTOMER']
          let obj = {
            text,
            style: {}
          }
          obj['rowSpan'] = text?record?.['CUSTOMER-span']?.['rowspan'] || 0:""
          return obj
        }
    },
    {
        title: 'D',
        align: 'center',
        dataIndex: 'D',
        children: [
            ...bottomTableLabels.map((item, index) => {
                return {
                    title: item,
                    align: 'center',
                    dataIndex: bottomTableProps_IN[index],
                    className: 'pro_auto-center'
                };
            })
        ],
        className: 'pro_auto-center'
    },
    {
        title: 'D',
        align: 'center',
        dataIndex: 'D',
        children: [
            ...bottomTableLabels.map((item, index) => {
                return {
                    title: item,
                    align: 'center',
                    dataIndex: bottomTableProps_OU[index],
                    className: 'pro_auto-center'
                };
            })
        ],
        className: 'pro_auto-center'
    }
];