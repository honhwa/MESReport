export const CompanyData: Company[] = [
    /* {
        CompanyLog: 'INNOLUX',
        CompanyName: '群創光電',
        SystemTitle:'Innolux Report System',
        LogData:['I','NNO','L','U','X']
    }, */
    {
        CompanyLog: 'CARUX',
        CompanyName: 'CARUX',
        SystemTitle:'CARUX System',
        LogData:['C','A','R','U','X']
    }
]

export const CompanyMod: any[] = [
   /*  {
        label: 'ALL',
        value: 'INNOLUX'
    },  */
    {
        label: 'CARUX',
        value: 'CARUX'
    }
]

export type Company = {
    CompanyLog: string;
    CompanyName: string;
    SystemTitle:string;
    LogData?:string[];
}