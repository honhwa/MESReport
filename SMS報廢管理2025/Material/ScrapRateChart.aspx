<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ScrapRateChart.aspx.cs" Inherits="Material_ScrapRateChart" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>單產品月趨勢圖</title>
    <script language="javascript" type="text/javascript">
        function document.onkeydown() {
            if (event.keyCode == 27) {
                window.close();
            }
        }
    </script>
</head>
<body style="margin:0;">
    <form id="form1" runat="server">
        <div>
            <asp:Chart ID="ChartData" runat="server" Height="400px" Visible="False" 
                Width="900px">
                <series>
                    <asp:Series Name="Series1">
                    </asp:Series>
                </series>
                <chartareas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </chartareas>
            </asp:Chart>
            <br />
            <asp:GridView ID="gvData" runat="server" Font-Size="10pt" 
                AutoGenerateColumns="False" 
                DataKeyNames="PROJECT_NAME,COMP_M_GROUP" Width="900px" 
                onrowcreated="gvData_RowCreated">
                <Columns>
                        <asp:BoundField DataField="PROJECT_NAME" HeaderText="ProjectName"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"/></asp:BoundField>
                        <asp:BoundField DataField="COMP_M_GROUP" HeaderText="物料類別"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left"  Width="150px" /></asp:BoundField>
                        <asp:BoundField DataField="M5" HeaderText="M5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M4" HeaderText="M4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M3" HeaderText="M3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M2" HeaderText="M2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M1" HeaderText="M1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M0" HeaderText="MTD" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W4" HeaderText="W4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W3" HeaderText="W3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W2" HeaderText="W2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W1" HeaderText="W1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D7" HeaderText="D7" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D6" HeaderText="D6" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D5" HeaderText="D5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D4" HeaderText="D4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D3" HeaderText="D3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D2" HeaderText="D2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D1" HeaderText="D1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="YellowGreen" />
            </asp:GridView>
            <asp:GridView ID="gvDataPartID" runat="server" Font-Size="10pt" 
                AutoGenerateColumns="False" 
                DataKeyNames="ProdID,PartID" Width="900px" 
                onrowcreated="gvDataPartID_RowCreated">
                <Columns>
                        <asp:BoundField DataField="ProdID" HeaderText="產品號碼"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px" /></asp:BoundField>
                        <asp:BoundField DataField="PartID" HeaderText="物料料號"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px" /></asp:BoundField>
                        <asp:BoundField DataField="M5" HeaderText="M5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M4" HeaderText="M4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M3" HeaderText="M3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M2" HeaderText="M2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M1" HeaderText="M1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="M0" HeaderText="MTD" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W4" HeaderText="W4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W3" HeaderText="W3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W2" HeaderText="W2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="W1" HeaderText="W1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D7" HeaderText="D7" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D6" HeaderText="D6" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D5" HeaderText="D5" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D4" HeaderText="D4" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D3" HeaderText="D3" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D2" HeaderText="D2" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                        <asp:BoundField DataField="D1" HeaderText="D1" DataFormatString="{0:N3}"><HeaderStyle Wrap="False" /><ItemStyle Wrap="False" HorizontalAlign="Right" /></asp:BoundField>
                </Columns>
                <HeaderStyle BackColor="YellowGreen" />
            </asp:GridView>
        </div>
    </form>
</body>
</html>
