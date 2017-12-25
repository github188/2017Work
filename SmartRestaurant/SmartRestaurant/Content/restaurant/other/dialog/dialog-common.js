function ShowDialog(title, url, success) {//ajax加载dialog
    BootstrapDialog.show({
        title: title,
        type: BootstrapDialog.TYPE_DEFAULT,
        //size: BootstrapDialog.SIZE_WIDE,
        cssClass: "fade",
        closeable: true,
        message: function (dialog) {
            var $message = $('<div></div>');
            var pageToLoad = dialog.getData('pageToLoad');
            $message.load(pageToLoad);
            return $message;
        },
        data: {
            'pageToLoad': url,
        },
        buttons: [{
            label: '取消',
            action: function (dialog) {
                dialog.close();
            }
        }, {
            label: '确定',
            cssClass: 'btn btn-primary',
            action: function (dialog) {
                success(dialog);
            }
        }]
    });
}
function error(dialog) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '错误 ',
        message: '未知异常导致请求失败,请重试!',
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '关闭',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }]
    });
}
function danger_dialog(msg) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_DANGER,
        title: '提示 ',
        message: msg,
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '关闭',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }],
    });
}
function success_dialog(msg) {
    BootstrapDialog.show({
        type: BootstrapDialog.TYPE_SUCCESS,
        title: '提示 ',
        message: msg,
        size: BootstrapDialog.SIZE_SMALL,//size为小，默认的对话框比较宽
        buttons: [{// 设置关闭按钮
            label: '关闭',
            action: function (dialogItself) {
                dialogItself.close();
            }
        }],
    });
}