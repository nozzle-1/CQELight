using System;

namespace CQELight.DAL.Attributes
{
    /// <summary>
    /// Attribute used to ignore a specific table or column.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class IgnoreAttribute : Attribute
    {
    }
}
