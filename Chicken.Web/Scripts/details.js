var detailsModule = (function () {
    var initLightbox = function() {
        $(document).delegate('*[data-toggle="lightbox"]', 'click', function(event) {
            event.preventDefault();
            $(this).ekkoLightbox({
                loadingMessage: "Загрузка..."
            });
        });
    };

    return {
        init: function () {
            var navigationHandler = new NavigationHandler({
                blocks: [$('.story'), $('.photos'), $('.comments')],
                bodyPadding: $('nav').height(),
                ul: $('.menu')
            });
            navigationHandler.init();
            mainModule.setPadding();
            initLightbox();
        }
    };
}());

function NavigationHandler(options) {
    var autoScroll = false;
    var scrollMaxValue = $(document).height() - $(window).height();
    var blocks = options.blocks;
    var links = options.ul.find('li>a');
    var bodyPadding = options.bodyPadding;

    var setActiveLink = function (link) {
        var activeClassName = 'active';
        links.each(function () { $(this).removeClass(activeClassName); });
        link.addClass(activeClassName);
    };

    var handleScroll = function (currentScrollPosition) {
        if (autoScroll) {
            return;
        }
        for (var i = 0; i < blocks.length; i++) {
            var el = blocks[i];
            var nextEl = i != blocks.length - 1 ? blocks[i + 1] : null;

            var elStartPosition = el.position().top - bodyPadding;
            var elEndPosition = nextEl ? nextEl.position().top - bodyPadding : scrollMaxValue;

            if (currentScrollPosition >= elStartPosition && currentScrollPosition < elEndPosition) {
                var linkEl = links.eq(i);
                setActiveLink(linkEl);
            }
        }
    };

    var handleClick = function (link) {
        autoScroll = true;
        setActiveLink(link);
        var linkIndex = links.index(link);
        var position = blocks[linkIndex].position();
        $('body, html').animate({ scrollTop: position.top - bodyPadding }, 'slow', 'swing',
        function () {
            autoScroll = false;
        });
    };

    this.init = function () {

        if ($(document).scrollTop() == 0) {
            setActiveLink(links.eq(0));
        }

        $(document).scroll(function () {
            handleScroll($(document).scrollTop());
        });

        links.click(function () {
            if ($(".navbar-toggle").is(':visible')) {
                $(".navbar-toggle").click();
            }
            handleClick($(this));
        });
    };
};
