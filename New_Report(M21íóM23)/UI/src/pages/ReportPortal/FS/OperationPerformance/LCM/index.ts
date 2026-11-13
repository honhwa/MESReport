import { cloneDeep } from 'lodash-es';

const {
  topBaseColumns,
  bottomBaseColumns,
} = require(`@/pages/ReportPortal/FS/OperationPerformance/LCM/Table`);

// 属性处理
function handprops(item, prop1, prop2) {
  item[prop1] = Number(item[prop1]);
  item[prop2] = Number(item[prop2]);
  return item[prop1] / item[prop2] == Infinity ||
    item[prop1] == 0 ||
    isNaN(item[prop1] / item[prop2])
    ? 0
    : (item[prop1] / item[prop2]).toFixed(2);
}
// float属性处理
function handfloatprops(item, prop1, prop2) {
  item[prop1] = Number(item[prop1]);
  item[prop2] = Number(item[prop2]);
  return item[prop1] / item[prop2] == Infinity ||
    item[prop1] == 0 ||
    isNaN(item[prop1] / item[prop2])
    ? 0
    : ((item[prop1] / item[prop2]) * 100).toFixed(2);
}
// 总计行
function handleData(dataSource: any[], colAttr: any[]) {
  let keysArr = colAttr;
  const tempObj = cloneDeep(dataSource[0]);
  keysArr.forEach((el, index) => {
    for (let i = 1; i < dataSource.length; i++) {
      if (el == 'FirstYield') {
        if (isNaN(dataSource[i][el])) {
          dataSource[i][el] = 0.0;
        } else if (isNaN(tempObj[el])) {
          tempObj[el] = 0.0;
        }
        tempObj[el] = parseFloat(tempObj[el] * 100) / 100;
        tempObj[el] *= parseFloat(dataSource[i][el] * 100) / 100;
        tempObj[el] = tempObj[el].toFixed(2);
        continue;
      } else if (
        [
          'TDR',
          'TRR',
          'RWK_PASS_QTY',
          'RWK_IN_QTY',
          'PASS2_QTYRate',
          'S1_WIP_QTY',
          'S2_WIP_QTY',
          'S3_WIP_QTY',
          'RepairRate',
        ].includes(el)
      ) {
        tempObj[el] = '';
        continue;
      } else {
        if (isNaN(dataSource[i][el])) {
          dataSource[i][el] = 0.0;
        } else if (isNaN(tempObj[el])) {
          tempObj[el] = 0.0;
        }
        tempObj[el] = Number(tempObj[el]);
        tempObj[el] += Number(dataSource[i][el]);
      }
    }
  });
  dataSource.push(tempObj);
}

export function handleindexTable(
  Data: any[],
  PageSetting: Object,
  topTableProps: any[],
  bottomTableProps: any[],
) {
  console.log('LCM的执行了');
  console.log('获取到了厂别：', PageSetting['FactoryType']);

  let TopList: any = [];
  let BottomList: any = [];
  Data.forEach((item) => {
    if (item.WORK_CTR >= 2000 && item.WORK_CTR < 3000) {
      item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
      item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
      item['PASS2_QTYRate'] = handfloatprops(item, 'PASS2_QTY', 'PASS_QTY');
      BottomList.push(item);
    } else if (item.WORK_CTR >= 1000 && item.WORK_CTR < 2000) {
      item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
      item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
      item['FirstYield'] = handfloatprops(item, 'PASS1_QTY', 'PASS_QTY');
      item['TotalYield'] =
        (item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY == Infinity
          ? 0
          : (((item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY) * 100).toFixed(2);
      item['TDR'] = handprops(item, 'RWK_WIP_QTY', 'RWK_PASS_QTY');
      item['TRR'] = handprops(item, 'RWK_PASS_QTY', 'RWK_IN_QTY');
      item['RepairRate'] = handfloatprops(item, 'RWK_TORWK_QTY', 'RWK_PASS_QTY');
      TopList.push(item);
    }
    // tvset1和set1
    else if (item.WORK_CTR >= 8000 && item.WORK_CTR < 9000) {
      item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
      item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
      item['FirstYield'] = handfloatprops(item, 'PASS1_QTY', 'PASS_QTY');
      item['TotalYield'] =
        (item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY == Infinity
          ? 0
          : (((item.PASS_QTY - item.TOTALRWK_QTY) / item.PASS_QTY) * 100).toFixed(2);
      item['TDR'] = handprops(item, 'RWK_WIP_QTY', 'RWK_PASS_QTY');
      item['TRR'] = handprops(item, 'RWK_PASS_QTY', 'RWK_IN_QTY');
      item['RepairRate'] = handfloatprops(item, 'RWK_TORWK_QTY', 'RWK_PASS_QTY');
      TopList.push(item);
    } else if (item.WORK_CTR >= 9000 && item.WORK_CTR < 10000) {
      item['TD'] = handprops(item, 'WIP_QTY', 'PASS_QTY');
      item['TR'] = handprops(item, 'PASS_QTY', 'IN_QTY');
      item['PASS2_QTYRate'] = handfloatprops(item, 'PASS2_QTY', 'PASS_QTY');
      BottomList.push(item);
    }
  });
  // 合计行
  handleData(TopList, topTableProps);
  handleData(BottomList, bottomTableProps);
  // 最后一行的计算
  TopList.forEach((item) => {
    if (Number(item.WORK_CTR) == 1100 || Number(item.WORK_CTR) == 8100) {
      TopList[TopList.length - 1].IN_QTY = item.IN_QTY;
    }
    if (Number(item.WORK_CTR) == 1910 || Number(item.WORK_CTR) == 8700) {
      TopList[TopList.length - 1].PASS_QTY = item.PASS_QTY;
    }
  });
  TopList[TopList.length - 1].TD = handprops(TopList[TopList.length - 1], 'WIP_QTY', 'PASS_QTY');
  TopList[TopList.length - 1].TR = handprops(TopList[TopList.length - 1], 'PASS_QTY', 'IN_QTY');
  TopList[TopList.length - 1]['WORK_CTR'] = 'SUM';
  let flag = true;
  BottomList = BottomList.filter(Boolean);
  if (BottomList.length) {
    BottomList.forEach((item) => {
      if (['SET1', 'SET2'].includes(PageSetting['FactoryType'])) {
        if (BottomList.length - 1) {
          BottomList[BottomList.length - 1].IN_QTY = 0;
          BottomList[BottomList.length - 1].PASS_QTY =
            BottomList[BottomList.length - 1].RWK_TOMAIN_QTY;
        }
      } else {
        // 出现两个work_ctr为2401的情况
        if (Number(item.WORK_CTR) == 2401 && flag) {
          BottomList[BottomList.length - 1].IN_QTY = item.IN_QTY;
          flag = false;
        }
        if (Number(item.WORK_CTR) == 2490) {
          BottomList[BottomList.length - 1].PASS_QTY =
            parseFloat(item.PASS_QTY * 100) / 100 +
            parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100;
        }
      }
    });
    if (BottomList.length - 1) {
      BottomList[BottomList.length - 1].TD = handprops(
        BottomList[BottomList.length - 1],
        'WIP_QTY',
        'PASS_QTY',
      );
      BottomList[BottomList.length - 1].TR = handprops(
        BottomList[BottomList.length - 1],
        'PASS_QTY',
        'IN_QTY',
      );
      BottomList[BottomList.length - 1]['WORK_CTR'] = 'RWK';
      // LCM2廠別的PASS_QTY計算特殊TOBESCRAP_QTY+RWK_TOMAIN_QTY
      if (['LCM2'].includes(PageSetting['FactoryType'])) {
        BottomList[BottomList.length - 1].PASS_QTY =
          BottomList[BottomList.length - 1].TOBESCRAP_QTY +
          BottomList[BottomList.length - 1].RWK_TOMAIN_QTY;
      }
    }
  }

  if (!['SET1', 'SET2'].includes(PageSetting['FactoryType'])) {
    if (BottomList.length) {
      TopList[TopList.length - 1].TotalYield =
        parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) /
        100 /
        (parseFloat(BottomList[BottomList.length - 1].TOBESCRAP_QTY * 100) / 100 +
          parseFloat(TopList[TopList.length - 1].PASS_QTY * 100) / 100);
    }
    TopList[TopList.length - 1].TotalYield =
      TopList[TopList.length - 1].TotalYield.toFixed(4) * 100;
    TopList[TopList.length - 1].TotalYield = TopList[TopList.length - 1].TotalYield.toFixed(2);
    if (PageSetting['FactoryType'] == 'LCM2') {
      // LCM2不用算TotalYield
      TopList[TopList.length - 1].TotalYield = '';
    }
  }

  PageSetting['TopList'] = TopList;
  PageSetting['BottomList'] = BottomList;
  PageSetting['TopColumns'] = topBaseColumns;
  PageSetting['BottomColumns'] = bottomBaseColumns;
}
