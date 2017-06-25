/****
* MODULE            : changeprofile.js
*                   : Change the user's profile.
* -----------------------------------------------------------------------------------------------
* NAMESPACE         : gam.form.changeProfile
* -----------------------------------------------------------------------------------------------
* NOTES             : TODO Multi-Lingual text access from the server.
*                   : TODO Disclaimer access from the server.
*                   : TODO Mobile version of disclaimer.
*                   : TODO Set profile on the server on disclaimer accept for re-use on subsequent pages.
*                   : TODO Header update to reflect current selected profile.
* -----------------------------------------------------------------------------------------------
****/


var gam = gam || {};
gam.form = gam.form || {};
gam.form.changeProfile = gam.form.changeProfile || {};

//Identify controls.
gam.form.changeProfile.controls = gam.form.changeProfile.controls || {};
gam.form.changeProfile.controls.changeProfileProfessionalCountry = "#changeProfileProfessionalCountry";
gam.form.changeProfile.controls.changeProfileIndividualCountry = "#changeProfileIndividualCountry";




gam.form.changeProfile.controls.changeProfileLanguage = "#changeProfileLanguage";
gam.form.changeProfile.controls.changeProfileInvestorType = "#changeProfileInvestorType";
gam.form.changeProfile.controls.changeProfileDialog = "#changeProfileDialog";
gam.form.changeProfile.controls.changeProfileDisclaimerDialog = "#changeProfileDisclaimerDialog";
gam.form.changeProfile.controls.changeProfileError = "#changeProfileError";


gam.form.changeProfile.dialogActions = gam.form.changeProfile.dialogActions || {};
gam.form.changeProfile.dialogActions.open = "open";
gam.form.changeProfile.dialogActions.close = "close";

// Process combined selections.
gam.form.changeProfile.country = "";
gam.form.changeProfile.language = "";
gam.form.changeProfile.investorType = "";
gam.form.changeProfile.investorTypeProfessional = "professional";
gam.form.changeProfile.investorTypeIndividual = "individual";

// Private functions.
gam.form.changeProfile.clearError = function ()
{
    $(gam.form.changeProfile.controls.changeProfileError).empty().hide();
};

gam.form.changeProfile.setError = function (error) {
    $(gam.form.changeProfile.controls.changeProfileError).text(error);
};

// Reset the form to its default state.
gam.form.changeProfile.resetForm = function()
{
    gam.form.changeProfile.clearError();

    // Default is to display the "larger" professional list of countries.
    var hidden = "hidden";
    $(gam.form.changeProfile.controls.changeProfileIndividualCountry).addClass(hidden);
    $(gam.form.changeProfile.controls.changeProfileProfessionalCountry).removeClass(hidden);


    $(gam.form.changeProfile.controls.changeProfileProfessionalCountry)[0].selectedIndex = 0;
    $(gam.form.changeProfile.controls.changeProfileIndividualCountry)[0].selectedIndex = 0;

    // Default to English on the English website and German on the German website.
    $(gam.form.changeProfile.controls.changeProfileLanguage).val(gam.helper.getCurrentLanguage());


    $(gam.form.changeProfile.controls.changeProfileInvestorType)[0].selectedIndex = 0;
};

// Validate the form.
gam.form.changeProfile.validateForm = function ()
{

    gam.form.changeProfile.clearError();

    gam.form.changeProfile.investorType = $(gam.form.changeProfile.controls.changeProfileInvestorType).val();

    if (gam.form.changeProfile.investorType == gam.form.changeProfile.investorTypeProfessional)
    {
        gam.form.changeProfile.country = $(gam.form.changeProfile.controls.changeProfileProfessionalCountry).val();
    }
    else
    {
        gam.form.changeProfile.country = $(gam.form.changeProfile.controls.changeProfileIndividualCountry).val();
    }

    gam.form.changeProfile.language = $(gam.form.changeProfile.controls.changeProfileLanguage).val();
    gam.form.changeProfile.valid = false;
    
    if (gam.form.changeProfile.investorType == "") {
        // User not selected full profile.
        gam.form.changeProfile.setError("Please select an investor type.");
    }
    else if (gam.form.changeProfile.country == "") {
        // User not selected full profile.
        gam.form.changeProfile.setError("Please select a country");
    }
    else if (gam.form.changeProfile.language == "") {
        // User not selected full profile.
        gam.form.changeProfile.setError("Please select a language");
    }
    else
    {
         gam.form.changeProfile.valid = true;
    }

    if (!gam.form.changeProfile.valid)
    {
        $(gam.form.changeProfile.controls.changeProfileError).show();
    } else
    {
        $(gam.form.changeProfile.controls.changeProfileError).empty().hide();
    }

    // Cancel submit if invalid.
    return gam.form.changeProfile.valid;
};

// Display the disclaimer and proceed to the self-cert home page.
gam.form.changeProfile.as = function (complianceGroup, countryNodeId, language, investorType) {
    var nop = function () {
    };

    var saveOK = function () {
        document.location = '/compliance/groups/' + investorType + '/' + complianceGroup + '/disclaimer/' + language;
    };

    gam.umbraco.client.services.authorization.savePendingCertProfile(complianceGroup, countryNodeId, investorType, saveOK, nop);

    return false;
};



// Public functions.
gam.form.changeProfile.submitForm = function () {

    if (gam.form.changeProfile.validateForm()) {

        $(gam.form.changeProfile.controls.changeProfileDialog).dialog(gam.form.changeProfile.dialogActions.close);


        // Display the disclaimer and self-cert the user per selections.
        var decodedCountry = gam.form.changeProfile.country.split(",");
        var complianceGroup = decodedCountry[0];
        var countryNodeId = decodedCountry[1];

        gam.form.changeProfile.as(complianceGroup, countryNodeId, gam.form.changeProfile.language, gam.form.changeProfile.investorType);
    }

    // Cancel a links following the href.
    return false;


};


gam.form.changeProfile.cancel = function()
{
    $(gam.form.changeProfile.controls.changeProfileDialog).dialog(gam.form.changeProfile.dialogActions.close);
    return false;
};


gam.form.changeProfile.selectionChanged = function () {

    $(gam.form.changeProfile.controls.changeProfileError).empty().hide();

};

gam.form.changeProfile.investorTypeSelectionChanged = function () {

    $(gam.form.changeProfile.controls.changeProfileError).empty().hide();

    // Make sure the correct country list is displayed for the type of investor displayed.
    // Currently, Ireland is the only addition to the Professional list.
    var investorType = $(gam.form.changeProfile.controls.changeProfileInvestorType).val();
    
    var hidden = "hidden";
    var countryText = "";

    if (investorType == gam.form.changeProfile.investorTypeProfessional) {
        // Need to make sure Professional Country list is displayed.
        // Make sure any current individual country selection is selected - if present in prof country list.
        if ($(gam.form.changeProfile.controls.changeProfileProfessionalCountry).hasClass(hidden))
        {
            // Get current individual country selection.
            countryText = $(gam.form.changeProfile.controls.changeProfileIndividualCountry + ' option:selected').text();
            // Set it in the professional country list if present.
            
            $(gam.form.changeProfile.controls.changeProfileProfessionalCountry + ' option').
                filter(function () {return ($(this).text() == countryText); }).attr('selected', 'selected'); 
            
            if ($(gam.form.changeProfile.controls.changeProfileProfessionalCountry + ' option:selected').text() != countryText) {
                //Did not manage to select the country - reset selection to prompt.
                $(gam.form.changeProfile.controls.changeProfileProfessionalCountry)[0].selectedIndex = 0;
            }
            
            $(gam.form.changeProfile.controls.changeProfileIndividualCountry).addClass(hidden);
            $(gam.form.changeProfile.controls.changeProfileProfessionalCountry).removeClass(hidden);
        }

    }
    else if (investorType == gam.form.changeProfile.investorTypeIndividual) {
        // Need to make sure Individual Country list is displayed.
        // Make sure any current country selection is selected.

        if ($(gam.form.changeProfile.controls.changeProfileIndividualCountry).hasClass(hidden)) {

            countryText = $(gam.form.changeProfile.controls.changeProfileProfessionalCountry + ' option:selected').text();
            
            $(gam.form.changeProfile.controls.changeProfileIndividualCountry + ' option').
                filter(function () { return ($(this).text() == countryText); }).attr('selected', 'selected');

            if ($(gam.form.changeProfile.controls.changeProfileIndividualCountry + ' option:selected').text() != countryText) {
                //Did not manage to select the country - reset selection to prompt.
                $(gam.form.changeProfile.controls.changeProfileIndividualCountry)[0].selectedIndex = 0;
            }

            $(gam.form.changeProfile.controls.changeProfileProfessionalCountry).addClass(hidden);
            $(gam.form.changeProfile.controls.changeProfileIndividualCountry).removeClass(hidden);
        }
    }
    
    //Note: If no type of investor is selected don't change anything - let the validation guide the user.


};


gam.form.changeProfile.chooseYourProfile = function ()
{
    
    gam.form.changeProfile.resetForm();
    
    $(gam.form.changeProfile.controls.changeProfileDialog).dialog({
            autoOpen: false,
            width: 320,
            modal: true,
            closeOnEscape: false,
            resizable: false,
            dialogClass: "no-close"
    });
    
    // Remove the standard dialog title.
    $(".ui-dialog-titlebar").hide();

    $(gam.form.changeProfile.controls.changeProfileDialog).dialog(gam.form.changeProfile.dialogActions.open);

    return false;

};



/****
* MODULE            : chooseprofile.js
*                   : Choose the user's profile.
* -----------------------------------------------------------------------------------------------
* NAMESPACE         : gam.form.chooseProfile
* -----------------------------------------------------------------------------------------------
* NOTES             : TODO Multi-Lingual text access from the server.
*                   : TODO Disclaimer access from the server.
*                   : TODO Mobile version of disclaimer.
*                   : TODO Set profile on the server on disclaimer accept for re-use on subsequent pages.
*                   : TODO Header update to reflect current selected profile.
* -----------------------------------------------------------------------------------------------
****/


var gam = gam || {};
gam.form = gam.form || {};
gam.form.chooseProfile = gam.form.chooseProfile || {};

//Identify controls.
gam.form.chooseProfile.controls = gam.form.chooseProfile.controls || {};
gam.form.chooseProfile.controls.chooseProfileProfessionalCountry = "#chooseProfileProfessionalCountry";
gam.form.chooseProfile.controls.chooseProfileIndividualCountry = "#chooseProfileIndividualCountry";
gam.form.chooseProfile.controls.chooseProfileLanguage = "#chooseProfileLanguage";
gam.form.chooseProfile.controls.chooseProfileInvestorType = "#chooseProfileInvestorType";
gam.form.chooseProfile.controls.chooseProfileDialog = "#chooseProfileDialog";
gam.form.chooseProfile.controls.chooseProfileDisclaimerDialog = "#chooseProfileDisclaimerDialog";
gam.form.chooseProfile.controls.chooseProfileError = "#chooseProfileError";


gam.form.chooseProfile.dialogActions = gam.form.chooseProfile.dialogActions || {};
gam.form.chooseProfile.dialogActions.open = "open";
gam.form.chooseProfile.dialogActions.close = "close";

// Process combined selections.
gam.form.chooseProfile.country = "";
gam.form.chooseProfile.language = "";
gam.form.chooseProfile.investorType = "";
gam.form.chooseProfile.investorTypeProfessional = "professional";
gam.form.chooseProfile.investorTypeIndividual = "individual";

// Private functions.
gam.form.chooseProfile.clearError = function () {
    $(gam.form.chooseProfile.controls.chooseProfileError).empty().hide();
};

gam.form.chooseProfile.message = function (msgId) {

    return gam.form.messages.chooseProfileMessages[gam.helper.getCurrentLanguage()][msgId];
}

gam.form.chooseProfile.setError = function (msgId) {
    alert("Msg:" + gam.form.chooseProfile.message(msgId));
    $(gam.form.chooseProfile.controls.chooseProfileError).text(gam.form.chooseProfile.message(msgId));
};




// Validate the form.
gam.form.chooseProfile.validateForm = function () {

    gam.form.chooseProfile.clearError();

    gam.form.chooseProfile.investorType = $(gam.form.chooseProfile.controls.chooseProfileInvestorType).val();

    if (gam.form.chooseProfile.investorType == gam.form.chooseProfile.investorTypeProfessional) {
        gam.form.chooseProfile.country = $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry).val();
    }
    else {
        gam.form.chooseProfile.country = $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry).val();
    }

    gam.form.chooseProfile.language = $(gam.form.chooseProfile.controls.chooseProfileLanguage).val();
    gam.form.chooseProfile.valid = false;

    if (gam.form.chooseProfile.investorType == "") {
        // User not selected full profile.
        gam.form.chooseProfile.setError("selectInvestorType");
    }
    else if (gam.form.chooseProfile.country == "") {
        // User not selected full profile.
        gam.form.chooseProfile.setError("selectCountry");
    }
    else if (gam.form.chooseProfile.language == "") {
        // User not selected full profile.
        gam.form.chooseProfile.setError("selectLanguage");
    }
    else {
        gam.form.chooseProfile.valid = true;
    }

    if (!gam.form.chooseProfile.valid) {
        $(gam.form.chooseProfile.controls.chooseProfileError).show();
    } else {
        $(gam.form.chooseProfile.controls.chooseProfileError).empty().hide();
    }

    // Cancel submit if invalid.
    return gam.form.chooseProfile.valid;
};


// Display the disclaimer and proceed to the self-cert home page.
gam.form.chooseProfile.as = function(complianceGroup, countryNodeId, language, investorType)
{
    var nop = function()
    {
    };

    var saveOK = function()
    {
        document.location = '/compliance/groups/' + investorType + '/' + complianceGroup + '/disclaimer/' + language;
    };

    gam.umbraco.client.services.authorization.savePendingCertProfile(complianceGroup, countryNodeId, investorType, saveOK, nop);

    return false;
};



// Public functions.
gam.form.chooseProfile.submitForm = function () {

    if (gam.form.chooseProfile.validateForm()) {

        // Display the disclaimer and self-cert the user per selections.
        var decodedCountry = gam.form.chooseProfile.country.split(",");
        var complianceGroup = decodedCountry[0];
        var countryNodeId = decodedCountry[1];

        gam.form.chooseProfile.as(complianceGroup, countryNodeId, gam.form.chooseProfile.language, gam.form.chooseProfile.investorType);
    }

    // Cancel a links following the href.
    return false;


};


gam.form.chooseProfile.selectionChanged = function () {

    $(gam.form.chooseProfile.controls.chooseProfileError).empty().hide();

};

gam.form.chooseProfile.investorTypeSelectionChanged = function () {

    $(gam.form.chooseProfile.controls.chooseProfileError).empty().hide();

    // Make sure the correct country list is displayed for the type of investor displayed.
    // Currently, Ireland is the only addition to the Professional list.
    var investorType = $(gam.form.chooseProfile.controls.chooseProfileInvestorType).val();

    var hidden = "hidden";
    var countryText = "";

    if (investorType == gam.form.chooseProfile.investorTypeProfessional) {
        // Need to make sure Professional Country list is displayed.
        // Make sure any current individual country selection is selected - if present in prof country list.
        if ($(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry).hasClass(hidden)) {
            // Get current individual country selection.
            countryText = $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry + ' option:selected').text();
            // Set it in the professional country list if present.

            $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry + ' option').
                filter(function () { return ($(this).text() == countryText); }).attr('selected', 'selected');

            if ($(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry + ' option:selected').text() != countryText) {
                //Did not manage to select the country - reset selection to prompt.
                $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry)[0].selectedIndex = 0;
            }

            $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry).addClass(hidden);
            $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry).removeClass(hidden);
        }

    }
    else if (investorType == gam.form.chooseProfile.investorTypeIndividual) {
        // Need to make sure Individual Country list is displayed.
        // Make sure any current country selection is selected.

        if ($(gam.form.chooseProfile.controls.chooseProfileIndividualCountry).hasClass(hidden)) {

            countryText = $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry + ' option:selected').text();

            $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry + ' option').
                filter(function () { return ($(this).text() == countryText); }).attr('selected', 'selected');

            if ($(gam.form.chooseProfile.controls.chooseProfileIndividualCountry + ' option:selected').text() != countryText) {
                //Did not manage to select the country - reset selection to prompt.
                $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry)[0].selectedIndex = 0;
            }

            $(gam.form.chooseProfile.controls.chooseProfileProfessionalCountry).addClass(hidden);
            $(gam.form.chooseProfile.controls.chooseProfileIndividualCountry).removeClass(hidden);
        }
    }

    //Note: If no type of investor is selected don't change anything - let the validation guide the user.


};






/// <reference path="../../lib/jQuery/jquery.placeholder.js" />
/****
* MODULE            [>] header.js
*                   [ ] Add header functionality
*                          : Placeholder functionality for older browsers IE8.
* -----------------------------------------------------------------------------------------------
* NOTES
*
*
****/

$(
    function () {
        $("input[type='search']").placeholder();

        $("a.menuTab").gammenu();
        
    }
);




/****
* MODULE            [>] tileLayout.js
*                   [ ] tile layout algorithm
* -----------------------------------------------------------------------------------------------
* NOTES
*
*
****/
var layoutManager = (function () {

    // variables private to layoutManager 

    var redrawRequested = false;                // is a redraw request already underway
    var lastContainerWidth = 0;                 // allows us to check if we have already done a layout for this screen  size

    // methods private to layoutManager 

    // fetch all tile elements from the dom
    // return an array with each tile extended with a .placed property
    var fetchTiles = function () {

        var t = [];
        // jQuery sort function
        var sortByPriority = function (a, b) {
            var a1 = a.attr('data-priority');
            var b1 = b.attr('data-priority');
            if (a1 == b1) return 0;
            return a1 > b1 ? 1 : -1;
        }

        // only position visible tiles
        $('.tile:visible').each(function (index) {
            var tile = $(this);
            tile.placed = false; // tile hasn't yet been laid on to the layout
            t.push(tile);
        });

        // sort tiles[] by data-priority attribute
        t.sort(sortByPriority);
        return t;

    }

    // LAYOUT STRATEGY: float
    // a basic float arrangement, tiles all set to the same width and ordered by data-priority
    // designed for smaller mobile sizes (blackberry, iphone)
    var layoutAsFloat = function () {

        var tiles = [];

        // $ will execute a move on any appended element which already exists in the dom
        // by appending in the correct order, we can sort the tiles into data-priority order
        var placeTiles = function (t) {
            for (var i = 0; i < t.length; i++) {
                // scrub any absolute positioning from the tiles and allow the normal float to operate
                tiles[i].css({ 'position': 'static', 'float': 'left', 'top': '0', 'left': '0', 'opacity': '1.0' });
                tiles[i].appendTo($('#tileContainer'));
            };
        };

        /* execution body */
        tiles = fetchTiles();
        placeTiles(tiles);
    };

    // LAYOUT STRATEGY: absolute
    // a typical tile interface, with elements sorted by data-priority but tesselated by best-fit
    // designed for desktops and tablets
    var layoutAbsolute = function () {

        // variables private to layoutTiles 

        var tiles = [],
            laidTiles = [],
            rowWidth = $('#tileContainer').width(),
            rowHeight = $('.tile').first().outerHeight(true);

        // functions private to layoutTiles 

        // animated tile placement
        // t, an array of tiles
        var placeTiles = function (t) {
            for (var i in t) {
                t[i].css('position', 'absolute');
                t[i].animate({
                    left: t[i].newx,
                    top: t[i].newy,
                    opacity: 1
                }, 750
                );
            };
        };

        /**
         *   rowWidth    : width of layout row
         *   t           : array of tiles, ordered by data-priority
         * 
         *   returns
         *       an array of tiles laid out for display 
         *       each tile will be extended with newx and newy layout values
         *
         ****/
        var calculateTilePositions = function (rowWidth, rowHeight, t) {

            var y = 0,              // tracks current row top; ie the absolute y value of a tile
                laidTiles = [];     // fn return

            for (var j = 0; j < t.length; j++) {

                var accumulatedWidth = rowWidth;
                var x = 0;          // tracks current tile left position; ie the absolute x value of a tile

                for (var i = 0; i < tiles.length; i++) {
                    if (!t[i].placed && tiles[i].width() <= accumulatedWidth) {
                        laidTiles.push(t[i]);
                        t[i].placed = true;
                        accumulatedWidth -= t[i].width();
                        t[i].newx = x;
                        t[i].newy = y;
                        x += t[i].outerWidth(true);
                    }
                }
                y += rowHeight;

                // break loop once all tiles have been placed
                if (laidTiles.length == t.length) {
                    // set the tile contianer to "contain" the positioned tiles
                    // or at least make sure following content (ie a footer) is in the right place
                    $('#tileContainer').css('min-height', y);
                    break;
                }
            }

            return laidTiles;

        };

        /* executuion body */
        tiles = fetchTiles();
        laidTiles = calculateTilePositions(rowWidth, rowHeight, tiles);
        placeTiles(laidTiles);

    }; // --layoutTiles()

    // check the container width and select the appropriate layout algorithm
    var layoutTiles = function () {

        if ($('#tileContainer').width() > 505) {
            layoutAbsolute();
        }
        else {
            layoutAsFloat();
        }

        // signal redraw completed
        redrawRequested = false;
        // track current layout width
        lastContainerWidth = $('#tileContainer').width();
    }

    // public methods in layoutManager 
    var requestRedraw = function (delay) {

        // guard, don't repeat layout work if the container size hasn't changed
        if (lastContainerWidth == $('#tileContainer').width()) return;

        if (redrawRequested == false) {
            redrawRequested = true;
            setTimeout(layoutTiles, delay);
        }

    }

    /* public interface */
    return {
        requestRedraw: requestRedraw
    };

})();


// document ready
$(
    function () {
        // request the tiles be laid out
        layoutManager.requestRedraw(0);
        // and set up a handler for subsequent browser resize events
        $(window).resize(
            function () {
                layoutManager.requestRedraw(450);
            });
    }
);




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
/****
* MODULE            [>] gam.app.include.js
*                   [ ] 
* -----------------------------------------------------------------------------------------------
* NOTES
*
*                       MANUALLY INCLUDE FILES USING MINDSCAPE
* 
****/
