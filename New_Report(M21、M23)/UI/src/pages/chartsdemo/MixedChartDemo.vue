<template>
    <div class="chart-container">
        <MixedChart height="50%" width="100%" :seriesData="propsData.seriesData" :xData="propsData.xData" id="Mixed1" />
        <MixedChart
            height="50%"
            width="100%"
            :seriesData="propsData.seriesData1"
            :xData="propsData.xData1"
        />
    </div>
</template>
<script lang="ts">
export default {
    name: 'MixedChartDemo'
};
</script>
<script lang="ts" setup>
import MixedChart from '@/components/charts/MixedChart.vue';//vs版本过低无法下载Volar
import { reactive } from '@vue/reactivity';
import { GetChartDemoData } from '@/api/demos/charts';
import { onMounted } from '@vue/runtime-core';

let propsData = reactive({
    seriesData: <Array<any>>[],
    xData: <Array<any>>[],
    resdata: { data: [] },
    seriesData1: <Array<any>>[],
    xData1: <Array<any>>[]
});
propsData.xData = (function () {
    //xData右侧为一个立即执行函数;
    let data: string[] = [];
    for (let i = 1; i < 13; i++) {
        data.push(i + 'month');
    }
    return data;
})();
onMounted(async () => {
    propsData.resdata = await GetChartDemoData({ type: 'SuccessRate' });
    AnalysisData(propsData.resdata.data);
});
function AnalysisData(alldata) {
    let target: any[] = [];
    for (let i = 0; i < alldata['rate'].length; i++) {
        target.push('90');
    }
    for (let i of alldata['time']) {
        let date = i.split('-');
        if (date.length == 1) {
            propsData.xData1.push('W' + date);
        } else if (date.length == 2) {
            propsData.xData1.push(date[1] + '月');
        } else if (date.length == 3) {
            propsData.xData1.push(date[1] + '月' + date[2]);
        }
    }
    propsData.seriesData1 = [
        {
            name: '縂回訓次數',
            type: 'bar',
            data: alldata['total'],
            barMaxWidth: 35,
            barGap: '10%',
            yAxisIndex: 1
        },
        {
            name: 'Target',
            type: 'line',
            data: target,
            symbolSize: 10,
            symbol: 'circle',
            yAxisIndex: 0
        },
        {
            name: '回訓成功率',
            type: 'line',
            data: alldata['rate'],
            symbolSize: 10,
            symbol: 'circle',
            yAxisIndex: 0
        }
    ];
}
// let seriesData1: any[] = ref([]);
propsData.seriesData = [
    {
        name: 'female',
        type: 'bar',
        stack: 'total',
        barMaxWidth: 35,
        barGap: '10%',
        itemStyle: {
            color: 'rgba(255,144,128,1)',
            label: {
                show: true,
                textStyle: {
                    color: '#fff'
                },
                position: 'insideTop',
                formatter(p: any) {
                    return p.value > 0 ? p.value : '';
                }
            }
        },
        data: [709, 1917, 2455, 2610, 1719, 1433, 1544, 3285, 5208, 3372, 2484, 4078]
    },

    {
        name: 'male',
        type: 'bar',
        stack: 'total',
        itemStyle: {
            color: 'rgba(0,191,183,1)',
            borderRadius: 0,
            label: {
                show: true,
                position: 'top',
                formatter(p: any) {
                    return p.value > 0 ? p.value : '';
                }
            }
        },
        data: [327, 1776, 507, 1200, 800, 482, 204, 1390, 1001, 951, 381, 220]
    },
    {
        name: 'average',
        type: 'line',
        stack: 'total',
        symbolSize: 10,
        symbol: 'circle',
        itemStyle: {
            color: 'rgba(252,230,48,1)',
            borderRadius: 0,
            label: {
                show: true,
                position: 'top',
                formatter(p: any) {
                    return p.value > 0 ? p.value : '';
                }
            }
        },
        data: [1036, 3693, 2962, 3810, 2519, 1915, 1748, 4675, 6209, 4323, 2865, 4298]
    }
];
</script>

<style lang="less" scoped>
.chart-container {
    position: relative;
    width: 100%;
    height: calc(99vh - 84px);
}
</style>
