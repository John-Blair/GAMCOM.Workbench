/****
* MODULE            [>] analytics.js
*                   [ ] google analytics + pdf events
* -----------------------------------------------------------------------------------------------
* NOTES
*
*   expects google analytics and jQuery to be available & configured on the page
*   expects <meta name="application-name" content="microsite" /> to contain the analytics category
*
****/

    window._gaq = window._gaq || [];
    window._gaq.push(['_setAccount', 'UA-17643673-1']);
    window._gaq.push(['_setDomainName', '.gam.com']);
    window._gaq.push(['_trackPageview']);

    (function () {
        var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
        ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
        var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
    })();

    $(function () {
        $("a[href$='.pdf'], a[href$='.PDF'], a[href$='.Pdf']").click(function () {
            var micrositeName = $('meta[name=application-name]').attr("content") || 'unknown';
            var hostPage = document.URL;
            var documentUrl = $(this).attr("href");
            _gaq.push(['_trackEvent', micrositeName, 'download', hostPage, documentUrl]);
        });
    })
