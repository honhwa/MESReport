import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface StartSummaryReport {
    ALLFormParams: Object;
}

export const MTLConsume = defineStore({
    id: 'MTLConsume',
    state: (): StartSummaryReport => ({
        ALLFormParams: {},
    }),
    actions: {
        AddALLFormParams(Params: Object) {
            this.ALLFormParams = Params
        },
        
    },
})

export function MTLConsumeWithOut() {
    return MTLConsume(store);
}