import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
import router from '@/router';

export const Top5OperationsLabel = ['BACKLIGHT','L.G.P ASSEMBLY','FILM ASSEMBLY','FINAL INSPECTION','PACKING','SHIPPING','Total']
export const Top5OperationsProps = ['1310', '1350', '1360', '1370', '1380', '1390']




const Top5Columns = GetColumns(Top5OperationsLabel, Top5OperationsProps)


export const Top5DefectCodeColumns: any[] = [

    {
        title: 'PROD',//label
        align: 'center',
        dataIndex: 'PROD1',//prop屬性,
        colSpan:'1',
        rowSpan:'1',
        fixed:'left',
    },
    ...Top5Columns

];

function GetColumns(Label: string[], Prop: string[]) {
    return Prop.map((item: string, index: number) => {
        return {
            title: Label[index],
            align: 'center' as any,
            dataIndex: item,
            colSpan: 2,
            children:
                [
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}`,
                        colSpan: 0,
                        rowSpan: 0,
                        customCell: (record: Object, rowIndex: number, column: Object) => {

                            if (!Reflect.has(record, 'OutYield') && record[item] != record[`${item}-DFCT_CODE`]) {
                                return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    style: {
                                        cursor: 'pointer',
                                    },
                                    
                                }
                            }
                            // else if(!record[item]||record[item]==) {
                            //     return {
                            //         colSpan: 0,
                            //         rowSpan: 0
                            //     }
                            // }
                            else {
                                return {
                                    colSpan: 2,
                                    rowSpan: 1

                                }
                            }
                        },
                    },
                    {
                        title: Label[index],
                        align: 'center',
                        dataIndex: `${item}-DFCT_CODE`,
                        colSpan: 0,
                        rowSpan: 0,
                        customCell: (record: Object, rowIndex: number, column: Object) => {
                            if (Reflect.has(record, 'OutYield') || record[`${item}-DFCT_CODE`] == record[item]) {
                                return {
                                    colSpan: 0,
                                    rowSpan: 0
                                }
                            }
                            else {
                                return {
                                    colSpan: 1,
                                    rowSpan: 1,
                                    // classname:"LightBlue",
                                   
                                }
                            }

                        },

                    }
                ]
        }
    })
}


