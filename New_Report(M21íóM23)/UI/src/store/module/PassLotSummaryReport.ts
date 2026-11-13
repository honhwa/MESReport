import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface PassLotSummaryReport {
    APIMessage: string;
    ALLFormParams: Object;
    
}

export const AddPassLotSummaryReport = defineStore({
    id: 'AddPassLotSummaryReport',
    state: (): PassLotSummaryReport => ({
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

export function AddPassLotSummaryReportWithOut() {
    return AddPassLotSummaryReport(store);
}