using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SmartRestaurant
{
    public static class JsonHelper
    {
        //public static string ToJson<T>(this T t) where T : IEntity
        //{
        //    if (t == null)
        //        return "{}";

        //    string jsonString = JsonSerializer.Serialize(t);


        //    string p = @"\\/Date\(([-]?\d+)\+\d+\)\\/";
        //    MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
        //    Regex reg = new Regex(p);
        //    jsonString = reg.Replace(jsonString, matchEvaluator);
        //    return jsonString;
        //}

    }
}