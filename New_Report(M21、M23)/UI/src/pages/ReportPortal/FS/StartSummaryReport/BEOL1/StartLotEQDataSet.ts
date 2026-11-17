
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";
import { getfinalsum, getdiffdate, Inserttotalrow } from "@/utils/reportcommon/Calculation";


async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;

}
export async function StartLotEQDataSet(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "StartLotEQDataSet",
        FactoryType: "BEOL1",
        date_f: '',
        date_t: '',
        ProdTypes: "",
        ReportFormat: "",
        SelPwo: [] as any,
        Selwo: [],
    }
    //給ParamsData賦值 
    let tempkeys = Object.keys(params);
    for (let i in ALLFormParams) {
        if (tempkeys.includes(i)) {
            params[i] = cloneDeep(ALLFormParams[i]);
        }
    }
    if (ALLFormParams["Selwo"].includes("ALL")) {
        params["SelPwo"] = ["ALL"];
        params["Selwo"] = [];
    } else {
        params["Selwo"] = ALLFormParams["Selwo"];
    }
    let Labels = ["EQP ID", "Category", "Date", "Time", "Group ID", "PFCD", "Owner", "Gls Qty", "Pnl Qty", "Carrier"];
    let Props = ["EQPT_ID", "CATEGORY", "CLM_DATE", "CLM_TIME", "GROUP_ID", "PFCD", "OWNER_ID", "SHEET_CNT", "PANEL_CNT", "CASET_ID"]
    await getToptableData(PageSetting, params);
    // for (let index = 0; index < PageSetting["TopList"].length; index++) {
    //     PageSetting["TopList"][index]["GROUP_ID"] =  PageSetting["TopList"][index]["GROUP_ID"].replace(/\s*/g,"").substring(PageSetting["TopList"][index]["GROUP_ID"].length-4)

    // }
    PageSetting["TopList"].forEach(item => {
        item["PRE_GROUP_ID"] = item["GROUP_ID"];
        let tempitem = item["GROUP_ID"].replace(/\s*/g, "");
        item["GROUP_ID"] = tempitem.substring(tempitem.length - 4)
    })
    console.log(PageSetting);
    let totalsum = getfinalsum(PageSetting["TopList"], ["SHEET_CNT", "PANEL_CNT"], "EQPT_ID");
    // 插入行
    let { insertIndexList, insertrowList } = Inserttotalrow(PageSetting["TopList"], ["EQPT_ID"], ["SHEET_CNT", "PANEL_CNT"], "CATEGORY");
    for (let i = 0; i < insertIndexList.length; i++) {
        PageSetting["TopList"].splice(insertIndexList[i] + i, 0, insertrowList[i]);
    }
    PageSetting["TopList"].push(totalsum)
    return { ToptableLabels: Labels, ToptableProps: Props, EQcliCkProps: Props };
}