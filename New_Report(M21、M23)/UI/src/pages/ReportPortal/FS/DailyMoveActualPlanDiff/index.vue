<template>
  <div class="DayliMove">
      <a-row>
          <a-col :span="24 - PageParams.TableSpan" class="ScrollForm">
              <SchemaForm @submit="confirm"> </SchemaForm>
          </a-col>
          <a-col :span="PageParams.TableSpan">
              <div ref="DaylimoveTable" >
                  <!--  
                      v-Infinitescroll:40="{ PageSetting }"
                  -->
                  <DynamicTable  showFavorite :PageParams="PageParams" row-key="id"
                      export-type="Api"
                      :excellparams="PageSetting.excellparams" 
                      :header-title="PageSetting.Title" :columns="PageSetting.Columns" :key="PageSetting.Columns" sticky
                      :data-source="PageSetting.moreData" :data-all="PageSetting.moreData" :pagination="false"
                      :export-file-name="PageSetting.Title" :row-class-name="getRowClassNmae" :flexwidth="1">
                      <template #export-button>
                          <a-button type="success"> 导出 </a-button>
                      </template>
                  </DynamicTable>
              </div>
          </a-col>
          <div hidden>{{ PageSetting.SQLHidden }}</div>
      </a-row>
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
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { megercell, TableRendering, getRowClassNmae } from '@/pages/ReportPortal/utils/common';
import { object } from 'vue-types';


const UseParamsAdd1 = AddCommonPageParams();
const UseParamsAdd = ReportCommonWithOut();
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
    const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/DailyMoveActualPlanDiff/${site}/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/FS/DailyMoveActualPlanDiff/${site}/table`);
    return { schemas, optionsdata, baseColumns };
    //const { schemas, optionsdata } = require(`@/pages/ReportPortal/FS/MaterialKittingDataReport/${site}/form-schema`);
}
let { schemas, optionsdata, baseColumns } = getImport(UseParamsAdd.Fab);
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
    APIColumns: [] as any,
    excellparams:{}
});
const [DynamicTable] = useTable();
// 左侧菜单的配置
const [SchemaForm,dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

let importurl = ref('LCM');
importurl.value = FactoryType;
switch (FactoryType) {
    case 'LCM1':
        PageSetting.Title = 'LCMA Daily Product Plan & Execute Diff';
        break;
    // case "LCM2":
    //     PageSetting.Title = "LCMB Material Kitting Report"
    //     break;
    // case "SET1":
    //     PageSetting.Title = "LCMA Material Kitting Report";
    //     importurl.value = FactoryType
    //     break;
    // case "BLU1":
    //     PageSetting.Title = "BLA Material Kitting Report";
    //     importurl.value = FactoryType
    //     break;
    // default:
    //     break;
}

// 这个是左边菜单最后的查询按钮，可以在这里获取到后端的数据做数据处理，数据处理可以写在另外的方法里面，在这里调用就好
async function confirm(values) {
    // 开启数据加载动画
    openFullLoading();
    let baseParams = {
        name: 'Data',
        apiName: 'DailyMoveByPlanDataSet',
        G_FAC: 'A'
    };
    try {
        let {
            Data: {
                Data0: { Table: Data },
                title1,
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
        PageSetting.excellparams = { ...baseParams, ...values };
        let temp: Object[] = TableRendering(title1, title2, temp1);
        PageSetting.moreData = Data;
        PageSetting.Columns = temp;
        console.log(temp);

        // PageSetting.Columns.forEach(item => {
        //     if (item.title == "Prod ID") {
        //         megercell(item, ["Prod ID"], PageSetting.moreData)
        //     }

        // })
        megercell(
            PageSetting.Columns,
            [
                'Size',
                'Product'
            ],
            PageSetting.moreData
        );

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
async function getPlanVer(optionsdata, FactoryType: string) {
    let returndata;
    let Data = await GetCommon({
        name: 'Data',
        apiName: 'DailyPlanVer',
    });
    Data = Data.Data.Table
    Data.forEach((item) => {
      returndata=item['PLAN_VER']
        });
    optionsdata['PlanVer'] = returndata;
    
} 
InfiniteScroll(report, PageSetting);
const { mounted, mountedowner, mountedOper } = useRemoteSelect();
// 在这里给左侧菜单进行初始化
onMounted(async () => {
    await mountedowner(optionsdata, FactoryType);
    await mountedOper(optionsdata, FactoryType);
    await getPlanVer(optionsdata, FactoryType);
    dynamicFormRef.formModel["PlanVer"] =  optionsdata["PlanVer"]
}
)
</script>

<style lang="less" scoped></style>
