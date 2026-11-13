<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BySNQyeryPanelID.aspx.cs" Inherits="WebReport.Reports.By_Lot_Qyery_Cartid" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>

    <script type="text/javascript" >
        function getMulti_Line(textBoxId)     
        {   
            var div = document.getElementById("getDIV"); 
            div.hidden = false;
            var btn = document.getElementById("btn");
            btn.value = textBoxId;
        }

        function btn_close()
        {     
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


    <title>By批查箱號</title>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        &nbsp;<table style="left: 41px; width: 503px; position: relative; top: 0px; height: 142px">
            <tr>
                <td colspan="2" style="width: 653px; height: 12px">
                    <span style="font-size: 16pt"><strong>&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp;</strong><span style="font-size: 25pt"><strong>&nbsp;&nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; <span style="font-size: 20pt"> &nbsp;<asp:Label ID="Label2" runat="server"
                                Font-Size="X-Large" ForeColor="DodgerBlue" Text="SN Query PanelID" Width="290px"></asp:Label><br />
                            </span></strong></span></span></td>
            </tr>
            <tr style="font-size: 12pt">
                <td colspan="2" style="width: 653px; height: 9px">
                    <strong><span><span style="font-size: 13pt">&nbsp;<span style="color: #000000"><span
                        style="font-size: 12pt"><span style="font-size: 13pt">Please&nbsp; &nbsp;input &nbsp;&nbsp;
                            the &nbsp; &nbsp;SN: &nbsp; </span> </span>
                    <asp:TextBox ID="txtEntityId" runat="server" Enabled="true" Font-Size="13pt" Width="160px" Height="25px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtEntityId');" id="Button1" />
                    &nbsp; &nbsp; 
                    <asp:Button ID="butQuery" runat="server" Font-Bold="True" Font-Size="12pt" OnClick="butQuery_Click"
                        Text="查詢" BorderStyle="Ridge" /></span></span></span></strong></td>
            </tr>
             <tr style="font-size: 12pt">
                <td colspan="2" style="width: 653px; height: 9px">
                    <strong><span><span style="font-size: 13pt">&nbsp;<span style="color: #000000"><span
                        style="font-size: 12pt"><span style="font-size: 13pt">Please
                        input the PanelID:</span> </span>
                    <asp:TextBox ID="txtPanelID" runat="server" Enabled="true" Font-Size="13pt" Width="160px" Height="25px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtPanelID');" id="Button2" />
                    &nbsp; &nbsp; 
                    <asp:Button ID="btnQuery" runat="server" Font-Bold="True" Font-Size="12pt" 
                        Text="查詢" BorderStyle="Ridge" OnClick="btnQuery_Click" /></span></span></span></strong></td>
            </tr>
            <tr style="font-size: 12pt">
                <td colspan="2" style="width: 653px;">
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
            <tr style="font-size: 12pt">
                <td colspan="2" style="width: 653px; height: 1px">
                    &nbsp;
                    <asp:Label ID="LblErr" runat="server" ForeColor="Red" Visible="False" Width="338px"></asp:Label></td>
            </tr>
            <tr style="font-size: 12pt">
                <td colspan="2" style="width: 653px; height: 421px;">
<%--                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="743px" Height="395px">
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="743px" Height="395px">
                    </rsweb:ReportViewer>
                </td>
            </tr>
        </table>
    
    </div>
    <div id="getDIV"  hidden="true">
		<textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
        <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
	</div>
        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br />
        <br />
        &nbsp;<br />
    </form>
</body>
</html>
