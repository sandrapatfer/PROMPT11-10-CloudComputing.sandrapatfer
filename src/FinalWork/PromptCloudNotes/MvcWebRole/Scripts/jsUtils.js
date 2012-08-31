(function (window) {

    function JsUtils() { }

    JsUtils.prototype.Action = function (method, action, data, cbk) {
        $.ajax({
            url: action,
            type: method,
            dataType: 'json',
            data: data,
            success: function (result) {
                if (result != null) {
                    if (result.redirect) {
                        window.location.replace(result.redirect);
                    }
                    else if (result.reload) {
                        window.location.reload();
                    }
                }
                if (cbk) {
                    cbk(result);
                }
            },
            statusCode: {
                302: function (data) {
                    window.location.replace(data.redirect);
                },
                404: function (data) {
                    alert("Resource not found");
                    window.location.reload();
                }
            },
            error: function () {
                alert("error in action: " + action);
            }
        });
    }

    window.jsUtils = new JsUtils;

})(window);

