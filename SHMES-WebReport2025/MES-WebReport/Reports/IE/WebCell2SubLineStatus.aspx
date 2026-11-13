<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebCell2SubLineStatus.aspx.cs" Inherits="ESM.WebCell2SubLineStatus" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Src="~/WebControl/SubLineUnit.ascx" TagName="SubLineUnit" TagPrefix="uc1" %>
<%@ Reference Control="~/WebControl/SubLineUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
<meta http-equiv="refresh" content=30 />
    <title>未命名頁面</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        &nbsp;
                <div id="All" style="border-right: black 1px solid; border-top: black 1px solid;
                    left: 8px; border-left: black 1px solid; width: 1920px; border-bottom: black 1px solid;
                    position: absolute; top: 8px; height: 1648px">
                    <div id="Rough Clean" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 32px; border-left: black 1px solid; width: 272px; border-bottom: black 1px solid;
                        position: absolute; top: 8px; height: 120px">
                        <div id="SubLineID2" style="left: 96px; width: 1px; position: absolute; top: 8px;
                            height: 96px">
                            <asp:Panel ID="PanelRC2" runat="server" Height="88px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitRough2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="SubLineID3" style="left: 184px; width: 72px; position: absolute; top: 8px;
                            height: 104px">
                            <asp:Panel ID="PanelRC3" runat="server" Height="88px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitRough3" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div15" style="left: 56px; width: 144px; position: absolute; top: 102px;
                            height: 16px">
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Rough Clean" Width="120px"></asp:Label></div>
                        <div id="SubLineID1" language="javascript" onclick="return SubLineID1_onclick()"
                            style="left: 16px; width: 1px; position: absolute; top: 8px; height: 1px">
                            <asp:Panel ID="PanelRC1" runat="server" Height="80px" Width="56px">
                                <uc1:SubLineUnit ID="SubLineUnitRough1" runat="server" SubLineID="BCSLR-01" />
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="Final Clean" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 312px; border-left: black 1px solid; width: 264px; border-bottom: black 1px solid;
                        position: absolute; top: 8px; height: 120px">
                        <div id="FinalSubLineID1" style="left: 8px; width: 64px; position: absolute; top: 8px;
                            height: 1px">
                            <asp:Panel ID="PanelFC1" runat="server" Height="96px" Width="56px">
                                <uc1:SubLineUnit ID="SubLineUnitFinal1" runat="server" SubLineID="BCSLF-01" />
                            </asp:Panel>
                        </div>
                        <div id="FinalSubLineID2" style="left: 96px; width: 40px; position: absolute; top: 8px;
                            height: 1px">
                            <asp:Panel ID="PanelFC2" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitFinal2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="FinalSubLineID3" style="left: 184px; width: 64px; position: absolute; top: 8px;
                            height: 1px">
                            <asp:Panel ID="PanelFC3" runat="server" Height="64px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitFinal3" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div1" language="javascript" onclick="return Div1_onclick()" style="left: 88px;
                            width: 112px; position: absolute; top: 102px; height: 16px">
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Final Clean" Width="88px"></asp:Label></div>
                    </div>
                    <div id="Div11" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 24px; border-left: black 1px solid; width: 440px; border-bottom: black 1px solid;
                        position: absolute; top: 144px; height: 894px">
                        <div id="Div9" style="left: 80px; width: 56px; position: absolute; top: 8px; height: 104px">
                            <asp:Panel ID="PanelES2" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div10" style="left: 152px; width: 64px; position: absolute; top: 8px; height: 80px">
                            <asp:Panel ID="PanelES3" runat="server" Height="96px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End3" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div8" style="left: 8px; width: 8px; position: absolute; top: 8px; height: 96px">
                            <asp:Panel ID="PanelES1" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End1" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div12" style="left: 72px; width: 152px; position: absolute; top: 520px;
                            height: 16px">
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="End Seal(Re-Gap)" Width="144px"></asp:Label></div>
                        <div id="Div13" style="left: 224px; width: 56px; position: absolute; top: 8px; height: 104px">
                            <asp:Panel ID="PanelES4" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End4" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div17" style="left: 8px; width: 40px; position: absolute; top: 136px; height: 88px">
                            <asp:Panel ID="PanelES5" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End5" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div18" style="left: 80px; width: 56px; position: absolute; top: 136px; height: 104px">
                            <asp:Panel ID="Panel2" runat="server" Height="88px" Width="40px">
                                <uc1:SubLineUnit ID="SubLineUnit1End6" runat="server" />
                            </asp:Panel>
                            <div id="Div19" style="left: 72px; width: 56px; position: absolute; top: 0px; height: 88px">
                                <asp:Panel ID="Panel3" runat="server" Height="96px" Width="64px">
                                    <uc1:SubLineUnit ID="SubLineUnit1End7" runat="server" />
                                </asp:Panel>
                                <div id="Div20" style="left: 72px; width: 48px; position: absolute; top: 0px; height: 88px">
                                    <asp:Panel ID="Panel4" runat="server" Height="88px" Width="48px">
                                        <uc1:SubLineUnit ID="SubLineUnit1End8" runat="server" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div id="Div21" style="left: 296px; width: 48px; position: absolute; top: 136px;
                            height: 88px">
                            <asp:Panel ID="Panel1" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End9" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div22" style="left: 368px; width: 48px; position: absolute; top: 136px;
                            height: 88px">
                            <asp:Panel ID="Panel5" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End10" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div23" style="left: 8px; width: 40px; position: absolute; top: 264px; height: 88px">
                            <asp:Panel ID="Panel6" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End11" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div24" style="left: 86px; width: 40px; position: absolute; top: 658px; height: 44px">
                            <asp:Panel ID="Panel7" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End12" runat="server" SubLineID="BCSLE-07" />
                            </asp:Panel>
                        </div>
                        <div id="Div25" style="left: 162px; width: 40px; position: absolute; top: 659px;
                            height: 67px">
                            <asp:Panel ID="Panel8" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End13" runat="server" SubLineID="BCSLE-08" />
                            </asp:Panel>
                        </div>
                        <div id="Div26" style="left: 224px; width: 40px; position: absolute; top: 264px;
                            height: 88px">
                            <asp:Panel ID="Panel9" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End14" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div27" style="left: 296px; width: 48px; position: absolute; top: 264px;
                            height: 88px">
                            <asp:Panel ID="Panel10" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End15" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div28" style="left: 368px; width: 48px; position: absolute; top: 264px;
                            height: 88px">
                            <asp:Panel ID="Panel11" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End16" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div29" style="left: 10px; width: 40px; position: absolute; top: 778px; height: 88px">
                            <asp:Panel ID="Panel12" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End17" runat="server" SubLineID="BCSLE-06" />
                            </asp:Panel>
                        </div>
                        <div id="Div30" style="left: 85px; width: 40px; position: absolute; top: 780px; height: 88px">
                            <asp:Panel ID="Panel13" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End18" runat="server" SubLineID="BCSLE-05" />
                            </asp:Panel>
                        </div>
                        <div id="Div31" style="left: 159px; width: 40px; position: absolute; top: 781px;
                            height: 73px">
                            <asp:Panel ID="Panel14" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End19" runat="server" SubLineID="BCSLE-04" />
                            </asp:Panel>
                        </div>
                        <div id="Div32" style="left: 230px; width: 40px; position: absolute; top: 780px;
                            height: 74px">
                            <asp:Panel ID="Panel15" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnit1End20" runat="server" SubLineID="BCSLE-03" />
                            </asp:Panel>
                        </div>
                        <div id="Div33" style="left: 301px; width: 48px; position: absolute; top: 780px;
                            height: 88px">
                            <asp:Panel ID="Panel16" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End21" runat="server" SubLineID="BCSLE-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div34" style="left: 371px; width: 48px; position: absolute; top: 781px;
                            height: 88px">
                            <asp:Panel ID="Panel17" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit1End22" runat="server" SubLineID="BCSLE-01" />
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="Div14" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 584px; border-left: black 1px solid; width: 176px; border-bottom: black 1px solid;
                        position: absolute; top: 8px; height: 120px">
                        <div id="Div2" style="left: 8px; width: 64px; position: absolute; top: 8px; height: 104px">
                            <asp:Panel ID="PanelCC1" runat="server" Height="96px" Width="56px">
                                <uc1:SubLineUnit ID="SubLineUnitCassette1" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div3" style="left: 96px; width: 64px; position: absolute; top: 8px; height: 104px">
                            <asp:Panel ID="PanelCC2" runat="server" Height="88px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitCassette2" runat="server" SubLineID="BCSLC-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div5" style="left: 24px; width: 136px; position: absolute; top: 102px; height: 16px">
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Cassette Clean" Width="128px"></asp:Label></div>
                    </div>
                    <div id="Div16" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 768px; border-left: black 1px solid; width: 176px; border-bottom: black 1px solid;
                        position: absolute; top: 8px; height: 120px">
                        <div id="Div4" style="left: 8px; width: 64px; position: absolute; top: 8px; height: 112px">
                            <asp:Panel ID="PanelLCb1" runat="server" Height="96px" Width="56px">
                                <uc1:SubLineUnit ID="SubLineUnitLCboat1" runat="server" SubLineID="BCSLL-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div6" style="left: 96px; width: 40px; position: absolute; top: 8px; height: 1px">
                            <asp:Panel ID="PanelLCb2" runat="server" Height="88px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitLCboat2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div7" style="left: 24px; width: 136px; position: absolute; top: 100px; height: 16px">
                            <asp:Label ID="Label4" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="LC boat Clean" Width="112px"></asp:Label></div>
                    </div>
                    &nbsp;<br />
                    <br />
                    <br />
                    <div id="Div9990" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 948px; border-left: black 1px solid; width: 181px; border-bottom: black 1px solid;
                        position: absolute; top: 8px; height: 120px">
                        <asp:Label ID="lblDown" runat="server" Text="Down" Width="41px"></asp:Label>
                        <asp:Label ID="lblENG" runat="server" Text="ENG" Width="41px"></asp:Label>
                        <asp:Label ID="lblIDLE" runat="server" Text="IDLE" Width="41px"></asp:Label>
                        <asp:Label ID="lblOther" runat="server" Text="Other" Width="41px"></asp:Label>
                        <asp:Label ID="lblPM" runat="server" Text="PM" Width="41px"></asp:Label>
                        <asp:Label ID="lblRUN" runat="server" Text="RUN" Width="41px"></asp:Label>
                        <asp:Label ID="lblSetup" runat="server" Text="Setup" Width="41px"></asp:Label>
                        <asp:Label ID="lblWait" runat="server" Text="Wait" Width="41px"></asp:Label><br />
                        <asp:Label ID="lblNonOperation" runat="server" Text="Nonoperation" Width="87px"></asp:Label>
                        <asp:Label ID="lblPartial" runat="server" Text="PartialDown " Width="83px"></asp:Label><br />
                        <asp:Label ID="Label6" runat="server" Width="174px" Height="24px"></asp:Label></div>
                    <br />
                    <div id="Div91" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 1024px; border-left: black 1px solid; width: 688px; border-bottom: black 1px solid;
                        position: absolute; top: 144px; height: 224px">
                        <div id="Div92" style="left: 8px; width: 48px; position: absolute; top: 8px; height: 72px">
                            <asp:Panel ID="Panel72" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitAS1" runat="server" Width="400" SubLineID="BCSLA-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div93" style="left: 8px; width: 48px; position: absolute; top: 120px; height: 56px">
                            <asp:Panel ID="Panel73" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitAS2" runat="server" Width="400" />
                            </asp:Panel>
                        </div>
                        <div id="Div94" style="left: 262px; width: 152px; position: absolute; top: 104px;
                            height: 16px">
                            <asp:Label ID="Label9" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Auto S/B" Width="128px"></asp:Label></div>
                    </div>
                    <br />
                    &nbsp;<br />
                    <br />
                    &nbsp;&nbsp;
                    <br />
                    <br />
                    <br />
                    <div id="Divl14" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 480px; border-left: black 1px solid; width: 376px; border-bottom: black 1px solid;
                        position: absolute; top: 144px; height: 896px">
                        <div id="Div35" style="left: 8px; width: 40px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel18" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi1" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div38" style="left: 80px; width: 56px; position: absolute; top: 8px; height: 104px">
                            <asp:Panel ID="Panel19" runat="server" Height="88px" Width="40px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi2" runat="server" />
                            </asp:Panel>
                            <div id="Div37" style="left: 72px; width: 56px; position: absolute; top: 0px; height: 88px">
                                <asp:Panel ID="Panel20" runat="server" Height="96px" Width="64px">
                                    <uc1:SubLineUnit ID="SubLineUnitLCi3" runat="server" />
                                </asp:Panel>
                                <div id="Div36" style="left: 72px; width: 48px; position: absolute; top: 0px; height: 88px">
                                    <asp:Panel ID="Panel21" runat="server" Height="88px" Width="48px">
                                        <uc1:SubLineUnit ID="SubLineUnitLCi4" runat="server" />
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                        <div id="Div40" style="left: 8px; width: 56px; position: absolute; top: 112px; height: 88px">
                            <asp:Panel ID="Panel22" runat="server" Height="96px" Width="64px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi6" runat="server" />
                            </asp:Panel>
                            <div id="Div39" style="left: 72px; width: 48px; position: absolute; top: 0px; height: 88px">
                                <asp:Panel ID="Panel23" runat="server" Height="88px" Width="48px">
                                    <uc1:SubLineUnit ID="SubLineUnitLCi7" runat="server" />
                                </asp:Panel>
                            </div>
                        </div>
                        <div id="Div41" style="left: 152px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel24" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi8" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div42" style="left: 304px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel25" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi5" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div43" style="left: 224px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel26" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi9" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div44" style="left: 304px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel27" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi10" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div45" style="left: 8px; width: 48px; position: absolute; top: 232px; height: 88px">
                            <asp:Panel ID="Panel28" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi11" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div46" style="left: 80px; width: 48px; position: absolute; top: 232px; height: 88px">
                            <asp:Panel ID="Panel29" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi12" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div47" style="left: 152px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel30" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi13" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div48" style="left: 224px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel31" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi14" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div49" style="left: 304px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel32" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi15" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div50" style="left: 8px; width: 48px; position: absolute; top: 336px; height: 88px">
                            <asp:Panel ID="Panel33" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi16" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div51" style="left: 80px; width: 48px; position: absolute; top: 336px; height: 88px">
                            <asp:Panel ID="Panel34" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi17" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div52" style="left: 152px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel35" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi18" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div53" style="left: 224px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel36" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi19" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div54" style="left: 304px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel37" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi20" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div55" style="left: 8px; width: 48px; position: absolute; top: 456px; height: 88px">
                            <asp:Panel ID="Panel38" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi21" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div56" style="left: 80px; width: 48px; position: absolute; top: 456px; height: 88px">
                            <asp:Panel ID="Panel39" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi22" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div57" style="left: 152px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel40" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi23" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div58" style="left: 224px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel41" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi24" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div59" style="left: 304px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel42" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi25" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div60" style="left: 8px; width: 48px; position: absolute; top: 560px; height: 88px">
                            <asp:Panel ID="Panel43" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi26" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div61" style="left: 80px; width: 48px; position: absolute; top: 560px; height: 88px">
                            <asp:Panel ID="Panel44" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi27" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div62" style="left: 152px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel45" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi28" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div63" style="left: 224px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel46" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi29" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div64" style="left: 304px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel47" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi30" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div65" style="left: 8px; width: 48px; position: absolute; top: 680px; height: 88px">
                            <asp:Panel ID="Panel48" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi31" runat="server" SubLineID="BCSLI-06" />
                            </asp:Panel>
                        </div>
                        <div id="Div66" style="left: 80px; width: 48px; position: absolute; top: 680px; height: 88px">
                            <asp:Panel ID="Panel49" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi32" runat="server" SubLineID="BCSLI-07" />
                            </asp:Panel>
                        </div>
                        <div id="Div67" style="left: 152px; width: 48px; position: absolute; top: 680px;
                            height: 88px">
                            <asp:Panel ID="Panel50" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi33" runat="server" SubLineID="BCSLI-08" />
                            </asp:Panel>
                        </div>
                        <div id="Div68" style="left: 224px; width: 48px; position: absolute; top: 680px;
                            height: 88px">
                            <asp:Panel ID="Panel51" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi34" runat="server" SubLineID="BCSLI-09" />
                            </asp:Panel>
                        </div>
                        <div id="Div69" style="left: 304px; width: 48px; position: absolute; top: 680px;
                            height: 88px">
                            <asp:Panel ID="Panel52" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi35" runat="server" SubLineID="BCSLI-10" />
                            </asp:Panel>
                        </div>
                        <div id="Div70" style="left: 8px; width: 48px; position: absolute; top: 784px; height: 88px">
                            <asp:Panel ID="Panel53" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi36" runat="server" SubLineID="BCSLI-05" />
                            </asp:Panel>
                        </div>
                        <div id="Div71" style="left: 80px; width: 48px; position: absolute; top: 784px; height: 88px">
                            <asp:Panel ID="Panel54" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi37" runat="server" SubLineID="BCSLI-04" />
                            </asp:Panel>
                        </div>
                        <div id="Div72" style="left: 152px; width: 48px; position: absolute; top: 784px;
                            height: 88px">
                            <asp:Panel ID="Panel55" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi38" runat="server" SubLineID="BCSLI-03" />
                            </asp:Panel>
                        </div>
                        <div id="Div73" style="left: 224px; width: 48px; position: absolute; top: 784px;
                            height: 88px">
                            <asp:Panel ID="Panel56" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi39" runat="server" SubLineID="BCSLI-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div74" style="left: 304px; width: 48px; position: absolute; top: 784px;
                            height: 88px">
                            <asp:Panel ID="Panel57" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitLCi40" runat="server" SubLineID="BCSLI-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div75" style="left: 133px; width: 152px; position: absolute; top: 440px;
                            height: 16px">
                            <asp:Label ID="Label7" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="LC injection" Width="128px"></asp:Label></div>
                    </div>
                    <div id="Div95" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 856px; border-left: black 1px solid; width: 152px; border-bottom: black 1px solid;
                        position: absolute; top: 144px; height: 896px">
                        <div id="Div79" style="left: 80px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel60" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa1" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div76" style="left: 32px; width: 128px; position: absolute; top: 440px;
                            height: 16px">
                            <asp:Label ID="Label8" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Vacuum anneal" Width="112px"></asp:Label></div>
                        <div id="Div77" style="left: 8px; width: 48px; position: absolute; top: 112px; height: 88px">
                            <asp:Panel ID="Panel58" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div78" style="left: 80px; width: 48px; position: absolute; top: 112px; height: 88px">
                            <asp:Panel ID="Panel59" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa3" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div80" style="left: 8px; width: 48px; position: absolute; top: 232px; height: 88px">
                            <asp:Panel ID="Panel61" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa4" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div81" style="left: 80px; width: 48px; position: absolute; top: 232px; height: 88px">
                            <asp:Panel ID="Panel62" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa5" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div82" style="left: 8px; width: 48px; position: absolute; top: 336px; height: 88px">
                            <asp:Panel ID="Panel63" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa6" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div83" style="left: 80px; width: 48px; position: absolute; top: 336px; height: 88px">
                            <asp:Panel ID="Panel64" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa7" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div84" style="left: 8px; width: 48px; position: absolute; top: 456px; height: 88px">
                            <asp:Panel ID="Panel65" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa8" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div85" style="left: 80px; width: 48px; position: absolute; top: 456px; height: 88px">
                            <asp:Panel ID="Panel66" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa9" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div86" style="left: 8px; width: 48px; position: absolute; top: 560px; height: 88px">
                            <asp:Panel ID="Panel67" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa10" runat="server" SubLineID="BCSLV-05" />
                            </asp:Panel>
                        </div>
                        <div id="Div87" style="left: 84px; width: 48px; position: absolute; top: 681px; height: 88px">
                            <asp:Panel ID="Panel68" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa11" runat="server" SubLineID="BCSLV-03" />
                            </asp:Panel>
                        </div>
                        <div id="Div88" style="left: 8px; width: 48px; position: absolute; top: 784px; height: 88px">
                            <asp:Panel ID="Panel69" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit13" runat="server" SubLineID="BCSLV-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div89" style="left: 80px; width: 48px; position: absolute; top: 784px; height: 88px">
                            <asp:Panel ID="Panel70" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnit14" runat="server" SubLineID="BCSLV-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div90" style="left: 8px; width: 48px; position: absolute; top: 680px; height: 88px">
                            <asp:Panel ID="Panel71" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitVa12" runat="server" SubLineID="BCSLV-04" />
                            </asp:Panel>
                        </div>
                    </div>
                    <div id="Div139" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 1024px; border-left: black 1px solid; width: 688px; border-bottom: black 1px solid;
                        position: absolute; top: 376px; height: 664px">
                        <div id="Div96" style="left: 8px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel74" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS1" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div97" style="left: 80px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel75" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS2" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div98" style="left: 152px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel76" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS3" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div99" style="left: 240px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel77" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS4" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div100" style="left: 312px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel78" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS5" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div101" style="left: 384px; width: 48px; position: absolute; top: 8px; height: 88px">
                            <asp:Panel ID="Panel79" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS6" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div102" style="left: 8px; width: 48px; position: absolute; top: 112px; height: 88px">
                            <asp:Panel ID="Panel80" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS7" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div103" style="left: 80px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel81" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS8" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div104" style="left: 152px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel82" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS9" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div105" style="left: 240px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel83" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS10" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div106" style="left: 312px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel84" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS11" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div107" style="left: 384px; width: 48px; position: absolute; top: 112px;
                            height: 88px">
                            <asp:Panel ID="Panel85" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS12" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div108" style="left: 8px; width: 48px; position: absolute; top: 232px; height: 88px">
                            <asp:Panel ID="Panel86" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS13" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div109" style="left: 80px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel87" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS14" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div110" style="left: 152px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel88" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS15" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div111" style="left: 240px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel89" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS16" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div112" style="left: 312px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel90" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS17" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div113" style="left: 384px; width: 48px; position: absolute; top: 232px;
                            height: 88px">
                            <asp:Panel ID="Panel91" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS18" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div114" style="left: 8px; width: 48px; position: absolute; top: 336px; height: 88px">
                            <asp:Panel ID="Panel92" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS19" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div115" style="left: 80px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel93" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS20" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div116" style="left: 152px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel94" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS21" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div117" style="left: 240px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel95" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS22" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div118" style="left: 312px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel96" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS23" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div119" style="left: 384px; width: 48px; position: absolute; top: 336px;
                            height: 88px">
                            <asp:Panel ID="Panel97" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS24" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div120" style="left: 8px; width: 48px; position: absolute; top: 456px; height: 88px">
                            <asp:Panel ID="Panel98" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS25" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div121" style="left: 80px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel99" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS26" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div122" style="left: 152px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel100" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS27" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div123" style="left: 240px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel101" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS28" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div124" style="left: 312px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel102" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS29" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div125" style="left: 384px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel103" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS30" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div126" style="left: 8px; width: 48px; position: absolute; top: 560px; height: 88px">
                            <asp:Panel ID="Panel104" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS34" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div127" style="left: 80px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel105" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS35" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div128" style="left: 152px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel106" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS36" runat="server" SubLineID="BCSLS-01" />
                            </asp:Panel>
                        </div>
                        <div id="Div129" style="left: 240px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel107" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS37" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div130" style="left: 312px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel108" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS38" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div131" style="left: 384px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel109" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS39" runat="server" SubLineID="BCSLS-02" />
                            </asp:Panel>
                        </div>
                        <div id="Div132" style="left: 472px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel110" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS31" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div133" style="left: 544px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel111" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS32" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div134" style="left: 616px; width: 48px; position: absolute; top: 456px;
                            height: 88px">
                            <asp:Panel ID="Panel112" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS33" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div135" style="left: 472px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel113" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS40" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div136" style="left: 544px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel114" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS41" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div137" style="left: 616px; width: 48px; position: absolute; top: 560px;
                            height: 88px">
                            <asp:Panel ID="Panel115" runat="server" Height="88px" Width="48px">
                                <uc1:SubLineUnit ID="SubLineUnitSS42" runat="server" />
                            </asp:Panel>
                        </div>
                        <div id="Div138" style="left: 456px; width: 152px; position: absolute; top: 320px;
                            height: 16px">
                            <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="Semi S/B" Width="128px"></asp:Label></div>
                    </div>
                </div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <asp:Timer ID="Timer1" runat="server" Interval="30000" OnTick="Timer1_Tick">
                </asp:Timer>
            </ContentTemplate>
        </asp:UpdatePanel>
    
    </div>
    </form>
</body>
</html>
