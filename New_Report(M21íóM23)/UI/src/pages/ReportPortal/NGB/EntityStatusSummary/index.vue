<template>
    <div class="EntityStatusSummary">
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="NGBScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'">
                        <!-- <template #remoteSearch="{ formModel, field }">
                            <ApiSelect v-model:value="formModel[field]" show-search option-filter-prop="label"
                                result-field="Table" label-field="EQUIP_NBR" value-field="EQUIP_NBR"
                                :api="optionsListApi" mode="multiple" />
                        </template> -->
                    </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">                   
                        <DynamicTable showFavorite sticky row-key="EntityStatusSummary" :PageParams="PageParams" :flexwidth="1"
                        export-type="Exceljs" id="EntityStatusSummary" :exceljsparams="PageSetting.exceljsparams"
                            :header-title="PageSetting.Title" :columns="PageSetting.Columns" :key="PageSetting.Columns"
                            :data-all="PageSetting.moreData" bordered  :data-source="PageSetting.moreData"
                            :pagination="false" export-file-name="导出" :row-class-name="getRowClassName">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                                <!-- <span>{{ $t('layout.multipleTab.reload') }}</span> -->
                            </template>
                        </DynamicTable>
                 
                </a-col>
            </a-row>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { getRowClassName, mergeTableRow,megercell } from '@/pages/ReportPortal/utils/NGBcommon';
// import { schemas } from './form-schema';
import { useForm, ApiSelect } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message } from 'ant-design-vue';
import { useRouter, useRoute } from 'vue-router';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

// import { useInfiniteScroll, useVirtualList } from '@vueuse/core'
const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1);


const EntityStatusSummary = ref<HTMLElement | null>(null);
function getImport(site: any) {   
    const { schemas } = require(`@/pages/ReportPortal/NGB/EntityStatusSummary/${bigfab}/form-schema`);
    const { baseColumns1,excelparams1 } = require(`@/pages/ReportPortal/NGB/EntityStatusSummary/${bigfab}/Table`);
    return { schemas, baseColumns1,excelparams1 };
}
let { schemas,  baseColumns1,excelparams1 } = getImport(useCommonParams.Fab);

const router = useRouter();
let ReportItem = reactive({
    // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',
});
let ReportNameParams = `EntityStatusSummary-${useCommonParams.Fab}`;
useReportCommon.AddObject(ReportNameParams,ReportItem);

let PageSetting = reactive({
    List: [],
    TableSpan: 20,
    showmenu: true,
    APIColumns: [],
    Columns: [] as any,
    moreData: [],
    SQLHidden:"",
    Title:"",
    exceljsparams: {
       
    }
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});



async function confirm(values) {
    openFullLoading();
    PageSetting.List=[];
    PageSetting.Columns = [];
    let baseParams = {
        apiName: 'EntityStaSumnewDataSet',
        FactoryType: useCommonParams.Fab, 
        G_FAC: useCommonParams.G_FAC_COM,
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });

    
    if (!values.Equipment)
    {
        closeFullLoading();
        return message.error('機台沒有選擇！'); 
    }
  
    try {
        
        const { api01: { Data: { Table: Data },
                 SQL01,
                 lstr_FromDate,
                 lstr_ToDate,
                 last_updatetime,
          
          } }  = await GetCommon({ ...baseParams, ...values });

          PageSetting.Title=`${useCommonParams.Fab} Status Summary ( Form ${values['FromDate']} To ${values['ToDate']}, last update time:${last_updatetime.slice(0,16)} )`

        
        if (Data.length!= 0) {
            
            let Message = {                
                lstr_FromDate: lstr_FromDate,
                lstr_ToDate: lstr_ToDate,
               };

            ReportItem.APIMessage = JSON.stringify(Message); 
            ReportItem.FormParams={ ...baseParams,...values }

            PageSetting.Columns = baseColumns1;
            PageSetting.exceljsparams = excelparams1;
            
            let combineCol1: any = {
                data: Data,
                mergeColNames: ['EQUIP_GRP','EQUIP_NBR'],
                firstMergeColNames: ['EQUIP_GRP','EQUIP_NBR'],
                firstMerge: 'EQUIP_GRP'
            };
            /**进行表格合并 */
            let Mergedata = mergeTableRow(combineCol1);
            PageSetting.moreData = Mergedata;           
            
            if (PageSetting.moreData.length > 500) {
                PageSetting.List = PageSetting.moreData.slice(0, 40);
            } //预先加载40条数据
            else {
                PageSetting.List = PageSetting.moreData;
            }
        } else {
            closeFullLoading();
            return message.warn('無數據!');
        }

        closeFullLoading();
    } catch {
        closeFullLoading();
    }
}
//实现无限滚动或者说懒加载
//InfiniteScroll(EntityStatusSummary, PageSetting);


// async function optionsListApi() {
//     const { api01 } = await GetCommon({
//         apiName: 'Equipment',
//         FactoryType: useCommonParams.Fab, 
//         G_FAC: useCommonParams.G_FAC_COM,
//         dept: 0
//     });
//     return api01['Data1'];
// }


</script>


