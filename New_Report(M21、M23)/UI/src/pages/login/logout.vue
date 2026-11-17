<template>
    <div>对不起,由于系统问题,暂时无法呈现</div>
</template>

<script setup>
import { onMounted } from "@vue/runtime-core"

onMounted(()=>{
    window.open(`http://tsamv4athe.cminl.oa/form/Logout.html?url=http://localhost:8080/`,'_self')
})
</script>