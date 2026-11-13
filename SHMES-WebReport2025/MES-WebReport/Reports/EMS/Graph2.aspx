<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Graph2.aspx.cs" Inherits=" WebReport.Reports.Graph2" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
        <div>
            <%--<rsweb:reportviewer id="reportViewer5" runat="server" height="350px" showtoolbar="False"
                width="350px"></rsweb:reportviewer>--%>
    
            <rsweb:ReportViewer ID="reportViewer5" runat="server" height="350px" showtoolbar="False" width="350px">
            </rsweb:ReportViewer>
    
        </div>
    </form>
</body>
</html>
