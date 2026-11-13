import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';

/**请不要在此文件中随意添加方法或函数，待owner确认后再决定是否添加 */

const UseParamsAdd = AddCommonPageParamsWithOut();

export async function GetLcdParamsData(optionsdata) {
    let apiParams = {
        G_FAC: UseParamsAdd.G_FAC_COM,
        FactoryType: UseParamsAdd.Fab,
        i_type: 2,
        apiName: 'LCDProduct'
    };
    let { api01 } = await GetCommon(apiParams);

    optionsdata.LCDSizeData = api01['Data']['Table']
        .filter((item) => item['SIZE1'] && item['SIZE1'] != '')
        .map((data) => {
            return { label: data['SIZE1'], value: data['SIZE1'] };
        });
    optionsdata.LCDProductData1 = optionsdata.LCDProductData = api01['Data1']['Table']
        .filter((item) => item['PART'] && item['PART'] != '')
        .map((data) => {
            return { label: data['PART'], value: data['PART'], SIZE1: data['SIZE1'] };
        });
    optionsdata.LCDProductData1.unshift({ label: 'ALL', value: 'ALL' });
    optionsdata.LCDSizeData.unshift({ label: 'ALL', value: 'ALL' });
}
export async function GetLCMProductSizeData(apiName: string) {
    let apiParams = {
        G_FAC: UseParamsAdd.G_FAC_COM,
        FactoryType: UseParamsAdd.Fab,
        apiName
    };
    let { api01 } = await GetCommon(apiParams);
    let Data = api01['Data']['Table']
        .filter((item: { [x: string]: string }) => item['SOPTION'] && item['SOPTION'] != ' ')
        .map((data: { [x: string]: any }) => {
            return { label: data['SOPTION'], value: data['SOPTION'] };
        });
    Data.unshift({ label: 'ALL', value: 'ALL' });
    return Data;
}