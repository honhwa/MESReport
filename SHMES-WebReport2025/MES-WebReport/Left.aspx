<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Left.aspx.cs" Inherits="WebReport.Left" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Left</title>
    <link href="App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<body topmargin="0" leftmargin="0" style="background-image: url(Images/left_bg.jpg)">
    <form id="form1" runat="server">
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 100%; padding-top: 0px; height: 100%;">
                <tr>
                    <td style="background-position-x: left; background-attachment: scroll; background-image: url(Images/left_bg.jpg);
                        vertical-align: top; width: 1px; background-repeat: repeat-y; height: 419px">
                        <%--<img src="Images/logo-m.png" style="background-image: url(Images/logo-m.png); " />--%>
                        <table style="width: 167px; height: 117px">
                            <tr>
                                <td style="height: 87px; width: 166px;">
                            <asp:TreeView ID="TreeView1" runat="server" ImageSet="Msdn" NodeIndent="10" NodeWrap="True"
                                Target="Main" ExpandDepth="0" ShowLines="True" Width="174px">
                                <ParentNodeStyle Font-Bold="False" />
                                <HoverNodeStyle BackColor="#CCCCCC" BorderColor="#888888" BorderStyle="Solid" Font-Underline="True" BorderWidth="1px" />
                                <SelectedNodeStyle BackColor="White" BorderColor="#888888" BorderStyle="Solid" BorderWidth="1px"
                                    Font-Underline="False" HorizontalPadding="3px" VerticalPadding="1px" />
                                <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                    NodeSpacing="1px" VerticalPadding="2px" />
                                <Nodes>
                                    <asp:TreeNode Text="TraceAbility Report" Value="TraceAbility Report">
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/BySNQyeryPanelID.aspx" Target="main"
                                            Text="SN Query PanelID" Value="BySNQyeryPanelID"></asp:TreeNode>
                                            
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/MaterialK.aspx" Target="main"
                                            Text="Key Material Trace Query" Value="Key Material Trace Query"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotTraceQuery.aspx" Text="LotTraceQuery"
                                            Value="LotTraceQuery" Target="main"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotWIP.aspx" Target="main" Text="LotWIP"
                                            Value="LotWIP"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/QueryScrapData.aspx" Target="main" Text="QueryScrapData"
                                            Value="QueryScrapData"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/OQCLabelScanReport.aspx" Text="OQCLabelScan"
                                            Value="OQCLabelScan" Target="main"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/OQCBlock.aspx" Target="main"
                                            Text="OQCLotBlock" Value="OQCLotBlock"></asp:TreeNode>
                                        <%--<asp:TreeNode NavigateUrl="~/Reports/TraceAbility/QueryCarton.aspx" Target="main"
                                            Text="QueryCarton" Value="QueryCarton"></asp:TreeNode>--%>
                                            
                                        <asp:TreeNode NavigateUrl="~/Reports/CARTONID/Select.aspx" Target="main" 
                                            Text="CARTONID Select" Value="CARTONID Select"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="http://10.119.8.137/BulkUpload/QueryDefectCode.aspx" 
                                            Target="main" Text="箱號查詢DefectCode" Value="箱號查詢DefectCode"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/OneClickQuery.aspx" Target="main" 
                                            Text="一鍵查詢報表" Value="OneClickQuery"></asp:TreeNode>    
                                    </asp:TreeNode>
                                    <asp:TreeNode Target="main" Text="製造報表" Value="製造報表">                                            
                                        <asp:TreeNode NavigateUrl="~/Reports/MPC/WIPAnalysis.aspx" Target="main" Text="WIP Analysis"
                                            Value="WIP Analysis"></asp:TreeNode>
                                         
                                         <asp:TreeNode Text="Input / Output" Value="Input / Output report">
                                            <asp:TreeNode NavigateUrl="~/Reports/MFG/FrmSBInputQuery.aspx" Target="main" Text="Start Input"
                                                Value="Start Input Query"></asp:TreeNode>
                                            <asp:TreeNode Target="main" Text="InputOutput&amp;Defect " Value="各站Input&amp;Output Query" NavigateUrl="~/Reports/MFG/FrmInputANDOutputQuery.aspx">
                                            </asp:TreeNode>
                                            
                                        </asp:TreeNode>
                                        
                                        <asp:TreeNode NavigateUrl="~/Reports/MFG/By_Customer_Wkctr.aspx" Target="main" Text="Query Product DefectCode"
                                            Value="Query Product DefectCode"></asp:TreeNode>
                                            
                                        <asp:TreeNode NavigateUrl="~/Reports/MFG/Product Summary.aspx" Target="main" Text="Query Product Summary Data"
                                            Value="Query Product Summary Data"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/MFG/ActOutputQuery.aspx?LINEID=4F-ASSY-L02" Target="main" Text="Line Product Board"
                                            Value="Line Product Board"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/Material/MaterialQuery.aspx" Text="物料明細報表" Value="物料明細報表" Target="main">
                                        </asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/MFG/Burn_In_RecordQuery.aspx" Target="main"
                                            Text="Burn In記錄查詢" Value="Burn In記錄查詢"></asp:TreeNode>
                                        <asp:TreeNode Text="批量上傳"
                                            Value="新節點" Target="main" NavigateUrl="http://10.119.8.137/BulkUpload/Default.aspx">
                                            <asp:TreeNode NavigateUrl="http://10.119.8.137/BulkUpload/querysn.aspx" Target="main"
                                                Text="LOTID,PANELID查詢SN,Barcode" Value="PANELID查詢SN暨Barcode"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="http://10.119.8.137/BulkUpload/querycartid.aspx" Target="main"
                                                Text="cartid查詢" Value="cartid查詢"></asp:TreeNode>
                                        </asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="http://10.119.8.137/BulkUpload/QueryDefectCode.aspx" Target="main"
                                            Text="箱號查詢DefectCode" Value="箱號查詢DefectCode"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/MFG/OperationIDQuery.aspx" Target="main" Text="OperationIDQuery"
                                            Value="OperationIDQuery"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/MFG/YaHeTest.aspx" Target="main" Text="壓合幾數據查詢"
                                            Value="YaHeTest"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="Reports/MFG/QueryOfflineHandling.aspx" Target="main" Text="Offline Handling查詢"
                                            Value="QueryOfflineHandling"></asp:TreeNode>
                                    </asp:TreeNode>
                                     <asp:TreeNode Target="main" Text="物料報表" Value="物料報表">    
                                     <asp:TreeNode   NavigateUrl="~/Reports/Material/MaterialQuery.aspx" Target="main" Text="Material Query"
                                            Value="Material Query"></asp:TreeNode>
                                     </asp:TreeNode>
                                    <asp:TreeNode Text="設備報表" Value="設備報表">
                                        <asp:TreeNode NavigateUrl="~/Reports/EMS/EMSStatusQuery.aspx" Target="main" Text="Line即時狀態查詢"
                                            Value="Line即時狀態查詢"></asp:TreeNode>
                                        <asp:TreeNode Text="設備歷史狀態信息" Value="設備歷史狀態信息" NavigateUrl="~/Reports/EMS/FrmEQSMMReport.aspx" Target="main"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="IE報表" Value="IE報表">
                                        <asp:TreeNode NavigateUrl="~/Reports/EMS/EQAndSublineStatusDetail.aspx" Target="main"
                                            Text="設備/產線狀態細節報表" Value="設備/產線狀態細節報表"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/IE/OEE_EXEC.aspx" Target="main" Text="Manual OEE"
                                            Value="Manual OEE"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/IE/default.aspx" Target="main"
                                            Text="OEE REPORT" Value="OEE REPORT"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/IE/unprodidtt.aspx" Target="main"
                                            Text="線別&amp;機種未維護信息" Value="線別&amp;機種未維護信息"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/IE/TactTime.aspx" Target="main"
                                            Text="Tact Time信息" Value="Tact Time信息"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/IE/DTactTime.aspx" Target="main"
                                            Text="Tact Time Detail 信息" Value="Tact Time Detail 信息"></asp:TreeNode>
                                         <asp:TreeNode NavigateUrl="~/Reports/IE/UNTactTime.aspx" Target="main"
                                            Text="Tact Time Target未維護信息" Value="Tact Time Target未維護信息"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/zhongweishu/IE_MEDIAN.aspx" Target="main"
                                        Text="中位數報表" Value="中位數報表"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/IE/CycleTime2.aspx" Target="main"
                                        Text="CycleTime" Value="CycleTime"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="PE報表" Value="PE報表" ToolTip="main">
                                       <asp:TreeNode NavigateUrl="~/Reports/PE/EngReport.aspx" Target="main" Text="ENG報表"
                                            Value="ENG報表"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/PE/PanelRecord.aspx" Target="main" Text="Panel Record 查詢"
                                            Value="Panel Record 查詢"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="YMS(良率管理系統)" Value="YMS(良率管理系統)" NavigateUrl="http://10.119.8.137/yms/mainbutton.aspx?fabid =B"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="PC報表" Value="PC報表">
                                        <asp:TreeNode NavigateUrl="~/Reports/PC/InputAndOutput.aspx" Target="main" Text="各站產出報表"
                                            Value="各站產出報表"></asp:TreeNode>
                                         <asp:TreeNode NavigateUrl="~/Reports/PC/SGradeDfectPanelDQuery.aspx" Target="main"
                                            Text="S Grade Dfect Pane lD Query" Value="S Grade Dfect Pane lD Query"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/PC/FinalOutQty.aspx" Target="main" Text="每日計劃產出維護主檔"
                                            Value="每日計劃產出維護主檔"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/PC/WOCompress.aspx" Target="main" Text="工單提前&延用報表"
                                            Value="工單提前&延用報表"></asp:TreeNode>
                                    </asp:TreeNode>
                                 
                                    <asp:TreeNode Text="會計報表" Value="會計報表">
                                    <asp:TreeNode Target="main" Text="SMS(報廢管理系統)" Value="SMS(報廢管理系統)"  NavigateUrl="http://10.119.8.137/sms/mainbutton.aspx?fabid =B"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/ACC/RMSP.aspx" Target="main" Text="Module報廢報表"
                                            Value="Module報廢報表"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/ACC/WOQuery.aspx" Target="main" Text="工單結案查詢"
                                            Value="工單結案查詢"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/ACC/8180Outputquery.aspx" Target="main" Text="Output Query"
                                            Value="8180OutputQuery"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="IEB報表" Value="IEB報表">
                                        <asp:TreeNode Target="main" Text="IEB Wip報表" Value="IEB Wip報表" NavigateUrl="~/Reports/IEB/IbeWipQuery.aspx"></asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="BMW Reports" Value="BMW Reports">
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotWIP.aspx" Target="main" Text="WIP Query"
                                            Value="WIP Query"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotTraceQuery.aspx" Target="main"
                                            Text="Lot History" Value="Lot History"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/PanelInfo.aspx" Target="main" Text="Panel History"
                                            Value="Panel History"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LOTSPLT.aspx" Target="main" Text="Lot Vs Panel ID"
                                            Value="Lot Vs Panel ID"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Lot Defect Query List" Value="Lot Defect Query List" NavigateUrl="~/Reports/TraceAbility/LotDefectQueryList.aspx">
                                        </asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Consume Trace" Value="Consume Trace" NavigateUrl="~/Reports/TraceAbility/ConsumeTrace.aspx"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Defect Report" Value="Defect Report" NavigateUrl="~/Reports/TraceAbility/DefectCodesummary.aspx"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/PE/EngReport.aspx" Target="main" Text="Yield Report"
                                            Value="Yield Report"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="MachinePMTracking" Value="MachinePMTracking" NavigateUrl="~/Reports/TraceAbility/MACHINEPMTRACKING.aspx"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Machine Run History" Value="Machine Run History" NavigateUrl="~/Reports/TraceAbility/MachineRunHistory.aspx"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/MaterialK.aspx" Target="main" Text="Material Lot Trace"
                                            Value="Material Lot Trace"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="OnLine Defect Summary" Value="OnLine Defect Summary" NavigateUrl="~/Reports/TraceAbility/OnlineDefectSummary.aspx">
                                        </asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Mlot&amp;SN Query PanelID" Value="Mlot&amp;SN Query PanelID" NavigateUrl="~/Reports/TraceAbility/MaterialLotSNPanel.aspx">
                                        </asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/PC/InputAndOutput.aspx" Target="main" Text="BMW Input &amp; Scrap"
                                            Value="BMW Input &amp; Scrap"></asp:TreeNode>
                                            <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/BMWTestResult.aspx" Target="main"
                                            Text="BMWTestResult" Value="BMWTestResult"></asp:TreeNode>
                                         
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/EQParameter.aspx" Target="main"
                                            Text="EQParameter" Value="EQParameter"></asp:TreeNode>
                                        
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="Panasonic Reports" Value="Panasonic Reports">
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotWIP.aspx" Target="main" Text="WIP Query"
                                            Value="WIP Query"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotTraceQuery.aspx" Target="main"
                                            Text="Panasonic Lot History" Value="Panasonic Lot History"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LOTSPLT.aspx" Target="main" Text="Panasonic Lot Vs Panel ID"
                                            Value="Panasonic Lot Vs Panel ID"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/PanelInfo.aspx" Target="main" Text="Panasonic Panel History"
                                            Value="Panasonic Panel History"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Lot Defect Query List" Value="Lot Defect Query List" NavigateUrl="~/Reports/TraceAbility/LotDefectQueryList.aspx">
                                        </asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="*Inventory" Value="Inventory"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/PE/EngReport.aspx" Target="main" Text="Yield Report"
                                            Value="Yield Report"></asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/LotTraceQuery.aspx" Target="main"
                                            Text="Lot Trace" Value="Lot Trace"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Consume Trace" Value="Consume Trace" NavigateUrl="~/Reports/TraceAbility/ConsumeTrace.aspx"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="OnLine Defect Summary" Value="OnLine Defect Summary" NavigateUrl="~/Reports/TraceAbility/OnlineDefectSummary.aspx">
                                        </asp:TreeNode>
                                        <asp:TreeNode NavigateUrl="~/Reports/TraceAbility/Cell2PanelRecord.aspx" Target="main"
                                            Text="Laser Repair Yield" Value="Laser Repair Yield"></asp:TreeNode>
                                        <asp:TreeNode Target="main" Text="Panel Rework history" Value="Panel Rework history" NavigateUrl="~/Reports/TraceAbility/PanelReworkInfo.aspx">
                                        </asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode NavigateUrl="~/Reports/LockSrew/LockScrewDataQuery.aspx" Text="Lock screw data" Value="Lock screw data" Target="main">
                                    </asp:TreeNode>
                                    <asp:TreeNode Text="Bosch Reports" Value="Bosch Reports">
                                        <asp:TreeNode Target="main" Text="Branda" Value="Panel Rework history" NavigateUrl="http://10.119.8.137/YMS/YMSReport/SH_Bosch_Yield.aspx">
                                        </asp:TreeNode>
                                    </asp:TreeNode>
                                    <asp:TreeNode NavigateUrl="~/Reports/AOI_FP/AOIFP.aspx" Target="main"
                                        Text="AOI復判" Value="AOI復判"></asp:TreeNode>
                                    <asp:TreeNode NavigateUrl="~/Reports/RMA/RMASelect.aspx" Target="main" Text="RMA"
                                        Value="RMASelect"></asp:TreeNode>
                                </Nodes>
                            </asp:TreeView>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 133px; width: 166px;">
                        <table>
                            <tr style="height: 33px">
                                <td style="width: 35px; text-align: right;">
                                    <img alt="" src="Images/user.png" style="background-image: url(Images/user.png)" />
                                </td>
                                <td style="width: 50px; vertical-align: middle;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 10px; height: 28px;">
                                                <img alt="" src="Images/left.png" /></td>
                                            <td style="width:213px;background-image: url(Images/tb_bg.png); background-repeat: repeat-x; height: 28px;">
                                                <asp:Label ID="lblUserName" runat="server" Width="105px" Font-Size="Small"></asp:Label></td>
                                            <td style="width: 9px; height: 28px;">
                                                <img alt="" src="Images/right.png" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 35px; text-align: right;">
                                    <img alt="" src="Images/ip.png" />
                                </td>
                                <td style="width: 50px; vertical-align: middle;">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 10px;">
                                                <img alt="" src="Images/left.png" /></td>
                                            <td style="background-image: url(Images/tb_bg.png); background-repeat: repeat-x">
                                                <asp:Label ID="lblIp" runat="server" Width="105px" Font-Size="Small"></asp:Label>
                                            </td>
                                            <td style="width: 9px;">
                                                <img alt="" src="Images/right.png" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="height: 41px;text-align:center;">
                                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/Images/Logout.png" OnClick="ImageButton1_Click" /></td>
                            </tr>
                        </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
