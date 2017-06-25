/****
* Plugin            : jquery.gammenu.js
*                   : Add a dynamic menu to a control.
* -----------------------------------------------------------------------------------------------
* Usage             : HTML:  menu tab has  id=<x>MenuTab
*                   :        menu has id=<x>Menu
*                   :        menu close control has id=<x>MenuClose
*                   : 
*                   : 
*                   : 
*                   : 
* -----------------------------------------------------------------------------------------------
****/
(function($) {
    function GAMMenu(menuTab$)
    {


        this.menuTab$ = menuTab$;
        // Get the menu associated with this menu tab - convention is the menu has the same id without a "Tab" suffix.
        var menuTabId = menuTab$.attr('id');

        this.menu$ = $("#" + menuTabId.replace('Tab', ''));
        var menuId = this.menu$.attr('id');

        this.menuClose$ = $("#" + menuId + "Close");

    }
    
    GAMMenu.prototype = {
        toggleMenu: function () {


            this.menu$.slideToggle();
            this.menuTab$.toggleClass('menuSelected');
        },

        closeMenu: function () {

            if (this.menuTab$.hasClass('menuSelected'))
            {
                this.menu$.slideToggle();
                this.menuTab$.toggleClass('menuSelected');
            }
    }
};

    $.fn.extend({
        gammenu: function() {
            return this.each(function () {

                var menuTab = $(this);
                var gammenu = new GAMMenu(menuTab);

                gammenu.menuTab$.click(function () {
                    gammenu.toggleMenu();
                    // Stop event propagation.
                    return false;
                });

                gammenu.menu$.click(function () {
                    gammenu.closeMenu();
                    // allow link click to proceed.
                });
                
                gammenu.menuClose$.click(function () {
                    gammenu.closeMenu();
                    // Stop event propagation.
                    return false;
                });

            });
        }
    });
})(jQuery);