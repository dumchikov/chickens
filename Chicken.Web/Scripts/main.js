var mainModule = (function () {

    var setPadding = function () {
        var bodyPadding = $('nav').height();
        $('body').css('padding-top', bodyPadding);
    };

    var initPagination = function (el, total, page, group) {
        el.bootpag({
            total: total,
            page: page,
            maxVisible: 6,
            href: '/' + group + "/{{number}}",
            leaps: false,
            firstLastUse: true,
            next: '&raquo;',
            prev: '&laquo;'
        });
    };

    var initMasonry = function() {
        var container = document.querySelector('.content');
        imagesLoaded(container, function() {
            var msnry = new Masonry(container, {
                columnWidth: '.item',
                itemSelector: '.item'
            });
        });
    };
    
    return {
        setPadding: setPadding,
        initPagination: initPagination,
        initMasonry: initMasonry,
        init: function () {
            setPadding();
            initMasonry();
            $('#suggest-post-link').click(function () {
                $('#new-post').slideToggle();
            });
        }
    };
}());