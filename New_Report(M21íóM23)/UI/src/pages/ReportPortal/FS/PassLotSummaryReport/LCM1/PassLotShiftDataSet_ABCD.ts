
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import { getPovitData, PovitParams } from "./provit";


async function getToptableData(PageSetting: Object, ALLFormParams: Object) {
    const { Date: { Data: { Table: TimeData }, Data1: { Table: Data } } } = await GetCommon(ALLFormParams);
    PageSetting["TopList"] = Data;
    TimeData.forEach((item) => {
        PageSetting["TimeData"].push(item["CAL_DATE"]);
      });
}
export async function PassLotShiftDataSet_ABCD(PageSetting: Object, ALLFormParams: Object) {
    let params = {
        name: "Date",
        apiName: "PassLotShiftDataSet_ABCD",
        FactoryType: "BEOL1",
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
    let leftLabels: string[] = [];
    let leftProps: string[] = [];
    if (ALLFormParams["Groups"].length == 0) {
        leftLabels = ["Operaction"].concat(ALLFormParams["Groups"]);
        leftProps = ["PROC_ID"].concat(ALLFormParams["Groups"]);
    } else {
        leftLabels = ALLFormParams["Groups"];
        leftProps = ALLFormParams["Groups"];
        leftLabels.forEach(item=>{
            if(item == "PROC_ID"){
                item = "Operaction"
            }
        })
    }
    let middleLabels: string[] = [];
    let middleProps: any[] = [];
    await getToptableData(PageSetting, params);
    for (let i = 0; i < PageSetting["TimeData"].length; i++) {
        let temp1 = PageSetting["TimeData"][i].slice(5).replace("-", "/")+"(早)";
        let temp2 = PageSetting["TimeData"][i].slice(5).replace("-", "/")+"(晚)";
        middleLabels.push(temp1);
        middleLabels.push(temp2);
        middleProps.push(`${PageSetting["TimeData"][i]} 1`);
        middleProps.push(`${PageSetting["TimeData"][i]} 2`);
    }
    let rightLabels = ["早","晚"];
    let rightProps = ["MSum","pSum"];
    let config: PovitParams = {
        Data: PageSetting["TopList"],
        povitColumn: {
            "MFGDATE SHIFT": "GLASS_CNT",
        },
        changeColumn:middleProps,
        repeatColumn: leftProps,
    };
    PageSetting["TopList"] = getPovitData(config);
    let ToptableLabels: string[] = leftLabels
        .concat(middleLabels)
        .concat(rightLabels);
    let ToptableProps: string[] = leftProps
        .concat(middleProps)
        .concat(rightProps);
    // 行總計
    PageSetting["TopList"].forEach(item=>{
        item["MSum"] = 0;
        item["pSum"] = 0;
        for (let i = 0; i < middleProps.length; i++) {
            if (isNaN(item[middleProps[i]])) {
                item[middleProps[i]] = 0;
            }
            // 如果包含1，早相加
            // 如果包含2，晚相加
            if(middleProps[i].split(" ")[1] == "1"){
                item["MSum"] += item[middleProps[i]];
            }else{
                item["pSum"] += item[middleProps[i]];
            }
        }
        
    })
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
    PageSetting["TopList"].push(getfinalsum(PageSetting["TopList"], rightProps, ToptableProps[1]));
    return { ToptableLabels: ToptableLabels, ToptableProps: ToptableProps,middleProps:middleProps };
}