using System;
using System.Reflection;

namespace Domain.IntegrationTests.Services.Core
{
    public static class DataFactory
    {
        private static readonly Random rnd = new Random();

        /// <summary>
        /// Attempts to create a valid instance of a given class T. In case it cannot resolve a 
        /// dummy data for a given property, it set it to the default value of object (null)
        /// </summary>
        /// <typeparam name="T">The type of the class to be instantiated</typeparam>
        /// <returns>Instance of the supplied class, filled with dummy data</returns>
        public static T  CreateInstance<T>() 
        {
            try
            {
                var obj = Activator.CreateInstance(typeof(T));

                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    TypeCode typeCode = Type.GetTypeCode(prop.PropertyType);
                    DataManipulator.FillObjectPropertiesWithData(typeCode, prop, ref obj);
                }

                return (T)Convert.ChangeType(obj, typeof(T));
            }

            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Fill the given object property with specific data supplied. Can be chained up to create a specific instance of an object
        /// </summary>
        /// <typeparam name="T">The type of the class to be instantiated</typeparam>
        /// <param name="obj">The object whose properties will be changed by this method</param>
        /// <param name="propertyName">Property name of the property that would be updated with a new value</param>
        /// <param name="propertyValue">Value that would be set</param>
        /// <returns></returns>
        public static T WithPropertyValue<T>(this T obj, string propertyName, object propertyValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(propertyName))
                {
                    throw new ArgumentException("propertyName cannot be null, empty, or white spaces");
                }

                PropertyInfo prop = obj.GetType().GetProperty(propertyName);
                prop.SetValue(obj, propertyValue);
                return obj;
            }
            catch (Exception)
            {
                return obj;
            }
        }

        public static class DataManipulator
        {
            public static void FillObjectPropertiesWithData(TypeCode typeCode, PropertyInfo prop, ref object obj)
            {

                switch (typeCode)
                {
                    case TypeCode.String:
                        prop.SetValue(obj, GetValidString());
                        break;
                    case TypeCode.Int32:
                        prop.SetValue(obj, GetValidNumber());
                        break;
                    case TypeCode.Decimal:
                        prop.SetValue(obj, GetValidDecimal());
                        break;
                    case TypeCode.DateTime:
                        prop.SetValue(obj, GetValidDateTime());
                        break;
                    case TypeCode.Boolean:
                        prop.SetValue(obj, true);
                        break;
                    default:
                        prop.SetValue(obj, null);
                        break;
                }
            }
            public static DateTime GetValidDateTime()
            {
                var start = new DateTime(1950, 1, 1);
                int range = (DateTime.Today - start).Days;
                return start.AddDays(rnd.Next(range));
            }
            public static double GetValidDecimal()
            {
                return rnd.NextDouble();
            }
            public static int GetValidNumber()
            {
                return rnd.Next(1, 100);
            }
            public static string GetValidString()
            {
                return $"{Guid.NewGuid()}";
            }
        }
    }
}
