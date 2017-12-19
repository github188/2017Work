function delete_success() {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_SUCCESS,
        title: '成功 ',
        message: '删除成功！',
        size: BootstrapDialog.SIZE_SMALL,
        buttons: [{
            label: '确定',
            action: function (dialogItself) {
                dialogItself.close();
                $('#tb_departments').bootstrapTable('refresh');
            }
        }]
    });
}
function delete_fail() {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '错误 ',
        message: '删除失败',
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '关闭',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function ShowDialog_delete(title, msg, url, data) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title:title,
        message: msg,
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '取消',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }, {
            label: '删除',
            cssClass: 'btn-danger',
            action: function (dialogItself) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    dataType: "json",
                    success: function (result) {
                        if (result.result == "success") {
                            delete_success();
                        }
                        else {
                            delete_fail();
                        }
                    },
                    error: function () {
                        error();
                    }
                });
                dialogItself.close();
            }
        }]
    });
}