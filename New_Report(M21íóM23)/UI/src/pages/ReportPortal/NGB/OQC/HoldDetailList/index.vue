<!--
 * @Author: snake19840
 * @Date: 2023-01-03 13:55:17
 * @LastEditors: snake19840
 * @LastEditTime: 2023-01-05 15:27:07
 * @Description: 
 * 
 * Copyright (c) 2023 by snake19840/www.snake19840.top, All Rights Reserved. 
-->

<template>
    <div>
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col
                    :span="24 - PageParams.TableSpan"
                    class="PanelWipStayChartForm"
                    Source-show="PageSetting.showmenu"
                >
                    <div class="NGBScrollForm">
                        <SchemaForm @submit="confirm"> </SchemaForm>
                    </div>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div class="ScrollTable" ref="HoldDetailListTable">
                        <DynamicTable sticky
                            showFavorite
                            EditExplain
                            row-key="id"
                            :header-title="PageSetting.HeadTitle"
                            :columns="PageSetting.Columns"
                            :key="PageSetting.Columns"
                            bordered
                            size="small" :PageParams="PageParams"
                            :data-source="PageSetting.List"
                            :data-all="PageSetting.moreData"
                            :pagination="false"
                            export-file-name="导出"
                            :row-class-name="getRowClassName"
                            :scroll="{ x: 900, y: 700 }"
                        >
                            <template #toolbar>
                                <!-- <a-button type="primary" @click="isShowExplain = !isShowExplain">{{
                                    isShowExplain ? '显示表格' : '显示说明文件'
                                }}</a-button> -->
                            </template>
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <div hidden>{{ PageSetting.SQLHidden }}</div>
            </a-row>
        </a-card>
    </div>
</template>

<script setup lang="ts" >
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash-es';
import {
    showHidden,
    getRowClassName,
    getSumData,
    mergeTableRow,
    getdiffdate
} from '@/pages/ReportPortal/utils/NGBcommon';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { message } from 'ant-design-vue';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'

let useCommonParams = AddCommonPageParams();
const DaylimoveTable = ref<HTMLElement | null>(null);
//vue3的mixin

function getImport(site: any) {
    //let { optionsdata } = require('@/pages/ReportPortal/Static/NGBCommon-Form-schema');
    const { schemas } = require(`@/pages/ReportPortal/NGB/OQC/HoldDetailList/LCM/form-schema`);
    const { baseColumns } = require(`@/pages/ReportPortal/NGB/OQC/HoldDetailList/LCM/Table`);
    return { schemas, baseColumns,};
}

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//let isShowExplain = ref<boolean>(false);
//
let repeatData = ['WORK_CTR', 'WO_NBR', 'LCM_OWNER', 'SIZE1', 'PROD_NBR', 'CUST_GRP', 'SO_NBR'];

let { schemas, baseColumns,  } = getImport(useCommonParams.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    TableSpan: 20,
    showmenu: true,
    APIColumns: [],
    Columns: [],
    moreData: []
});

const [DynamicTable] = useTable();

const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 130,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 18
    },
    layout: 'vertical'
});

/**
 * 提交表单,验证通过后进行数据获取与处理
 * @dynamicFormRef 表单的所有参数与属性
 * @ParamsData API接收的参数
 */
function confirm(values) {
    //table 初始化
    PageSetting.List = [];
    PageSetting.Columns = [];
    //加載防護罩開始
    openFullLoading();

    //基礎query參數條件值

    let baseParams = {
        name: 'Date',
        apiName: 'HoldDetail',
        FactoryType: useCommonParams.Fab,
        G_FAC: useCommonParams.G_FAC_COM
    };

    // if (!values[key]) {
    //             values[key] = '';
    //         }

    //獲取query頁面的條件值
    Object.keys(values).forEach((key) => {
        console.log(key, values[key]);

        if (key == 'MessageType' && values[key] == 'Panel_id') {
            values['pnl_id'] = !values['TypeVal'] ? '' : values['TypeVal'];
        } else if (key == 'MessageType' && values[key] == 'Cart_id') {
            values['cart_id'] = !values['TypeVal'] ? '' : values['TypeVal'];
        } else if (key == 'MessageType' && values[key] == 'Cart_ID_cust') {
            values['cart_id_cust'] = !values['TypeVal'] ? '' : values['TypeVal'];
        } else if (key == 'MessageType' && values[key] == 'hold_code') {
            values['dfct_code'] = !values['TypeVal'] ? '' : values['TypeVal'];
        } else if (key == 'ShowType') {
            values['show_type'] = !values[key] ? '' : values[key];
        } else if (key == 'Prod_nbr') {
            values['prod_nbr'] = !values[key] ? '' : values[key];
        } else if (key == 'Floor') {
            values['floor'] = !values[key] ? '' : values[key];
        }else if (key == 'CompleteFromDate') {
            values['FromDate'] = !values[key] ? '' : values[key];
        }else if (key == 'CompleteToDate') {
            values['ToDate'] = !values[key] ? '' : values[key];
        }
    });
    values['fab'] = useCommonParams.Fab;

    for (let i in dynamicFormRef?.formModel) {
        if (i == 'Wo_nbr') values[i] = dynamicFormRef.formModel[i].split('\n');
        else {
            values[i] = dynamicFormRef.formModel[i];
        }
    }

    if (values.Prod_nbr && values.Prod_nbr.length > 1000 )
    {
         closeFullLoading();
         return message.error('料號输入超過 “最大限制筆數:1000” '); 
    }

    //獲取數據綁定table
    dynamicFormRef.validate().then(async () => {
        try {
            const {
                Date: {
                    Data: { Table: Data },
                    sql
                }
            } = await GetCommon({ ...baseParams, ...values });

            PageSetting.SQLHidden = 'HoldDetailList_DataSet-SQLHidden:' + sql;
            //console.log("TDownGrade_DataSet-SQLHidden:",SQL01)
            // let isLongData = ref(true)
            PageSetting.Title = `${useCommonParams.Fab} Hold Detail List`;

            if (Data.length != 0) {
                //循環查詢data，獲取table Colums 數據
                let ColumsData: any = [];
                let WidthPars = 50;
                let titlePars = ''; //label
                let classNamePars = 'v-center';
                let middleProps: string[] = []; //API提供轉置后的時間處理（例如：A2022_11_30）

                // for (let item in Data[0]) {
                //     titlePars = item;
                //     classNamePars = 'pro_auto-center';
                //     ColumsData.push({
                //         title: titlePars,
                //         align: 'center',
                //         dataIndex: item,
                //         className: classNamePars,
                //         width: WidthPars
                //     });
                // }
                //為加總提供要加總 計算的Columns
                // PageSetting.APIColumns = [];
                // PageSetting.APIColumns = middleProps;
                Data.forEach((d) => {
                    d['STATION'] = '1760';
                });

                PageSetting.Columns = [...baseColumns]; //vue3或者adv无法监测push,shift等方法
                PageSetting.moreData = Data;
                if (PageSetting.moreData.length > 100) PageSetting.List = PageSetting.moreData.slice(0, 40);
                else PageSetting.List = PageSetting.moreData;
            } else {
                message.warning('無數據！');
            }
            //加載防護罩結束
            closeFullLoading();
        } catch (error) {
            closeFullLoading();
            return Promise.reject(error);
        }
    });
}

onMounted(async () => {
    // await mounted(optionsdata, useCommonParams.Fab);
    // await mountedLCMowner(optionsdata, useCommonParams.Fab);
    // await mountedLCDowner(optionsdata, useCommonParams.Fab);
    // await mountedLCDGrade(optionsdata, useCommonParams.Fab);
    // await mountedModuleData(optionsdata, useCommonParams.Fab);
    // await mountedSourceFab(optionsdata, useCommonParams.Fab);
    // await mountedWoTypeBySap(optionsdata, useCommonParams.Fab);

    // optionsdata.LcdOwnerLabel = await  getCommonFormParams('LCDOWNER',{
    //     name: "Date",
    //     apiName: "LcdOwner",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })

    // optionsdata.ModuleData = await getCommonFormParams('PROD_TYPE',{
    //     name: "Date",
    //     apiName: "Module",
    //     FactoryType:useCommonParams.Fab,
    //     G_FAC: useCommonParams.G_FAC_COM
    // })
});
</script>

<style scoped>
</style>