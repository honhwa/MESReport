// import type { TableColumn } from '@/components/core/dynamic-table';
import router from '@/router';
import { AddCommonPageParams } from '@/store/module/commonpage';
import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import { ReportCommonWithOut } from '@/store/module/ReportCommon';

const useCommonParams = AddCommonPageParams();
const useReportCommon = ReportCommonWithOut();
export const PanelWipLabel = [ '0~1 h', '1~2 h', '2~4 h', '4~8 h', '8~12 h', '12~24 h', '1~2 d', '2~3 d','3~4 d','4~5 d','5~7 d','7~15 d','大於15 d','Sum'];
export const PanelWipProps = ['QTY1', 'QTY2', 'QTY3', 'QTY4', 'QTY5', 'QTY6', 'QTY7', 'QTY8', 'QTY9', 'QTY10', 'QTY11', 'QTY12', 'QTY13','TOTAL_QTY'];

export const PanelWipStayDetailLabel = [
  'Product',
  'Revision',
  'Panel ID',
  'Owner',
  'WO Nbr',
  'Floor',
  'P Work Ctr',
  'Operation',
  'Operation Desc',
  'STPN Time',
  'Last Trans Date',
  'Hold Code',
  'Hold Commnet',
]
const PanelWipStayDetailProps = [
  'Product',
  'Revision',
  'Panel ID',
  'Owner',
  'WO Nbr',
  'Floor',
  'P Work Ctr',
  'Operation',
  'Operation Desc',
  'STPN Time',
  'Last Trans Date',
  'Hold Code',
  'Hold Commnet',
]

let PanelWipStayDetailWidth= []

export const PanelWipColumns = TableCustomCell(PanelWipLabel, PanelWipProps, PanelWipProps,[])
export const PanelWipDetailColumns = TableCustomCell(PanelWipStayDetailLabel, PanelWipStayDetailProps, [],PanelWipStayDetailWidth)

function TableCustomCell(Label: string[], Prop: string[], ClickColumn: string[] = [],Width:number[]=[]) {
  return Prop.map((item: string, index: number) => {
    return {
      title: ClickColumn.includes(item) ? `${Label[index]}*` : Label[index],//可点击的栏位显示*号
      align: 'center' as any,
      dataIndex: item,
      width: Width[index],
      //@ts-ignore
      customCell: (record: Object, rowindex: number, column: Object) => {
        return {
          onDblclick: () => {
            //const UseParamsAdd = AddCommonPageParamsWithOut()
            //UseParamsAdd.AddPanelWIPStayFirstTableParams({ ...record, ...column['__originColumn__'] }) 
            
            //const useReportParams = ReportCommonWithOut()
            useReportCommon.AddFirstTableParams(`PanelWipStayDetail_${useCommonParams.Fab}`,{ ...record, ...column['__originColumn__'] })

            if (item != 'WORK_CTR') {
              router.push({ name: `PanelWipStayDetail_${useCommonParams.Fab}` });
            }

          }, style: {
            cursor: ClickColumn.includes(item) ? 'pointer' : '',
          }
        }
      },
      className:['PROD_NBR','WORK_CTR'].includes(item)?'v-center':'pro_auto-center'
    }
  })

}

