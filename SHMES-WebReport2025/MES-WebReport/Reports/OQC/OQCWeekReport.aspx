<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OQCWeekReport.aspx.cs" Inherits="WebReport.Reports.OQCWeekReport" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>未命名頁面</title>
      <meta name="CODE_LANGUAGE" Content="C#">
	<meta name="vs_defaultClientScript" content="JavaScript">
</head>
<body topmargin = 0 leftmargin = 0 style="width: 100%; height: 100%" background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server"></asp:ScriptManager>
    <div >
        <div>
            <table cellspacing="0" style="padding-right: 0px; padding-left: 0px; padding-bottom: 0px;
                margin: 0px; width: 103%; padding-top: 0px; height: 1064px;">
                <tr>
                    <td style="background-position-x: right; background-attachment: scroll; background-image: url(../Images/left_bg.jpg);
                        vertical-align: top; width: 176px; background-repeat: repeat-y">
                        </td>
                    <td style="background-position-x: left; background-image: url(../Images/right_bg.jpg);
                        width: 12px; background-repeat: repeat-y">
                    </td>
                    <td rowspan="2" style="background-position-x: center; background-image: url(../Images/right_bg.jpg);
                        width: 448px; background-repeat: repeat-y; vertical-align: top;">
                        <div id="filter">
                            <table style="vertical-align: text-bottom; width: 749px; height: 68px; text-align: center">
                                <tr>
                                    <td colspan="11" rowspan="1" style="font-size: 24px; color: black; height: 13px">
                                        OQC周報表</td>
                                </tr>
                                <tr>
                                    <td style="width: 74px; height: 13px" rowspan="2">
                                        群組</td>
                                    <td style="width: 109px; height: 13px; text-align: left" rowspan="2">
                                        <asp:DropDownList ID="DlGroup" runat="server" Width="102px">
                                        </asp:DropDownList></td>
                                    <td colspan="1" rowspan="2" style="width: 41px; height: 13px; text-align: center">
                                        年份</td>
                                    <td colspan="1" rowspan="2" style="width: 69px; height: 13px; text-align: center">
                                        <asp:DropDownList ID="dlYear" runat="server" Width="64px">
                                            <asp:ListItem>2008</asp:ListItem>
                                            <asp:ListItem>2009</asp:ListItem>
                                            <asp:ListItem>2010</asp:ListItem>
                                            <asp:ListItem>2011</asp:ListItem>
                                            <asp:ListItem>2012</asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td colspan="1" rowspan="2" style="width: 41px; height: 13px; text-align: center">
                                        周別</td>
                                    <td colspan="1" rowspan="2" style="width: 65px; height: 13px; text-align: left">
                                        <asp:DropDownList ID="dlWeek" runat="server" Width="65px">
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
                                        </asp:DropDownList></td>
                                    <td style="height: 13px; text-align: left;" colspan="5" rowspan="2">
                                        <asp:Button ID="btnSerch" runat="server" OnClick="btnSerch_Click" Text="查詢" /></td>
                                </tr>
                                <tr>
                                </tr>
                            </table>
                        </div>
                        <div style="width: 749px; height: 343px">
                            <asp:Label ID="lblErrMsg" runat="server" ForeColor="Red"></asp:Label></div>
                        <div style="width: 747px; height: 372px">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="background-position-x: right; background-attachment: scroll; background-image: url(../Images/left_bg.jpg);
                        vertical-align: top; background-repeat: repeat-y; height: 965px; width: 176px;">
                    </td>
                    <td rowspan="1">
                    </td>
                </tr>
            </table>
        </div>
    
    </div>
    </form>
</body>
</html>
