export let baseColumns: any[] = [
    {
        title: 'Owner',
        align: 'center',
        dataIndex: 'LCM_OWNER',
        className: "v-center",
        fixed: 'left',
        width: 100,
    },
    {
        title: 'Product',
        align: 'center',
        className: "v-center",
        dataIndex: 'PROD_NBR',
        fixed: 'left',
        width: 100,
    },
    {
        title: 'Customer',
        align: 'center',
        className: "v-center",
        dataIndex: 'CUSTOMER',
        fixed: 'left',
        width: 100,
    },
];
export let allColumns = {
    title: 'all',
    align: 'center',
    children: [
        {
            title: '總計',
            align: 'center',
            children: [
                    {
                        title: `scrap小計`,
                        dataIndex: `scrap_sum`,
                        key: `scrap_sum`,
                        className: "Green",
                    }
            ]
        },
        {
            title: 'all小計',
            align: 'center',
            children: [
                    {
                        title: `all小計`,
                        dataIndex: `all_sum`,
                        key: `all_sum`,
                        className: "Yellow",

                    }
            ]
        },
        {
            title: '總計',
            align: 'center',
            children: [
                    {
                        title: `總計`,
                        dataIndex: `all_totalsum`,
                        key: `all_totalsum`,
                        className: "Yellow",

                    }
            ]
        }
    ]


}
export let holdandnoholdColumns = {
    title: 'all',
    align: 'center',
    children: [
        {
            title: '總計',
            align: 'center',
            children: [
                    {
                        title: `總計`,
                        dataIndex: `all_totalsum`,
                        key: `all_totalsum`,
                        className: "Yellow",

                    }
            ]
        }
    ]


}