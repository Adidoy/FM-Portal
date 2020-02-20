using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;

namespace PUPFMIS.Helpers
{
    public static class HtmlHelpers
    {
        //public static string DisplayShortNameFor<TModel, TValue>(Expression<TModel> t, Expression<Func<TModel, TValue>> exp)
        //{
        //    CustomAttributeNamedArgument? DisplayName = null;
        //    var prop = exp.Body as MemberExpression;
        //    if (prop != null)
        //    {
        //        var DisplayAttrib = (from c in prop.Member.GetCustomAttributesData()
        //                                where c.AttributeType == typeof(DisplayAttribute)
        //                                select c).FirstOrDefault();
        //        if (DisplayAttrib != null)
        //            DisplayName = DisplayAttrib.NamedArguments.Where(d => d.MemberName == "ShortName").FirstOrDefault();
        //    }
        //    return DisplayName.HasValue ? DisplayName.Value.TypedValue.Value.ToString() : "";
        //}
        public static string DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.ShortDisplayName;

            return string.IsNullOrWhiteSpace(description) ? "" : description;
        }
    }
}

