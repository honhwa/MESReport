import type { FormSchema } from '@/components/core/schema-form/';
import { StartDateParams, EndDateParams } from '@/pages/ReportPortal/Static/CommonParams'



export const userSchemas: FormSchema[] = [
    {
        ...StartDateParams,
         colProps: {
            span: 8
        }
    },
    {
        ...EndDateParams, 
        colProps: {
            span: 8
        }
    },
    // {
    //     field: 'FactoryType',
    //     component: 'Input',
    //     label: '廠別',
    //     rules: [{ required: true }],
    //     colProps: {
    //         span: 8
    //     }
    // },
    {
        field: 'FactoryType',
        component: 'Input',
        label: '廠別',
        required: true,
        slot: 'remoteSearch',
        colProps: {
          span: 6,
        },
      },
];



