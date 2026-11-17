<template>
    <div class="PanelWIPHistoryQuery">
        <a-card>

        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <DynamicTable row-key="id" header-title="LCMA Panel history Query " showFavorite sticky
                 :PageParams="PageParams" :columns="PageSetting.TopColumns"
                    :key="PageSetting.TopColumns" bordered size="small" :data-source="PageSetting.TopList"
                    :data-all="PageSetting.TopList"
                    :flexwidth="1"
                    :pagination="false"  export-file-name="导出">
                    <template #export-button>
                        <a-button type="success"> 导出 </a-button>
                    </template>
                </DynamicTable>
            </a-col>

        </a-row>

        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table'
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
// import { cloneDeep } from 'lodash-es';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { cloneDeep } from 'lodash';
import { AddCommonPageParams } from '@/store/module/commonpage';
const UseParamsAdd = AddCommonPageParams();

let FactoryType: string = UseParamsAdd.Fab;




function getImport(site: any) {
    const { schemas, optionsdata, getSizeData, getOperaction, OperationLabel } = require(`@/pages/ReportPortal/${site}/PanelWIPHistoryQuery/LCM1/form-schema`);
    return { schemas, optionsdata, getSizeData, getOperaction, OperationLabel };
}
const { schemas, optionsdata, getSizeData, getOperaction, OperationLabel } = getImport('FS');



// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    ALLparams: {},

});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: "vertical"
});

async function confirm(values) {
    let TopTableParams = {
        name: "Date",
        apiName: "PanelHisDataSet",
        FactoryType: "LCM1",
        G_FAC: "A",
        G_CLASS: "",
        vDate_s: "",
        vDate_e: "",
        Prod_nbr: ["ALL"],
        Oper: [],
        query_type: 1,
        query_func: "",
        pnl_ids: [] as any,
    }
    //給ParamsData賦值 
    for (let i in values) {
        if (i == "pnl_ids") {
            if (values[i] == undefined) {
                values[i] = [];
            } else {
                // values[i] = values[i].trim().split(',');
                values[i] = values[i].split(/[\r\n\t]/).filter(item => item)
            }
        } /* else if (i == "BigProduct") {
            // values["Product"] = values[i].trim().split(',');
            values["Product"] = values[i].split(/[\r\n\t]/).filter(item => item)
        } else if (i == "Product") {
            // values["Prod_nbr"] = values[i].trim().split(',');
            values["Prod_nbr"] = values[i].split(/[\r\n\t]/).filter(item => item)
        } */
    }
    /* let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef["formModel"])) {
        if (tempkeys.includes(i)) {
            if (i == 'pnl_ids') {
                TopTableParams[i] = dynamicFormRef.formModel[i].trim().split(',');
            } else {
                TopTableParams[i] = cloneDeep(dynamicFormRef["formModel"][i]);
            }

        }
    } */
    TopTableParams = { ...TopTableParams, ...values };
    console.log("輸出TopTableParams", TopTableParams);


    dynamicFormRef.validate().then(async () => {
        if (TopTableParams["query_func"] == "panel" && [1, 2].includes(TopTableParams["query_type"]) && TopTableParams["pnl_ids"].length == 0) {
            return message.warning('Please input pnl_id!');
        } else if (TopTableParams["query_func"] == "prod") {
            if (TopTableParams["query_type"] == 1 && TopTableParams["pnl_ids"].length == 0) {

                return message.warning('Please input pnl_id!')
            } else if (TopTableParams["query_type"] == 2) {
                if (TopTableParams["pnl_ids"].length == 0) {
                    return message.warning('Please input pnl_id!')
                } else if (TopTableParams["Prod_nbr"].length == 0) {
                    return message.warning("Please select product!")
                } else if (TopTableParams["Oper"].length == 0) {
                    message.warning("Please select operation!")

                }
            }
        } /* else { */
        const { Date: { Data: { Table: Data }, Message: sqlmessage } } = await GetCommon(TopTableParams);
        if (Data.length == 0) {
            return message.warn("無數據！");
        }
        PageSetting.TopList = Data;
        // 选了wip
        // let wipLabels = ["fac_id", "panel_id", "lcm_prod", "oper", "last_trans_date", "equip_nbr", "lcm_owner", "cassette_id", "lcm_grade", "hold_flag", "ship_date", "comments", "def_com", "lcd_grade", "lcd_fab", "lcd_abn_lcd abn_flag", "LCD Bright Flag"]
        let wipLabels = ["fac_id", "panel_id", "lcm_prod", "oper", "last_trans_date", "equip_nbr", "lcm_owner", "cassette_id", "lcm_grade", "hold_flag", "ship_date", "comments", "def_com", "lcd_grade", "lcd_fab", "lcd_abn_lcd abn_flag",]
        let wipProps = ["FAC_ID", "PANELID", "LCM_PROD", "CURRENT_OPER", "DATETIME", "LAST_EQP_NBR", "LCM_OWNER", "CASSETTE_ID", "LCM_GRADE", "HOLD_FLAG", "SHIP_DATE", "COMMENTS", "DEF_COM", "LCD_GRADE", "SOURCE_FAB", "ABN_FLAG"]
        // 选了his
        // let hisLabel = ["fac_id", "panel_id", "prod_nbr", "oper", "trans_date", "equip_nbr", "lcm_owner", "operator", "cassette_id", "grade", "comments", "def_com", "trans_type", "lcd_grade", "lcd_fab"]
        let hisLabel = ["fac_id", "panel_id", "prod_nbr", "oper", "trans_date", "equip_nbr", "lcm_owner", "operator", "cassette_id", "grade", "comments", "def_com", "trans_type", "lcd_grade"]
        // let hisProps = ["FAC_ID", "PNL_ID", "PROD_NBR", "OPER", "TRANS_DATE", "EQUIP_NBR", "LCM_OWNER", "OPERATORS", "CST_ID", "GRADE", "COMMENTS", "DEF_COM", "TRANS_TYPE", "LCD_GRADE", "SOURCE_FAB"];
        let hisProps = ["FAC_ID", "PNL_ID", "PROD_NBR", "OPER", "TRANS_DATE", "EQUIP_NBR", "LCM_OWNER", "OPERATORS", "CST_ID", "GRADE", "COMMENTS", "DEF_COM", "TRANS_TYPE", "LCD_GRADE"];
        if (TopTableParams["query_type"] == 1) {
            PageSetting.TopColumns = TableCustomCell(wipLabels, wipProps);
        } else if (TopTableParams["query_type"] == 2) {
            PageSetting.TopColumns = TableCustomCell(hisLabel, hisProps);
        }
        // }




    })


}
function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = []) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            //@ts-ignore
        }
    })

}

onMounted(async () => {
   /*  await getOperaction("LCM1");
    optionsdata.SizeData = await getSizeData(GetCommon, 'SIZE1', "Size", FactoryType);
    optionsdata.BigProductData1 = optionsdata.BigProductData = await getSizeData(GetCommon, 'PART', "BigProduct", FactoryType);
    optionsdata.ProductData1 = optionsdata.ProductData = await getSizeData(GetCommon, 'SOPTION', "Product", FactoryType); */

})

</script>


