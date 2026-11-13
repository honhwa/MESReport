<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WIPAnalysis.aspx.cs" 
    Inherits="WebReport.Reports.WIPAnalysis" StylesheetTheme="UNICOMTheme" 
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>WIP Analysis</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>

<script language="javascript" type="text/javascript">
function DIV1_onclick()
{

}
</script>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div>
        <table style="width: 900px; height: 103px">
            <tr>
                <td style="width: 19px; height: 26px">
                </td>
                <td style="width: 123px; height: 26px">
                </td>
                <td style="width: 171px; height: 26px; text-align: left">
                    &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 19px; height: 42px">
                </td>
                <td align="center" style="width: 123px; height: 42px">
                    工單</td>
                <td style="width: 171px; height: 42px; text-align: left">
                        &nbsp;<asp:TextBox ID="txtWotype" runat="server" Width="107px"></asp:TextBox></td>
                <td align="center" style="width: 56px; height: 42px">
                </td>
                <td style="width: 201px; height: 42px; text-align: left">
                        &nbsp;
                </td>
            </tr>
            <tr>
                <td style="width: 19px; height: 7px">
                </td>
                <td align="center" style="width: 123px; height: 7px">
                    Cell2<br />
                    站&nbsp; 別</td>
                <td style="width: 171px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 176px; border-bottom: black 1px solid; top: 78px; height: 58px">
                        <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="True" OnCheckedChanged="chkAll_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstStation" runat="server" AutoPostBack="True" OnDataBinding="lstStation_SelectedIndexChanged"
                            Width="98%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="width: 56px; height: 7px">
                    Moudle站 &nbsp; 別</td>
                <td style="width: 201px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 173px; border-bottom: black 1px solid; top: 78px; height: 57px">
                        <asp:CheckBox ID="CheckModuleALL" runat="server" AutoPostBack="True" OnCheckedChanged="CheckModuleALL_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstModuleStation" runat="server" OnDataBinding="lstModuleStation_SelectedIndexChanged"
                            Width="99%">
                        </asp:CheckBoxList></div>
                </td>
                <td align="center" style="width: 57px; height: 7px">
                    群 組</td>
                <td style="width: 161px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 148px; border-bottom: black 1px solid; top: 78px; height: 63px">
                        <asp:CheckBox ID="chkAllProdGroup" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdGroup_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProdGroup" runat="server" AutoPostBack="True" OnDataBinding="lstProdGroup_SelectedIndexChanged"
                            Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
                <td align="center" style="width: 143px; height: 7px">
                    產品型號</td>
                <td style="width: 103px; height: 7px; text-align: left">
                    <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                        z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                        width: 148px; border-bottom: black 1px solid; top: 78px; height: 63px">
                        <asp:CheckBox ID="chkAllProdID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdID_CheckedChanged"
                            Text="ALL" Width="100%" />
                        <asp:CheckBoxList ID="lstProductID" runat="server" AutoPostBack="True" OnDataBinding="lstProductID_SelectedIndexChanged"
                            Width="99%">
                        </asp:CheckBoxList>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 19px; height: 14px">
                </td>
                <td colspan="4" style="vertical-align: middle; height: 26px">
                    <asp:Label ID="lblErrMsg" runat="server" Width="312px"></asp:Label>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="查詢" Width="49px" />
                    </td>
            </tr>
        </table>
    
    </div>
        <div id="Div8" onclick="return DIV1_onclick()" style="border-right: black 1px solid;
            border-top: black 1px solid; z-index: 104; left: 202px; overflow: auto; border-left: black 1px solid;
            width: 899px; border-bottom: black 1px solid; top: 78px; height: 235px">
            &nbsp;<asp:ImageButton ID="ImageButton1" runat="server" Height="17px" ImageUrl="xls.jpg"
                        OnClick="ImageButton1_Click" Width="25px" />
            <asp:GridView ID="DataGridSum" runat="server" AutoGenerateColumns="False" EnableEventValidation = "false" AutoEventWireup="true"
                Height="205px" OnRowCommand="DataGridSum_RowCommand" Width="819px" >
                <Columns>
                    <asp:ButtonField ButtonType="Button" HeaderText="詳細資訊" Text="詳細資訊" />
                    <asp:BoundField DataField="PARTID" HeaderText="PARTID" />
                    <asp:BoundField DataField="WORKCTR" HeaderText="WORKCTR" />
                    <asp:BoundField DataField="TTL_QTY" HeaderText="TTL_QTY" />
                    <asp:BoundField DataField="HOLD" HeaderText="HOLD" />
                    <asp:BoundField DataField="PENDING" HeaderText="PENDING" />
                    <asp:BoundField DataField="RUN" HeaderText="RUN" />
                    <asp:BoundField DataField="WAIT" HeaderText="WAIT" />
                </Columns>
                <RowStyle Font-Names="Arial" Font-Size="Small" />
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:GridView>
        </div>
        <div id="Div2" onclick="return DIV1_onclick()" style="border-right: black 1px solid;
            border-top: black 1px solid; z-index: 104; left: 202px; overflow: auto; border-left: black 1px solid;
            width: 899px; border-bottom: black 1px solid; top: 78px; height: 386px">
            &nbsp;<asp:ImageButton ID="ImageButton2" runat="server" Height="17px" ImageUrl="xls.jpg"
                        OnClick="ImageButton2_Click" Width="25px" />
            <asp:GridView ID="DataGridDetail" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                Font-Names="Arial" Font-Size="Small">
                <Columns>
                    <asp:BoundField DataField="COUNT" HeaderText="COUNT" />
                    <asp:BoundField DataField="CSHOPCD" HeaderText="CSHOPCD" />
                    <asp:BoundField DataField="WIP_TYPE" HeaderText="WIP_TYPE" />
                    <asp:BoundField DataField="ENTITYID" HeaderText="ENTITYID" />
                    <asp:BoundField DataField="LOTTYPE" HeaderText="LOTTYPE" />
                    <asp:BoundField DataField="ENTITYSTS" HeaderText="ENTITYSTS" />
                    <asp:BoundField DataField="PRODGROUP" HeaderText="PRODGROUP" />
                    <asp:BoundField DataField="PRODID" HeaderText="PRODID" />
                    <asp:BoundField DataField="SOPRODID" HeaderText="SOPRODID" />
                    <asp:BoundField DataField="CWORKCTR" HeaderText="CWORKCTR" />
                    <asp:BoundField DataField="WORKNAME" HeaderText="WORKNAME" />
                    <asp:BoundField DataField="PACKFLG" HeaderText="PACKFLG" />
                    <asp:BoundField DataField="LOTPTY" HeaderText="LOTPTY" />
                    <asp:BoundField DataField="LOTGRADE" HeaderText="LOTGRADE" />
                    <asp:BoundField DataField="LOTLEVEL" HeaderText="LOTLEVEL" />
                    <asp:BoundField DataField="ENTITYTQTY" HeaderText="ENTITYTQTY" />
                    <asp:BoundField DataField="STARTDATE" HeaderText="STARTDATE" />
                    <asp:BoundField DataField="TRACKINDATE" HeaderText="TRACKINDATE" />
                    <asp:BoundField DataField="TRACKOUTDATE" HeaderText="TRACKOUTDATE" />
                    <asp:BoundField DataField="CMT" HeaderText="CMT" />
                    <asp:BoundField DataField="SUBLINEID" HeaderText="SUBLINEID" />
                    <asp:BoundField DataField="USERID" HeaderText="USERID" />
                    <asp:BoundField DataField="HOLDCD" HeaderText="HOLDCD" />
                    <asp:BoundField DataField="HOLDDESC" HeaderText="HOLDDESC" />
                    <asp:BoundField DataField="HOLDCTR" HeaderText="HOLDCTR" />
                    <asp:BoundField DataField="WAITIME" HeaderText="WAITIME" />
                    <asp:BoundField DataField="HOLDTIME" HeaderText="HOLDTIME" />
                    <asp:BoundField DataField="PROCESSTIME" HeaderText="PROCESSTIME" />
                    <asp:BoundField DataField="Pendingtime" HeaderText="Pendingtime" />
                    <asp:BoundField DataField="WONBR" HeaderText="WONBR" />
                    <asp:BoundField DataField="LOTOWNER" HeaderText="LOTOWNER" />
                </Columns>
                <RowStyle Font-Names="Arial" Font-Size="Small" />
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:GridView>
        </div><div id="Div3" onclick="return DIV1_onclick()" style="border-right: black 1px solid;
            border-top: black 1px solid; z-index: 104; left: 202px; overflow: auto; border-left: black 1px solid;
            width: 831px; border-bottom: black 1px solid; top: 78px; height: 1px">
            &nbsp;<asp:GridView ID="GridViewTmp" runat="server" AutoGenerateColumns="False" EnableEventValidation = "false" AutoEventWireup="true"
                Height="205px" OnRowCommand="DataGridSum_RowCommand" Width="819px">
                <Columns>
                    <asp:BoundField DataField="PARTID" HeaderText="PARTID" />
                    <asp:BoundField DataField="WORKCTR" HeaderText="WORKCTR" />
                    <asp:BoundField DataField="TTL_QTY" HeaderText="TTL_QTY" />
                    <asp:BoundField DataField="HOLD" HeaderText="HOLD" />
                    <asp:BoundField DataField="PENDING" HeaderText="PENDING" />
                    <asp:BoundField DataField="RUN" HeaderText="RUN" />
                    <asp:BoundField DataField="WAIT" HeaderText="WAIT" />
                </Columns>
                <RowStyle Font-Names="Arial" Font-Size="Small" />
                <HeaderStyle Font-Names="Arial" Font-Size="Small" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
