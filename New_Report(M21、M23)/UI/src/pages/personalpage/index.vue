<template>
    <Card class="mt-3">
        <Descriptions title="账号信息" :column="2" bordered>
            <Descriptions.Item label="姓名">
                <span>{{ UserMessage.MemName }}</span>
            </Descriptions.Item>
            <Descriptions.Item label="工号">
                <span>{{ UserMessage.MemID }}</span>
            </Descriptions.Item>
            <Descriptions.Item label="Token(测试用)">
                <span>{{ UserMessage.SSOToken }}</span>
            </Descriptions.Item>
            <Descriptions.Item label="登录系统次数">
                <span>{{ ClickCount }}次</span>
            </Descriptions.Item>
            <!-- <Descriptions.Item label="权限等级">
                <Tag>admin</Tag>
            </Descriptions.Item> -->
        </Descriptions>
    </Card>
</template>

<script lang="tsx" setup>
import { ref } from '@vue/reactivity';
import { onMounted } from '@vue/runtime-core';
import { Descriptions, Card, Tag, message } from 'ant-design-vue';
import axios from 'axios';
import { Storage } from '@/utils/Storage';

const UserMessage = Storage.get('USERMESSAGE');
const data = {
    SysName: process.env.VUE_APP_SYSID,
    empno: UserMessage.MemID
};
const data1 = {
    EmpName: UserMessage.MemName,
    EmpNo: UserMessage.MemID,
    HostName: '',
    SysName: process.env.VUE_APP_SYSID,
    SysFunc: '首頁',
    SysModule: '首頁刷新'
};
let ClickCount = ref(0);
const InsertClick = async (method: string, params: Object) => {
    return await new Promise((resolve) => {
        axios({
            method: method,
            url: 'http://ngbreportapi.cminl.oa/NGBAP_Number_Of_Hits_Api/',
            //数据内容
            data: method === 'POST' ? params : null,
            params: method === 'GET' ? params : null
        })
            .then((res) => {
                resolve(res.data);
            })
            .catch((e) => {
                console.log(e);
            });
    });
};
const GetClick = async (method: string, params: Object) => {
    return await new Promise((resolve) => {
        axios({
            method: method,
            url: 'http://fsapiqportal.cminl.oa/getFSClick',
            //数据内容
            data: method === 'POST' ? params : null,
            params: method === 'GET' ? params : null
        })
            .then((res) => {
                resolve(res?.data);
            })
            .catch((e) => {
                console.log(e);
            });
    });
};
onMounted(async () => {
    let ResData = await InsertClick('POST', data1);
    //@ts-ignore
    if (ResData.RetMsg && ResData.RetMsg == 'Insert success') {
        const ClickData = await GetClick('POST', data);
        //@ts-ignore
        ClickCount.value = ClickData.data;
    } else {
        message.error('获取点击率失败');
    }
});

// const BlankLink = ({ url = '', text }) => {
//     const target = /^http(s)?:/.test(url) ? url : `https://www.npmjs.com/package/${url}`;
//     return (
//         <a href={target} target="_blank">
//             {text}
//         </a>
//     );
// };
</script>
