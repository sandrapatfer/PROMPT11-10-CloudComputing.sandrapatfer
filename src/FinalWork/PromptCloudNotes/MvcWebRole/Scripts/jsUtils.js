(function (window) {

    function JsUtils() { }

    JsUtils.prototype.Action = function (method, action, data) {
        $.ajax({
            url: action,
            type: method,
            dataType: 'json',
            data: data,
            success: function (result) {
                if (result.redirect) {
                    window.location.replace(result.redirect);
                }
                else {
                    window.location.reload();
                }
            },
            statusCode: {
                302: function (data) {
                    window.location.replace(data.redirect);
                }
            },
            error: function () {
                alert("error in action: " + action);
            }
        });
    }

    window.jsUtils = new JsUtils;
})(window);