import { cloneDeep } from 'lodash-es';
// 最後一行總計，列計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：給屬性賦值為總計
isNegative：是否轉爲負數
     */
export function getfinalsum(dataSource: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
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
// 行計算
/* 
dataSource：數據源
calcuList：要計算的列
sumprops：添加的屬性的key
isNegative：是否轉爲負數
*/
export function getrowsum(dataSource: any[], calcuList: any[], sumprop: string, isNegative: boolean = false) {
    dataSource.forEach(item => {
        item[sumprop] = 0;
        for (let i = 0; i < calcuList.length; i++) {
            if (!isNaN(item[calcuList[i]])) {
                item[sumprop] += item[calcuList[i]];
            }
        }
        if (isNegative) {
            item[sumprop] = -item[sumprop];
        }
    })
}
// 插入行計算
// 插入行合计,dataSource数据源，standard:相同的屬性才合計，calcuList要合计的列，是否是负数isNegative,合计属性sumprops
export function Inserttotalrow(dataSource: any[], standard: any[], calcuList: any[], sumprops: string, isNegative: boolean = false) {
    let temp: any = cloneDeep(dataSource[0]);
    let totalsum: any[] = [];
    let totalindex: any[] = [];
    let regArr: any[] = [];
    for (let i = 0; i < dataSource.length - 1; i++) {
        standard.forEach(item => {
            regArr.push(dataSource[i][item] == dataSource[i + 1][item])
        })
        if (regArr.includes(false)) {
            regArr = [];
            Object.keys(temp).forEach((item) => {
                if (!calcuList.includes(item)) {
                    temp[item] = '';
                }
                if (item == sumprops) {
                    temp[item] = '合計';
                }
                // 合計需要是負數的處理
                // 不包含在數組裏面的就是整數
                if (isNegative && calcuList.includes(item)) {
                    temp[item] = -Math.ceil(temp[item]);
                }
            });

            totalindex.push(i + 1);
            totalsum.push(temp);
            temp = cloneDeep(dataSource[i + 1]);
            continue;
        } else {
            for (let j = 0; j < calcuList.length; j++) {
                if(isNaN(temp[calcuList[j]])){
                    temp[calcuList[j]] = 0;
                }if(!isNaN(dataSource[i + 1][calcuList[j]])){
                    temp[calcuList[j]] += dataSource[i + 1][calcuList[j]];
                }
            }
        }
       
    }
    Object.keys(temp).forEach((item) => {
        if (!calcuList.includes(item)) {
            temp[item] = '';
        }

        if (item == sumprops) {
            temp[item] = '合計';
        }
        // 合計需要是負數的處理
        if (isNegative && calcuList.includes(item)) {
            temp[item] = -Math.ceil(temp[item]);
        }
    });
    totalsum.push(temp);
    totalindex.push(dataSource.length);
    return {
        insertrowList: totalsum,
        insertIndexList: totalindex
    };
    /* 
    for (let i = 0; i < insertIndexList.length; i++) {
        temp.splice(insertIndexList[i] + i, 0, insertrowList[i]);
    } */

}

// 
export function getdiffdate(stime:string, etime:string){ //获取两个日期中间的日期
    //初始化日期列表，数组
    var diffdate = new Array();
    var i = 0;
    //开始日期小于等于结束日期,并循环
    while (stime <= etime) {
        diffdate[i] = stime;

        //获取开始日期时间戳
        var stime_ts = new Date(stime).getTime();
        console.log('当前日期：' + stime + '当前时间戳：' + stime_ts);

        //增加一天时间戳后的日期
        var next_date = stime_ts + (24 * 60 * 60 * 1000);

        //拼接年月日，这里的月份会返回（0-11），所以要+1
        var next_dates_y = new Date(next_date).getFullYear() + '-';
        var next_dates_m = (new Date(next_date).getMonth() + 1 < 10) ? '0' + (new Date(next_date).getMonth() + 1) + '-' : (new Date(next_date).getMonth() + 1) + '-';
        var next_dates_d = (new Date(next_date).getDate() < 10) ? '0' + new Date(next_date).getDate() : new Date(next_date).getDate();

        stime = next_dates_y + next_dates_m + next_dates_d;

        //增加数组key
        i++;
    }
    return diffdate;
}

