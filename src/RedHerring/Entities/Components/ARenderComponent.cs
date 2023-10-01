﻿using Vortice.Mathematics;

namespace RedHerring.Entities.Components;

public abstract class ARenderComponent : AnEntityComponent
{
    // TODO: inject
    private readonly ModelComponent? _modelComponent;
    
    public Color Color { get; set; }
    public bool IsShadowCaster { get; set; }
}