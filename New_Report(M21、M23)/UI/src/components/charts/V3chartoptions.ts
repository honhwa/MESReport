interface Ibaroption{
 name:string,
 data:any[],
 stack:string
}
export function baroption(baroption:Ibaroption) {
    return {
        name: baroption.name,
        type: 'bar',
        barWidth: '15%',
        data: baroption.data,
        stack: baroption.stack,
        emphasis: {
            //折线图的高亮状态。
            focus: 'series' //聚焦当前高亮的数据所在的系列的所有图形。
        },
        //背景色
        // showBackground: true,
        backgroundStyle: {
            color: 'rgba(180, 180, 180, 0.2)'
        }
    }
}