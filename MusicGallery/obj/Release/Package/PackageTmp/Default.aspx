<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MusicGallery._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Maurice's Music Gallery</title>
</head>
<body>
    <h1 style="text-align:center">Welcome to Jules Maurice's MP3 Music Gallery</h1>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="sm1" runat="server" />
        <div style="
                    /*background-color: #00294a;*/
                    padding:30px;
                    width:75%;
                    margin-left: 10%;
                    border:3px solid #00294a;
                    border-radius:5px;
                    /*color:white;*/
          ">
            Upload Your MP3 Music Here:
            <asp:FileUpload ID="upload" runat="server" />
            <asp:Button ID="submitButton" runat="server" Text="Submit" OnClick="submitMusic" />
        </div>
        <div style=" 
                    width:80%;
                    padding: 10px;
                    margin-top:20px;
                    margin-left: 10%;
                    border:3px solid #00294a;
                    border-radius:5px;
         ">
            <asp:UpdatePanel ID="up1" runat="server">
                <ContentTemplate>
                    <asp:ListView ID="MusicGalleryDisplayControl" runat="server">
                        <LayoutTemplate>
                            <asp:Image ID="itemPlaceholder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <audio src='<%# Eval("Url") %>' controls="" preload="none"></audio>
                            <asp:Literal ID="label" Text='<%# Eval("Title") %>' runat="server" /><br />                        
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:Timer ID="timer1" runat="server" Interval="15000" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>



