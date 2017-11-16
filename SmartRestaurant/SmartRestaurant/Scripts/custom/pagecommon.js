var spinner = new Spinner({
    lines: 13, // loading小块的数量
    length: 7, // 小块的长度
    width: 4, // 小块的宽度
    radius: 10, // 整个圆形的半径
    corners: 1, // 小块的圆角，越大则越圆
    rotate: 0, // loading动画的旋转度数，貌似没什么实际作用
    color: '#000', // 颜色
    speed: 1, // 变换速度
    trail: 60, // 余晖的百分比
    shadow: false, // 是否渲染出阴影
    hwaccel: false, // 是否启用硬件加速
    className: 'spinner', // 给loading添加的css样式名
    zIndex: 2e9, // The z-index (defaults to 2000000000)
    top: 'auto', // Top position relative to parent in px
    left: 'auto' // Left position relative to parent in px
});

String.prototype.isNullOrEmpty = function () {
    if (this == null) return true;
    if (this.length == 0) return true;
    return false;
}

function ajax(option) {
    var defaultop = {
        async: true,
        type: 'GET',
        data: {},
        url: '',
        dataType: 'html',
        success: null,
        error: null,
        complete: null,
        timeout: 5000
    }

    $.extend(defaultop, option);
    lodingUI();
    console.log(defaultop.url);
    $.ajax({
        async: defaultop.async,
        type: defaultop.type,
        url: defaultop.url,
        data:defaultop.data,
        timeout: defaultop.timeout,
        success: function (res) {
            if (defaultop.success != null) {
                defaultop.success(res)
            }
        },
        error: function (res) {
            alert(res);
            if (defaultop.error != null) {
                defaultop.error(res)
            }
        },
        complete: function (res) {
            removeLoading();
            if (defaultop.complete != null) {
                defaultop.complete(res)
            }
        }
    })


}
function lodingUI() {
    //var opts = {
    //    lines: 13, // loading小块的数量
    //    length: 7, // 小块的长度
    //    width: 4, // 小块的宽度
    //    radius: 10, // 整个圆形的半径
    //    corners: 1, // 小块的圆角，越大则越圆
    //    rotate: 0, // loading动画的旋转度数，貌似没什么实际作用
    //    color: '#000', // 颜色
    //    speed: 1, // 变换速度
    //    trail: 60, // 余晖的百分比
    //    shadow: false, // 是否渲染出阴影
    //    hwaccel: false, // 是否启用硬件加速
    //    className: 'spinner', // 给loading添加的css样式名
    //    zIndex: 2e9, // The z-index (defaults to 2000000000)
    //    top: 'auto', // Top position relative to parent in px
    //    left: 'auto' // Left position relative to parent in px
    //};

    //option = $.extend({}, opts, option);
    //var spinner = new Spinner(opts);
    //$('body').addClass("modal-open");

    $('#loading').remove();

    $('<div id="loding" class="logdingUI"></div>').appendTo('body')

    spinner.spin($('#loding')[0]);
}

function removeLoading() {
    spinner.stop();
    $('#loading').remove();
}



(function (factory) {
    if (typeof define === 'function' && define.amd) {
        // AMD. Register as anonymous module.
        define(['jquery'], factory);
    } else if (typeof exports === 'object') {
        // Node / CommonJS
        factory(require('jquery'));
    } else {
        // Browser globals.
        factory(jQuery);
    }
})
        (function ($) {
            'use strict';

            function isNumber(n) {
                return typeof n === 'number';
            }

            function toArray(obj, offset) {
                var args = [];

                if (isNumber(offset)) { // It's necessary for IE8
                    args.push(offset);
                }

                return args.slice.apply(obj, args);
            }

            function isUndefined(n) {
                return typeof n === 'undefined';
            }

            function BiaoGe(element, options) {
                this.$table = element,
                this.options = $.extend({}, BiaoGe.DEFAULTS, options),
                this.page_size = 10,
                this.page_num = 1,
                this.loadingTrHtml = '<tr><td colspan="100" class="loading100"><i class="fa fa-spinner fa-spin fa-2x"></i>&nbsp;<span>正在加载，请稍后...</span></td></tr>';
                this.emptyTrHtml = '<tr><td colspan="100" class="loading100"><span>没有您要搜索的内容</span></td></tr>';
                this.load();
            }
            ;

            BiaoGe.DEFAULTS = {
                columns: [],
                pageSizes: [10, 25, 50, 100]
            };

            BiaoGe.prototype = {
                constructor: BiaoGe,

                load: function () {
                    this.initHead();
                    this.initBody();
                    this.initPager();
                    this.loadData();
                    this.addListener();
                },

                initHead: function () {
                    var options = this.options;
                    if (options.columns.length > 0) {
                        var theadHtml = '<thead><tr>';
                        for (var i = 0; i < options.columns.length; i++) {
                            var col = options.columns[i];
                            theadHtml += '<th';
                            if (col.width)
                                theadHtml += ' width="' + col.width + '"';
                            if (col.align)
                                theadHtml += ' class="text-' + col.align + '"';
                            theadHtml += '>' + col.title + '</th>';
                        }
                        theadHtml += '</tr></thead>';
                        $(theadHtml).appendTo(this.$table);
                    }
                },

                initBody: function () {
                    this.$tbody = $('<tbody></tbody>');
                    this.$tbody.appendTo(this.$table);
                    this.emptyLoading();
                },

                emptyLoading: function () {
                    this.$tbody.html(this.loadingTrHtml);
                },

                initPager: function () {
                    this.$table_pager = $('<div></div>');
                    var $table_parent = $($(this.$table).parent().get(0));
                    if ($table_parent.hasClass("table-responsive")) {
                        this.$table_pager.insertAfter($table_parent);
                    }
                    else {
                        this.$table_pager.insertAfter(this.$table);
                    }
                    this.$table_pager.addClass('table-pager');

                    this.initPageSize();
                    this.initPageNum();
                },

                initPageSize: function () {
                    var $group_page_size = $('<div></div>');
                    $group_page_size.appendTo(this.$table_pager);
                    $group_page_size.addClass('input-group pager-size');

                    var $span = $('<span >显示</span>');
                    $span.appendTo($group_page_size);
                    $span.addClass('input-group-addon');

                    this.$page_size = $('<select></select>');
                    this.$page_size.appendTo($group_page_size);
                    this.$page_size.addClass('form-control input-sm');
                    var options = '';
                    for (var i = 0; i < this.options.pageSizes.length; i++) {
                        var size = this.options.pageSizes[i];
                        options += '<option value="' + size + '">' + size
                                + '</option>';
                    }
                    this.$page_size.html(options);
                    this.page_size = this.$page_size.val();

                    $span = $('<span >条 共</span>');
                    $span.appendTo($group_page_size);
                    $span.addClass('input-group-addon');

                    this.$record_total = $('<strong>0</strong>');
                    this.$record_total.appendTo($span);

                    $span.append('条');
                },

                initPageNum: function () {
                    var $group_page_num = $('<div></div>');
                    $group_page_num.appendTo(this.$table_pager);
                    $group_page_num.addClass('input-group pager-num');

                    var $span = $('<span >第</span>');
                    $span.appendTo($group_page_num);
                    $span.addClass('input-group-addon');

                    this.$page_num = $('<select></select>');
                    this.$page_num.appendTo($group_page_num);
                    this.$page_num.addClass('form-control input-sm');
                    var options = '<option value="1">1</option>';
                    this.$page_num.html(options);

                    $span = $('<span >页</span>');
                    $span.appendTo($group_page_num);
                    $span.addClass('input-group-addon');
                },

                addListener: function () {
                    this.$page_size.on("change", $.proxy(this.pageSizeChange,
                            this));
                    this.$page_num.on("change", $.proxy(this.pageNumChange,
                            this));
                },

                pageSizeChange: function () {
                    this.page_size = this.$page_size.val();
                    this.page_num = 1;
                    this.loadData();
                },

                pageNumChange: function () {
                    this.page_num = this.$page_num.val();
                    this.loadData();
                },

                loadData: function () {
                    if (this.options.loadData
                            && typeof this.options.loadData === 'function') {
                        this.options.loadData(this.page_num, this.page_size, $
                                .proxy(this.initData, this));
                    }
                },

                refreshData: function () {
                    this.emptyLoading();
                    this.loadData();
                },

                reloadData: function () {
                    this.emptyLoading();
                    this.page_num = 1;
                    this.loadData();
                },

                initData: function (data) {
                    var options = this.options;
                    var tbodyHtml = '';
                    if (data && data.rows) {
                        for (var c = 0; c < data.rows.length; c++) {
                            tbodyHtml += '<tr>';
                            for (var i = 0; i < options.columns.length; i++) {
                                tbodyHtml += this.initTd(options.columns[i],
                                        data.rows[c], c);
                            }
                            tbodyHtml += '</tr>';
                        }
                    }
                    if (tbodyHtml.isNullOrEmpty())
                        tbodyHtml = this.emptyTrHtml;
                    this.$tbody.data('d', data);
                    this.$tbody.html(tbodyHtml);
                    this.setPager(data.total);
                },

                initTd: function (col, row, c) {
                    var tdHtml = '<td';
                    if (col.width)
                        tdHtml += ' width="' + col.width + '"';
                    var tdclass = '';
                    if (col.align)
                        tdclass += 'text-' + col.align;
                    if (col.td_class)
                        tdclass += ' ' + col.td_class;
                    if (tdclass.length > 0)
                        tdHtml += ' class="' + tdclass + '"';
                    tdHtml += '>';
                    if (col.field) {
                        var value = '';

                        if ( typeof row[col.field] !="undefined")
                            value = row[col.field];
                        
                        if (col.formatter
                                && typeof col.formatter === 'function') {
                            tdHtml += col.formatter(value, row, c);
                        } else if (col.maxLength) {
                            var content = value;
                            if (content.length > col.maxLength)
                                content = content.substring(0, col.maxLength);
                            tdHtml += '<div title="' + value + '">' + content
                                    + '</div>';
                        } else {
                            tdHtml += value;
                        }
                        
                    } else {
                        tdHtml += ((this.page_size * (this.page_num - 1)) + c + 1);
                    }
                    tdHtml += '</td>';
                    return tdHtml;
                },

                setPager: function (total) {
                    this.$record_total.html(total);
                    var pages = Math.ceil(total / this.page_size);
                    var options = "";
                    for (var i = 1; i <= pages; i++) {
                        options += '<option value="' + i + '" ' + ((i == this.page_num) ? 'selected="selected"' : '') + '>' + i
                                + '</option>';
                    }
                    this.$page_num.html(options);
                },

                getRowData: function (index) {
                    var data = this.$tbody.data('d');
                    if (data && data.rows) {
                        return data.rows[index];
                    }
                    else
                        return null;
                }
            };

            $.fn.BiaoGe = function (options) {
                var args = toArray(arguments, 1),
                result;

                this.each(function () {
                    var $this = $(this),
                    data = $this.data('BiaoGe'),
                    fn;

                    if (!data) {
                        $this.data('BiaoGe', (data = new BiaoGe(this,
                                        options)));
                    }

                    if (typeof options === 'string'
                            && $.isFunction((fn = data[options]))) {
                        result = fn.apply(data, args);
                    }
                });

                return isUndefined(result) ? this : result;
            };
        });


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