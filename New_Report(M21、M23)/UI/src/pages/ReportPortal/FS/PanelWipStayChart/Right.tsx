import router from '@/router';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import ContextMenu from './Menu/openContextMenus';
import { useUserStore } from '@/store/module/user';
import { cloneDeep } from 'lodash';
const ReportCommon = ReportCommonWithOut();

interface ITableColumn {
  record: Object;
  rowindex?: Number;
  // 當前的columns
  column: Object;
  item: Object;
  index?: Number;
  // 所有的columns
  Allcolumns: Object;
  reportname?: string;
  apiparams: Object;
  // 日期键值数组
  DateArray?: string[];
  // 观察栏位，x轴
  ObservedColumnXOptions:{label:string | number,value:string | number}[];
  // 要观察的列,label是标题，value是dataindex
  ObservedColumnY: {label:string | number,value:string | number};
  // 观察值，y轴
  ObservedColumnYValueOptions:{label:string | number,value:string | number}[]
}

function handleapiparams(params: any, DateArray: string[] = []) {
  let tempconfigdata = cloneDeep(params);
  let keyArr: string[] = [];
  let apiJob: any;
  let apiName: any;
  if (Object.prototype.toString.call(tempconfigdata) === '[object Object]') {
    for (let key in tempconfigdata) {
      if (DateArray.length) {
        if (DateArray.includes(key)) {
          if (tempconfigdata[key].length == 10) {
            tempconfigdata[key] = 'yyyy-mm-dd';
          } else if (tempconfigdata[key].length == 19) {
            tempconfigdata[key] = 'yyyy-mm-dd hh:mm:ss';
          }
        }
      }
      if (Array.isArray(tempconfigdata[key])) {
        tempconfigdata[key] = tempconfigdata[key].join("','");
      }
      keyArr.push(key);
    }
    apiJob = [tempconfigdata];
  } else if (Array.isArray(tempconfigdata)) {
    tempconfigdata.forEach((item) => {
      for (let key in item) {
        if (DateArray.length) {
          if (DateArray.includes(key)) {
            if (tempconfigdata[key].length == 10) {
              tempconfigdata[key] = 'yyyy-mm-dd';
            } else if (tempconfigdata[key].length == 19) {
              tempconfigdata[key] = 'yyyy-mm-dd hh:mm:ss';
            }
          }
        }
        if (Array.isArray(item[key])) {
          item[key] = item[key].join("','");
        }
        keyArr.push(key);
      }
    });
    keyArr = Array.from(new Set(keyArr));
    apiJob = [...tempconfigdata];
  } else {
    apiName = tempconfigdata;
  }
  let temp = JSON.stringify(apiJob);
  if (temp) tempconfigdata = 'apiJob=' + temp;
  else tempconfigdata = 'apiName=' + apiName;
  return tempconfigdata;
}
// 鼠标右键功能
export function mousemodal(TableColumn: ITableColumn) {
  return {
    onContextmenu: async (e: Event) => {
      const userStore = useUserStore();
      if (userStore.perms != 'Admin') {
        return;
      }
      //取消默认的浏览器自带右键
      e.preventDefault();
      ContextMenu(e, [
        {
          label: '添加排程',
          onClick: () => {
            TableColumn.apiparams = handleapiparams(TableColumn.apiparams);
            ReportCommon.AddScheduling({
              ...TableColumn,
              // 路由id
              routeId: router.currentRoute.value.name,
              // 路由名字
              MT_COLUMN:router.currentRoute.value.meta.title,
              type: 'add',
            });
            router.push({ name: 'Scheduling' });
          },
        },
      ]);
    },
  };
}
