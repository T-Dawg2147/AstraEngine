namespace AstraEngine.Editor
{
    public sealed class EditorLayout
    {
        public HierarchyPanel Hierarchy { get; } = new();
        public InspectorPanel Inspector { get; } = new();
        public ViewportPanel Viewport { get; } = new();

        public void Draw()
        {
            Hierarchy.Draw();
            Inspector.Draw();
            Viewport.Draw();
        }
    }
}
