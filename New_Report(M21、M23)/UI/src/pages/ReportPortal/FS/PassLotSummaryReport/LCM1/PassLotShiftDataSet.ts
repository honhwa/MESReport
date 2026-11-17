
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";

function changeValue(dataSource:any[],key:string,preValue:string,newValue:string) {
    if(dataSource.length!=0){
        dataSource.forEach(item=>{
            if(item[key] == preValue){
                item[key] = newValue;
            }
        })
    }
}
async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: TimeData }, Data1: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
    });
    
}
export async function PassLotShiftDataSet(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "PassLotShiftDataSet",
        FactoryType: "BEOL1",
        rpt: "A",
        date_f: '',
        date_t: '',
        Groups: [],
        ProdTypes: "",
        Owner: "",
        SelPROC: [],
    }
    //給ParamsData賦值 
    let tempkeys = Object.keys(params);
    for (let i in ALLFormParams) {
        if (tempkeys.includes(i)) {
            params[i] = cloneDeep(ALLFormParams[i]);
        }
    }

    // 顯示的label根據group by 和 Report Type 判斷
    

    /* 情況1：
         Report Type（rpt）：ALL
                label1:["Total(日)","Total(日)","Total(夜)","Total(夜)","Total(日+夜)","Total(日+夜)"]
                label2:["Move in","Move out","Move in","Move out","Move in","Move out"]
                props:["PnlQty","PnlQty","PnlQty","PnlQty","PnlQty","PnlQty","PnlQty","PnlQty"]
        Report Type：LOGN
                label1:["Total(日)","Total(夜)","Total(日+夜)"]
                label2:["Move in","Move in","Move in"]
                props:["PnlQty","PnlQty","PnlQty"]
        Report Type：LOGF
                label1:["Total(日)","Total(日)","Total(夜)","Total(日+夜)"]
                label2:["Move out","Move out","Move out"]
                props:["PnlQty","PnlQty","PnlQty"]
     
    情況2：
        選中group by（Groups）中的多少項，顯示多少個label
          */
    // 左邊lebel和props
    let leftLabels: string[] = [];
    let leftProps: string[] = ALLFormParams["Groups"];
    let GroupsOptions = [
        {
            label: 'Operation',
            value: 'PROC_ID'
        },
        {
            label: 'Entity',
            value: 'EQPT_ID'
        },
        {
            label: 'PFCD',
            value: 'PFCD'
        },
        {
            label: 'Owner',
            value: 'OWNER_ID'
        },
        {
            label: 'Grade',
            value: 'GRADE'
        },
        {
            label: 'Work Order',
            value: 'WORK_ORDER'
        }, {
            label: 'User ID',
            value: 'CLM_USER'
        }
    ]
    if (ALLFormParams["Groups"].length != 0) {
        ALLFormParams["Groups"].forEach(item => {
            GroupsOptions.forEach(Gitem => {
                if (item == Gitem["value"]) {
                    leftLabels.push(Gitem["label"])
                }
            })
        })
    } else {
        leftLabels = ["Operation"];
        leftProps = ["PROC_ID"]

    }

    await getToptableData(PageSetting, params);
    // 將CHON替換為LOGN,將CHOF替換為LOGF
    changeValue(PageSetting["TopList"],"CATEGORY","CHON","LOGN");
    changeValue(PageSetting["TopList"],"CATEGORY","CHOF","LOGF");
    // 中間的label和props
    let middleLabels: string[] = [];
    let middleProps: string[] = [];
    // 後面的label和props
    let rightLabels: string[] = [];
    let rightProps: string[] = [];
    switch (ALLFormParams["rpt"]) {
        /* 
        movein 的情況 LOGN CHON
        moveout 的情況 LOGF CHOF
        */
        case "ALL":
            PageSetting["TimeData"].forEach(item => {
                middleProps.push(`${item} 1 LOGN`);
                middleLabels.push(`${item}(日)movein`);
                middleProps.push(`${item} 1 LOGF`);
                middleLabels.push(`${item}(日)moveout`);
                middleProps.push(`${item} 2 LOGN`);
                middleLabels.push(`${item}(夜)movein`);
                middleProps.push(`${item} 2 LOGF`);
                middleLabels.push(`${item}(夜)moveout`);
            })
            rightProps = ["total(日)movein", "total(日)moveout", "total(夜)movein", "total(夜)moveout", "total(日+夜)movein", "total(日+夜)moveout"]
            rightLabels = ["total(日)movein", "total(日)moveout", "total(夜)movein", "total(夜)moveout", "total(日+夜)movein", "total(日+夜)moveout"]

            break;
        case "LOGN":
            PageSetting["TimeData"].forEach(item => {
                middleLabels.push(`${item}(日)movein`);
                middleProps.push(`${item} 1 LOGN`);
                middleLabels.push(`${item}(夜)movein`);
                middleProps.push(`${item} 2 LOGN`);
            })
            rightProps = ["total(日)movein", "total(夜)movein", "total(日+夜)movein"]
            rightLabels = ["total(日)movein", "total(夜)movein", "total(日+夜)movein"]

            break;
        case "LOGF":
            PageSetting["TimeData"].forEach(item => {
                middleLabels.push(`${item}(日)moveout`);
                middleProps.push(`${item} 1 LOGF`);
                middleLabels.push(`${item}(夜)moveout`);
                middleProps.push(`${item} 2 LOGF`);
            })
            rightProps = ["total(日)moveout", "total(夜)moveout", "total(日+夜)moveout"]
            rightLabels = ["total(日)moveout", "total(夜)moveout", "total(日+夜)moveout"]
            break;
        default:
            break;
    }

    let config: PovitParams = {
        Data: PageSetting["TopList"],
        povitColumn: {
            'MFGDATE SHIFT CATEGORY': 'PANEL_CNT'
        },
        changeColumn: middleProps,
        repeatColumn: leftProps,
    };
    PageSetting["TopList"] = getPovitData(config);
    let ToptableLabels: string[] = [];
    ToptableLabels = leftLabels
        .concat(middleLabels)
        .concat(rightLabels);
    let ToptableProps: string[] = [];
    ToptableProps = leftProps
        .concat(middleProps)
        .concat(rightProps);
    switch (ALLFormParams["rpt"]) {
        case "ALL":
            PageSetting["TopList"].forEach(item => {
                item["total(日)movein"] = 0;
                item["total(日)moveout"] = 0;
                item["total(夜)movein"] = 0;
                item["total(夜)moveout"] = 0;
                item["total(日+夜)movein"] = 0;
                item["total(日+夜)moveout"] = 0;
                for (let index = 0; index < middleProps.length; index++) {
                    if (!isNaN(item[middleProps[index]])) {
                        if (middleProps[index].indexOf("1 LOGF") != -1) {
                            item["total(日)movein"] += item[middleProps[index]];
                        } else if (middleProps[index].indexOf("1 LOGN") != -1) {
                            item["total(日)moveout"] += item[middleProps[index]];
                        }
                        else if (middleProps[index].indexOf("2 LOGF") != -1) {
                            item["total(夜)movein"] += item[middleProps[index]];
                        }
                        else if (middleProps[index].indexOf("2 LOGN") != -1) {
                            item["total(夜)moveout"] += item[middleProps[index]];
                        }
                        item["total(日+夜)movein"] = item["total(日)movein"] + item["total(夜)movein"];
                        item["total(日+夜)moveout"] = item["total(日)moveout"] + item["total(夜)moveout"];


                    }
                }
            })
            break;
        case "LOGN":
            PageSetting["TopList"].forEach(item => {
                item["total(日)movein"] = 0;
                item["total(夜)movein"] = 0;
                item["total(日+夜)movein"] = 0;
                for (let index = 0; index < middleProps.length; index++) {
                    if (!isNaN(item[middleProps[index]])) {
                        if (middleProps[index].indexOf("1 LOGN") != -1) {
                            item["total(日)movein"] += item[middleProps[index]];
                        }
                        else if (middleProps[index].indexOf("2 LOGN") != -1) {
                            item["total(夜)movein"] += item[middleProps[index]];
                        }
                        item["total(日+夜)movein"] = item["total(日)movein"] + item["total(夜)movein"];


                    }
                }
            })
            break;
        case "LOGF":
            PageSetting["TopList"].forEach(item => {
                item["total(日)movein"] = 0;
                item["total(日)moveout"] = 0;
                item["total(夜)movein"] = 0;
                item["total(夜)moveout"] = 0;
                item["total(日+夜)movein"] = 0;
                item["total(日+夜)moveout"] = 0;
                for (let index = 0; index < middleProps.length; index++) {
                    if (!isNaN(item[middleProps[index]])) {
                          if (middleProps[index].indexOf("1 LOGF") != -1) {
                            item["total(日)moveout"] += item[middleProps[index]];
                        }
                        else if (middleProps[index].indexOf("2 LOGF") != -1) {
                            item["total(夜)moveout"] += item[middleProps[index]];
                        }
                        item["total(日+夜)moveout"] = item["total(日)moveout"] + item["total(夜)moveout"];


                    }
                }
            })
            break;
        default:
            break;
    }


    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps, middleProps: middleProps };

}