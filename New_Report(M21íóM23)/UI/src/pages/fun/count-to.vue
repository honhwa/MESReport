<template>
  <div title="数字动画示例">
    <Card>
      <Card.Grid class="count-to-demo-card">
        <CountTo prefix="$" :color="'#409EFF'" :startVal="1" :endVal="200000" :duration="3000" />
      </Card.Grid>
    </Card>
  </div>
</template>
<script lang="ts" setup>
  import { defineComponent } from 'vue';
  import { Card } from 'ant-design-vue';
  import { CountTo } from '@/components/feat/CountTo/index';

</script>
<style lang="less" scoped>
  .count-to-demo {
    &-card {
      width: 50%;
      font-size: 30px;
      text-align: center;
    }
  }
</style>
