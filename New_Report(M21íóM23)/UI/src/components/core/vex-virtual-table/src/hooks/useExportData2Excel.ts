//@ts-nocheck
import { isEmpty } from 'lodash-es';
import { VexVirtualTableProps } from '../vex-virtual-table';
import type { TableMethods, TableState } from './index';
import { exportexcelljs } from '@/utils/Excelljsutils';
import { exportJson2Excel } from '@/utils/Export2Excel';
import { convert } from '@/components/core/dynamic-table/src/hooks/useExportData2Excel';
import router from '@/router';
import { cloneDeep } from 'lodash';
import { GetCommon } from '@/api/pages/common';
import { TableColumn } from '../types/column';

export type UseExportData2ExcelContext = {
  state: TableState;
  methods: TableMethods;
  props: VexVirtualTableProps;
  isMerge: boolean;
};

/**
 * 导出表格Excel
 */
export const useExportData2Excel = ({ props, state, methods }: UseExportData2ExcelContext) => {
  const exportData2Excel = async () => {
    let routerTitle = router.currentRoute.value?.meta?.title;

    const { columns, exportFileName, exportBookType, exportHeader, exportType, exceljsparams,excellparams } =
      props;

    const { getColumnKey } = methods;
    const { tableExportData } = state;

    let FinalExportFileName =
      exportFileName == '导出' ? routerTitle : exportFileName ?? routerTitle;
    // columns.forEach((item: TableColumn) => item.dataIndex = item.field)
    const theaders = columns.filter(
      (n) => (getColumnKey(n) && getColumnKey(n) !== '$action') || Reflect.has(n, 'children'),
    );
    let headerColumns = convert(theaders, []);
    let header = headerColumns.map((n) => n.title);
    let multiHeader = [exportHeader.flat(), header].filter((item) => item.length > 0);
    // console.log(multiHeader);
    if (exportType == 'Exceljs') {
      exportexcelljs({
        tableprops: columns,
        tabledatas: tableExportData.value,
        exceljsparams: exceljsparams,
        filename: FinalExportFileName,
      });
    } else if (exportType == 'Api') {
      let tempexcellparams = cloneDeep(excellparams);
      if (tempexcellparams.name) {
        delete tempexcellparams.name;
      }
      if (tempexcellparams != null) {
        let {
          api01: { url },
        } = await GetCommon({
          ...tempexcellparams,
          isExportExcel: 'true',
        });
        window.location.href = url;
      } else {
        return message.warn('請點擊查詢再導出！');
      }
    } else {
      if (isEmpty(headerColumns) || isEmpty(tableExportData.value)) {
        return;
      } else {
        exportJson2Excel({
          multiHeader,
          header,
          data: tableExportData.value.map((v) =>
            headerColumns.map((item) => v[getColumnKey(item)!]),
          ),
          filename: FinalExportFileName,
          bookType: exportBookType,
          autoWidth: false,
          isMerge: false,
        });
      }
    }
  };
  return { exportData2Excel };
};
