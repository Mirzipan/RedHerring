using RedHerring.Render.Models;
using Vortice.Mathematics;

namespace RedHerring.Motive.Entities.Components;

public sealed class ModelComponent : AnEntityComponent
{
    private Model? _model;
    private bool _isDirty;
    
    public Model? Model
    {
        get => _model;
        set
        {
            if (_model != value)
            {
                MarkAsDirty();
            }
            
            _model = value;
        }
    }

    public BoundingBox BoundingBox;
    public BoundingSphere BoundingSphere;

    #region Lifecycle

    public ModelComponent()
    {
    }

    public ModelComponent(Model? model)
    {
        _model = model;
        MarkAsDirty();
    }

    #endregion Lifecycle

    #region Internal

    internal void MarkAsDirty()
    {
        _isDirty = true;
    }

    internal void Recalculate()
    {
        if (_model is null)
        {
            BoundingBox = BoundingBox.Empty;
            BoundingSphere = BoundingSphere.Empty;
            return;
        }

        foreach (var mesh in _model.Meshes)
        {
            BoundingBox = BoundingBox.CreateMerged(BoundingBox, mesh.BoundingBox);
        }
        
        BoundingSphere = BoundingSphere.CreateFromBoundingBox(BoundingBox);

        _isDirty = false;
    }

    #endregion Internal
}