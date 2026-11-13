let a={"TRANS_DATE": "2022-12-01T08:04:19",
"WORK_CTR": 1760,
"SECTION_NO": "T72AMB57AK05",
"SERIAL_NO": "0GD2L4U2C1A1382",
"PRODUCT_ID": "2V430DJ1Q1T0S",
"JUDGEMENT": "OK",
"OQC_GRADE": "A",
"DEFECT_CODE": "PCC07",
"OQCCMT": "Cell內異物",
"OQCCMT1": " ",
"OQCCMT2": " ",
"OQCCMT3": " ",
"OQCCMT4": " ",
"OQC_PROCESS": "OK",
"CGRADE": "A",
"CDEFECT": " ",
"C_OPERATOR": "20035060",
"OPERATOR": "12114002",
"LEVEL_JUDGEMENT": "OK"}

function GetArrayLabel(Params) {
    console.log(Params.split(',')); 
}

function GetArrayKey(Params){
   console.log(Object.keys(Params)); 
}
GetArrayKey(a)
