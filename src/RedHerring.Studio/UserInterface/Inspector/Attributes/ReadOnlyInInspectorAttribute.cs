﻿namespace RedHerring.Studio.UserInterface.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public sealed class ReadOnlyInInspectorAttribute : Attribute
{
}