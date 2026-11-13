import { Popconfirm, Modal } from 'ant-design-vue';
import { cloneDeep } from 'lodash-es';
import { computed, reactive, ref ,watch} from 'vue';
import { defineComponent } from 'vue';

export let LCM1baseColumns = [
    {
        title: '機種名',
        dataIndex: 'PROD_NBR',
        key: 'PROD_NBR',
        className: 'v-center',
        
    },
    {
        title: '抽驗頻率',
        dataIndex: 'INSP_FREQUENCY',
        key: 'INSP_FREQUENCY'
    },
    {
        title: '計算別',
        dataIndex: 'CNT_FLAG',
        key: 'CNT_FLAG'
    },
    {
        title: '是否撕膜檢',
        dataIndex: 'PROD_FLAG',
        key: 'PROD_FLAG'
    }
];
export let operaction = (cancel:Function,update:Function) => {
    return {
        title: '新增',
        dataIndex: 'operation',
        align: 'center',
        customRender: ({ text, record, index, column }) => {
            return footer(text, record, index, column,cancel,update);
        }
    };
};
// 編輯按鈕
let footer = (text, record, index, column,cancel,update) => {
    return (
        <div >
            <Popconfirm title="Sure to cancel?" onConfirm={() => cancel(text, record, index, column)}>
                <a-button type="primary" size="small">
                    刪除
                </a-button>
            </Popconfirm>
            <a-button type="primary" size="small" onClick={() => update(text, record, index, column)}>
                修改
            </a-button>
        </div>
    );
};

type IProps = {
    UpdateInfo: { PROD_NBR: String; INSP_FREQUENCY: String; PROD_FLAG: String };
    visible: Boolean;
    MType: Number;
};
interface IUpdateInfo {
    PROD_NBR?: String | undefined;
    INSP_FREQUENCY?: String | undefined;
    PROD_FLAG?: String | undefined;
}
// 彈出框
export const customdialog = defineComponent({
    model:{

    },
    props: {
        UpdateInfo: { type: Object as PropType<IProps['UpdateInfo']> },
        visible: { type: Boolean, default: false },
        MType: { type: Number },
        FactoryType:String
    },
    emits: ['closeDialog','update:updata','update:addnew'],
    setup(props, ctx) {
        const { slots, emit } = ctx;
        // 確認按鈕
        function handleOk() {
            // 通知父組件
            // 新增
                emit("update:addnew",props.MType)
        }
        // 取消按鈕
        function handleCancel() {
            // 通知父組件
            emit("closeDialog",false)
        }
        const change = (e: Event,type:string) => {
            const target = e.target as HTMLInputElement
            emit("update:updata",type,target.value)
          }
        return () => (
            <a-modal
                visible={props.visible}
                title={props.MType == 1 ? '新增' : '修改'}
                onOk={() => handleOk()}
                onCancel={() => handleCancel()}
                okText={props.MType == 1 ? '新增' : '修改'}
            >
                <div class="container">
                    <a-form
                        model={props.UpdateInfo}
                        name="basic"
                        label-col={{ span: 8 }}
                        wrapper-col={{ span: 16 }}
                        autocomplete="off"
                    >
                        <a-form-item label="機種">
                            <a-input
                                value={props.UpdateInfo?.PROD_NBR}
                                bordered
                                allowClear
                                onInput={(e)=>change(e,"PROD_NBR")}
                                disabled={props.MType == 2 ? true : false}
                            ></a-input>
                        </a-form-item>
                        <a-form-item label="抽檢頻率">
                            <a-input value={props.UpdateInfo?.INSP_FREQUENCY} bordered allowClear onInput={(e)=>change(e,"INSP_FREQUENCY")}></a-input>
                        </a-form-item>
                        {
                            props.FactoryType == "LCM1" && <a-form-item label="是否撕膜檢">
                            <a-input value={props.UpdateInfo?.PROD_FLAG} bordered allowClear onInput={(e)=>change(e,"PROD_FLAG")} maxlength={1} showCount></a-input>
                        </a-form-item>
                        }
                        
                    </a-form>
                </div>
            </a-modal>
        );
    }
});

export let LCM2baseColumns = [
    {
        title: '機種名',
        dataIndex: 'PROD_NBR',
        key: 'PROD_NBR',
        className: 'v-center'
    },
    {
        title: '抽驗頻率',
        dataIndex: 'INSP_FREQUENCY',
        key: 'INSP_FREQUENCY'
    },
    {
        title: '計算別',
        dataIndex: 'CNT_FLAG',
        key: 'CNT_FLAG'
    }
];
