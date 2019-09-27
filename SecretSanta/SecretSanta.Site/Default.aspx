<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Secret Santa</title>
    <link href="Styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:PlaceHolder ID="Table1" runat="server" />
        </div>
        <h2>2019</h2>
        <ul>
            <li><a href="~/Default.aspx?list=Current" runat="server">Adults</a></li>
        </ul>
        <h2>2018</h2>
        <ul>
            <li><a href="~/Default.aspx?list=Adults2018" runat="server">Adults</a></li>
            <li><a href="~/Default.aspx?list=Kids2018" runat="server">Kids</a></li>
        </ul>
        <h2>2017</h2>
        <ul>
            <li><a href="~/Default.aspx?list=Adults2017" runat="server">Adults</a></li>
        </ul>
    </form>
</body>
</html>
