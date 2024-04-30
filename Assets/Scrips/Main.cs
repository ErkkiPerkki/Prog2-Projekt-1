using Godot;
using System;

namespace ElementSandbox
{
	public partial class Main : Node
	{
		public static Random random = new();


		public override void _Process(double delta)
		{
			
		}

		public override void _Input(InputEvent @event)
		{
			if (Input.IsActionPressed("SwitchTileUp")) {
				int currentElementID = (int)Grid.selectedElement;
				
				ElementID nextElementID = (ElementID)((currentElementID + 1) % Enum.GetNames(typeof(ElementID)).Length);
				Grid.selectedElement = nextElementID;

                GD.Print(Grid.selectedElement);
            }
			else if (Input.IsActionPressed("SwitchTileDown")) {
				int currentElementID = (int)Grid.selectedElement;
				
				ElementID nextElementID = (ElementID)(Math.Abs(currentElementID - 1) % Enum.GetNames(typeof(ElementID)).Length);
				Grid.selectedElement = nextElementID;

				GD.Print(Grid.selectedElement);
			}
		}

		void _OnGridUpdate()
		{
			
		}

	}
}
