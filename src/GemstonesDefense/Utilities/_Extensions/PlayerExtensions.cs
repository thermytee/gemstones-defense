using System.Runtime.CompilerServices;

namespace GemstonesDefense.Utilities;

/// <summary>
///     Provides <see cref="Player" /> extensions.
/// </summary>
public static class PlayerExtensions
{
    /// <summary>
    ///     Attempts to remove a buff from the player.
    /// </summary>
    /// <param name="player">The player to remove the buff from.</param>
    /// <typeparam name="T">The type of the buff to remove.</typeparam>
    /// <returns><c>true</c> if the buff was removed; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TryRemoveBuff<T>(this Player player) where T : ModBuff
    {
        var index = player.FindBuffIndex(ModContent.BuffType<T>());

        if (index == -1)
        {
            return false;
        }

        player.DelBuff(index);

        return true;
    }

    /// <summary>
    ///     Checks whether the player has just double tapped up or not.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns><c>true</c> if the player has just double tapped up; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool JustDoubleTappedUp(this Player player) => player.controlUp && player.releaseUp && player.doubleTapCardinalTimer[1] < 15;

    /// <summary>
    ///     Checks whether the player has just landed on a solid surface or not.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <returns><c>true</c> if the player has just landed on a solid surface; otherwise, <c>false</c>.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool JustLanded(this Player player) => player.velocity.Y == 0f && player.oldVelocity.Y != 0f;
}