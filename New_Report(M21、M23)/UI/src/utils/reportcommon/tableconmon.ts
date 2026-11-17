// 表格共用方法

import message from "ant-design-vue/es/message"
import { cloneDeep } from "lodash-es"
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
                        tempStyle = { ...tempStyle, ...coloritem["cstyle"] }
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
                                tempStyle = { ...tempStyle, ...coloritem["cstyle"] }
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
                style: stylefunc(),
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
                onDblclick: tempcustomCell["onDblclick"] ? tempcustomCell["onDblclick"] : null,
                style: stylefunc(),
                rowSpan:tempcustomCell["rowSpan"] ? tempcustomCell["rowSpan"] : null
            }
        }
    })
}


