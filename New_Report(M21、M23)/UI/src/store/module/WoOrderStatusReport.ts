import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface StartSummaryReport {
    ALLFormParams: Object;
}

export const WoOrderStatusReport = defineStore({
    id: 'WoOrderStatusReport',
    state: (): StartSummaryReport => ({
        ALLFormParams: {},
    }),
    actions: {
        AddALLFormParams(Params: Object) {
            this.ALLFormParams = Params
        },
        
    },
})

export function WoOrderStatusReportWithOut() {
    return WoOrderStatusReport(store);
}