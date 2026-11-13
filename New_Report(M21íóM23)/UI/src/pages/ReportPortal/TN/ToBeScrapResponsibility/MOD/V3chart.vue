<template>
  <div
    ref="chartRef"
    :class="props.className"
    :style="{ height: props.height, width: props.width }"
  />
</template>

<script lang="ts" setup>
  import {
    onActivated,
    PropType,
    onMounted,
    onBeforeUnmount,
    unref,
    ref,
    watch,
    nextTick,
    defineProps,
  } from 'vue';
  import type { ECharts, EChartsOption } from 'echarts';
  import * as echarts from 'echarts';
  const props = defineProps({
    // 自定义组件的类名
    className: {
      type: String as PropType<string>,
      default: 'chart',
    },
    // 图表容器的宽度
    width: {
      type: String as PropType<string>,
      default: '100%',
    },
    // 图表容器的高度
    height: {
      type: String as PropType<string>,
      default: '100%',
    },
    // 图表参数
    options: {
      type: Object as PropType<EChartsOption | undefined>,
      default: undefined,
    },
    // 图表是否具有点击事件
    isClick: {
      type: Boolean as PropType<boolean>,
      default: false,
    },
  });
  const chartRef = ref<HTMLCanvasElement | null>(null);
  let chart: ECharts | null = null;
  watch(
    () => props.options,
    (options: EChartsOption | undefined) => {
      nextTick(() => {
        if (chart) {
          chart.setOption(options as EChartsOption, true);
        }
      });
    },
    {
      deep: true,
    },
  );
  const emits = defineEmits(['callbackFun']);
  onMounted(() => {
    // 设置异步，不然图例一开始的宽度不正确。
    nextTick(() => {
      initChart();
    });
  });
  const initChart = (): void => {
    // 初始化echart
    const chartRefWrap = unref(chartRef);
    if (chartRefWrap) {
      chart = echarts.init(chartRefWrap);
      // 若图表需要点击事件做些其他功能，在初始化示例时注册图表的点击事件
      if (props.isClick) {
        chart.on('click', (params: any) => {
          emits('callbackFun', params);
        });
      }
      chart.setOption(props.options as EChartsOption, true);
      window.addEventListener('resize', chartsResize);
    }
  };
  onActivated(() => {
    // 防止keep-alive之后图表变形
    if (chart) {
      nextTick(() => {
        chart!.resize();
      });
    }
  });

  const chartsResize = () => {
    chart && chart.resize();
  };

  onBeforeUnmount(() => {
    window.removeEventListener('resize', chartsResize);
    chart = null;
  });
</script>

<style></style>
