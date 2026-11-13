import { AddCommonPageParams } from '@/store/module/commonpage';
import router from '@/router';
import { ReportCommon } from '@/store/module/ReportCommon';
import { reactive } from 'vue';

const useCommonParams = AddCommonPageParams();
let useReportCommon = ReportCommon();

export const Top5OperationsDetailLabel = ['prod_nbr', 'entity_no', 'trans_date', 'trans_type', 'equip_nbr', 'operator', 'source_fab','lcd_grade','lcm_grade','lcm_owner',  'defect_code', 'errc_descr', 'rework 次數', 'dfct 次數', '合計片數'];
export const Top5OperationsDetailProps = ["PROD_NBR",
    "ENTITY_NO",
    "TRANS_DATE",
    "TRANS_TYPE",
    "EQUIP_NBR",
    "OPERATOR",
    "SOURCE_FAB",
    "LCD_GRADE",    
    "LCM_GRADE",
    "LCM_OWNER",
    "DFCT_CODE",
    "ERRC_DESCR",
    "STIMES_TTL",
    "ITEM_NBR",
    "PASS_QTY"]
    const Top5OperationsDetailWidth=[80,75,120,50,50,50,50,50,50,50,60,70,50,45,45] 

    export let Top5SecondColumns: any[] = [ 
      {
        title: 'Panel ID',
        align: 'center',
        dataIndex: `ENTITY_NO`,
        key: `ENTITY_NO`,
        width:65,
      },
      {
        title: 'prod_nbr',
        align: 'center',
        dataIndex: `PROD_NBR`,
        key: `PROD_NBR`,
        width:75,
      },
      {
        title: 'Oper',
        align: 'center',
        dataIndex: `WORK_CTR`,
        key: `WORK_CTR`,
        width:30,
      },
      {
        title: 'process',
        align: 'center',
        dataIndex: `LCMPROCESS`,
        key: `LCMPROCESS`,
        width:50,
      },
      {
        title: 'equip_nbr',
        align: 'center',
        dataIndex: `EQUIP_NBR`,
        key: `EQUIP_NBR`,
        width:50,
      },
      {
        title: 'trans_date1',
        align: 'center',
        dataIndex: `TRANS_DATE1`,
        key: `TRANS_DATE1`,
        width:50,
      },
      {
        title: 'trans_date2',
        align: 'center',
        dataIndex: `TRANS_DATE2`,
        key: `TRANS_DATE2`,
        width:70,
      },
      {
        title: 'TA',
        align: 'center',
        dataIndex: `OPERATOR`,
        key: `OPERATOR`,
        width:60,
      },
      {
        title: 'trans_type',
        align: 'center',
        dataIndex: `TRANS_TYPE`,
        key: `TRANS_TYPE`,
        width:50,
      },
      {
        title: 'comment',
        align: 'center',
        dataIndex: `COMMENT1`,
        key: `COMMENT1`,
        width:120,
      },
    ];


export const Top5DetailColumns = GetTableColumn(Top5OperationsDetailLabel, Top5OperationsDetailProps,Top5OperationsDetailWidth,)

function GetTableColumn(Label: string[], Prop: string[],Width:number[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            width:Width[index],
            customCell: (record:Object, rowIndex:number, column:Object) => {
                return {
                    onDblclick: () => {
                      // console.log("rowindex------:",rowIndex);
                      
                        
                        useReportCommon.AddSecondTableParams(`DownGradeTop5DefectCode-${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'], rowindex:rowIndex}) 
                        
                    }, style: { cursor: 'pointer' }
                }
            }
        }
    })
}


