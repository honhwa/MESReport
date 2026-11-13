//@ts-nocheck
import { saveAs } from 'file-saver';
import { SSF, utils, write } from 'xlsx';
import type { WorkBook, WorkSheet, BookType } from 'xlsx';
import { IsDate } from '@/pages/ReportPortal/utils/common';
import dayjs from 'dayjs';
import { cloneDeep } from 'lodash-es';
interface ICell {
    v: Date | number | boolean | string;
    t: string;
    z: string;
}

class Workbook implements WorkBook {
    SheetNames: string[] = [];

    Sheets: { [sheet: string]: WorkSheet } = {};
}
// 使用dom方式導出excell
const generateArray = (table: HTMLElement) => {
    const out = [];
    const rows = table.querySelectorAll('tr');
    const ranges = [];
    let DeleteR = 0;
    for (let R = 0; R < rows.length; ++R) {
        const outRow = [];
        const row = rows[R];
        let columns: string | any[] | NodeListOf<HTMLTableCellElement>;
        columns = row.querySelectorAll('td');
        if (!columns || columns.length <= 0) {
            columns = row.querySelectorAll('th');
            DeleteR = R + 1;
        }

        for (let C = 0; C < columns.length; ++C) {
            const cell = columns[C];
            const colspanStr = cell.getAttribute('colspan');
            const rowspanStr = cell.getAttribute('rowspan');
            let colspan;
            let rowspan;
            if (colspanStr) {
                colspan = parseInt(colspanStr);
            }
            if (rowspanStr) {
                rowspan = parseInt(rowspanStr);
            }
            const cellValue = cell.innerText;
            if (cellValue == ' ') {
                console.log(1);
            }
            // Skip ranges
            ranges.forEach(function (range) {
                if (R >= range.s.r && R <= range.e.r && outRow.length >= range.s.c && outRow.length <= range.e.c) {
                    for (let i = 0; i <= range.e.c - range.s.c; ++i) outRow.push(null);
                }
            });
            // Handle Row Span
            if (rowspan || colspan) {
                rowspan = rowspan || 1;
                colspan = colspan || 1;
                ranges.push({
                    s: {
                        r: R,
                        c: outRow.length
                    },
                    e: {
                        r: R + rowspan - 1,
                        c: outRow.length + colspan - 1
                    }
                });
            }
            // Handle Value
            outRow.push(cellValue && cellValue !== '' ? cellValue : null);
            // Handle Colspan
            if (colspan) {
                for (let k = 0; k < colspan - 1; ++k) {
                    outRow.push(null);
                }
            }
        }
        // if (outRow.length > 0) 
        out.push(outRow);
    }
    out.splice(DeleteR, 1);
    return [out, ranges];
};

function datenum(v, date1904?) {
    if (date1904) v += 1462;
    const epoch = Date.parse(v);
    return (epoch - new Date(Date.UTC(1899, 11, 30)).getTime()) / (24 * 60 * 60 * 1000);
}

function sheetFromArrayOfArrays(data) {
    const ws = {};
    const range = {
        s: {
            c: 10000000,
            r: 10000000
        },
        e: {
            c: 0,
            r: 0
        }
    };
    for (let R = 0; R != data.length; ++R) {
        for (let C = 0; C != data[R].length; ++C) {
            if (range.s.r > R) range.s.r = R;
            if (range.s.c > C) range.s.c = C;
            if (range.e.r < R) range.e.r = R;
            if (range.e.c < C) range.e.c = C;
            const cell: ICell = {
                v: data[R][C],
                t: '',
                z: ''
            };
            if (cell.v == null) continue;
            const cellRef = utils.encode_cell({
                c: C,
                r: R
            });
            //
            if (typeof cell.v === 'number') cell.t = 'n';
            if (cell.v && !isNaN(cell.v)) {
                // console.log('符合數字');
                cell.t = 'n';
                cell.v = Number(cell.v);
            } else if (typeof cell.v === 'boolean') cell.t = 'b';
            else if (cell.v instanceof Date) {
                cell.t = 'n';
                cell.z = SSF._table[14];
                cell.v = datenum(cell.v);
            } else cell.t = 's';
            if (cell.v && IsDate(String(cell.v))) {
                // cell.t = 'd';
                /* cell.z = SSF._table[14];
                cell.w = 'YYYY/M/D'; 
                 cell.z = 'yyyy/m/d';*/
                /*   cell.w = 'yyyy/m/d';
                  cell.z = SSF._table[14];
                  cell.v = dayjs(cell.v).format('YYYY/M/D'); */


                cell.t = 'd';
                /*  cell.w = new Date(dayjs(cell.v).format('YYYY/M/D'));
                 cell.v = new Date(dayjs(cell.v).format('YYYY/M/D'));
                 cell.z = SSF._table[14]; */
                cell.z = 'yyyy/m/d';
                cell.w = 'YYYY/M/D';
                cell.v = dayjs(cell.v).format('YYYY/M/D');
                // cell.v = "44358";
            }
            ws[cellRef] = cell;
        }
    }

    if (range.s.c < 10000000) ws['!ref'] = utils.encode_range(range);
    return ws;
}

function s2ab(s: any) {
    const buf = new ArrayBuffer(s.length);
    const view = new Uint8Array(buf);
    for (let i = 0; i != s.length; ++i) view[i] = s.charCodeAt(i) & 0xff;
    return buf;
}
// 通過table轉excell
export function exportTableToExcel({
    id = '',
    filename = 'excel-list',
    bookType = 'xlsx' as BookType,
    isMerge = false
} = {}) {
    const theTable = document.getElementById(id);
    filename = filename || 'excel-list';
    const oo = generateArray(theTable);
    const ranges = oo[1];
    /* original data */
    const data = oo[0];
    const wsName = 'SheetJS';

    const wb = new Workbook(),
        ws = sheetFromArrayOfArrays(data);

    /* add ranges to worksheet */
    // ws['!cols'] = ['apple', 'banan'];
    if (isMerge) {
        ws['!merges'] = ranges;
    } else {
        // 將合并的轉爲不合并
        /**
         * range.s.r开始行的数据
         * range.e.r 结束行的数据
         * range.s.c 开始列的数据
         * range.e.c 结束列的数据
         */
        ranges.forEach((item) => {
            // 保留第一個合并位置的數據
            let original_address = { c: item.s.c, r: item.s.r };
            let original_cell_ref = utils.encode_cell(original_address);
            let origin_cell = ws[original_cell_ref];
            for (let R = item.s.r; R <= item.e.r; ++R) {
                for (let C = item.s.c; C <= item.e.c; ++C) {
                    let cell_address = { c: C, r: R };
                    let cell_ref = utils.encode_cell(cell_address);
                    if (typeof origin_cell == 'undefined') {
                        // 列合并
                        let precell = cloneDeep(original_address);
                        precell.r = precell.r - 1;
                        let precell_cell_ref = utils.encode_cell(precell);
                        let current_cell = ws[precell_cell_ref];
                        ws[cell_ref] = current_cell;
                    } else {
                        // 行合并
                        ws[cell_ref] = origin_cell;
                    }
                }
            }
            // console.log('輸出合并分組-------------------------');
        });
    }

    /* add worksheet to workbook */
    wb.SheetNames.push(wsName);
    wb.Sheets[wsName] = ws;
    // console.log('通過table導出excell', ws);
    const wbout = write(wb, {
        bookType: 'xlsx',
        bookSST: false,
        type: 'binary'
        // SheetJS 处理日期和时间时，默认行为是生成数字单元， 日期单元格会自动转换回具有适当数字格式的数字单元格
        // cellDates: true
    });

    saveAs(
        new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }),
        `${filename}.${bookType}`
    );
}

// 通過json轉excell
export function exportJson2Excel({
    multiHeader = [] as string[][],
    header = [] as string[],
    data = [] as any[],
    filename = 'excel-list',
    merges = [] as any[],
    autoWidth = true,
    bookType = 'xlsx' as BookType,
    isMerge = false
} = {}) {
    /* original data */
    filename = filename || 'excel-list';
    data = [...data];
    if (multiHeader.length > 0) {
        for (let i = multiHeader.length - 1; i > -1; i--) {
            data.unshift(multiHeader[i]);
        }
    } else {
        data.unshift(header);
    }


    const wsName = 'SheetJS';
    const wb = new Workbook(),
        ws = sheetFromArrayOfArrays(data);
    if (isMerge) {
        if (merges.length > 0) {
            if (!ws['!merges']) ws['!merges'] = [];
            merges.forEach((item) => {
                ws['!merges'].push(utils.decode_range(item));
            });
        }
    }

    if (autoWidth) {
        /*设置worksheet每列的最大宽度*/
        const colWidth = data.map((row) =>
            row.map((val: any) => {
                /*先判断是否为null/undefined*/
                if (val == null) {
                    return {
                        wch: 10
                    };
                } else if (val.toString().charCodeAt(0) > 255) {
                    /*再判断是否为中文*/
                    return {
                        wch: val.toString().length * 2
                    };
                } else {
                    return {
                        wch: val.toString().length
                    };
                }
            })
        );
        /*以第一行为初始值*/
        const result = colWidth[0];
        for (let i = 1; i < colWidth.length; i++) {
            for (let j = 0; j < colWidth[i].length; j++) {
                if (result[j]['wch'] < colWidth[i][j]['wch']) {
                    result[j]['wch'] = colWidth[i][j]['wch'];
                }
            }
        }

        ws['!cols'] = result;
    }

    /* add worksheet to workbook */
    wb.SheetNames.push(wsName);
    wb.Sheets[wsName] = ws;
    const wbout = write(wb, {
        bookType,
        bookSST: false,
        type: 'binary',
        // cellDates: true,
        // cellStyles: true,
        // cellNF: SSF.is_date("yyyy-mm-dd"),
    });

    saveAs(
        new Blob([s2ab(wbout)], {
            type: 'application/octet-stream'
        }),
        `${filename}.${bookType}`
    );
}
