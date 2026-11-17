
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";
import { getfinalsum,getdiffdate ,getrowsum} from "@/utils/reportcommon/Calculation";



async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: TimeData }, Data1: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
      });
}
export async function StartLotDataSet(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "StartLotDataSet",
        FactoryType: "BEOL2",
        date_f: '',
        date_t: '',
        ProdTypes: "",
        ReportFormat: "",
        SelPwo: [] as any,
        Selwo: [],
        SelPROC:[],
        Owner:"",
        worder:'',
        worder_cate:"",
        Work_order:""
    }
    console.log("ALLFormParams",ALLFormParams);
    
    //給ParamsData賦值 
    let tempkeys = Object.keys(params);
    for (let i in ALLFormParams) {
        if (tempkeys.includes(i)) {
            params[i] = cloneDeep(ALLFormParams[i]);
        }
    }
   /*  if (ALLFormParams["Selwo"].includes("ALL")) {
        params["SelPwo"] = ["ALL"];
        params["Selwo"] = [];
    } else {
        params["Selwo"] = ALLFormParams["Selwo"];
    } */
    let leftLabels = ["Category", "PROC_ID","PROD", "PFCD", "Work_Order", "Owner","worder_cate"];
    let leftProps = ["CATEGORY","PROC_ID","PROD","PFCD","WORK_ORDER","OWNER_ID","WORDER_CATE"];
    await getToptableData(PageSetting, params);
    PageSetting["TopList"].forEach(item=>{
        if(item["CATEGORY"] == "STRS"){
            item["CATEGORY"] = "Cancel"
        }else{
            item["CATEGORY"] = "Start"
        }
    })
    // 設置中間的label
    let middleLabels = getdiffdate(ALLFormParams["date_f"], ALLFormParams["date_t"]);
    let middleProps:string[] = cloneDeep(middleLabels);
    // 更改樣式
    console.log(middleLabels);
    for (let index = 0; index < middleLabels.length; index++) {
        middleLabels[index] = middleLabels[index].slice(5).replace("-", "/");
    }
    let ToptableLabels = leftLabels.concat(middleLabels).concat(["Summary"]);
    let ToptableProps:string[] = leftProps.concat(middleProps).concat(["Sum"]);
    let config: PovitParams = {
        Data: PageSetting["TopList"],
        povitColumn: {
            MFGDATE: "PANEL_CNT",
        },
        changeColumn: middleProps,
        repeatColumn: leftProps,
    };
    PageSetting["TopList"] = getPovitData(config);
    // 添加行統計
    getrowsum(PageSetting["TopList"],middleProps,"Sum");
    //添加總計行
    PageSetting["TopList"].push(getfinalsum(PageSetting["TopList"],middleProps.concat(["Sum"]),leftProps[0]));
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps,ClickProps:middleProps.concat(["Sum"]) };
}