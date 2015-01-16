$(function () {

    $('body').on('click', '.section-open', function () {
        $(this).toggle();
        $(this).parent().find('.section-close').toggle();
        $(this).closest('.section').children('.section').show();
    });

    $('body').on('click', '.section-close', function () {
        $(this).toggle();
        $(this).parent().find('.section-open').toggle();
        $(this).closest('.section').children('.section').hide();
    });

    $('body').on('click', '.show-all', function (e) {
        e.preventDefault();
        $('.section-hidden').show();
        $('.section-open').hide();
        $('.section-close').show();
    });

    $('body').on('click', '.hide-all', function (e) {
        e.preventDefault();
        $('.section-hidden').hide();
        $('.section-open').show();
        $('.section-close').hide();
    });
});
