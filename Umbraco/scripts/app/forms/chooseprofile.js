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





