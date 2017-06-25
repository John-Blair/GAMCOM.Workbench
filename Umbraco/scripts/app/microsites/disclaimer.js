/****
* MODULE            [>] disclaimer.js
*                   [ ] GAM Microsite Disclaimer and Cookie Consent Module
* -----------------------------------------------------------------------------------------------
* GOALS
*
*   Display disclaimer text in a Shadowbox overlay. When the accepts the terms, the overlay is 
*   removed and the page displayed. The accept only lasts for the current session. 
*
****/
var disclaimer = (function (global) {

    /****
    * GOOGLE SETTINGS
    * -----------------------------------------------------------------------------------------------
    *   The standard setup, but forcing the _gaq definition into global scope.
    ****/

    global._gaq = global._gaq || [];
    global._gaq.push(['_setAccount', 'UA-17643673-1']);
    global._gaq.push(['_setDomainName', '.gam.com']);
    global._gaq.push(['_trackPageview']);

    /****
    * CONSTANTS
    * -----------------------------------------------------------------------------------------------
    *   VTOKEN          [>] a validation token value
    *                       should be unique for each Microsite to prevent cross site validation
    *   DISCLAIMER_TEXT [>] the location of the disclaimer text
    *                       may be a RELATIVE location for simple microsites, eg. 'disclaimer/text.htm'
    *                       or a FULLY QUALIFIED location for multi-level sites, eg. 'https://www.gam.com/microsites/example/disclaimer/myDisclaimer.htm'
    *   COOKIE_ID       [>] the name of the cookie that contains persisted consent information
    *   CONSENT_CHECKBOX_ID
    *                   [>] the id for the cookie guard checkbox 
    *                       will only need changed if you change the id of the input box in the disclaimer text template
    ****/

    var VTOKEN = 'JN1987-Dynamic';
    var DISCLAIMER_TEXT = 'disclaimertext.htm';
    var COOKIE_ID = 'cookieGuard';
    var CONSENT_CHECKBOX_ID = '#gamCookieConsentCheckbox';

    /****
    * PRIVATE METHOD    [>] loadDisclaimer
    *                   [ ] load the disclaimer text and display it in a configured shadowbox
    * -----------------------------------------------------------------------------------------------
    * NOTES
    *   The configuration of the shadowbox is set as part of the .open() method call.
    *   This is done with the options object; overriding the values in the autorun shadowbox initialisation.
    ****/
    var loadDisclaimer = function () {
        $.ajax({
            url: DISCLAIMER_TEXT,
            success: function (response, status, xhr) {
                Shadowbox.open({
                    content: response
                        , player: "html"
                        , height: 600
                        , width: 750
                        , options: {
                            animate: false
                            , enableKeys: false
                            , modal: true
                            , onFinish: disclaimerLoadedEvent
                        }
                });
            }
        });
    };


    /****
    * PRIVATE METHOD    [>] disclaimerLoadedEvent 
    *                   [ ] once this disclaimer is loaded, this function is called
    ****/
    var disclaimerLoadedEvent = function () {

        // remove the close button from the Shadowbox 
        $('#sb-nav-close').hide();

        // update the consent checkbox with any stored consent value
        synchPrivacyCheckbox();

    };

    /****
    * PRIVATE METHOD    [>] cookieConsentStatus 
    *                   [ ] returns True if there's a cookieGuard cookie; otherwise False;
    ****/
    var cookieConsentStatus = function () {

        var cookieValue = $.cookie(COOKIE_ID);
        if (cookieValue) {
            return cookieValue === 'Allowed';
        };

        return false; // default

    };

    /****
    * PRIVATE METHOD    [>] setCookieConsentStatus
    *                   [ ] set the value of the cookieGuard cookie, or remove it if there's no value passed
    ****/
    var setCookieConsentStatus = function (value) {

        if (value) {
            $.cookie(COOKIE_ID, value, { expires: 365 });
        } else {
            $.cookie(COOKIE_ID, null);
        };

    };

    /****
    * PRIVATE METHOD    [>] synchPrivacyCheckbox
    *                   [ ] update the cookie consent checkbox with the current setting
    ****/
    var synchPrivacyCheckbox = function () {

        var consentStatus = cookieConsentStatus();
        var consentCheckbox = $(CONSENT_CHECKBOX_ID);

        if (consentCheckbox.length > 0) {
            consentCheckbox.attr('checked', consentStatus);
        };

    };

    /****
    * PRIVATE METHOD    [>] storeCheckboxConsent
    *                   [ ] read the consent value form the checkbox and store it as a cookie
    ****/
    var storeCheckboxConsent = function () {

        var consentCheckbox = $(CONSENT_CHECKBOX_ID);

        if (consentCheckbox.length > 0) {
            if (consentCheckbox.attr('checked')) {
                setCookieConsentStatus('Allowed');
            } else {
                setCookieConsentStatus('Refused');
            }
        };

    };

    /****
    * PRIVATE METHOD    [>] removeAllCookies
    *                   [ ] remove all cookies, under the current domain
    * -----------------------------------------------------------------------------------------------
    * NOTES
    *
    *   Take a good look at the space after the semicolon in the split statement. You need that! The cookie 
    *   string contains spaces and if you try to delete a cookie where the name has a space in front of it, 
    *   it won't work.
    *
    *   ie 'cookie1=first value; cookie2=next value'
    *
    ****/
    var removeAllCookies = function () {

        // parse the list of name/value pairs from the document's cookie property
        var cookieList = document.cookie.split('; ');

        // then delete each one, passing the name only
        for (var i = 0; i < cookieList.length; i++) {
            removeNamedCookie(cookieList[i].split('=')[0]);
        }

    };

    /****
    * PRIVATE METHOD    [>] removeNamedCookie 
    *                   [ ] remove a named cookie
    * -----------------------------------------------------------------------------------------------
    *   name            [>] cookie name, should not contain any spaces
    * - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
    * NOTES
    *   Google analytics cookies are written under the .gam.com domain, so we try that first.
    *   Then we try again under the natural domain.
    ****/
    var removeNamedCookie = function (name) {
        $.cookie(name, null, { domain: '.gam.com' });
        $.cookie(name, null);
    }

    /****
    * PRIVATE METHOD    [>] checkCookies
    *                   [ ] if we previously had consent, but now don't, we should remove all cookies
    ****/
    var removeOldCookiesIfNoConsent = function () {

        if (!cookieConsentStatus()) {
            removeAllCookies();
        }

    };

    /****
    * PRIVATE METHOD    [>] addGoogleAnalyticsToPage
    *                   [ ] wrapper for the standard Google Analytics include script
    * -----------------------------------------------------------------------------------------------
    * NOTES
    *   Analytics will only be added if the consent check passes.  
    ****/
    var conditionalAddGoogleAnalyticsToPage = function () {

        if (cookieConsentStatus()) {
            var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
            ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
            var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
        };

    };

    /****
    * PUBLIC METHOD     [>] disclaimer.accept
    *                   [ ] call this method to accept the disclaimer and show the page
    * -----------------------------------------------------------------------------------------------
    * NOTES
    *   Use with a hyperlink eg. <a href="#" onclick="disclaimer.accept();"> 
    ****/
    var accept = function () {

        // set the window name, indicating the disclaimer has been accepted
        window.name = VTOKEN;
        setCookieConsentStatus('Allowed');

        // close the colorbox overlay and allow the base window to show scrollbars
        Shadowbox.close();
        window.onload = function () { oldLoad() };

        // if analytics are permitted, allow them to be loaded for the page
        conditionalAddGoogleAnalyticsToPage();

        // call any onLoad method which was defined before the disclaimer code grabbed the event
        oldLoad();

    };

    /****
    * PUBLIC METHOD     [>] disclaimer.refuse
    *                   [ ] call this method to refuse the disclaimer and show the page
    * -----------------------------------------------------------------------------------------------
    * NOTES
    *   Use with a hyperlink eg. <a href="#" onclick="disclaimer.accept();"> 
    ****/
    var refuse = function () {

        // set the window name, indicating the disclaimer has been accepted
        window.name = VTOKEN;
        setCookieConsentStatus('Refused');

        // close the colorbox overlay and allow the base window to show scrollbars
        Shadowbox.close();

        // restore old onLoad
        window.onload = function () { oldLoad() };
        oldLoad();

    };

    /****
    * AUTORUN: shadowbox initialisation
    * -----------------------------------------------------------------------------------------------
    *   Initialises the default values for the shadowbox. These are the values that will be used
    *   if shadowbox is used by the Microsite code to display a video.
    ****/
    Shadowbox.init({
        overlayOpacity: "0.8",
        fadeDuration: "0.5"
    });

    /****
    * AUTORUN: main
    * -----------------------------------------------------------------------------------------------
    ****/

    // the window has not passed disclaimer checking
    // google analytics will be conditionally added to the page by the accept method
    if (window.name !== VTOKEN) {

        // hook the window load, but play nice with any existing onload functions
        var oldLoad = window.onload || function () { return true; };
        window.onload = loadDisclaimer;

    } else {

        // the window has passed disclaimer checking
        // google analytics will be conditionally added to the page here
        conditionalAddGoogleAnalyticsToPage();
    }


    /****
    * PUBLIC METHODS
    * These methods are available to pages which reference disclaimer.js
    * -----------------------------------------------------------------------------------------------
    ****/
    return {
        accept: accept
        , refuse: refuse
    };

})(window);
