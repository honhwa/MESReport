import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
interface OperationPerformance {
    [key:string]:any;
    APIMessage: Object;
    FormParams: Object;
    FirstTableParams: Object;
    SecondTableParams: Object;
    ThirdTableParams: Object;
    ThirdTableClickParams: Object;
}

export const AddOperationParams = defineStore({
    id: 'AddOperationParams',
    state: (): OperationPerformance => ({
        // 首頁後端返回的sql以及sql參數
        APIMessage:{},
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
    }),
    actions: {
        AddAPIResultMessage(Params: Object){
            this.APIMessage = Params
        },
        AddFormParams(Params: Object) {
            this.FormParams = Params;
        },
        AddFirstTableParams(Params: Object) {
            this.FirstTableParams = Params
        },
        AddSecondTableParams(Params: Object){//这里的first指的是第几层而不是第几个
            this.SecondTableParams = Params
        },
        AddThirdTableParams(Params: Object) {
            this.ThirdTableParams = Params
        },
        AddThirdTableClickParams(Params: Object) {
            this.ThirdTableClickParams = Params
        },
    },
})

export function AddOperationParamsWithOut() {
    return AddOperationParams(store);
}