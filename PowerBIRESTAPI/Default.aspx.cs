using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Specialized;
using Newtonsoft.Json;

public partial class Default : System.Web.UI.Page
{
    public AuthenticationResult authResult { get; set; }
    public string clientId = "81...9a";

    public string dashboardId = "52...5e";
    public string tileId = "d7...56";
    public string reportId = "dc...ce";

    public string redirectUrl = "http://localhost:30000/Redirect.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (PowerBITileFrame.Attributes["onload"] == null)
        {
            // Add onload event to IFrames
            PowerBITileFrame.Attributes.Add("onload", "postActionLoadTile()");

            // Define and change IFrame style (height/width)
            TileWidth.Text = "520";
            TileHeight.Text = "360";
            PowerBITileFrame.Attributes.Add("style", String.Format("width: {0}px; height: {1}px;", TileWidth.Text, TileHeight.Text));
        }

        if (PowerBIReportFrame.Attributes["onload"] == null)
        {
            // Add onload event to IFrames
            PowerBIReportFrame.Attributes.Add("onload", "postActionLoadReport()");

            // Define and change IFrame style (height/width)
            ReportWidth.Text = "1000";
            ReportHeight.Text = "600";
            PowerBIReportFrame.Attributes.Add("style", String.Format("width: {0}px; height: {1}px;", ReportWidth.Text, ReportHeight.Text));
        }

        // We are authenticated
        if (Session["authResult"] != null)
        {
            // Hide signin button and show Load buttons
            SignIn.Visible = false;
            Explanation.Visible = true;

            LoadTile.Visible = true;
            LoadReport.Visible = true;

            PowerBITile.Visible = false;
            PowerBIReport.Visible = false;

            // Get authorization token from session
            authResult = (AuthenticationResult)Session["authResult"];
            AccessToken.Text = authResult.AccessToken;
        }
        else
        {
            // Show signin button and hide IFrames
            SignIn.Visible = true;
            Explanation.Visible = false;

            LoadTile.Visible = false;
            LoadReport.Visible = false;

            PowerBITile.Visible = false;
            PowerBIReport.Visible = false;
        }
    }

    protected void SignIn_Click(object sender, EventArgs e)
    {
        //Create a query string
        //Create a sign-in NameValueCollection for query string
        var @params = new NameValueCollection
        {
            //Azure AD will return an authorization code. 
            //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
            {"response_type", "code"},

            //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
            //You get the client id when you register your Azure app.
            {"client_id", clientId},

            //Resource uri to the Power BI resource to be authorized
            {"resource", "https://analysis.windows.net/powerbi/api"},

            //After user authenticates, Azure AD will redirect back to the web app
            {"redirect_uri", redirectUrl}
        };

        //Create sign-in query string
        var queryString = HttpUtility.ParseQueryString(string.Empty);
        queryString.Add(@params);

        //Redirect authority
        //Authority Uri is an Azure resource that takes a client id to get an Access token
        string authorityUri = "https://login.windows.net/common/oauth2/authorize/";
        Response.Redirect(String.Format("{0}?{1}", authorityUri, queryString));
    }

    protected void LoadTile_Click(object sender, EventArgs e)
    {
        LoadTile.Visible = false;
        PowerBITile.Visible = true;

        var baseAddress = new Uri("https://api.powerbi.com/");
        using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        {
            // Add authorization token to request header
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", authResult.AccessToken));

            // Retrieve dashboard tile json
            using (var response = httpClient.GetAsync(String.Format("beta/myorg/dashboards/{0}/tiles/{1}", dashboardId, tileId)))
            {
                EmbedTiles Tiles = JsonConvert.DeserializeObject<EmbedTiles>(response.Result.Content.ReadAsStringAsync().Result);
                // Embed tile url to tile IFrame
                PowerBITileFrame.Attributes.Add("src", Tiles.embedUrl);
            }
        }
    }

    protected void LoadReport_Click(object sender, EventArgs e)
    {
        LoadReport.Visible = false;
        PowerBIReport.Visible = true;

        var baseAddress = new Uri("https://api.powerbi.com/");
        using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        {
            // Add authorization token to request header
            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", String.Format("Bearer {0}", authResult.AccessToken));

            // Retrieve report json
            using (var response = httpClient.GetAsync(String.Format("beta/myorg/reports")))
            {
                // Retrieve list of accecable reports
                EmbedReports Reports = JsonConvert.DeserializeObject<EmbedReports>(response.Result.Content.ReadAsStringAsync().Result);

                // Embed filtered report embedurl to tile IFrame
                PowerBIReportFrame.Attributes.Add("src", Reports.value.Find(r => r.id == reportId).embedUrl);
            }
        }
    }
}

#region EmbedTile / EmbedReport
// Object returned by Power BI
public class EmbedTiles
{
    public string odatacontext { get; set; }
    public string id { get; set; }
    public string title { get; set; }
    public string subTitle { get; set; }
    public string embedUrl { get; set; }
    public string embedData { get; set; }
}

public class EmbedReports
{
    public string odatacontext { get; set; }
    public List<EmbedReport> value { get; set; }
}
public class EmbedReport
{
    public string id { get; set; }
    public string name { get; set; }
    public string webUrl { get; set; }
    public string embedUrl { get; set; }
}
#endregion