/****
* MODULE            [>] showProfSelfCerts.js
*                   [ ] 
* -----------------------------------------------------------------------------------------------
* NOTES
*
*   provides the logic to link the self certification compliance to the appropriate disclaimer
*   use <a onClick='javascript:ertify.as('UK-Prof')>
*
****/
var certifyInd = (function () {

    // extract the current language from the url
    function currentLanguageCode() {
        // current language is de is current url contains /de/ or ends with /de
        if ( (document.location.href.indexOf("/de/") != -1)
              || (document.location.href.indexOf("/de", document.location.href.length - 3) !== -1) ) {
            return 'de';
        }
        return 'en';
    }

    var as = function (complianceGroup, country) {

        var nop = function () { };

        var saveOK = function () {
            document.location = '/compliance/groups/individual/' + complianceGroup + '/disclaimer/' + currentLanguageCode();
        };

        gam.umbraco.client.services.authorization.savePendingCertProfile(complianceGroup, country, 'individual', saveOK, nop);
        
        return false;
    }

    /* public interface */
    return {
        as: as
    };

})();