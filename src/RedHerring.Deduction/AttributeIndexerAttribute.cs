﻿namespace RedHerring.Deduction;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class AttributeIndexerAttribute : Attribute
{
    public Type AttributeType { get; }
}