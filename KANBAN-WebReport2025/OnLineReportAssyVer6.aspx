<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnLineReportAssyVer6.aspx.cs" Inherits="WebReport.OnLineReportAssyVer6" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="Library/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <script src="Library/jquery.min.js" type="text/javascript"></script>
    <script src="Library/popper.min.js" type="text/javascript"></script>
    <script src="Library/bootstrap.min.js" type="text/javascript"></script>
  <style>
  /* Make the image fully responsive */
  .carousel-inner img {
      width: 100%;
      height: 100%;
  }
  </style>
    <title></title>
    <style type="text/css">
        .style1
        {
            width: 1063px;
        }
        .style3
        {
            height: 100px;
        }
        .style4
        {
            height: 238px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" width="100%">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Timer ID="TimerPage" runat="server" 
        ontick="TimerPage_Tick">
    </asp:Timer>
    <table cellpadding="0" cellspacing="0" style="border: 1px solid #000000">
        <tr >
            <td class="style1"> 
                <table border=none width="1080px">
                    <tr >
                        <td align="left"  colspan="2" 
                            style="border: none; background-color: #313A46" class="style4">
                            <table cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td align="left" height="100%">
                                        <asp:Image ID="imgLogo" runat="server" ImageUrl="getLogo.ashx?Customer=''" Width="240px"
                                            Height="230px" />
                                    </td>
                                    <td align="center" style=" width:860px;">
                                        <asp:Label ID="LabelNAME" runat="server" Text="Label" Font-Size="100px" 
                                            ForeColor="White"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div id="demo" class="carousel slide" data-ride="carousel">
 
  <!-- 指示符 -->
<%--  <ul class="carousel-indicators">
    <li data-target="#demo" data-slide-to="0" class="active"></li>
    <li data-target="#demo" data-slide-to="1"></li>
    <li data-target="#demo" data-slide-to="2"></li>
  </ul>--%>
 
  <!-- 轮播图片 -->
  <div class="carousel-inner">
    <div class="carousel-item active">
      <asp:Image ID="ImgSHOW1" runat="server" Width="1080px" Height="1700px" />
    </div>
    <div class="carousel-item">
      <asp:Image ID="ImgSHOW2" runat="server" Width="1080px" Height="1700px" />
    </div>
    <div class="carousel-item">
      <asp:Image ID="ImgSHOW3" runat="server" Width="1080px" Height="1700px" />
    </div>
  </div>
 
<%--  <!-- 左右切换按钮 -->
  <a class="carousel-control-prev" href="#demo" data-slide="prev">
    <span class="carousel-control-prev-icon"></span>
  </a>
  <a class="carousel-control-next" href="#demo" data-slide="next">
    <span class="carousel-control-next-icon"></span>
  </a>--%>
 
</div>
            </td>
        </tr>
    </table>
    
    
   
    </form>
</body>
</html>
