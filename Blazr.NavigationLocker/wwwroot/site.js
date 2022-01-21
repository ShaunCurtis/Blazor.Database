let blazr_PreventNavigation = false;  // main control bool to stop/allow navigation
let idCounter = 0;
let lastHistoryItemId = 0; //needed to detect back/forward, in popstate event handler

window.blazr_setPageLock = function (lock) {
    blazr_PreventNavigation = lock;
}

window.history.pushState = (function (basePushState) {
    return function (...args) {
        if (blazr_PreventNavigation) {
            return;
        }
        basePushState.apply(this, args);
    }
})(window.history.pushState);

window.addEventListener('beforeunload', e => {
    if (blazr_PreventNavigation) {
        e.returnValue = 'There are unsaved changes'
    }
});

window.addEventListener('load', () => {
    let preventingPopState = false;
    function popStateListener(e) {
        if (blazr_PreventNavigation) {
            let shouldStay;
            //popstate can be triggered twice, but we want to show confirm dialog only once
            if (!preventingPopState) {
                preventingPopState = true;
                shouldStay = blazr_PreventNavigation;
                // shouldStay = confirm('There are unsaved changes. Would you like to stay?');
            }
            if (preventingPopState || shouldStay) {
                //this will cancel Blazor navigation, but the url is already changed
                e.stopImmediatePropagation();
                e.preventDefault();
                e.returnValue = false;
            }
            if (shouldStay) {
                //detect back vs forward 
                const currentId = e.state && e.state.__incrementalId;
                const navigatingForward = currentId > lastHistoryItemId;

                //revert url
                if (navigatingForward) {
                    history.back();
                }
                else {
                    history.forward();
                }
                setTimeout(() => preventingPopState = false, 50); //avoid showing another confirm dialog when reverting
            }
        }
    }

    window.addEventListener('popstate', popStateListener, { capture: true });

    window.history.pushState = (function (basePushState) {
        return function (...args) {
            //if (blazr_PreventNavigation && confirm('There are unsaved changes. Would you like to stay?')) {
            if (blazr_PreventNavigation) {
                return;
            }
            if (!args[0]) {
                args[0] = {};
            }
            lastHistoryItemId = history.state && history.state.__incrementalId;
            args[0].__incrementalId = ++idCounter; //track order of history items            
            basePushState.apply(this, args);
        }
    })(window.history.pushState);
});
