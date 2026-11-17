  import type { FormSchema } from '@/components/core/schema-form/'
  import { AddCommonPageParamsWithOut } from '@/store/module/commonpage';
  import { NGBSelectType} from '@/pages/ReportPortal/Static/NGBCommon-Form-schema';
  import useRemoteSelect from '@/pages/ReportPortal/utils/NGBuseRemoteSelect';
  import {FromDateParams,ToDateParams,GroupParams, PreLineParams,ProdIDParams,ProdNumParams} from '@/pages/ReportPortal/Static/NGBCommonParams'
  const useCommonParams = AddCommonPageParamsWithOut();
  const { getCommonFormParams,getCommonFormParamsNoAll,mountedEDCEquipment } = useRemoteSelect()
  export let PreProc = [
    {value: "ALL", label: "ALL" },
    {value: "COG", label: "COG" },
    {value: "OLB", label: "OLB" }, 
    {value: "PCB", label: "PCB" },
    {value: "FPC", label: "FPC" }
    ]

    export let opID = [
      {value: "NO", label: "NO" },
      {value: "YES", label: "YES"}
      ]

    export let OPER = [
      {value: "", label: "ALL" },
      {value: "1300", label: "1300" },
      {value: "1400", label: "1400" }, 
      {value: "1600", label: "1600" }, 
      {value: "1700", label: "1700" }, 
      {value: "1800", label: "1800" }, 
      {value: "1930", label: "1930" }
      ]
    export let SPEC = [
      {value: "ALL", label: "ALL" },
      {value: "BA", label: "BA-Bonding精度" },
      {value: "BS", label: "BS-Bonding強度" }, 
      {value: "PN", label: "PN-導電粒子顆數" }, 
      {value: "CB", label: "CB-平衡電壓" }, 
      {value: "NC", label: "NC-負電荷消散時間" }, 
      {value: "PC", label: "PC-正電荷消散時間" },
      {value: "TE", label: "TE-溫度" },
      {value: "TP", label: "TP-微粒子數" },
      {value: "SP", label: "SP-小粒子數" }, 
      {value: "MP", label: "MP-中粒子數" }, 
      {value: "BP", label: "BP-中粒子數" }, 
      {value: "HP", label: "HP-中粒子數" }, 
      {value: "PS", label: "PS-沖切偏移" },
      {value: "AP", label: "AP-ACF貼附精度" },
      {value: "EF", label: "EF-Flicker&Vcom" },
      {value: "PP", label: "PP-本壓刀頭壓著精度" }, 
      {value: "BW", label: "BW-壓痕" },  
      {value: "OC", label: "OC" }, 
      {value: "DS", label: "DS-距離" },
      {value: "FF", label: "FF-FPC偏移量" },
      {value: "JC", label: "JC-膠長" },
      {value: "JH", label: "JH-膠厚" }, 
      {value: "JK", label: "JK-膠寬" },  
      {value: "TJ", label: "TJ-間隙" }
    ]
		
export const schemas = (FactoryType:string,optionsdata:any)=>{
  return [
  //Spc Floor
  {
   component: 'RadioGroup',
    field: 'SpcFloor',

    componentProps: {
      options: [
        {
          label: 'N'+useCommonParams.G_FAC_COM+'1',
          value: '1'
        },
        {
          label: 'N'+useCommonParams.G_FAC_COM+'2',
          value: '2'
        },
        {
          label: 'N'+useCommonParams.G_FAC_COM+'3',
          value: '3'
        }
        
      ],
    },
    defaultValue: '1'

  },
  //chartType
  {
    component: 'RadioGroup',
    field: 'chartType',

    componentProps: {
      options: [
        {
          label: 'PX',
          value: 'PX'
        },
        {
          label: 'Other',
          value: 'Other'
        }
        
      ],
    },
    defaultValue: 'PX'

  },
  
  //chartStat
  {
    component: 'RadioGroup',
    field: 'chartStat',

    componentProps: {
      options: [
        {
          label: 'ALL',
          value: 'ALL'
        },
        {
          label: 'Running',
          value: 'Running'
        }
        
      ],
    },
    defaultValue: 'ALL'

  },
  //Measure ToolID 
  {
    component: 'RadioGroup',
    field: 'DataPat',

    componentProps: {
      options: [
        {
          label: 'Measure ToolID ',
          value: 'M'
        }
        
      ],
    },
    defaultValue: 'M'

  },
  {
    label: 'Pre-Proc',
    component: 'Select',
    field: 'PreProc',
    componentProps: () => {
      return {
        options: PreProc,      
      };
    },
    defaultValue:'ALL'
  },
  ...new PreLineParams(optionsdata,FactoryType)._obj,
  ...FromDateParams,
  ...ToDateParams,
  ...new ProdIDParams(optionsdata,FactoryType)._obj,
  //SHOW DEL.
  {
    label: 'SHOW DEL.',
    component: 'Select',
    field: 'opID',
    componentProps: () => {
      return {
        options: opID,      
      };
    },
    defaultValue:'NO'
  },
  ...new GroupParams(optionsdata,FactoryType)._obj,
  ...new ProdNumParams(optionsdata,FactoryType)._obj,
   //OPER
   {
    label: 'OPER',
    component: 'Select',
    field: 'OPER',
    componentProps: () => {
      return {
        options: OPER,      
      };
    },
    defaultValue:'ALL'
  },
   //SPEC
   {
    label: 'SPEC',
    component: 'Select',
    field: 'SPEC',
    componentProps: () => {
      return {
        options: SPEC,      
      };
    },
    defaultValue:'ALL'
  }
  
];
}
