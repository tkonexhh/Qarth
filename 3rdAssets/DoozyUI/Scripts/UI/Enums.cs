namespace DoozyUI
{
    /// <summary>
    /// Types of Input Modes.
    /// </summary>
    public enum ControllerInputMode
    {
        /// <summary>
        /// The button will only react to mouse clicks and touches.
        /// </summary>
        None,
        /// <summary>
        /// The button will react to set KeyCodes.
        /// </summary>
        KeyCode,
        /// <summary>
        /// The button will react to set virtual button names (set up in the InputManager).
        /// </summary>
        VirtualButton
    }
}