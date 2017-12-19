$(document).ready(function () {
    var animationSpeed = 300;

    $(document.body).on('click', '.sidebar-menu li a', function (e) {
        var $this = $(this);
        var checkElement = $this.next();

        if (checkElement.is('.treeview-menu') && checkElement.is(':visible')) {//父菜单已被打开
            checkElement.slideUp(animationSpeed, function () {
                checkElement.removeClass('menu-open');
            });
            checkElement.parent("li").removeClass("active");
            //查找到右侧的箭头图标
            //var parent_i = checkElement.parent("li").find('i.glyphicon.glyphicon-triangle-bottom').first();
            //parent_i.removeClass('glyphicon glyphicon-triangle-bottom');
            //更换成下箭头
            //parent_i.addClass('glyphicon glyphicon-triangle-right');
        }

            //If the menu is not visible
        else if ((checkElement.is('.treeview-menu')) && (!checkElement.is(':visible'))) {//父菜单未被打开
            //Get the parent menu
            var parent = $this.parents('ul').first();
            //Close all open menus within the parent
            var ul = parent.find('ul:visible').slideUp(animationSpeed);
            //Remove the menu-open class from the parent
            ul.removeClass('menu-open');
            //Get the parent li
            var parent_li = $this.parent("li");
            //查找到右侧的箭头图标
            //var parent_i = parent_li.find('i.glyphicon.glyphicon-triangle-right').first();
            //parent_i.removeClass('glyphicon glyphicon-triangle-right');
            //更换为下箭头表示菜单被打开
            //parent_i.addClass('glyphicon glyphicon-triangle-bottom');

            //Open the target menu and add the menu-open class
            checkElement.slideDown(animationSpeed, function () {
                //Add the class active to the parent li
                checkElement.addClass('menu-open');
                parent.find('li.active').removeClass('active');
                parent_li.addClass('active');
                
            });
        }
        //if this isn't a link, prevent the page from being redirected
        if (checkElement.is('.treeview-menu')) {
            e.preventDefault();
        }
    });
});