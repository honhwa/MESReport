<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ZJStatusQuery.aspx.cs" Inherits="Report.Reports.EMS.ZJStatusQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>ZJ Status Real-time Monitor</title>
    <style type="text/css">.hidden{display:none;}</style>
    <meta content="JavaScript" name="vs_defaultClientScript"/>
    <meta content="C#" name="CODE_LANGUAGE"/>
    <meta http-equiv="refresh" content="30" />
</head>
<body>

    <form id="form1" runat="server">
    <div style="text-align:center">
        <table>
                    <tr>
                <td style="height: 21px; font-weight: bold; font-size: 30pt; color: royalblue; font-family: Arial; position: static; width: 563px;" align="center">
                    ZJ Status Real-time Monitor</td>
                    <td rowspan="2">
        <asp:GridView ID="GridView3" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Names="Arial" Font-Size="X-Small" ForeColor="#333333" GridLines="None" Height="71px" Width="46px" OnRowDataBound="GridView3_RowDataBound">
            <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <Columns>
                <asp:BoundField DataField="Status" HeaderText="Status" />
                <asp:BoundField DataField="COL" HeaderText="Color" />
            </Columns>
            <RowStyle BackColor="#EFF3FB" />
            <EditRowStyle BackColor="#2461BF" />
            <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
            <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
            <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
            <AlternatingRowStyle BackColor="White" />
        </asp:GridView>
    
    </td>
            </tr>
            <tr>
                <td align="center" style="width: 563px">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" Font-Names="Arial" Font-Size="Small" OnRowDataBound="GridView1_RowDataBound" CellPadding="1" ForeColor="#333333" GridLines="None" style="border-top-width: thin; border-left-width: thin; border-left-color: black; border-bottom-width: thin; border-bottom-color: black; border-top-color: black; border-right-width: thin; border-right-color: black" CellSpacing="1">
                        <Columns>
                            <asp:BoundField DataField="Process" HeaderText="Process" />
                            <asp:BoundField DataField="M1" HeaderText="1" />
                            <asp:BoundField DataField="M2" HeaderText="2" />
                            <asp:BoundField DataField="M3" HeaderText="3" />
                            <asp:BoundField DataField="M4" HeaderText="4" />
                            <asp:BoundField DataField="M5" HeaderText="5" />
                            <asp:BoundField DataField="M6" HeaderText="6" />
                            <asp:BoundField DataField="M7" HeaderText="7" />
                            <asp:BoundField DataField="M8" HeaderText="8" />
                            <asp:BoundField DataField="M9" HeaderText="9" />
                            <asp:BoundField DataField="M10" HeaderText="10" />
                            <asp:BoundField DataField="M11" HeaderText="11" />
                            <asp:BoundField DataField="M12" HeaderText="12" />
                            <asp:BoundField DataField="M13" HeaderText="13" />
                            <asp:BoundField DataField="M14" HeaderText="14" />
                            <asp:BoundField DataField="M15" HeaderText="15" />
                            
                            <asp:BoundField DataField="C1" HeaderText="C1" >
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                            <asp:BoundField DataField="C2" HeaderText="C2" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C3" HeaderText="C3" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C4" HeaderText="C4" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C5" HeaderText="C5" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C6" HeaderText="C6" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C7" HeaderText="C7" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C8" HeaderText="C8" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C9" HeaderText="C9" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C10" HeaderText="C10" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C11" HeaderText="C11" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C12" HeaderText="C12" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C13" HeaderText="C13" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C14" HeaderText="C14" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="C15" HeaderText="C15" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            
                            <asp:BoundField DataField="CMT1" HeaderText="CMT1" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT2" HeaderText="CMT2" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT3" HeaderText="CMT3" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT4" HeaderText="CMT4" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT5" HeaderText="CMT5" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT6" HeaderText="CMT6" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT7" HeaderText="CMT7" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT8" HeaderText="CMT8" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT9" HeaderText="CMT9" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT10" HeaderText="CMT10" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT11" HeaderText="CMT11" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT12" HeaderText="CMT12" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT13" HeaderText="CMT13" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT14" HeaderText="CMT14" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>
                            <asp:BoundField DataField="CMT15" HeaderText="CMT15" >                                 <ItemStyle CssClass="hidden" />                                <HeaderStyle CssClass="hidden" />                                <FooterStyle CssClass="hidden" /></asp:BoundField>

                            
                            <asp:BoundField DataField="TDown" HeaderText="Run/Total" />
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td style="height: 21px; width: 563px;" align="center">
                    <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" Font-Names="Arial" Font-Size="Small" OnRowDataBound="GridView2_RowDataBound" CellPadding="1" ForeColor="#333333" GridLines="None">
                        <Columns>
                            <asp:BoundField DataField="ProdID" HeaderText="ProdID" />
                            <asp:BoundField DataField="EQID" HeaderText="EQID" />
                            <asp:BoundField DataField="EQCCDCOLOR" HeaderText="Col"  >
                                <ItemStyle CssClass="hidden" />
                                <HeaderStyle CssClass="hidden" />
                                <FooterStyle CssClass="hidden" />
                            </asp:BoundField>
                        </Columns>
                        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <RowStyle BackColor="#EFF3FB" />
                        <EditRowStyle BackColor="#2461BF" />
                        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
                        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
                        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="White" />
                    </asp:GridView>
                </td>
                <td></td>
            </tr>
        </table>
    
    </div>
    </form>
</body>
</html>
