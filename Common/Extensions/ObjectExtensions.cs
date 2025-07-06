using System.Collections;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Common.Extensions;

public static class ObjectExtensions
{
    public static T As<T>(this object obj)
        where T : class
    {
        return (T)obj;
    }
    public static bool AreEqual<T>(T obj1, T obj2, List<string> ignoredProperties = null)
    {
        if (Equals(obj1, default(T)) || Equals(obj2, default(T)))
            return Equals(obj1, default(T)) && Equals(obj2, default(T));

        ignoredProperties ??= new List<string>();

        Type type = typeof(T);
        foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (ignoredProperties.Contains(prop.Name))
                continue;

            object value1 = prop.GetValue(obj1);
            object value2 = prop.GetValue(obj2);

            if (value1 == null || value2 == null)
            {
                if (value1 != value2)
                    return false;
            }
            else if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }

    public static bool IsEqualTo<T>(this T obj1, T obj2, HashSet<string> ignoredProperties = null)
    {
        if (obj1 == null || obj2 == null)
            return obj1 == null && obj2 == null;

        ignoredProperties ??= [];

        Type type = typeof(T);
        foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (ignoredProperties.Contains(prop.Name))
                continue;

            object value1 = prop.GetValue(obj1);
            object value2 = prop.GetValue(obj2);

            if (value1 == null || value2 == null)
            {
                if (value1 != value2)
                    return false;
            }
            else if (!value1.Equals(value2))
            {
                return false;
            }
        }

        return true;
    }
    public static string ToJsonString(this object obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
    }
    public static string ToJsJson(this object obj)
    {
        return JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
    }
    public static bool IsEmpty(this IEnumerable<object> list)
    {
        return list == null || list.Count() == 0;
    }
    public static bool IsEqual<T>(this IEnumerable<T> list, IEnumerable<T> other)
    {

        if (list == null || other == null)
        {
            return list != other;
        }

        return !list.SequenceEqual(other);


    }
    public static bool IsEqual<T>(this IList<T> list, IList<T> other)
    {

        if (list == null || other == null)
        {
            return list != other;
        }

        return list.SequenceEqual(other);


    }
    public static bool HasValue(this IEnumerable<object> list) => !list.IsEmpty();

    public static bool HasAttribute<T>(this PropertyInfo propertyInfo) => Attribute.IsDefined(propertyInfo, typeof(T));

    public static bool IsPrimitiveType(this Type type)
    {
        // Check if the type is a primitive type
        if (type.IsPrimitive) return true;

        // Check for other types often treated as primitives
        if (type == typeof(string) || type == typeof(decimal) || type == typeof(DateTime))
            return true;

        return false;
    }


        
}