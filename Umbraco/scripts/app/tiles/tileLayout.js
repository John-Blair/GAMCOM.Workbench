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



