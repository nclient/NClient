﻿using System;

namespace NClient.Annotations
{
    /// <summary>Indicates that a type and all derived types are used to serve API responses or/and to send requests.</summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class FacadeAttribute : Attribute, IFacadeAttribute
    {
    }
}
