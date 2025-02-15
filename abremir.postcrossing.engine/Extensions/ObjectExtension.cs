﻿using System;
using System.Collections;
using System.Reflection;

namespace abremir.postcrossing.engine.Extensions
{
    public static class ObjectExtensions
    {
        public static void TrimAllStrings<TSelf>(this TSelf obj)
        {
            if (obj != null)
            {
                if (obj is IEnumerable)
                {
                    foreach (var listItem in obj as IEnumerable)
                    {
                        listItem.TrimAllStrings();
                    }
                }
                else
                {
                    const BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;

                    foreach (PropertyInfo p in obj.GetType().GetProperties(flags))
                    {
                        Type currentNodeType = p.PropertyType;
                        if (currentNodeType == typeof(String))
                        {
                            string currentValue = (string)p.GetValue(obj, null);
                            if (currentValue != null && p.CanWrite)
                            {
                                p.SetValue(obj, currentValue.Trim(), null);
                            }
                        }
                        // see http://stackoverflow.com/questions/4444908/detecting-native-objects-with-reflection
                        else if (currentNodeType != typeof(object) && Type.GetTypeCode(currentNodeType) == TypeCode.Object)
                        {
                            p.GetValue(obj, null).TrimAllStrings();
                        }
                    }
                }
            }
        }
    }
}
