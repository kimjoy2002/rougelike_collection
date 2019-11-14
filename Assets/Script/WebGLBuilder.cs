using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class WebGLBuilder
{
    static void build()
    {
        string[] scenes = { "Assets/Scenes/MainGame.unity" };
        UnityEditor.BuildPipeline.BuildPlayer(scenes, "WebGL-Dist", UnityEditor.BuildTarget.WebGL, UnityEditor.BuildOptions.None);
    }
}