using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

public partial class Redirect : System.Web.UI.Page
{
    public string clientId = "81...9a";
    public string clientKey = "Mr..U=";
    public string redirectUrl = "http://localhost:30000/Redirect.aspx";

    protected void Page_Load(object sender, EventArgs e)
    {
        //Redirect uri must match the redirect_uri used when requesting Authorization code.
        string redirectUri = redirectUrl;
        string authorityUri = "https://login.windows.net/common/oauth2/authorize/";

        // Get the auth code
        string code = Request.Params.GetValues(0)[0];

        // Get auth token from auth code       
        AuthenticationContext AC = new AuthenticationContext(authorityUri);

        ClientCredential cc = new ClientCredential(clientId, clientKey);

        AuthenticationResult AR = AC.AcquireTokenByAuthorizationCode(code, new Uri(redirectUri), cc);

        //Set Session "authResult" index string to the AuthenticationResult
        Session["authResult"] = AR;

        //Redirect back to Default.aspx
        Response.Redirect("/Default.aspx");
    }
}