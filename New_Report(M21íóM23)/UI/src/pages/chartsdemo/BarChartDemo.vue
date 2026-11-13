<template>
  <div class="chart-container">
    <PieChart
      height="100%"
      width="100%"
      id="Mixed1"
    />
  </div>
</template>
<script lang="ts">
export default {
    name: 'BarChartDemo'
};
</script>
<script lang="ts" setup>
import PieChart from '@/components/charts/PieChart.vue'

</script>

<style lang="less" scoped>
.chart-container {
  position: relative;
  width: 100%;
  height: calc(99vh - 84px);
}
</style>
