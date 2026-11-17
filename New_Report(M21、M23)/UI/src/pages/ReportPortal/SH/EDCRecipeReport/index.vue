<template>
    <div class="EDCRecipe">
        <a-card>
            <!-- <a-row>
                <span class="menu-fold" @click="showHidden(PageSetting)">
                    <component :is="!PageSetting.showmenu ? MenuUnfoldOutlined : MenuFoldOutlined" />
                </span>
            </a-row> -->
            <a-row>
                <a-col :span="24 - PageParams.TableSpan" class="SHScrollForm" v-show="PageSetting.showmenu">
                    <SchemaForm @submit="confirm"> </SchemaForm>
                </a-col>
                <a-col :span="PageParams.TableSpan">
                  <div class="ScrollTable" ref="EDCRecipe">
                    <DynamicTable sticky
                        showFavorite
                        row-key="id"
                        :header-title="PageSetting.Title"
                        :columns="PageSetting.Columns"
                        :key="PageSetting.Columns"
                        bordered
                        size="small"
                        :PageParams="PageParams"
                        :data-source="PageSetting.List"
                        :pagination="false"
                        export-file-name="导出"
                        :scroll="{ x: PageSetting.x,}"
                        :data-all="PageSetting.moreData"
                    >
                        <!-- <template #toolbar>
                                <a-button type="primary" @click="isShowExplain = !isShowExplain">{{isShowExplain?'显示表格':'显示说明文件'}}</a-button>
                            </template>  -->
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                  </div>
                </a-col>
            </a-row>
             <div hidden>{{ PageSetting.SQLHidden }}</div>
        </a-card>
    </div>
</template>


<script lang="tsx" setup>
import { MenuFoldOutlined, MenuUnfoldOutlined } from '@ant-design/icons-vue';
// import { schemas } from './form-schema';
import { useForm } from '@/components/core/schema-form';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, unref, ref, onMounted, watch, computed, Ref } from 'vue';
import { GetCommon } from '@/api/pages/common';
import { cloneDeep } from 'lodash-es';
import useRemoteSelect from '@/pages/ReportPortal/utils/SHuseRemoteSelect';
import { showHidden, getSumData ,handfloatprops,mergeTableRow} from '@/pages/ReportPortal/utils/SHcommon';
import { message } from 'ant-design-vue';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { useLoading } from '@/components/feat/Loading';
import {PageParams} from '@/components/core/dynamic-table/src/dynamic-table'
import { Console } from 'console';

const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

//let isShowExplain = ref<boolean>(false);
const useCommonParams = AddCommonPageParams();
let bigfab=useCommonParams.Fab.slice(0,useCommonParams.Fab.length-1)


const EDCRecipe = ref<HTMLElement | null>(null)
function getImport(site: any) {
    const { schemas } = require(`@/pages/ReportPortal/SH/EDCRecipeReport/${bigfab}/form-schema`);
    return { schemas };
}

let { schemas, } = getImport(useCommonParams.Fab);
let { InfiniteScroll,} = useRemoteSelect();

let PageSetting = reactive({
    List: [] as any,
    TableSpan: 20,
    showmenu: true,
    APIColumns: [] as any,
    Columns: [] as any,
    Title:"",
    moreData: [] as any,
    SQLHidden:'',
    scroll:{}as any,
    x: '' as any,
});



const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas:schemas(useCommonParams.Fab,reactive({})),
    actionColOptions: {
        span: 24
    },
    layout: 'vertical'
});

async function confirm(values) {   
      //table 初始化
    PageSetting.List=[];
    PageSetting.Columns=[];
    openFullLoading();
    let baseParams = { name: "Date",apiName: 'P3_EDCDataSet', FactoryType: useCommonParams.Fab, G_FAC:useCommonParams.G_FAC_COM, };
    
    if ((values.Equipment && values.Equipment.length > 0)&&(values.Unit && values.Unit.length > 0)) { 
            /**values为所设定的所有参数,将values里为undefined的值赋为空*/
            Object.keys(values).forEach((key) => {
                if (!values[key]) {
                    values[key] = '';
                }
            });

            let ChoosePara="";

            for (let i in dynamicFormRef?.formModel) {
                if (i == 'PanelId') values[i] = dynamicFormRef.formModel[i].split('\n');
                else { 
                    values[i] = dynamicFormRef.formModel[i]; 
                    if(i == 'Choose'){
                    ChoosePara=values[i];
                    } 
                    if(i == 'Item'&& ChoosePara=="EDC"){
                        //過濾ALL選項值
                    values[i] = dynamicFormRef.formModel[i]
                    values[i].map((val,j)=>{
                        if(val==="ALL"){
                            values[i].splice(j,1);
                        }
                        })
                    }else{
                    values[i] = dynamicFormRef.formModel[i];
                    }         
                    
                }
            }

            
            dynamicFormRef.validate().then(async () => {
            try{
                const { Date: { Data: { Table: Data },
                            Title,
                            SQL01,} }  = await GetCommon({ ...baseParams, ...values });
                
                //console.log("P3_EDCDataSet-SQLHidden:",SQL01);
                PageSetting.SQLHidden="P3_EDCDataSet-SQLHidden:"+SQL01;

                PageSetting.Title=`${useCommonParams.Fab} EDC Query Report`
                let arryTitle=Title.split(',')
                let ColumsData: any = [];
                let titlePars="";//label
                let itemPars="";//value
                let middleProps: string[] = []
                let pushdata:any = []; 
                
                    if (Data.length != 0) {  

                       if(ChoosePara==="EDC"){
                            //循環查詢data，獲取table Colums 數據 
                            let i=0;
                            for (let item in Data[0]) {                    
                            titlePars=arryTitle[i];
                            i++;
                            if(i<=3){
                               ColumsData.push({ title: titlePars, align: 'center', dataIndex: item, fixed:"left",className:"v-center",width:item=="GLASS_ID"?140:110 });
                            }else{
                               ColumsData.push({ title: titlePars, align: 'center', dataIndex: item,width:70  });
                            }
                            
                            }

                            PageSetting.Columns=ColumsData;
                            PageSetting.moreData=Data;

                       }else{
                          
                           //RCP
                           let OriginData = cloneDeep(Data);
                           ColumsData.push({  align: 'center', dataIndex: 'ITEM',fixed:"left",width:120,className:"v-center",});
                           
                           for(let j=0; j <OriginData.length; j++){
                               titlePars="Col"+j
                               middleProps.unshift(titlePars);
                               ColumsData.push({ align: 'center', dataIndex: titlePars, width:60 });
                             }

                             let i=0;
                             for (let item in Data[0]) {
                                itemPars=arryTitle[i];
                                let SubDadta=getRowToColData(OriginData, middleProps,item)
                                pushdata.splice(pushdata.length, 0, Object.assign(SubDadta, { ITEM: itemPars }));
                                i++;
                              }
                              
                              PageSetting.Columns=ColumsData;
                              PageSetting.moreData=pushdata;
                           }

                            PageSetting.x = null;
                            if (PageSetting.Columns.length > 7) {
                                PageSetting.x = 150 * PageSetting.Columns.length;
                            }

                        if (PageSetting.moreData.length > 100) {
                            PageSetting.List = PageSetting.moreData.slice(0, 40)
                            // isLongData.value = true
                        }//预先加载50条数据
                        else {
                            PageSetting.List = PageSetting.moreData
                            // isLongData.value = false
                        }
                    }else{

                        message.warning("無數據！")
                    }

                    closeFullLoading();
                    
                }catch(error: any){ 
                closeFullLoading();
                return Promise.reject(error); 
                }
                


        })
    }else{
            closeFullLoading();
            return message.error('線別or Unit為空，請選擇數據！');
    }
}


/**
 *  
 * @param data 原始的縂數據
 * @param colname 新表列名稱
 * @param dataIndex 需要轉置的列名
 * @returns 獲取轉置后數據
 */
 function getRowToColData(data: any[], colname: any[],dataIndex:string) {
    let DataObj = {};    
    let tempdata:any;
    let i=colname.length-1;
    for (let item of colname) {
           if(item!="ITEM"){
            tempdata = data[i][dataIndex];
            DataObj[item] = tempdata;
            i--;
           }
            
        }    
    return DataObj;
}

InfiniteScroll(EDCRecipe, PageSetting)
//載入api 方法
onMounted(async () => {
    // optionsdata.LCMEDCEquipmentLabel = await getCommonFormParamsNoAll('EQP_ID', {
    //         name: "Date",
    //         apiName: "EDCEquipment",
    //         FactoryType: useCommonParams.Fab,
    //         G_FAC: useCommonParams.G_FAC_COM,
    //         Choose:"EDC",
    //       })

})

</script>

