# Power BI REST API

Simple demo to show the capabilities of Power BI REST API to embed reports / dashboard tiles into a web application

# Power BI
This demo embeds a report and a dashboard tile. In `Default.aspx.cs` add the guids for the

- `dashboardId`: dashboard guid
- `tileId`: tile id of the same dashboard
- `reportId`: guid of a report

Also the app needs to be registered via [https://dev.powerbi.com/apps?type=web](https://dev.powerbi.com/apps?type=web).
Make sure that the application url is set tp `http://localhost:30000/Default.aspx`
and the redirect url to `http://localhost:30000/Redirect.aspx` or else the authorization doesn't work.

After registration there will be a `cliendId` and `clientKey` available that needs to be
added to either `Default.aspx.cs` (only `clientId`) and `Redirect.aspx` and the corresponding variables. 

# How to use the webapplication demos

- **Authorization**: Button to login to Power BI and retrieve an accessToken
- **Single Tile**: Webpage with a Power BI Dashboard Tile embedded. Press *Load* to staart loading the tile. 
- **Complete Report**: Webpage with a Power BI Report embedded. Press *Load* to start loading the report.

## NuGet dependecies
Open the solution in Visual Studio 2005 and install all the missing `nuget` packages.

## Running webserver
Start the website project and it will automaticly start a browser with a link to [http://localhost:30000/Default.aspx](http://localhost:30000/Default.aspx)