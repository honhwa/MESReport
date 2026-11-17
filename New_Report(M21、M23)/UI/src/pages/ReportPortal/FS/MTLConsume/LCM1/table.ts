export let baseColumns: any[] = [
    {
        title: '日期',
        align: 'center',
        dataIndex: "ACCT_DATE",
        key: "ACCT_DATE",
        // fixed: 'left',
        className:'v-center',
    },
    {
        title: '工單',
        align: 'center',
        dataIndex: "WO_NBR",
        key: "WO_NBR",
    },
    {
        title: '材料類別',
        align: 'center',
        dataIndex: "MAT_TYPE",
        key: "MAT_TYPE",
    },
    {
        title: '料號',
        align: 'center',
        dataIndex: "MAT_ID",
        key: "MAT_ID",
    },
    {
        title: '材料數量',
        align: 'center',
        dataIndex: "QTY",
        key: "QTY",
    },
    {
        title: 'PNL數量',
        align: 'center',
        dataIndex: "QTY0",
        key: "QTY0",
    },
    {
        title: '機種',
        align: 'center',
        dataIndex: "PROD_NBR",
        key: "PROD_NBR",
    },

    {
        title: '機台',
        align: 'center',
        dataIndex: "EQUIP_NBR",
        key: "EQUIP_NBR",
    },

]
export let detailColumsn: any[] = [
    {
        title: 'DateTime',
        align: 'center',
        dataIndex: "TRANS_DATE",
        key: "TRANS_DATE",
    },
    {
        title: '工單',
        align: 'center',
        dataIndex: "WO_NBR",
        key: "WO_NBR",
    },
    {
        title: '材料類別',
        align: 'center',
        dataIndex: "MAT_TYPE",
        key: "MAT_TYPE",
    },

    {
        title: '料號',
        align: 'center',
        dataIndex: "MAT_ID",
        key: "MAT_ID",
    },
    {
        title: 'PANEL ID',
        align: 'center',
        dataIndex: "PNL_ID",
        key: "PNL_ID",
    },
    {
        title: "LOT NUMBER",
        align: 'center',
        dataIndex: "LOT_NUMBER",
        key: "LOT_NUMBER",
    },
    {
        title: "Material Qty",
        align: 'center',
        dataIndex: "MAT_QTY_OK",
        key: "LOT_NUMBER",
    },
    {
        title: "Product",
        align: 'center',
        dataIndex: "PROD_NBR",
        key: "PROD_NBR",
    },
    {
        title: "Equipment",
        align: 'center',
        dataIndex: "EQUIP_NBR",
        key: "EQUIP_NBR",
    },

]