<template>
    <!--官方文档未更新ref以及this.$refs的新写法-->
    <!--1.在标签中添加ref再在setup中定义ref为一个HTMLElement即可-->
    <div :class="props.classname" :style="{ height: props.height, width: props.width }" ref="MixedChart" />
</template>
<!--2.getCurrentInstance也可以做到类似vue2中的refs的写法-->

<script lang="ts" setup>
import { onActivated, onBeforeUnmount, onDeactivated, onMounted, nextTick, watch, ref } from 'vue';
import resize from './mixins/resize';
import { init, EChartsOption } from 'echarts';
import { chartprops } from './config/ChartSetting';
const MixedChart = ref<Nullable<HTMLElement>>(null);
// console.log(MixedChart);

const props = defineProps(chartprops);
const { mounted, chart, beforeDestroy, activated, deactivated } = resize();
const initChart = () => {
    const barChart = init(MixedChart.value as HTMLDivElement);
    barChart.setOption({
        title: {
            text: props.title,
            left: 'center',
            textStyle: {
                color: props.titlecolor
            }
        },
        grid: {
            left: '1%',
            right: '4%',
            bottom: '8%',
            containLabel: true
        },
        legend: {
            bottom: '0%',
            textStyle: {
                color: props.titlecolor
            }
        },
        tooltip: props.tooltip,
        color: props.color,
        backgroundColor: props.backgroundColor,
        xAxis: {
            type: 'category',
            data: props.xData,

            axisLabel: {
                interval: 0,
                rotate: props.rotate,
                // formatter: function (value) {
                //     var str = '';
                //     var num = 8; //每行显示字数
                //     var valLength = value.length; //该项x轴字数
                //     var rowNum = Math.ceil(valLength / num); // 行数

                //     if (rowNum > 1) {
                //         for (var i = 0; i < rowNum; i++) {
                //             var temp = '';
                //             var start = i * num;
                //             var end = start + num;

                //             temp = value.substring(start, end) + '\n';
                //             str += temp;
                //         }
                //         return str;
                //     } else {
                //         return value;
                //     }
                // },
                // textStyle: {
                //     color: props.titlecolor
                // }
            }
        },
        yAxis: props.yData,
        series: props.seriesData,
        label: {
            show: true,
            color: props.titlecolor,

            position: 'inside'
        }
    } as EChartsOption);
    chart.value = barChart;
};

onMounted(() => {
    mounted();
    nextTick(() => {
        if (props.seriesData.length > 0) {
            initChart();
        }
    });
});

onBeforeUnmount(() => {
    beforeDestroy();
});

onActivated(() => {
    activated();
});
watch(() => props.seriesData, initChart);

onDeactivated(() => {
    deactivated();
});
</script>
