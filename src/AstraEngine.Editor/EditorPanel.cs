namespace AstraEngine.Editor
{
    public abstract class EditorPanel
    {
        public string Title { get; }
        
        protected EditorPanel(string title)
        {
            Title = title;
        }

        public abstract void Draw();
    }

    public sealed class HierarchyPanel : EditorPanel
    {
        public HierarchyPanel() : base("Hierarchy") { }

        public override void Draw()
        {

        }
    }

    public sealed class InspectorPanel : EditorPanel
    {
        public InspectorPanel() : base("Inspector") { }

        public override void Draw()
        {

        }
    }

    public sealed class ViewportPanel : EditorPanel
    {
        public ViewportPanel() : base("Viewport") { }

        public override void Draw()
        {

        }
    }
}
