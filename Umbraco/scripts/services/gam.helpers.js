/// <reference path="../lib/jQuery/jquery-1.10.2.min.js" />
/****
* MODULE            : gam.helpers.js
*                   : Helper methods used by the umbraco client services.
* -----------------------------------------------------------------------------------------------
* NAMESPACE         : 
*                   : gam.string
*                   : gam.debug
*                   : gam.uri
* -----------------------------------------------------------------------------------------------
* HELPER METHODS    
*                   : gam.string.format: Format a string e.g. gam.string.format('{0} is not {1} + {2}', 'JavaScript', 'Java', 'Script')
*                   : gam.debug.print: Print debug messages to the browser
*                   : gam.debug.printError: Print debug error messages to the browser
*                   : gam.debug.objectToString: Convert an object (recursively) to a string
*                   : gam.debug.print.options: Debug options including switching on/off, and target container for messages.
*                   : gam.uri.parseUri: Parse a url into its constituent parts - including querystring argument extraction.
* -----------------------------------------------------------------------------------------------
****/

// Keep everything in the gam namespace.
var gam = gam || {};

// "Extend" the string object with helper functions.
gam.string = gam.string || {};

/****
* PUBLIC METHOD    
*   Formats arguments according to the formatting string.  
*   Each occurence of the "{\d+}" substring refers to  
*   the appropriate argument.  
*   Use the gam namespace to avoid potential override in the global namespace.
*   Example 
*   gam.string.format('{0} is not {1} + {2}', 'JavaScript', 'Java', 'Script')
*/
gam.string.format = function (stringToFormat) {
    // The functions takes a variable number of arguments according to the placeholders present in the string to format.
    var args = arguments;

    return stringToFormat.replace(/\{(\d+)\}/g, function (p0, p1) {
        // Skip over the string to format from the arguments to 
        // align the format placeholders with the actual arguments.

        // p0 is the matched string e.g. {0}
        // p1 is the 0 part of the matched string.
        var argIndex = parseInt(p1) + 1;
        return args[argIndex] !== void 0 ? args[argIndex] : p0;
    });
};

// Url helper methods.
gam.uri = gam.uri || {};

/****
* PUBLIC METHOD    
* 
* parseUri 1.2.2
* (c) Steven Levithan <stevenlevithan.com>
*  MIT License
****/
gam.uri.parseUri = function (str) {
    var o = gam.uri.parseUri.options,
          m = o.parser[o.strictMode ? "strict" : "loose"].exec(str),
          uri = {},
          i = 14;

    while (i--) uri[o.key[i]] = m[i] || "";

    uri[o.q.name] = {};
    uri[o.key[12]].replace(o.q.parser, function ($0, $1, $2) {
        if ($1) uri[o.q.name][$1] = $2;
    });

    return uri;
};

gam.uri.parseUri.options = {
    strictMode: true,
    key: ["source", "protocol", "authority", "userInfo", "user", "password", "host", "port", "relative", "path", "directory", "file", "query", "anchor"],
    q: {
        name: "queryKey",
        parser: /(?:^|&)([^&=]*)=?([^&]*)/g
    },
    parser: {
        strict: /^(?:([^:\/?#]+):)?(?:\/\/((?:(([^:@]*)(?::([^:@]*))?)?@)?([^:\/?#]*)(?::(\d*))?))?((((?:[^?#\/]*\/)*)([^?#]*))(?:\?([^#]*))?(?:#(.*))?)/,
        loose: /^(?:(?![^:@]+:[^:@\/]*@)([^:\/?#.]+):)?(?:\/\/)?((?:(([^:@]*)(?::([^:@]*))?)?@)?([^:\/?#]*)(?::(\d*))?)(((\/(?:[^?#](?![^?#\/]*\.[^?#\/.]+(?:[?#]|$)))*\/?)?([^?#\/]*))(?:\?([^#]*))?(?:#(.*))?)/
    }
};


// Debug helper methods.
gam.debug = gam.debug || {};


/****
* PUBLIC METHOD    
* Print a debug message to the debug container 
* (which is created in the body element if it doesn't exist).
****/
gam.debug.print = function (message) {
    // Only output debug messages if enabled.
    if (!gam.debug.print.options.enabled) return;

    gam.debug.print.createContainer();

    // Output the debug message to the end of the debug container.
    var messageContainer = gam.string.format('<div class="debugInfo" style="{0}"/>', gam.debug.print.options.messageStyle);
    $(messageContainer)
        .html(String(message))
        .appendTo($('#' + gam.debug.print.options.targetId));
};

/****
* PUBLIC METHOD    
* Print an error message to the debug container 
* (which is created in the body element if it doesn't exist).
* Errors will appear in red.
****/
gam.debug.printError = function (message) {
    // Only output debug messages if enabled.
    if (!gam.debug.print.options.enabled) return;

    gam.debug.print.createContainer();

    // Output the debug error message to the end of the debug container.
    var messageContainer = gam.string.format('<div class="debugError" style="{0}"/>', gam.debug.print.options.errorMessageStyle);
    $(messageContainer)
        .html(String(message))
        .appendTo($('#' + gam.debug.print.options.targetId));
};

/****
* HELPER METHOD    
* Create the debug message container 
****/
gam.debug.print.createContainer = function () {
    // Only output debug messages if enabled.
    if (!gam.debug.print.options.enabled) return;

    var debugContainerId = gam.debug.print.options.targetId;

    // Create the debug container - if it doesn't exist.
    if ($('#' + debugContainerId).length == 0) {
        var container = gam.string.format('<div id="{0}" style="{1}"><div class="header" style="{2}">{3}</div></div>', debugContainerId, gam.debug.print.options.targetStyle, gam.debug.print.options.headerStyle, gam.debug.print.options.headerText);
        $(container).appendTo('body');
    }
};

gam.debug.print.options = {
    // Enable or disable debug message display in the browser.
    // Switch debug off prior to releasing.
    enabled: true,

    // Identify the body tag container element as the recipient of debug messages.
    // The element will be created if it does not exist.
    targetId: "gamDebugMessages",
    targetStyle: "border:2px solid black; padding:10px;",
    headerText: "GAM Ajax Debug Messages",
    headerStyle: "text-align: center; color: #066; background-color:#D3D3D3; margin-bottom:10px;",
    messageStyle: "border:1px solid #D3D3D3; margin-bottom:10px; padding: 5px;",
    errorMessageStyle: "border:1px solid red; margin-bottom:10px; padding: 5px; color: red;"

};

/****
* PUBLIC METHOD    
* Convert an object to its string representation - recursion applied to embedded objects (with properties).
* Output formatted for display in a browser.
****/
gam.debug.objectToString = function (obj) {
    var str = '{<br/>';
    for (var p in obj) {
        if (obj.hasOwnProperty(p) && (typeof (obj[p]) != "function")) {
            str += p + ':' + obj[p] + ',<br/>';
            //Following causes an Out of Stack error on IE9 only - only need first level properties for now.
            //if (!$.isEmptyObject(obj[p])) {
            //  str += gam.debug.objectToString(obj[p]);
            //}
        }
    }
    str += "}";

    return str;
};


gam.helper = gam.helper || {};

// Toggle all checkboxes.
gam.helper.toggle = function (obj)
{
    var checkboxes = $(obj).closest('form').find(':checkbox');
    if ($(obj).prop('checked')) {
        checkboxes.prop('checked', true);
    } else {
        checkboxes.prop('checked', false);
    }
};

// Get the current browser language en or de.
gam.helper.getCurrentLanguage = function ()
{
    var text = $("#currentLanguage").text();
    var currentLanguage = $.trim(text).toLowerCase();
    return currentLanguage;
    
};

// Replace the language element in the url with current language.
gam.helper.URLInCurrentLanguage = function (url) {
    return url.replace("/en/", "/" + gam.helper.getCurrentLanguage() + "/");
};

// Add the current language to a set of posted values.
// This is used server side to return language based text and urls.
gam.helper.addCurrentLanguage = function (postedData)
{
    if (postedData.length > 0)
    {
        postedData += "&";
    }
    postedData += "currentLanguage=" + gam.helper.getCurrentLanguage();
    return postedData;
};


