let prodinfoColumn = ['PROD_NBR', 'CATE_CODE1', 'SEQ'];
let prodgradeColumn = ['PROD_NBR', 'CATE_CODE2'];
let dailytartColumn = ['PROD_NBR', 'CATE_CODE3', 'ACCT_DATE', 'DAILY_QTY'];

export const baseColumns = {
    PRODINFO: prodinfoColumn.map((item) => {
        return {
            title: item,
            align: 'center',
            dataIndex: item,
            className: 'v-center'
        };
    }),
    PRODGRADE: prodgradeColumn.map((item) => {
        return {
            title: item,
            align: 'center',
            dataIndex: item,
            className: 'v-center'
        };
    }),
    DAILYTART: dailytartColumn.map((item) => {
        return {
            title: item,
            align: 'center',
            dataIndex: item,
            className: 'v-center'
        };
    })
};
