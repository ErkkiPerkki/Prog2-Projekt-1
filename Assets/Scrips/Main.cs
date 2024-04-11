using Godot;

namespace ElementSandbox
{
    public partial class Main : Node
    {
        static Timer GameUpdateTimer;

        public override void _Ready()
        {
            GameUpdateTimer = GetNode<Timer>("GameUpdate");
        }

        public override void _Process(double delta)
        {

        }

        void _OnGameUpdate()
        {
            
        }

    }
}
