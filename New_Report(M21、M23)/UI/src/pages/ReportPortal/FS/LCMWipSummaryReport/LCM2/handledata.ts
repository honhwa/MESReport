import { getrowsum } from "@/utils/reportcommon/Calculation";
import { getPovitData, PovitParams } from "@/utils/reportcommon/provit";

export function handledata(Data: any[], HoldType: string, Povitkey: string) {
    // 取出嵌套表頭的key值
    let calcuList: any[] = [];
    let changeColumn: any[] = [];
    // 獲取表頭
    let work_ctrgroup: number[] = [];
    Data.forEach(item => {
        if (!work_ctrgroup.includes(item.WORK_CTR)) {
            work_ctrgroup.push(item.WORK_CTR)
        }
    })
    if (HoldType == "non-hold") {
        changeColumn = work_ctrgroup;
    } else {
        let temp: any[] = [];
        Data.forEach(item => {
            if (!temp.includes(item.WORK_CTR1)) {
                temp.push((item.WORK_CTR1))
            }
        })
        temp = temp.map(item => Number(item.replace("hold", "")));
        temp.sort(function (a, b) {
            return a - b;
        });
        temp = temp.map(item => `hold${item}`);

        changeColumn = temp;
    }
    work_ctrgroup.sort(function (a, b) {
        return a - b;
    });
    calcuList = [...changeColumn];
    // 最上層表頭，non-hold或者hold
    let non_hold = {
        title: HoldType,
        children: [] as any,
    }
    let forword = {
        title: "Forword",
        children: [] as any,

    };
    let backword = {
        title: "Backword",
        children: [] as any,

    };
    let Rework = {
        title: "Rework",
        children: [] as any,

    };
    let non_hold_sum = {
        title: `${HoldType}小計`,
        children: [
            {
                title: `${HoldType}小計`,
                dataIndex: `${HoldType}_sum`,
                key: `${HoldType}_sum`,
                className: "Green",
            }
        ],

    };
    calcuList.push(`${HoldType}_sum`);
    work_ctrgroup.forEach((item, index) => {
        // 添加Forward
        if (item >= 1010 && item <= 1300) {
            forword.children.push({
                title: item,
                dataIndex: changeColumn[index],
                key: item,

            })
        }
        // 添加backword
        else if (item >= 1320 && item <= 1910) {
            backword.children.push({
                title: item,
                dataIndex: changeColumn[index],
                key: item,
            })
        }
        // 添加rework
        else if (item >= 2110 && item <= 2500) {
            Rework.children.push({
                title: item,
                dataIndex: changeColumn[index],
                key: item,
            })
        }
    })
    // 數據轉換
    let config: PovitParams = {
        Data,
        povitColumn: {

        },
        changeColumn: changeColumn,
        repeatColumn: ["CUSTOMER", "PROD_NBR", "LCM_OWNER"]
    }
    config["povitColumn"][Povitkey] = 'QTY';
    let povitData = getPovitData(config);
    // if (forword.children.length != 0) {
    // 添加forword小計
    getrowsum(povitData, forword.children.map(item => item.dataIndex), `${HoldType}forwordsum`);
    forword.children.push({
        title: "Forward小計",
        dataIndex: `${HoldType}forwordsum`,
        key: `${HoldType}forwordsum`,
        className: "Green",
    })
    non_hold.children.push(forword);
    calcuList.push(`${HoldType}forwordsum`);
    // 添加forword小計
    getrowsum(povitData, backword.children.map(item => item.dataIndex), `${HoldType}backwordsum`);
    backword.children.push({
        title: "Backword小計",
        dataIndex: `${HoldType}backwordsum`,
        key: `${HoldType}backwordsum`,
        className: "Green",
    })
    calcuList.push(`${HoldType}backwordsum`);




    // if (HoldType == "non-hold") {
        // 添加0-1910總計
        getrowsum(povitData, [`${HoldType}forwordsum`, `${HoldType}backwordsum`], `${HoldType}fbsum`);
        backword.children.push({
            title: "0至1910總計",
            dataIndex: `${HoldType}fbsum`,
            key: `${HoldType}fbsum`,
            className: "Yellow",
        })
        calcuList.push(`${HoldType}fbsum`);
        // non_hold.children.push(backword);
        // 添加Rework小計
        getrowsum(povitData, Rework.children.map(item => item.dataIndex), `${HoldType}reworksum`);
        Rework.children.push({
            title: "Rework小計",
            dataIndex: `${HoldType}reworksum`,
            key: `${HoldType}reworksum`,
            className: "Green",
        })
        // non_hold.children.push(Rework)
        calcuList.push(`${HoldType}reworksum`);
        getrowsum(povitData, [`${HoldType}fbsum`, `${HoldType}reworksum`], `${HoldType}_sum`);
        non_hold.children.push(backword);
        non_hold.children.push(Rework)
   /*  } else {
        // 為hold時候
        // 計算hold小計
        getrowsum(povitData, [`${HoldType}backwordsum`], `${HoldType}_sum`);
        non_hold.children.push(backword);

    } */




    non_hold.children.push(non_hold_sum);
    calcuList.push(`${HoldType}_sum`);
    return { non_hold, povitData, calcuList };
}