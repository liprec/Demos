$(function () {
    // Setting embed configuration
    var embedConfiguration = {
        type: 'report',
        id: reportIdPBI,
        embedUrl: 'https://app.powerbi.com/reportEmbed'
    };

    // Retrieve report container
    var $staticReportContainer = $('#reportstatic');

    powerbi.accessToken = appTokenPBI;

    // Embed the report
    var report = powerbi.embed($staticReportContainer.get(0), embedConfiguration);

    report.on('loaded', event => {
        // Report loaded event
    });
    
    report.on('error', errors => {
        // Report error event
    });
});