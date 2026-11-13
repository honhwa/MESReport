export const baseColumns: any[] = [

    {
        title: 'Product',
        align: 'center',
        dataIndex: 'PROD_NBR',
        // className: "v-center",
    },
    {
        title: 'S.Bank',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "G Grade",
                dataIndex: 'SBANK_G',
                key: "SBANK_G",
            },
            {
                title: "D Grade",
                dataIndex: 'SBANK_D',
                key: "SBANK_D",
            }
        ]
    },
    {
        title: 'Start',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Start",
                dataIndex: 'START_QTY',
                key: "START_QTY",
            },
        ]
    },
    {
        title: 'Move',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Move",
                dataIndex: 'MOVE_QTY',
                key: "MOVE_QTY",
            },
        ]
    },
    {
        title: 'Return',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Return",
                dataIndex: 'RETURN_QTY',
                key: "RETURN_QTY",
            },
        ]
    },
    {
        title: 'Wip',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "All Wip",
                dataIndex: 'ALL_WIP',
                key: "ALL_WIP",
            },
            {
                title: "All-Hold",
                dataIndex: 'ALL_HOLD_WIP',
                key: "ALL_HOLD_WIP",
            },
            {
                title: "Rework",
                dataIndex: 'RWK_WIP',
                key: "RWK_WIP",
            },
        ]
    },
    {
        title: 'Rework',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "To Rwk",
                dataIndex: 'TORWK_QTY',
                key: "TORWK_QTY",
            },
            {
                title: "Rwk R",
                dataIndex: 'TORRWK_QTY',
                key: "TORRWK_QTY",
            },

        ]
    },
    {
        title: 'To be scarp',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "In scarp",
                dataIndex: 'TOBESCRAP_QTY',
                key: "TOBESCRAP_QTY",
            },
            {
                title: "Wip",
                dataIndex: 'SCRAP_WIP',
                key: "SCRAP_WIP",
            },
            {
                title: "Scarp",
                dataIndex: 'SCRAP_QTY',
                key: "SCRAP_QTY",
            },

        ]
    },
    {
        title: 'Ship',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Total",
                dataIndex: 'SHIP_QTY',
                key: "SHIP_QTY",
            },
            {
                title: "G Grade",
                dataIndex: 'SHIP_G_QTY',
                key: "SHIP_G_QTY",
            },
            {
                title: "D Grade",
                dataIndex: 'SHIP_D_QTY',
                key: "SHIP_D_QTY",
            },

        ]
    },
    {
        title: 'Yield',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Yield",
                dataIndex: 'pp',
                key: "pp",
            },


        ]
    },
    {
        title: 'Month to Day',
        align: 'center',
        // className: "v-center",
        children: [
            {
                title: "Start",
                dataIndex: 'START_TOTALQTY',
                key: "START_TOTALQTY",
            },
            {
                title: "To be scrap",
                dataIndex: 'TOBESCRAP_TOTALQTY',
                key: "TOBESCRAP_TOTALQTY",
            },
            {
                title: "Shipping",
                dataIndex: 'SHIP_TOTALQTY',
                key: "SHIP_TOTALQTY",
            },
            {
                title: "Yield",
                dataIndex: 'pp1',
                key: "pp1",
            },
            {
                title: "G Grade",
                dataIndex: 'SHIP_G_TOTALQTY',
                key: "SHIP_G_TOTALQTY",
            },
            {
                title: "D Grade",
                dataIndex: 'SHIP_D_TOTALQTY',
                key: "SHIP_D_TOTALQTY",
            },
            // G RAte
            {
                title: "G Rade",
                dataIndex: 'pp2',
                key: "pp2",
            },


        ]
    },

];
export const WipDistDetail3Columns:any[] = [
    {
        title: 'Equipment',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
    },
    {
        title: 'Operation',
        align: 'center',
        dataIndex: 'MAIN_WC',
    },
    {
        title: 'Qty',
        align: 'center',
        dataIndex: 'QTY',
    },
]
export const WipDistDetail2Columns:any[] = [
    {
        title: 'Equipment',
        align: 'center',
        dataIndex: 'EQUIP_NBR',
    },
    {
        title: 'Cassette ID',
        align: 'center',
        dataIndex: 'CST_ID',
    },
    {
        title: 'Panel ID',
        align: 'center',
        dataIndex: 'ENTITY_NO',
    },
]

export const WipDistDetail1Columns:any[] = [
    {
        title: 'Operation',
        align: 'center',
        dataIndex: 'WORK_CTR',
    },
    {
        title: 'Cassette Id',
        align: 'center',
        dataIndex: 'CST_ID',
    },
    {
        title: 'Panel Id',
        align: 'center',
        dataIndex: 'PNL_ID',
    },
    {
        title: 'Over 15',
        align: 'center',
        dataIndex: 'OVER15',
    },
    {
        title: 'Grade',
        align: 'center',
        dataIndex: 'LCM_GRADE',
    },
    {
        title: 'Version',
        align: 'center',
        dataIndex: 'VER',
    },
    {
        title: 'Hold Mark',
        align: 'center',
        dataIndex: 'HOLD_MARK',
    },
]