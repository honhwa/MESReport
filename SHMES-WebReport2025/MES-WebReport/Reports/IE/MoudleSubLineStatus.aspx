<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MoudleSubLineStatus.aspx.cs" Inherits="ESM.MoudleSubLineStatus" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<%@ Register Src="~/WebControl/ModuleSubLineUnit.ascx" TagName="SubLineUnit" TagPrefix="uc1" %>
<%@ Register Src="~/WebControl/ModuleSubLineUnit.ascx" TagName="ModuleSubLineUnit"
    TagPrefix="uc2" %>
    <%@ Reference Control="~/WebControl/ModuleSubLineUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>未命名頁面</title>
<script language="javascript" type="text/javascript">
<!--

function TABLE1_onclick() {

}

// -->
</script>
</head>
<%--<body style="border-top-width: 1px; border-left-width: 1px; border-left-color: black; border-bottom-width: 1px; border-bottom-color: black; border-top-color: black; border-right-width: 1px; border-right-color: black;">--%>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        &nbsp;&nbsp;&nbsp; &nbsp;
        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table style="vertical-align: top; width: 672px; height: 368px; border-top-width: 1px; border-left-width: 1px; border-left-color: black; border-bottom-width: 1px; border-bottom-color: black; border-top-color: black; border-right-width: 1px; border-right-color: black;" id="TABLE1" language="javascript" onclick="return TABLE1_onclick()">
                    <tr>
                        <td style="border-right: black 1px solid; border-top: black 1px solid; vertical-align: top;
                            border-left: black 1px solid; border-bottom: black 1px solid; height: 12px; width: 128px;">
                            <div id="Div1" style="left: 8px; width: 100px; position: absolute; top: 32px; height: 24px">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit1" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 12px; width: 128px;">
                            <div id="Div2" style="left: 176px; width: 100px; position: absolute; top: 32px; height: 32px">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit2" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 12px" width="128">
                            <div id="Div3" style="left: 344px; width: 100px; position: absolute; top: 32px; height: 32px">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit3" runat="server" />
                            </div>
                            <a href="MoudleSubLineStatus.aspx"></a>
                        </td>
                        <td style="vertical-align: top; height: 12px; width: 140px;">
                            <div id="Div4" style="left: 512px; width: 100px; position: absolute; top: 32px; height: 32px">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit4" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; height: 43px; width: 128px;">
                            <div id="Div5" style="width: 136px; height: 32px; left: 8px; position: absolute; top: 56px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit5" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 43px;">
                            <div id="Div6" style="width: 100px; height: 24px; left: 176px; position: absolute; top: 56px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit6" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 43px" width="128">
                            <div id="Div7" style="width: 128px; height: 24px; left: 344px; position: absolute; top: 56px;" contenteditable="true">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit7" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 43px; width: 140px;">
                            <div id="Div8" style="width: 136px; height: 24px; left: 512px; position: absolute; top: 56px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit8" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 26px;">
                            <div id="Div9" style="width: 128px; height: 16px; left: 8px; position: absolute; top: 80px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit9" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 26px;">
                            <div id="Div10" style="width: 136px; height: 24px; left: 176px; position: absolute; top: 80px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit10" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 26px" width="128">
                            <div id="Div11" style="width: 136px; height: 24px; left: 344px; position: absolute; top: 80px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit11" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 26px; width: 140px;">
                            <div style="width: 128px; height: 24px; left: 512px; position: absolute; top: 80px;" id="Div12">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit12" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 15px;">
                            <div id="Div13" style="width: 128px; height: 24px; left: 8px; position: absolute; top: 104px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit13" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 15px;">
                            <div id="Div14" style="width: 100px; height: 40px; left: 176px; position: absolute; top: 104px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit14" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 15px" width="128">
                            <div id="Div15" style="width: 120px; height: 32px; left: 344px; position: absolute; top: 104px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit15" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 15px; width: 140px;">
                            <div id="Div16" style="width: 100px; height: 24px; left: 512px; position: absolute; top: 104px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit16" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 7px;">
                            <div id="Div17" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 128px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit17" runat="server" />
                            </div><div id="Div29" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 176px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit29" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 7px;">
                            <div id="Div18" style="width: 100px; height: 24px; left: 176px; position: absolute; top: 128px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit18" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 7px" width="128">
                            <div id="Div19" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 128px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit19" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 7px; width: 140px;">
                            <div id="Div20" style="width: 100px; height: 32px; left: 512px; position: absolute; top: 128px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit20" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 10px;">
                            <div id="Div21" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 152px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit21" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 10px;">
                            <div id="Div22" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 152px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit22" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 10px" width="128">
                            <div id="Div23" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 152px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit23" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 10px; width: 140px;">
                            <div id="Div24" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 152px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit24" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 21px;">
                            <div id="Div25" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 224px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit25" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 21px;">
                            <div id="Div26" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 176px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit26" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 21px" width="128">
                            <div id="Div27" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 176px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit27" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 21px; width: 140px;">
                            <div id="Div28" style="width: 100px; height: 24px; left: 512px; position: absolute; top: 176px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit28" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; position: absolute; height: 19px;">
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 19px;">
                            <div id="Div30" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 200px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit30" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 19px" width="128">
                            <div id="Div31" style="width: 100px; height: 24px; left: 344px; position: absolute; top: 200px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit31" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 19px; width: 140px;">
                            <div id="Div32" style="width: 100px; height: 8px; left: 512px; position: absolute; top: 200px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit32" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 4px;">
                            <div id="Div33" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 200px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit33" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 4px;">
                            <div id="Div34" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 224px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit34" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 4px" width="128">
                            <div id="Div35" style="width: 100px; height: 24px; left: 344px; position: absolute; top: 224px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit35" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 4px; width: 140px;">
                            <div id="Div36" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 224px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit36" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px; height: 21px;">
                            <div id="Div37" style="width: 120px; height: 16px; left: 8px; position: absolute; top: 248px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit37" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; height: 21px;">
                            <div id="Div38" style="width: 120px; height: 16px; left: 176px; position: absolute; top: 248px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit38" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 21px" width="128">
                            <div id="Div39" style="width: 120px; height: 16px; left: 344px; position: absolute; top: 248px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit39" runat="server" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 21px; width: 140px;">
                            <div id="Div40" style="width: 120px; height: 8px; left: 512px; position: absolute; top: 248px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit40" runat="server" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; width: 128px;">
                            <div id="Div41" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 272px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit41" runat="server" SubLineID="BMSLP-14" />
                            </div><div id="Div49" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 296px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit45" runat="server" SubLineID="BMSLP-13" />
                            </div><div id="Div53" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 320px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit49" runat="server" SubLineID="BMSLP-12" />
                            </div>
                            <div id="Div57" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 344px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit53" runat="server" SubLineID="BMSLP-11" />
                            </div>
                        </td>
                        <td style="vertical-align: top; width: 128px; position: absolute;">
                        </td>
                        <td style="vertical-align: top; height: 90px" width="128">
                            <div id="Div43" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 272px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit43" runat="server" SubLineID="BMSLA-14A" />
                            </div><div id="Div42" style="width: 120px; height: 16px; left: 176px; position: absolute; top: 272px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit42" runat="server" SubLineID="BMSLB-14" />
                            </div>
                            <div id="Div50" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 296px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit46" runat="server" SubLineID="BMSLB-13" />
                            </div><div id="Div54" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 320px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit50" runat="server" SubLineID="BMSLB-12" />
                            </div>
                            <div id="Div58" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 344px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit54" runat="server" SubLineID="BMSLB-11" />
                            </div>
                        </td>
                        <td style="vertical-align: top; height: 90px; width: 140px;">
                            <div id="Div44" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 272px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit44" runat="server" SubLineID="BMSLA-14B" />
                            </div><div id="Div51" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 296px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit47" runat="server" SubLineID="BMSLA-13A" />
                            </div><div id="Div52" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 296px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit48" runat="server" SubLineID="BMSLA-13B" />
                            </div>
                            <div id="Div55" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 320px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit51" runat="server" SubLineID="BMSLA-12A" />
                            </div>
                            <div id="Div56" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 320px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit52" runat="server" SubLineID="BMSLA-12B" />
                            </div>
                            <div id="Div59" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 344px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit55" runat="server" SubLineID="BMSLA-11A" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top; height: 353px; width: 128px;"><div id="Div45" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 368px;">
                            <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit57" runat="server" SubLineID="BMSLP-10" />
                        </div>
                            <div id="Div61" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 392px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit61" runat="server" SubLineID="BMSLP-09" />
                            </div>
                            <div id="Div65" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 416px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit65" runat="server" SubLineID="BMSLP-08" />
                            </div>
                            <div id="Div69" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 440px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit69" runat="server" SubLineID="BMSLP-07" />
                            </div>
                            <div id="Div73" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 464px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit73" runat="server" SubLineID="BMSLP-06" />
                            </div>
                            <div id="Div74" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 464px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit74" runat="server" SubLineID="BMSLB-06" />
                            </div>
                            <div id="Div75" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 464px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit75" runat="server" SubLineID="BMSLA-06A" />
                            </div>
                            <div id="Div76" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 464px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit76" runat="server" SubLineID="BMSLA-06B" />
                            </div>
                            <div id="Div77" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 488px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit77" runat="server" SubLineID="BMSLP-05" />
                            </div>
                            <div id="Div78" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 488px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit78" runat="server" SubLineID="BMSLB-05" />
                            </div>
                            <div id="Div79" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 488px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit79" runat="server" SubLineID="BMSLA-05A" />
                            </div>
                            <div id="Div80" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 488px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit80" runat="server" SubLineID="BMSLA-05B" />
                            </div>
                            <div id="Div81" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 512px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit81" runat="server" SubLineID="BMSLP-04" />
                            </div>
                            <div id="Div82" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 512px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit82" runat="server" SubLineID="BMSLB-04" />
                            </div>
                            <div id="Div83" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 512px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit83" runat="server" SubLineID="BMSLA-04A" />
                            </div>
                            <div id="Div84" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 512px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit84" runat="server" SubLineID="BMSLA-04B" />
                            </div>
                            <div id="Div85" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 536px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit85" runat="server" SubLineID="BMSLP-03" />
                            </div>
                            <div id="Div86" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 536px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit86" runat="server" SubLineID="BMSLB-03" />
                            </div>
                            <div id="Div87" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 536px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit87" runat="server" SubLineID="BMSLA-03A" />
                            </div>
                            <div id="Div88" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 536px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit88" runat="server" SubLineID="BMSLA-03B" />
                            </div>
                            <div id="Div89" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 560px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit89" runat="server" SubLineID="BMSLP-02" />
                            </div>
                            <div id="Div90" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 560px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit90" runat="server" SubLineID="BMSLB-02" />
                            </div>
                            <div id="Div91" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 560px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit91" runat="server" SubLineID="BMSLA-02A" />
                            </div>
                            <div id="Div92" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 560px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit92" runat="server" SubLineID="BMSLA-02B" />
                            </div>
                            <div id="Div93" style="width: 100px; height: 16px; left: 8px; position: absolute; top: 584px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit93" runat="server" SubLineID="BMSLP-01" />
                            </div>
                            <div id="Div94" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 584px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit94" runat="server" SubLineID="BMSLB-01" />
                            </div>
                            <div id="Div95" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 584px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit95" runat="server" SubLineID="BMSLA-01A" />
                            </div>
                            <div id="Div96" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 584px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit96" runat="server" SubLineID="BMSLA-01B" />
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="Label5" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="M1" Width="144px"></asp:Label></td>
                        <td style="vertical-align: top; height: 353px; width: 128px;"><div id="Div46" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 368px;">
                            <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit58" runat="server" SubLineID="BMSLB-10" />
                        </div>
                            <div id="Div62" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 392px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit62" runat="server" SubLineID="BMSLB-09" />
                            </div>
                            <div id="Div66" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 416px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit66" runat="server" SubLineID="BMSLB-08" />
                            </div>
                            <div id="Div70" style="width: 100px; height: 16px; left: 176px; position: absolute; top: 440px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit70" runat="server" SubLineID="BMSLB-07" />
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="M2" Width="144px"></asp:Label></td>
                        <td style="vertical-align: top; height: 353px" width="128"><div id="Div47" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 368px;">
                            <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit59" runat="server" SubLineID="BMSLA-10A" />
                        </div>
                            <div id="Div63" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 392px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit63" runat="server" SubLineID="BMSLA-09A" />
                            </div>
                            <div id="Div67" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 416px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit67" runat="server" SubLineID="BMSLA-08A" />
                            </div>
                            <div id="Div71" style="width: 100px; height: 16px; left: 344px; position: absolute; top: 440px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit71" runat="server" SubLineID="BMSLA-07A" />
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <asp:Label ID="Label2" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="M3A" Width="144px"></asp:Label><br />
                        </td>
                        <td style="vertical-align: top; height: 353px; width: 140px;"><div id="Div48" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 344px;">
                            <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit56" runat="server" SubLineID="BMSLA-11B" />
                        </div>
                            <div id="Div60" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 368px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit60" runat="server" SubLineID="BMSLA-10B" />
                            </div>
                            <div id="Div64" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 392px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit64" runat="server" SubLineID="BMSLA-09B" />
                            </div>
                            <div id="Div68" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 416px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit68" runat="server" SubLineID="BMSLA-08B" />
                            </div>
                            <div id="Div72" style="width: 100px; height: 16px; left: 512px; position: absolute; top: 440px;">
                                <uc2:ModuleSubLineUnit ID="ModuleSubLineUnit72" runat="server" SubLineID="BMSLA-07B" />
                            </div>
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                            <br />
                    <br />                            
                            <asp:Label ID="Label3" runat="server" Font-Bold="True" Font-Names="新細明體" Height="16px"
                                Text="M3B" Width="144px"></asp:Label></td>
                                        <br />
                    <div id="Div9990" style="border-right: black 1px solid; border-top: black 1px solid;
                        left: 730px; border-left: black 1px solid; width: 181px; border-bottom: black 1px solid;
                        position: absolute; top: 80px; height: 120px">
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
                    </tr>
                </table>
                <asp:Timer ID="Timer1" runat="server" Interval="10000" OnTick="Timer1_Tick">
                </asp:Timer>
                &nbsp;
            </ContentTemplate>
        </asp:UpdatePanel>
        &nbsp;&nbsp;
    </form>
</body>
</html>
