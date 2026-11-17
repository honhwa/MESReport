<template>
  <SigleSubject :params="params" />
</template>

<script setup>
import { Subject, SubjectName, Answer } from "./index";
import SigleSubject from "@/components/Subject/SigleSubject.vue";

const params = { Subject, SubjectName, Answer };
</script>
