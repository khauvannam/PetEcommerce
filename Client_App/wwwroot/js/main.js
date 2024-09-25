'use strict';

window.scrollToElement = function (element) {
    if (element) {
        element.scrollIntoView({behavior: 'smooth', block: 'start'});
    }
};

