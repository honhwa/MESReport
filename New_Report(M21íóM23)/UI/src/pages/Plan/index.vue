<script lang="ts" setup>
import { reactive, ref, watch } from 'vue';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
const ReportCommonParams = ReportCommonWithOut();
let SchedulingParams = reactive<any>(ReportCommonParams.Scheduling);

import AddScheduling from './Add.vue';
import Query from './Query.vue';
const tableref = ref<HTMLElement | null>(null);



watch(
    () => ReportCommonParams.Scheduling,
    (newvalue) => {
        SchedulingParams = newvalue;
    },
    {
        deep: true,
        immediate: true
    }
)

let isShow = ref(true);
function changeComp(type: string) {
    switch (type) {
        case "add":
            isShow.value = true;
            break;
        case "query":
            isShow.value = false;
            (tableref as any).value.gettableData();
            break;
        default:
            break;
    }
}


</script>

<template>
    <div class="Scheduling">
        <div>
            <a-space>
                <a-button @click="changeComp('add')">新增</a-button>
                <a-button type="primary" @click="changeComp('query')">查詢</a-button>
            </a-space>
        </div>
        <add-scheduling :SchedulingParams="SchedulingParams" v-show="isShow"></add-scheduling>
        <div v-show="!isShow">
            <Query ref="tableref"></Query>
        </div>
    </div>
</template>
<style scoped lang="less">
.Scheduling {
    width: 80%;
    margin: 0 auto;
    height: 90vh;
    overflow: auto;
}
</style>