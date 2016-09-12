$(function () {
    // Setting embed configuration
    var embedConfiguration = {
        type: 'report',
        accessToken: appToken,
        id: reportId,
        embedUrl: 'https://embedded.powerbi.com/appTokenReportEmbed'
    };

    // Retrieve report container
    var $staticReportContainer = $('#reportstatic');

    // Embed the report
    var report = powerbi.embed($staticReportContainer.get(0), embedConfiguration);

    report.on('loaded', event => {
        // Report loaded event
    });
    
    report.on('error', errors => {
        // Report error event
    });
});