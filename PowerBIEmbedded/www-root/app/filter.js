$(function () {
    // Setting embed configuration
    var embedConfiguration = {
        type: 'report',
        accessToken: appToken,
        id: reportId,
        embedUrl: 'https://embedded.powerbi.com/appTokenReportEmbed',
        settings: {
            filterPaneEnabled: false
        }
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

    // 'Filter Report' clicked
    $('#filter').on('click', function() {
        const basicFilter = { 
            $schema: "http://powerbi.com/product/schema#basic",
            target: {
                table: "Store",
                column: "Territory"
            },
            operator: "In",
            values: ["MD"]
        }

        // Set report filter
        report.setFilters([basicFilter]);
    });

    // 'Clear filter' clicked
    $('#clear').on('click', function() {
        report.removeFilters();
    });

    report.on('filtersApplied', event => {
        // Applied filter event
    });
});