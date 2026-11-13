<template>
    <div >
        <a-card>
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                    <SchemaForm @submit="confirm" :showTextPosition="'center'" showUploadButton @upload="upload">
                    </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                    <div ref="Tableref" class="ScrollForm">
                        <DynamicTable sticky :PageParams="PageParams" showFavorite :header-title="PageSetting.Title"
                            :data-all="PageSetting.moreData" :columns="PageSetting.Columns" :key="PageSetting.Columns"
                            bordered :data-source="PageSetting.moreData" :pagination="{
                                pageSize: 50,
                                pageSizeOptions: ['50', '150', '300', '600']
                            }" export-file-name="导出" :row-class-name="PageSetting.Title">
                            <template #export-button>
                                <a-button type="success"> 导出 </a-button>
                            </template>
                        </DynamicTable>
                    </div>
                </a-col>
                <UploadExcell :visible="visible" @closemodal="closemodal" @uploadfile="uploadfile"></UploadExcell>
            </a-row>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, onMounted, ref, h } from 'vue';
import { GetCommon, UploadCommon } from '@/api/pages/common';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import { cloneDeep } from 'lodash';
import dayjs from 'dayjs';
import UploadExcell from '@/components/upload/UploadCommon.vue';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { ReportCommon } from '@/store/module/ReportCommon';
import { message, Popconfirm, TypographyLink } from 'ant-design-vue';
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

let useReportCommon = ReportCommon();
let useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)

let ReportNameParams=`EntityStatusSummaryTactTimeSetup-${useCommonParams.Fab}`;
let formParams= useReportCommon[ReportNameParams].FormParams;

const UseParamsAdd = AddCommonPageParams();
function getImport(site: any) {
    const {
        schemasdetail, 
    } = require(`@/pages/ReportPortal/SH/EntityStatusSummaryTactTimeSetup/${bigfab}/form-schema`);
    const { detailbaseColumns } = require(`@/pages/ReportPortal/SH/EntityStatusSummaryTactTimeSetup/${bigfab}/Table`);
    return { schemasdetail,  detailbaseColumns };
}
let { schemasdetail,  detailbaseColumns } =getImport(useCommonParams.Fab);

let PageSetting = reactive<BasePageParams.BasePageSetting>({
    List: [],
    APIColumns: [],
    Columns: [],
    moreData: []
});
const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemasdetail(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});


//  function confirm(values) {
//     openFullLoading();
//     formParams= useReportCommon[ReportNameParams].FormParams;
//     let baseParams = {
//         apiName: 'EntityStaTactTimeMaintenance',
//         FactoryType: formParams["FactoryType"],
//         G_FAC:formParams["G_FAC"],
//     };
    
//     Object.keys(values).forEach((key) => {
//         if (!values[key]) {
//             values[key] = '';
//         }
//     });
//     try {
//         if (values["action"] == "Delete") {
//            GetData(values["action"],values,baseParams)

//            GetData("Query",values,baseParams)

//         }else if (values["action"] == "Query") {
//             GetData(values["action"],values,baseParams)
//         }

//         closeFullLoading();
//     } catch (error){
//         closeFullLoading();
//     }
// }

// async function GetData(QueryType,Formvalues,Basevalues){
//     let Func={Function:QueryType}
//     let { api01 } = await GetCommon({ ...Formvalues, ...Basevalues,...Func});
//     if (QueryType == "Delete") {
//             let Message = api01;
//             if (Message.includes("Success")) {
//                 message.success("資料刪除成功！")
//             } else {
//                 message.error("資料刪除失敗！")
//             }
//         } else if (QueryType == "Query") {  
//             debugger;        
//             console.log("api01",api01)  
//             PageSetting.Title = `${formParams["FactoryType"]} Entity Status Summary Tact Time Setup`;
//             PageSetting.Columns = detailbaseColumns;
//             PageSetting.moreData = api01['Data']['Table'];
//             if (PageSetting.moreData.length == 0) {
//                 closeFullLoading();
//                 return message.warn('無數據!');
//             }
//         }
// }

async function confirm(values) {
    openFullLoading();
    formParams= useReportCommon[ReportNameParams].FormParams;
    // debugger;
    let baseParams = {
        apiName: 'EntityStaTactTimeMaintenance',
        FactoryType: formParams["FactoryType"],
        G_FAC:formParams["G_FAC"],
        Function:values["action"],
    };
    Object.keys(values).forEach((key) => {
        if (!values[key]) {
            values[key] = '';
        }
    });
    try {
        let { api01 } = await GetCommon({ ...values, ...baseParams });
        if (values["action"] == "Delete") {
            let Message = api01;
            if (Message.includes("Success")) {
                message.success("資料刪除成功！5秒后查詢確認")                
            } else {
                message.error("資料刪除失敗！")
            }
        } else if (values["action"] == "Query") {
            PageSetting.Title = `${formParams["FactoryType"]} Entity Status Summary Tact Time Setup`;
            PageSetting.Columns = detailbaseColumns;
            PageSetting.moreData = api01['Data']['Table'];
            if (PageSetting.moreData.length == 0) {
                closeFullLoading();
                return message.warn('無數據!');
            }
        }

        closeFullLoading();
    } catch (error){
        closeFullLoading();
    }
}



// 打開文件上傳彈窗按鈕
let visible = ref(false);
let Months = "";
function upload(values) {
    // 獲取月份
    visible.value = true;
    Months = values["Month"];
    console.log("文件上傳獲取的參數1", values);

}
// 關閉文件上傳彈窗
function closemodal() {
    visible.value = false;
}

const createElementVNode = (content: any) => {
    // 定义文本内容及样式（这里的样式延用的官方的）
    const innerText = h('span', { style: { color: '#262626', fontSize: '14px' } }, content);
    // 自定义关闭图标
    const innerIcon = h('span', {
        style: { marginLeft: '10px', color: '#000', cursor: 'pointer', 'font-size': '18px' },
        class: "removes",
        attrs: { type: 'close', title: "关闭" },
        onClick: () => {
            message.destroy()
        }
    }, "x");
    // 合到一个 VNode 节点中
    const container = h('span', { style: {} }, [innerText, innerIcon]);
    // 最终返回
    return container;
};


// 獲取文件上傳列表
async function uploadfile(fileList, formData) {
    console.log("輸出fileList", fileList);
    formParams= useReportCommon[ReportNameParams].FormParams;

    let baseParams = {
        apiName: 'EntityStaTactTimeMaintenance',
        FactoryType: formParams["FactoryType"],
        G_FAC:formParams["G_FAC"],
        Function:"Import",
        Month:Months,
    };
    Object.keys(baseParams).forEach(key => {
        formData.append(key, baseParams[key])
    })
    console.log("輸出formData", formData);
    try {
       
        let Message = await UploadCommon(formData);
        if (Message.includes("Success") ) {
            message.success(createElementVNode("資料上傳成功！5秒后查詢確認"));
            // if (Data.length != 0) {
            //     PageSetting.moreData = Data;
            //     PageSetting.Columns = Object.keys(PageSetting.moreData[0]).map(item => {
            //         return {
            //             title: item,
            //             align: 'center',
            //             dataIndex: item,
            //         }
            //     })
            // }
        } else {
            message.error(createElementVNode("資料上傳失敗！"))
        }
    } catch (error: any) {
        
        message.error(error)
    }


}


</script>

