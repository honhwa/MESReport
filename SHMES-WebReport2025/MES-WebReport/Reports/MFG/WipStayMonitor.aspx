<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WipStayMonitor.aspx.cs" Inherits="Report.Reports.MFG.WipStayMonitor" %>

<%@ Register Assembly="DundasWebChart" Namespace="Dundas.Charting.WebControl" TagPrefix="DCWC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>8179 WIP STAY MONITOR</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="1100">
            <tr>
                <td style="width: 1500px; height: 26px; text-align: center">
                    <strong><span style="font-size: 16pt; font-family: Arial">&nbsp; &nbsp; &nbsp; &nbsp;
                        &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; PASSINGBOX
                        WIP MONITOR</span></strong></td>
            </tr>
        </table>
        <DCWC:Chart ID="Chart1" runat="server" Height="721px" Palette="Fire" Width="1014px">
            <Legends>
                <DCWC:Legend Alignment="Center" Docking="Bottom" Name="Default">
                </DCWC:Legend>
            </Legends>
            <Titles>
                <DCWC:Title Name="Title1">
                </DCWC:Title>
            </Titles>
            <Series>
                <DCWC:Series BackGradientEndColor="162, 164, 255" BorderColor="64, 64, 64" Name="Series1"
                    ShadowOffset="1">
                    <SmartLabels Enabled="True" />
                </DCWC:Series>
                <DCWC:Series BackGradientEndColor="188, 255, 255" BorderColor="64, 64, 64" Name="Series2"
                    ShadowOffset="1">
                    <SmartLabels Enabled="True" />
                </DCWC:Series>
            </Series>
            <ChartAreas>
                <DCWC:ChartArea Name="Default">
                    <AxisY>
                        <LabelStyle ShowEndLabels="False" />
                        <MajorGrid Enabled="False" />
                        <MajorTickMark Enabled="False" />
                    </AxisY>
                    <AxisX>
                        <MajorGrid Enabled="False" />
                    </AxisX>
                </DCWC:ChartArea>
            </ChartAreas>
        </DCWC:Chart>
    
    </div>
    </form>
</body>
</html>
