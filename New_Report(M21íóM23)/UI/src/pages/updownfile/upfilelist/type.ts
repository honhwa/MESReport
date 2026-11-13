export type InnoDriveType = {
  name?: string;
  parentId: string;
  overwrite: string;
  files: File;
}

export const basicProps = {
  visible: {
    type: Boolean as PropType<boolean>,
    default: false
  },
  helpText: {
    type: String as PropType<string>,
    default: '',
  },
  // 文件最大多少MB
  maxSize: {
    type: Number as PropType<number>,
    default: Infinity,
  },
  // 最大数量的文件，Infinity不限制
  maxNumber: {
    type: Number as PropType<number>,
    default: Infinity,
  },
  // 根据后缀，或者其他
  accept: {
    type: Array as PropType<string[]>,
    default: () => [],
  },
  multiple: {
    type: Boolean as PropType<boolean>,
    default: true,
  },
  uploadParams: {
    type: Object as PropType<any>,
  },
  api: {
    type: Function as PropType<PromiseFn>,
    default: null,
    // required: true,
  },
  name: {
    type: String as PropType<string>,
    default: 'file',
  },
  filename: {
    type: String as PropType<string>,
    default: null,
  },
  exampfilename:{
    type: String as PropType<string>,
    default: "",
  },
  examplefileurl:{
    type: String as PropType<string>,
    default: "",
  }
};