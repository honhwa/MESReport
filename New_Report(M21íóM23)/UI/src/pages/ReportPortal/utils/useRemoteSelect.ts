/*
 * @Description: echarts 自适应问题
 */
import { Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { useInfiniteScroll } from '@vueuse/core';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { optionsdata } from '../FS/PanelWipStayChart/LCM1/form-schema';

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
    // 不包含Data
    async function getOwner(params: String, funcparams: String, FactoryType: String) {
        let ReturnData: object[] = [];
        let Date = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        let Data;
        if (funcparams == 'LCMOwner') {
            Data = Date.Date.Data.Table;
        } else {
            Data = Date.Date.Table;
        }

        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: Data[i][params], value: Data[i][params] });
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    // moveout time使用的operaction
    async function getOperation(funcparams: String, FactoryType) {
        let ReturnData: any = [];
        let { api01: ApiData } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            apiName: funcparams,
            FactoryType: FactoryType
        });
        let Data: any[] = [];
        if (Reflect.has(ApiData, 'Table')) Data = ApiData.Table;
        else Data = ApiData.Data.Table;
        for (let i in Data) {
            //@ts-ignore
            ReturnData.push({ label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`, value: Data[i]['WORK_CTR'] });
        }
        // ReturnData.unshift({ label: 'ALL', value: 'ALL' });
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
    // 共用operaction
    async function getOper(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: { Table: Data }
        } = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        for (let i in Data) {
            if (FactoryType == 'BEOL2') {
                ReturnData.push({ label: Data[i]['PROC_ID'], value: Data[i]['PROC_ID'] });
            } else {
                //@ts-ignore
                ReturnData.push({
                    label: `${Data[i]['WORK_CTR']}-${Data[i]['WORK_NAME']}`,
                    value: Data[i]['WORK_CTR']
                });
            }
        }
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getEquipment(funcparams: string, FactoryType: String, DataType: number) {
        let Equipment: any = [];
        let BigEquipment: any = [];
        let {
            Date: {
                Data: { Table: EquipmentData },
                Data1: { Table: BigEquipmentData }
            }
        } = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType,
            dept: DataType
        });
        EquipmentData.forEach((item) => {
            Equipment.push({
                label: item['CODE'],
                value: item['CODE']
            });
        });
        Equipment.unshift({ label: 'ALL', value: 'ALL' });
        BigEquipmentData.forEach((item) => {
            BigEquipment.push({
                label: item['EQUIP_NBR'],
                value: item['EQUIP_NBR']
            });
        });
        BigEquipment.unshift({ label: 'ALL', value: 'ALL' });

        return { Equipment, BigEquipment };
    }
    async function getLcmProdType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        
        Data.forEach((item, index) => {
            if (index == 0) {
                ReturnData.push({ label: 'Normal', value: 'Normal' });
            } else {
                ReturnData.push({ label: item['SOPTION'], value: item['SOPTION'] });
            }
        });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAMatType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: '1',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['MAT_ID'], value: item['MAT_ID'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAProductType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: '2',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['PRODUCT_ID'], value: item['PRODUCT_ID'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAModelType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: '3',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['MODEL_NAME'], value: item['MODEL_NAME'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAFabType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: 'fab',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['FAB'], value: item['FAB'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMA3Date(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: 'rma3_date',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['ACCT_DATE'], value: item['ACCT_DATE'] });
             });
        // ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAPartType(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: 'part',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['PART'], value: item['PART'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getRMAPart3Type(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            type: 'part3',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
               ReturnData.push({ label: item['PART'], value: item['MAT_TYPE'] });
             });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getEDCLineid(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
           
            ReturnData.push({ label: item['SOPTION'], value: item['SOPTION'] });
            
        });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }
    async function getEDCUnit(funcparams: String, FactoryType: String) {
        let ReturnData: any = [];
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: funcparams,
            FactoryType: FactoryType
        });
        Data.forEach((item, index) => {
           
            ReturnData.push({ label: item['SOPTION'], value: item['SOPTION'] });
            
        });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        return ReturnData;
    }

    async function getPlanVersion(funcparams: String, FactoryType: String) {
        let {
            api01: { Data: Version }
        } = await GetCommon({
            apiName: funcparams,
            FactoryType: FactoryType
        });
        return Version;
    }

    const mounted = async (optionsdata, FactoryType) => {
        if (['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
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
            optionsdata.TN_LCMOwner = await getSizeData('OWNER_CODE', 'TN_Owner', FactoryType);
            const {
                Date: {
                    Data: { Table: Data }
                }
            } = await GetCommon({
                name: 'Date',
                apiName: 'TN_Operation',
                FactoryType: FactoryType
            });
            optionsdata.TN_OperationLabel = Data;
            optionsdata.TN_LcdGrade = await getSizeData('LCD_GRADE', 'TN_LcdGrade', FactoryType);
            optionsdata.TN_PlanVersion = await getPlanVersion('TN_PlanVersion', FactoryType);
        } else {
            optionsdata.SizeData = await getSizeData('SIZE1', 'Size', FactoryType);
            optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(
                'PART',
                'BigProduct',
                FactoryType
            );
            let temp = await getSizeData('SOPTION', 'Product', FactoryType);
            // 刪除bigprod的ALL選項
            temp.shift();
            optionsdata.ProductData1 = optionsdata.ProductData = temp;
            // 有些BLU1和BLU2不需要bigproduct
            switch (FactoryType) {
                case 'BLU1':
                case 'BLU2':
                    break;
                default:
                    optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(
                        'PART',
                        'BigProduct',
                        FactoryType
                    );
                    break;
            }
        }
        return optionsdata;
    };
    const mountedowner = async (optionsdata, FactoryType, optionsdataprop: string = 'OwnerName') => {
        let params = 'SOPTION';
        switch (FactoryType) {
            case 'BEOL2':
                params = 'CODE';
                break;
            default:
                break;
        }
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getOwner(params, 'Owner', FactoryType);
        }
    };
    //LCMowner 查詢
    const mountedLCMowner = async (optionsdata, FactoryType, optionsdataprop: string = 'OwnerName') => {
        optionsdata[optionsdataprop] = await getOwner('SOPTION', 'LCMOwner', FactoryType);
    };
    const mountedOper = async (optionsdata, FactoryType, optionsdataprop: string = 'OperationLabel') => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getOper('Operation', FactoryType);
        }
    };
    const mountedEquip = async (
        optionsdata,
        FactoryType,
        DataType,
        optionsdataprop1: string = 'EquipmentData',
        optionsdataprop2: string = 'BigEquipmentData',
        optionsdataprop3: string = 'BigEquipmentData1'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let { Equipment, BigEquipment } = await getEquipment('Equipment', FactoryType, DataType);
            optionsdata[optionsdataprop1] = Equipment;
            optionsdata[optionsdataprop2] = optionsdata[optionsdataprop3] = BigEquipment;
        }
    };
    const mountedLcmProdType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'LcmProdTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getLcmProdType('LCMProductType', FactoryType);
        }
    };
    const mountedRMAMatType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAMatTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAMatType('MaterialPredctQuery', FactoryType);
        }
    };
    const mountedRMAProductType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAProductTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAProductType('MaterialPredctQuery', FactoryType);
        }
    };
    const mountedRMAModelType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAModelTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAModelType('MaterialPredctQuery', FactoryType);
        }
    };
    const mountedRMAFabType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAFabTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAFabType('StockCheckQuery', FactoryType);
        }
    };
    const mountedRMA3Date = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMA3DateData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMA3Date('PredCompareQuery', FactoryType);
        }
    };
    const mountedRMAPartType = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAPartTypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAPartType('StockCheckQuery', FactoryType);
        }
    };
    const mountedRMAPart3Type = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'RMAPart3TypeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getRMAPart3Type('PredCompareQuery', FactoryType);
        }
    };
    const mounteEDCLineid = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'EDCLineidData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getEDCLineid('EDCLineid', FactoryType);
        }
    };
    const mounteEDCUnit = async (
        optionsdata: object,
        FactoryType: string,
        optionsdataprop: string = 'EDCUnitData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'CARUX'].includes(FactoryType)) {
            optionsdata[optionsdataprop] = await getEDCUnit('EDCUnit', FactoryType);
        }
    };
    const mountedLCDGrade = async (optionsdata, FactoryType, optionsdataprop: string = 'LcdGradeLabel') => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            /*  let { Date: { Data: { Table: Data } } }  */
            let Date = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'LCDGrade',
                FactoryType: FactoryType
            });
            let Data = [];
            if (Date.Date.ds) {
                Data = Date.Date.ds.Table;
            } else {
                Data = Date.Date.Data.Table;
            }
            Data.forEach((item) => {
                ReturnData.push({ label: item['SGRADE'], value: item['SGRADE'] });
            });
            optionsdata[optionsdataprop] = ReturnData;
        }
    };
    const mountedLCMProductSize = async (optionsdata, FactoryType, optionsdataprop: string = 'LCMProductSizeData') => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            let {
                Date: {
                    Data: { Table: Data }
                }
            } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'LCMProductSize_TBSR',
                FactoryType: FactoryType
            });
            Data.forEach((item) => {
                ReturnData.push({ label: item['SOPTION'], value: item['SOPTION'] });
            });
            ReturnData.unshift({ label: 'ALL', value: 'ALL' });
            optionsdata[optionsdataprop] = ReturnData;
        }
    };
    const mountedDefectCode = async (optionsdata, FactoryType, optionsdataprop: string = 'DefectCodeData') => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            let { Date } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'DefectCode',
                FactoryType: FactoryType
            });
            let Data = Date.Data.Table;
            let Data1 = Date.Data1.Table;
            let Data2 = Date?.Data2?.Table;
            Data.forEach((item) => {
                ReturnData.push({
                    label: `${item['ERRC_NBR']} defect`,
                    value: `substr(dfct_code,1,1) = '${item['ERRC_NBR']}'`
                });
            });
            Data1.forEach((item) => {
                ReturnData.push({
                    label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
                    value: `dfct_code = '${item['ERRC_NBR']}'`
                });
            });
            if (!['BLU1', 'BLU2'].includes(FactoryType)) {
                Data2.forEach((item) => {
                    ReturnData.push({
                        label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
                        value: `dfct_code = '${item['ERRC_NBR']}'`
                    });
                });
            }
            ReturnData.unshift({ label: 'ALL', value: '1=1' });
            optionsdata[optionsdataprop] = ReturnData;
        }
    };
    const mountedLCDProduct = async (
        optionsdata,
        FactoryType,
        DataType,
        optionsdataprop1: string = 'LCDProductData',
        optionsdataprop2: string = 'BigLCDProductData',
        optionsdataprop3: string = 'BigLCDProductData1'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let Equipment: any = [];
            let BigEquipment: any = [];
            let {
                Date: {
                    Data: { Table: EquipmentData },
                    Data1: { Table: BigEquipmentData }
                }
            } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'LCDProduct',
                FactoryType: FactoryType,
                i_type: DataType
            });
            EquipmentData.forEach((item) => {
                Equipment.push({
                    label: item['SIZE1'],
                    value: item['SIZE1']
                });
            });
            Equipment.unshift({ label: 'ALL', value: 'ALL' });
            BigEquipmentData.forEach((item) => {
                BigEquipment.push({
                    label: item['PART'],
                    value: item['PART']
                });
            });
            BigEquipment.unshift({ label: 'ALL', value: 'ALL' });
            optionsdata[optionsdataprop1] = Equipment;
            optionsdata[optionsdataprop2] = BigEquipment;
            optionsdata['tempBigLCDProductData'] = BigEquipment;
            optionsdata[optionsdataprop3] = BigEquipmentData;
            /*  // 為已處理的格式，格式爲{label:XX,value:XX}
            console.log("輸出LCDProductData",optionsdata[optionsdataprop1]);
            // 為已處理的格式，格式爲{label:XX,value:XX}
            console.log("輸出BigLCDProductData",optionsdata[optionsdataprop2]);
            // 為未處理的格式，格式爲{SIZE1:XX,PART:XX}
            console.log("輸出BigLCDProductData1",optionsdata[optionsdataprop3]); */
        }
    };
    const mountedExpectDefectCode = async (
        optionsdata,
        FactoryType,
        optionsdataprop: string = 'ExpectDefectCodeData'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            const {
                Date: {
                    Data: { Table: Data }
                }
            } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'ExpectDefectCode_QT',
                FactoryType: FactoryType
            });
            Data.forEach((item) => {
                ReturnData.push({
                    label: `${item['ERRC_NBR']}-${item['ERRC_DESCR']}`,
                    value: item['ERRC_NBR']
                });
            });
            ReturnData.unshift({ label: 'ALL', value: '1==1' });
            optionsdata[optionsdataprop] = ReturnData;
        }
        // 	OutputByGradeDownGrade的共用接口(LCMGrade)，不是公用的
    };
    const mountedLCMGrade = async (optionsdata, FactoryType, optionsdataprop: string = 'LCMGradeData') => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            const {
                Date: {
                    Data: { Table: Data }
                }
            } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'LCMGrade_OPBGDG',
                FactoryType: FactoryType
            });
            Data.forEach((item) => {
                ReturnData.push({
                    label: item['SGRADE'],
                    value: item['SGRADE']
                });
            });
            optionsdata[optionsdataprop] = ReturnData;
        }
    };
    // 公用接口LCM Grade
    const mountedcommonLCMGrade = async (optionsdata, FactoryType, optionsdataprop: string = 'LCMGradeData') => {
        let ReturnData: any = [];

        let Date = await GetCommon({
            G_FAC: 'A',
            name: 'Date',
            apiName: 'LCMGrade',
            FactoryType: FactoryType
        });
        let Data = [];
        if (Date.Date.ds) {
            Data = Date.Date.ds.Table;
        } else {
            Data = Date.Date.Data.Table;
        }
        Data.forEach((item) => {
            ReturnData.push({
                label: item['LCM_GRADE'],
                value: item['LCM_GRADE']
            });
        });
        ReturnData.unshift({
            label: 'ALL',
            value: 'ALL'
        });
        optionsdata[optionsdataprop] = ReturnData;
    };
    const mountedWorkOrderInformation = async (
        optionsdata,
        FactoryType,
        FromDate,
        ToDate,
        optionsdataprop: string = 'WorkOrderInformation'
    ) => {
        if (!['MOD1', 'MOD2', 'N9', 'MOD3', 'MOD4'].includes(FactoryType)) {
            let ReturnData: any = [];
            const {
                Date: {
                    Data: { Table: Data }
                }
            } = await GetCommon({
                G_FAC: 'A',
                name: 'Date',
                apiName: 'Work_OPBYDG',
                FactoryType: FactoryType,
                fdate: FromDate,
                tdate: ToDate
            });
            Data.forEach((item) => {
                ReturnData.push({
                    label: item['WO_NBR'],
                    value: item['WO_NBR']
                });
            });
            optionsdata[optionsdataprop] = ReturnData;
        }
    };
    // FS的BLU2的size,bigproduct,product特殊，单独封装
    const mountedFSBLU2 = async (optionsdata, FactoryType: string = 'BLU2') => {
        optionsdata.SizeData = await getSizeData('SIZE1', 'Size', FactoryType);
        let {
            Date: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: 'BigProduct',
            FactoryType: FactoryType
        });
        let tempbigproduct: object[] = [];
        Data.forEach((item) =>
            tempbigproduct.push({
                label: `${item.PART}(${item.SIZE1})`,
                value: item.SIZE1
            })
        );
        tempbigproduct.unshift({ label: 'ALL', value: 'ALL' });
        optionsdata.BigProductData1 = optionsdata.BigProductData = tempbigproduct;
        let tempproduct: object[] = [];
        let {
            Date: {
                Data: { Table: ProductData }
            }
        } = await GetCommon({
            G_FAC: UseCommonParams.G_FAC_COM,
            name: 'Date',
            apiName: 'Product',
            FactoryType: FactoryType
        });
        ProductData.forEach((item) => {
            tempproduct.push({
                label: item.PART_NBR,
                // value: item.SOPTION,
                value: item.PART_NBR
            });
        });
        optionsdata.ProductData1 = optionsdata.ProductData = tempproduct;
    };
    // Hold code
    const mountedHoldCode = async (optionsdata, FactoryType, optionsdataprop: string = 'HoldCodeData') => {
        let ReturnData: any[] = [];
        let {
            api01: {
                Data: { Table: Data }
            }
        } = await GetCommon({
            G_FAC: 'A',
            // name: "Date",
            apiName: 'HoldCode',
            FactoryType: FactoryType
        });
        Data.forEach((item) => {
            ReturnData.push({
                label: `${item['CODE']}-${item['C_DESC']}`,
                value: item['CODE']
            });
        });
        ReturnData.unshift({ label: 'ALL', value: 'ALL' });
        optionsdata[optionsdataprop] = ReturnData;
        return ReturnData;
    };

    // Material
    const mountedMaterial = async (optionsdata, FactoryType, optionsdataprop: string = 'Marterial') => {
      let ReturnData: any[] = [];
      let {
          api01: {
              Data: { Table: Data }
          }
      } = await GetCommon({
          G_FAC: 'A',
          // name: "Date",
          apiName: 'Marterial',
          FactoryType: FactoryType
      });
      Data.forEach((item) => {
          ReturnData.push({
              label: item['MAT_TYPE'],
              value: item['MAT_TYPE']
          });
      });
      ReturnData.unshift({ label: 'ALL', value: 'ALL' });
      optionsdata[optionsdataprop] = ReturnData;
      return ReturnData;
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
        mountedFSBLU2,
        getSizeData,
        InfiniteScroll,
        mountedowner,
        mountedOper,
        getCommonFormParams,
        getOperation,
        mountedEquip,
        mountedLcmProdType,
        mountedRMAMatType,
        mountedRMAProductType,
        mountedRMAModelType,
        mountedRMAFabType,
        mountedRMA3Date,
        mountedRMAPartType,
        mountedRMAPart3Type,
        mounteEDCLineid,
        mounteEDCUnit,
        mountedLCMProductSize,
        mountedDefectCode,
        mountedLCDProduct,
        mountedExpectDefectCode,
        mountedLCMGrade,
        mountedLCDGrade,
        mountedcommonLCMGrade,
        mountedWorkOrderInformation,
        mountedHoldCode,
        mountedLCMowner,
        mountedMaterial,
    };
}
