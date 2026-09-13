using GemstonesDefense.Content.Cabochon;
using GemstonesDefense.Utilities;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using Terraria.Graphics;
using Terraria.Graphics.CameraModifiers;
using Terraria.Graphics.Renderers;

namespace GemstonesDefense.Content.Buffs.Cabochon;

public sealed class OnyxArmorPlayer : ModPlayer
{
    /// <summary>
    ///     Gets or sets the overlay texture.
    /// </summary>
    /// <remarks>
    ///     Fetched during <see cref="Load"/> if the client is not a server.
    /// </remarks>
    public static Asset<Texture2D> OverlayTexture { get; private set; } = null!;

    /// <summary>
    ///     Gets or sets the overlay outline texture.
    /// </summary>
    /// <remarks>
    ///     Fetched during <see cref="Load"/> if the client is not a server.
    /// </remarks>
    public static Asset<Texture2D> OverlayOutlineTexture { get; private set; } = null!;

    public override void Load()
    {
        base.Load();

        On_LegacyPlayerRenderer.DrawPlayerStoned += LegacyPlayerRenderer_DrawPlayerStoned_Hook;

        if (Main.dedServ)
        {
            return;
        }

        OverlayTexture = ModContent.Request<Texture2D>($"{nameof(GemstonesDefense)}/Assets/Textures/PlayerObsidianObelisk");
        OverlayOutlineTexture = ModContent.Request<Texture2D>($"{nameof(GemstonesDefense)}/Assets/Textures/PlayerObsidianObelisk_Outline");
    }

    public override void PostUpdateMiscEffects()
    {
        base.PostUpdateMiscEffects();

        var hasCowl = Player.TryGetModPlayer(out CabochonCowlPlayer cowlPlayer) && cowlPlayer.Enabled;
        var hasCloak = Player.TryGetModPlayer(out CabochonCloakPlayer cloakPlayer) && cloakPlayer.Enabled;

        if (!hasCowl || !hasCloak || Player.mount.Active)
        {
            return;
        }

        if (Player.HasBuff<OnyxArmorBuff>())
        {
            if (!InputUtilities.Keyboard.Pressed(Keys.W))
            {
                Player.TryRemoveBuff<OnyxArmorBuff>();

                SpawnDustEffects();
                return;
            }

            if (!Player.JustLanded())
            {
                return;
            }

            Main.instance.CameraModifiers.Add(new PunchCameraModifier(Player.Center, Vector2.UnitY, 2.5f, 6f, 20, 1000f));
        }
        else if (Player.JustDoubleTappedUp())
        {
            Player.AddBuff(ModContent.BuffType<OnyxArmorBuff>(), 60);

            SpawnDustEffects();
        }

        Player.oldVelocity = Player.velocity;
    }

    private void SpawnDustEffects()
    {
        for (var i = 0; i < 20; i++)
        {
            var dust = Dust.NewDustDirect(Player.position, Player.width, Player.height, DustID.Obsidian, Main.rand.NextFloat(-2f, 2f), Main.rand.NextFloat(-2f, 2f));

            dust.scale = Main.rand.NextFloat(1f, 1.3f);

            dust.noGravity = true;
        }
    }

    // This is code directly adapted from how vanilla renders the player while stoned.
    private static void LegacyPlayerRenderer_DrawPlayerStoned_Hook(On_LegacyPlayerRenderer.orig_DrawPlayerStoned orig, LegacyPlayerRenderer self, Camera camera, Player drawPlayer, Vector2 position)
    {
        if (!drawPlayer.HasBuff<OnyxArmorBuff>())
        {
            orig(self, camera, drawPlayer, position);
            return;
        }
        
        var effects = drawPlayer.direction != 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        camera.SpriteBatch.Draw
        (
            OverlayTexture.Value,
            new Vector2
            (
                (int)(position.X - camera.UnscaledPosition.X - drawPlayer.bodyFrame.Width / 2f + drawPlayer.width / 2f),
                (int)(position.Y - camera.UnscaledPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 8f)
            ) +
            drawPlayer.bodyPosition +
            new Vector2(drawPlayer.bodyFrame.Width / 2f, drawPlayer.bodyFrame.Height / 2f) -
            new Vector2(0f, 4f),
            null,
            Lighting.GetColor((int)(position.X + drawPlayer.width * 0.5) / 16, (int)(position.Y + drawPlayer.height * 0.5) / 16, Color.White),
            0f,
            OverlayTexture.Size() / 2f,
            1f,
            effects,
            0f
        );

        camera.SpriteBatch.Draw
        (
            OverlayOutlineTexture.Value,
            new Vector2
            (
                (int)(position.X - camera.UnscaledPosition.X - drawPlayer.bodyFrame.Width / 2f + drawPlayer.width / 2f),
                (int)(position.Y - camera.UnscaledPosition.Y + drawPlayer.height - drawPlayer.bodyFrame.Height + 8f)
            ) +
            drawPlayer.bodyPosition +
            new Vector2(drawPlayer.bodyFrame.Width / 2f, drawPlayer.bodyFrame.Height / 2f) -
            new Vector2(0f, 4f),
            null,
            Lighting.GetColor((int)(position.X + drawPlayer.width * 0.5) / 16, (int)(position.Y + drawPlayer.height * 0.5) / 16, new Color(87, 81, 173, 0)) * 0.5f,
            0f,
            OverlayOutlineTexture.Size() / 2f,
            1f,
            effects,
            0f
        );
    }
}