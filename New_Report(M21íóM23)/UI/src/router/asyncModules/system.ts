/**
 * system module
 */
export default {
  '/DashBoard': () => import('@/components/LayoutJSX/index.vue'),
  '/Assembly/table': () => import('@/pages/permission/role/index.vue'),
  '/Assembly/charts': () => import('@/pages/chartsdemo/BarChartDemo.vue'),
  '/Assembly/MixCharts': () => import('@/pages/chartsdemo/MixedChartDemo.vue'),
  '/Assembly/UpFile': () => import('@/components/up-down-file/up-filelist/index.vue'),
  '/Assembly/UpExcel': () => import('@/pages/excel/upexcel.vue'),
  '/Assembly/vex-table':()=>import('@/components/core/vex-table/index.vue'),
  '/Assembly/Form/UserForm':()=>import('@/pages/form/use-form/index.vue'),
  '/Assembly/Form/BasicForm':()=>import('@/pages/form/basic-form/index.vue'),
  '/Assembly/Form/DynamicForm':()=>import('@/pages/form/dynamic-form/index.vue'),
  '/Function/WaterMark': () => import('@/pages/fun/WaterMark.vue'),//build时这里一直报找不到的错，简直有毒
  '/Function/VScroll': () => import('@/pages/scroll/VirtualScroll.vue'),
  '/Function/CountTo': () => import('@/pages/fun/count-to.vue'),
  '/Function/Transition': () => import('@/pages/fun/transition.vue'),
  '/Function/Modal': () => import('@/pages/fun/modal.vue'),
  '/JS/First': () => import('@/views/JS/test1/index.vue'),
  '/JS/Second': () => import('@/views/JS/test2/index.vue'),
  '/HTML/Second': () => import('@/views/HTMLTEST/test2/index.vue'),
  '/Manage/Proof/Apply': () => import('@/views/HTMLTEST/test1/index.vue'),
  '/FSUI/DayliMove': () => import('@/pages/daylimove/index.vue'),
  '/FSUI/OperationPerformance': () => import('@/pages/OperationPerformance/index.vue'),
  '/FSUI/BLU1/PassLotSummaryReport': () => import('@/pages/PassLotSummaryReport/index.vue'),
  // '/DailyMove': () => import('@/pages/daylimove/index.vue'),
};
