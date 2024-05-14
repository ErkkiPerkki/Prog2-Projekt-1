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
			string inputText = @event.AsText();
			bool inputIsNumber = int.TryParse(inputText, out int inputKeycode);

			if (inputIsNumber) {
				if (inputKeycode <= Enum.GetNames(typeof(ElementID)).Length) {
                    ElementID elementID = (ElementID)inputKeycode - 1;
					GD.Print(inputKeycode - 1);

					Grid.selectedElement = elementID;
					GD.Print(elementID);
                }
			}
		}

		void _OnGridUpdate()
		{
			
		}

	}
}
