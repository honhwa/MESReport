import { defineStore } from 'pinia';
import { store } from '@/store';
import { isReactive, reactive } from 'vue';
interface ICommonPageParams {
    APIMessage: string;
    FormParams: Object;
    FirstTableParams: Object;
    SecondTableParams: Object;
    ThirdTableParams: Object;
    ThirdTableClickParams: Object;
    Fab: string;
    G_FAC_COM: string;
}
interface CommonPageParams {
    Fab: string;
    G_FAC_COM: string;
    SubFab:string;
    G_CLASS:string;
    Scheduling:Object;
    // 報表共用類型
    [key: number]: ICommonPageParams;
    
}
// 不可以直接創建動態二階對象
function createobj(this:any,report:string,attr:string,Params:Object) {
    Object.keys(Params).forEach(key=>{
        if(!Params[key]){
            Params[key] = ''
        }
    })
    if(!this[report]){
        this[report] = reactive({});
        this[report][attr] = Params
    }else{
        this[report][attr] = {...this[report][attr],...Params};
    }
}
export const ReportCommon = defineStore({
    id: 'ReportCommon',
    state: (): CommonPageParams => ({
        //廠別
        Fab: '',
        G_FAC_COM: '',
        SubFab:'',
        G_CLASS:'',
        Scheduling:{},
    }),
    actions: {
        /**
         * 添加整個ICommonPageParams類型對象
         * @param report 報表的名字
         * @param Params 參數
         */
        AddObject(report: string, Params: ICommonPageParams) {
            this[report] = reactive(Params);
        },
        AddAPIResultMessage(report: string, Params: string) {
            // // this[report]['APIMessage'] = Params;
            if(!this[report]){
                this[report] = reactive({});
                this[report]["APIMessage"] = Params
            }else{
                this[report]["APIMessage"] = Params;
            }
        },
        AddFormParams(report: string, Params: Object) {
            createobj.call(this,report,"FormParams",Params);
            
        },
        AddFirstTableParams(report: string, Params: Object) {
            console.log("111111111111111111",Params);
            createobj.call(this,report,"FirstTableParams",Params);
        },
        AddSecondTableParams(report: string, Params: Object) {
            createobj.call(this,report,"SecondTableParams",Params);
        },
        AddThirdTableParams(report: string, Params: Object) {
            createobj.call(this,report,"ThirdTableParams",Params);
        },
        AddThirdTableClickParams(report: string, Params: Object) {
            createobj.call(this,report,"ThirdTableClickParams",Params);
            
        },
        AddFabType(FabType: string, G_FAC: string,SubFab:string,G_CLASS:string) {
            this['Fab'] = FabType;
            this['G_FAC_COM'] = G_FAC;
            this['SubFab'] = SubFab;
            this["G_CLASS"] = G_CLASS
        },
        AddScheduling(Scheduling:Object){
            console.log("更新之前的Scheduling",this["Scheduling"]);
            this["Scheduling"] = Scheduling;
            console.log("更新之后的Scheduling",this["Scheduling"]);
            console.log("isReactive:",isReactive(this["Scheduling"]));
            
            
        }
    }
});

export function ReportCommonWithOut() {
    return ReportCommon(store);
}
