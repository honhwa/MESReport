<template>
  <div class="QualityReport">
    <a-row>
      <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
        <SchemaForm @submit="confirm"> </SchemaForm>
      </a-col>
      <a-col :span="PageParams.TableSpan">
        <div ref="QualityReport">
          <DynamicTable
            id="QualityReport"
            row-key="reportref"
            :header-title="PageSetting.Title"
            showFavorite
            :PageParams="PageParams"
            :columns="PageSetting.TopColumns"
            :key="PageSetting.TopColumns"
            sticky
            export-type="Multi"
            :row-class-name="getRowClassNmae"
            :data-source="PageSetting.moreData"
            :pagination="false"
            :data-all="PageSetting.moreData"
            export-file-name="QualityReport"
            :flexwidth="1"
          >
            <template #export-button>
              <a-button type="success"> 导出 </a-button>
            </template>
          </DynamicTable>
        </div>
      </a-col>
    </a-row>
  </div>
</template>

<script lang="tsx" setup>
  import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
  import { useTable } from '@/components/core/dynamic-table';
  import { reactive, ref } from '@vue/reactivity';
  import { useForm } from '@/components/core/schema-form';
  import { GetCommon } from '@/api/pages/common';

  import { onMounted } from '@vue/runtime-core';
  import { message } from 'ant-design-vue';
  import { getrowsum, Inserttotalrow } from '@/pages/ReportPortal/utils/common';
  import { cloneDeep } from 'lodash';
  import { PovitParams } from '@/utils/reportcommon/provit';
  import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
  import { ReportCommon } from '@/store/module/ReportCommon';
  import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
  import { useRouter } from 'vue-router';
  // import { schemas, optionsdata } from '@/pages/ReportPortal/FS/ExpQualityQuery/LCM1/form-schema';

  import { useLoading } from '@/components/feat/Loading';
  const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...',
  });
  const UseParamsAdd = ReportCommon();
  let FactoryType: any = UseParamsAdd.Fab;
  let SubFab: any = UseParamsAdd.SubFab;
  const router = useRouter();
  let parentId = router.currentRoute.value.meta.parentId;
  // 頁面數據
  let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    Title: '',
    moreData: [] as any,
  });

  function getImport(site: any) {
    const {
      schemas,
      optionsdata,
    } = require(`@/pages/ReportPortal/FS/ExpQualityQuery/${site}/form-schema`);
    return { schemas, optionsdata };
  }
  const { schemas, optionsdata } = getImport('LCM1');

  const [DynamicTable] = useTable();
  const {
    mountedowner,
    mountedDefectCode,
    mountedLCMGrade,
  } = useRemoteSelect();

  const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
      span: 24,
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical',
  });
  let TopTableParams = reactive({});
  async function confirm(values) {
    let baseParams = {
      apiName: 'ExpQualityDataSet',
      FactoryType: FactoryType,
      G_FAC: 'A',
      ReportType: '',
    };
    openFullLoading();
    PageSetting.TopList = [];
    PageSetting.TopColumns = [];

    Object.keys(values).forEach((key) => {
      if (!values[key]) {
        values[key] = '';
      }
      if (['WorkOrderLike', 'Operator'].includes(key)) {
        // values[key] = values[key].split(',');
        values[key] = values[key].split(/[\r\n\t]/).filter((item) => item);
      }
    });

    TopTableParams = { ...baseParams, ...values };

    try {
      const { api01 } = await GetCommon(TopTableParams);
      let Data0 = api01['Data']['Table'];
      let Data1 = api01['Data1']['Table'];
      let SQLData = api01['Message'];
      if (Data0.length == 0 && Data1.length == 0) {
        closeFullLoading();
        return message.warn('无数据！');
      }
      let baseColumns: any[] = [
        {
          title: 'process name',
          align: 'center',
          dataIndex: `DFCT_CODE`,
          key: `DFCT_CODE`,
          className: 'v-center',
          fixed: 'left',
        },
        {
          title: 'process name',
          align: 'center',
          dataIndex: `ERRC_DESCR`,
          key: `ERRC_DESCR`,
          className: 'v-center',
          fixed: 'left',
        },
      ];
      let finalData: object[] = [];
      // inspection qty记录
      let inspectionobj = {
        DFCT_CODE: 'INSPECTION QTY',
        ERRC_DESCR: 'INSPECTION QTY',
        'TOTAL YIELD QTY': '+PD',
        'TOTAL YIELD RATE': '-PD',
      };
      let good = {
        DFCT_CODE: 'GOOD',
        ERRC_DESCR: 'GOOD',
        'TOTAL YIELD QTY': 0,
        'TOTAL YIELD RATE': 0,
      };
      let ng = {
        DFCT_CODE: 'NG',
        ERRC_DESCR: 'NG',
        'TOTAL YIELD QTY': 0,
        'TOTAL YIELD RATE': 0,
      };
      let yieldrate = {
        DFCT_CODE: 'YIELD Rate',
        ERRC_DESCR: 'YIELD Rate',
        'TOTAL YIELD QTY': 1,
        'TOTAL YIELD RATE': 1,
      };
      let yieldloss = {
        DFCT_CODE: 'YIELD LOSS',
        ERRC_DESCR: 'YIELD LOSS',
        'TOTAL YIELD QTY': 1,
        'TOTAL YIELD RATE': 1,
      };
      let defectitem = {
        DFCT_CODE: 'defect ITEM',
        ERRC_DESCR: 'defect ITEM',
        'TOTAL YIELD QTY': 'QTY',
        'TOTAL YIELD RATE': '%',
      };
      let middleProps: any[] = [];
      let middlerateProps: any[] = [];
      let middlesecondqtyprops: any[] = [];

      Data0.forEach((item) => {
        middleProps.push(item['WORK_CTR']);
        middlerateProps.push(`${item['WORK_CTR']}RATE`);
        middlesecondqtyprops.push(`${item['WORK_CTR']}-SECOND_QTY`);
        baseColumns.push(
          {
            title: FactoryType == 'SET1' ? item['WORK_CTR'] : item['WORK_NAME'],
            align: 'center',
            dataIndex: `${item['WORK_CTR']}`,
            key: `${item['WORK_CTR']}QTY`,
          },
          {
            title: FactoryType == 'SET1' ? item['WORK_CTR'] : item['WORK_NAME'],
            align: 'center',
            dataIndex: `${item['WORK_CTR']}RATE`,
            key: `${item['WORK_CTR']}RATE`,
          },
        );
        inspectionobj[`${item['WORK_CTR']}`] = item['QTY'];
        inspectionobj[`${item['WORK_CTR']}RATE`] = item['QTY'];
      });

      finalData.push(inspectionobj);
      // Data1添加属性
      Data1.forEach((item) => {
        item[`${item['WORK_CTR']}-SECOND_QTY`] = item['SECOND_QTY'];
      });

      // Data1處理,行列轉換
      // 表格数据
      let config: PovitParams = {
        Data: Data1,
        povitColumn: {
          WORK_CTR: 'MAIN_QTY',
        },
        changeColumn: middleProps,
        repeatColumn: ['DFCT_CODE', 'ERRC_DESCR'],
      };
      let povitData = TableMerge(config);
      function TableMerge(config: PovitParams) {
        // 步骤1：行转列
        config.Data.forEach((item) => {
          for (let i in config.povitColumn) {
            // 遍历key
            if (i.trim().split(' ').length > 1) {
              let key = '';
              i.trim()
                .split(' ')
                .forEach((Iitem, index) => {
                  if (index != i.trim().split(' ').length - 1) {
                    key += item[Iitem] + ' ';
                  } else {
                    key += item[Iitem];
                  }
                });
              item[key] = item[config.povitColumn[i]];
            } else {
              item[item[i]] = item[config.povitColumn[i]];
            }
          }
        });
        // 步骤2：进行合并，两个数组进行合并
        // Data1和Data2是比较数组
        let Data1 = cloneDeep(config.Data);
        let Data2 = cloneDeep(config.Data);
        // 保存不同的索引
        let Data3: number[] = [];
        let regArr: any[] = [];
        for (let i = 0; i < Data1.length; i++) {
          for (let j = i + 1; j < Data2.length; j++) {
            config.repeatColumn.forEach((item) => {
              regArr.push(Data1[i][item] == Data2[j][item]);
            });
            // 如果不相同
            if (regArr.includes(false)) {
              regArr = [];
              continue;
            } else {
              // 相同，合并并且删除Data1中和Data2相同索引
              Data1[i] = { ...Data1[i], ...Data2[j] };
              Data3.push(j);
            }
          }
        }
        // 需要倒叙删除，不然就使用用filter
        Data3 = [...new Set(Data3)];
        for (let i = Data1.length; i >= 0; i--) {
          Data3.forEach((item) => {
            if (i == item) {
              Data1.splice(i, 1);
            }
          });
        }
        Data1.forEach((Titem) => {
          // 步骤3：添加属性,不能一边删除一边添加属性
          config.changeColumn.forEach((item) => {
            if (!Titem.hasOwnProperty(item)) {
              Titem[item] = null;
            }
          });
        });
        return Data1;
      }
      // 計算？品質
      povitData.forEach((item) => {
        item['condition'] = item['DFCT_CODE'].substr(0, 1);
      });
      let tempmiddleProps = [...middleProps, ...middlerateProps];
      for (let i = 0; i < tempmiddleProps.length; i++) {
        tempmiddleProps[i] = String(tempmiddleProps[i]);
      }
      // 列计算
      getrowsum(povitData, middleProps, 'TOTAL YIELD QTY');
      let { insertrowList, insertIndexList } = Inserttotalrow(
        povitData,
        ['condition'],
        [...tempmiddleProps, ...middlesecondqtyprops, 'TOTAL YIELD QTY'],
        'DFCT_CODE',
        '總計',
        false,
        false,
      );
      // 数组头部添加新的值
      insertIndexList.unshift(0);
      // 数组头部移除最后一位数字
      insertIndexList.pop();
      // 插入之后的位置
      let afterinsert: number[] = [];
      for (let i = 0; i < insertIndexList.length; i++) {
        povitData.splice(insertIndexList[i] + i, 0, insertrowList[i]);
        afterinsert.push(insertIndexList[i] + i);
      }
      afterinsert.forEach((item) => {
        povitData[item]['DFCT_CODE'] = `${povitData[item + 1]['condition']}品`;
        povitData[item]['ERRC_DESCR'] = `${povitData[item + 1]['condition']}品`;
      });

      povitData.forEach((item) => {
        // 當前行倒數第二列的括號計算方式
        let totalqty = 0;
        middleProps.forEach((titem, index) => {
          if (item[titem] != null) {
            // 计算从第6行开始的百分号那一列的计算
            // debugger
            item[middlerateProps[index]] =
              finalData[0][titem] != 0
                ? Number(((item[titem] / finalData[0][titem]) * 100).toFixed(3))
                : 0;
            // 添加括号（）
            if (item[`${titem}-SECOND_QTY`] != undefined) {
              item[titem] = `${item[titem]}(${item[`${titem}-SECOND_QTY`]})`;
              totalqty += Number(`${item[`${titem}-SECOND_QTY`]}`);
            } else {
              item[titem] = '';
            }
          }
        });
        item['LastQtyData'] = totalqty;
        // item["TOTAL YIELD RATE"] = (item["TOTAL YIELD QTY"] /tempsum).toFixed(3)
        // item["TOTAL YIELD RATE"] = Math.floor((item["TOTAL YIELD QTY"] / tempsum) * 1000) / 1000

        // 最後一列的計算公式
      });
      Data0.forEach((item) => {
        let NGData = parseInt(povitData[0][item['WORK_CTR']].split('(')[0]);
        ng[`${item['WORK_CTR']}`] = NGData;
        ng[`${item['WORK_CTR']}RATE`] = NGData;
        // ng[`${item['WORK_CTR']}RATE`] = povitData[0][`${item['WORK_CTR']}RATE`];
        good[`${item['WORK_CTR']}`] = item['QTY'] - NGData;
        good[`${item['WORK_CTR']}RATE`] = item['QTY'] - NGData;
        yieldrate[`${item['WORK_CTR']}`] =
          item['QTY'] != 0 ? (((item['QTY'] - NGData) / item['QTY']) * 100).toFixed(3) : 0;
        yieldrate[`${item['WORK_CTR']}RATE`] =
          item['QTY'] != 0 ? (((item['QTY'] - NGData) / item['QTY']) * 100).toFixed(3) : 0;
        yieldloss[`${item['WORK_CTR']}`] =
          item['QTY'] != 0 ? ((NGData / item['QTY']) * 100).toFixed(3) : 0;
        yieldloss[`${item['WORK_CTR']}RATE`] =
          item['QTY'] != 0 ? ((NGData / item['QTY']) * 100).toFixed(3) : 0;
        defectitem[`${item['WORK_CTR']}`] = 'QTY';
        defectitem[`${item['WORK_CTR']}RATE`] = '%';
        good['TOTAL YIELD QTY'] += good[`${item['WORK_CTR']}`];
        good['TOTAL YIELD RATE'] += good[`${item['WORK_CTR']}RATE`];
        ng['TOTAL YIELD QTY'] += ng[`${item['WORK_CTR']}`];
        ng['TOTAL YIELD RATE'] += ng[`${item['WORK_CTR']}`];
      });

      middleProps.forEach((item) => {
        yieldrate['TOTAL YIELD QTY'] *= Number(yieldrate[item]) / 100;
      });
      yieldrate['TOTAL YIELD QTY'] = Number(yieldrate['TOTAL YIELD QTY'].toFixed(5)) * 100;
      yieldrate['TOTAL YIELD QTY'] = Number(yieldrate['TOTAL YIELD QTY'].toFixed(3));

      yieldrate['TOTAL YIELD RATE'] = yieldrate['TOTAL YIELD QTY'];
      yieldloss['TOTAL YIELD QTY'] = Number((100 - yieldrate['TOTAL YIELD QTY']).toFixed(3));
      yieldloss['TOTAL YIELD RATE'] = yieldloss['TOTAL YIELD QTY'];
      // 统一添加百分号
      [...middleProps, ...middlerateProps, 'TOTAL YIELD QTY', 'TOTAL YIELD RATE'].forEach(
        (item) => {
          yieldrate[item] = yieldrate[item] + '%';
          yieldloss[item] = yieldloss[item] + '%';
        },
      );

      povitData.forEach((item) => {
        let LastRate =
          ng['TOTAL YIELD QTY'] != 0
            ? (item['TOTAL YIELD QTY'] / ng['TOTAL YIELD QTY']) *
              Number(String(yieldloss['TOTAL YIELD RATE']).replace('%', ''))
            : 0;
        item['TOTAL YIELD RATE'] = LastRate.toFixed(3);
        item['TOTAL YIELD QTY'] = `${item['TOTAL YIELD QTY']}(${item['LastQtyData']})`;
      });
      
      finalData.push(good, ng, yieldrate, yieldloss, defectitem);
      finalData.push(...povitData);
      baseColumns.push(
        {
          title: 'TOTAL YIELD',
          align: 'center',
          dataIndex: `TOTAL YIELD QTY`,
          key: `TOTAL YIELD QTY`,
        },
        {
          title: 'TOTAL YIELD',
          align: 'center',
          dataIndex: `TOTAL YIELD RATE`,
          key: `TOTAL YIELD RATE`,
        },
      );
      async function handclick(record, rowindex, column, item, index) {
        if (['BLU1', 'BLU2'].includes(FactoryType)) {
          if (rowindex <= 5) {
            return message.warn('此欄位沒有第二層資料');
          }
        }
        let bottomparams = cloneDeep(TopTableParams);

        bottomparams['ReportType'] = 'QualityDetail1';
        if (['BLU1', 'BLU2'].includes(FactoryType)) {
          bottomparams['opercond'] = item['dataIndex'].replace('RATE', '');
        } else {
          bottomparams['opercond'] = item['dataIndex'];
        }
        if (item['dataIndex'] == 'TOTAL YIELD QTY' || item['dataIndex'] == 'TOTAL YIELD RATE') {
          bottomparams['opercond'] = 'TOTAL YIELD';
        }
        bottomparams['defcond1'] = record['DFCT_CODE'];
        bottomparams['dataIndex'] = record[bottomparams['opercond']] ?? '';

        UseParamsAdd.AddFirstTableParams('ExpQualityQuery' + FactoryType, {
          SQLData,
          ...bottomparams,
        });
        router.push({ name: `ExpQualityReportDetail` + FactoryType });
      }
      let ClickColumn = [...tempmiddleProps, 'TOTAL YIELD QTY', 'TOTAL YIELD RATE'];
      // 合并表头
      baseColumns.forEach((item, index) => {
        if (index % 2 === 0) {
          item['colSpan'] = 2;
        } else {
          item['colSpan'] = 0;
        }
        // item["title"] = ClickColumn.includes(item["dataIndex"]) ? `${item["title"]}*` : item["title"];//可点击的栏位显示*号
        item['customCell'] = (record: Object, rowIndex: number, column: Object) => {
          let obj = {};
          // 双击功能功能
          let stylefunc = () => {
            let tempStyle = {};
            if (ClickColumn.includes(item['dataIndex'])) {
              tempStyle = { cursor: 'pointer' };
            } else {
              tempStyle = {};
            }
            return { ...tempStyle };
          };
          (obj['onDblclick'] = async () => {
            if (ClickColumn.includes(item['dataIndex'])) {
              handclick(record, rowIndex, column, item, index);
            } else {
              message.warning('此栏位没有二阶数据');
            }
          }),
            (obj['style'] = stylefunc());
          if (rowIndex < 5) {
            if (column['key'].includes('QTY') && !column['key'].includes('TOTAL')) {
              return {
                colSpan: 2, // 跨几列合并，这里现在是跨两列
                ...obj,
              };
            } else if (column['key'].includes('RATE') && !column['key'].includes('TOTAL')) {
              return {
                colSpan: 0,
                ...obj,
              };
            } else if (column['key'].includes('DFCT_CODE')) {
              return {
                colSpan: 2,
                ...obj,
              };
            } else if (column['key'].includes('ERRC_DESCR')) {
              return {
                colSpan: 0,
                ...obj,
              };
            }
          } else if (rowIndex == 5) {
            if (column['key'].includes('DFCT_CODE')) {
              return {
                colSpan: 2,
                ...obj,
              };
            } else if (column['key'].includes('ERRC_DESCR')) {
              return {
                colSpan: 0,
                ...obj,
              };
            }
          } else if (rowIndex > 5) {
            return {
              ...obj,
            };
          }
        };
      });
      PageSetting.TopColumns = baseColumns;
      // 清除為0或者0(0)的數據
      let keyarr = Object.keys(finalData[0]);
      finalData.forEach((item, index) => {
        if (index > 5) {
          keyarr.forEach((kitem) => {
            if ([0, '0(0)'].includes(item[kitem])) {
              item[kitem] = '';
            }
          });
        }
      });
      
      PageSetting.moreData = finalData;

      // if (PageSetting.moreData.length > 20) {
      //   //预先加载40条数据
      //   PageSetting.TopList = PageSetting.moreData.slice(0, 40);
      // } else {
      //   PageSetting.TopList = PageSetting.moreData;
      // }
      closeFullLoading();
    } catch (error) {
      console.log(error);
      closeFullLoading();
    }
  }

  onMounted(async () => {
    await mountedowner(optionsdata, FactoryType);
    await mountedDefectCode(optionsdata, FactoryType);
    await mountedLCMGrade(optionsdata, FactoryType);
    // switch (FactoryType) {
    //   case 'LCM1':
    //     await mountedOper(optionsdata, FactoryType);
    //     await mountedLCDProduct(optionsdata, FactoryType, 1);
    //     await mountedLCMProductSize(optionsdata, FactoryType);
    //     await mounted(optionsdata, FactoryType);
    //     await mountedEquip(optionsdata, FactoryType, 0);
    //     await mountedLcmProdType(optionsdata, FactoryType);
    //     await mountedExpectDefectCode(optionsdata, FactoryType);
    //     break;
    //   case 'LCM2':
    //     await mountedLCDProduct(optionsdata, FactoryType, 2);
    //     await mountedLCMProductSize(optionsdata, FactoryType);
    //     await mounted(optionsdata, FactoryType);
    //     await mountedEquip(optionsdata, FactoryType, 0);
    //     await mountedLcmProdType(optionsdata, FactoryType);
    //     await mountedExpectDefectCode(optionsdata, FactoryType);
    //     break;
    //   default:
    //     break;
    // }
  });
</script>

<style lang="less" scoped>
  .btn-rows {
    button {
      margin-right: 12px;
    }
  }

  .swapper {
    display: flex;
    flex-direction: column;
  }
</style>
