using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Input;

namespace GemstonesDefense.Utilities;

public static class InputUtilities
{
    public static class Keyboard
    {
        /// <summary>
        ///     Checks whether the specified key is currently pressed.
        /// </summary>
        /// <param name="key">
        ///     The key to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified key is currently pressed; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Pressed(Keys key) => Main.keyState.IsKeyDown(key);
    
        /// <summary>
        ///     Checks whether the specified key is currently released.
        /// </summary>
        /// <param name="key">
        ///     The key to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified key is currently released; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Released(Keys key) => Main.oldKeyState.IsKeyDown(key);
        
        /// <summary>
        ///    Checks whether the specified key has just been pressed.
        /// </summary>
        /// <param name="key">
        ///     The key to check.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified key has just been pressed; otherwise, <see langword="false"/>.
        /// </returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool JustPressed(Keys key) => Pressed(key) && !Released(key);
    }
}