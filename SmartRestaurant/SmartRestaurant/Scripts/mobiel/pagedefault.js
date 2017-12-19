function warn(msg, opt, left, top) {
    if (opt) {
        var obj = $("#" + opt);
    }
    new Toast({ context: $('body'), message: msg }, obj, left, top).show();

}
var Toast = function (config, obj, left, top) {
    this.context = config.context == null ? $('body') : config.context;//上下文
    this.message = config.message;//显示内容
    this.time = config.time == null || typeof config.time != "number" ? 3000 : config.time;//持续时间
    this.left = config.left;//距容器左边的距离
    this.top = (screen.availHeight / 8);//距容器上方的距离
    if (obj) {
        this.left = obj.offset().left + left;
        this.top = obj.offset().top + top;
    }
    this.init();
}
var msgEntity;
Toast.prototype = {
    //初始化显示的位置内容等
    init: function () {
        $("#toastMessage").remove();
        //设置消息体
        var msgDIV = new Array();
        msgDIV.push('<div id="toastMessage">');
        msgDIV.push('<span>' + this.message + '</span>');
        msgDIV.push('</div>');
        msgEntity = $(msgDIV.join('')).appendTo(this.context);
        //设置消息样式
        var left = this.left == null ? this.context.width() / 2 - msgEntity.find('span').width() / 2 : this.left;
        var top = this.top == null ? '20px' : this.top;
        msgEntity.css({ position: 'absolute', top: top, 'z-index': '1002', left: left, 'background-color': 'black', color: 'white', 'font-size': '20px', padding: '5px', margin: '5px', 'border-radius': '4px', '-moz-border-radius': '4px', '-webkit-border-radius': '4px', opacity: '0.5', 'font-family': '微软雅黑' });
        //msgEntity.addClass(".toast");
        msgEntity.hide();
    },
    //显示动画
    show: function () {
        msgEntity.fadeIn(this.time / 2);
        msgEntity.fadeOut(this.time / 2);
    }

}