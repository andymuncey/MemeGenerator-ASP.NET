<%@ Page Title="Meme Generator" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MemeGen._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Meme Generator</h1>
        <p class="lead"></p>
    </div>

    <div class="row">
        <div class="col-md-4 form-group">
            <div class="form-group">
                <asp:Label ID="lblTopLine" runat="server" Text="Top line of Meme" AssociatedControlID="txtTopLine"></asp:Label>
                <asp:TextBox ID="txtTopLine" runat="server" required="required" CssClass="form-control" placeholder="Books holiday during resit period"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblBottomLine" runat="server" Text="Bottom Line of Meme" AssociatedControlID="txtBottomLine"></asp:Label>
                <asp:TextBox ID="txtBottomLine" runat="server" required="required" CssClass="form-control" placeholder="Fails exam"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Label ID="lblImageFile" runat="server" Text="Image File (max 4MB)" AssociatedControlID="ImageUploader"></asp:Label>
                <asp:FileUpload ID="ImageUploader" runat="server" required="required" accept="image/*" CssClass="form-control-file" />
                
            </div>
            <div class="form-group">
<asp:Button ID="SubmitButton" CssClass="btn btn-primary" runat="server" Text="Upload" OnClick="SubmitButton_Click" />
            </div>
            <asp:Image ID="MemeImage" runat="server" />
        </div>
    </div>

</asp:Content>
