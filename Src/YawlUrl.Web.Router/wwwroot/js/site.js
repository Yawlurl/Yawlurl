// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var yawl = yawl || {};


yawl.router = (function () {

    var init = function () {
        $(".btnCopy").click(function () {
            alert("Clicked Copy");
        });
    };

    return {
        init: init
    };
})();

yawl.router.default = (function () {

    var validate = function () {
        if (validURL($("#txtLongUrl").val())) {
            return true;
        }

        alert("Please write valid url");
        $("#txtLongUrl").focus();
        return false;
    };

    var clickShorten = function () {
        var isValid = validate();
        if (isValid) {
            $.post("/generate", { LongUrl: $("#txtLongUrl").val() }, function (data) {
                alert("Generated link >" + data);
            });

            var settings = {
                "async": true,
                "crossDomain": true,
                "url": "/generate",
                "method": "POST",
                "headers": {
                    "Content-Type": "application/json",
                    "cache-control": "no-cache"
                },
                "processData": false,
                "data": JSON.stringify({"LongUrl": $("#txtLongUrl").val()})
            };

            $.ajax(settings).done(function (response) {
                console.log(response);
                $("#shortenLink").attr("href", response);
                $("#shortenLink").text(response);
                $("#resultBar").toggleClass("invisible", false);
                $("#resultBar").toggleClass("visible", true);
            });



        }
    };

    var validURL = function (myURL) {
        var pattern = new RegExp('^(https?:\\/\\/)?' + // protocol
            '((([a-z\\d]([a-z\\d-]*[a-z\\d])*)\\.?)+[a-z]{2,}|' + // domain name
            '((\\d{1,3}\\.){3}\\d{1,3}))' + // ip (v4) address
            '(\\:\\d+)?(\\/[-a-z\\d%_.~+]*)*' + //port
            '(\\?[;&amp;a-z\\d%_.~+=-]*)?' + // query string
            '(\\#[-a-z\\d_]*)?$', 'i');
        return pattern.test(myURL);
    };

    return {
        clickShorten: clickShorten
    };
})();
