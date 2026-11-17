<template>
    <div>
        <Alert message="MasterFlow" type="info" show-icon>
            <template #description> MasterFlow簽核示例 </template>
        </Alert>
        <Card title="demo" style="margin-top: 20px" class="buttonclass">
            <img src="http://inlcnws/PublicWebservice/Images/logo/logo.png" />
            <div class="buttonmenu" v-for="item in ButtonList" :key="item.label">
                <a-button
                    :key="item.label"
                    :type="item.type"
                    class="filter-item"
                    @click="item.handle()"
                    v-if="item.isShow"
                    >{{ item.label }}
                </a-button>
            </div>
        </Card>
    </div>
</template>

<script lang="ts">
export default {
    name: 'MasterFlow'
};
</script>
<script lang="ts" setup>
import { Alert, Card } from 'ant-design-vue';
import { ButtonSetting, masterFlowSchemas } from './formschema';
import { useFormModal } from '@/hooks/useModal/useFormModal';

const [showModal] = useFormModal();

const openMenuModal = async (MasterParams: any) => {
    const [formRef] = await showModal<API.UpdateRoleParams>({
        modalProps: {
            title: `${MasterParams.id ? '编辑' : '新增'}角色`,
            width: '50%',
            onFinish: async (values) => {
                MasterParams.id && (values.roleId = MasterParams.id);
                // dynamicTableInstance?.reload();
            }
        },
        formProps: {
            labelWidth: 100,
            schemas: masterFlowSchemas
        }
    });

    // 如果是编辑的话，需要获取角色详情
    if (MasterParams.id) {
        // const data = await getRoleInfo({ roleId: record.id });
        // const menuIds = data.menus.map((n) => n.menuId);
        // const deptIds = data.depts.map((n) => n.departmentId);

        formRef?.setFieldsValue({
            ...MasterParams
            // name: data1.list[0].name,
            // label: data1.roleInfo.label,
            // remark: data1.roleInfo.remark,
            // menus: getCheckedKeys(menuIds, []),
            // depts: getCheckedKeys(deptIds, [])
        });
    }
};

const ButtonList: ButtonSetting[] = [
    {
        label: '作廢',
        icon: 'el-icon-plus',
        handle: () => {
            openMenuModal(MasterParams);
        },
        type: 'danger',
        isShow: true
    },
    {
        label: '送審',
        icon: 'el-icon-search',
        handle: () => {
            openMenuModal(1);
        },
        type: 'warning',
        isShow: true
    },
    {
        label: '加簽',
        icon: 'el-icon-search',
        handle: () => {
            openMenuModal(1);
        },
        type: 'warning',
        isShow: true
    },
    {
        label: '改簽',
        icon: 'el-icon-search',
        handle: () => {
            openMenuModal(1);
        },
        type: 'warning',
        isShow: true
    }
];
const MasterParams = {
    // ProcessName: System,
    ProcessName: 'FS_IT_CUP',
    IncidentNo: 1069,
    StepName: 'Supervisor',
    // StepName: 'Begin',
    // UserName: `CMINL/${SAMV4Params.ENAME}`,
    UserName: `CMINL/ZUYU.XU`,
    SendAction: 0,
    ParamA: [
        {
            Name: 'g_form_no',
            Value: ['2022060103']
        },
        {
            Name: 'g_proc_summary',
            Value: ''
        },
        {
            Name: 'g_assigner',
            Value: ['CMINL/XINGZHONG.XIE']
        }
    ]
};
</script>
