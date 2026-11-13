let aheadTableLabels = ['Size', 'Product', 'PROJECT', 'CUSTOMER', 'BL_TYPE', 'MPC_COMMENT', 'Item'];
let aheadTableProps = ['SIZE1', 'PROD_NBR', 'PROJECT', 'CUSTOMER', 'BL_TYPE', 'MPC_COMMENT', 'ITEM'];
let rearTableLabels = ['總計'];
let rearTableProps = ['TOTAL'];
let spanColumns = ['Size', 'Product', 'MPC_COMMENT'];

export const aheadColumns = [
    ...aheadTableLabels.map((item, index) => {
        return {
            title: item,
            align: 'center',
            dataIndex: aheadTableProps[index],
            className: 'pro_auto-center',
            customCell: (record) => {
                if (spanColumns.includes(item)) {
                    let text = record[aheadTableProps[index]];
                    let obj = {
                        text,
                        style: {}
                    };
                    obj['rowSpan'] = text ? record?.[aheadTableProps[index] + '-span']?.['rowspan'] || 0 : '';
                    return obj;
                }
            }
        };
    })
];

export const rearColumns = [
    ...rearTableLabels.map((item, index) => {
        return {
            title: item,
            align: 'center',
            dataIndex: rearTableProps[index],
            className: 'pro_auto-center'
        };
    })
];
