layui.use('element', function () {
    var $ = layui.jquery,
        element = layui.element; //Tab的切换功能，切换事件监听等，需要依赖element模块

    $('.tabItem').on('click', function () {
        var othis = $(this),
            title = othis.html(),
            id = othis.data('id').toString(),
            url = othis.data('href');

        var obj = $(".layui-tab-title").children("li");
        //为true则新增,否则切换选项卡
        var flag = true;
        for (var i = 0; i < obj.length; i++) {
            var newid = $(obj[i]).attr("lay-id").toString();
            if (newid == id) {
                flag = false;
                break;
            }
        }
        if (flag) {
            //新增一个选项卡
            element.tabAdd('demo', {
                title: title,
                content: "<iframe src='" + url +
                "' frameborder='0' scrolling='no'></iframe>",
                id: id
            });
        }
        //切换到已存在的选项卡
        element.tabChange('demo', id);
    })
});