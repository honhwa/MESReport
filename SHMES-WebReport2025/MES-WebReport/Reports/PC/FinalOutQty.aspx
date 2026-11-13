<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FinalOutQty.aspx.cs" Inherits="Report.Reports.PC.FinalOutQty" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
</head>
<body background="../../Images/right_bg.jpg" style="font-size: 12pt">
    <form id="form1" runat="server">
    <div>
        &nbsp;</div>
        <table style="width: 799px; height: 14px">
            <tr>
                <td align="center" colspan="2" style="height: 41px">
                    <strong><span style="font-size: 16pt">每日計劃產出維護主檔</span></strong></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkFab" runat="server" Text="FabID" Width="102px" OnCheckedChanged="ChkFab_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <br />
                    <asp:DropDownList ID="ddlfab" runat="server" Width="170px">
                        <asp:ListItem>SHLCM</asp:ListItem>
                       
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;&nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;</td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkDate" runat="server" Text="MFG Date" Width="102px" OnCheckedChanged="ChkDate_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlDate" runat="server" Width="170px">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                    <asp:Label ID="labUpload" runat="server" Font-Bold="True" Text="上傳資料區:(僅限EXCEL檔)" Width="206px" Visible="False"></asp:Label></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkSub" runat="server" Text="Subline" Width="100px" OnCheckedChanged="ChkSub_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlSub" runat="server" Width="170px">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;<asp:FileUpload ID="FileUpload1" runat="server"
                        Width="202px" Visible="False" /></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 16px;" align="left">
                    <asp:CheckBox ID="ChkPro" runat="server" Text="Prodid" Width="100px" OnCheckedChanged="ChkPro_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 16px;">
                    <asp:DropDownList ID="ddlProdid" runat="server" Width="170px">
                    </asp:DropDownList>
                    &nbsp; <asp:Button ID="ButQuery" runat="server" Height="26px" OnClick="ButQuery_Click" Text="查詢報表"
                        Width="101px" />
                    &nbsp; &nbsp; &nbsp; &nbsp;
                    <asp:Button ID="butUpload" runat="server" Height="26px" OnClick="butUpload_Click" Text="上傳"
                        Width="101px" Visible="False" /></td>
            </tr>
        </table>
        <table style="width: 799px">
            <tr>
                <td style="width: 194px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="792px"></asp:Label></td>
                <td style="width: 67px">
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="3" DataKeyNames="FabID,Shop,Year,Month,WeekNum,MFGDate,ProdLine,ProductID"
             OnRowEditing ="GridView1_RowEditing" OnRowUpdating= "GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit" OnPageIndexChanging="GridView1_PageIndexChanging" BackColor="White" BorderColor="#CCCCCC" BorderStyle="None" BorderWidth="1px">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
                <asp:TemplateField HeaderText="編輯" ShowHeader="False">
                    <EditItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="True" CommandName="Update"
                            Text="更新" />&nbsp;<asp:Button ID="Button2" runat="server" CausesValidation="False"
                                CommandName="Cancel" Text="取消" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Button ID="Button1" runat="server" CausesValidation="False" CommandName="Edit"
                            Text="編輯" />
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Button ID="butAdd" runat="server" Height="25px" Text="新增" Width="40px" OnClick="butAdd_Click" /><asp:Button
                            ID="butYes" runat="server" Height="25px" Text="確定" Width="40px" Visible="False" OnClick="butYes_Click" /><asp:Button ID="butCancel"
                                runat="server" Height="25px" Text="取消" Width="40px" OnClick="butCancel_Click" Visible="False" />
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DataEnd_Time" SortExpression="DataEnd_Time">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxDateEnd_Time" runat="server" Enabled="False" Text='<%# Bind("DataEnd_Time") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label39" runat="server" Text='<%# Bind("DataEnd_Time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="FabID" SortExpression="FabID">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxFabID" runat="server" Text='<%# Bind("FabID") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlFabID" runat="server" Visible="False" Width="145px">
                            <asp:ListItem>SHLCM</asp:ListItem>                          
                        </asp:DropDownList>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# Bind("FabID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Shop" SortExpression="Shop">
                    <EditItemTemplate>
                        &nbsp;<asp:TextBox ID="TextBoxShop" runat="server" Text='<%# Bind("Shop") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# Bind("Shop") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlShop" runat="server" Visible="False" Width="145px">
                            <asp:ListItem>Finish</asp:ListItem>
                            <asp:ListItem>Assembly</asp:ListItem>
                            <asp:ListItem>Module</asp:ListItem>
                        </asp:DropDownList>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Year" SortExpression="Year">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxYear" runat="server" Text='<%# Bind("Year") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtYear" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Year") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Month" SortExpression="Month">
                    <EditItemTemplate>
                        &nbsp;<asp:TextBox ID="TextBoxMonth" runat="server" Text='<%# Bind("Month") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtMonth" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label6" runat="server" Text='<%# Bind("Month") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="WeekNum" SortExpression="WeekNum">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxWeekNum" runat="server" Text='<%# Bind("WeekNum") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtWeekNum" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label7" runat="server" Text='<%# Bind("WeekNum") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="MFGDate" SortExpression="MFGDate">
                    <EditItemTemplate>
                        &nbsp;<asp:TextBox ID="TextBoxMFGDate" runat="server" Text='<%# Bind("MFGDate") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtMFGDate" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label38" runat="server" Text='<%# Bind("MFGDate") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProdLine" SortExpression="ProdLine">
                    <EditItemTemplate>
                        &nbsp;<asp:TextBox ID="TextBoxProdLine" runat="server" Text='<%# Bind("ProdLine") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlProdLine" runat="server" DataSourceID="DataProdLine" DataTextField="sublineid"
                            DataValueField="sublineid" Visible="False" Width="145px">
                        </asp:DropDownList><asp:SqlDataSource ID="DataProdLine" runat="server" ConnectionString="<%$ ConnectionStrings:Connect7 %>"
                            SelectCommand="select DISTINCT sublineid  from mslm"></asp:SqlDataSource>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label8" runat="server" Text='<%# Bind("ProdLine") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="ProductID" SortExpression="ProductID">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBoxProductID" runat="server" Text='<%# Bind("ProductID") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <FooterTemplate>
                        <asp:DropDownList ID="ddlProductID_1" runat="server" DataSourceID="DataProductID"
                            DataTextField="prodid" DataValueField="prodid" Visible="False" Width="145px">
                        </asp:DropDownList><asp:SqlDataSource ID="DataProductID" runat="server" ConnectionString="<%$ ConnectionStrings:Connect7 %>"
                            SelectCommand="select distinct project_name AS prodid from mpim order by project_name"></asp:SqlDataSource>
                    </FooterTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label9" runat="server" Text='<%# Bind("ProductID") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_8" SortExpression="Hour_8">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("Hour_8") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label10" runat="server" Text='<%# Bind("Hour_8") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt8" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_9" SortExpression="Hour_9">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("Hour_9") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label11" runat="server" Text='<%# Bind("Hour_9") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt9" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_10" SortExpression="Hour_10">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("Hour_10") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label12" runat="server" Text='<%# Bind("Hour_10") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt10" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_11" SortExpression="Hour_11">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Hour_11") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label13" runat="server" Text='<%# Bind("Hour_11") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt11" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_12" SortExpression="Hour_12">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("Hour_12") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label14" runat="server" Text='<%# Bind("Hour_12") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt12" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_13" SortExpression="Hour_13">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("Hour_13") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label15" runat="server" Text='<%# Bind("Hour_13") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt13" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_14" SortExpression="Hour_14">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("Hour_14") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label16" runat="server" Text='<%# Bind("Hour_14") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt14" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_15" SortExpression="Hour_15">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("Hour_15") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label17" runat="server" Text='<%# Bind("Hour_15") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt15" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_16" SortExpression="Hour_16">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("Hour_16") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label18" runat="server" Text='<%# Bind("Hour_16") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt16" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_17" SortExpression="Hour_17">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("Hour_17") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label19" runat="server" Text='<%# Bind("Hour_17") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt17" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_18" SortExpression="Hour_18">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("Hour_18") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label20" runat="server" Text='<%# Bind("Hour_18") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt18" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_19" SortExpression="Hour_19">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox13" runat="server" Text='<%# Bind("Hour_19") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label21" runat="server" Text='<%# Bind("Hour_19") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt19" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_20" SortExpression="Hour_20">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox14" runat="server" Text='<%# Bind("Hour_20") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label22" runat="server" Text='<%# Bind("Hour_20") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt20" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_21" SortExpression="Hour_21">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox15" runat="server" Text='<%# Bind("Hour_21") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label23" runat="server" Text='<%# Bind("Hour_21") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt21" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_22" SortExpression="Hour_22">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox16" runat="server" Text='<%# Bind("Hour_22") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label24" runat="server" Text='<%# Bind("Hour_22") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt22" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_23" SortExpression="Hour_23">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox17" runat="server" Text='<%# Bind("Hour_23") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label25" runat="server" Text='<%# Bind("Hour_23") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt23" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_24" SortExpression="Hour_24">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox18" runat="server" Text='<%# Bind("Hour_24") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label26" runat="server" Text='<%# Bind("Hour_24") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt24" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_1" SortExpression="Hour_1">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox19" runat="server" Text='<%# Bind("Hour_1") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label27" runat="server" Text='<%# Bind("Hour_1") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt1" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_2" SortExpression="Hour_2">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox20" runat="server" Text='<%# Bind("Hour_2") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label28" runat="server" Text='<%# Bind("Hour_2") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt2" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_3" SortExpression="Hour_3">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox21" runat="server" Text='<%# Bind("Hour_3") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label29" runat="server" Text='<%# Bind("Hour_3") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt3" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_4" SortExpression="Hour_4">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox22" runat="server" Text='<%# Bind("Hour_4") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label30" runat="server" Text='<%# Bind("Hour_4") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt4" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_5" SortExpression="Hour_5">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox23" runat="server" Text='<%# Bind("Hour_5") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label31" runat="server" Text='<%# Bind("Hour_5") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt5" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_6" SortExpression="Hour_6">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox24" runat="server" Text='<%# Bind("Hour_6") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label32" runat="server" Text='<%# Bind("Hour_6") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt6" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hour_7" SortExpression="Hour_7">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox25" runat="server" Text='<%# Bind("Hour_7") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label33" runat="server" Text='<%# Bind("Hour_7") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txt7" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="TargetYield" SortExpression="TargetYield">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox30" runat="server" Text='<%# Bind("TargetYield") %>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label40" runat="server" Text='<%# Bind("TargetYield") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                    <HeaderStyle Wrap="False" HorizontalAlign="Center" VerticalAlign="Middle" />
                      <FooterTemplate>
                        <asp:TextBox ID="txt_Target" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Create_Time" SortExpression="Create_Time">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox26" runat="server" Text='<%# Bind("Create_Time") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label34" runat="server" Text='<%# Bind("Create_Time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Create_By" SortExpression="Create_By">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox27" runat="server" Text='<%# Bind("Create_By") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label35" runat="server" Text='<%# Bind("Create_By") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtCreate_By" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modify_Time" SortExpression="Modify_Time">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox28" runat="server" Text='<%# Bind("Modify_Time") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label36" runat="server" Text='<%# Bind("Modify_Time") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Modify_by" SortExpression="Modify_by">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox29" runat="server" Text='<%# Bind("Modify_by") %>' Enabled="False"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="False" />
                    <ItemTemplate>
                        <asp:Label ID="Label37" runat="server" Text='<%# Bind("Modify_by") %>'></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:TextBox ID="txtModify_by" runat="server" Visible="False"></asp:TextBox>
                    </FooterTemplate>
                </asp:TemplateField>
            </Columns>
            <RowStyle ForeColor="#000066" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left" />
            <HeaderStyle BackColor="#006699" Font-Bold="True" ForeColor="White" />
        </asp:GridView>
    </form>
</body>
</html>