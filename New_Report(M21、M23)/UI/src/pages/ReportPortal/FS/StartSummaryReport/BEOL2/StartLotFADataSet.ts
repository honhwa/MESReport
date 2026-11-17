
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";
import { getfinalsum, getdiffdate, Inserttotalrow } from "@/utils/reportcommon/Calculation";



async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
}
export async function StartLotFADataSet(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "StartLotFADataSet",
        FactoryType: "BEOL2",
        date_f: '',
        date_t: '',
        ProdTypes: "",
        ReportFormat: "",
        SelPwo: [] as any,
        Selwo: [],
        worder:'',
        Work_order:""
    }
    //給ParamsData賦值 
    let tempkeys = Object.keys(params);
    for (let i in ALLFormParams) {
        if (tempkeys.includes(i)) {
            params[i] = cloneDeep(ALLFormParams[i]);
        }
    }
    /* if (ALLFormParams["Selwo"].includes("ALL")) {
        params["SelPwo"] = ["ALL"];
        params["Selwo"] = [];
    } else {
        params["Selwo"] = ALLFormParams["Selwo"];
    } */
    let ToptableLabels = ["Src_shop", "category","clm_mfdt", "pfcd", "owner_id", "Gls Qty", "Pnl Qty"];
    let ToptableProps = ["SRC_SHOP","CATEGORY","CLM_MFDT","PFCD","OWNER_ID","SHEET_CNT","PANEL_CNT"];
    console.log(PageSetting);
    await getToptableData(PageSetting, params);
    let totalsum = getfinalsum(PageSetting["TopList"], ["SHEET_CNT", "PANEL_CNT"], "SRC_SHOP");
    // 插入行
    let { insertIndexList, insertrowList } = Inserttotalrow(PageSetting["TopList"], ["SRC_SHOP","CATEGORY"], ["SHEET_CNT", "PANEL_CNT"], "CATEGORY");
    for (let i = 0; i < insertIndexList.length; i++) {
        PageSetting["TopList"].splice(insertIndexList[i] + i, 0, insertrowList[i]);
    }
    PageSetting["TopList"].push(totalsum)
    
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps  };
}