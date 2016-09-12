$(function () {
    // Setting embed configuration
    var embedConfiguration = {
        type: 'report',
        accessToken: appToken,
        id: reportId,
        embedUrl: 'https://embedded.powerbi.com/appTokenReportEmbed',
        settings: {
            navContentPaneEnabled: false
        }
    };

    var currentPage = 0;

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

    // 'Next page' clicked
    $('#next').on('click', function() {
        var pageName;

        // Still a next page waiting
        if (currentPage < 4) {
            currentPage++;

            // Get all the pages of the report
            report.getPages()
                .then(pages => {
                    // Get the next page name
                    pageName = pages[currentPage].name;

                    // Set the next page
                    var page = report.page(pageName);

                    // And make active
                    page.setActive();
                });
        }
    });

    // 'Prev page' clicked
    $('#prev').on('click', function() {
        var pageName;

        // Still a previous page waiting
        if (currentPage > 1) {
            currentPage--;

            // Get all the pages of the report
            report.getPages()
                .then(pages => {
                    // Get the previous page name
                    pageName = pages[currentPage].name;

                    // Set the previous page
                    var page = report.page(pageName);

                    // And make active
                    page.setActive();
                });
        }
    });

    // 'Home page' clicked
    $('#home').on('click', function() {
        // Set page to the 'Overview' page
        var page = report.page("ReportSection3"); 

        // Make active
        page.setActive();
    });

});