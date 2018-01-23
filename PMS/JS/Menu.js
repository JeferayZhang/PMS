var ajaxHelper = new AjaxHelper();

$(function () {
    layui.use(['table', 'layer'], function () {
        var table = layui.table, layer = layui.layer,
            $ = layui.jquery;

        delcallback = function (d) {

            table.render({
                elem: '#pagetable',
                data: d,
                height: 400,
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
                            field: 'MenuName',
                            title: '页面名称',
                            width: 150
                        }, {
                            field: 'ParentName',
                            title: '所属分类',
                            width: 150
                        }, {
                            field: 'Remark',
                            title: '说明',
                            width: 150
                        }, {
                            field: 'Url',
                            title: '页面路径',
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
                            width: 80,
                            sort: true,
                            fixed: 'right'
                        }, {
                            fixed: 'right',
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
                        title: '页面编辑',
                        type: 2,
                        maxmin: true,
                        zIndex: layer.zIndex,
                        skin: 'layui-layer-molv',
                        area: ['900px', '400px'],
                        shadeClose: true,
                        content: "/Menu/Edit"
                    });
                    //layer.msg('ID：' + data.ID + ' 的查看操作');
                } else if (obj.event === 'del') {
                    layer.confirm('真的删除行么', function (index) {
                        obj.del();
                        layer.close(index);
                    });
                } else if (obj.event === 'edit') {
                    layer.alert('编辑行：<br>' + JSON.stringify(data))
                }
            });
        }
        ajaxHelper.get("/api/Menu/getPages/", null, delcallback);

        // $('.demoTable .layui-btn').on('click', function () {
        //     var type = $(this).data('type');
        //     active[type] ? active[type].call(this) : '';
        // });
    });
})