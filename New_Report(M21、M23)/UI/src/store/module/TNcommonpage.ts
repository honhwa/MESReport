import { defineStore } from 'pinia';
import { store } from '@/store';
import { reactive } from 'vue';
import { RouteRecordRaw } from 'vue-router';
interface CommonPageParams {
    APIMessage: string;
    FormParams: Object;
    FirstTableParams: Object;
    SecondTableParams: Object;
    ThirdTableParams: Object;
    ThirdTableClickParams: Object;
    Fab:string;
    G_FAC_COM:string;

    PanelWIPStayFormParams: Object;
    StartBankStatusFormParams: Object;
    PanelWIPStayAPIMessage: string;
    StartBankStatusAPIMessage: string;
    PanelWIPStayFirstTableParams: Object;
    StartBankStatusFirstTableParams: Object;
    
}

export const AddCommonPageParams = defineStore({
    id: 'AddCommonPageParams',
    state: (): CommonPageParams => ({
        // 首頁後端返回的sql以及sql參數
        APIMessage:'',
        // 左側請求參數
        FormParams: {},
        // 二級頁面所需參數
        FirstTableParams: {},
        // 其餘頁面所需參數
        SecondTableParams:reactive({}),
        ThirdTableParams:{},
        ThirdTableClickParams:{},
        //廠別
        Fab:'',
        G_FAC_COM:'',

        PanelWIPStayFormParams:{},
        StartBankStatusFormParams : {},
        PanelWIPStayAPIMessage : '',
        StartBankStatusAPIMessage : '',
        PanelWIPStayFirstTableParams : {},
        StartBankStatusFirstTableParams : {},
    }),
    actions: {
        AddAPIResultMessage(Params: string){
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
        AddFabType(FabType:string,G_FAC:string){
            this.Fab = FabType
            this.G_FAC_COM = G_FAC
        },

        AddPanelWIPStayFormParams(Params: Object) {
            this.PanelWIPStayFormParams = Params;
        },
        AddStartBankStatusFormParams(Params: Object) {
            this.StartBankStatusFormParams = Params;
        },
        AddPanelWIPStayAPIResultMessage(Params: string){
            this.PanelWIPStayAPIMessage = Params
        },
        AddStartBankStatusAPIResultMessage(Params: string){
            this.StartBankStatusAPIMessage = Params
        },
        AddPanelWIPStayFirstTableParams(Params: Object) {
            this.PanelWIPStayFirstTableParams = Params
        },
        AddStartBankStatusFirstTableParams(Params: Object) {
            this.StartBankStatusFirstTableParams = Params
        },
        
    },
})

export function AddCommonPageParamsWithOut() {
    return AddCommonPageParams(store);
}