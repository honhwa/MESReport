import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface StartSummaryReport {
    APIMessage: string;
    ALLFormParams: Object;
    
}

export const AddStartSummaryReport = defineStore({
    id: 'AddStartSummaryReport',
    state: (): StartSummaryReport => ({
        // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        ALLFormParams: {},
    }),
    actions: {
        AddAPIResultMessage(Params: string){
            this.APIMessage = Params
        },
       
        AddALLFormParams(Params: Object) {
            this.ALLFormParams = Params
        },
        
    },
})

export function AddStartSummaryReportWithOut() {
    return AddStartSummaryReport(store);
}