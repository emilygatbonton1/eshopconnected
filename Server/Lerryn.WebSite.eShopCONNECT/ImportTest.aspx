<%@ Page Language="VB" ValidateRequest="false" Debug="true" AutoEventWireup="false" CodeFile="ImportTest.aspx.vb" Inherits="ImportTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
		<table border="1" width="100%" id="table1" cellspacing="0" cellpadding="3" style="border-collapse: collapse">
			<tr>
				<td style="width: 20%"><b><font face="Arial">Select eShopCONNECT web service function</font></b></td>
				<td width="35%"><font size="3" face="Arial">
                    <input type="text" name="BaseURL" id="BaseURL" size="50" runat="server" />
                    <select name="SelectURL" id="SelectURL" style="font-weight: 700" runat="server" >
                        <option selected="selected" value=""></option>
                        <option value="GenericXMLImport.ashx">GenericXMLImport.ashx</option>
                        <option value="ShopComOrder.ashx">ShopComOrder.ashx</option>
                        <option value="GenericImportStatus.ashx">GenericImportStatus.ashx</option>
                    </select></font></td>
				<td width="29%" rowspan="5">
				<p align="center">
				<img border="0" src="eshopdiagram2.png" alt="eShopCONNECT" width="260" height="503"/></p></td>
			</tr>
			<tr>
				<td colspan="2"><b><font face="Arial">Input XML : --</font></b></td>
			</tr>
			<tr>
				<td colspan="2"><font size="3" face="Arial">
                    <textarea name="XMLToSend" id="XMLToSend" cols="80" rows="10" style="font-weight: 700" runat="server"></textarea></font></td>
			</tr>
			<tr>
				<td colspan="2"><b><font face="Arial">Response XML : --</font></b></td>
			</tr>
			<tr>
				<td colspan="2"><font size="3" face="Arial">
                    <textarea name="ResponseXML" id="ResponseXML" cols="80" rows="10" style="font-weight: 700" runat="server"></textarea></font></td>
			</tr>
		</table>
		<br />
        <input name="SendXML" id="SendXML" type="submit" value="Send XML to eShopCONNECT" runat="server" />
    
    </div>
    </form>
</body>
</html>
