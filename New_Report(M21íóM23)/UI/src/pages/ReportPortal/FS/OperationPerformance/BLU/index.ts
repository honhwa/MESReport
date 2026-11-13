import { dclick } from '@/pages/ReportPortal/utils/common';
import { TopTable, BottomTable } from '@/pages/ReportPortal/FS/OperationPerformance/BLU/Table'
import router from '@/router';
import { AddOperationParamsWithOut } from '@/store/module/operationDeatil';
import { cloneDeep } from 'lodash-es';
import { message } from 'ant-design-vue';
const UseParamsAdd = AddOperationParamsWithOut();
let FactoryType = "";
function tofirsttable(record, rowindex, column, item, index) {
    console.log("输出record",record["WORK_CTR"]);
    if(record["WORK_CTR"]>=2000){
        UseParamsAdd.AddFirstTableParams({ record, rowindex, column, item, index })
        router.push({ name: `OperationPerformanceDetail${FactoryType}` });
    } else if(record[item["dataIndex"]]>0 ){
        UseParamsAdd.AddFirstTableParams({ record, rowindex, column, item, index })
        router.push({ name: `OperationPerformanceDetail${FactoryType}` });
    }else{
        message.warning("此栏位没有二阶数据!")
    }
    
}
// 首頁下表格跳轉

export function handleindexTable(Data: any[], PageSetting: object) {
    FactoryType = PageSetting["FactoryType"];
    let TopList: any = [];
    let BottomList: any = [];
    Data.forEach(item => {
        if (item.WORK_CTR >= 2000 && item.WORK_CTR <= 3500) {
            //rate計算
            if (item["PASS_QTY"] > 0) {
                item["RATE"] = (item["PASS2_QTY"] / (item["PASS_QTY"] * 100)).toFixed(2)
            } else {
                item["RATE"] = 0;
            }
            BottomList.push(item);
        } else if (item.WORK_CTR >= 1000 && item.WORK_CTR < 2000) {
            if (item["PASS_QTY"] > 0) {
                // rwk/pass Rate
                item["rwkpassRate"] = (item["TOTALRWK_QTY"] / (item["PASS_QTY"]) * 100).toFixed(4);
                // 1' st yield
                item["firstyield"] = ((item["PASS1_QTY"] / item["PASS_QTY"]) * 100).toFixed(4);
                // total yield
                item["totalyield"] = (((item["PASS_QTY"] - item["TOTALRWK_QTY"]) / (item["PASS_QTY"])) * 100).toFixed(4);
                // 维修再发率
                item["repairrate"] = ((item["PASS_QTY"] - item["PASS1_QTY"] - item["TOTALRWK_QTY"]) / (item["PASS_QTY"])*100).toFixed(4)
            } else {
                item["rwkpassRate"] = 0
                item["firstyield"] = 0
                item["totalyield"] = 0
                item["repairrate"] = 0;
            }
            TopList.push(item);
        }
    })
    let tempTopColumns  = cloneDeep(TopTable.ToptableColumns);
    dclick(tempTopColumns, TopTable.ClickColumn, tofirsttable);
    let tempBottomColumns = cloneDeep(BottomTable.BottomTableColums);
    tempBottomColumns.forEach(item => {
        dclick(item.children, BottomTable.ClickColumn, tofirsttable);
    })
    PageSetting["TopColumns"] = tempTopColumns;
    PageSetting["BottomColumns"] = tempBottomColumns;
    PageSetting["TopList"] = TopList;
    PageSetting["BottomList"] = BottomList;

}