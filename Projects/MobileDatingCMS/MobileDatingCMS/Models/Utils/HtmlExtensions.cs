using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MobileDatingCMS.Models
{
    public static class HtmlExtensions
    {

        public const string HtmlLabelDivClass = "col-sm-3 col-md-2";
        public const string HtmlLabelClass = "col-sm-3 col-md-2 control-label";
        public const string HtmlControlDivClass = "col-sm-9 col-md-10";
        public const string HtmlControlClass = "form-control";

        public static HtmlString FormControl(this HtmlHelper helper, string label, string control = "input",
            string name = null, string labelClass = HtmlExtensions.HtmlLabelClass,
            string controlDivClass = HtmlExtensions.HtmlControlDivClass, string controlClass = HtmlExtensions.HtmlControlClass,
            string type = "text", bool required = false, string value = null, bool @checked = false, string html = null)
        {
            return new HtmlString(
                string.Format(
                    "<div class=\"form-group\"> <label class=\"{0}\" for=\"{4}\">{1}</label> <div class=\"{2}\"> <{3} type=\"{6}\" id=\"{4}\" name=\"{4}\" class=\"{5}\" placeholder=\"{1}\" {7} {8} {9}>{10}</{3}> </div> </div>",
                    labelClass, label, controlDivClass, control, name, controlClass, type,
                    required ? "required" : "",
                    (value != null ? "value = \"" + value + "\"" : null),
                    (@checked ? "checked" : null),
                    html));
        }

        public static HtmlString FormCombo(this HtmlHelper helper, string label,
            string name = null, string labelClass = HtmlExtensions.HtmlLabelClass,
            string controlDivClass = HtmlExtensions.HtmlControlDivClass, string controlClass = HtmlExtensions.HtmlControlClass,
            string type = "text",
            string[] values = null, string[] texts = null, int selectedIndex = 0)
        {
            StringBuilder result = new StringBuilder();

            result.Append("<div class=\"form-group\">");
            result.AppendFormat("<label class=\"{0}\">{1}</label>", labelClass, label);

            result.AppendFormat("<div class=\"{0}\">", controlDivClass);

            result.AppendFormat("<select class=\"{0}\" name=\"{1}\" id=\"{1}\">", controlClass, name);

            for (int i = 0; i < values.Length; i++)
            {
                result.AppendFormat("<option value=\"{0}\" {2}>{1}</option>",
                    values[i], texts[i], selectedIndex == i ? "selected" : null);
            }

            result.Append("</select>");

            result.Append("</div>");
            result.Append("</div>");

            return result.ToString().ToHtmlString();
        }

        public static HtmlString ToHtmlString(this String pString)
        {
            return new HtmlString(pString);
        }

        public static string ToJavascriptUtcString(this DateTime pDateTime)
        {
            return pDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

    }
}