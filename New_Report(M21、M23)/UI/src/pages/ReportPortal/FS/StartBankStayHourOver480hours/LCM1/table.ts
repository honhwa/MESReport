 const baseColumnsYLabel = [
    'size',
    'oper',
    'panel_pn',
    'owner',
    'lcd_grade_grp',
    'cassette in date',
    'Cst_qty',
    'Cst_id',
    'stay hours',
    '申請單號',
    '申請人',
    'q_time',
    'Comment',
    'ABN Code'
];
 const baseColumnsYProps = [
    'SIZE1',
    'OPER',
    'PANEL_PN',
    'OWNER',
    'LCD_GRADE',
    'TRANS_DATE',
    'CST_QTY',
    'CST_ID',
    'STAY_HOUR',
    'EXP_NBR',
    'EXP_ENGINEER',
    'Q_TIME',
    'COMMENT1',
    'ABN_CODE'
];
export const baseColumnsYcolumns = baseColumnsYLabel.map((item, index) => {
    return {
        title: item, 
        align: 'center',
        dataIndex: baseColumnsYProps[index] 
    };
});
 const baseColumnsNLabel = [
    'size',
    'oper',
    'panel_pn',
    'owner',
    'lcd_grade_grp',
    'cassette in date',
    'Cst_qty',
    'Cst_id',
    'stay hours',
    '申請單號',
    '申請人',
    'q_time',
    'Comment'
];
 const baseColumnsNprops = [
    'SIZE1',
    'OPER',
    'PANEL_PN',
    'OWNER',
    'LCD_GRADE',
    'TRANS_DATE',
    'CST_QTY',
    'CST_ID',
    'STAY_HOUR',
    'EXP_NBR',
    'EXP_ENGINEER',
    'Q_TIME',
    'COMMENT1'
];
export const baseColumnsNcolumns = baseColumnsNLabel.map((item, index) => {
    return {
        title: item, 
        align: 'center',
        dataIndex: baseColumnsNprops[index] 
    };
});
