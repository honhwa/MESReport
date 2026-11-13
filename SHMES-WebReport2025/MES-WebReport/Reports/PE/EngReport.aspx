<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="EngReport.aspx.cs" Inherits="Report.Reports.PE.EngReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>EngReport</title>
      <link href="../../App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="popcalendar.js"></script>
    <script type="text/javascript">dateFormat = 'yyyy-mm-dd'</script>
</head>
<body style="background-image: url(../../Images/right_bg.jpg)">
    <form id="form1" runat="server">
        <div id="Div1" class="LabelText">
            <table style="width: 945px; text-align: right">
                <tr align="center">
                    <td style="height: 43px">
                        SHOP</td>
                    <td style="height: 43px; text-align: left; width: 257px;">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal"
                            Width="249px">
                            <asp:ListItem Selected="True">MODULE</asp:ListItem>
                            <asp:ListItem>CELL2</asp:ListItem>
                        </asp:RadioButtonList></td>
                    <td style="width: 221px; height: 43px">
                    </td>
                    <td style="width: 359px; height: 43px">
                        </td>
                </tr>
                                          <tr>
                                        <td style="vertical-align: middle; width: 73px; height: 28px">
                                            <div id="timePickerLabelLeft" class="LabelText" style="vertical-align: middle; text-align: center">
                                                <span style="font-family: Arial"><strong>開始時間</strong>&nbsp;</span></div>
                                        </td>
                                        <td style="width: 158px; font-family: Times New Roman; height: 28px">
                                            <div id="timePickerLeftPicker" class="timePicker">
                                                <asp:TextBox ID="txtFromTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                                <input id="btnFromTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtFromTime, dateFormat,-1,-1,false);"
                                                    type="button" /></div>
                                        </td>
                                        <td style="font-family: Times New Roman; height: 28px">
                                            <div id="timePickerLabelRight" class="LabelText">
                                                <strong>結束時間</strong></div>
                                        </td>
                                        <td style="font-size: 12pt; width: 149px; font-family: Times New Roman; height: 28px">
                                            <div id="timePickerRightPicker" class="timePicker">
                                                <asp:TextBox ID="txtToTime" runat="server" MaxLength="19" Width="120px"></asp:TextBox>
                                                <input id="btnToTime" class="CalendarBotton" onclick="popUpCalendar(this, form1.txtToTime, dateFormat,-1,-1,false);"
                                                    type="button" /></div>
                                        </td>
                                    </tr>
 
                
                <tr>
                    <td style="height: 21px">
                        <div id="Div3" class="LabelText">
                            Group</div>
                    </td>
                    <td style="height: 21px; width: 257px;" align="left">
                        <div id="partIdList">
                            <asp:DropDownList ID="dlGroup" runat="server" Width="158px" OnSelectedIndexChanged="dlGroup_SelectedIndexChanged">
                            </asp:DropDownList></div>
                    </td>
                    <td style="width: 221px; height: 21px">
                        <div id="Div4" class="LabelText">
                            機種</div>
                    </td>
                    <td style="width: 359px; height: 21px" align="left">
                        <asp:DropDownList ID="DlProdID" runat="server" Width="158px" OnSelectedIndexChanged="DlProdID_SelectedIndexChanged">
                        </asp:DropDownList></td>
                </tr>
                <tr>
                    <td style="height: 43px">
                        <div id="subLineLabel" class="LabelText">
                            Lot Type</div>
                    </td>
                    <td style="height: 43px; width: 257px;" align="left">
                        <div id="subLineList">
                            <div id="Div7" onclick="return DIV1_onclick()" style="border-right: black 1px solid;
                                border-top: black 1px solid; z-index: 104; left: 202px; overflow: auto; border-left: black 1px solid;
                                width: 167px; border-bottom: black 1px solid; top: 78px; height: 83px">
                                <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True" OnCheckedChanged="CheckBox1_CheckedChanged"
                                    Text="全選" /><asp:CheckBoxList ID="cblLotType" runat="server" Height="22px" Width="99px">
                                    </asp:CheckBoxList></div>
                        </div>
                    </td>
                    <td style="width: 221px; height: 43px">
                    </td>
                    <td style="width: 359px; height: 43px">
                        </td>
                </tr>
                <tr>
                    <td style="height: 20px">
                    </td>
                    <td style="height: 20px; width: 257px;">
                    </td>
                    <td style="width: 221px; height: 20px">
                    </td>
                    <td style="width: 359px; height: 20px">
                        <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" Text="查詢報表" /></td>
                </tr>
                <tr>
                </tr>
            </table>
            <table>
            <tr>
            <td style="width: 1056px">
            <asp:Label ID="lblErrMsg" runat="server" Font-Names="Times New Roman" Font-Size="11pt"
                ForeColor="Red" Width="98px"></asp:Label></td>
            <td>
                <asp:ImageButton ID="ImageButton1" runat="server" Height="17px" Width="25px" ImageUrl="xls.jpg" OnClick="ImageButton1_Click" /></td>
                </tr>
                </table>
        <div id="Div8" onclick="return DIV1_onclick()" style="border-right: black 1px solid;
                                border-top: black 1px solid; z-index: 104; left: 202px; overflow: auto; border-left: black 1px solid;
                                width: 956px; border-bottom: black 1px solid; top: 78px; height: 350px">
            <asp:DataGrid ID="DataGrid1" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                Height="1px" Width="221px" AutoGenerateColumns="False" OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditItemStyle BackColor="#2461BF" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <AlternatingItemStyle BackColor="White" />
                <ItemStyle BackColor="#EFF3FB" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
<asp:BoundColumn DataField="Group" HeaderText="Group">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ProdID" HeaderText="ProdID">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Input" HeaderText="Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PlanYield" HeaderText="PlanYield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="ActualYield" HeaderText="ActualYield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top1" HeaderText="Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top1y" HeaderText="Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top2" HeaderText="Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top2y" HeaderText="Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top3" HeaderText="Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top3y" HeaderText="Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top4" HeaderText="Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top4y" HeaderText="Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top5" HeaderText="Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top5y" HeaderText="Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
																				<asp:BoundColumn DataField="Top6" HeaderText="Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top6y" HeaderText="Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
																				<asp:BoundColumn DataField="Top7" HeaderText="Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top7y" HeaderText="Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
																				<asp:BoundColumn DataField="Top8" HeaderText="Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top8y" HeaderText="Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
																				<asp:BoundColumn DataField="Top9" HeaderText="Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top9y" HeaderText="Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
																				<asp:BoundColumn DataField="Top10" HeaderText="Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="Top10y" HeaderText="Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M1Input" HeaderText="M1-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1ActualYield" HeaderText="M1-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top1" HeaderText="M1-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top1y" HeaderText="M1-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top2" HeaderText="M1-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top2y" HeaderText="M1-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top3" HeaderText="M1-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top3y" HeaderText="M1-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top4" HeaderText="M1-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top4y" HeaderText="M1-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top5" HeaderText="M1-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top5y" HeaderText="M1-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M1Top6" HeaderText="M1-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top6y" HeaderText="M1-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top7" HeaderText="M1-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top7y" HeaderText="M1-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top8" HeaderText="M1-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top8y" HeaderText="M1-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top9" HeaderText="M1-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top9y" HeaderText="M1-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top10" HeaderText="M1-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M1Top10y" HeaderText="M1-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="COGInput" HeaderText="COG-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGActualYield" HeaderText="COG-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop1" HeaderText="COG-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop1y" HeaderText="COG-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop2" HeaderText="COG-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop2y" HeaderText="COG-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop3" HeaderText="COG-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop3y" HeaderText="COG-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop4" HeaderText="COG-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop4y" HeaderText="COG-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop5" HeaderText="COG-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop5y" HeaderText="COG-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="COGTop6" HeaderText="COG-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop6y" HeaderText="COG-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop7" HeaderText="COG-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop7y" HeaderText="COG-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop8" HeaderText="COG-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop8y" HeaderText="COG-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop9" HeaderText="COG-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop9y" HeaderText="COG-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop10" HeaderText="COG-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COGTop10y" HeaderText="COG-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="COG1Input" HeaderText="COG1Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1ActualYield" HeaderText="COG1Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top1" HeaderText="COG1Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top1y" HeaderText="COG1Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top2" HeaderText="COG1Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top2y" HeaderText="COG1Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top3" HeaderText="COG1Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top3y" HeaderText="COG1Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top4" HeaderText="COG1Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top4y" HeaderText="COG1Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top5" HeaderText="COG1Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top5y" HeaderText="COG1Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="COG1Top6" HeaderText="COG1Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top6y" HeaderText="COG1Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top7" HeaderText="COG1Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top7y" HeaderText="COG1Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top8" HeaderText="COG1Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top8y" HeaderText="COG1Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top9" HeaderText="COG1Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top9y" HeaderText="COG1Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top10" HeaderText="COG1Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="COG1Top10y" HeaderText="COG1Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>



										<asp:BoundColumn DataField="M2COGInput" HeaderText="M2COG-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGActualYield" HeaderText="M2COG-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop1" HeaderText="M2COG-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop1y" HeaderText="M2COG-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop2" HeaderText="M2COG-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop2y" HeaderText="M2COG-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop3" HeaderText="M2COG-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop3y" HeaderText="M2COG-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop4" HeaderText="M2COG-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop4y" HeaderText="M2COG-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop5" HeaderText="M2COG-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop5y" HeaderText="M2COG-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M2COGTop6" HeaderText="M2COG-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop6y" HeaderText="M2COG-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop7" HeaderText="M2COG-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop7y" HeaderText="M2COG-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop8" HeaderText="M2COG-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop8y" HeaderText="M2COG-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop9" HeaderText="M2COG-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop9y" HeaderText="M2COG-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop10" HeaderText="M2COG-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2COGTop10y" HeaderText="M2COG-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M2Input" HeaderText="M2-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2ActualYield" HeaderText="M2-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top1" HeaderText="M2-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top1y" HeaderText="M2-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top2" HeaderText="M2-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top2y" HeaderText="M2-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top3" HeaderText="M2-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top3y" HeaderText="M2-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top4" HeaderText="M2-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top4y" HeaderText="M2-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top5" HeaderText="M2-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top5y" HeaderText="M2-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M2Top6" HeaderText="M2-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top6y" HeaderText="M2-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top7" HeaderText="M2-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top7y" HeaderText="M2-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top8" HeaderText="M2-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top8y" HeaderText="M2-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top9" HeaderText="M2-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top9y" HeaderText="M2-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top10" HeaderText="M2-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M2Top10y" HeaderText="M2-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										                                        <asp:BoundColumn DataField="PCBInput" HeaderText="PCB-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBActualYield" HeaderText="PCB-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop1" HeaderText="PCB-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop1y" HeaderText="PCB-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop2" HeaderText="PCB-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop2y" HeaderText="PCB-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop3" HeaderText="PCB-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop3y" HeaderText="PCB-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop4" HeaderText="PCB-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop4y" HeaderText="PCB-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop5" HeaderText="PCB-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop5y" HeaderText="PCB-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="PCBTop6" HeaderText="PCB-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop6y" HeaderText="PCB-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop7" HeaderText="PCB-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop7y" HeaderText="PCB-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop8" HeaderText="PCB-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop8y" HeaderText="PCB-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop9" HeaderText="PCB-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop9y" HeaderText="PCB-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop10" HeaderText="PCB-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="PCBTop10y" HeaderText="PCB-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
																		        <asp:BoundColumn DataField="TAILFPCInput" HeaderText="TAILFPC-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCActualYield" HeaderText="TAILFPC-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop1" HeaderText="TAILFPC-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop1y" HeaderText="TAILFPC-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop2" HeaderText="TAILFPC-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop2y" HeaderText="TAILFPC-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop3" HeaderText="TAILFPC-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop3y" HeaderText="TAILFPC-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop4" HeaderText="TAILFPC-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop4y" HeaderText="TAILFPC-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop5" HeaderText="TAILFPC-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop5y" HeaderText="TAILFPC-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="TAILFPCTop6" HeaderText="TAILFPC-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop6y" HeaderText="TAILFPC-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop7" HeaderText="TAILFPC-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop7y" HeaderText="TAILFPC-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop8" HeaderText="TAILFPC-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop8y" HeaderText="TAILFPC-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop9" HeaderText="TAILFPC-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop9y" HeaderText="TAILFPC-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop10" HeaderText="TAILFPC-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="TAILFPCTop10y" HeaderText="TAILFPC-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										

										<asp:BoundColumn DataField="M3U1Input" HeaderText="M3U1-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1ActualYield" HeaderText="M3U1-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top1" HeaderText="M3U1-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top1y" HeaderText="M3U1-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top2" HeaderText="M3U1-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top2y" HeaderText="M3U1-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top3" HeaderText="M3U1-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top3y" HeaderText="M3U1-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top4" HeaderText="M3U1-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top4y" HeaderText="M3U1-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top5" HeaderText="M3U1-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top5y" HeaderText="M3U1-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M3U1Top6" HeaderText="M3U1-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top6y" HeaderText="M3U1-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top7" HeaderText="M3U1-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top7y" HeaderText="M3U1-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top8" HeaderText="M3U1-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top8y" HeaderText="M3U1-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top9" HeaderText="M3U1-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top9y" HeaderText="M3U1-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top10" HeaderText="M3U1-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3U1Top10y" HeaderText="M3U1-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M3Input" HeaderText="M3-Input">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3ActualYield" HeaderText="M3-Actual Yield(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Center"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top1" HeaderText="M3-Top1(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top1y" HeaderText="M3-Top1(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top2" HeaderText="M3-Top2(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top2y" HeaderText="M3-Top2(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top3" HeaderText="M3-Top3(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top3y" HeaderText="M3-Top3(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top4" HeaderText="M3-Top4(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top4y" HeaderText="M3-Top4(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top5" HeaderText="M3-Top5(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top5y" HeaderText="M3-Top5(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
										<asp:BoundColumn DataField="M3Top6" HeaderText="M3-Top6(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top6y" HeaderText="M3-Top6(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top7" HeaderText="M3-Top7(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top7y" HeaderText="M3-Top7(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top8" HeaderText="M3-Top8(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top8y" HeaderText="M3-Top8(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top9" HeaderText="M3-Top9(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top9y" HeaderText="M3-Top9(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top10" HeaderText="M3-Top10(Defect)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										<asp:BoundColumn DataField="M3Top10y" HeaderText="M3-Top10(%)">
											<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
											<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
										</asp:BoundColumn>
										
                </Columns>
            </asp:DataGrid>
            <asp:DataGrid ID="DataGrid2" runat="server" CellPadding="4" ForeColor="#333333" GridLines="None"
                Height="1px" Width="221px" AutoGenerateColumns="False" OnSelectedIndexChanged="DataGrid1_SelectedIndexChanged">
                <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <EditItemStyle BackColor="#2461BF" />
                <SelectedItemStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                <AlternatingItemStyle BackColor="White" />
                <ItemStyle BackColor="#EFF3FB" />
                <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                <Columns>
                    <asp:BoundColumn DataField="Group" HeaderText="Group">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ProdID" HeaderText="ProdID">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Input" HeaderText="Input">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="PlanYield" HeaderText="PlanYield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="ActualYield" HeaderText="ActualYield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top1" HeaderText="Top1(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top1y" HeaderText="Top1(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top2" HeaderText="Top2(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top2y" HeaderText="Top2(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top3" HeaderText="Top3(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top3y" HeaderText="Top3(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top4" HeaderText="Top4(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top4y" HeaderText="Top4(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top5" HeaderText="Top5(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Top5y" HeaderText="Top5(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    
					<asp:BoundColumn DataField="Top6" HeaderText="Top6(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top6y" HeaderText="Top6(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top7" HeaderText="Top7(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top7y" HeaderText="Top7(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top8" HeaderText="Top8(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top8y" HeaderText="Top8(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top9" HeaderText="Top9(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top9y" HeaderText="Top9(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top10" HeaderText="Top10(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="Top10y" HeaderText="Top10(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
                    
                    <asp:BoundColumn DataField="M1Input" HeaderText="8040-Input">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1ActualYield" HeaderText="8040-Actual Yield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top1" HeaderText="8040-Top1(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top1y" HeaderText="8040-Top1(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top2" HeaderText="8040-Top2(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top2y" HeaderText="8040-Top2(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top3" HeaderText="8040-Top3(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top3y" HeaderText="8040-Top3(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top4" HeaderText="8040-Top4(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top4y" HeaderText="8040-Top4(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top5" HeaderText="8040-Top5(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M1Top5y" HeaderText="8040-Top5(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    
					<asp:BoundColumn DataField="M1Top6" HeaderText="8040-Top6(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top6y" HeaderText="8040-Top6(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top7" HeaderText="8040-Top7(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top7y" HeaderText="8040-Top7(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top8" HeaderText="8040-Top8(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top8y" HeaderText="8040-Top8(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top9" HeaderText="8040-Top9(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top9y" HeaderText="8040-Top9(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top10" HeaderText="8040-Top10(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M1Top10y" HeaderText="8040-Top10(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
                    
                    <asp:BoundColumn DataField="M2Input" HeaderText="8050-Input">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2ActualYield" HeaderText="8050-Actual Yield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top1" HeaderText="8050-Top1(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top1y" HeaderText="8050-Top1(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top2" HeaderText="8050-Top2(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top2y" HeaderText="8050-Top2(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top3" HeaderText="8050-Top3(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top3y" HeaderText="8050-Top3(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top4" HeaderText="8050-Top4(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top4y" HeaderText="8050-Top4(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top5" HeaderText="8050-Top5(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M2Top5y" HeaderText="8050-Top5(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    
					<asp:BoundColumn DataField="M2Top6" HeaderText="8050-Top6(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top6y" HeaderText="8050-Top6(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top7" HeaderText="8050-Top7(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top7y" HeaderText="8050-Top7(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top8" HeaderText="8050-Top8(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top8y" HeaderText="8050-Top8(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top9" HeaderText="8050-Top9(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top9y" HeaderText="8050-Top9(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top10" HeaderText="8050-Top10(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M2Top10y" HeaderText="8050-Top10(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
                    
                    <asp:BoundColumn DataField="M3Input" HeaderText="8080-Input">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3ActualYield" HeaderText="8080-Actual Yield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top1" HeaderText="8080-Top1(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top1y" HeaderText="8080-Top1(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top2" HeaderText="8080-Top2(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top2y" HeaderText="8080-Top2(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top3" HeaderText="8080-Top3(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top3y" HeaderText="8080-Top3(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top4" HeaderText="8080-Top4(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top4y" HeaderText="8080-Top4(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top5" HeaderText="8080-Top5(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M3Top5y" HeaderText="8080-Top5(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    
					<asp:BoundColumn DataField="M3Top6" HeaderText="8080-Top6(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top6y" HeaderText="8080-Top6(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top7" HeaderText="8080-Top7(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top7y" HeaderText="8080-Top7(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top8" HeaderText="8080-Top8(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top8y" HeaderText="8080-Top8(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top9" HeaderText="8080-Top9(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top9y" HeaderText="8080-Top9(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top10" HeaderText="8080-Top10(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M3Top10y" HeaderText="8080-Top10(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
                    
                    <asp:BoundColumn DataField="M4Input" HeaderText="8090-Input">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4ActualYield" HeaderText="8090-Actual Yield(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Center" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top1" HeaderText="8090-Top1(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top1y" HeaderText="8090-Top1(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top2" HeaderText="8090-Top2(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top2y" HeaderText="8090-Top2(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top3" HeaderText="8090-Top3(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top3y" HeaderText="8090-Top3(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top4" HeaderText="8090-Top4(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top4y" HeaderText="8090-Top4(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top5" HeaderText="8090-Top5(Defect)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="M4Top5y" HeaderText="8090-Top5(%)">
                        <HeaderStyle HorizontalAlign="Center" Wrap="False" />
                        <ItemStyle Font-Names="Arial" Font-Size="X-Small" HorizontalAlign="Left" Wrap="False" />
                    </asp:BoundColumn>
                    
					<asp:BoundColumn DataField="M4Top6" HeaderText="8090-Top6(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top6y" HeaderText="8090-Top6(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top7" HeaderText="8090-Top7(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top7y" HeaderText="8090-Top7(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top8" HeaderText="8090-Top8(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top8y" HeaderText="8090-Top8(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top9" HeaderText="8090-Top9(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top9y" HeaderText="8090-Top9(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top10" HeaderText="8090-Top10(Defect)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
					<asp:BoundColumn DataField="M4Top10y" HeaderText="8090-Top10(%)">
						<HeaderStyle Wrap="False" HorizontalAlign="Center"></HeaderStyle>
						<ItemStyle Font-Size="X-Small" Font-Names="Arial" Wrap="False" HorizontalAlign="Left"></ItemStyle>
					</asp:BoundColumn>
                </Columns>
            </asp:DataGrid></div>
        </div>
    </form>
</body>
</html>
