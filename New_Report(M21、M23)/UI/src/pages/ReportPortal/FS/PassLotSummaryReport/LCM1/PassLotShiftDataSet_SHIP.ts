
import { GetCommon } from '@/api/pages/common';
import { getPovitData, PovitParams } from "./provit";
import { cloneDeep } from "lodash-es";


async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: TimeData }, Data1: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
    });
}
export async function PassLotShiftDataSet_SHIP(PageSetting: Object, ALLFormParams: Object) {
    ALLFormParams = cloneDeep(ALLFormParams);
    for (let index = 0; index < ALLFormParams["Groups"].length; index++) {
        if (ALLFormParams["Groups"][index] == "EQPT_ID") {
            ALLFormParams["Groups"].splice(index, 1);
        } else if (ALLFormParams["Groups"][index] == "PROC_ID") {
            ALLFormParams["Groups"][index] = "CR_PROC_ID"
        }

    }
    let params = {
        name: "Date",
        apiName: "PassLotShiftDataSet_SHIP",
        FactoryType: "BEOL1",
        date_f: ALLFormParams["date_f"],
        date_t: ALLFormParams["date_t"],
        Groups: ALLFormParams["Groups"],
        ProdTypes: ALLFormParams["ProdTypes"],
        Owner: ALLFormParams["Owner"],
        SelPwo: [] as any,
        Selwo: [],
    }
    if (ALLFormParams["Selwo"].includes("ALL")) {
        params["SelPwo"] = ["ALL"];
        params["Selwo"] = [];
    } else {
        params["Selwo"] = ALLFormParams["Selwo"];
    }
    let leftLabels: string[] = [];
    let leftProps: string[] = [];
    // 如果什麽都不選
    if (ALLFormParams["Groups"].length != 0) {
        // 如果同時不存在
        if (!ALLFormParams["Groups"].includes("PFCD") && !ALLFormParams["Groups"].includes("GRADE")) {
            leftLabels = ["Category", "PFCD", "GRADE"].concat(ALLFormParams["Groups"]);
            leftProps = ["CATE", "PFCD", "GRADE"].concat(ALLFormParams["Groups"]);
        }
        // 如果選中了PFCD,沒選中GRADE
        if (ALLFormParams["Groups"].includes("PFCD") && !ALLFormParams["Groups"].includes("GRADE")) {
            leftLabels = ["Category", "GRADE"].concat(ALLFormParams["Groups"]);
            leftProps = ["CATE",  "GRADE"].concat(ALLFormParams["Groups"]);
        }

        // 如果選中了GRADE,沒選中PFCD
        if (!ALLFormParams["Groups"].includes("PFCD") && ALLFormParams["Groups"].includes("GRADE")) {
            leftLabels = ["Category", "PFCD"].concat(ALLFormParams["Groups"]);
            leftProps = ["CATE",  "PFCD"].concat(ALLFormParams["Groups"]);
        }
        // 如果都選中了
        if (ALLFormParams["Groups"].includes("PFCD") && ALLFormParams["Groups"].includes("GRADE")) {
            leftLabels = ["Category"].concat(ALLFormParams["Groups"]);
            leftProps = ["CATE"].concat(ALLFormParams["Groups"]);
        }

    } else {
        leftLabels = ["Category", "PFCD", "GRADE"].concat(ALLFormParams["Groups"]);
        leftProps = ["CATE", "PFCD", "GRADE"].concat(ALLFormParams["Groups"]);
    }
    let tempgroups:any[] = [];
     cloneDeep(leftProps).forEach(item=>{
        if(item != "CATE"){
            tempgroups.push(item);
        }
    });
    params["Groups"] = tempgroups;

   
  

    await getToptableData(PageSetting, params);
    let middleLabels: string[] = [];
    let middleProps: any[] = [];
    for (let i = 0; i < PageSetting["TimeData"].length; i++) {
        let temp = PageSetting["TimeData"][i].slice(5).replace("-", "/");
        middleLabels.push(temp);
    }
    middleProps = PageSetting["TimeData"];
    let rightLabels = ["Sum"];
    let rightProps = ["Sum"];
    let config: PovitParams = {
        Data: PageSetting["TopList"],
        povitColumn: {
            MFGDATE: "PANEL_CNT",
        },
        changeColumn: PageSetting["TimeData"],
        repeatColumn: leftProps,
    };
    PageSetting["TopList"] = getPovitData(config);
    PageSetting["TopList"].forEach((item) => {
        item["Sum"] = 0;
        for (let i = 0; i < middleProps.length; i++) {
            if (isNaN(item[middleProps[i]])) {
                item[middleProps[i]] = 0;
            }
            item["Sum"] += item[middleProps[i]];
        }
    });
    let ToptableLabels: string[] = leftLabels
        .concat(middleLabels)
        .concat(rightLabels);
    let ToptableProps: string[] = leftProps
        .concat(middleProps)
        .concat(rightProps);
    // 添加總計
    function getfinalsum(dataSource: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
        if (Array.isArray(dataSource) && dataSource.length != 0) {
            const tempObj = cloneDeep(dataSource[0]);
            calcuList.forEach((el) => {
                for (let i = 1; i < dataSource.length; i++) {
                    tempObj[el] = Number(tempObj[el]);
                    tempObj[el] += Number(dataSource[i][el]);
                }
            });
            // 不包含在属性数组中的属性设置为空
            Object.keys(tempObj).forEach((el,) => {
                if (!calcuList.includes(el)) {
                    tempObj[el] = null;
                }
                if (el == sumprops) {
                    tempObj[el] = '總計';
                }
                // 總計需要是負數的處理
                // 不包含在數組裏面的就是整數
                if (isNegative && calcuList.includes(el)) {
                    tempObj[el] = -tempObj[el];
                }
            });
            return tempObj;
        }
    }
    PageSetting["TopList"].push(getfinalsum(PageSetting["TopList"], middleProps.concat(rightProps), ToptableProps[1]));
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps };


}