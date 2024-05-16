using Godot;
using System;

namespace ElementSandbox
{
	public partial class Main : Node
	{
		public static Random random = new();

		public override void _Input(InputEvent @event)
		{
			string inputText = @event.AsText();
			bool inputIsNumber = int.TryParse(inputText, out int inputKeycode);

			// Calculates the keycode value for the given input and sets the selected element
			// to the keycode if it is within range of ElementID
			if (inputIsNumber) {
				if (inputKeycode <= Enum.GetNames(typeof(ElementID)).Length) {
                    ElementID elementID = (ElementID)inputKeycode - 1;
					GD.Print(inputKeycode - 1);

					Grid.selectedElement = elementID;
					GD.Print(elementID);
                }
			}
		}

	}
}
