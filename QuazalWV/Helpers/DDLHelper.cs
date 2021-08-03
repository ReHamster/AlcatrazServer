using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuazalWV.Helpers
{
	class DDLHelper
	{
        // Function to get property values
        public static object[] HandlePropertyValues(Type[] typeList, Stream str)
        {
            var paramsInstances = new List<object>();

            foreach (var type in typeList)
            {
                if (type == null)
                    continue;

                paramsInstances.Add(ReadObject(type, str));
            }

            return paramsInstances.ToArray();
        }

        // Complex objects

        // reads type and returns new object instance
        public static T ReadObject<T>(Stream s) where T : class
		{
            return (T)ReadObject(typeof(T), s);
		}

        public static object ReadObject(Type currentType, Stream str)
		{
            object instance;

            // handle parameters
            if (currentType == typeof(string))
            {
                instance = Helper.ReadString(str);
            }
            else if (currentType == typeof(bool))
            {
                instance = Helper.ReadBool(str);
            }
            else if (currentType == typeof(float))
            {
                instance = Helper.ReadFloat(str);
            }
            else if (currentType == typeof(double))
            {
                instance = Helper.ReadDouble(str);
            }
            else if (currentType == typeof(byte))
            {
                instance = Helper.ReadU8(str);
            }
            else if (currentType == typeof(uint) ||
                    currentType == typeof(int))
            {
                instance = Convert.ChangeType(Helper.ReadU32(str), currentType);
            }
            else if (currentType == typeof(ushort) ||
                     currentType == typeof(short))
            {
                instance = Convert.ChangeType(Helper.ReadU16(str), currentType);
            }
            else if (typeof(IEnumerable).IsAssignableFrom(currentType))
            {
                var arrayItemType = currentType.GetGenericArguments().SingleOrDefault();

                // get array size
                uint size = Helper.ReadU32(str);

                var arrayValues = new List<object>();

                // new array of objects 
                for (int i = 0; i < size; i++)
                {
                    var itemInstance = ReadObject(arrayItemType, str);
                    arrayValues.Add(itemInstance);
                }
                instance = arrayValues;
            }
            else if (typeof(Stream).IsAssignableFrom(currentType))
			{
                instance = str;
            }
            else if(typeof(RMCPRequest).IsAssignableFrom(currentType))
			{
                instance = Activator.CreateInstance(currentType, new object[] { str });
            }
            else // read complex object
			{
                // collect all properties even from base types
                var allProperties = new List<PropertyInfo>();

                var nType = currentType;
                do
                {
                    // FIXME: prepend or append?
                    allProperties.AddRange(currentType.GetProperties());
                    nType = nType.BaseType;
                } while (nType != null);

                // make creation lambda and use default constructor
                var newObjectFunc = Expression.Lambda<Func<object>>(
                    Expression.New(currentType.GetConstructor(Type.EmptyTypes))
                ).Compile();

                // get types and skip read-only
                var allPropertyTypes = allProperties.Select(x => x.CanWrite ? x.PropertyType : null);

                // this will recurse
                var allPropertyValues = HandlePropertyValues(allPropertyTypes.ToArray(), str);

                // create instance
                instance = newObjectFunc();

                // assign all values to new instance
                for (int i = 0; i < allProperties.Count; i++)
                {
                    allProperties[i].SetValue(instance, allPropertyValues[i]);
                }
            }

            return instance;
		}

        // writes object to buffer
		public static void WriteObject<T>(Stream s, T obj) where T : class
		{

		}
	}
}
