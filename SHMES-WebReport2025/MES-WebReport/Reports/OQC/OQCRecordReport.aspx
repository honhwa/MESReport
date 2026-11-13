<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OQCRecordReport.aspx.cs" Inherits="WebReport.Reports.OQCRecordReport" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
  <title>未命名頁面</title>  
<%--      <link href="../Stylesheet.css" rel="Stylesheet" type="text/html" />--%>
    <script type ="text/javascript" src ="popcalendar.js"></script>
<script language="javascript" type="text/javascript">
// <!CDATA[

dateFormat = 'yyyy-mm-dd'

function report_onclick() {

}

// ]]>
</script>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div>
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 100%; padding-top: 0px; vertical-align: text-top; height: 727px; text-align: left;">
                <tr>
               <%--     <td style="background-position-x: right; background-attachment: scroll; background-image: url(../Images/left_bg.jpg);
                        vertical-align: top; width: 3px; background-repeat: repeat-y">
                        </td>--%>
                  <%--  <td style="background-position-x: left; background-image: url(../Images/right_bg.jpg);
                        width: 3px; background-repeat: repeat-y">
                    </td>--%>
                    <td rowspan="2" style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        width: 433px; background-repeat: repeat-y; vertical-align: text-top; height: 775px;">
                            <table style="vertical-align: text-bottom; width: 780px; height: 81px; text-align: center">
                                <tr>
                                    <td style="width: 58px; height: 17px">
                                        開始日期</td>
                                    <td style="width: 199px; height: 17px; text-align: left">
                                        <asp:TextBox ID="txtFromTime" runat="server" Width="143px"></asp:TextBox>
                                        <input id="btnFromTime" onclick="popUpCalendar(this, txtFromTime, dateFormat,-1,-1,false)"
                                            style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
                                            background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" /></td>
                                    <td style="width: 105px; height: 17px">
                                        機種</td>
                                    <td style="width: 60px; height: 17px; text-align: left">
                                        <asp:DropDownList ID="dlProduct" runat="server" Width="143px" AutoPostBack="True">
                                        </asp:DropDownList></td>
                                    <td style="width: 91px; height: 17px">
                                        周別</td>
                                    <td style="width: 145px; height: 17px; text-align: left"><asp:DropDownList ID="dlWeek" runat="server" Width="122px">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem>1</asp:ListItem>
                                        <asp:ListItem>2</asp:ListItem>
                                        <asp:ListItem>3</asp:ListItem>
                                        <asp:ListItem>4</asp:ListItem>
                                        <asp:ListItem>5</asp:ListItem>
                                        <asp:ListItem>6</asp:ListItem>
                                        <asp:ListItem>7</asp:ListItem>
                                        <asp:ListItem>8</asp:ListItem>
                                        <asp:ListItem>9</asp:ListItem>
                                        <asp:ListItem>10</asp:ListItem>
                                        <asp:ListItem>11</asp:ListItem>
                                        <asp:ListItem>12</asp:ListItem>
                                        <asp:ListItem>13</asp:ListItem>
                                        <asp:ListItem>14</asp:ListItem>
                                        <asp:ListItem>15</asp:ListItem>
                                        <asp:ListItem>16</asp:ListItem>
                                        <asp:ListItem>17</asp:ListItem>
                                        <asp:ListItem>18</asp:ListItem>
                                        <asp:ListItem>19</asp:ListItem>
                                        <asp:ListItem>20</asp:ListItem>
                                        <asp:ListItem>21</asp:ListItem>
                                        <asp:ListItem>22</asp:ListItem>
                                        <asp:ListItem>23</asp:ListItem>
                                        <asp:ListItem>24</asp:ListItem>
                                        <asp:ListItem>25</asp:ListItem>
                                        <asp:ListItem>26</asp:ListItem>
                                        <asp:ListItem>27</asp:ListItem>
                                        <asp:ListItem>28</asp:ListItem>
                                        <asp:ListItem>29</asp:ListItem>
                                        <asp:ListItem>30</asp:ListItem>
                                        <asp:ListItem>31</asp:ListItem>
                                        <asp:ListItem>32</asp:ListItem>
                                        <asp:ListItem>33</asp:ListItem>
                                        <asp:ListItem>34</asp:ListItem>
                                        <asp:ListItem>35</asp:ListItem>
                                        <asp:ListItem>36</asp:ListItem>
                                        <asp:ListItem>37</asp:ListItem>
                                        <asp:ListItem>38</asp:ListItem>
                                        <asp:ListItem>39</asp:ListItem>
                                        <asp:ListItem>40</asp:ListItem>
                                        <asp:ListItem>41</asp:ListItem>
                                        <asp:ListItem>42</asp:ListItem>
                                        <asp:ListItem>43</asp:ListItem>
                                        <asp:ListItem>44</asp:ListItem>
                                        <asp:ListItem>45</asp:ListItem>
                                        <asp:ListItem>46</asp:ListItem>
                                        <asp:ListItem>47</asp:ListItem>
                                        <asp:ListItem>48</asp:ListItem>
                                        <asp:ListItem>49</asp:ListItem>
                                        <asp:ListItem>50</asp:ListItem>
                                        <asp:ListItem>51</asp:ListItem>
                                        <asp:ListItem>52</asp:ListItem>
                                        <asp:ListItem>53</asp:ListItem>
                                        <asp:ListItem>54</asp:ListItem>
                                    </asp:DropDownList></td>
                                    <td style="width: 141px; height: 17px; text-align: left">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 58px">
                                        結束日期</td>
                                    <td style="width: 199px; text-align: left">
                                        <asp:TextBox ID="txtToTime" runat="server" Width="143px"></asp:TextBox>
                                        <input id="btnToTime" onclick="popUpCalendar(this, txtToTime, dateFormat,-1,-1,false)"
                                            style="border-right: 0px; padding-right: 0px; border-top: 0px; padding-left: 0px;
                                            background-image: url(../../images/Button.gif); padding-bottom: 0px; border-left: 0px;
                                            width: 25px; padding-top: 0px; border-bottom: 0px; height: 17px" type="button" /></td>
                                    <td style="width: 105px">
                                        &nbsp;機種群組</td>
                                    <td style="width: 60px; text-align: left">
                                        <asp:DropDownList ID="dlGroup" runat="server" AutoPostBack="True" Width="140px">
                                        </asp:DropDownList></td>
                                    <td style="width: 91px">
                                        產綫</td>
                                    <td style="width: 145px; height: 21px; text-align: left">
                                        <asp:DropDownList ID="DLSubLine" runat="server" Width="127px">
                                        </asp:DropDownList></td>
                                    <td style="width: 141px; height: 21px; text-align: left">
                                        <asp:Button ID="btnSerch" runat="server" OnClick="btnSerch_Click" Text="查詢" /></td>
                                </tr>
                            </table>
                        <div id="report" style="width: 99%; height: 621px" onclick="return report_onclick()">
                            &nbsp;<asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label><br />
<%--                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="777px" Height="234px">
                                </rsweb:ReportViewer>--%>
                            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="777px" Height="234px">
                            </rsweb:ReportViewer>
                            <asp:Panel ID="Panel2" runat="server" Height="272px" Width="747px">
                            &nbsp;
                            &nbsp;
<%--                                <rsweb:reportviewer ID="ReportViewer2" runat="server" Width="778px" Height="353px">
                                </rsweb:ReportViewer>--%>

                                <rsweb:ReportViewer ID="ReportViewer2" runat="server" Width="778px" Height="353px">
                                </rsweb:ReportViewer>

                            </asp:Panel>

                        </div>
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
