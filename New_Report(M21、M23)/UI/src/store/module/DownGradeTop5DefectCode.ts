import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface StartSummaryReport {
    ALLFormParams: Object;
}

export const DownGradeTop5DefectCode = defineStore({
    id: 'DownGradeTop5DefectCode',
    state: (): StartSummaryReport => ({
        ALLFormParams: {},
    }),
    actions: {
        AddALLFormParams(Params: Object) {
            this.ALLFormParams = Params
        },
        
    },
})

export function DownGradeTop5DefectCodeWithOut() {
    return DownGradeTop5DefectCode(store);
}