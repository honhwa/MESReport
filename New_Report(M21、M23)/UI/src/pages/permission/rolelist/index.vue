<template>
    <Tabs v-model:activeKey="activeKey">
        <Tabs.TabPane key="1" tab="角色成员管理">
            <UserManage />
        </Tabs.TabPane>
        <Tabs.TabPane key="2" tab="角色功能维护" force-render>
            <RoleFun />
        </Tabs.TabPane>
        <Tabs.TabPane key="3" tab="角色管理" force-render>
            <RoleManage />
        </Tabs.TabPane>
    </Tabs>
</template>

<script setup lang="tsx">
import { ref } from 'vue';
import UserManage from './components/roleuser/index.vue';
import RoleManage from './components/rolemanage/index.vue';
import RoleFun from './components/rolefun/index.vue';
import { Tabs } from 'ant-design-vue';

let activeKey = ref('1');
</script>

<style></style>
