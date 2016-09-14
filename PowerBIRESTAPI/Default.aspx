<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="Default" MasterPageFile="~/MasterPage.master"%>

<asp:Content ID="SigninHeader" ContentPlaceHolderID="SigninHeader" runat="server">
    <div>
        <asp:LinkButton ID="SignIn" runat="server" OnClick="SignIn_Click" Text="Sign in" Class="uk-button"/>
        <asp:Label ClientIDMode="Inherit" ID="Explanation" runat="server" Visible="false" CssClass="uk-article">Already signed in. No action needed</asp:Label>
        <asp:TextBox ClientIDMode="Inherit" ID="AccessToken" style="visibility:hidden;" runat="server"></asp:TextBox>
    </div>
</asp:Content>

<asp:Content ID="PowerBIDashboardTile" ContentPlaceHolderID="PowerBIDashboardTile" runat="server">
    <asp:UpdatePanel ID="PowerBITilePanel" runat="server">
        <ContentTemplate>
            <div style="height:0px;">
                <asp:TextBox ClientIDMode="Inherit" ID="TileHeight" style="visibility:hidden" runat="server"></asp:TextBox>
                <asp:TextBox ClientIDMode="Inherit" ID="TileWidth" style="visibility:hidden" runat="server"></asp:TextBox>
                <asp:TextBox ClientIDMode="Inherit" ID="TileAccessToken" style="visibility:hidden;" runat="server"></asp:TextBox>
                <script>
                    function postActionLoadTile() {
                        // get the access token.
                        accessToken = document.getElementById('SigninHeader_AccessToken').value;
                        h = document.getElementById('PowerBIDashboardTile_TileHeight').value;
                        w = document.getElementById('PowerBIDashboardTile_TileWidth').value;

                        // return if no a
                        if ("" === accessToken)
                            return;

                        // construct the push message structure
                        var m = { action: "loadTile", accessToken: accessToken, height: h, width: w };
                        message = JSON.stringify(m);

                        // push the message.
                        iframe = document.getElementById('PowerBIDashboardTile_PowerBITileFrame');
                        iframe.contentWindow.postMessage(message, "*");

                        // reset refresh spinner
                        document.getElementById('tileRefresh').className = "uk-icon-refresh uk-icon-hover";
                    }

                    function reloadTileFrame()
                    {
                        document.getElementById('tileRefresh').className += " uk-icon-spin";
                        var iframe = document.getElementById('PowerBIDashboardTile_PowerBITileFrame');
                        iframe.src = iframe.src;
                    }
                </script>
            </div>
            <div>
                <asp:Panel ID="PowerBITile" runat="server" Visible="false">
                    <h3>Single tile
                        <a id="tileRefresh" onclick="reloadTileFrame()" class="uk-icon-refresh uk-icon-hover uk-icon-spin"></a>
                    </h3>
                    <iframe id="PowerBITileFrame" runat="server" />
                </asp:Panel>
                <asp:LinkButton ID="LoadTile" runat="server" OnClick="LoadTile_Click" Text="Load" Class="uk-button" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="PowerBISingleReport" ContentPlaceHolderID="PowerBISingleReport" runat="server">
    <asp:UpdatePanel ID="PowerBIReportPanel" runat="server">
        <ContentTemplate>
            <div style="height:0px;">
                <asp:TextBox ClientIDMode="Inherit" ID="ReportHeight" style="visibility:hidden" runat="server"></asp:TextBox>
                <asp:TextBox ClientIDMode="Inherit" ID="ReportWidth" style="visibility:hidden" runat="server"></asp:TextBox>
                <asp:TextBox ClientIDMode="Inherit" ID="ReportAccessToken" style="visibility:hidden;" runat="server"></asp:TextBox>
                <script>
                    function postActionLoadReport() {
                        // get the access token.
                        accessToken = document.getElementById('SigninHeader_AccessToken').value;
                        h = document.getElementById('PowerBISingleReport_ReportHeight').value;
                        w = document.getElementById('PowerBISingleReport_ReportWidth').value;

                        // return if no a
                        if ("" === accessToken)
                            return;

                        // construct the push message structure
                        var m = { action: "loadReport", accessToken: accessToken, height: h, width: w };
                        message = JSON.stringify(m);

                        // push the message.
                        iframe = document.getElementById('PowerBISingleReport_PowerBIReportFrame');
                        iframe.contentWindow.postMessage(message, "*");

                        // reset refresh spinner
                        document.getElementById('reportRefresh').className = "uk-icon-refresh uk-icon-hover";
                    }

                    function reloadReportFrame()
                    {
                        document.getElementById('reportRefresh').className += " uk-icon-spin";
                        var iframe = document.getElementById('PowerBISingleReport_PowerBIReportFrame');
                        iframe.src = iframe.src;
                    }
                </script>
            </div>
            <div>
                <asp:Panel ID="PowerBIReport" runat="server" Visible="false">
                    <h3>
                        Compete report
                        <a id="reportRefresh" onclick="reloadReportFrame()" class="uk-icon-refresh uk-icon-hover uk-icon-spin"></a>
                    </h3>
                    <iframe id="PowerBIReportFrame" runat="server" />
                </asp:Panel>
                <asp:LinkButton ID="LoadReport" runat="server" OnClick="LoadReport_Click" Text="Load" Class="uk-button" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>