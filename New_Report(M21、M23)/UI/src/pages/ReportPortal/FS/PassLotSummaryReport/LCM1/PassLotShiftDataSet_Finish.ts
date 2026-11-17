
import { GetCommon } from '@/api/pages/common';
// import { ref } from 'vue';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";

let tempFinishData: any = [];
let tempUnFinishData: any = [];

async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    tempFinishData = [];
    tempUnFinishData = [];
    const { Date: { Data: { Table: TimeData }, Data1: { Table: Data }, Data2: { Table: Data1 } } } = await GetCommon(ALLFormParams);
    tempFinishData = Data;
    tempUnFinishData = Data1;

    // PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
    });
}
function getfinalsum(dataSource: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
    if (Array.isArray(dataSource) && dataSource.length != 0) {
        const tempObj = cloneDeep(dataSource[0]);
        calcuList.forEach((el) => {
            for (let i = 1; i < dataSource.length; i++) {
                if (isNaN(tempObj[el])) {
                    tempObj[el] = 0;
                }
                if (!isNaN(dataSource[i][el])) {
                    tempObj[el] = Number(tempObj[el]);
                    tempObj[el] += Number(dataSource[i][el]);
                }
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
export async function PassLotShiftDataSet_Finish(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "PassLotShiftDataSet_Finish",
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
        let temp1 = PageSetting["TimeData"][i].slice(5).replace("-", "/") + "(早)";
        let temp2 = PageSetting["TimeData"][i].slice(5).replace("-", "/") + "(晚)";
        middleLabels.push(temp1);
        middleLabels.push(temp2);
        middleProps.push(`${PageSetting["TimeData"][i]} 1`);
        middleProps.push(`${PageSetting["TimeData"][i]} 2`);
    }

    let rightLabels = ["Sum"];
    let rightProps = ["Sum"];

    /* let config: PovitParams = {
        Data: PageSetting["TopList"],
        povitColumn: {
            "MFGDATE SHIFT": "PANEL_CNT",
        },
        changeColumn: middleProps,
        repeatColumn: leftProps,
    };
    PageSetting["TopList"] = getPovitData(config);
    // 計算第一行的sum
    PageSetting["TopList"].forEach((item, index) => {
        if (index == 0) {
            item["Sum"] = 0;
            middleProps.forEach(el => {
                if (!isNaN(item[el])) {
                    item["Sum"] += item[el]
                }

            })
        }
    }); */

    let ToptableLabels: string[] = leftLabels
        .concat(middleLabels)
        .concat(rightLabels);
    let ToptableProps: string[] = leftProps
        .concat(middleProps)
        .concat(rightProps);

    // finish數據
    let config1: PovitParams = {
        Data: tempFinishData,
        povitColumn: {
            "MFGDATE SHIFT": "PANEL_CNT",
        },
        changeColumn: middleProps,
        repeatColumn: leftProps,
    };
    tempFinishData = getPovitData(config1);
    tempFinishData.forEach((item) => {
        // if (index == 0) {
        item["Sum"] = 0;
        middleProps.forEach(el => {
            if (!isNaN(item[el])) {
                item["Sum"] += item[el]
            }

        })
        // }
    });
    if(tempFinishData.length!=0){
        tempFinishData.push(getfinalsum(tempFinishData, middleProps.concat(rightProps), leftProps[1]));
    }
    

    //unfinish
    let config2: PovitParams = {
        Data: tempUnFinishData,
        povitColumn: {
            "MFGDATE SHIFT": "PANEL_CNT",
        },
        changeColumn: middleProps,
        repeatColumn: leftProps,
    };
    tempUnFinishData = getPovitData(config2);

    tempUnFinishData.forEach((item) => {
        // if (index == 0) {
        item["Sum"] = 0;
        middleProps.forEach(el => {
            if (!isNaN(item[el])) {
                item["Sum"] += item[el]
            }

        })
        // }
    });
    if(tempUnFinishData.length!=0){
        tempUnFinishData.push(getfinalsum(tempUnFinishData, middleProps.concat(rightProps), leftProps[1]));

    }
    PageSetting["TopList"] = tempFinishData.concat(tempUnFinishData);


    // PageSetting["TopList"] = tempFinishData;   
    // 下表格的finish計算
    let bottomfinishleftLabels:any[] = ["Category","Grade"];
    let bottomfinishmiddleLabels:any[] = [];
    let finishgrade:any[] = [];
    let bottomfinishLeftProps:any[] = ["CATE","GRADE"];
    // middelprops和label是同一個
    
    tempFinishData.forEach(item=>{
        if(item["PFCD"] != null){
            
        if(!bottomfinishmiddleLabels.includes(item["PFCD"].slice(2,6))){
            bottomfinishmiddleLabels.push(item["PFCD"].slice(2,6));
        }
    }
        if(!finishgrade.includes(item["GRADE"])){
            finishgrade.push(item["GRADE"])
        }
    })
    let bottomfinishDate:any[] = [];
    // 添加category和grade和Sum
    for(let i = 0;i<finishgrade.length;i++){
        bottomfinishDate.push({
            "CATE":"Finish",
            "GRADE":finishgrade[i],
            "Sum":0
        })
    }
    let finishLabels = bottomfinishleftLabels.concat(bottomfinishmiddleLabels).concat(["Sum"])
    let finishProps = bottomfinishLeftProps.concat(bottomfinishmiddleLabels).concat(["Sum"])
    
    bottomfinishDate.forEach(item=>{
        for(let i =0;i<bottomfinishmiddleLabels.length;i++){
            item[bottomfinishmiddleLabels[i]] = 0
        }
    })


    // 下表格的unfinish計算
    let bottomunfinishmiddleLabels:any[] = [];
    let unfinishgrade:any[] = [];
   
    tempUnFinishData.forEach(item=>{
        if(item["PFCD"] != null){
            if(!bottomunfinishmiddleLabels.includes(item["PFCD"].slice(2,6))){
                bottomunfinishmiddleLabels.push(item["PFCD"].slice(2,6));
            }
        }
        
        if(!unfinishgrade.includes(item["GRADE"])){
            unfinishgrade.push(item["GRADE"])
        }
    })
    let bottomunfinishDate:any[] = [];
    // 添加category和grade和Sum
    for(let i = 0;i<unfinishgrade.length;i++){
        bottomunfinishDate.push({
            "CATE":"UnFinish",
            "GRADE":unfinishgrade[i],
            "Sum":0
        })
    }
    bottomunfinishDate.forEach(item=>{
        for(let i =0;i<bottomfinishmiddleLabels.length;i++){
            item[bottomfinishmiddleLabels[i]] = 0
        }
    })
    let bottomLabels = finishLabels;
    let bottomProps = finishProps;
    if(bottomfinishDate.length!=0){
        bottomfinishDate[bottomfinishDate.length-1]["GRADE"] = "Sum";
    }
    if(bottomunfinishDate.length!=0){
        bottomunfinishDate[bottomunfinishDate.length-1]["GRADE"] = "Sum";
    }
    let bottomDate = bottomfinishDate.concat(bottomunfinishDate)
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps ,bottomLabels,bottomProps,bottomDate};




}