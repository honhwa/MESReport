let aheadTableProps = ['PROD_NBR', 'PRJ_NAME', 'GRADE'];
let aheadTableLabels = ['產品', 'Project name', '可出貨\r\nGRADE'];
let rearTableProps = ['STOCK_QTY', 'WIP_ASSY', 'WIP_ACD', 'WIP_PKQC', 'WIP_1910', 'DIFF_QTY', 'OUTPUT_COMM'];
let rearTableLabels = ['庫存', 'Assy/Demura', 'Aging/CD Ken', 'Packing/OQC', '1910', '週差異', '出貨重點說明'];
let aheadTableWidth: number[] = [120, 120, 60];
let rearTableWidth: number[] = [50, 100, 100, 100, 50, 50, 80];
let thYellow = ['產品', '可出貨\r\nGRADE', '庫存'];
let thYellowGreen = ['Project name', 'Assy/Demura', 'Aging/CD Ken', 'Packing/OQC', '1910', '週差異', '出貨重點說明'];
let thRedColor = ['產品', 'Project name', '出貨重點說明'];

export const aheadColumns = [
    ...aheadTableLabels.map((item, index) => {
        return {
            title: item,
            align: 'center',
            dataIndex: aheadTableProps[index],
            className: 
                'v-center ' + 
                (thYellow.includes(item)? 'thYellow ': '') +
                (thYellowGreen.includes(item)? 'thYellowGreen ': '')+
                (thRedColor.includes(item)? 'thRedColor': ''),
            width: aheadTableWidth[index] || undefined
        };
    })
];

export const rearColumns = [
    ...rearTableLabels.map((item, index) => {
        return {
            title: item,
            align: 'center',
            dataIndex: rearTableProps[index],
            className:
                'v-center ' + 
                (thYellow.includes(item)? 'thYellow ': '') +
                (thYellowGreen.includes(item)? 'thYellowGreen ': '')+
                (thRedColor.includes(item)? 'thRedColor': ''),
            width: rearTableWidth[index] || undefined,
            customCell: (record) => {
                let rs: any = {};
                if (item == '週差異') {
                    rs.style = {
                        color: '#00f',
                        'background-color': '#cdf'
                    };
                    if (record[rearTableProps[index]] < 0) {
                        rs.style.color = '#f00';
                    }
                }
                return rs;
            }
        };
    })
];
