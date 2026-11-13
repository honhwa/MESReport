import { GetCommon } from "@/api/pages/common";
import { cloneDeep } from "lodash-es";
import { getPovitData, PovitParams } from "./provit";

async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const {
        Date: {
            Data: { Table: TimeData },
            Data1: { Table: Data },
        },
    } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
    });
}
export async function PassLotShiftDataSet_Finish_Un(
    PageSetting: Object,
    ALLFormParams: Object
) {
    let params = {
        name: "Date",
        apiName: "PassLotShiftDataSet_Finish_Un",
        FactoryType: "BEOL1",
        date_f: ALLFormParams["date_f"],
        date_t: ALLFormParams["date_t"],
        Groups: ALLFormParams["Groups"],
        ProdTypes: ALLFormParams["ProdTypes"],
        Owner: ALLFormParams["Owner"],
        SelPwo: [] as any,
        Selwo: [],
    };
    if (ALLFormParams["Selwo"].includes("ALL")) {
        params["SelPwo"] = ["ALL"];
        params["Selwo"] = [];
    } else {
        params["Selwo"] = ALLFormParams["Selwo"];
    }
    let leftLabels: string[] = [];
    let leftProps: string[] = [];
    if (ALLFormParams["Groups"].length != 0) {
        leftLabels = ["Category"].concat(ALLFormParams["Groups"]);
        leftProps = ["CATE",].concat(ALLFormParams["Groups"]);
    } else {
        leftLabels = ["Category", "PROC_ID", "pfcd", "grade"];
        leftProps = ["CATE", "PROC_ID", "PFCD", "GRADE"];
    }
    let middleLabels: string[] = [];
    let middleProps: any[] = [];
    await getToptableData(PageSetting, params);

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
        repeatColumn: [
            "CATE",
            "EQPT_ID",
            "PFCD",
            "OWNER_ID",
            "GRADE",
            "WORK_ORDER",
        ],
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
    PageSetting["TopList"].push(getfinalsum(PageSetting["TopList"], middleProps.concat(rightProps), ToptableProps[1]))
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps };

}
