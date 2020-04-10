using System.Collections;
using System.Linq;
using System.Text;
using System.Web;

namespace Core.ExtensionHelpers
{
    public static class ViewModelExtensions
    {
        public static string ToQueryString<T>(this T model)
        {
            var properties = from p in model.GetType().GetProperties()
                             where p.GetValue(model, null) != null
                             select GetText(model, p);

            return string.Join("&", properties.ToArray());
        }

        private static string GetText<T>(T model, System.Reflection.PropertyInfo p)
        {
            var value = p.GetValue(model, null);
            var queryTextFormat = "{0}={1}";
            if (typeof(IEnumerable).IsAssignableFrom(p.PropertyType) && !typeof(string).IsAssignableFrom(p.PropertyType))
            {
                StringBuilder sb = new StringBuilder();
                foreach (var item in (IEnumerable)value)
                {
                    sb.AppendFormat(queryTextFormat, p.Name, item);
                    sb.Append('&');
                }

                sb.Remove(sb.Length - 1, 1);
                return sb.ToString();
            }
            return p.Name + "=" + HttpUtility.UrlEncode(value.ToString());
        }
    }
}
