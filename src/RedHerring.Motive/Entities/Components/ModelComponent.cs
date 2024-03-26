using RedHerring.Numbers;
using RedHerring.Render.Models;

namespace RedHerring.Motive.Entities.Components;

public sealed class ModelComponent : EntityComponent
{
    private Scene? _model;
    private bool _isDirty;
    
    public Scene? Model
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

    public ModelComponent(Scene? model)
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
            BoundingBox = BoundingBox.Include(BoundingBox, mesh.BoundingBox);
        }
        
        BoundingSphere = BoundingSphere.FromBox(BoundingBox);

        _isDirty = false;
    }

    #endregion Internal
}