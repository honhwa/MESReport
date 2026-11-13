/*
 * @Description: echarts 自适应问题
 */
import { Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { asyncComputed, useInfiniteScroll } from '@vueuse/core';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';

const UseCommonParams = AddCommonPageParamsWithOut();
export default function () {
    async function getCommonFormParams(extractparams: string, apiparams: Object) {
        let ReturnData: any = [];
        let ApiData = await GetCommon(apiparams);
        let Data = ApiData.Date.Data.Table ?? ApiData.api01.Data.Table;
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][extractparams], value: Data[i][extractparams] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getOperation(funcparams: String, FactoryType) {
        let ReturnData: any = [];
        let { api01: ApiData } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            apiName: funcparams,
            FactoryType: FactoryType
        });
        let Data: any[] = [];
        if (Reflect.has(ApiData, 'Table')) Data = ApiData.Table;
        else Data = ApiData.api01.Data.Table;
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`, value: Data[i]['WORK_CTR'] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getEQPData(params: String, funcparams: String, FactoryType: String, ReportType: String) {
        let ReturnData: any[] = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType,
            ReportType: ReportType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params], value: Data[i][params] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getSizeData(params: String, funcparams: String, FactoryType: String) {
        let ReturnData: any[] = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params], value: Data[i][params] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getSize4Data(FactoryType: String) {
        let ReturnData: any[] = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: 'TN_Size4',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i]['CODE_DESC'], value: Data[i]['CODE_1'] });
        }
        return ReturnData;
    }
    const mounted = async (optionsdata, FactoryType) => {
        // optionsdata.TN_SizeData = await getSizeData('SIZE1', 'TN_Size', FactoryType);
        // optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData = await getSizeData('PART', 'TN_BigProduct', FactoryType);
        // optionsdata.TN_ProductData1 = optionsdata.TN_ProductData = await getSizeData('SOPTION', 'TN_Product', FactoryType);
        //optionsdata.TN_LCMOwner = await getSizeData('OWNER_CODE', 'TN_Owner', FactoryType);
        //TN_OperationLabel
        // const { Date: { Data: { Table: Data } } } = await GetCommon({
        //     name: "Date",
        //     apiName: "TN_Operation",
        //     FactoryType: FactoryType
        // })
        // optionsdata.TN_OperationLabel = Data;
        //TN_OperationLabelByDownGrade
        // const { Date: { Data: { Table: Data_DownGrade } } } = await GetCommon({
        //     name: "Date",
        //     apiName: "TN_OperationByDownGrade",
        //     FactoryType: FactoryType
        // })
        // optionsdata.TN_OperationByDownGradeLabel = Data_DownGrade;

        //optionsdata.TN_LcdGrade = await getSizeData('LCD_GRADE', 'TN_LcdGrade', FactoryType);
        //optionsdata.TN_LcdOwner = await getSizeData('LCD_OWNER', 'TN_LcdOwner', FactoryType);
        //optionsdata.TN_PanelSource = await getSizeData('PANELSOURCE', 'TN_PanelSource', FactoryType);

        // const { Date: { Data: { Table: Data_HoldCode } } } = await GetCommon({
        //     name: "Date",
        //     apiName: "TN_HoldCode",
        //     FactoryType: FactoryType
        // })
        // optionsdata.TN_HoldCode = Data_HoldCode;
        // optionsdata.TN_BigEquipment = await getSizeData('CODE', 'TN_BigEquipment', FactoryType);
        // optionsdata.TN_Equipment1 = optionsdata.TN_Equipment = await getSizeData('PART', 'TN_Equipment', FactoryType);
        return optionsdata;
    };

    // //Down Grade Table:RT_USR_CODE where CODE_CATE='OPER_L2'
    // const mountedDoenCodeL2 = async (optionsdata, FactoryType) => {
    //     const { Date: { Data: { Table: Data } } } = await GetCommon({
    //         name: "Date",
    //         apiName: "TN_OperationByDownGradeL2",
    //         FactoryType: FactoryType
    //     })
    //     optionsdata.TN_OperationByDownGradeL2Label = Data;
    // }

    // //Error Code 查詢
    // const mountedErrorCode = async (optionsdata, FactoryType) => {
    //     const { Date: { Data: { Table: Data } } } = await GetCommon({
    //         name: "Date",
    //         apiName: "TN_ErrorCode",
    //         FactoryType: FactoryType
    //     })
    //     optionsdata.TN_ErrorCode = Data;
    // }

    // const mountedDefectCode = async (optionsdata, FactoryType, optionsdataprop: string = 'DefectCodeData') => {
    //     let ReturnData: any = [];
    //     let { Date} = await GetCommon({
    //         name: 'Date',
    //         apiName: 'TN_DefectCode',
    //         FactoryType: FactoryType
    //     });
    //     let Data = Date.Data.Table;
    //     let Data1 = Date.Data1.Table;
    //     let Data2 = Date?.Data2?.Table;
    //     debugger
    //     Data.forEach((item) => {
    //         ReturnData.push({
    //             label: `${item['ERRC_NBR']} defect`,
    //             value: `substr(dfct_code,1,1) = '${item['ERRC_NBR']}'`
    //         });
    //     });
    //     Data1.forEach((item) => {
    //         ReturnData.push({
    //             label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
    //             value: `rtrim(dfct_code) = '${item['ERRC_NBR']}'`
    //         });
    //     });
    //     Data2.forEach((item) => {
    //         ReturnData.push({
    //             label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
    //             value: `rtrim(dfct_code) = '${item['ERRC_NBR']}'`
    //         });
    //     });
    //     ReturnData.unshift({ label: 'ALL', value: '1=1' });
    //     optionsdata.TN_DefectCode = ReturnData;
    //     return optionsdata

    // };

    //EQP
    const mountedEQP = async (optionsdata, FactoryType, ReportType) => {
        optionsdata.TN_EQPGroup = await getEQPData('CODE', 'TN_EQPGroup', FactoryType, ReportType);
        optionsdata.TN_EQPID1 = optionsdata.TN_EQPID = await getEQPData('PART', 'TN_EQPID', FactoryType, ReportType);
        return optionsdata;
    };

    //Equipment
    const mountedTNEquipment = async (optionsdata, FactoryType) => {
        optionsdata.TN_BigEquipment = await getSizeData('CODE', 'TN_BigEquipment', FactoryType);
        optionsdata.TN_Equipment1 = optionsdata.TN_Equipment = await getSizeData('PART', 'TN_Equipment', FactoryType);
    };

    //LCM Owner
    const mountedLCMOwner = async (optionsdata, FactoryType) => {
        optionsdata.TN_LCMOwner = await getSizeData('OWNER_CODE', 'TN_Owner', FactoryType);
    };

    const mountedTreeLCMOwner = async (optionsdata, FactoryType) => {
        optionsdata.TN_LCMOwner = await getTreeLCMOwner(FactoryType);
    };

    //Operation
    const mountedOper = async (optionsdata, FactoryType) => {
        optionsdata['TN_OperationLabel'] = await getOper(FactoryType);
    };

    //Size
    const mountedTNSize = async (optionsdata, FactoryType) => {
        optionsdata.TN_SizeData = await getSizeData('SIZE1', 'TN_Size', FactoryType);
        optionsdata.TN_BigProductData1 = optionsdata.TN_BigProductData = await getSizeData(
            'PART',
            'TN_BigProduct',
            FactoryType
        );
        optionsdata.TN_ProductData1 = optionsdata.TN_ProductData = await getSizeData(
            'SOPTION',
            'TN_Product',
            FactoryType
        );
    };

    //Size
    const mountedTNSize4 = async (optionsdata, FactoryType) => {
        optionsdata.TN_SizeData = await getSize4Data(FactoryType);
    };
    // 共用operaction
    async function getOper(FactoryType: String) {
        let ReturnData: any = [];
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'TN_Operation',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
                value: Data[i]['WORK_CTR']
            });
        }
        debugger;
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getTreeLCMOwner(FactoryType){
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: 'TN_TreeLCMOwner',
            FactoryType: FactoryType
        });
        let ReturnData:any = [{ id:"ALL",title: 'ALL', value: 'ALL', children: [] }];
        let rs = {};
        for (let i in Data) {
            rs[Data[i]['OWNER_TYPE']] = rs[Data[i]['OWNER_TYPE']] || [];
            rs[Data[i]['OWNER_TYPE']].push(Data[i]['OWNER_ID']);
        }
        for (let i in rs) {
            let fir:any = {
                id: i,
                title: i,
                value: 'TYPE-' + i,
                children:[]
            }
            for (let j in rs[i]) {
                let sec:any = {
                    id: i + '-' +rs[i][j],
                    title: rs[i][j],
                    value: rs[i][j]
                };
                fir.children?.push(sec);
            }
            ReturnData[0].children?.push(fir);
        }
        console.log(ReturnData);
        return ReturnData;
    }

    //LCD Owner
    const mountedLCDOwner = async (optionsdata, FactoryType) => {
        optionsdata.TN_LcdOwner = await getSizeData('LCD_OWNER', 'TN_LcdOwner', FactoryType);
    };

    //LCD Grade
    const mountedLCDGrade = async (optionsdata, FactoryType) => {
        optionsdata.TN_LcdGrade = await getSizeData('LCD_GRADE', 'TN_LcdGrade', FactoryType);
    };

    //Down Grade
    const mountedDownGrade = async (optionsdata, FactoryType) => {
        optionsdata['TN_OperationByDownGradeLabel'] = await getDownGrade(FactoryType);
    };

    // 共用operaction
    async function getDownGrade(FactoryType: String) {
        let ReturnData: any = [];
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'TN_OperationByDownGrade',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
                value: Data[i]['WORK_CTR']
            });
        }
        debugger;
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    //Down Grade L2
    const mountedDownGradeL2 = async (optionsdata, FactoryType) => {
        optionsdata['TN_OperationByDownGradeL2Label'] = await getDownGradeL2(FactoryType);
    };

    // 共用operaction
    async function getDownGradeL2(FactoryType: String) {
        let ReturnData: any = [];
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'TN_OperationByDownGradeL2',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
                value: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`
            });
        }
        debugger;
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    //Hold Code
    const mountedHoldCode = async (optionsdata, FactoryType) => {
        optionsdata['TN_HoldCode'] = await getHoldCode(FactoryType);
    };

    // 共用operaction
    async function getHoldCode(FactoryType: String) {
        let ReturnData: any = [];
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'TN_HoldCode',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['HOLD_CODE']}-${Data[i]['HOLD_DESC']}`,
                value: Data[i]['HOLD_CODE']
            });
        }
        debugger;
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    //Error Code
    const mountedErrorCode = async (optionsdata, FactoryType) => {
        optionsdata['TN_ErrorCode'] = await getErrorCode(FactoryType);
    };

    // 共用operaction
    async function getErrorCode(FactoryType: String) {
        let ReturnData: any = [];
        const {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            name: 'Date',
            apiName: 'TN_ErrorCode',
            FactoryType: FactoryType
        });
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({
                label: `${Data[i]['ERRC_NBR']}-${Data[i]['ERRC_NBR']}`,
                value: Data[i]['ERRC_NBR']
            });
        }
        debugger;
        //ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    //PanelSource
    const mountedPanelSource = async (optionsdata, FactoryType) => {
        optionsdata.TN_PanelSource = await getSizeData('PANELSOURCE', 'TN_PanelSource', FactoryType);
    };

    //Defect Code
    const mountedDefectCode = async (optionsdata, FactoryType) => {
        let ReturnData: any = [];
        let { Date } = await GetCommon({
            name: 'Date',
            apiName: 'TN_DefectCode',
            FactoryType: FactoryType
        });
        let Data = Date.Data.Table;
        let Data1 = Date.Data1.Table;
        let Data2 = Date?.Data2?.Table;
        debugger;
        Data.forEach((item) => {
            ReturnData.push({
                label: `${item['ERRC_NBR']} defect`,
                value: `substr(dfct_code,1,1) = '${item['ERRC_NBR']}'`
            });
        });
        Data1.forEach((item) => {
            ReturnData.push({
                label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
                value: `rtrim(dfct_code) = '${item['ERRC_NBR']}'`
            });
        });
        Data2.forEach((item) => {
            ReturnData.push({
                label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
                value: `rtrim(dfct_code) = '${item['ERRC_NBR']}'`
            });
        });
        ReturnData.unshift({ label: 'ALL', value: '1=1' });
        optionsdata.TN_DefectCode = ReturnData;
        return optionsdata;
    };

    //Date Month
    const mountedDateMonth = async (optionsdata, FactoryType, DateRange) => {
        let ReturnData: any = [];
        let { Date } = await GetCommon({
            name: 'Date',
            apiName: 'TN_DateMonth',
            FactoryType: FactoryType,
            DateRange : DateRange
        });
        let Data = Date.Data.Table;
        Data.forEach((item) => {
            ReturnData.push({
                label: `${item['DATEMONTH']}`,
                value: `${item['DATEMONTH']}`
            });
        });
        optionsdata.TN_DateMonth = ReturnData;
        return optionsdata;
    };

    /**
     *
     * @param dom 設定的ref節點
     * @param PageSetting 全部數據
     * @distance 距離底部的距離
     */
    function InfiniteScroll(dom: Ref<HTMLElement | null>, PageSetting: BasePageParams.BasePageSetting) {
        useInfiniteScroll(
            dom,
            () => {
                // load more
                if (PageSetting.moreData.length > 100) {
                    let start = PageSetting.List.length;
                    let end = PageSetting.List.length + 20;
                    let newData = PageSetting.moreData.slice(start, end);
                    PageSetting.List.push(...newData);
                }
            },
            { distance: 10 }
        );
    }

    return {
        mounted,
        getSizeData,
        InfiniteScroll,
        getCommonFormParams,
        getOperation,

        mountedEQP,
        mountedTNSize,
        mountedTNSize4,
        mountedLCMOwner,
        mountedOper,
        mountedLCDOwner,
        mountedLCDGrade,
        mountedDownGrade,
        mountedDownGradeL2,
        mountedHoldCode,
        mountedErrorCode,
        mountedPanelSource,
        mountedDefectCode,
        mountedTNEquipment,
        mountedTreeLCMOwner,
        mountedDateMonth,
        
    };
}
