﻿namespace RedHerring.Core.Components;

public abstract class AComponent<TContainer> : AComponent where TContainer : class, IComponentContainer
{
    public abstract override TContainer? Container { get; }
}