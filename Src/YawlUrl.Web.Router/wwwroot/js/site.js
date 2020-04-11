var yawl = yawl || {};

yawl.router = (function () {

    var init = function () {
        $(".button-copy").click(function () {
            $(".button-copy").button('toggle');
            copyToClipboard($("#shortenLink").text());
            $(".button-copy").button('toggle');
        });
    };

    var copyToClipboard = function (text) {
        var $tempInput = $("<textarea>");
        $("body").append($tempInput);
        $tempInput.val(text).select();
        document.execCommand("copy");
        $tempInput.remove();
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

            $("#button-shorten").prop('disabled', true);
            $("#resultBar").fadeOut(100).addClass("d-none");

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

            $("#button-shorten .spinner-border").fadeIn(100).removeClass(" d-none");
            $("#button-shorten .shorten-button-text").text("Working");
            $.ajax(settings).done(function (response) {
                const shortUrl = response.url;
                $("#shortenLink").attr("href", shortUrl);
                $("#shortenLink").text(shortUrl);
                $("#resultBar").fadeIn(100).removeClass("d-none");

                $("#button-shorten .spinner-border").fadeOut(100).addClass(" d-none");
                $("#button-shorten .shorten-button-text").text("Shorten");
                $("#button-shorten").prop('disabled', false);
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
