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


