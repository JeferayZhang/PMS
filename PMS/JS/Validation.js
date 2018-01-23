
////作用 : 页面ajax提交数据前对文本框数据进行必要效验
////verifyDiv : 文本框集合所在的divid

function verifyBeforeAjax(verifyDiv) {
    var flag = true;
    $('#' + verifyDiv + ' :text[data-validation="true"]').each(function () {
        if (flag === false) return flag;
        var $this = $(this),
               verify = $this.attr('data-verify').split('|'),
               textValue = $this.val(),
               verifyMsg = $this.attr('data-verify-msg').split('|');
        for (var item in verify) {
            switch (verify[item]) {
                //必填项
                case 'required': {
                    if ($.trim(textValue) === '' ? commonWarning($this, verifyMsg[item]) : true)
                        break;
                    else {
                        flag = false;
                        return;
                    }
                }
                    //同时效验手机和电话
                case 'phone': {
                    if ($.trim(textValue) === '' ? true : (!/((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)/.test(textValue) ? commonWarning($this, verifyMsg[item]) : true))
                        break;
                    else {
                        flag = false;
                        return;
                    }

                }
                    //邮箱
                case 'email': {
                    if ($.trim(textValue) === '' ? true : (!/^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/.test(textValue) ? commonWarning($this, verifyMsg[item]) : true))
                        break;
                    else {
                        flag = false;
                        return;
                    }
                }
                    //传真
                case 'fax': {
                    if ($.trim(textValue) === '' ? true : (!/^[+]{0,1}(\d){1,3}[ ]?([-]?((\d)|[ ]){1,12})+$/.test(textValue) ? commonWarning($this, verifyMsg[item]) : true))
                        break;
                    else {
                        flag = false;
                        return;
                    }
                }
                    //数字
                case 'number': {
                    if ($.trim(textValue) === '' ? true : (!/^[0-9]*$/.test(textValue) ? commonWarning($this, verifyMsg[item]) : true))
                        break;
                    else {
                        flag = false;
                        return;
                    }
                }
                    //正负浮点数
                case 'float': {
                    if ($.trim(textValue) === '' ? true : (!/(^[1-9]\d*\.\d*|0\.\d*[1-9]\d*$)|(^-[1-9]\d*\.\d*|-0\.\d*[1-9]\d*$)/.test(textValue) ? commonWarning($this, verifyMsg[item]) : true))
                        break;
                    else {
                        flag = false;
                        return;
                    }
                }
            }
        }
    });
    return flag;
}

function commonWarning($this, msg) {
    $this.css('border-color', '#ff0000');
    $.mvcAlertWarning(msg);
    return false;
}