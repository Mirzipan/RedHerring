﻿using System.Numerics;
using RedHerring.Render.Features;
using Silk.NET.Maths;

namespace RedHerring.Render;

public interface RendererContext
{
    RenderFeatureCollection Features { get; }
    Shared                  Shared   { get; }

    void AddFeature(RenderFeature feature);
    void Init();
    void Close();
    bool BeginDraw();
    void Draw();
    void EndDraw();
    void Resize(Vector2D<int> size);

    void SetCameraViewMatrix(Matrix4x4 world, Matrix4x4 view, Matrix4x4 projection, float fieldOfView, float clipPlaneNear,
        float clipPlaneFar);

    void ReloadShaders();
}