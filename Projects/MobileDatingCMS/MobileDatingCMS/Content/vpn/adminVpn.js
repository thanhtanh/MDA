
//Use for active menu sidebar that opened
function menuActive(href) {
    var liActive = $('a[href$="' + href + '"]').parent();
    var liChild = $(liActive).parent();
    var liParent = $(liChild).parent();
    liParent.addClass("open");
    liChild.show();
    liActive.addClass("active");
}
$(document).ready(function () {
    menuActive(window.location.pathname.split('?')[0]);
});