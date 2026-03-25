using AstraEngine.Scene;

namespace AstraEngine.Editor
{
    public sealed class EditorState
    {
        public Scene.Scene? Scene { get; set; }
        public ISceneObject? SelectedObject { get; set; }

        public string SelectedName => SelectedObject switch
        {
            MeshEntity mesh => mesh.Material.Name,
            null => "None",
            _ => SelectedObject.GetType().Name
        };
    }
}
