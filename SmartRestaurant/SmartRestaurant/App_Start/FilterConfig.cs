﻿using SmartRestaurant.Areas.Restaurant.Controllers;
using SmartRestaurant.Infrastructure;

namespace SmartRestaurant.App_Start
{
    public class FilterConfig
    {
        public static void Configure(System.Web.Mvc.GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            //filters.Add(new AuthenticationAttribute());
        }
    }
}
