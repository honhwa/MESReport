//@ts-nocheck
import { isEmpty } from 'lodash-es';
import { DynamicTableProps } from '../dynamic-table';
import type { TableMethods, TableState } from './index';
import { exportJson2Excel, exportTableToExcel } from '@/utils/Export2Excel';
import { exportexcelljs } from '@/utils/Excelljsutils';
import router from '@/router';
import dayjs from 'dayjs';
import { cloneDeep } from 'lodash';
import { GetCommon } from '@/api/pages/common';

export type UseExportData2ExcelContext = {
    state: TableState;
    methods: TableMethods;
    props: DynamicTableProps;
    isMerge: boolean;
};

/**
 * 导出表格Excel
 */
export const useExportData2Excel = ({ props, state, methods, isMerge }: UseExportData2ExcelContext) => {
    const exportData2Excel = async () => {
        let routerTitle = router.currentRoute.value?.meta?.title;

        const {
            columns,
            exportFileName,
            exportBookType,
            exportType,
            id,
            excellparams,
            exceljsparams,
            exportHeader
        } = props;

        const { getColumnKey } = methods;
        const { tableExportData } = state;
        const theaders = columns.filter(
            (n) => (getColumnKey(n) && getColumnKey(n) !== '$action') || Reflect.has(n, 'children')
        );

        let FinalExportFileName = exportFileName == '导出' ? routerTitle : exportFileName ?? routerTitle;
        if (exportType == 'Multi') {
            /** 与exportFormatter区别为Function已固定 */
            // const { header, data, multiHeader } = ExportExcelFunction(theaders, tableExportData.value, getColumnKey);
            let headerColumns = convert(theaders, []);
            let header = headerColumns.map((n) => n.title)
            const oneDArrays = exportHeader.map(subArray => subArray.concat())
            let multiHeader = [...oneDArrays, header].filter(item=>item.length>0);

            if (isEmpty(headerColumns) || isEmpty(tableExportData.value)) {
                return;
            } else {
                exportJson2Excel({
                    multiHeader,
                    header,
                    data: tableExportData.value.map((v) => headerColumns.map((item) => v[getColumnKey(item)!])),
                    filename: FinalExportFileName,
                    bookType: exportBookType,
                    autoWidth: false,
                    isMerge: false
                });
            }
        } else if (exportType == 'Table') {
            exportTableToExcel({ id, filename: exportFileName, isMerge: false });
        } else if (exportType == 'Api') {
            let tempexcellparams = cloneDeep(excellparams);
            if (tempexcellparams.name) {
                delete tempexcellparams.name;
            }
            if (tempexcellparams != null) {
                let {
                    api01: { url }
                } = await GetCommon({
                    ...tempexcellparams,
                    isExportExcel: 'true'
                });
                window.location.href = url;
            } else {
                return message.warn('請點擊查詢再導出！');
            }
        } else if (exportType == 'Exceljs') {
            exportexcelljs({
                tableprops: columns,
                tabledatas: tableExportData.value,
                exceljsparams: exceljsparams,
                filename: FinalExportFileName
            });
        } else {
            exportJson2Excel({
                header: theaders.map((n) => n.title),
                data: tableExportData.value.map((v) => theaders.map((header) => v[getColumnKey(header)!])),
                filename: FinalExportFileName,
                bookType: exportBookType,
                autoWidth: false,
                isMerge: false
            });
        }
    };
    return { exportData2Excel };
};

/**
 *
 * @param columns 原始列
 * @param tableData 原始數據
 * @param getColumnKey 獲取key
 * @returns 表頭,數據,多級表頭
 */
export function ExportExcelFunction(columns: TableColumn<any>[], tableData: any[], getColumnKey: Function) {
    let headerColumns: TableColumn<any>[] = [],
        multiHeaderColumns = [] as any[][];
    multiHeaderColumns[0] = [];
    multiHeaderColumns[1] = [];
    columns.forEach((item) => {
        if (Reflect.has(item, 'children')) {
            item['children'].forEach((i) => {
                // headerColumns.push(i);
                // datatest.push(item);
                if (Reflect.has(i, 'children')) {
                    i['children'].forEach((j) => {
                        headerColumns.push(j);
                        multiHeaderColumns[1].push(i['title']);
                        multiHeaderColumns[0].push(item['title']);
                    });
                } else {
                    headerColumns.push(i);
                    multiHeaderColumns[0].push(item['title']);
                }
            });
        } else {
            headerColumns.push(item);
            multiHeaderColumns[0].push(item['title']);
            multiHeaderColumns[1].push(item['title']);
        }
    });
    if (multiHeaderColumns[1].length < multiHeaderColumns[0].length) {
        multiHeaderColumns[1] = [];
    }
    return {
        header: headerColumns.map((n) => n.title),
        data: tableData.map((v) => headerColumns.map((item) => v[getColumnKey(item)!])),
        multiHeader: multiHeaderColumns
    };
}
export function convert(array: any[], headerColumns: TableColumn<any>[] = []) {
    if (!array.length) return headerColumns
    array.forEach((item) => {
        if (Reflect.has(item, 'children')) {
            convert(item['children'], headerColumns)
        } else {
            headerColumns.push(item)
        }
    });
    return headerColumns
}
