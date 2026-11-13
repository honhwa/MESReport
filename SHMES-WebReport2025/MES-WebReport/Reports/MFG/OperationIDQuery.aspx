<%@ Page Language="C#" EnableEventValidation ="false" AutoEventWireup="true" CodeBehind="OperationIDQuery.aspx.cs" Inherits="Report.Reports.MFG.OperationIDQuery" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>无标题页</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">        dateFormat = 'yyyy-mm-dd'</script>
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript" >
        function getMulti_Line(textBoxId) {
            var div = document.getElementById("getDIV");
            div.hidden = false;
            var btn = document.getElementById("btn");
            btn.value = textBoxId;
        }

        function btn_close() {
            var btn = document.getElementById("btn");
            var div = document.getElementById("getDIV");
            var textbox = document.getElementById(btn.value);
            var input = document.getElementById("txtInput");

            var myString = input.value;
            var myArray = myString.split(String.fromCharCode(10));
            var retStr = "";
            for (i = 0; i < myArray.length; i++) {
                var str = myArray[i];
                var array = str.split(String.fromCharCode(9));
                for (j = 0; j < array.length; j++) {
                    var strSub = array[j];

                    strSub = strSub.replace(String.fromCharCode(13), "");

                    if (strSub.length > 0) {
                        retStr += strSub + ",";
                    }
                }
            }
            window.returnValue = retStr;

            textbox.value = retStr;
            div.hidden = true;
        }
    </script>
    <style type="text/css">
    </style>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
    <br />
    <table>
    <tr>
        <td style="width: 110px; height: 4px">開始日期</td>
        <td style="width: 219px; height: 4px">
            <div id="timePickerLeftPicker" class="timePicker">
                <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Style="left: -60px; position: relative;
                    top: -9px" Width="120px"></asp:TextBox></div>
                <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"
                    style="left: 156px; width: 24px;  top:-4px; position: relative;" type="button" />    
        </td>
        <td style="width: 100px; height: 4px">結束日期</td>
        <td style="height: 4px; width: 237px;">
            <div id="timePickerRightPicker" class="timePicker">
                <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Style="left: -78px; position: relative;
                    top: -11px" Width="120px"></asp:TextBox>
                <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"
                    style="left: -49px; width: 25px; position: relative; top: -6px" type="button" />
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 50px; height: 7px;" align="left">機 種</td>
        <td style="width: 121px; text-align: left; height: 7px;">
            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                width: 201px; border-bottom: black 1px solid; top: 78px; height: 100px">
                <asp:CheckBox ID="chkAllProdID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllProdID_CheckedChanged" Text="ALL" Width="100%" />
                <asp:CheckBoxList ID="lstProdId" runat="server" OnSelectedIndexChanged="lstProdId_SelectedIndexChanged" Width="99%">
                </asp:CheckBoxList>
            </div>
        </td>
        <td style="width: 50px; height: 7px;" align="left">線 別</td>
        <td style="width: 121px; text-align: left; height: 7px;">
            <div style="border-right: black 1px solid; border-top: black 1px solid; overflow-y: scroll;
                z-index: 104; left: 202px; overflow-x: hidden; border-left: black 1px solid;
                width: 201px; border-bottom: black 1px solid; top: 78px; height: 100px">
                <asp:CheckBox ID="chkAllSubLineID" runat="server" AutoPostBack="True" OnCheckedChanged="chkAllSubLineID_CheckedChanged" Text="ALL" Width="100%" />
                <asp:CheckBoxList ID="lstSubLineID" runat="server" OnSelectedIndexChanged="lstSubLineID_SelectedIndexChanged" Width="99%">
                </asp:CheckBoxList>
            </div>
        </td>
    </tr> 
    <tr>
        <td>PanelID</td>
        <td  colspan="2" align="left";valign="middle" style="height: 35px";>
            <asp:TextBox ID="txtpld" runat="server" Height="25px" Width="160px"></asp:TextBox>
            <input type="button" value="..." onclick="OnClick=getMulti_Line('txtpld');" id="Button5" />
        </td>
        <td>
            <asp:Button ID="btnSerch" runat="server" OnClick="btnSerch_Click" Text="查詢" />
            &nbsp;&nbsp;&nbsp;&nbsp;
            <asp:ImageButton ID="ImageButton1" runat="server" Height="17px" Width="25px" ImageUrl="~/Images/xls.jpg" OnClick="ImageButton1_Click" />
            導出</td>
    </tr>
        <tr>
            <td colspan="4" >
                <asp:Label ID="lblErrMsg" runat="server" style="position: relative;"></asp:Label>
            </td>
        </tr>
    </table>
    </div>
    <div>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
            CellPadding="4" ForeColor="#333333" GridLines="Both" OnPageIndexChanging="GridView1_PageIndexChanging"
            PageSize="30" Font-Size="Small" Width="817px">
            <RowStyle BackColor="#EFF3FB" />
            <Columns>
                <asp:BoundField DataField="FABID" HeaderText="FABID" />
                <asp:BoundField DataField="ENTITYID" HeaderText="ENTITYID" />
                <asp:BoundField DataField="SubLineID" HeaderText="SubLineID" />
                <asp:BoundField DataField="PRODID" HeaderText="PRODID" />
                <asp:BoundField DataField="OperationID" HeaderText="OperationID" />
                <asp:BoundField DataField="WORKCTR" HeaderText="WORKCTR" />
                <asp:BoundField DataField="TRANSDATE" HeaderText="TRANSDATE" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}"/>
                <asp:BoundField DataField="USERID" HeaderText="USERID" />
            </Columns>
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <EditRowStyle BackColor="#2461BF" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    </div>
        <div id="getDIV"  hidden="true">
	        <textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
            <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
        </div>
    </form>
</body>
</html>
