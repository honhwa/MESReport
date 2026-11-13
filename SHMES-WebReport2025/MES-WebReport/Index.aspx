<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebReport.Index" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="App_Themes/UNICOMTheme/Stylesheet.css" rel="stylesheet" type="text/css" />
</head>
<frameset id="top" rows="59,90%" border="0" framespacing="0" frameborder="0">
	<frame name="banner" src="top.aspx" noResize scrolling="auto">
		<frameset id="oa_frame" border="0" frameSpacing="0" rows="*" frameBorder="0" cols="175,10,*">
			<frame name="contents" marginWidth="0" marginHeight="0" src="TreeMenu.aspx" noResize scrolling="auto">
			<FRAME src="T_L.HTM" frameBorder="NO" noResize scrolling="no">
			<frame name="main" src="Main.aspx" noresize scrolling="auto" frameborder="0">
		</frameset>
	</frameset>
</html>

