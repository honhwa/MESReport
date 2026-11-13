<template>
    <div>
        <a-card>
             <div v-bind="containerProps" style="height: 80vh">
                <DynamicTable v-bind="wrapperProps" sticky row-key="id" :header-title="PageSetting.Title"
                    :columns="PageSetting.Columns" :key="PageSetting.List" bordered size="small"
                    :data-source="PageSetting.List" :pagination="false" export-file-name="导出"
                   :data-all="PageSetting.moreData" :explain="false" :OwnerParams="false"
                    >
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </div>
             <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
    </div>
</template>

<script lang="ts" setup>
import { useTable } from '@/components/core/dynamic-table';
import { watch, reactive, onMounted, watchEffect ,computed} from 'vue';
import { AddCommonPageParams} from '@/store/module/commonpage';
 import { GetCommon } from '@/api/pages/common';
import { useLoading } from '@/components/feat/Loading';
import { useVirtualList } from '@vueuse/core';
//import { getStartBankStatusWipDetail} from './LCM/RenderFunction';
import { ReportCommon } from '@/store/module/ReportCommon';


import { message } from 'ant-design-vue';
import { timestamp } from '@vueuse/shared';
let useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)
let ReportNameParams=`StartBankStayHour-${useCommonParams.Fab}`;

let tableParams = useReportCommon[ReportNameParams].FirstTableParams;
let formParams= useReportCommon[ReportNameParams].FormParams;

function getImport(site: any) {    
    const { getStartBankDetail,} = require(`@/pages/ReportPortal/SH/StartBankStayHour/${bigfab}/RenderFunction`);
    const { SecondLabel} = require(`@/pages/ReportPortal/SH/StartBankStayHour/${bigfab}/Table`);
    return { getStartBankDetail,SecondLabel};
}
let { getStartBankDetail,SecondLabel} = getImport("SH");

 
let PageSetting = reactive({
    List: [] as any,
    Columns: [] as any,
    Title: '',
    moreData: [],
    SQLHidden:'',
});

const [DynamicTable] = useTable(); 
PageSetting.Title=`${useCommonParams.Fab} Start Bank Stay Hour Detail`


let onMoreDataLoad = computed(() => {
    //console.log("PageSetting.moreData.length",PageSetting.moreData.length)
    if (PageSetting.moreData.length > 0) return PageSetting.moreData
    else return []
})
const { list, containerProps, wrapperProps } = useVirtualList(
    onMoreDataLoad,
    {
        itemHeight: 22,
    },
)
watch(() => list.value,
    () => {
        let pushData: any[] = []
        PageSetting.List = []        
        list.value.forEach(item => {
            if (item.index < PageSetting.moreData.length) {
                pushData.push(item.data)
            }
        })
        
        PageSetting.List = pushData
    })
// 点击提交
watchEffect(async() => {
    //一阶表格监控
    PageSetting['List'] = [];
    PageSetting['Columns'] = [];
    PageSetting['Title'] = "";
    PageSetting['SQLHidden'] = "";
    tableParams = useReportCommon[ReportNameParams].FirstTableParams;    
    getStartBankDetail(tableParams, formParams,PageSetting,SecondLabel);    
});


   
     
        







</script>
<!-- <script lang="ts">
export default{
    name:'OperationPerformanceDetail'
}
</script> -->
<style lang="less" scoped>
.btn-rows {
    button {
        margin-right: 12px;
    }
}


.StartBankStatusWip :deep(.v-center) {
    background-color: #f0f0f0;
    border: 1px solid black;
}

</style>
