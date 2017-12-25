function insert_success(dialog) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_SUCCESS,
        title: '成功 ',
        message: '添加成功！',
        size: BootstrapDialog.SIZE_SMALL,
        buttons: [{
            label: '确定',
            action: function (dialogItself) {
                dialogItself.close();
                dialog.close();
                $('#tb_departments').bootstrapTable('refresh');
            }
        }]
    });
}
function insert_fail(dialog) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '错误 ',
        message: '添加失败,请检查!',
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '关闭',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function insert_success1(dialog) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_SUCCESS,
        title: '成功 ',
        message: '添加成功！',
        size: BootstrapDialog.SIZE_SMALL,
        buttons: [{
            label: '确定',
            action: function (dialogItself) {
                dialogItself.close();
                dialog.close();
            }
        }]
    });
}