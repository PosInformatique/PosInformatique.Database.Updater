//-----------------------------------------------------------------------
// <copyright file="TestTools.cs" company="P.O.S Informatique">
//     Copyright (c) P.O.S Informatique. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace PosInformatique.Database.Updater.Tests
{
    using System.Reflection;

    public static class TestTools
    {
        public static T GetFieldValue<T>(this object obj, string fieldName)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            var value = field!.GetValue(obj);

            return (T)value!;
        }
    }
}
