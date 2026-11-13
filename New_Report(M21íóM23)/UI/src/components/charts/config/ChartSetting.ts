import type { PropType, ExtractPropTypes } from 'vue';


export let chartprops = {
    backgroundColor: {
        type: String as PropType<string>,
        default: '#ffffff'
    },
    seriesData: {
        type: Array as PropType<Array<any>[]>,
        required: true,
        default: () => [],
    },
    xData: {
        type: Array as PropType<Array<any>[]>,
        required: true,
        default: () => [],
    },
    yData: {
        type: Array,
        default: function () {
            return [
                {
                    splitLine: {
                        show: true,
                        lineStyle: {
                            color: 'black',
                            width: 0.5,
                            type: 'dashed'
                        }
                    },
                    axisLine: {
                        show: true, lineStyle: {
                            color: 'black',
                            width: 0.5,
                        }
                    },
                    axisTick: { show: true },
                    type: 'value'
                },
                {
                    type: 'value'
                }
            ];
        }
    },
    height: {
        type: String as PropType<string>,
        default: '200px'
    },
    width: {
        type: String as PropType<string>,
        default: '200px'
    },
    // id: {
    //     type: String as PropType<string>,
    //     default: 'mixedChartsComponents'
    // },
    classname: {
        type: String as PropType<string>,
        default: 'chart'
    },
    showdataZoom: {
        type: Boolean as PropType<boolean>,
        default: true
    },
    title: {
        type: String as PropType<string>,
        default: '混合图'
    },
    color: {
        type: Array as PropType<Array<any>[]>,
        default: () => ['#5470c6', '#91cc75', '#fac858', '#ee6666', '#73c0de', '#3ba272', '#fc8452', '#9a60b4', '#ea7ccc'],
    },
    titlecolor: {
        type: String as PropType<string>,
        default: 'black'
    },
    tooltip: {
        type: Object as PropType<object>,
        default: function () {
            return {
                trigger: 'axis',
                axisPointer: {
                    type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'};
                },
                confine: true //把浮窗限制在圖表區域

            };
        }
    },
    rotate:{
        type:Number as  PropType<number>,
        default:0
    }
} as const
export type ChartProps = ExtractPropTypes<typeof chartprops>;
