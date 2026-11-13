<template>
    <div class="DayliMove">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="LCDPanelAtLCM">
                    <DynamicTable  showFavorite :PageParams="PageParams" row-key="id"
                        :header-title="PageSetting.Title" :columns="PageSetting.Columns" :key="PageSetting.Columns" sticky
                        :data-source="PageSetting.moreData" :data-all="PageSetting.moreData" :pagination="false"
                        :export-file-name="PageSetting.Title" :row-class-name="getRowClassNmae" :flexwidth="1">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="ts" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, unref } from 'vue';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
import { GetCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useRouter  } from 'vue-router';
import { getPovitData, PovitParams } from '@/utils/helper/povitHelper';
import {
    getTotalData,
    getSumData,
    mergeTableRow,
    getdiffdate,
    getAllSumData,
    getRowClassNmae,
    Inserttotalrow,
    getfinalsum,
    getrowsum
} from '@/pages/ReportPortal/utils/common';

const UseParamsAdd = AddCommonPageParams();
const { mounted, InfiniteScroll } = useRemoteSelect();
const router = useRouter();
const LCDPanelAtLCM = ref<HTMLElement | null>(null);
function getImport(site: any) {
    let { optionsdata } = require('@/pages/ReportPortal/FS/PanelWipStayChart/LCM1/form-schema');
    const { schemas } = require(`@/pages/ReportPortal/FS/LCDPanelAtLCM/${site}/form-schema`);
    const {
        LcdPanelAtLcmColumns,
        LCDProps
    } = require(`@/pages/ReportPortal/FS/LCDPanelAtLCM/${site}/Table`);
    return { schemas, optionsdata, LcdPanelAtLcmColumns, LCDProps };
}
let { schemas, optionsdata, LcdPanelAtLcmColumns, LCDProps } = getImport(UseParamsAdd.Fab);
let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [] as any,
    moreData: [],
    Title: ''
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(UseParamsAdd.Fab),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});
async function confirm(values) {
    PageSetting.APIColumns = getdiffdate(values.FromDate, values.ToDate);
    let DateColumns = PageSetting.APIColumns.map((item: string) => {
        return {
            title: item,
            align: 'center',
            dataIndex: item,
            customCell: (record: Object, rowIndex: number, column: Object) => {
                return {
                    onDblclick: () => {
                        UseParamsAdd.AddFirstTableParams({ ...record, ...column['__originColumn__'] });
                        switch(UseParamsAdd.Fab){
                            case "LCM1":
                                router.push({ name: 'LCDSTPNDetailLCM1PC' });
                                break;
                            case "LCM2":
                                router.push({ name: 'LCDSTPNDetailLCM2PC' });
                                break;
                        }
                        
                    },
                    style: {
                        cursor: 'pointer'
                    }
                };
            }
        };
    });

    let baseParams = { apiName: 'LCDSTPN', ReportType: '', FactoryType: UseParamsAdd.Fab, G_FAC: 'A' };
    let { api01 } = await GetCommon({ ...baseParams, ...values });
    let Data = api01['Data0']['Table'];
    let config: PovitParams = {
        Data,
        povitColumn: {
            PD: 'QTY'
        },
        changeColumn: PageSetting.APIColumns,
        repeatColumn: LCDProps
    };

    let povitData = getPovitData(unref(config)); //进行类行列转置的动作
    // 最後一行總計  
    let Totalsum = getfinalsum(cloneDeep(povitData), PageSetting.APIColumns, "LCD_PROD", "TOTAL");
    Inserttotalrow(povitData, ["LCM_PROD"], PageSetting.APIColumns, "LCM_OWNER", "小計");
    povitData.push(Totalsum);
    getrowsum(povitData, PageSetting.APIColumns, "Total");
    console.log("輸出轉化的數據", povitData);
    PageSetting.moreData = povitData;
    // 清除為0或者0(0)的數據
    let keyarr = Object.keys(PageSetting.moreData[0]);
    PageSetting.moreData.forEach((item, index) => {
        keyarr.forEach(kitem => {
            if ([0, '0'].includes(item[kitem])) {
                item[kitem] = "";
            }
        });
    });
    PageSetting.Columns = [
        ...LcdPanelAtLcmColumns,
        ...DateColumns,
        {
            title: '總計',
            align: 'center',
            dataIndex: 'Total'
        }
    ];
    /* if (PageSetting.moreData.length > 100) {
        PageSetting.List = PageSetting.moreData.slice(0, 40);
    } //预先加载40条数据
    else {
        PageSetting.List = PageSetting.moreData;
    } */
    PageSetting.Title = '(MA205) LCD廠PANEL於lcm廠投產分佈狀況 ';
    UseParamsAdd.AddFormParams({ ...baseParams, ...values });
    UseParamsAdd.AddAPIResultMessage(api01['Message']);
    console.log(PageSetting)

}

//实现无限滚动或者说懒加载
InfiniteScroll(LCDPanelAtLCM, PageSetting);
onMounted(async () => {
    //获取Size,bigproduct,prodcut数据
    // optionsdata = await mounted(optionsdata, UseParamsAdd.Fab);
});
</script>
