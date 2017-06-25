/// <reference path="../lib/jQuery/jquery-1.10.2.min.js" />
/****
* MODULE            : gam.umbraco.client.services.js
*                   : Ajax services to interact with the Umbraco BASE rest extension methods in 
*                     Gam.Umbraco.Services.dll
* -----------------------------------------------------------------------------------------------
* NAMESPACE         : gam.umbraco
*                   : gam.umbraco.client
*                   : gam.umbraco.client.services
*                   : gam.umbraco.client.services.manager
*                   : gam.umbraco.client.services.fund
*                   : gam.umbraco.client.services.authorization
* -----------------------------------------------------------------------------------------------
* PUBLIC METHODS    : gam.umbraco.client.services.manager.commentary
*                   : gam.umbraco.client.services.manager.biography
*                   : gam.umbraco.client.services.manager.funds
*                   : gam.umbraco.client.services.fund.findDocument
*                   : gam.umbraco.client.services.authorization.certifyAs(complianceGroup)
*                   : gam.umbraco.client.services.authorization.savePendingCertProfile(complianceGroup, country, investorType, onSuccess, onFailure) 
* -----------------------------------------------------------------------------------------------
****/

var gam = gam || {};
gam.umbraco = gam.umbraco || {};
gam.umbraco.client = gam.umbraco.client || {};
gam.umbraco.client.services = gam.umbraco.client.services || {};
gam.umbraco.client.services.manager = gam.umbraco.client.services.manager || {};
gam.umbraco.client.services.fund = gam.umbraco.client.services.fund || {};
gam.umbraco.client.services.authorization = gam.umbraco.client.services.authorization || {};
gam.umbraco.client.services.user = gam.umbraco.client.services.user || {};


// Global client side error handler.
$(document).ajaxError(function (event, request, settings, exception) {
    var errorMsg = gam.string.format("Response:{0}<br/>Request:{1}<br/>Exception:{2}",
                gam.debug.objectToString(request),
                gam.debug.objectToString(settings),
                exception
               );
    gam.debug.printError(errorMsg);
});


// Central service call specific error handler.
gam.umbraco.client.services.error = function (jqXHR, textStatus, errorThrown) {
    var errorMsg = gam.string.format("jqXHR:{0}\ntextStatus:{1}\nerrorThrown:{2}",
               gam.debug.objectToString(jqXHR),
               textStatus,
               errorThrown
              );
    alert(errorMsg);
}



/****
* PUBLIC SERVICE METHODS
*    Get a manager commentary
****/
gam.umbraco.client.services.manager.commentary = function () {

    //alert('Entered gam.umbraco.client.services.manager.commentary');

    var managerName = $("#managerCommentaryName option:selected").text();

    var request = {
        url: '/base/Manager/Commentary/' + managerName + ".aspx",
        type: 'get',
        success: function (data) {
            $("#managerServiceResult").empty().append(data);
        }
    };

    $.ajax(request);

    // Cancel the default click behaviour - avoiding a screen refresh.
    return false;
};

gam.umbraco.client.services.manager.biography = function () {

    //alert('Entered gam.umbraco.client.services.manager.biography');
    var managerName = $("#managerBiographyName option:selected").text();

    var request = {
        url: '/base/Manager/Biography/' + managerName + ".aspx",
        type: 'get',
        success: function (data) {
            $("#managerBiographyServiceResult").empty().append(data);
        }
    };

    $.ajax(request);

    return false;
};

gam.umbraco.client.services.manager.funds = function () {
    var managerName = $("#managerFundsName option:selected").text().trim();

    var request = {
        url: '/base/Manager/Funds/' + managerName + ".aspx",
        type: 'get',
        success: function (data) {
            $("#managerFundsServiceResult").empty().append(data);
        }
    };

    $.ajax(request);

    return false;
};


gam.umbraco.client.services.fund.findDocument = function ()
{

    var request = {
        url: '/base/Fund/FindDocument.aspx',
        type: 'post',
        data: gam.helper.addCurrentLanguage($('form#findDocument').serialize()),
        success: function (data)
        {
            // Debug: Display the posted values.
            gam.debug.print(data);
            if (data.indexOf("/") == 0)
            {
                window.location.href = data;
            } else
            {
                $("#findDocumentServiceResult").empty().append(data);
            }
        }
    };

    //var infoMsg = gam.string.format("Request:{0}<br/>",gam.debug.objectToString(request));
    //gam.debug.print(infoMsg);

    $.ajax(request);

    return false;
};

gam.umbraco.client.services.authorization.certifyAs = function (complianceGroup, onSuccess, onFailure)
{

    var request = {
        url: '/base/Authorization/CertifyAs/' + complianceGroup,
        type: 'post',
        success: function (data) { onSuccess(); },
        error:  function (data) { onFailure(); }
    };

    $.ajax(request);
    return false;

};

gam.umbraco.client.services.authorization.certifyAsProfile = function (complianceGroup, country, investorType, onSuccess, onFailure) {

    var service = gam.string.format("/base/Authorization/CertifyAsProfile/{0}/{1}/{2}",complianceGroup, country, investorType);
    var request = {
        url: service,
        type: 'post',
        success: function (data) { onSuccess(); },
        error: function (data) { onFailure(); }
    };

    $.ajax(request);
    return false;

};

gam.umbraco.client.services.authorization.savePendingCertProfile = function (complianceGroup, country, investorType, onSuccess, onFailure) {

    var service = gam.string.format("/base/Authorization/StorePendingCertValues/{0}/{1}/{2}", complianceGroup, country, investorType);
    var request = {
        url: service,
        type: 'post',
        success: function (data) { onSuccess(); },
        error: function (data) { onFailure(); }
    };

    $.ajax(request);
    return false;

};

gam.umbraco.client.services.user.setProfile = function (country, investorType) {

    var service = gam.string.format("/base/User/SetProfile/{0}/{1}", country, investorType);
    var request = {
        url: service,
        type: 'post',
        success: function (data) { alert("success:" + data) },
        error: gam.umbraco.client.services.error
    };

    $.ajax(request);
    return false;

};