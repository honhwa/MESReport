<template>
    <div class="StartBankStatusWip">
        <a-row>
            <a-col :span="24 - PageParams.TableSpan" class="ScrollForm" v-show="PageParams.ShowMenu">
                <SchemaForm @submit="confirm"> </SchemaForm>
                <span style="color: red;text-align: center;" v-if="FactoryType == 'BEOL2'">(BEOL2 原二階DB已取消，BEOL2無二階)</span>
            </a-col>
            <a-col :span="PageParams.TableSpan">
                <div ref="reportref">
                    <DynamicTable id="StartBankStatusWip" row-key="reportref" header-title="LCMA Start Bank Status Wip"
                        showFavorite :PageParams="PageParams" :columns="PageSetting.TopColumns"
                        :key="PageSetting.TopColumns" bordered size="small" :data-source="PageSetting.moreData"
                        :data-all="PageSetting.moreData" :pagination="false" export-type="Exceljs" sticky :flexwidth="1"
                         export-file-name="导出" :row-class-name="getRowClassNmae">
                        <template #export-button>
                            <a-button type="success"> 导出 </a-button>
                        </template>
                    </DynamicTable>
                </div>
            </a-col>
        </a-row>
    </div>
</template>

<script lang="tsx" setup>
import { PageParams } from '@/components/core/dynamic-table/src/dynamic-table';
import { useTable } from '@/components/core/dynamic-table';
import { reactive, ref } from '@vue/reactivity';
import { useForm } from '@/components/core/schema-form';
import { GetCommon } from '@/api/pages/common';
import { onMounted } from '@vue/runtime-core';
import { message } from 'ant-design-vue';
import { getfinalsum, getrowsum, } from '@/utils/reportcommon/Calculation';
import { cloneDeep } from 'lodash';
import { getPovitData, PovitParams } from '@/utils/reportcommon/provit';
import { getRowClassNmae, megercell,Inserttotalrow } from '@/pages/ReportPortal/utils/common';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';
import { useRouter } from 'vue-router';
import { useLoading } from '@/components/feat/Loading';
import useRemoteSelect from '@/pages/ReportPortal/utils/useRemoteSelect';
const router = useRouter();
const [openFullLoading, closeFullLoading] = useLoading({
    tip: '數據加載中...'
});

const reportref = ref<HTMLElement | null>(null);

const UseParamsAdd = ReportCommonWithOut();
let FactoryType: any = UseParamsAdd.Fab;
function getImport(site: any) {
    const {
        schemas,
        optionsdata
    } = require(`@/pages/ReportPortal/FS/StartBankStatusWip/${site}/form-schema`);
    const {
        baseColumns,
        abncolumns,
        sumcolumns
    } = require(`@/pages/ReportPortal/FS/StartBankStatusWip/${site}/Table`);
    return { schemas, optionsdata, baseColumns, abncolumns, sumcolumns };
}
const { schemas, optionsdata, baseColumns, abncolumns, sumcolumns } = getImport(FactoryType);

const {  mountedLCDGrade, InfiniteScroll, mountedLCMowner } = useRemoteSelect();

// 頁面數據
let PageSetting = reactive({
    TopList: [] as any,
    TopColumns: [] as any,
    Message: {},
    moreData: [] as any,
    exceljsparams: {
        Header: {
            key: [] as any[],
            title: [] as any[],
        },
        MutiHeader:[
            
        ] as any[]
    }
});

const [DynamicTable] = useTable();
const [SchemaForm, dynamicFormRef] = useForm({
    labelWidth: 120,
    schemas: schemas(FactoryType),
    actionColOptions: {
        span: 24
    },
    fieldMapToTime: [['fieldTime', ['vDate_s', 'vDate_e1'], 'YYYY-MM-DD']],
    layout: 'vertical'
});

async function confirm() {
    openFullLoading();
    let TopTableParams = {
        name: 'Date',
        apiName: 'StartBankStatusDataSet',
        FactoryType: FactoryType,
        G_FAC: 'A',
        vDate_e: '',
        owner_code: [],
        EntyGroup: [],
        grade_code: [],
        show_abn: '',
        show_lcd: '',
        show_his: '',
        STK: ''
    };
    //給ParamsData賦值
    let tempkeys = Object.keys(TopTableParams);
    for (let i in cloneDeep(dynamicFormRef.formModel)) {
        if (tempkeys.includes(i)) {
            TopTableParams[i] = cloneDeep(dynamicFormRef.formModel[i]);
        } else if (i == "Product") {
            TopTableParams["EntyGroup"] = cloneDeep(dynamicFormRef.formModel[i]);
        }
    }
    let TopTableLeftLabels: string[] = ['Size', 'Prod(PFCD/SKD)-CST首碼', 'Grade'];
    let TopTableLeftProps: string[] = ['SUB', 'LCD_PROD', 'GRADE'];

    let tempColumns = [...baseColumns];
    // 如果顯示Abnormal code:yes
    if (TopTableParams['show_abn'] == 'Y') {
        TopTableLeftLabels.push('ABN Code');
        TopTableLeftProps.push('ABN_CODE');
        tempColumns = [...tempColumns, ...abncolumns];
    }
    try {
        const {
            Date: {
                Data: { Table: Data },
                Data1: { Table: Data1 },
                Message: sqlmessage
            }
        } = await GetCommon(TopTableParams);
        if (Data.length != 0) {
            PageSetting.TopList = Data;
            PageSetting.Message = sqlmessage;
            let middleProps: string[] = [];
            let middleLabels: string[] = [];
            let tempmiddleProps: Object[] = [];
            Data1.forEach((item) => {
                middleLabels.push(`${item['LCD_OWNER']} ${item['WORK_CTR']}`);
                let titem = `${item['LCD_OWNER']} ${item['WORK_CTR']}`;
                tempmiddleProps.push({
                    title: item['LCD_OWNER'],
                    children: [
                        {
                            title: item['WORK_CTR'],
                            align: 'center',
                            dataIndex: `${item['LCD_OWNER']} ${item['WORK_CTR']}`,
                            key: `${item['LCD_OWNER']} ${item['WORK_CTR']}`,
                            customCell:
                                FactoryType == 'BEOL2'
                                    ? null
                                    : (record: Object, rowindex: number, column: Object) => {
                                        return {
                                            onDblclick: async () => {
                                                if (!isNaN(record[titem])) {
                                                    bottomParams['grade'] = record['GRADE'];
                                                    bottomParams['work_ctr'] = titem.split(' ')[1];
                                                    bottomParams['owner'] = item['LCD_OWNER'];
                                                    bottomParams['panel_pn'] = record['LCD_PROD'];
                                                    bottomParams['show_abn'] = dynamicFormRef.formModel['show_abn'];
                                                    bottomParams['StrSql_w'] = PageSetting.Message['StrSql_w'];
                                                    bottomParams['size'] = dynamicFormRef.formModel['size'];
                                                    if (dynamicFormRef.formModel['show_abn'] == 'Y') {
                                                        bottomParams['abn_code'] = record['ABN_CODE'];
                                                    }
                                                    await getbuttomtable(bottomParams);
                                                }
                                            },
                                            style: { cursor: 'pointer' }
                                        };
                                    }
                        }
                    ]
                });
            });
            tempColumns = [...tempColumns, ...tempmiddleProps];
            middleProps = middleLabels;
            let config: PovitParams = {
                Data: PageSetting['TopList'],
                povitColumn: {
                    'OWNER CTR': 'COUNT'
                },
                changeColumn: middleProps,
                repeatColumn: TopTableLeftProps
            };
            if (TopTableParams['show_abn'] == 'Y') {
                config['repeatColumn'] = TopTableLeftProps.concat(middleProps);
            }
            PageSetting['TopList'] = getPovitData(config);
            // 添加行总计
            getrowsum(PageSetting.TopList, middleProps, 'Sum');
            let tempsum = getfinalsum(PageSetting.TopList, middleProps.concat(['Sum']), 'GRADE');

             // 添加插入行小計
             let { insertIndexList, insertrowList } = Inserttotalrow(
                PageSetting.TopList,
                ['SUB'],
                middleProps.concat(['Sum']),
                'GRADE',
                "小計"
            );
           

           

           
            PageSetting.TopList.push(tempsum);
            megercell(baseColumns, ['SUB', 'LCD_PROD', 'GRADE',"ERP_PROD_ID"], PageSetting.TopList);
            tempColumns = [...tempColumns, ...sumcolumns];
            PageSetting.TopColumns = tempColumns;
            console.log("输出",PageSetting.TopColumns );
            
            PageSetting.moreData = PageSetting.TopList;
             PageSetting.exceljsparams.Header.key = [];
             PageSetting.exceljsparams.Header.title = [];
             let MutiHeaderobj:any = [];
             PageSetting.TopColumns.forEach(item=>{
                PageSetting.exceljsparams.Header.key.push(item.children  ? item.children[0]["dataIndex"] : item.dataIndex)
                PageSetting.exceljsparams.Header.title.push(item.title)
                MutiHeaderobj.push(item.children  ? item.children[0]["title"] : item.title);

             })
             PageSetting.exceljsparams.MutiHeader.push(MutiHeaderobj)
            if (PageSetting.moreData.length > 20) {
                //预先加载40条数据
                PageSetting.TopList = PageSetting.moreData.slice(0, 40);
            } else {
                PageSetting.TopList = PageSetting.moreData;
            }
            closeFullLoading();
        } else {
            closeFullLoading();
            message.warn('无数据！');
        }
    } catch (error) {
        closeFullLoading();
        console.log(error);
    }
}

// 需要Prod(PFCD/SKD)-CST首碼，Grade,Prod 990这些
let bottomParams = {
    name: 'Date',
    apiName: 'StartBankStatusDetail',
    FactoryType: FactoryType,
    G_FAC: 'A',
    panel_pn: '',
    size: '',
    abn_code: '',
    grade: '',
    owner: '',
    work_ctr: '',
    show_abn: '',
    StrSql_w: ''
};
// 添加合并列功能
function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [], ToOtherpage: Boolean = false) {
    return Prop.map((item: string, index: number) => {
        return {
            title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index], //可点击的栏位显示*号
            align: 'center' as any,
            dataIndex: item,
            //@ts-ignore
            customCell: (record: Object, rowindex: number, column: Object) => {
                if (ClickColumn.includes(item)) {
                    return {
                        onDblclick: async () => {
                            // 新页面打开
                            if (ToOtherpage) {
                               
                            } else {
                                if (!isNaN(record[item])) {
                                    console.log("輸出record",record);
                                    
                                    bottomParams['grade'] = record['GRADE'];
                                    bottomParams['work_ctr'] = item.split(' ')[1];
                                    bottomParams['owner'] = record['OWNER'];
                                    bottomParams['panel_pn'] = record['LCD_PROD'];
                                    bottomParams['show_abn'] = dynamicFormRef.formModel['show_abn'];
                                    bottomParams['StrSql_w'] = PageSetting.Message['StrSql_w'];
                                    bottomParams['size'] = dynamicFormRef.formModel['size'];

                                    if (dynamicFormRef.formModel['show_abn'] == 'Y') {
                                        bottomParams['abn_code'] = record['ABN_CODE'];
                                    }
                                    // 当前页面显示下面的表格
                                    // 请求下表格数据
                                    await getbuttomtable(bottomParams);
                                }
                            }
                        },
                        style: { cursor: 'pointer' }
                    };
                } else {
                    return {
                        onDblclick: () => {
                            message.warning('此栏位没有二阶数据');
                        },
                        style: {}
                    };
                }
            }
        };
    });
}

// 表格2数据请求以及处理
async function getbuttomtable(bottomParams: Object) {
    let BottomLabels = [
        'LCD Product',
        'LCD Owner',
        'LCD Grade_Grp',
        'LCD_ABN_LCD',
        'CST_ID',
        'CST_QTY',
        'Status',
        'Stocker_ID',
        'ERP_LCD_PROD',
        'LCD_Ship_Date',
        'LCM_In_Date',
        'CST_Set_CD',
        'Q_time',
        '實驗者',
        '實驗單號',
        'comments'
    ];
    let BottomProps = [
        'PANEL_PN',
        'LCD_OWNER',
        'LCD_GRADE_GRP',
        'LCD_ABN_LCD',
        'CST_ID',
        'CST_QTY',
        'STATUS',
        'STOCKER_ID',
        'ERP_LCD_PROD_ID',
        'LCD_SHIP_DATE',
        'IN_DATE',
        'CST_SET_CD',
        'Q_TIME',
        'RESERVED_11',
        'RESERVED_12',
        'COMMENT1'
    ];
    const bottomBaseColumns = TableCustomCell(BottomLabels, BottomProps);
    if (FactoryType == 'BEOL2') {
        message.warn('BEOL2 原二階DB已取消，BEOL2無二階');
    } else {
        UseParamsAdd.AddFirstTableParams('StartBankStatus' + FactoryType, {
            Columns: bottomBaseColumns,
            apiParams: bottomParams
        });
        router.push({ name: 'StartBankStatusWipDetail' + FactoryType });
    }
}
InfiniteScroll(reportref, PageSetting as any);

onMounted(async () => {
    const {
        Date: {
            Data: { Table: Data }
        }
    } = await GetCommon({
        name: 'Date',
        apiName: 'StartBankStatusWip_Size',
        G_FAC: 'A',
        FactoryType: FactoryType
    });
    let SizeData: object[] = [];
    Data.forEach((item) => {
        SizeData.push({
            label: item['SIZE1'],
            value: item['SIZE1']
        });
    });
    SizeData.unshift({
        label: 'ALL',
        value: 'ALL'
    });
    optionsdata.SizeData = SizeData;
    let {
        Date: { Table: ProductData }
    } = await GetCommon({
        name: 'Date',
        apiName: 'StartBankStatusWip_Product',
        G_FAC: 'A',
        Size: '',
        FactoryType: FactoryType
    });
    // 不经过处理的数据
    let nohandleData = cloneDeep(ProductData);
    // 为ALL的时候的数据
    let ALLproduct: object[] = [];
    ProductData.forEach((item) => {
        ALLproduct.push({
            label: item['PART'],
            value: item['PART']
        });
    });
    optionsdata.ProductData1 = optionsdata.ProductData = ALLproduct;
    if(["BEOL2"].includes(FactoryType)){
        await mountedLCDGrade(optionsdata, FactoryType);
    }
});
</script>
