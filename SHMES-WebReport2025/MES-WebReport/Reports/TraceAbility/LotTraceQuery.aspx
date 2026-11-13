<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LotTraceQuery.aspx.cs" Inherits="WebReport.Reports.LotTraceQuery" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
  <title>未命名頁面</title>  
<%--      <link href="../Stylesheet.css" rel="Stylesheet" type="text/html" />--%>
    <script type ="text/javascript" src ="popcalendar.js"></script>
<script language="javascript" type="text/javascript">
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
// <!CDATA[

dateFormat = 'yyyy-mm-dd'

function report_onclick() {

}

// ]]>
</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<br />
            <table style="width: 1018px">
                <tr>
                    <td>
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp;
                        <asp:Label ID="Label1" runat="server" Font-Size="X-Large" ForeColor="Blue" Text="Lot Trace Query"></asp:Label></td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td style="width: 294px; height: 30px;">
                        請輸入批號：<asp:TextBox ID="txtLot" runat="server" Width="143px"></asp:TextBox>
                       <input type="button" value="..." onclick="OnClick=getMulti_Line('txtLot');" id="Button2" /> </td>
                    <td style="width: 231px; height: 30px;">
                        <asp:Button ID="ButQ" runat="server" BorderStyle="Ridge" Font-Bold="True" Font-Size="12pt"
                            OnClick="ButQ_Click" Text="查詢" /></td>
                </tr>
            </table>
            &nbsp;<br />
            <br />
            <br />
            <table style="width: 1018px">
                <tr>
                    <td style="height: 21px">
                        <%--<rsweb:reportviewer id="ReportViewer1" runat="server" height="472px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote"></rsweb:reportviewer>--%>

                        <rsweb:ReportViewer ID="ReportViewer1" runat="server" height="472px" width="961px" Font-Names="Verdana" Font-Size="8pt" ProcessingMode="Remote">
                        </rsweb:ReportViewer>

                    </td>
                </tr>
            </table>
            <br />
            <br />
            &nbsp; &nbsp; &nbsp; &nbsp;
            <br />
        </div>
    </div>
    <div id="getDIV"  hidden="true">
		<textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
        <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
	</div>
    </form>
</body>
</html>

