import { cloneDeep } from "lodash-es";

export interface PovitParams{
    Data:any[];//原始数组
    povitColumn:Object;//需要进行转置的列
    changeColumn: any[];//实际转置后生成的栏位
    repeatColumn: string[];//依据这些栏位进行判断
}
//去除重复数据
export function deRepeat(arr:any[], repeatData:string[]) {
    for (var i = 0; i < arr.length - 1; i++) {
        for (var j = i + 1; j < arr.length; j++) {
            if (verifyParams(repeatData, arr[j], arr[i])) {
                arr.splice(j, 1);
                j--;
            }
        }
    }
    return arr;
}
/**
 * 将DataA里ArrayData的参数依次与DataB进行比较,所有的列名相等返回true,否则返回false 
 * @ArrayData 需要比较的所有列名
 * @DataA 第一个循环的元素
 * @DataB 第二个循环的元素
 */
 export function verifyParams(ArrayData: string[], DataA: Object, DataB: Object) {
    let count = 0;
    for (let i of ArrayData) {
        if (DataA[i] != DataB[i]) {
            count += 1;
        }
    }
    if (count > 0) {
        return false;
    } else {
        return true;
    }

}
export function getPovitData(config:PovitParams) {
    let OriginData = cloneDeep(config.Data);
    for (let m of config.changeColumn) {
          OriginData.map((item) => {
            for (let i in config.povitColumn) {
                let columns = i.split(' ');
                if (columns.length > 1) {
                    let cloumnsparams = '';
                    for(let j = 0;j<columns.length;j++){
                        if(j < columns.length-1){
                            cloumnsparams += item[columns[j]] + ' ';
                        }else {
                            cloumnsparams += item[columns[j]];
                        }
                    }
                    if (cloumnsparams == m) {
                        item[m] = item[config.povitColumn[i]]; //添加屬性
                        // delete item[i];
                        // delete item[config.povitColumn[i]];
                    }
                } else {
                    if (item[i] == m) {
                        item[m] = item[config.povitColumn[i]]; //添加屬性
                        delete item[i];
                        delete item[config.povitColumn[i]];
                    }
                }
            }
        });
        /**
         * 将符合条件的数据转置到一起
         * @repeatData 需要比较的列
         * @data1[j] 第一个循环的元素
         * @data1[i] 第二个循环的元素
         */
        for (let i in OriginData) {
            for (let j in OriginData) {
                if (verifyParams(config.repeatColumn, OriginData[j], OriginData[i])) {
                    // console.log(m,OriginData[j][m],OriginData[i][m]);
                    OriginData[i][m] =OriginData[i][m]??OriginData[j][m];
                }
            }
        }
        // console.log(m,config.changeColumn.length);
    }
    /**去除重复数据 */
    return deRepeat(OriginData, config.repeatColumn);
}

