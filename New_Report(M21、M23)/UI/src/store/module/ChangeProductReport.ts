import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface StartSummaryReport {
    ALLFormParams: Object;
}

export const ChangeProductReport = defineStore({
    id: 'ChangeProductReport',
    state: (): StartSummaryReport => ({
        ALLFormParams: {},
    }),
    actions: {
        AddALLFormParams(Params: Object) {
            this.ALLFormParams = Params
        },
        
    },
})

export function ChangeProductReportWithOut() {
    return ChangeProductReport(store);
}