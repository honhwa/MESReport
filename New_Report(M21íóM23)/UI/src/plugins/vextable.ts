// import VXETable from 'vxe-table'
import 'vxe-table/lib/style.css';
import type { App } from 'vue';
import {
  // 全局对象
  VxeGrid,

  // 表格功能
  // Filter,
  // Edit,
  // Menu,
  // Export,
  // Keyboard,
  // Validator,

  // 可选组件
  Icon,
  Column,
  // Colgroup,
  // Grid,
  // Tooltip,
  // Toolbar,
  // Pager,
  // Form,
  // FormItem,
  // FormGather,
  // Checkbox,
  // CheckboxGroup,
  // Radio,
  // RadioGroup,
  // RadioButton,
  // Switch,
  // Input,
  // Select,
  // Optgroup,
  // Option,
  // Textarea,
  // Button,
  // Modal,
  // List,
  // Pulldown,

  // 表格
  Table
} from 'vxe-table'

export function setupVexTable(app: App<Element>) {
  //@ts-ignore
    app.use(Table).use(Icon).use(Column).use(VxeGrid)
  }