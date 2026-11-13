let tableLabels = [
    //'FAC_ID',
    'VERSION',
    'ACCT_DATE',
    'OPER',
    'PLAN_TYPE',
    'PROJECT',
    'CUSTOMER',
    'PROD_MODULE',
    'PROD_NBR',
    'BL_TYPE',
    'WO_TYPE',
    'MPC_COMMENT',
    'OWNER',
    'DAILY_PLAN'
];
let tableProps = [
    //'FAC_ID',
    'VERSION',
    'ACCT_DATE',
    'OPER',
    'PLAN_TYPE',
    'PROJECT',
    'CUSTOMER',
    'PROD_MODULE',
    'PROD_NBR',
    'BL_TYPE',
    'WO_TYPE',
    'MPC_COMMENT',
    'OWNER',
    'DAILY_PLAN'
];

export const baseColumns = tableLabels.map((item, index) => {
    return {
        title: item,
        align: 'center',
        dataIndex: tableProps[index],
        className: 'pro_auto-center'
    };
});
