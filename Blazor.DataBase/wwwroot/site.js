window.cecblazor_setEditorExitCheck = function (show) {
    if (show) {
        window.addEventListener("beforeunload", cecblazor_showExitDialog);
    }
    else {
        window.removeEventListener("beforeunload", cecblazor_showExitDialog);
    }
}

window.cecblazor_showExitDialog = function (event) {
    event.preventDefault();
    event.returnValue = "There are unsaved changes on this page.  Do you want to leave?";
}

window.blazor_setMouseUpEvent = function (show) {
    if (show) {
        window.addEventListener("mouseup", CallBlazorMouseUp);
    }
    else {
        window.removeEventListener("mouseup", CallBlazorMouseUp);
    }
}

function CallBlazorMouseUp(event) {
    DotNet.invokeMethodAsync("Blazor.Database.Server", "ButtonMouseUpOut");
}

windows.cecblazor_disableBack = function () {
    window.history.forward();
}

windows.cecblazor_NoBack = function () {
    setTimeout("cecblazor_disableBack()", 0);
}
