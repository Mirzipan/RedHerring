﻿using Vortice.Mathematics;

namespace RedHerring.Motive.Entities.Components;

public abstract class RenderComponent : EntityComponent
{
    // TODO: inject
    private readonly ModelComponent? _modelComponent;
    
    public Color Color { get; set; }
    public bool IsShadowCaster { get; set; }
}