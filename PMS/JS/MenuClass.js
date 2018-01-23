var ajaxHelper = new AjaxHelper();


var table = layui.table, thisindex = 0,
    $ = layui.jquery;

//列表数据
ajaxHelper.get("/api/MenuClass/getMenuTypeForAll/", null, function (d) {
    table.render({
        elem: '#pagetable',
        id: "fucktable",
        data: d,
        height: 350,
        width: 965,
        page: true,
        cols: [
            [
                {
                    checkbox: true,
                    fixed: 'left',
                    LAY_CHECKED: false
                }, {
                    type: 'numbers',
                    title: '序号',
                    width: 80
                },
                {
                    field: 'Name',
                    title: '类别名称',
                    width: 150
                },
                {
                    field: 'ParentName',
                    title: '所属分类',
                    width: 150
                },
                {
                    field: 'Remark',
                    title: '备注',
                    width: 150
                }, {
                    field: 'Status',
                    title: '启用',
                    width: 100,
                    templet: '#checkboxTpl',
                    unresize: true
                }, {
                    field: 'Sort',
                    title: '排序',
                    width: 70,
                    sort: true,
                    fixed: 'right'
                }, {
                    fixed: 'right',
                    title: '操作',
                    width: 207,
                    align: 'center',
                    toolbar: '#ToolBar'
                }
            ]
        ],
        skin: 'row',
        even: true,
        page: true,
        limits: [10, 15, 20],
        limit: 10
    });

    //监听工具条
    table.on('tool(demo)', function (obj) {
        var data = obj.data;
        if (obj.event === 'detail') {
            layer.open({
                title: '编辑',
                type: 2,
                maxmin: true,
                skin: 'layui-layer-molv',
                area: ['800px', '400px'],
                shadeClose: true,
                content: "/MenuClass/Edit?ID=" + data.ID
            });
        } else if (obj.event === 'del') {
            layer.confirm('是否删除所选?', function (index) {
                var data = { '': obj.data.ID };
                ajaxHelper.post("/api/MenuClass/delMenuClassByID/", data, function (d) {
                    if (d) {
                        obj.del();
                        layer.close(index);
                        layer.msg('操作成功!');
                    }
                    else {
                        layer.close(index);
                        layer.msg('操作失败!');
                    }
                });

            });
        } else if (obj.event === 'edit') {
            layer.alert('编辑行：<br>' + JSON.stringify(data))
        }
    });
});

var active = {
    create: function () { //获取选中数据
        thisindex = layer.open({
            title: '添加',
            type: 2,
            maxmin: true,
            skin: 'layui-layer-molv',
            area: ['800px', '400px'],
            shadeClose: true,
            content: "/MenuClass/Edit"
        });
    }, delcheck: function () { //获取选中数目
        var checkStatus = table.checkStatus('pagetable')
            , data = checkStatus.data;
        layer.msg('选中了：' + data.length + ' 个');
    }
};

$('#btnGroup .layui-btn').on('click', function () {
    var type = $(this).data('type');
    active[type] ? active[type].call(this) : '';
});