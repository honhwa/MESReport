<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OneClickQuery.aspx.cs" Inherits="Report.Reports.TraceAbility.OneClickQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>未命名頁面</title>
    <%--<meta http-equiv="Content-Type" content="text/html; "/>--%>
    <link href="tableStyle.css" rel="stylesheet" type="text/css" />
    <link href="superTables.css" rel="stylesheet" type="text/css" />
    <script src="jquery-1.4.1.js" type="text/javascript"></script>
    <script src="superTables.js" type="text/javascript"></script>
    <script src="jquery.superTable.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#gvmain").toSuperTable({ width: "100%", height: "600px", fixedCols: 9, headerRows: 1 });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table id="tblcontrol" runat ="server" width="100%">
        <tr >
        <td colspan ="6" align="center"  style =" font-size:medium; font-family:Arial ;  font-weight :bolder; background-color:Silver"  >一&nbsp; &nbsp; &nbsp; &nbsp;鍵&nbsp; &nbsp; &nbsp; &nbsp;查&nbsp; &nbsp; &nbsp; &nbsp;詢</td>
        </tr>
        <tr>
            <td style="width:5%" ><asp:Label ID='lblyear' runat ="server" >PanelID：</asp:Label> </td>
            <td style="width:10%" > <asp:TextBox ID="txtENTITYID" runat="server" Width="200px"></asp:TextBox></td>
            <td colspan ="3" style="width:50%" >
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnquery" Text="Query"  runat ="server" OnClick="btnquery_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnfresh" Text="Fresh"  runat ="server" OnClick="btnfresh_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btnexport" Text="Export Excel"  runat ="server" OnClick="btnexport_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Label ID="lblRunTime" runat="server" Font-Bold="False" Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Blue"></asp:Label>
                <asp:Label ID="lblqueryRS" runat="server" Font-Bold="False" 
                    Font-Names="Microsoft Sans Serif" Font-Size="10pt" ForeColor="Brown" ></asp:Label>
                </td>
                
            <td valign="bottom" align="right" >
                <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" ToolTip="返回主頁面">Main Menu</asp:HyperLink>
                <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="True" Font-Names="Arial" Font-Size="10pt" NavigateUrl="~/Description/OneClickQuery.htm" Target="_blank" ToolTip="報表說明">Help</asp:HyperLink>
            </td>             
        </tr>
    </table>
    <asp:TextBox ID="tbFABID" runat="server" Visible="False" Width="50px"></asp:TextBox>
    <asp:TextBox ID="tbFABName" runat="server" Visible="False" Width="50px"></asp:TextBox>
    <asp:TextBox ID="tbLoginNTName" runat="server" Visible="False" Width="50px"></asp:TextBox>
    <asp:TextBox ID="tbLoginPCName" runat="server" Visible="False" Width="50px"></asp:TextBox>
    <asp:TextBox ID="tbLoginPCIP" runat="server" Visible="False" Width="50px"></asp:TextBox>
    <asp:Label ID="lblerror" Width="100%" runat ="server"  ForeColor ="red"  Text=""></asp:Label> 
    <asp:Panel ID="PNLMAIN" runat ="server" Width ="100%">
        <asp:Panel ID="Panel1" runat="server">
        
    <div id="divmain" runat ="server">
    <asp:GridView ID="gvmain" runat="server" AutoGenerateColumns="False"  CssClass="MyGridView" 
       lay-size="sm" EnableModelValidation="True" OnRowDataBound="gvmain_RowDataBound">
        <Columns>
            <%--<asp:BoundField DataField="panelid" HeaderText="PanelID">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>--%>

            <asp:BoundField DataField="entityid" HeaderText="entityid">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="TransDate" HeaderText="Trans Date" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="TrackInDate" HeaderText="Track In Date">
               <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrEntityID" HeaderText="Fr Entity ID">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="ToEntityID" HeaderText="To Entity ID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="verb" HeaderText="verb">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="PWorkCtr" HeaderText="PWork Ctr">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="CWorkCtr" HeaderText="CWork Ctr">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="NWorkCtr" HeaderText="NWork Ctr">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="mlotid" HeaderText="mlotid">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrProdID" HeaderText="Fr Prod ID">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />   
            </asp:BoundField>
            <asp:BoundField DataField="ToProdID" HeaderText="To Prod ID">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="FrWoNbr" HeaderText="Fr Wo Nbr">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="ToWoNbr" HeaderText="To Wo Nbr">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="PTrackOutDate" HeaderText="PTrack Out Date">
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <%--<asp:BoundField DataField="MLotID1" HeaderText="MLot ID 1" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>--%>
            <asp:BoundField DataField="FrPEntityUnit" HeaderText="Fr PEntity Unit" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />   
            </asp:BoundField>
            <asp:BoundField DataField="ToPEntityUnit" HeaderText="To PEntity Unit" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />   
            </asp:BoundField>
            <asp:BoundField DataField="FrEntityUnit" HeaderText="Fr Entity Unit" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
                <asp:BoundField DataField="ToEntityUnit" HeaderText="To Entity Unit" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="EntityCarry" HeaderText="Entity Carry" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="CEntityTQty" HeaderText="CEntity TQty" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Nentitytqty" HeaderText="Nentitytqty" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrWoType" HeaderText="Fr Wo Type" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="ToWoType" HeaderText="To Wo Type" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrLotType" HeaderText="Fr Lot Type" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="ToLotType" HeaderText="To Lot Type" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrLotGrade" HeaderText="Fr Lot Grade" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="ToLotGrade" HeaderText="To Lot Grade" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="FrLotLevel" HeaderText="Fr Lot Level" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="ToLotLevel" HeaderText="To Lot Level" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="CSUBLineID" HeaderText="CSUBLine ID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="NSUBLineID" HeaderText="NSUBLine ID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="userid" HeaderText="userid" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="ShiftID" HeaderText="Shift ID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="CustomerID" HeaderText="Customer ID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="SALEORDER" HeaderText="SALEORDER" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="TmpFIELD" HeaderText="Tmp FIELD" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="intervaltime" HeaderText="制程時間間隔" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="Sorting" HeaderText="是否經過Sorting" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="Reworkworkctr" HeaderText="經過Rework站點" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="HOIDSTATE" HeaderText="是否Hold過" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="PARTID" HeaderText="PARTID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="PARTTYPE" HeaderText="PARTTYPE" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="PARTLOTID" HeaderText="PARTLOTID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="SUBLINEID" HeaderText="SUBLINEID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="OperationID" HeaderText="OperationID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="OPTRANSDATE" HeaderText="OP TRANSDATE" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="WORKCTR" HeaderText="WORKCTR" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="USERID" HeaderText="USERID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="SHIFTID1" HeaderText="SHIFTID" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="CMT" HeaderText="CMT" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="Rework_times" HeaderText="Rework_times" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="FGBarcode" HeaderText="FG Barcode" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="SN" HeaderText="SN" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="LabelPrintNum" HeaderText="Label補印次數" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="LabelPrintLastUserid" HeaderText="最后一次補印工號" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="LabelPrintLastTime" HeaderText="最后一次補印時間" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="DQworkctr" HeaderText="目前WIP站點" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="BANKID" HeaderText="在庫倉別" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />  
            </asp:BoundField>
            <asp:BoundField DataField="OBA" HeaderText="是否經過OBA" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="YSCartid" HeaderText="原始箱號" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="OBACartid" HeaderText="OBA后箱號" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="DN" HeaderText="出貨DN" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
            </asp:BoundField>
            <asp:BoundField DataField="SHIPDATE" HeaderText="出貨時間" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="ASN_GROUPKEY" HeaderText="出貨客戶" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
            <asp:BoundField DataField="ASN_SHIP_TO" HeaderText="客戶代碼" >
                <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" /> 
            </asp:BoundField>
        </Columns>
        <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center"  Wrap="False" CssClass="gridcell" />
        </asp:GridView> 
    </div>
    </asp:Panel>
    <div >
        <asp:GridView ID="gvOEE" runat="server" AutoGenerateColumns="False"  CssClass="MyGridView" 
       lay-size="sm" EnableModelValidation="True">
            <Columns>
                <asp:BoundField DataField="sublineid" HeaderText="線別">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="prodid" HeaderText="型號">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="workctr" HeaderText="工站">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="prevstatus" HeaderText="宕機代碼">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="eqdes" HeaderText="宕機描述">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="startdate" HeaderText="開始時間">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="enddate" HeaderText="結束時間">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>                
                <asp:BoundField DataField="depart" HeaderText="責任部門">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
                <asp:BoundField DataField="cmt" HeaderText="備註">
                    <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                </asp:BoundField>
            </Columns> 
        </asp:GridView>
    </div>
        
    <asp:Label  ID="lbl40" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >1.Cell2 </asp:Label>
    <div id="div4" runat ="server" style ="width:100%">
    <asp:Panel ID="Panel5" runat="server" Width="100%">        
            </asp:Panel>
        <asp:GridView id="gv_d_cell2" runat ='server' Width="100%" OnRowDataBound="gv_d_cell2_RowDataBound" OnSelectedIndexChanged="gv_d_cell2_SelectedIndexChanged"  >
        </asp:GridView>
    </div>
    <asp:Label  ID="LBL50" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >2.Finishing </asp:Label>
    <div id="div5" runat ="server" style ="width:100%">
        <asp:GridView id="gv_d_m2" runat ='server' Width="100%" OnRowDataBound="gv_d_m2_RowDataBound"   >
        </asp:GridView>
    </div>
    <asp:Label  ID="lbl60" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >3.Assembly </asp:Label>
    <div id="div6" runat ="server" style ="width:100%">
        <asp:GridView id="gv_d_m3" runat ='server' Width="100%" OnRowDataBound="gv_d_m3_RowDataBound"   >
        </asp:GridView>
    </div>
    <asp:Label  ID="lbl10" Width ="100%" runat ="server" BackColor ="Goldenrod" Visible="False" >Cell2 Yield Loss Trend Chart By Defect</asp:Label>
    <div id="div1" runat ="server" style ="width:100%">
        <asp:GridView id="gvcell2_d" runat ='server' Width="100%" OnRowDataBound="gvcell2_d_RowDataBound">
        </asp:GridView>
    </div>
    <asp:Label ID="lbl20"  BackColor ="Goldenrod" Width ="100%"  Visible="false"  runat='server'>Module(finishing) Yield Loss Trend Chart By Defect</asp:Label>
    <asp:Panel ID="Panel2" runat="server" Width="100%">        
            </asp:Panel>
    <div id="div2" runat ="server" style ="width:100%">
        <asp:GridView id="gvfini_d" runat ='server' Width="100%" OnRowDataBound="gvfini_d_RowDataBound" OnSelectedIndexChanged="gvfini_d_SelectedIndexChanged" >
        </asp:GridView>
    </div>
    <asp:Label ID="lbl30"  BackColor ="Goldenrod"  Width ="100%" Visible="false"  runat='server'>Module(assembly) Yield Loss Trend Chart By Defect</asp:Label>
     <div id="div3" runat ="server" style ="width:100%">
        <asp:GridView id="gvassy_d" runat ='server' Width="100%" OnRowDataBound="gvassy_d_RowDataBound"  >
        </asp:GridView>
    </div>
    </asp:Panel>
    </form>
</body>
</html>
