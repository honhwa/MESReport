import { FileItem, PreviewFileItem, UploadResultStatus } from './typing';
import {
  // checkImgType,
  isImgTypeByName,
} from '../helper';
import { Progress, Tag } from 'ant-design-vue';
// import TableAction from '@/components/core/dynamic-table/src/components/table-action.vue';
import ThumbUrl from './ThumbUrl.vue';


// 文件上传列表
export function createTableColumns(handleRemove): any[] {
  return [
    {
      dataIndex: 'thumbUrl',
      title: '缩略图',
      width: 180,
      customRender: ({ record }) => {
        const { thumbUrl } = (record as FileItem) || {};
        return thumbUrl && <ThumbUrl fileUrl={thumbUrl} />;
      },
    },
    {
      dataIndex: 'name',
      title: '文件名',
      align: 'left',
      customRender: ({ text, record }) => {
        const { percent, status: uploadStatus } = (record as FileItem) || {};
        let status: 'normal' | 'exception' | 'active' | 'success' = 'normal';
        if (uploadStatus === UploadResultStatus.ERROR) {
          status = 'exception';
        } else if (uploadStatus === UploadResultStatus.UPLOADING) {
          status = 'active';
        } else if (uploadStatus === UploadResultStatus.SUCCESS) {
          status = 'success';
        }
        return (
          <span>
            <p class="truncate mb-1" title={text}>
              {text}
            </p>
            <Progress percent={percent} size="small" status={status} />
          </span>
        );
      },
    },
    {
      dataIndex: 'size',
      title: '文件大小',
      width: 180,
      customRender: ({ text = 0 }) => {
        return text && (text / 1024).toFixed(2) + 'KB';
      },
    },
    {
      dataIndex: 'status',
      title: '状态',
      width: 100,
      customRender: ({ text }) => {
        if (text === UploadResultStatus.SUCCESS) {
          return <Tag color="green">{() => '上传成功'}</Tag>;
        } else if (text === UploadResultStatus.ERROR) {
          return <Tag color="red">{() => '上传失败'}</Tag>;
        } else if (text === UploadResultStatus.UPLOADING) {
          return <Tag color="blue">{() => '上传中'}</Tag>;
        }

        return text;
      },
    },
    {
      title: '操作',
      width: 160,
      dataIndex: '$action',
      hideInSearch: true,
      align: 'center',
      fixed: 'right',
      actions: ({ record }) => [
        {
          label: '删除',
          comtype: 'a-button',
          auth: {
            perm: 'sys.role.update',
            effect: 'disable'
          },
          onClick: () => handleRemove(record)
        }
      ]
    }
  ];
}

// 文件预览列表
export function createPreviewColumns({handleRemove,handleDownload}: {
    handleRemove: Fn;
    handleDownload: Fn;
  }) {
  return [
   
    {
      dataIndex: 'name',
      title: '文件名',
      align: 'left',
    },
    {
      dataIndex: 'url',
      title: '文件路径',
      width:200,
      maxwidth:800,
      minwidth:200,
      resizable:true
    },
    {
      title: '操作',
      dataIndex: '$action',
      hideInSearch: true,
      align: 'center',
      fixed: 'right',
      actions: ({ record, index }) => [

        {
            label: '下载',
            comtype: 'a-button',
            onClick: () => handleDownload(record)
        },
        {
            label: '删除',
            comtype: 'a-button',
            popConfirm: {
                title: '你确定要删除吗？',
                onConfirm: () => handleRemove(record)
            }
        }
    ]
      // customRender: ({ record }) => {
      //   return <div>
      //     <a href={record.url}>下载</a>
      //     <a-button>刪除</a-button>
      //   </div>
      // },
      //       customRender: ({ record }) => {
      //   const actions: any[] = [
      //     {
      //       label: '删除',
      //       color: 'error',
      //       onClick: handleRemove.bind(null, record),
      //     },
      //     {
      //       label: '下载',
      //       onClick: handleDownload.bind(null, record),
      //     },
      //   ];

      //   return <TableAction actions={actions} outside={true} />;
      // },
    }
  ];
}

// export function createPreviewActionColumn({
//   handleRemove,
//   handleDownload,
// }: {
//   handleRemove: Fn;
//   handleDownload: Fn;
// }): any {
//   return {
//     width: 160,
//     title: '操作',
//     dataIndex: 'action',
//     fixed: false,
//     customRender: ({ record }) => {
//       const actions: any[] = [
//         {
//           label: '删除',
//           color: 'error',
//           onClick: handleRemove.bind(null, record),
//         },
//         {
//           label: '下载',
//           onClick: handleDownload.bind(null, record),
//         },
//       ];

//       // return <TableAction actions={actions} outside={true} />;
//       return actions
//     },
//   };
// }
