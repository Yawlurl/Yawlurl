var yawl = yawl || {};

yawl.default = (function () {

    let isValid = function () {
        let longUrl = $("#txtLongUrl").val();
        let validateError = validate({ website: longUrl }, { website: { url: { allowLocal: true } } })
        if (validateError) {
            $("#shortPanel .alert-danger").removeClass(" d-none");
            $("#shortPanel .alert-danger").text(validateError.website[0]);
            $("#txtLongUrl").focus();
            return false;
        }
        else {
            $("#shortPanel .alert-danger").addClass(" d-none");
            return true;
        }
    };

    let clickShorten = function () {
        $("#resultBar").fadeOut(100).addClass("d-none");

        if (!isValid()) {
            return;
        }

        $("#shortPanel .dimmer").addClass(" active");
       
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
            "data": JSON.stringify({ "LongUrl": $("#txtLongUrl").val() })
        };


        $.ajax(settings).done(function (response) {
            const shortUrl = response.url;
            $("#shortenLink").attr("href", shortUrl);
            $("#shortenLink").text(shortUrl);
            $("#resultBar").fadeIn(100).removeClass("d-none");

            $("#shortPanel .dimmer").removeClass(" active");
        });
    };
    return {
        clickShorten: clickShorten,
    };
})();

yawl.router = (function () {

    let init = function () {
        $(document).on("click", ".button-copy", function () {
            $(".button-copy").button('toggle');
            copyToClipboard($("#shortenLink").text());
            $(".button-copy").button('toggle');
        });

        $(document).on("click", "#button-shorten", function () {
            yawl.default.clickShorten();
        });

        document.addEventListener('paste', function (event) {
            let longUrl = event.clipboardData.getData('Text');

            $("#txtLongUrl").val(longUrl)

        });
    };

    let copyToClipboard = function (text) {
        let $tempInput = $("<textarea>");
        $("body").append($tempInput);
        $tempInput.val(text).select();
        document.execCommand("copy");
        $tempInput.remove();
    };

    return {
        init: init
    };
})();







