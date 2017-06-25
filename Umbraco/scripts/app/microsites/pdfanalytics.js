/****
* MODULE            [>] pdfanalytics.js
*                   [ ] add google analytics for any pdf resources
* -----------------------------------------------------------------------------------------------
* NOTES
*
*   expects google analytics and jQuery to be available & configured on the page
*   expects <meta name="application-name" content="microsite" /> to contain the analytics category
*
*   jQuery selectors are case sensitive on the right-hand side of the expression, although not on the left
*   hence the "or" syntax to match the href with typical case versions
*   making the selection case-insensitive is unpleasantly fiddly, so unless there's a strong reason it's best done as below
*
****/

$(function () {
    if (_gaq !== null) {
        $("a[href$='.pdf'], a[href$='.PDF'], a[href$='.Pdf']").click(function () {
            var micrositeName = $('meta[name=application-name]').attr("content") || 'unknown';
            var hostPage = document.URL;
            var documentUrl = $(this).attr("href");
            _gaq.push(['_trackEvent', micrositeName, 'download', hostPage + ';  ' + documentUrl]);
            return true;
        });
    }
})
