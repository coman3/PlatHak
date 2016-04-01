using System;

namespace PlatHack.Game
{
    public interface IInputSurface
    {
        void OnInput(InputEventArgs args);
    }

    public class InputEventArgs : EventArgs
    {
        public InputType InputType { get; }
        public InputValue ValueType { get; }

        /// <summary>
        /// Key Value (Key-code), 
        /// Or X axis Value,
        /// Or If button was on Down Or Up (0, 1 respectably)
        /// </summary>
        public int ValueX { get; }

        /// <summary>
        /// Y axis Value, otherwise null
        /// </summary>
        public int? ValueY { get; }

        public InputEventArgs(InputType inputType, InputValue valueType, int valueX = 0, int? valueY = null)
        {
            InputType = inputType;
            ValueType = valueType;
            ValueX = valueX;
            ValueY = valueY;
        }
    }

    public enum InputValue
    {
        //Mouse
        RightMouse,
        LeftMouse,
        ScrollWheel,
        ScrollWheelValue,
        MouseMove,

        //Keyboard
        KeyDown,
        KeyUp,

        //GamePad,
        Button1, //XBox: A
        Button2, //XBox: B
        Button3, //XBox: Y
        Button4, //XBox: X

        DPadLeft, //Click
        DPadRight, //Click
        DPadUp, //Click
        DPadDown, //Click

        LeftStickMove, //Axis
        LeftStick, //Click

        RightStickMove, //Axis
        RightStick, //Click

        RightShoulder, //Click
        LeftShoulder, //Click
        RightTrigger, //Axis
        LeftTrigger, //Axis

        Start,
        Back,
        Center //XBox: Logo Button


    }

    public enum InputType
    {
        Keyboard,
        Mouse,
        GamePad
    }
}