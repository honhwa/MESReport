<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="QueryCarton.aspx.cs" Inherits="WebReport.Reports.QueryCarton" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1"  runat="server">
    <title>QueryCarton</title>  
    <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd '</script>
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
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <table border="1" width="90%"  >
                <tr>
                    <td colspan="4" style="font-family: Times New Roman;  text-align: center; vertical-align :middle ; height: 60px; font-weight: bold; font-size: 35px;">
                    QueryCarton                      
                    </td>
                </tr>
                <tr>
                <td colspan ="4" style="font-family: Times New Roman;  text-align: center; vertical-align :middle ; font-weight: bold; font-size: 14pt;">
                    <asp:RadioButton id="ByLot" GroupName="ByWhat"  Text="ByLot"  runat="server" AutoPostBack="True" Checked="True" OnCheckedChanged="ByWhat_CheckedChanged"/>
                     &nbsp; &nbsp; &nbsp; By Piece (
                     <asp:RadioButton id="Automotive" GroupName="ByWhat"  Text="Automotive"  runat="server" AutoPostBack="True" OnCheckedChanged="ByWhat_CheckedChanged"/>
                    <asp:RadioButton id="BMW" GroupName="ByWhat"  Text="BMW"  runat="server" AutoPostBack="True" OnCheckedChanged="ByWhat_CheckedChanged"/>
                    <asp:RadioButton id="Panasonic" GroupName="ByWhat"  Text="Panasonic"  runat="server" AutoPostBack="True" OnCheckedChanged="ByWhat_CheckedChanged"/>
                    <asp:RadioButton id="Audi" GroupName="ByWhat"  Text="Audi"  runat="server" AutoPostBack="True" OnCheckedChanged="ByWhat_CheckedChanged"/>
                    )
                </td>
                </tr>
                
                <tr>                  
                  
                    <td  align="left";valign="middle";style="width:10%;height:40px"; >
                       <asp:CheckBox ID="Chkdate" runat="server"  AutoPostBack="True" Font-Bold="True" Font-Size="13pt"
                             OnCheckedChanged="Chkdate_CheckedChanged" Text="開始時間: "/></td>
                    <td  align="left"; valign="middle" style="width:35%;" >
                        <div  id="timePickerLeftPicker" class="timePicker" style="width:100%";>
                            <asp:TextBox ID="txtFromTime" runat="server" Font-Size="11pt" Height="25px" MaxLength="22" Width="160px" Enabled="False"></asp:TextBox>
                            <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,true);"type="button" /></div>
                    </td>
                    <td style="font-family: Times New Roman ;height:40px;width:12% " align="Left"; valign="middle"; >
                        <asp:Label ID="Label1" runat="server" Text="結束時間：" Font-Bold="True" Font-Size="Medium" Height="26px" Width="97%"></asp:Label></td>
                    <td style="font-size: 14pt;font-family: Times New Roman;width:35% " align="left";valign="middle";  >
                        <div id="timePickerRightPicker" class="timePicker" style="width:100%">
                            <asp:TextBox ID="txtToTime" runat="server" Font-Size="11pt" Height="25px" MaxLength="22" Width="160px" Enabled="False" ></asp:TextBox>
                            <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,true);"type="button" /></div></td>
                </tr>
                
                <tr>
                    <td  align="left";valign="middle"style="width: 10%; height: 35px;";>
                    <asp:CheckBox ID="ChkLOTID" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="13pt" OnCheckedChanged="ChkLOTID_CheckedChanged" Text="LOTID:" Height="30px" Width="100px" />
                     </td>
                    <td  colspan="3" align="left";valign="middle" style="height: 35px";>
                    <asp:TextBox ID="txtLOTID" runat="server" Enabled="False" Font-Size="13pt" Width="160px" Height="25px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtLOTID');" id="Button1" />
                    <asp:Label ID="Label4" runat="server" Font-Size="10pt" Text="(注：BY PIECE過賬不能使用LOT查詢,請用拆批后的PANEL)"></asp:Label>
                    </td>
                 </tr>
                 
                    <tr>
                    <td  align="left";valign="middle"style="width: 10%; height: 35px";>
                    <asp:CheckBox ID="ChkCARTON" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="13pt" OnCheckedChanged="ChkCARTON_CheckedChanged" Text="CARTON:" Height="30px" Width="100px" />
                     </td>
                    <td  colspan="3" align="left";valign="middle" style="height: 35px";>
                    <asp:TextBox ID="txtCARTON" runat="server" Enabled="False" Font-Size="13pt" Width="160px" Height="25px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtCARTON');" id="Button2" />
                    </td>
                 </tr>
                 
                 <tr>
                    <td  align="left";valign="middle"style="width:10%; height: 35px;";>
                    <asp:CheckBox ID="ChkPRODID" runat="server" AutoPostBack="True" Font-Bold="True" Font-Size="13pt" OnCheckedChanged="ChkPRODID_CheckedChanged" Text="PRODID:" Height="30px" Width="100px" />
                     </td>
                    <td  colspan="3" align="left";valign="middle" style="height: 35px";>
                    <asp:TextBox ID="txtPRODID" runat="server" Enabled="False" Font-Size="13pt" Width="160px" Height="25px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtPRODID');" id="Button3" />
                    </td>
                 </tr>
                 
                 <tr>
                  <td  align="left";valign="middle"style="width: 10%; height: 35px; font-weight: bold; font-size: large;";>
                        &nbsp; &nbsp;<asp:Label ID="Label3" runat="server" Font-Bold="True"
                            Font-Size="13pt" Height="26px" Text="PANELID：" Width="48%"></asp:Label>
                  </td>
                  <td  colspan="3" align="left";valign="middle" style="height: 35px";>
                    <asp:TextBox ID="txtPANELID" runat="server" Height="25px" Width="160px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtPANELID');" id="Button5" />
                  </td>
                 </tr>
                    <tr>
                    <td  align="left";valign="middle"style="width: 10%; height: 35px; font-weight: bold; font-size: large;";>
                        &nbsp; &nbsp; &nbsp; &nbsp;<asp:Label ID="Label2" runat="server" Font-Bold="True"
                            Font-Size="13pt" Height="26px" Text="SN：" Width="46%"></asp:Label></td>
                    <td  colspan="3" align="left";valign="middle" style="height: 35px";>
                    <asp:TextBox ID="txtSN" runat="server" Height="25px" Width="160px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('txtSN');" id="Button4" />
                    &nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp;<asp:Button ID="Refresh" runat="server" Font-Bold="True" Font-Size="14pt" Height="29px"
                            OnClick="Refresh_Click" Text="刷新" Width="61px" />
                    &nbsp; &nbsp; &nbsp; &nbsp;<asp:Button ID="ButQuery" runat="server" Font-Bold="True" Font-Size="14pt" Height="29px"
                            OnClick="ButQuery_Click" Text="查詢" Width="61px" />      
                        
                    </td>
                 </tr>
                 
                  <tr>
                    <td colspan="4" style="font-family: Times New Roman;  text-align:left; vertical-align :middle ; height: 40px; font-weight: bold; font-size: 20px;">
                    <asp:Label ID="lblErrMsg" runat="server"  Font-Underline="True" ForeColor="Red" Width="90%" Height="30px"></asp:Label>                 
                    </td>
                </tr>
           </table>      
        </div>
        <table style="font-size: 12pt; width: 736px">
            <tr>
                <td style="width: 100px">
                    &nbsp;
<%--                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        Height="385px" Width="1000px" Visible="False">
                        <LocalReport ReportPath="RDLC\LotQueryCarton.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>--%>
<%--                    <rsweb:ReportViewer ID="ReportViewer2" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        Height="385px" Width="1000px" Visible="False">
                        <LocalReport ReportPath="RDLC\PieceQueryCarton.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>--%>
                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Verdana" Font-Size="8pt"
                        Height="385px" Width="1000px" Visible="False">
                        <LocalReport ReportPath="RDLC\LotQueryCarton.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                    <rsweb:ReportViewer ID="ReportViewer2" runat="server"  Font-Names="Verdana" Font-Size="8pt"
                        Height="385px" Width="1000px" Visible="False">
                        <LocalReport ReportPath="RDLC\PieceQueryCarton.rdlc">
                        </LocalReport>
                    </rsweb:ReportViewer>
                    &nbsp;

                </td>
            </tr>
        </table>
        <div id="getDIV"  hidden="true">
	        <textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
            <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
        </div>
        &nbsp; &nbsp; &nbsp; &nbsp;
    </form>
</body>
</html>

