<template>
    <div class="report">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm2 @submit="confirm2"> </SchemaForm2>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  ref="report">
                        <DynamicTable
                            row-key="report"
                            :header-title="PageSetting.Title"
                            :PageParams="PageParams"
                            :columns="PageSetting.Columns"
                            showFavorite
                            :key="PageSetting.Columns"
                            export-type="Exceljs"
                            :flexwidth="80"
                            bordered
                            :row-class-name="getRowClassNmae"
                            sticky
                            :data-source="PageSetting.moreData"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                        >
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                            <template #bodyCell="{ column, text, record }">
                                <template v-if="['OQC_GROUP'].includes(column.dataIndex)">
                                    <div>
                                        <a-input
                                            v-if="editableData[record.key]"
                                            v-model:value="editableData[record.key][column.dataIndex]"
                                            style="margin: -5px 0"
                                        />
                                        <template v-else>
                                            {{ text }}
                                        </template>
                                    </div>
                                </template>
                                <template v-else-if="column.dataIndex === 'operation'">
                                    <div class="editable-row-operations">
                                        <span v-if="editableData[record.key]">
                                            <!-- <a-radio-button value="small">Small</a-radio-button> -->
                                            <TypographyLink @click="save(record.key)" :style="{ margin: '4px' }">保存</TypographyLink>
                                            <Popconfirm title="Sure to cancel?" @confirm="cancel(record.key)">
                                                <a>取消</a>
                                            </Popconfirm>
                                        </span>
                                        <span v-else>
                                            <a @click="edit(record.key)">編輯</a>
                                        </span>
                                    </div>
                                </template>
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
import { message, Popconfirm, TypographyLink } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { getRowClassNmae } from '@/pages/ReportPortal/utils/common';
const editableData = reactive({});
const UseParamsAdd = AddCommonPageParams();
// 获取url的厂别
let FactoryType: any = UseParamsAdd.Fab;
// 数据加载中的动画
const [openFullLoading, closeFullLoading] = useLoading({
  tip: '數據加載中...'
});
const { InfiniteScroll } = useRemoteSelect();
// 无限滚动的dom节点
const report = ref<HTMLElement | null>(null);
// 根据不同厂别导入不同的文件夹方法
let showTable = ref(true);
function getImport(site: any) {
  const { schemas1, optionsdata } = require(`@/pages/ReportPortal/FS/OQCDefectGroupSetup/${site}/form-schema`);
  const { baseColumns } = require(`@/pages/ReportPortal/FS/OQCDefectGroupSetup/${site}/table`);
  return { schemas1, optionsdata, baseColumns };
  //const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/form-schema`);
}
let { schemas1, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
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
const [SchemaForm2] = useForm({
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
    PageSetting.Title = '(MA406) OQC Defect Group Setup';
    break;
  case 'LCM2':
    PageSetting.Title = '(MB406) OQC Defect Group Setup';
    break;
  case 'SET1':
    PageSetting.Title = '(MV406) OQC Defect Group Setup';
    importurl.value = FactoryType;
    break;
  case 'BLU2':
    PageSetting.Title = '(BL2 403) OQC Defect Group Setup';
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
    apiName: 'OQCDfctGroupSet',
    FactoryType: UseParamsAdd.Fab,
    G_FAC: UseParamsAdd.G_FAC_COM
  };
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
    if (FactoryType == 'BLU1') {
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
    console.log("當前的PageSetting.Columns", PageSetting.Columns);

    // PageSetting.Columns.forEach(item => {
    //     if (item.title == "Prod ID") {
    //         megercell(item, ["Prod ID"], PageSetting.moreData)
    //     }

    // })
    //megercell(PageSetting.Columns, ["Prod ID"], PageSetting.moreData)

    if (PageSetting.moreData.length > 100) {
      //预先加载40条数据
      PageSetting.List = PageSetting.moreData.slice(0, 40);
    } else {
      PageSetting.List = PageSetting.moreData;
    }
    PageSetting.moreData.forEach((item: any, index) => {
      item['key'] = index.toString();
    });
    closeFullLoading();
  } catch (error) {
    closeFullLoading();
  }
}
async function getDefectcode(optionsdata, FactoryType: string) {
  let returndata: object[] = [];
  let {
    Data: {
      Data: { Table: Data }
    }
  } = await GetCommon({
    G_FAC: 'A',
    name: 'Data',
    apiName: 'OQCDfctGroupSet_Defectcode',
    FactoryType: FactoryType
  });
  Data.forEach((item) => {
    returndata.push({
      label: item['PART'],
      value: item['PART']
    });
  });
  returndata.unshift({ label: 'ALL', value: 'ALL' });
  optionsdata['defectcode'] = returndata;
}
async function getCategory(optionsdata, FactoryType: string) {
  let returndata: object[] = [];
  let {
    Data: {
      Data: { Table: Data }
    }
  } = await GetCommon({
    G_FAC: 'A',
    name: 'Data',
    apiName: 'Category_OQCDefectGroupSetup',
    FactoryType: FactoryType
  });
  Data.forEach((item) => {
    returndata.push({
      label: item['SIZE1'],
      value: item['SIZE1']
    });
  });
  returndata.unshift({ label: 'ALL', value: 'ALL' });
  optionsdata['Category'] = returndata;
}
async function Update(FactoryType: string, Update_data: Object) {
  let { Data } = await GetCommon({
    G_FAC: 'A',
    name: 'Data',
    apiName: 'OQCDfctGroupSet_Updata',
    FactoryType: FactoryType,
    PROD_NBR: Update_data['ERRC_NBR'],
    downgrade: Update_data['OQC_GROUP']
  });
  return Data;
}
const edit = (key: string) => {
  editableData[key] = cloneDeep(PageSetting.moreData.filter((item) => key === item['key'])[0]);
};
const save = async (key: string) => {
  let temp123;
  console.log(editableData[key]);
  temp123 = await Update(FactoryType, editableData[key]);
  if (temp123 == 'Success!') {
    message.success('success');
  } else {
    message.warn('false');
  }
  Object.assign(PageSetting.moreData.filter((item) => key === item['key'])[0], editableData[key]);
  delete editableData[key];
};
const cancel = (key: string) => {
  delete editableData[key];
};
InfiniteScroll(report, PageSetting);
const { mounted, mountedowner, mountedOper } = useRemoteSelect();
// 在这里给左侧菜单进行初始化
onMounted(async () => {
  await getCategory(optionsdata, FactoryType);
  await getDefectcode(optionsdata, FactoryType);
});
</script>

<style lang="less" scoped>
/* :deep(.ant-table){
    height: 80vh !important;
    overflow: auto;

} */</style>
