<template>
    <div class="report">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm2 @submit="confirm2" v-if="UseParamsAdd.Fab=='BLU1' || UseParamsAdd.Fab=='BLU2'"> </SchemaForm2>
                    <SchemaForm1 @submit="confirm1" v-else-if="showTable"> </SchemaForm1>
                    <SchemaForm2 @submit="confirm2" v-else> </SchemaForm2>
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
                            id="MaterialKittingDataReport"
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
import { reactive, unref, ref, onMounted } from 'vue';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import { getRowClassNmae, megercell } from '@/pages/ReportPortal/utils/common';

const UseParamsAdd = AddCommonPageParams();
// 获取url的厂别
let FactoryType: any = UseParamsAdd.Fab;
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
    } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/table`);
    return { schemas2, schemas1, optionsdata, baseColumns };
    //const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/form-schema`);
}
let { schemas2, schemas1, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
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
    labelWidth: 200,
    schemas:schemas2(),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical',
    submitButtonOptions: {
        text: '确认'
    }
});
const [SchemaForm2] =  useForm({
    labelWidth: 120,
    schemas: schemas1(),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

let importurl = ref('LCM');
importurl.value = FactoryType;
switch (FactoryType) {
    case 'LCM1':
        PageSetting.Title = 'LCMA Material Kitting Report';
        break;
    case 'LCM2':
        PageSetting.Title = 'LCMB Material Kitting Report';
        break;
    case 'SET1':
        PageSetting.Title = 'LCMA Material Kitting Report';
        importurl.value = FactoryType;
        break;
    case 'BLU1':
        PageSetting.Title = 'BLA Material Kitting Report';
        importurl.value = FactoryType;
        break;
    case 'BLU2':
        PageSetting.Title = 'BLA Material Kitting Report';
        importurl.value = FactoryType;
        break;
    default:
        break;
}

async function confirm1(values) {
    if (values['password'] == '25500') {
        showTable.value = false;
        message.success('success');
        //  let { schemas, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab,showTable);
        //   const [SchemaForm, dynamicFormRef] = useForm({
        //   labelWidth: 120,
        //   schemas,
        //   actionColOptions: {
        //       span: 24
        //   },
        //   layout: 'vertical'
        // });
    } else {
        showTable.value = true;
        message.warn('密碼錯誤！，請重新輸入');
    }
}

 // 这个是左边菜单最后的查询按钮，可以在这里获取到后端的数据做数据处理，数据处理可以写在另外的方法里面，在这里调用就好
 async function confirm2(values) {
  // 开启数据加载动画
  openFullLoading();
  let baseParams = {
      name: 'Data',
      apiName: 'MaterialKitDataSet',
      FactoryType: UseParamsAdd.Fab,
      G_FAC: UseParamsAdd.G_FAC_COM
  };
  if(UseParamsAdd.Fab == 'LCM1' || UseParamsAdd.Fab == 'LCM2' || UseParamsAdd.Fab == 'SET1'){
    if (values['isWoStartDay'] != 'By日期查詢' ) {
      values['FromDate'] = "";
      values['ToDate'] = "";
  }
  }
  
  if (values['PanelID'] != "") {
    //   values['PanelID'] = values['PanelID'].split("\n");
      values['PanelID'] = values['PanelID'].split(/[\r\n\t]/).filter(item => item)
  }
  if (values['BLID'] != "") {
    //   values['PanelID'] = values['PanelID'].split("\n");
      values['BLID'] = values['BLID'].split(/[\r\n\t]/).filter(item => item)
  }
  Object.keys(values).forEach((key) => {
      if (!values[key]) {
          values[key] = '';
      }
  });

    try {
        let {
            Data: {
                Data: { Table: Data },
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
        if (FactoryType == 'BLU1' || FactoryType == 'BLU2') {
            title.forEach((item, index) => {
                temp.push({
                    title: title2[index],
                    dataIndex: temp1[index],
                    key: title2[index],
                    resizable: true,
                    width: 100
                });
            });
        } else {
            title.forEach((item, index) => {
                if (item == title2[index] && index < 3) {
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
        }

        PageSetting.moreData = Data;
        PageSetting.Columns = temp;
        // PageSetting.Columns.forEach(item => {
        //     if (item.title == "Prod ID") {
        //         megercell(item, ["Prod ID"], PageSetting.moreData)
        //     }

        // })
        megercell(PageSetting.Columns, ['Prod ID'], PageSetting.moreData);

        closeFullLoading();
    } catch (error) {
        closeFullLoading();
    }
}
// async function getMarterial(optionsdata, FactoryType: string) {
//     let returndata: object[] = [];
//     let {
//         Data: {
//             Data: { Table: Data }
//         }
//     } = await GetCommon({
//         G_FAC: 'A',
//         name: 'Data',
//         apiName: 'Marterial',
//         FactoryType: FactoryType
//     });
//     console.log(Data);
    
//     Data.forEach((item) => {
//         returndata.push({
//             label: item['MAT_TYPE'],
//             value: item['MAT_TYPE']
//         });
//     });
//     returndata.unshift({ label: 'ALL', value: 'ALL' });
//     optionsdata['Material'] = returndata;
// }
// // const { mounted, mountedowner, mountedOper } = useRemoteSelect();
// // 在这里给左侧菜单进行初始化
// onMounted(async () => {
//     await getMarterial(optionsdata, FactoryType);
// });
</script>

<style lang="less" scoped></style>
