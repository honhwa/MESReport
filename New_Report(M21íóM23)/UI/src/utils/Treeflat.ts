import { ColumnsType } from 'ant-design-vue/lib/table';
type Column = PropType<ColumnsType<any>>;
type IColumn = Column & {
  level: number;
  children?: IColumn[];
  dataIndex: string;
  title: string;
};
// 获取根节点最大的宽度，用于不够的补上去
function getwidth(LeafColumns: IColumn) {
  let maxwidth = 0;
  function getmaxwidth(LeafColumns: IColumn) {
    if (LeafColumns.children) {
      for (let i = 0; i < LeafColumns.children.length; i++) {
        if (LeafColumns.children[i].children) {
          getmaxwidth(LeafColumns.children[i]);
        } else {
          maxwidth += 1;
        }
      }
    } else {
      maxwidth += 1;
    }
  }
  getmaxwidth(LeafColumns);
  return maxwidth;
}
// 添加層級標記符號
function arrayTreeSetLevel(treeData: IColumn[], level: number = 0) {
  if (!Array.isArray(treeData)) return 0;
  let maxLevel = 0;
  function loop(data: IColumn[], level: number) {
    data.forEach((item: IColumn) => {
      item.level = level;
      maxLevel = Math.max(level, maxLevel);
      if (item.children && item.children.length) {
        loop(item.children, level + 1);
      }
    });
  }
  loop(treeData, level);
  return maxLevel;
}
// 每一个节点的处理
function handleleafcolumns(leafColumns: IColumn, titlearr: any[], maxlevel: number) {
  function handle(leafColumns: IColumn) {
    // 插入根group
    for (let i = 0; i < getwidth(leafColumns); i++) {
      titlearr[leafColumns.level].push(leafColumns.title);
    }
    if (leafColumns.children) {
      for (let j = 0; j < leafColumns.children.length; j++) {
        if (leafColumns.children[j].children) {
          handle(leafColumns.children[j]);
        } else {
          titlearr[leafColumns.children[j].level].push(leafColumns.children[j].title);
          // 如果是最后一层，但是没有children，而其他的有children，需要补上,由于不规则，最后一阶的需要特殊处理
          if (!leafColumns.children[j].children) {
            if (leafColumns.children[j].level < maxlevel) {
              for (let index = leafColumns.children[j].level + 1; index <= maxlevel; index++) {
                titlearr[index].push(leafColumns.children[j].title);
              }
            }
          }
        }
      }
    }
  }
  if (leafColumns.children) {
    handle(leafColumns);
  } else {
    for (let i = 0; i <= maxlevel; i++) {
      titlearr[i].push(leafColumns.title);
      // keyarr[i].push(leafColumns.dataIndex);
    }
  }
}
// 獲取最後一級的key
function getlastkey(Columns: IColumn[]) {
  let key: string[] = [];
  function fillkey(Columns: IColumn[]) {
    Columns.forEach((item) => {
      if (item.children) {
        fillkey(item.children);
      } else {
        key.push(item.dataIndex);
      }
    });
  }
  fillkey(Columns);
  return key;
}

// 最终的数据处理
export function handleColuns(Columns: IColumn[]) {
  // 获取最大长度
  let returnlevel = arrayTreeSetLevel(Columns, 0);
  let maxlength = returnlevel;
  // 表頭
  let titlearr = new Array();
  // 最後一層表頭對應的key鍵值
  let keyarr = getlastkey(Columns);
  //   初始化数组
  for (let i = 0; i <= maxlength; i++) {
    titlearr[i] = [];
  }
  Columns.forEach((item) => {
    handleleafcolumns(item, titlearr, maxlength);
  });
 /*  console.log('表头title', titlearr);
  titlearr.forEach((item, index) => {
    console.log(`表头titile${index}长度`, item.length);
  });
  console.log('表头对应的键', keyarr); */
  return {titlearr,keyarr}
}
