/**
 * Component list, register here to setting it in the form
 */
import {
  Input,
  Select,
  Radio,
  Checkbox,
  AutoComplete,
  Cascader,
  DatePicker,
  InputNumber,
  Switch,
  TimePicker,
  TreeSelect,
  Tree,
  Slider,
  Rate,
  Divider,
  Alert
} from 'ant-design-vue';
import { Tinymce } from '@/components/Tinymce/index';
import { AButton } from '@/components/basic/button/index';
import { IconFont } from '@/components/basic/icon-font/index';
const componentMap = {
  //自定义组件
  Tinymce,
  //ant组件库组件
  Input,
  InputGroup: Input.Group,
  InputPassword: Input.Password,
  InputSearch: Input.Search,
  InputTextArea: Input.TextArea,
  InputNumber,
  AutoComplete,
  Select,
  TreeSelect,
  Tree,
  Switch,
  RadioGroup: Radio.Group,
  Checkbox,
  CheckboxGroup: Checkbox.Group,
  Cascader,
  Slider,
  Rate,
  DatePicker,
  MonthPicker: DatePicker.MonthPicker,
  RangePicker: DatePicker.RangePicker,
  WeekPicker: DatePicker.WeekPicker,
  TimePicker,
  Divider,
  //html原生标签(组件)
  div: 'div',
  span: 'span',
  AButton,
  IconFont,
  Alert,
};

export type ComponentMapType = keyof typeof componentMap;

export { componentMap };
