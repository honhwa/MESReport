<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutQty.aspx.cs" Inherits="Report.Reports.Material.OutQty" %>

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
                <td align="center" colspan="2" style="height: 42px">
                    <strong><span style="font-size: 16pt">每日計劃產出維護主檔</span></strong></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkFab" runat="server" Text="FabID" Width="102px" OnCheckedChanged="ChkFab_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlfab" runat="server" Width="170px">
                        <asp:ListItem>LCM1</asp:ListItem>
                        <asp:ListItem>LCM2</asp:ListItem>
                        <asp:ListItem>VAP</asp:ListItem>
                    </asp:DropDownList></td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkDate" runat="server" Text="MFG Date" Width="102px" OnCheckedChanged="ChkDate_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlDate" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 25px" align="left">
                    <asp:CheckBox ID="ChkSub" runat="server" Text="Subline" Width="100px" OnCheckedChanged="ChkSub_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 25px">
                    <asp:DropDownList ID="ddlSub" runat="server" Width="170px">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td style="width: 14px; height: 20px;" align="left">
                    <asp:CheckBox ID="ChkPro" runat="server" Text="Prodid" Width="100px" OnCheckedChanged="ChkPro_CheckedChanged" AutoPostBack="True" /></td>
                <td style="height: 20px;">
                    <asp:DropDownList ID="ddlProdid" runat="server" Width="170px">
                    </asp:DropDownList>
                    &nbsp; &nbsp; &nbsp; &nbsp; 
                    <asp:Button ID="ButQuery" runat="server" Height="26px" OnClick="ButQuery_Click" Text="查詢報表"
                        Width="101px" />
                    &nbsp; &nbsp; &nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" Text="新增" Height="26px" Width="101px" />
                    &nbsp; &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
                    &nbsp; &nbsp; &nbsp;</td>
            </tr>
        </table>
        <br />
        <table style="width: 799px">
            <tr>
                <td style="width: 100px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red" Width="172px"></asp:Label></td>
                <td style="width: 100px">
                </td>
            </tr>
        </table>
        &nbsp;
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False"
            CellPadding="4" DataKeyNames="FabID,Shop,Year,Month,WeekNum,MFGDate,ProdLine,ProductID"
             OnRowEditing ="GridView1_RowEditing" OnRowUpdating= "GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit"
            DataSourceID="SqlDataSource1" ForeColor="#333333" GridLines="None">
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
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
                </asp:TemplateField>
                <asp:BoundField DataField="DataEnd_Time" HeaderText="DataEnd_Time" SortExpression="DataEnd_Time" />
                <asp:BoundField DataField="FabID" HeaderText="FabID" ReadOnly="True" SortExpression="FabID" />
                <asp:BoundField DataField="Shop" HeaderText="Shop" ReadOnly="True" SortExpression="Shop" />
                <asp:BoundField DataField="Year" HeaderText="Year" ReadOnly="True" SortExpression="Year" />
                <asp:BoundField DataField="Month" HeaderText="Month" ReadOnly="True" SortExpression="Month" />
                <asp:BoundField DataField="WeekNum" HeaderText="WeekNum" ReadOnly="True" SortExpression="WeekNum" />
                <asp:BoundField DataField="MFGDate" HeaderText="MFGDate" ReadOnly="True" SortExpression="MFGDate" />
                <asp:BoundField DataField="ProdLine" HeaderText="ProdLine" ReadOnly="True" SortExpression="ProdLine" />
                <asp:BoundField DataField="ProductID" HeaderText="ProductID" ReadOnly="True" SortExpression="ProductID" />
                <asp:BoundField DataField="Hour_8" HeaderText="Hour_8" SortExpression="Hour_8" />
                <asp:BoundField DataField="Hour_9" HeaderText="Hour_9" SortExpression="Hour_9" />
                <asp:BoundField DataField="Hour_10" HeaderText="Hour_10" SortExpression="Hour_10" />
                <asp:BoundField DataField="Hour_11" HeaderText="Hour_11" SortExpression="Hour_11" />
                <asp:BoundField DataField="Hour_12" HeaderText="Hour_12" SortExpression="Hour_12" />
                <asp:BoundField DataField="Hour_13" HeaderText="Hour_13" SortExpression="Hour_13" />
                <asp:BoundField DataField="Hour_14" HeaderText="Hour_14" SortExpression="Hour_14" />
                <asp:BoundField DataField="Hour_15" HeaderText="Hour_15" SortExpression="Hour_15" />
                <asp:BoundField DataField="Hour_16" HeaderText="Hour_16" SortExpression="Hour_16" />
                <asp:BoundField DataField="Hour_17" HeaderText="Hour_17" SortExpression="Hour_17" />
                <asp:BoundField DataField="Hour_18" HeaderText="Hour_18" SortExpression="Hour_18" />
                <asp:BoundField DataField="Hour_19" HeaderText="Hour_19" SortExpression="Hour_19" />
                <asp:BoundField DataField="Hour_20" HeaderText="Hour_20" SortExpression="Hour_20" />
                <asp:BoundField DataField="Hour_21" HeaderText="Hour_21" SortExpression="Hour_21" />
                <asp:BoundField DataField="Hour_22" HeaderText="Hour_22" SortExpression="Hour_22" />
                <asp:BoundField DataField="Hour_23" HeaderText="Hour_23" SortExpression="Hour_23" />
                <asp:BoundField DataField="Hour_24" HeaderText="Hour_24" SortExpression="Hour_24" />
                <asp:BoundField DataField="Hour_1" HeaderText="Hour_1" SortExpression="Hour_1" />
                <asp:BoundField DataField="Hour_2" HeaderText="Hour_2" SortExpression="Hour_2" />
                <asp:BoundField DataField="Hour_3" HeaderText="Hour_3" SortExpression="Hour_3" />
                <asp:BoundField DataField="Hour_4" HeaderText="Hour_4" SortExpression="Hour_4" />
                <asp:BoundField DataField="Hour_5" HeaderText="Hour_5" SortExpression="Hour_5" />
                <asp:BoundField DataField="Hour_6" HeaderText="Hour_6" SortExpression="Hour_6" />
                <asp:BoundField DataField="Hour_7" HeaderText="Hour_7" SortExpression="Hour_7" />
                <asp:BoundField DataField="Create_Time" HeaderText="Create_Time" SortExpression="Create_Time" />
                <asp:BoundField DataField="Create_By" HeaderText="Create_By" SortExpression="Create_By" />
                <asp:BoundField DataField="Modify_Time" HeaderText="Modify_Time" SortExpression="Modify_Time" />
                <asp:BoundField DataField="Modify_by" HeaderText="Modify_by" SortExpression="Modify_by" />
            </Columns>
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <EditRowStyle BackColor="#999999" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:Connect7 %>"
            DeleteCommand="DELETE FROM [ROPPRD] WHERE [FabID] = @FabID AND [Shop] = @Shop AND [Year] = @Year AND [Month] = @Month AND [WeekNum] = @WeekNum AND [MFGDate] = @MFGDate AND [ProdLine] = @ProdLine AND [ProductID] = @ProductID"
            InsertCommand="INSERT INTO [ROPPRD] ([DataEnd_Time], [FabID], [Shop], [Year], [Month], [WeekNum], [MFGDate], [ProdLine], [ProductID], [Hour_8], [Hour_9], [Hour_10], [Hour_11], [Hour_12], [Hour_13], [Hour_14], [Hour_15], [Hour_16], [Hour_17], [Hour_18], [Hour_19], [Hour_20], [Hour_21], [Hour_22], [Hour_23], [Hour_24], [Hour_1], [Hour_2], [Hour_3], [Hour_4], [Hour_5], [Hour_6], [Hour_7], [Create_Time], [Create_By], [Modify_Time], [Modify_by]) VALUES (@DataEnd_Time, @FabID, @Shop, @Year, @Month, @WeekNum, @MFGDate, @ProdLine, @ProductID, @Hour_8, @Hour_9, @Hour_10, @Hour_11, @Hour_12, @Hour_13, @Hour_14, @Hour_15, @Hour_16, @Hour_17, @Hour_18, @Hour_19, @Hour_20, @Hour_21, @Hour_22, @Hour_23, @Hour_24, @Hour_1, @Hour_2, @Hour_3, @Hour_4, @Hour_5, @Hour_6, @Hour_7, @Create_Time, @Create_By, @Modify_Time, @Modify_by)"
            SelectCommand="SELECT * FROM [ROPPRD]" UpdateCommand="UPDATE [ROPPRD] SET [DataEnd_Time] = @DataEnd_Time, [Hour_8] = @Hour_8, [Hour_9] = @Hour_9, [Hour_10] = @Hour_10, [Hour_11] = @Hour_11, [Hour_12] = @Hour_12, [Hour_13] = @Hour_13, [Hour_14] = @Hour_14, [Hour_15] = @Hour_15, [Hour_16] = @Hour_16, [Hour_17] = @Hour_17, [Hour_18] = @Hour_18, [Hour_19] = @Hour_19, [Hour_20] = @Hour_20, [Hour_21] = @Hour_21, [Hour_22] = @Hour_22, [Hour_23] = @Hour_23, [Hour_24] = @Hour_24, [Hour_1] = @Hour_1, [Hour_2] = @Hour_2, [Hour_3] = @Hour_3, [Hour_4] = @Hour_4, [Hour_5] = @Hour_5, [Hour_6] = @Hour_6, [Hour_7] = @Hour_7, [Create_Time] = getdate(), [Create_By] = @Create_By, [Modify_Time] = getdate(), [Modify_by] = @Modify_by WHERE [FabID] = @FabID AND [Shop] = @Shop AND [Year] = @Year AND [Month] = @Month AND [WeekNum] = @WeekNum AND [MFGDate] = @MFGDate AND [ProdLine] = @ProdLine AND [ProductID] = @ProductID">
            <DeleteParameters>
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="Year" Type="Int32" />
                <asp:Parameter Name="Month" Type="Int32" />
                <asp:Parameter Name="WeekNum" Type="Int32" />
                <asp:Parameter Name="MFGDate" Type="DateTime" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="DataEnd_Time" Type="DateTime" />
                <asp:Parameter Name="Hour_8" Type="Int64" />
                <asp:Parameter Name="Hour_9" Type="Int64" />
                <asp:Parameter Name="Hour_10" Type="Int64" />
                <asp:Parameter Name="Hour_11" Type="Int64" />
                <asp:Parameter Name="Hour_12" Type="Int64" />
                <asp:Parameter Name="Hour_13" Type="Int64" />
                <asp:Parameter Name="Hour_14" Type="Int64" />
                <asp:Parameter Name="Hour_15" Type="Int64" />
                <asp:Parameter Name="Hour_16" Type="Int64" />
                <asp:Parameter Name="Hour_17" Type="Int64" />
                <asp:Parameter Name="Hour_18" Type="Int64" />
                <asp:Parameter Name="Hour_19" Type="Int64" />
                <asp:Parameter Name="Hour_20" Type="Int64" />
                <asp:Parameter Name="Hour_21" Type="Int64" />
                <asp:Parameter Name="Hour_22" Type="Int64" />
                <asp:Parameter Name="Hour_23" Type="Int64" />
                <asp:Parameter Name="Hour_24" Type="Int64" />
                <asp:Parameter Name="Hour_1" Type="Int64" />
                <asp:Parameter Name="Hour_2" Type="Int64" />
                <asp:Parameter Name="Hour_3" Type="Int64" />
                <asp:Parameter Name="Hour_4" Type="Int64" />
                <asp:Parameter Name="Hour_5" Type="Int64" />
                <asp:Parameter Name="Hour_6" Type="Int64" />
                <asp:Parameter Name="Hour_7" Type="Int64" />
                <asp:Parameter Name="Create_By" Type="String" />
                <asp:Parameter Name="Modify_by" Type="String" />
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="Year" Type="Int32" />
                <asp:Parameter Name="Month" Type="Int32" />
                <asp:Parameter Name="WeekNum" Type="Int32" />
                <asp:Parameter Name="MFGDate" Type="DateTime" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="DataEnd_Time" Type="DateTime" />
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="Year" Type="Int32" />
                <asp:Parameter Name="Month" Type="Int32" />
                <asp:Parameter Name="WeekNum" Type="Int32" />
                <asp:Parameter Name="MFGDate" Type="DateTime" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
                <asp:Parameter Name="Hour_8" Type="Int64" />
                <asp:Parameter Name="Hour_9" Type="Int64" />
                <asp:Parameter Name="Hour_10" Type="Int64" />
                <asp:Parameter Name="Hour_11" Type="Int64" />
                <asp:Parameter Name="Hour_12" Type="Int64" />
                <asp:Parameter Name="Hour_13" Type="Int64" />
                <asp:Parameter Name="Hour_14" Type="Int64" />
                <asp:Parameter Name="Hour_15" Type="Int64" />
                <asp:Parameter Name="Hour_16" Type="Int64" />
                <asp:Parameter Name="Hour_17" Type="Int64" />
                <asp:Parameter Name="Hour_18" Type="Int64" />
                <asp:Parameter Name="Hour_19" Type="Int64" />
                <asp:Parameter Name="Hour_20" Type="Int64" />
                <asp:Parameter Name="Hour_21" Type="Int64" />
                <asp:Parameter Name="Hour_22" Type="Int64" />
                <asp:Parameter Name="Hour_23" Type="Int64" />
                <asp:Parameter Name="Hour_24" Type="Int64" />
                <asp:Parameter Name="Hour_1" Type="Int64" />
                <asp:Parameter Name="Hour_2" Type="Int64" />
                <asp:Parameter Name="Hour_3" Type="Int64" />
                <asp:Parameter Name="Hour_4" Type="Int64" />
                <asp:Parameter Name="Hour_5" Type="Int64" />
                <asp:Parameter Name="Hour_6" Type="Int64" />
                <asp:Parameter Name="Hour_7" Type="Int64" />
                <asp:Parameter Name="Create_Time" Type="DateTime" />
                <asp:Parameter Name="Create_By" Type="String" />
                <asp:Parameter Name="Modify_Time" Type="DateTime" />
                <asp:Parameter Name="Modify_by" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
        <asp:FormView ID="FormView1" runat="server" DataKeyNames="FabID,Shop,Year,Month,WeekNum,MFGDate,ProdLine,ProductID"
            DataSourceID="SqlDataSource2">
            <EditItemTemplate>
                DataEnd_Time:
                <asp:TextBox ID="DataEnd_TimeTextBox" runat="server" Text='<%# Bind("DataEnd_Time") %>'>
                </asp:TextBox><br />
                FabID:
                <asp:Label ID="FabIDLabel1" runat="server" Text='<%# Eval("FabID") %>'></asp:Label><br />
                Shop:
                <asp:Label ID="ShopLabel1" runat="server" Text='<%# Eval("Shop") %>'></asp:Label><br />
                Year:
                <asp:Label ID="YearLabel1" runat="server" Text='<%# Eval("Year") %>'></asp:Label><br />
                Month:
                <asp:Label ID="MonthLabel1" runat="server" Text='<%# Eval("Month") %>'></asp:Label><br />
                WeekNum:
                <asp:Label ID="WeekNumLabel1" runat="server" Text='<%# Eval("WeekNum") %>'></asp:Label><br />
                MFGDate:
                <asp:Label ID="MFGDateLabel1" runat="server" Text='<%# Eval("MFGDate") %>'></asp:Label><br />
                ProdLine:
                <asp:Label ID="ProdLineLabel1" runat="server" Text='<%# Eval("ProdLine") %>'></asp:Label><br />
                ProductID:
                <asp:Label ID="ProductIDLabel1" runat="server" Text='<%# Eval("ProductID") %>'></asp:Label><br />
                Hour_8:
                <asp:TextBox ID="Hour_8TextBox" runat="server" Text='<%# Bind("Hour_8") %>'>
                </asp:TextBox><br />
                Hour_9:
                <asp:TextBox ID="Hour_9TextBox" runat="server" Text='<%# Bind("Hour_9") %>'>
                </asp:TextBox><br />
                Hour_10:
                <asp:TextBox ID="Hour_10TextBox" runat="server" Text='<%# Bind("Hour_10") %>'>
                </asp:TextBox><br />
                Hour_11:
                <asp:TextBox ID="Hour_11TextBox" runat="server" Text='<%# Bind("Hour_11") %>'>
                </asp:TextBox><br />
                Hour_12:
                <asp:TextBox ID="Hour_12TextBox" runat="server" Text='<%# Bind("Hour_12") %>'>
                </asp:TextBox><br />
                Hour_13:
                <asp:TextBox ID="Hour_13TextBox" runat="server" Text='<%# Bind("Hour_13") %>'>
                </asp:TextBox><br />
                Hour_14:
                <asp:TextBox ID="Hour_14TextBox" runat="server" Text='<%# Bind("Hour_14") %>'>
                </asp:TextBox><br />
                Hour_15:
                <asp:TextBox ID="Hour_15TextBox" runat="server" Text='<%# Bind("Hour_15") %>'>
                </asp:TextBox><br />
                Hour_16:
                <asp:TextBox ID="Hour_16TextBox" runat="server" Text='<%# Bind("Hour_16") %>'>
                </asp:TextBox><br />
                Hour_17:
                <asp:TextBox ID="Hour_17TextBox" runat="server" Text='<%# Bind("Hour_17") %>'>
                </asp:TextBox><br />
                Hour_18:
                <asp:TextBox ID="Hour_18TextBox" runat="server" Text='<%# Bind("Hour_18") %>'>
                </asp:TextBox><br />
                Hour_19:
                <asp:TextBox ID="Hour_19TextBox" runat="server" Text='<%# Bind("Hour_19") %>'>
                </asp:TextBox><br />
                Hour_20:
                <asp:TextBox ID="Hour_20TextBox" runat="server" Text='<%# Bind("Hour_20") %>'>
                </asp:TextBox><br />
                Hour_21:
                <asp:TextBox ID="Hour_21TextBox" runat="server" Text='<%# Bind("Hour_21") %>'>
                </asp:TextBox><br />
                Hour_22:
                <asp:TextBox ID="Hour_22TextBox" runat="server" Text='<%# Bind("Hour_22") %>'>
                </asp:TextBox><br />
                Hour_23:
                <asp:TextBox ID="Hour_23TextBox" runat="server" Text='<%# Bind("Hour_23") %>'>
                </asp:TextBox><br />
                Hour_24:
                <asp:TextBox ID="Hour_24TextBox" runat="server" Text='<%# Bind("Hour_24") %>'>
                </asp:TextBox><br />
                Hour_1:
                <asp:TextBox ID="Hour_1TextBox" runat="server" Text='<%# Bind("Hour_1") %>'>
                </asp:TextBox><br />
                Hour_2:
                <asp:TextBox ID="Hour_2TextBox" runat="server" Text='<%# Bind("Hour_2") %>'>
                </asp:TextBox><br />
                Hour_3:
                <asp:TextBox ID="Hour_3TextBox" runat="server" Text='<%# Bind("Hour_3") %>'>
                </asp:TextBox><br />
                Hour_4:
                <asp:TextBox ID="Hour_4TextBox" runat="server" Text='<%# Bind("Hour_4") %>'>
                </asp:TextBox><br />
                Hour_5:
                <asp:TextBox ID="Hour_5TextBox" runat="server" Text='<%# Bind("Hour_5") %>'>
                </asp:TextBox><br />
                Hour_6:
                <asp:TextBox ID="Hour_6TextBox" runat="server" Text='<%# Bind("Hour_6") %>'>
                </asp:TextBox><br />
                Hour_7:
                <asp:TextBox ID="Hour_7TextBox" runat="server" Text='<%# Bind("Hour_7") %>'>
                </asp:TextBox><br />
                Create_Time:
                <asp:TextBox ID="Create_TimeTextBox" runat="server" Text='<%# Bind("Create_Time") %>'>
                </asp:TextBox><br />
                Create_By:
                <asp:TextBox ID="Create_ByTextBox" runat="server" Text='<%# Bind("Create_By") %>'>
                </asp:TextBox><br />
                Modify_Time:
                <asp:TextBox ID="Modify_TimeTextBox" runat="server" Text='<%# Bind("Modify_Time") %>'>
                </asp:TextBox><br />
                Modify_by:
                <asp:TextBox ID="Modify_byTextBox" runat="server" Text='<%# Bind("Modify_by") %>'>
                </asp:TextBox><br />
                <asp:LinkButton ID="UpdateButton" runat="server" CausesValidation="True" CommandName="Update"
                    Text="更新">
                </asp:LinkButton>
                <asp:LinkButton ID="UpdateCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="取消">
                </asp:LinkButton>
            </EditItemTemplate>
            <InsertItemTemplate>
                FabID: &nbsp; &nbsp; &nbsp; &nbsp;<asp:TextBox ID="FabIDTextBox" runat="server" Text='<%# Bind("FabID") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="FabIDTextBox"
                    ErrorMessage="請輸入廠別！"></asp:RequiredFieldValidator><br />
                Shop: &nbsp; &nbsp; &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="ShopTextBox" runat="server" Text='<%# Bind("Shop") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="ShopTextBox"
                    ErrorMessage="請輸入制程！"></asp:RequiredFieldValidator><br />
                ProdLine: &nbsp; &nbsp;<asp:TextBox ID="ProdLineTextBox" runat="server" Text='<%# Bind("ProdLine") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ProdLineTextBox"
                    ErrorMessage="請輸入線別！"></asp:RequiredFieldValidator><br />
                ProductID: &nbsp;<asp:TextBox ID="ProductIDTextBox" runat="server" Text='<%# Bind("ProductID") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="ProductIDTextBox"
                    ErrorMessage="請輸入產品號碼！"></asp:RequiredFieldValidator><br />
                Hour_8: &nbsp; &nbsp; &nbsp;<asp:TextBox ID="Hour_8TextBox" runat="server" Text='<%# Bind("Hour_8") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="Hour_8TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_9: &nbsp; &nbsp; &nbsp;<asp:TextBox ID="Hour_9TextBox" runat="server" Text='<%# Bind("Hour_9") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="Hour_9TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_10: &nbsp; &nbsp;<asp:TextBox ID="Hour_10TextBox" runat="server" Text='<%# Bind("Hour_10") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="Hour_10TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_11: &nbsp; &nbsp;<asp:TextBox ID="Hour_11TextBox" runat="server" Text='<%# Bind("Hour_11") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="Hour_11TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_12: &nbsp; &nbsp;<asp:TextBox ID="Hour_12TextBox" runat="server" Text='<%# Bind("Hour_12") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="Hour_12TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_13:&nbsp; &nbsp;
                <asp:TextBox ID="Hour_13TextBox" runat="server" Text='<%# Bind("Hour_13") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="Hour_13TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_14:&nbsp; &nbsp;
                <asp:TextBox ID="Hour_14TextBox" runat="server" Text='<%# Bind("Hour_14") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="Hour_14TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_15: &nbsp; &nbsp;<asp:TextBox ID="Hour_15TextBox" runat="server" Text='<%# Bind("Hour_15") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="Hour_15TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_16: &nbsp; &nbsp;<asp:TextBox ID="Hour_16TextBox" runat="server" Text='<%# Bind("Hour_16") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="Hour_16TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_17: &nbsp; &nbsp;<asp:TextBox ID="Hour_17TextBox" runat="server" Text='<%# Bind("Hour_17") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator14" runat="server" ControlToValidate="Hour_17TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_18: &nbsp; &nbsp;<asp:TextBox ID="Hour_18TextBox" runat="server" Text='<%# Bind("Hour_18") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator15" runat="server" ControlToValidate="Hour_18TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_19:&nbsp; &nbsp;
                <asp:TextBox ID="Hour_19TextBox" runat="server" Text='<%# Bind("Hour_19") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator16" runat="server" ControlToValidate="Hour_19TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_20:&nbsp; &nbsp;
                <asp:TextBox ID="Hour_20TextBox" runat="server" Text='<%# Bind("Hour_20") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator17" runat="server" ControlToValidate="Hour_20TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_21:&nbsp; &nbsp;
                <asp:TextBox ID="Hour_21TextBox" runat="server" Text='<%# Bind("Hour_21") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator18" runat="server" ControlToValidate="Hour_21TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_22: &nbsp; &nbsp;<asp:TextBox ID="Hour_22TextBox" runat="server" Text='<%# Bind("Hour_22") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator19" runat="server" ControlToValidate="Hour_22TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_23: &nbsp; &nbsp;<asp:TextBox ID="Hour_23TextBox" runat="server" Text='<%# Bind("Hour_23") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator20" runat="server" ControlToValidate="Hour_23TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_24: &nbsp; &nbsp;<asp:TextBox ID="Hour_24TextBox" runat="server" Text='<%# Bind("Hour_24") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator21" runat="server" ControlToValidate="Hour_24TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_1: &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="Hour_1TextBox" runat="server" Text='<%# Bind("Hour_1") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator22" runat="server" ControlToValidate="Hour_1TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_2: &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="Hour_2TextBox" runat="server" Text='<%# Bind("Hour_2") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator23" runat="server" ControlToValidate="Hour_2TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_3: &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="Hour_3TextBox" runat="server" Text='<%# Bind("Hour_3") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator24" runat="server" ControlToValidate="Hour_3TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_4: &nbsp; &nbsp; &nbsp;<asp:TextBox ID="Hour_4TextBox" runat="server" Text='<%# Bind("Hour_4") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator25" runat="server" ControlToValidate="Hour_4TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_5: &nbsp; &nbsp; &nbsp;<asp:TextBox ID="Hour_5TextBox" runat="server" Text='<%# Bind("Hour_5") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator26" runat="server" ControlToValidate="Hour_5TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_6: &nbsp; &nbsp; &nbsp;<asp:TextBox ID="Hour_6TextBox" runat="server" Text='<%# Bind("Hour_6") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator27" runat="server" ControlToValidate="Hour_6TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Hour_7: &nbsp;&nbsp; &nbsp;
                <asp:TextBox ID="Hour_7TextBox" runat="server" Text='<%# Bind("Hour_7") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator28" runat="server" ControlToValidate="Hour_7TextBox"
                    ErrorMessage="請輸入產量！"></asp:RequiredFieldValidator><br />
                Create_By:
                <asp:TextBox ID="Create_ByTextBox" runat="server" Text='<%# Bind("Create_By") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator29" runat="server" ControlToValidate="Create_ByTextBox"
                    ErrorMessage="請輸入您的工號！"></asp:RequiredFieldValidator><br />
                Modify_by:
                <asp:TextBox ID="Modify_byTextBox" runat="server" Text='<%# Bind("Modify_by") %>'></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator30" runat="server" ControlToValidate="Modify_byTextBox"
                    ErrorMessage="請輸入您的姓名！"></asp:RequiredFieldValidator><br />
                <asp:LinkButton ID="InsertButton" runat="server" CausesValidation="True" CommandName="Insert"
                    Text="插入"></asp:LinkButton>
                <asp:LinkButton ID="InsertCancelButton" runat="server" CausesValidation="False" CommandName="Cancel"
                    Text="取消"></asp:LinkButton>
            </InsertItemTemplate>
            <ItemTemplate>
                DataEnd_Time:
                <asp:Label ID="DataEnd_TimeLabel" runat="server" Text='<%# Bind("DataEnd_Time") %>'>
                </asp:Label><br />
                FabID:
                <asp:Label ID="FabIDLabel" runat="server" Text='<%# Eval("FabID") %>'></asp:Label><br />
                Shop:
                <asp:Label ID="ShopLabel" runat="server" Text='<%# Eval("Shop") %>'></asp:Label><br />
                Year:
                <asp:Label ID="YearLabel" runat="server" Text='<%# Eval("Year") %>'></asp:Label><br />
                Month:
                <asp:Label ID="MonthLabel" runat="server" Text='<%# Eval("Month") %>'></asp:Label><br />
                WeekNum:
                <asp:Label ID="WeekNumLabel" runat="server" Text='<%# Eval("WeekNum") %>'></asp:Label><br />
                MFGDate:
                <asp:Label ID="MFGDateLabel" runat="server" Text='<%# Eval("MFGDate") %>'></asp:Label><br />
                ProdLine:
                <asp:Label ID="ProdLineLabel" runat="server" Text='<%# Eval("ProdLine") %>'></asp:Label><br />
                ProductID:
                <asp:Label ID="ProductIDLabel" runat="server" Text='<%# Eval("ProductID") %>'></asp:Label><br />
                Hour_8:
                <asp:Label ID="Hour_8Label" runat="server" Text='<%# Bind("Hour_8") %>'></asp:Label><br />
                Hour_9:
                <asp:Label ID="Hour_9Label" runat="server" Text='<%# Bind("Hour_9") %>'></asp:Label><br />
                Hour_10:
                <asp:Label ID="Hour_10Label" runat="server" Text='<%# Bind("Hour_10") %>'></asp:Label><br />
                Hour_11:
                <asp:Label ID="Hour_11Label" runat="server" Text='<%# Bind("Hour_11") %>'></asp:Label><br />
                Hour_12:
                <asp:Label ID="Hour_12Label" runat="server" Text='<%# Bind("Hour_12") %>'></asp:Label><br />
                Hour_13:
                <asp:Label ID="Hour_13Label" runat="server" Text='<%# Bind("Hour_13") %>'></asp:Label><br />
                Hour_14:
                <asp:Label ID="Hour_14Label" runat="server" Text='<%# Bind("Hour_14") %>'></asp:Label><br />
                Hour_15:
                <asp:Label ID="Hour_15Label" runat="server" Text='<%# Bind("Hour_15") %>'></asp:Label><br />
                Hour_16:
                <asp:Label ID="Hour_16Label" runat="server" Text='<%# Bind("Hour_16") %>'></asp:Label><br />
                Hour_17:
                <asp:Label ID="Hour_17Label" runat="server" Text='<%# Bind("Hour_17") %>'></asp:Label><br />
                Hour_18:
                <asp:Label ID="Hour_18Label" runat="server" Text='<%# Bind("Hour_18") %>'></asp:Label><br />
                Hour_19:
                <asp:Label ID="Hour_19Label" runat="server" Text='<%# Bind("Hour_19") %>'></asp:Label><br />
                Hour_20:
                <asp:Label ID="Hour_20Label" runat="server" Text='<%# Bind("Hour_20") %>'></asp:Label><br />
                Hour_21:
                <asp:Label ID="Hour_21Label" runat="server" Text='<%# Bind("Hour_21") %>'></asp:Label><br />
                Hour_22:
                <asp:Label ID="Hour_22Label" runat="server" Text='<%# Bind("Hour_22") %>'></asp:Label><br />
                Hour_23:
                <asp:Label ID="Hour_23Label" runat="server" Text='<%# Bind("Hour_23") %>'></asp:Label><br />
                Hour_24:
                <asp:Label ID="Hour_24Label" runat="server" Text='<%# Bind("Hour_24") %>'></asp:Label><br />
                Hour_1:
                <asp:Label ID="Hour_1Label" runat="server" Text='<%# Bind("Hour_1") %>'></asp:Label><br />
                Hour_2:
                <asp:Label ID="Hour_2Label" runat="server" Text='<%# Bind("Hour_2") %>'></asp:Label><br />
                Hour_3:
                <asp:Label ID="Hour_3Label" runat="server" Text='<%# Bind("Hour_3") %>'></asp:Label><br />
                Hour_4:
                <asp:Label ID="Hour_4Label" runat="server" Text='<%# Bind("Hour_4") %>'></asp:Label><br />
                Hour_5:
                <asp:Label ID="Hour_5Label" runat="server" Text='<%# Bind("Hour_5") %>'></asp:Label><br />
                Hour_6:
                <asp:Label ID="Hour_6Label" runat="server" Text='<%# Bind("Hour_6") %>'></asp:Label><br />
                Hour_7:
                <asp:Label ID="Hour_7Label" runat="server" Text='<%# Bind("Hour_7") %>'></asp:Label><br />
                Create_Time:
                <asp:Label ID="Create_TimeLabel" runat="server" Text='<%# Bind("Create_Time") %>'></asp:Label><br />
                Create_By:
                <asp:Label ID="Create_ByLabel" runat="server" Text='<%# Bind("Create_By") %>'></asp:Label><br />
                Modify_Time:
                <asp:Label ID="Modify_TimeLabel" runat="server" Text='<%# Bind("Modify_Time") %>'></asp:Label><br />
                Modify_by:
                <asp:Label ID="Modify_byLabel" runat="server" Text='<%# Bind("Modify_by") %>'></asp:Label><br />
                <asp:LinkButton ID="EditButton" runat="server" CausesValidation="False" CommandName="Edit"
                    Text="編輯" OnClick="EditButton_Click">
                </asp:LinkButton>
                <asp:LinkButton ID="DeleteButton" runat="server" CausesValidation="False" CommandName="Delete"
                    Text="刪除" OnClick="DeleteButton_Click">
                </asp:LinkButton>
                <asp:LinkButton ID="NewButton" runat="server" CausesValidation="False" CommandName="New"
                    Text="新增" OnClick="NewButton_Click">
                </asp:LinkButton>
            </ItemTemplate>
        </asp:FormView>
        <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:Connect7 %>"
            DeleteCommand="DELETE FROM [ROPPRD] WHERE [FabID] = @FabID AND [Shop] = @Shop AND [Year] = @Year AND [Month] = @Month AND [WeekNum] = @WeekNum AND [MFGDate] = @MFGDate AND [ProdLine] = @ProdLine AND [ProductID] = @ProductID"
            InsertCommand="INSERT INTO [ROPPRD] ([DataEnd_Time], [FabID], [Shop], [Year], [Month], [WeekNum], [MFGDate], [ProdLine], [ProductID], [Hour_8], [Hour_9], [Hour_10], [Hour_11], [Hour_12], [Hour_13], [Hour_14], [Hour_15], [Hour_16], [Hour_17], [Hour_18], [Hour_19], [Hour_20], [Hour_21], [Hour_22], [Hour_23], [Hour_24], [Hour_1], [Hour_2], [Hour_3], [Hour_4], [Hour_5], [Hour_6], [Hour_7], [Create_Time], [Create_By], [Modify_Time], [Modify_by]) VALUES (getdate(), @FabID, @Shop, year(getdate()), month(getdate()), datepart(ww,getdate()), getdate(), @ProdLine, @ProductID, @Hour_8, @Hour_9, @Hour_10, @Hour_11, @Hour_12, @Hour_13, @Hour_14, @Hour_15, @Hour_16, @Hour_17, @Hour_18, @Hour_19, @Hour_20, @Hour_21, @Hour_22, @Hour_23, @Hour_24, @Hour_1, @Hour_2, @Hour_3, @Hour_4, @Hour_5, @Hour_6, @Hour_7, getdate(), @Create_By, getdate(), @Modify_by)"
            SelectCommand="SELECT * FROM [ROPPRD]" UpdateCommand="UPDATE [ROPPRD] SET [DataEnd_Time] = @DataEnd_Time, [Hour_8] = @Hour_8, [Hour_9] = @Hour_9, [Hour_10] = @Hour_10, [Hour_11] = @Hour_11, [Hour_12] = @Hour_12, [Hour_13] = @Hour_13, [Hour_14] = @Hour_14, [Hour_15] = @Hour_15, [Hour_16] = @Hour_16, [Hour_17] = @Hour_17, [Hour_18] = @Hour_18, [Hour_19] = @Hour_19, [Hour_20] = @Hour_20, [Hour_21] = @Hour_21, [Hour_22] = @Hour_22, [Hour_23] = @Hour_23, [Hour_24] = @Hour_24, [Hour_1] = @Hour_1, [Hour_2] = @Hour_2, [Hour_3] = @Hour_3, [Hour_4] = @Hour_4, [Hour_5] = @Hour_5, [Hour_6] = @Hour_6, [Hour_7] = @Hour_7, [Create_Time] = @Create_Time, [Create_By] = @Create_By, [Modify_Time] = @Modify_Time, [Modify_by] = @Modify_by WHERE [FabID] = @FabID AND [Shop] = @Shop AND [Year] = @Year AND [Month] = @Month AND [WeekNum] = @WeekNum AND [MFGDate] = @MFGDate AND [ProdLine] = @ProdLine AND [ProductID] = @ProductID">
            <DeleteParameters>
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="Year" Type="Int32" />
                <asp:Parameter Name="Month" Type="Int32" />
                <asp:Parameter Name="WeekNum" Type="Int32" />
                <asp:Parameter Name="MFGDate" Type="DateTime" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
            </DeleteParameters>
            <UpdateParameters>
                <asp:Parameter Name="DataEnd_Time" Type="DateTime" />
                <asp:Parameter Name="Hour_8" Type="Int64" />
                <asp:Parameter Name="Hour_9" Type="Int64" />
                <asp:Parameter Name="Hour_10" Type="Int64" />
                <asp:Parameter Name="Hour_11" Type="Int64" />
                <asp:Parameter Name="Hour_12" Type="Int64" />
                <asp:Parameter Name="Hour_13" Type="Int64" />
                <asp:Parameter Name="Hour_14" Type="Int64" />
                <asp:Parameter Name="Hour_15" Type="Int64" />
                <asp:Parameter Name="Hour_16" Type="Int64" />
                <asp:Parameter Name="Hour_17" Type="Int64" />
                <asp:Parameter Name="Hour_18" Type="Int64" />
                <asp:Parameter Name="Hour_19" Type="Int64" />
                <asp:Parameter Name="Hour_20" Type="Int64" />
                <asp:Parameter Name="Hour_21" Type="Int64" />
                <asp:Parameter Name="Hour_22" Type="Int64" />
                <asp:Parameter Name="Hour_23" Type="Int64" />
                <asp:Parameter Name="Hour_24" Type="Int64" />
                <asp:Parameter Name="Hour_1" Type="Int64" />
                <asp:Parameter Name="Hour_2" Type="Int64" />
                <asp:Parameter Name="Hour_3" Type="Int64" />
                <asp:Parameter Name="Hour_4" Type="Int64" />
                <asp:Parameter Name="Hour_5" Type="Int64" />
                <asp:Parameter Name="Hour_6" Type="Int64" />
                <asp:Parameter Name="Hour_7" Type="Int64" />
                <asp:Parameter Name="Create_Time" Type="DateTime" />
                <asp:Parameter Name="Create_By" Type="String" />
                <asp:Parameter Name="Modify_Time" Type="DateTime" />
                <asp:Parameter Name="Modify_by" Type="String" />
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="Year" Type="Int32" />
                <asp:Parameter Name="Month" Type="Int32" />
                <asp:Parameter Name="WeekNum" Type="Int32" />
                <asp:Parameter Name="MFGDate" Type="DateTime" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
            </UpdateParameters>
            <InsertParameters>
                <asp:Parameter Name="FabID" Type="String" />
                <asp:Parameter Name="Shop" Type="String" />
                <asp:Parameter Name="ProdLine" Type="String" />
                <asp:Parameter Name="ProductID" Type="String" />
                <asp:Parameter Name="Hour_8" Type="Int64" />
                <asp:Parameter Name="Hour_9" Type="Int64" />
                <asp:Parameter Name="Hour_10" Type="Int64" />
                <asp:Parameter Name="Hour_11" Type="Int64" />
                <asp:Parameter Name="Hour_12" Type="Int64" />
                <asp:Parameter Name="Hour_13" Type="Int64" />
                <asp:Parameter Name="Hour_14" Type="Int64" />
                <asp:Parameter Name="Hour_15" Type="Int64" />
                <asp:Parameter Name="Hour_16" Type="Int64" />
                <asp:Parameter Name="Hour_17" Type="Int64" />
                <asp:Parameter Name="Hour_18" Type="Int64" />
                <asp:Parameter Name="Hour_19" Type="Int64" />
                <asp:Parameter Name="Hour_20" Type="Int64" />
                <asp:Parameter Name="Hour_21" Type="Int64" />
                <asp:Parameter Name="Hour_22" Type="Int64" />
                <asp:Parameter Name="Hour_23" Type="Int64" />
                <asp:Parameter Name="Hour_24" Type="Int64" />
                <asp:Parameter Name="Hour_1" Type="Int64" />
                <asp:Parameter Name="Hour_2" Type="Int64" />
                <asp:Parameter Name="Hour_3" Type="Int64" />
                <asp:Parameter Name="Hour_4" Type="Int64" />
                <asp:Parameter Name="Hour_5" Type="Int64" />
                <asp:Parameter Name="Hour_6" Type="Int64" />
                <asp:Parameter Name="Hour_7" Type="Int64" />
                <asp:Parameter Name="Create_By" Type="String" />
                <asp:Parameter Name="Modify_by" Type="String" />
            </InsertParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
