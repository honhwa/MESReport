export let CKENDataSetColumn = {
    'Query': [{
        title: '點燈標準時間(s)',
        align: 'center',
        dataIndex: 'NORMAL_TIME',
    },{
        title: '小于標準點燈時間數量',
        dataIndex: 'L_QTY',
    },{
        title: '標準作業時間(s)',
        dataIndex: 'NORMAL_W_TIME',

    },{
        title: '小于標準作業時間數量',
        dataIndex: 'C_QTY',
    },
    ],
    'Query1': [
        {
            title: '機種',
            align: 'center',
            dataIndex: 'PROD_NBR',
            fixed: 'left',
            className: 'v-center',
        },{
            title: '點燈標準時間',
            dataIndex: 'NORMAL_TIME',
        },{
            title: '標準作業時間',
            dataIndex: 'NORMAL_W_TIME',
        },{
            title: '更新時間',
            dataIndex: 'UPDATE_TIME',
        },
    ]
}