

var setupToggles = function () {
    
    $('#hideworkspaceKV').click(function () {
        $('.workspaceKV').toggle();

    });

    $('#hideworkspaceContext').click(function () {
        $('.workspaceContext').toggle();

    });

    $('.toggleTables').click(function () {
        $('.workspaceKV').toggle();
        $('.workspaceContext').toggle();
    });

    $('#toggleTables').click(function () {
        $('.workspaceKV').toggle();
        $('.workspaceContext').toggle();
    });

};

$(document).ready(setupToggles);

