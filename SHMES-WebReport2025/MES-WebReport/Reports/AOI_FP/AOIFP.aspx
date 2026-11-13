<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AOIFP.aspx.cs" Inherits="Report.Reports.AOI_FP.AOIFP" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
    <script type ="text/javascript" src ="NewDate.js"></script>
    
    <script language="javascript" type="text/javascript">
        <script type="text/javascript" >
            function getMulti_Line(textBoxId) {
            var div = document.getElementById("getDIV");
            div.hidden = false;
            var btn = document.getElementById("btn");
            btn.value = textBoxId;
        }

            function btn_close() {
            var btn = document.getElementById("btn");
            var div = document.getElementById("getDIV");
            var textbox = document.getElementById(btn.value);
            var input = document.getElementById("txtInput");

            var myString = input.value;
            var myArray = myString.split(String.fromCharCode(10));
            var retStr = "";
            for (i = 0; i < myArray.length; i++) {
                var str = myArray[i];
            var array = str.split(String.fromCharCode(9));
            for (j = 0; j < array.length; j++) {
                    var strSub = array[j];

            strSub = strSub.replace(String.fromCharCode(13), "");

                    if (strSub.length > 0) {
                retStr += strSub + ",";
                    }
                }
            }
            window.returnValue = retStr;

            textbox.value = retStr;
            div.hidden = true;
        }
    </script>
// <!CDATA[

dateFormat = 'yyyy-mm-dd'

function report_onclick() {

}

// ]]>
 function SignCheck(cbox) {
            var obj = document.getElementsByTagName("input");
            for (var i = 0; i < obj.length; i++) {
                if (obj[i].type == "checkbox") {
                    obj[i].checked = false;
                }
                var sid = cbox.id;
                document.getElementById(sid).checked = true;
            }
        }
</script>
<style type="text/css">
        .style1
        {
            border-right: black 1px solid; 
            border-top: black 1px solid;
            overflow-y: scroll;
            z-index: 104; 
            left: 202px;  
            overflow-x: hidden;
            border-left: black 1px solid;
            width: 150px; 
            border-bottom: black 1px solid; 
            top: 78px; 
            height: 60px
        }
        </style>
</head>
<body background="../../Images/right_bg.jpg">
    <form id="form1" runat="server">
    <div style="height:200px; width:1200px;">
    <table>
    <tr><td align="center" colspan="10"><strong><span style="font-size: 24pt">手動線AOI復判</span></strong></td></tr>
    <tr>
    <td>FROM:</td>
    <td><asp:TextBox ID="txtDate" runat="server"></asp:TextBox></td>
    <td>TO:</td>
    <td><asp:TextBox ID="txtToDate" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>
        product id :</td>
    <td rowspan="2">
        <div class="style1" style="width:300px; background:#FFF">
            <asp:CheckBoxList ID="chkProd" runat="server" DataSourceID="SqlDataSource2" DataTextField="AA" DataValueField="PRODID">
            </asp:CheckBoxList><asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectPRD %>"
                SelectCommand="SELECT (PRODID+'  '+'('+PRODDESC+')') AS [AA],[PRODID] FROM [MPIM] WHERE PRODGROUP NOT IN('BMW','Audi','Panasonic')"></asp:SqlDataSource>
        </div>
    </td>
    <td>
        Line:</td>
    <td rowspan="2">
        <div class="style1" style="width:150px; background:#FFF">
        <asp:CheckBoxList ID="chkline" runat="server" style="font-size: 11pt" AutoPostBack="True" DataSourceID="SqlDataSource1" DataTextField="SUBLINEID" DataValueField="SUBLINEID">
        </asp:CheckBoxList><asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectPRD %>"
            SelectCommand="SELECT [SUBLINEID] FROM [MSLM] WHERE WORKCTR IN('8210','8230') AND (SUBLINEID LIKE '%COG%' OR  SUBLINEID LIKE '%BOD%')">
        </asp:SqlDataSource>
        </div>
    </td>
    <td>站點：</td>
    <td>
        <div class="style1" style="width:80px; background:#FFF">
       <asp:CheckBoxList ID="CheckBoxList1" AutoPostBack="true" runat="server" Width="155px" style="font-size: 11pt ">
                    <asp:ListItem Value="0" Selected="false" Text="8210"></asp:ListItem>
                    <asp:ListItem Value="1" Selected="false" Text="8230"></asp:ListItem>
                </asp:CheckBoxList></div></td>
    <td>
        UserDefine:</td>
    <td rowspan="2">
        <div class="style1" style="width:80px; background:#FFF">
       <asp:CheckBoxList ID="CheckBoxList2" runat="server" Width="155px" style="font-size: 11pt">
                    <asp:ListItem Value="0" Selected="false" Text="OK"></asp:ListItem>
                    <asp:ListItem Value="1" Selected="false" Text="NG"></asp:ListItem>
                </asp:CheckBoxList></div>
    </td>
    <td>
        查詢數量:</td>
        <td rowspan="2">
        <asp:TextBox ID="tbnm" runat="server" Width="30px" onkeyup="this.value=this.value.replace(/[^\d]/g,'') " onafterpaste="this.value=this.value.replace(/[^\d]/g,'') "></asp:TextBox>
        片</td>
    </tr>
    <tr>
    
    </tr>
    <tr>
    <td>PANELID:</td>
    <td><span style="font-size: 11pt">
                    
                    <asp:TextBox ID="tbpld" runat="server" Size="13pt" Width="166px"></asp:TextBox>
                    <input type="button" value="..." onclick="OnClick=getMulti_Line('tbpld');" id="Button1" /></span> </td>
    </tr>
    <tr>
    <td>
        <asp:Button ID="btQuery" runat="server" Text="查詢" onclick="Query_Click" /></td>
        <td><asp:Button ID="btClear" runat="server" Text="重置" onclick="btClear_Click" /></td>
    </tr>
    </table>
    </div>
    <div style="height:150px; width:1200px; overflow:auto;">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" EnableEventValidation = "false" AutoEventWireup="true" DataKeyNames="FRENTITYID"
         OnSelectedIndexChanged="DataGridSum_SelectedIndexChanged" OnRowEditing="GridView1_RowEditing">
        <Columns>
            <asp:TemplateField HeaderText="选择" ItemStyle-Width="50px" >
            <ItemTemplate>
                <asp:CheckBox ID="CheckBox1" onclick="SignCheck(this);" runat="server" />
            </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TRANSDATE" HeaderText="DATE" ReadOnly="True" ItemStyle-Width="100px"/>
            <asp:BoundField DataField="TRANSTIME" HeaderText="TRANSID" ReadOnly="True" ItemStyle-Width="200px" />
            <asp:BoundField DataField="TOPRODID" HeaderText="TOPRODID" ReadOnly="True" ItemStyle-Width="180px" />
            <asp:BoundField DataField="FRENTITYID" HeaderText="FRENTITYID" ReadOnly="True" ItemStyle-Width="150px" />
            <asp:BoundField DataField="CWORKCTR" HeaderText="CWORKCTR" ReadOnly="True" ItemStyle-Width="100px" />
            <asp:BoundField DataField="CSUBLINEID" HeaderText="CSUBLINEID" ReadOnly="True" ItemStyle-Width="150px" />
            <asp:BoundField DataField="TOLOTLEVEL" HeaderText="TOLOTLEVEL" ReadOnly="True" ItemStyle-Width="100px" />
        </Columns>
        <RowStyle Font-Names="Arial" HorizontalAlign="Center" BackColor="#FFFFFF"  />
        <HeaderStyle Font-Names="Arial" HorizontalAlign="Center" BackColor="#507CD1" />
        </asp:GridView>
    </div>
    <div>
        <asp:Button ID="Button2" runat="server" Text="vedio查看" OnClick="Button2_Click" /> </div>
        <div style="height:740px; width:940px; position: absolute; border: black 1px solid; background:#000;">
    <asp:Panel ID="Panel1" runat="server">
        <asp:Label id="lbl_MediaPlayer" style="Z-INDEX: 101; LEFT: 8px; POSITION: absolute; TOP: 8px"
            runat="server" Width="104px"></asp:Label>
    </asp:Panel>
    </div>
    <div style="z-index: 1; left: 1025px; top: 400px; position: absolute; height: 250px; width: 200px; border: black 1px solid;">
    <table>
    <tr>
    <td align="center"><strong><span style="font-size: 16pt;">復判</span></strong></td>
    </tr>
    <tr>
    <td>PANELID</td>
    </tr>
    <tr>
    <td>
        <asp:TextBox ID="tbpanel" runat="server" Enabled="False"></asp:TextBox></td>
    </tr>
    <tr>
    <td>人員工號</td>
    </tr>
    <tr>
    <td>
        <asp:TextBox ID="tbusr" runat="server"></asp:TextBox></td>
    </tr>
    <tr>
    <td>密碼</td>
    </tr>
    <tr>
    <td>
        <asp:TextBox ID="tbpwd" runat="server" TextMode="Password"></asp:TextBox></td>
    </tr>
    <tr>
    <td><asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Text="OK"></asp:ListItem>
        <asp:ListItem Text="NG"></asp:ListItem>
        </asp:RadioButtonList></td>
    </tr>
    <tr>
    <td>
        <asp:Button ID="Button3" runat="server" Text="提交" onclick="Button3_Click" /> </td>
    </tr>
    </table>
    

    </div>
        <div id="getDIV"  hidden="true">
	        <textarea id="txtInput" name="txtInput" rows="13" cols="50" style="WIDTH: 200px; HEIGHT: 300px"></textarea><br/>
            <input  id="btn" onclick="btn_close();" type="button" value="  確定  " style="WIDTH: 58px; HEIGHT: 24px" />
        </div>
    </form>
</body>
</html>
