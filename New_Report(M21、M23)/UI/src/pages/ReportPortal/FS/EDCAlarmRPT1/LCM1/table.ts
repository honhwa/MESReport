export function baseColumns (title:string){
return [
  

  {
    title: '幾臺別',
    align: 'center',
    // className: "v-center",
    children: [
        {
            title: "項目",
            dataIndex: 'UNIT_ID',
            key: "",
        },
        {
          title: "項目",
          dataIndex: 'ITEM_TYPE',
          key: "",
      },
        {
            title: "項目",
            dataIndex: 'ITEMGROUP',
            key: "",
        }
    ]
  },
  {
    title: '幾臺別',
    align: 'center',
    // className: "v-center",
    children: [
        {
            title: "RECIPE ID",
            children: [
              {
                  title: "公差",
                  dataIndex: 'SPEC',
                  key: "",
              },
          ]
        },
    ]
  },
  {
    title: title,
    align: 'center',
    // className: "v-center",
    children: [
        {
            title: "",
            children: [
              {
                  title: "幾臺設定",
                  dataIndex: 'VALUE_SETTING',
                  key: "",
              },
          ]
        },{
          title: "",
          children: [
            {
                title: "實際",
                dataIndex: 'VALUE_ACTUAL',
                key: "",
            },
        ]
      },
    ]
  },
]

} 