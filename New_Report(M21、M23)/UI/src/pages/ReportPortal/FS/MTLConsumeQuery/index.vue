<script lang="ts" setup>
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref } from 'vue';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect'
import { AddCommonPageParams } from '@/store/module/commonpage';


const { mounted, InfiniteScroll } = useRemoteSelect()
const UseParamsAdd = AddCommonPageParams()
const WipPanelSummary = ref<HTMLElement | null>(null)
function getImport(site: any) {
    let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/MoveOutTime/${site}/form-schema`);
    const {MoveOutTimeColumns,} = require(`@/pages/ReportPortal/FS/MoveOutTime/${site}/Table`);
    return { schemas, optionsdata,MoveOutTimeColumns, };
}
let { schemas, optionsdata,MoveOutTimeColumns, } = getImport(UseParamsAdd.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: [],
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas,
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {
    // let dynamicColumns: TableColumn[] = []
    // let baseParams = { apiName: 'WipPanelSummary', FactoryType: 'LCM1', G_FAC: 'A', scolname: '', sqlcond: '' }
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    PageSetting.Columns = MoveOutTimeColumns
    PageSetting.Title = 'Move Out Time'
    
    // UseParamsAdd.AddFormParams({ ...baseParams, ...values })
    // UseParamsAdd.AddAPIResultMessage(api01['Message'])
}
//实现无限滚动或者说懒加载
InfiniteScroll(WipPanelSummary, PageSetting)
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据    
    optionsdata = await mounted(optionsdata,UseParamsAdd.Fab)
})

</script>

<template>
    <div class="DayliMove">
        <a-card>
          
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div  ref="MoveOutTime">
                        <DynamicTable
                            row-key="MoveOutTime"
                            :header-title="PageSetting.Title"
                            showFavorite
                            sticky
                            :PageParams="PageParams"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Title"
                            bordered
                            size="small"
                            :data-source="PageSetting.moreData"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
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


