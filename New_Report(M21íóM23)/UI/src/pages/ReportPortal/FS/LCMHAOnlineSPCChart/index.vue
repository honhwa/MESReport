<template>
  <div class="report">
      <a-card>
          <a-row>
              <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                  <SchemaForm1 @submit="confirm"> </SchemaForm1>
              </a-col>
              <a-col :span="PageParams.TableSpan">
                  <div ref="report">
                      <DynamicTable
                          row-key="report"
                          :header-title="PageSetting.Title"
                          :PageParams="PageParams"
                          sticky
                          :columns="PageSetting.Columns"
                          showFavorite
                          :key="PageSetting.Columns"
                          export-type="Table"
                          id="LCMHAOnlineSPCChart"
                          bordered
                          :row-class-name="getRowClassNmae"
                          :data-source="PageSetting.moreData"
                          :data-all="PageSetting.moreData"
                          :pagination="false"
                          :flexwidth="1"
                          :export-file-name="PageSetting.Title"
                      >
                          <template #export-button>
                              <a-button type="success"> 导出 </a-button>
                          </template>
                      </DynamicTable>
                  </div>
              </a-col>
          </a-row>
      </a-card>
  </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useLoading } from '@/components/feat/Loading';
import { GetCommon } from '@/api/pages/common';
import { reactive, ref, onMounted } from 'vue';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter } from 'vue-router';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';


const UseParamsAdd = ReportCommonWithOut();
const UseParamsAdd1 = AddCommonPageParams();
const router = useRouter();
// 获取url的厂别
let FactoryType: any = UseParamsAdd.SubFab;
// 数据加载中的动画
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});
// 无限滚动的dom节点
const report = ref<HTMLElement | null>(null);
// 根据不同厂别导入不同的文件夹方法
let showTable = ref(true);
function getImport(site: any) {
    const {
        schemas2,
        schemas1,
        optionsdata
    } = require(`@/pages/ReportPortal/FS/LCMHAOnlineSPCChart/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/LCMHAOnlineSPCChart/${site}/table`);
    return { schemas2, schemas1, optionsdata, baseColumns };
    //const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/form-schema`);
}
let { schemas2, schemas1, optionsdata, baseColumns } = getImport(FactoryType);
// 表格的配置
let PageSetting = reactive({
    //let PageSetting = reactive({
    // 数据源
    List: [] as any,
    // 表格表头
    Columns: [] as any,
    // 表格的标题
    Title: '',
    // 无限滚动数据
    moreData: [],
    // 后端返回的sql语句
    SQLHidden: '',
    // 表格表头
    APIColumns: [] as any
});
const [DynamicTable] = useTable();
// 左侧菜单的配置
const [SchemaForm1] = useForm({
    labelWidth: 120,
    schemas: schemas1,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

let importurl = ref('LCM');
importurl.value = FactoryType;
switch (FactoryType) {
    case 'LCM1':
        PageSetting.Title = 'LCMA On-Line Chart';
        break;
    case 'LCM2':
        PageSetting.Title = 'LCMB On-Line Chart';
        break;
    case 'SET1':
        PageSetting.Title = 'LCMA Material Kitting Report';
        importurl.value = FactoryType;
        break;
    case 'BLU1':
        PageSetting.Title = 'BLA Material Kitting Report';
        importurl.value = FactoryType;
        break;
    default:
        break;
}

function customRow2(record, index) {
    return {
        style: {
            'background-color': index % 2 == 0 ? '#fcc6fd' : '#fafafa'
        }
    };
}


// 这个是左边菜单最后的查询按钮，可以在这里获取到后端的数据做数据处理，数据处理可以写在另外的方法里面，在这里调用就好
async function confirm(values) {
    console.log(values);
    // 开启数据加载动画
    openFullLoading();
    let baseParams = {
        name: 'Data',
        apiName: 'LCMHAOnlineSPCChart',
        FactoryType: UseParamsAdd.SubFab,
        ReportType: '',
        wFab: 'MA'
    };

    try {
        baseParams = { ...baseParams, ...values };
        let {
            Data: {
                Data0: { Table: Data },
                title,
                title2
            }
        } = await GetCommon({ ...baseParams, ...values });
        if (Data.length == 0) {
            closeFullLoading();
            return message.warn('无数据!');
        }
        let temp1: Object[] = [];
        Object.keys(Data[0]).forEach((item) => {
            temp1.push(item);
        });

        let temp: Object[] = [];
        let children1: Object[] = [];
        let temp13 = true;
        title.forEach((item, index) => {
            if (item == title2[index]) {
                temp.push({
                    title: item,
                    dataIndex: temp1[index],
                    key: item,
                    resizable: true,
                    width: 100
                });
            } else {
                if (title[index - 1] == item) {
                    children1.push({
                        title: title2[index - 1],
                        dataIndex: temp1[index - 1],
                        key: title2[index - 1],
                        resizable: true,
                        width: 100
                    });
                } else if (title[index - 1] != 'Prod ID') {
                    children1.push({
                        title: title2[index - 1],
                        dataIndex: temp1[index - 1],
                        key: title2[index - 1],
                        resizable: true,
                        width: 100
                    });
                    temp.push({
                        title: title[index - 1],
                        children: children1
                    });
                    children1 = [];
                    temp13 = true;
                }
                if (index == title.length - 1) {
                    children1.push({
                        title: title2[index],
                        dataIndex: temp1[index],
                        key: title2[index],
                        resizable: true,
                        width: 100
                    });
                    temp.push({
                        title: item,
                        children: children1
                    });
                    children1 = [];
                    temp13 = true;
                }
            }
        });
        PageSetting.moreData = Data;
        PageSetting.Columns = temp;
        PageSetting.Columns.forEach((item) => {
            item['customCell'] = (record: Object, rowindex: number, column: Object) => {
                return {
                    onDblclick: () => {
                        if (['ONCHTYPE'].includes(item['dataIndex'])) {
                            let bottomparams = cloneDeep(baseParams);
                            bottomparams.ReportType = 'DRAW_EDC_CHART';
                            bottomparams['String_Onchtype'] = record['ONCHTYPE'];
                            bottomparams['String_Onchid'] = record['ONCHID'];
                            UseParamsAdd1.AddFirstTableParams(bottomparams);
                            router.push({ name: 'DRAW_EDC_CHARTLCM1' });
                        } else {
                            message.warning('此栏位没有二阶数据');
                        }
                    }
                };
            };
        });

        // PageSetting.Columns.forEach(item => {
        //     if (item.title == "Prod ID") {
        //         megercell(item, ["Prod ID"], PageSetting.moreData)
        //     }

        // })
        // megercell(PageSetting.Columns, ["Prod ID"], PageSetting.moreData)

        if (PageSetting.moreData.length > 100) {
            //预先加载40条数据
            PageSetting.List = PageSetting.moreData.slice(0, 40);
        } else {
            PageSetting.List = PageSetting.moreData;
        }
        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }
}
async function getCategory(optionsdata, FactoryType: string) {
    let returndata: object[] = [];
    let {
        Data: {
            Data1: { Table: Data }
        }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Data',
        apiName: 'LCMHAOnlineSPCChart_MT',
        FactoryType: FactoryType
    });
    Data.forEach((item) => {
        returndata.push({
            label: item['EQPT_ID'],
            value: item['EQPT_ID']
        });
    });
    returndata.unshift({ label: 'CMO01', value: 'CMO01' });
    optionsdata['MeasTool'] = returndata;
}
async function getGroupID(optionsdata, FactoryType: string) {
    let returndata: object[] = [];
    let {
        Data: {
            Data0: { Table: Data }
        }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Data',
        apiName: 'LCMHAOnlineSPCChart_GroupID',
        FactoryType: FactoryType
    });
    Data.forEach((item) => {
        returndata.push({
            label: item['DATA_GROUP'],
            value: item['DATA_GROUP']
        });
    });
    returndata.unshift({ label: 'ALL', value: '' });
    optionsdata['GroupID'] = returndata;
}
async function getProcess(optionsdata, FactoryType: string) {
    let returndata: object[] = [];
    let {
        Data: {
            Data0: { Table: Data }
        }
    } = await GetCommon({
        G_FAC: 'A',
        name: 'Data',
        apiName: 'LCMHAOnlineSPCChart_Process',
        FactoryType: FactoryType
    });
    Data.forEach((item) => {
        returndata.push({
            label: item['PROC_ID'],
            value: item['PROC_ID']
        });
    });
    returndata.unshift({ label: 'ALL', value: '' });
    optionsdata['Process'] = returndata;
}
const { mounted, mountedowner, mountedOper } = useRemoteSelect();
// 在这里给左侧菜单进行初始化
onMounted(async () => {
    await getGroupID(optionsdata, FactoryType);
    await getProcess(optionsdata, FactoryType);
    await getCategory(optionsdata, FactoryType);
});
</script>

<style lang="less" scoped></style>
