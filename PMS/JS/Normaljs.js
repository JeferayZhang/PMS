
layui.use(['element', 'layer', 'form', 'laydate'], function () {
    var $ = layui.jquery;
    var form = layui.form;
    var laydate = layui.laydate;
    var layer = layui.layer; //按需加载

    //全选
    form.on('checkbox(allChoose)', function (data) {
        var child = $(data.elem).parents('table').find('tbody input[type="checkbox"]');
        child.each(function (index, item) {
            item.checked = data.elem.checked;
        });
        form.render('checkbox');
    });

    form.on('checkbox(table_ck)', function (data) {
        $(data.elem).parents('table').find('thead input[type="checkbox"]').each(function (index, item) {
            item.checked = $(data.elem).parents('table').find('tbody input[type="checkbox"]').length == $(data.elem).parents('table').find('tbody input[type="checkbox"]:checked').length ? true : false;
        });
        form.render('checkbox');
    });
});


function openUrlByJumpOutPage_NoScroll(src, width, height, title) {
    layer.open({
        shade: [0.1, "#fff", true],
        anim: 2,
        title: [title, 'backgroup:#252325;color:#7fd7c5;'],
        maxmin: false,
        type: 2,
        area: [width, height],
        content: [src, 'no']
    });
}

function openUrlByJumpOutPage_Scroll(src, width, height, title) {
    layer.open({
        shade: [0.1, "#fff", true],
        anim: 2,
        title: [title, 'backgroup:#252325;color:#7fd7c5;'],
        maxmin: false,
        type: 2,
        area: [width, height],
        content: [src]
    });
}

jQuery.ajJson = function (url, sendindata, successfn, errorfn) {
    showLoading();
    $.ajax({
        type: "post",
        data: sendindata,
        url: url,
        dataType: "json",
        cache: false,
        async: false,
        success: function (data) {
            hideLoading();
            successfn(data);
        },
        error: function (e) {
            hideLoading();
            $.mvcAlertWarning("请求超时，请重试！", function () {
                if (errorfn != undefined && errorfn != null)
                    errorfn();
            });
        }
    });
};

jQuery.ajJson_withOutUrl = function (sendindata, successfn, errorfn) {
    showLoading();
    $.ajax({
        type: "post",
        data: sendindata,
        dataType: "json",
        cache: false,
        async: true,
        success: function (data) {
            hideLoading();
            successfn(data);
        },
        error: function (e) {
            hideLoading();
            $.mvcAlertWarning("请求超时，请重试！", function () {
                if (errorfn != undefined && errorfn != null)
                    errorfn();
            });
        }
    });
};

function listDelBatch(tableid, action, msg1, msg2) {
    if (msg1 == undefined)
        msg1 = "是否确认删除？";
    if (msg2 == undefined)
        msg2 = "删除成功！";
    if ($("#" + tableid).find("[name=table_ck]:checked").length == 0) {
        $.mvcAlertWarning("请勾选需要进行操作的数据！");
        return;
    }
    var delkeys = "";
    $("#" + tableid).find("[name=table_ck]:checked").each(function () {
        delkeys += $(this).val() + ",";
    });
    delkeys = delkeys.substring(0, delkeys.length - 1);
    $.mvcConfirmTwoBtn(msg1,
      "确定",
      function () {
          $.ajJson_withOutUrl({ "action_post": action, "delkeys_post": delkeys }, function (data) {
              if (data["RESULT"] == "true") {
                  $.mvcAlertSuccess(msg2, function () { location.reload(); });
              }
              else {
                  $.mvcAlertWarning(data["REASON"]);
              }
          });
      },
      "取消");
}

jQuery.mvcConfirmTwoBtn = function (text, btn1text, btn1fn, btn2text, btn2fn) {
    layer.confirm(text, {
        btn: [btn1text, btn2text],
        btn1: function (index) {
            layer.close(index);
            if (btn1fn != undefined && btn1fn != null)
                btn1fn();
        },
        btn2: function (index) {
            layer.close(index);
            if (btn2fn != undefined && btn2fn != null)
                btn2fn();
        }
    })
}


//弹出自定义消息
//text:消息的内容
//type: 1表示成功，2表示失败
//time: 数字，单位毫秒，默认750毫秒
//endfn：消息关闭时执行的事件
jQuery.mvcAlertSpecial = function (text, type, time, endfn) {
    var endtime = 750;
    if (time != undefined && time != null)
        endtime = time;
    layer.msg(text, {
        icon: type,
        shade: [0.0001, "#000", true],
        time: endtime,
        end: function () {
            if (endfn != undefined && endfn != null)
                endfn();
        }
    });
}

jQuery.mvcAlertSuccess = function (text, endfn) {
    layer.msg(text, {
        icon: 1,
        shade: [0.0001, "#000", true],
        time: 750,
        end: function () {
            if (endfn != undefined && endfn != null)
                endfn();
        }
    });
}



jQuery.mvcAlertWarning = function (text, endfn) {
    layer.alert(text, {
        icon: 2,
        shade: [0.0001, "#000", true],
        closeBtn: 0,
        end: function () {
            if (endfn != undefined && endfn != null)
                endfn();
        }
    });
}


var loadingIndex;

function showLoading() {
    var Index = layer.load(2);
    loadingIndex = Index;
}

function hideLoading() {
    layer.close(loadingIndex);
}