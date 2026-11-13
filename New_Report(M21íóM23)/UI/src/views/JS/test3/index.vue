<template>
    <div>
        <AllSub :params="params" />
    </div>
</template>

<script setup>
import { Subject, SubjectName, Answer, DesCription } from './index';
import AllSub from '@/components/Subject/AllSubject.vue';


const params = { Subject, SubjectName, Answer, DesCription };
</script>
