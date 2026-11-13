<%@ Page language="c#" AutoEventWireup="false" Inherits="webapp02.WebForm2" Codebehind="multi_line.aspx.cs" %>
<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
  Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.0 Transitional//EN" >
<html xmlns="http://www.w3.org/1999/xhtml" >
	<head>
		<title>多行輸入窗口</title>
		<script language="JavaScript" type="text/JavaScript">
<!--
function window_onunload()
{
	var myString=document.form1.txtInput.value;
	var myArray=myString.split(String.fromCharCode(13));
	var retStr="";
	for(i=0;i<myArray.length;i++)
	{
		var str=myArray[i];
		var array=str.split(String.fromCharCode(9));		
		for(j=0;j<array.length;j++)
		{
			var strSub=array[j];
			
			strSub=strSub.replace(String.fromCharCode(10),"");
			
			if(strSub.length>0)
			{
				retStr+=strSub+",";
			}
		}
	}	
	window.returnValue=retStr;
}
//-->
		</script>
	</head>
	<body  onunload="window_onunload();">
		<div style="text-align:center">
			<form id="form1" runat="server">
                <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
				<textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 430px; HEIGHT: 530px"></textarea><br/>
				<input onclick="window.close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
			</form>
		</div>
	</body>
</html>
