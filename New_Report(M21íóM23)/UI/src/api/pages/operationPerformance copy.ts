import { request } from '@/utils/request';
export function GetOperationPerformancelcm1(data: []) {
    return request({
        url: '/api/OperationPerformanceLcm1/Lcm1',
        method: 'post',
        data
    });
}

export function GetOperationPerformancePassQty(data: []) {
    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformancePassQty',
        method: 'post',
        data
    });
}
export function GetOperationPerformanceDetail9(data: []) {
    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformanceDetail9',
        method: 'post',
        data
    });
}

export function GetOperationDetailWipQty(data: []) {
    return request({
        url: '/api/OperationPerformance/LCMA/WipQty',
        method: 'post',
        data
    })
}
export function GetWipDetailData(data: []) {
    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformanceDetail',
        method: 'post',
        data
    })
}

export function GetWipQty10(data: []) {
    return request({
        url: '/api/OperationPerformance/LCMA/WipQty10',
        method: 'post',
        data
    })
}

export function GetReworkQty(data: []) {

    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformanceReworkQty',
        method: 'post',
        data
    })
}
export function GetPassQtyTOM(data: []) {//后->前
    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformancePassQtyZ',
        method: 'post',
        data
    })
}
export function GetPassQtyTOMDetail(data: []) {

    return request({
        url: '/api/OperationPerformance/LCMA/OperationPerformancePassQtyZ1',
        method: 'post',
        data
    })
}

export function GetReworkD2000(data:[]){//大於兩千的站點點擊In
    return request({
        url: '/api/OperationPerformance/LCMA/Rework',
        method: 'post',
        data
    })
}
export function GetOperperDetail7(data:[]) {
    return request({
        url:"/api/OperationPerformance/LCMA/PanelWipStayChart",
        method:'post',
        data
    })
}