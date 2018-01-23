//-------------------页面JS公共变量--------------------------//
var mvcchildpageindex; //记录当前打开的弹出层的index

//----------------------------------------------------------//

$(function () {
    if (parent.length == 0)
        location.href = "/Home/Index";
    else
        mvcchildpageindex = parent.layer.getFrameIndex(window.name)
});

function mvccloseall() {
    parent.layer.close(mvcchildpageindex);
}