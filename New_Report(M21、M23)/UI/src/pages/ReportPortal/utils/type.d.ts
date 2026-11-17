declare namespace BasePageParams{
    type BasePageSetting = {
        List:any[];
        APIColumns:string[];
        Columns:any[];
        moreData:any[];
        SQLHidden?:string;
        Title?:string;
        [x: string]: any
    }
}