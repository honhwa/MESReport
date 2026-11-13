
import type { TableColumn } from '@/components/core/dynamic-table';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import router from '@/router';
import { cloneDeep } from "lodash-es"


/**
 * @param PageSetting 頁面所有配置
 */
export function showHidden(PageSetting) {
    PageSetting.showmenu = !PageSetting.showmenu;
    if (PageSetting.showmenu) PageSetting.TableSpan = 20;
    else PageSetting.TableSpan = 24;
}

/**
 * 
 * @param count 結束行
 * @param count1 開始行
 * @param data 原始的縂數據
 * @param calculation 需要加總的列名
 * @returns 獲取加總的數據
 */
export function getSumData(count: number, count1: number, data: any[], calculation: any[]) {
    let SubTotal = {};
    for (let item of calculation) {
        let sum = 0;
        for (let i = count1; i < count; i++) {
            let a = isNaN(data[i][item]) ? 0 : data[i][item];
            sum = sum + a;
        }
        SubTotal[item] = sum;
    }
    return SubTotal;
}

export function getRowClassName(record: Object) {
    if (record && Object.keys(record).length != 0) {
        for (const [key, value] of Object.entries(record)) {
            if (['小计', 'BANK_SUM', 'WIP_SUM','小計','合計'].includes(value)) {
                return 'Yellow';
            } else if (['總計', 'SUM', 'RWK', '总计', 'Sum', '合计'].includes(value)) {
                return 'Green';
            }
        }
    }
}

export function mergeTableRow(config: any) {//合并表格的方法
    let data = config['data'];
    const { mergeColNames, firstMerge } = config;
    if (!mergeColNames || mergeColNames.length === 0) {
        return data;
    }
    mergeColNames.forEach((item: string, mergeIndex: number) => {
        //遍历需要合并的列
        const mList = {};
        data = data.map((Source: any, index: number) => {
            const rowVal = Source[item]; //每行对应合并列的值
            if (mList[rowVal] && mList[rowVal].newIndex === index) {
                // mergeColNames.forEach((meta: string,metaIndex:number)=>{
                //     if(meta==item){没思路，改不了，等会改

                //     }
                // })
                if (item == firstMerge) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                }
                else if (mergeIndex == 1 && data[index][firstMerge] == data[index - 1][firstMerge]) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                }
                else if (
                    mergeIndex == 2 &&
                    data[index][firstMerge] == data[index - 1][firstMerge] &&
                    data[index][mergeColNames[mergeIndex - 1]] == data[index - 1][mergeColNames[mergeIndex - 1]]
                ) {
                    data[mList[rowVal]['index']][item + '-span'].rowspan++;
                    Source[item + '-span'] = {
                        rowspan: 0,
                        colspan: 0
                    };
                    mList[rowVal]['num']++;
                    mList[rowVal]['newIndex']++;
                } else {
                    mList[rowVal] = { num: 1, index: index, newIndex: index + 1 };
                    Source[item + '-span'] = {
                        rowspan: 1,
                        colspan: 1
                    };
                }
            } else {
                mList[rowVal] = { num: 1, index: index, newIndex: index + 1 };
                Source[item + '-span'] = {
                    rowspan: 1,
                    colspan: 1
                };
            }
            return Source;
        });
    });
    return data;
}

export function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[],isMergeColumns: boolean = false, ToOtherpage?: string) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            col:index,
            width: Width[index],
            // className: 'v-center',
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        onDblclick: () => {
                            const UseParamsAdd = AddCommonPageParamsWithOut()
                            // UseParamsAdd.AddSecondTableParams({ ...record, ...column })
                            UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] })
                            if (ToOtherpage) router.push({ name: ToOtherpage });
                        }, style: { cursor: 'pointer',width:40 }
                    }
                }
                else if (isMergeColumns) {
                    return {
                        rowSpan: record?.[`${item}-span`]?.['rowspan'] || 0,
                        colSpan:record?.[`${item}-span`]?.['colspan'] || 0,
                         style: { width:40 }
                    }
                }
                else {
                    return {
                         style: { width:40 },
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据')
                        }
                    }
                }

            }
        }
    })

}
/**
 * 
 * @param stime 起始日期
 * @param etime 终止日期
 * @returns 获取两个日期中间的日期
 */
export function getdiffdate(stime: string, etime: string) {
    //初始化日期列表，数组
    var diffdate = new Array();
    var i = 0;
    //开始日期小于等于结束日期,并循环
    while (stime <= etime) {
        diffdate[i] = stime;

        //获取开始日期时间戳
        var stime_ts = new Date(stime).getTime();
        // console.log('当前日期：' + stime + '当前时间戳：' + stime_ts);

        //增加一天时间戳后的日期
        var next_date = stime_ts + (24 * 60 * 60 * 1000);

        //拼接年月日，这里的月份会返回（0-11），所以要+1
        var next_dates_y = new Date(next_date).getFullYear() + '-';
        var next_dates_m = (new Date(next_date).getMonth() + 1 < 10) ? '0' + (new Date(next_date).getMonth() + 1) + '-' : (new Date(next_date).getMonth() + 1) + '-';
        var next_dates_d = (new Date(next_date).getDate() < 10) ? '0' + new Date(next_date).getDate() : new Date(next_date).getDate();

        stime = next_dates_y + next_dates_m + next_dates_d;

        //增加数组key
        i++;
    }
    return diffdate;
}
// 最后一行总计
export function getTotalData(data: any[], DateColumns: any[]) {
    //合計
    let obj = {};
    DateColumns.map((item: string) => {
        let sum = 0;
        data.map((n) => {
            sum += n[item];
        });
        obj[item] = sum;
    });
    return obj;
}
// 列总计
export function getAllSumData(row: any, DateColumns: any[]) {
    let sum = 0;
    DateColumns.map((item: any) => {
        row[item] = isNaN(row[item]) ? 0 : row[item];
        sum += row[item];
    });
    return sum;
}
// 表格共用方法
// 动态添加属性
// text:當前行的值     key：當前元素的列名    index:當前行的索引   dataSource:數據源
function mergeCells(text: string, key: string, index: number, dataSource: any[]) {
    // 上一行该列数据是否一样
    if (index !== 0 && text === dataSource[index - 1][key]) {
        return 0
    }
    let rowSpan = 1
    // 判断下一行是否相等
    for (let i = index + 1; i < dataSource.length; i++) {
        if (text !== dataSource[i][key]) {
            break
        }
        rowSpan++
    }
    return rowSpan
}
export function TableCustomCell0(Label: string[], Prop: string[], ClickColumn: string[] = [], Func: Function,mergeCellCol: any[] = [], dataSource: any[] = [], ColStyle: Object[] = []) {
    return Prop.map((item,index)=>{
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            // className: 'v-center',
            customCell: (record: Object, rowindex: number, column: Object) => {
                // 合并行功能
            const obj = {
                children: record[item] !== null ? record[item] : "",
                attrs: {}
            }
            if (mergeCellCol.includes(item)) {
                obj.attrs["rowSpan"] = mergeCells(record[item], item, rowindex, dataSource)
            }
            let stylefunc = () => {
                let tempStyle = {};
                if (ClickColumn.includes(item)) {
                    tempStyle = { cursor: 'pointer' }
                }
                // 单元格添加样式功能
                ColStyle.forEach(coloritem => {
                    if (item == coloritem["dataIndex"]) {
                        tempStyle = { ...tempStyle, ...coloritem["style"] }
                    }
                })
                return { ...tempStyle };
            }
            return {
                onDblclick: async () => {
                    if (ClickColumn.includes(item)) {
                        Func(record, rowindex, column, item, index);
                    } else {
                        message.warning('此栏位没有二阶数据');
                    }
                },
                style: stylefunc(),
                rowSpan: obj.attrs["rowSpan"],
            }
            }
        }
    })
}
// 一级表头表格功能:包含双击，添加样式，合并列
export function TableCustomCell1(Columns: object[], ClickColumn: string[] = [], mergeCellCol: any[] = [], dataSource: any[] = [], ColStyle: Object[] = [], Func: Function) {
    Columns.forEach((item, index) => {
        item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
        item["customCell"] = (record: Object, rowindex: number, column: Object) => {
            // 合并行功能
            const obj = {
                children: record[item["dataIndex"]] !== null ? record[item["dataIndex"]] : "",
                attrs: {}
            }
            if (mergeCellCol.includes(item["dataIndex"])) {
                obj.attrs["rowSpan"] = mergeCells(record[item["dataIndex"]], item["dataIndex"], rowindex, dataSource)
            }
            let stylefunc = () => {
                let tempStyle = {};
                if (ClickColumn.includes(item["dataIndex"])) {
                    tempStyle = { cursor: 'pointer' }
                }
                // 单元格添加样式功能
                ColStyle.forEach(coloritem => {
                    if (item["dataIndex"] == coloritem["dataIndex"]) {
                        tempStyle = { ...tempStyle, ...coloritem["style"] }
                    }
                })
                return { ...tempStyle };
            }
            return {
                onDblclick: async () => {
                    if (ClickColumn.includes(item["dataIndex"])) {
                        Func(record, rowindex, column, item, index);
                    } else {
                        message.warning('此栏位没有二阶数据');
                    }
                },
                style: stylefunc(),
                rowSpan: obj.attrs["rowSpan"],
            }
        }

    })
}

// 二阶表头表格功能
export function TableCustomCell2(Columns: object[], ClickColumn: string[] = [], mergeCellCol: any[] = [], dataSource: any[] = [], ColStyle: Object[] = [], Func: Function) {
    Columns.forEach(item => {
        if (item["children"]) {
            item["children"].forEach((citem: object, cindex: number) => {
                citem["title"] = ClickColumn.includes(citem["dataIndex"]) ? `${citem["title"]}*` : citem["title"];//可点击的栏位显示*号
                citem["customCell"] = (record: Object, rowindex: number, column: Object) => {
                    // 合并行功能
                    const obj = {
                        children: record[citem["dataIndex"]] !== null ? record[citem["dataIndex"]] : "",
                        attrs: {}
                    }
                    if (mergeCellCol.includes(citem["dataIndex"])) {
                        obj.attrs["rowSpan"] = mergeCells(record[citem["dataIndex"]], citem["dataIndex"], rowindex, dataSource)
                    }
                    let stylefunc = () => {
                        let tempStyle = {};
                        if (ClickColumn.includes(citem["dataIndex"])) {
                            tempStyle = { cursor: 'pointer' }
                        }
                        // 单元格添加样式功能
                        ColStyle.forEach(coloritem => {
                            if (citem["dataIndex"] == coloritem["dataIndex"]) {
                                tempStyle = { ...tempStyle, ...coloritem["style"] }
                            }
                        })
                        return { ...tempStyle };
                    }
                    return {
                        onDblclick: async () => {
                            if (ClickColumn.includes(citem["dataIndex"])) {
                                Func(record, rowindex, column, citem, cindex, item);
                            } else {
                                message.warning('此栏位没有二阶数据');
                            }
                        },
                        style: stylefunc(),
                        rowSpan: obj.attrs["rowSpan"],
                    }
                }

            })
        }
    })
}

// 合并列功能
export function megercell(Columns: object[], mergeCellCol: any[] = [], dataSource: any[] = []) {
    Columns.forEach((item, index) => {
        let tempcustomCell = {};
        if (item["customCell"]) {
            tempcustomCell = cloneDeep(item["customCell"]());
        }
        item["customCell"] = (record: Object, rowindex: number, column: Object) => {
            // 合并行功能
            const obj = {
                children: record[item["dataIndex"]] !== null ? record[item["dataIndex"]] : "",
                attrs: {}
            }
            if (mergeCellCol.includes(item["dataIndex"])) {
                obj.attrs["rowSpan"] = mergeCells(record[item["dataIndex"]], item["dataIndex"], rowindex, dataSource)
            }
            return {
                ...tempcustomCell,
                rowSpan: obj.attrs["rowSpan"],
                
            }
        }

    })
}

// 双击功能功能
export function dclick(Columns: object[], ClickColumn: string[] = [], Func: Function) {
    Columns.forEach((item, index) => {
        item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
        let tempcustomCell = {};
        if (item["customCell"]) {
            tempcustomCell = cloneDeep(item["customCell"]());
        }
        item["customCell"] = (record: Object, rowindex: number, column: Object) => {
            let stylefunc = () => {
                let tempStyle = {};
                if (ClickColumn.includes(item["dataIndex"])) {
                    tempStyle = { cursor: 'pointer' }
                }
                if (tempcustomCell && tempcustomCell["style"]) {
                    return { ...tempStyle, ...tempcustomCell["style"] }
                } else {

                    return { ...tempStyle };
                }
            }
            return {
                ...tempcustomCell,
                onDblclick: async () => {
                    if (ClickColumn.includes(item["dataIndex"])) {
                        Func(record, rowindex, column, item, index);
                    } else {
                        message.warning('此栏位没有二阶数据');
                    }
                },
                style:stylefunc()
            }
        }
    })
}
// 添加樣式功能
/* 
StyleArr:對象數組
    dataIndex:屬性名
    [
        {dataIndex:"",style:{}}
    ]
*/
export function addstyle(Columns: object[],StyleArr:Object[]){
    Columns.forEach((item, index) => {
        let tempcustomCell = {};
        if (item["customCell"]) {
            tempcustomCell = cloneDeep(item["customCell"]());
        }
        item["customCell"] = (record: Object, rowindex: number, column: Object) => {
            let stylefunc = () => {
                StyleArr.forEach(sitem=>{
                    if(sitem["dataIndex"] == item["dataIndex"]){
                        if(tempcustomCell["style"]){
                            return {...tempcustomCell["style"],...sitem["style"]}
                        }else{
                            return sitem["style"]
                        }
                    }else{

                    }
                })
            }
            return {
                ...tempcustomCell,
                style: stylefunc(),
            }
        }
    })
}

// 最後一行總計，列計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：給屬性賦值為總計
isNegative：是否轉爲負數
     */
export function getfinalsum(dataSource: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
    if (Array.isArray(dataSource) && dataSource.length != 0) {
        const tempObj = cloneDeep(dataSource[0]);
        calcuList.forEach((el) => {
            for (let i = 1; i < dataSource.length; i++) {
                if (isNaN(tempObj[el])) {
                    tempObj[el] = 0;
                }
                if (!isNaN(dataSource[i][el])) {
                    tempObj[el] = Number(tempObj[el]);
                    tempObj[el] += Number(dataSource[i][el]);
                }
            }
        });
        // 不包含在属性数组中的属性设置为空
        Object.keys(tempObj).forEach((el,) => {
            if (!calcuList.includes(el)) {
                tempObj[el] = null;
            }
            if (el == sumprops) {
                tempObj[el] = '總計';
            }
            // 總計需要是負數的處理
            // 不包含在數組裏面的就是整數
            if (isNegative && calcuList.includes(el)) {
                tempObj[el] = -tempObj[el];
            }
        });

        return tempObj;
    }
}
// 行計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：添加的屬性的key
isNegative：是否轉爲負數
*/
export function getrowsum(dataSource: any[], calcuList: any[], sumprop: string, isNegative: boolean = false) {
    if(dataSource.length!=0){
        dataSource.forEach(item => {
            item[sumprop] = 0;
            for (let i = 0; i < calcuList.length; i++) {
                if (!isNaN(item[calcuList[i]])) {
                    item[sumprop] += item[calcuList[i]];
                }
            }
            if (isNegative) {
                item[sumprop] = -item[sumprop];
            }
        })
    }
    
}
// 插入行計算
// 插入行合计,dataSource数据源，standard:相同的屬性才合計，calcuList要合计的列，是否是负数isNegative,合计属性sumprops
export function Inserttotalrow(dataSource: any[], standard: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
    let temp: any = cloneDeep(dataSource[0]);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let regArr: any[] = [];
    for (let i = 0; i < dataSource.length - 1; i++) {
        standard.forEach(item => {
            regArr.push(dataSource[i][item] == dataSource[i + 1][item])
        })
        if (regArr.includes(false)) {
            regArr = [];
            Object.keys(temp).forEach((item) => {
                if (!calcuList.includes(item)) {
                    temp[item] = '';
                }
                if (item == sumprops) {
                    temp[item] = '合計';
                }
                // 合計需要是負數的處理
                // 不包含在數組裏面的就是整數
                if (isNegative && calcuList.includes(item)) {
                    temp[item] = -Math.ceil(temp[item]);
                }
            });

            totalindex.push(i + 1);
            totalsum.push(temp);
            temp = cloneDeep(dataSource[i + 1]);
            continue;
        } else {
            for (let j = 0; j < calcuList.length; j++) {
                if(isNaN(temp[calcuList[j]])){
                    temp[calcuList[j]] = 0;
                }if(!isNaN(dataSource[i + 1][calcuList[j]])){
                    temp[calcuList[j]] += dataSource[i + 1][calcuList[j]];
                }
            }
        }
       
    }
    Object.keys(temp).forEach((item) => {
        if (!calcuList.includes(item)) {
            temp[item] = '';
        }

        if (item == sumprops) {
            temp[item] = '合計';
        }
        // 合計需要是負數的處理
        if (isNegative && calcuList.includes(item)) {
            temp[item] = -Math.ceil(temp[item]);
        }
    });
    totalsum.push(temp);
    totalindex.push(dataSource.length);
    for (let i = 0; i < totalindex.length; i++) {
        dataSource.splice(totalindex[i] + i, 0, totalsum[i]);
    }
    return {
        insertrowList: totalsum,
        insertIndexList: totalindex
    };
}

// float属性处理
export function handfloatprops(item, prop1, prop2) {
    item[prop1] = Number(item[prop1]);
    item[prop2] = Number(item[prop2]);
    return item[prop1] / item[prop2] == Infinity || item[prop1] == 0 || isNaN(item[prop1] / item[prop2])
        ? 0
        : ((item[prop1] / item[prop2]) * 100).toFixed(2)+"%";
}






