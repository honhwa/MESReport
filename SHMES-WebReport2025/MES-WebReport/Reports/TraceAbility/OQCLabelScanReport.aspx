<%@ Page Language="C#" AutoEventWireup="true" Inherits="WebReport.Reports.OQCLabelScanReport" Codebehind="OQCLabelScanReport.aspx.cs" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>OQCLabelScan</title>
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
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
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <div>
                &nbsp; &nbsp; &nbsp;<table style="width: 1018px">
                    <tr>
                        <td>
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                            &nbsp; &nbsp; &nbsp; <strong>
                           <asp:Label ID="Label1" runat="server" Font-Size="20pt" ForeColor="Black" Text="OQC Label Scan"></asp:Label></strong></td>
                    </tr>
                </table>
                <br />
                <table style="font-size: 12pt" width="1000">
                    <tr>
                        <td style="width: 600px; height: 40px">
                            &nbsp;&nbsp;<asp:CheckBox ID="ChkLOTID" runat="server" Font-Bold="True" AutoPostBack="True" 
                            Height="30px" OnCheckedChanged="ChkLOTID_CheckedChanged" Text="LOTID:" Width="100px" EnableTheming="True" Font-Italic="False" Font-Names="新細明體" Font-Size="Medium" Font-Strikeout="False" />
                            &nbsp; &nbsp;
                            <asp:TextBox ID="txtLOTID" runat="server" Width="150px" Height="30px" Enabled="False" Font-Bold="False" ></asp:TextBox>
                            <input type="button" value="..." onclick="OnClick=getMulti_Line('txtLOTID');" id="Button1" /></td>
                        <td style="width: 600px; height: 40px">
                            &nbsp;&nbsp;<asp:CheckBox ID="ChkPANELID" runat="server" Font-Bold="True" AutoPostBack="True" 
                            Height="30px" OnCheckedChanged="ChkPANELID_CheckedChanged" Text="PANELID:" Width="100px" EnableTheming="True" Font-Italic="False" Font-Names="新細明體" Font-Size="Medium" Font-Strikeout="False" />
                            &nbsp; &nbsp; 
                            <asp:TextBox ID="txtPANELID" runat="server" Height="30px" Width="150px" Enabled="False" Font-Bold="False"></asp:TextBox>
                            <input type="button" value="..." onclick="OnClick=getMulti_Line('txtPANELID');" id="Button2" /></td>
                        <td style="width: 600px; height: 40px; text-align: left">
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 600px; height: 30px">
                            &nbsp;&nbsp;<asp:CheckBox ID="ChkSN" runat="server" Font-Bold="True" AutoPostBack="True" 
                             Height="30px" OnCheckedChanged="ChkSN_CheckedChanged" Text="SN:" Width="100px" EnableTheming="True" Font-Italic="False" Font-Names="新細明體" Font-Size="Medium" Font-Strikeout="False" />
                             &nbsp; &nbsp; 
                            <asp:TextBox ID="txtSN" runat="server" Height="30px" Width="150px" Enabled="False" Font-Bold="False" ></asp:TextBox>
                            <input type="button" value="..." onclick="OnClick=getMulti_Line('txtSN');" id="Button3" /></td>
                        <td style="width: 600px; height: 30px">
                            &nbsp;&nbsp;<asp:CheckBox ID="ChkBarcode" runat="server" Font-Bold="True"  AutoPostBack="True" 
                            Height="30px" OnCheckedChanged="ChkBarcode_CheckedChanged" Text="Barcode:" Width="100px" EnableTheming="True" Font-Italic="False" Font-Names="新細明體" Font-Size="Medium" Font-Strikeout="False" />
                            &nbsp; &nbsp;
                            <asp:TextBox ID="txtBarcode" runat="server" Height="30px" Width="150px" Enabled="False" Font-Bold="False"></asp:TextBox>
                            <input type="button" value="..." onclick="OnClick=getMulti_Line('txtBarcode');" id="Button4" /></td>
                        <td style="width:600px; height: 30px; text-align: left">
                            <asp:Button ID="Refresh" runat="server" Font-Bold="True" Font-Size="14pt" Height="30px" 
                            OnClick="Refresh_Click" Text="Refresh" Width="85px" />
                            &nbsp; &nbsp;<asp:Button ID="ButQ" runat="server" Font-Bold="True" Font-Size="14pt" Height="30px"
                            OnClick="ButQ_Click" Text="Query" Width="61px" /></td>
                    </tr>
                </table>
                &nbsp;<br />
                &nbsp;<br />
                <asp:Label ID="lblErr" runat="server" Font-Names="Arial" Font-Size="Small" Font-Underline="True"
                    ForeColor="Red" Width="251px"></asp:Label><br />
                &nbsp; &nbsp; &nbsp; &nbsp;
                <br />
                <%--<rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="973px" Font-Names="Verdana" Font-Size="8pt" Height="400px" Visible="False">
                    <LocalReport ReportPath="RDLC\OQCLabelScan.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>--%>

                <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="973px" Font-Names="Verdana" Font-Size="8pt" Height="400px" Visible="False">
                    <LocalReport ReportPath="RDLC\OQCLabelScan.rdlc">
                    </LocalReport>
                </rsweb:ReportViewer>

            </div>
        </div>
        <div id="getDIV"  hidden="true">
		    <textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
            <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
	    </div>
    </form>
</body>
</html>
