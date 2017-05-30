// Write your Javascript code.
"use strict";

function ShowTip_Error(message) {
    ShowTips(message, "error");
}
function ShowTip_Success(message) {
    ShowTips(message, "success");
}
//tipType:info,success,warning,error
function ShowTips(message, tipType) {
    alert(message);
}

function PostApi(btnDocElement,url, data, successFun) {
    var btn = $(btnDocElement);

    btn.attr("disabled", "disabled");

    $.post(url, data)
        .success(successFun)
        .error(function() {
            ShowTip_Error("请检查您当前网络是否正常！");
        })
        .complete(function() {
            btn.removeAttr("disabled");
        });
}