using Terraria.GameContent;

namespace GemstonesDefense.Content.Cabochon;

public class RubyRoostItem : ModItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.noUseGraphic = true;
        Item.noMelee = true;

        Item.width = 32;
        Item.height = 30;

        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.UseSound = SoundID.Item2;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.buffTime = 3600;
        Item.buffType = ModContent.BuffType<OnyxOwlBuff>();

        Item.shoot = ModContent.ProjectileType<OnyxOwlProjectile>();
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Ruby, 10)
            .AddIngredient(ItemID.Diamond)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

public class OnyxOwlBuff : ModBuff
{
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.buffNoTimeDisplay[Type] = true;
        Main.lightPet[Type] = true;
    }

    public override void Update(Player player, ref int buffIndex)
    {
        base.Update(player, ref buffIndex);

        var unused = false;

        player.BuffHandle_SpawnPetIfNeededAndSetTime(buffIndex, ref unused, ModContent.ProjectileType<OnyxOwlProjectile>());
    }
}

public class OnyxOwlProjectile : ModProjectile
{
    public Player Owner => Main.player[Projectile.owner];

    public ref float Timer => ref Projectile.ai[0];

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.projPet[Type] = true;
        Main.projFrames[Type] = 13;

        ProjectileID.Sets.LightPet[Type] = true;

        ProjectileID.Sets.TrailingMode[Type] = 3;
        ProjectileID.Sets.TrailCacheLength[Type] = 12;
    }

    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.tileCollide = true;
        Projectile.netImportant = true;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 30;
        Projectile.height = 30;

        Projectile.aiStyle = -1;
    }

    public override bool OnTileCollide(Vector2 oldVelocity) => false;
    
    public override void AI()
    {
        base.AI();

        UpdateAnimation();
        UpdateActive();
        UpdateMovement();
        UpdateTeleport();
        UpdateCollision();

        if (Main.dedServ)
        {
            return;
        }

        var brightness = 0.5f * Projectile.Opacity;

        Lighting.AddLight(Projectile.Center, brightness, brightness, brightness);
    }
    
    private void UpdateAnimation()
    {
        Projectile.spriteDirection = MathF.Sign(Owner.Center.X - Projectile.Center.X);

        Projectile.frameCounter++;

        var moving = Owner.velocity.Length() > 1f;
        var frameRate = moving ? 5 - (int)MathHelper.Clamp(Projectile.velocity.Length(), 0f, 2f) : 10;

        if (!moving)
        {
            frameRate = Projectile.frame == 0 ? 60 : frameRate;
        }

        if (Projectile.frameCounter < frameRate)
        {
            return;
        }

        var minFrame = moving ? 7 : 0;
        var maxFrame = moving ? 12 : 6;

        Projectile.frame++;

        if (Projectile.frame > maxFrame)
        {
            Projectile.frame = minFrame;
        }

        Projectile.frameCounter = 0;
    }

    private void UpdateActive()
    {
        if (Owner.dead || Owner.ghost || !Owner.HasBuff<OnyxOwlBuff>())
        {
            return;
        }

        Projectile.timeLeft = 2;

        if (Owner.active)
        {
            return;
        }

        Projectile.active = false;
    }

    private void UpdateCollision()
    {
        Projectile.tileCollide = Projectile.DistanceSQ(Owner.Center) < 50f * 16f * 50f * 16f;

        if (!Projectile.tileCollide)
        {
            return;
        }

        if (Projectile.position.X == Projectile.oldPosition.X)
        {
            Projectile.velocity.Y -= 0.2f;
        }
        else
        {
            Projectile.velocity.Y *= 0.95f;
        }
    }

    private void UpdateMovement()
    {
        Timer++;

        if (Owner.velocity.Length() <= 1f)
        {
            var offset = new Vector2(0f, MathF.Sin(Timer / 20f) * 4f);
            var position = Owner.Center + new Vector2(32f * Owner.direction, 0f) + offset;

            Projectile.Center = Vector2.SmoothStep(Projectile.Center, position, 0.2f);

            Projectile.velocity *= 0.95f;
        }
        else
        {
            var speed = 0.05f;
            var rotation = Timer * speed % MathHelper.TwoPi;

            var radius = 50f;
            var offset = new Vector2(MathF.Cos(rotation) * radius, MathF.Sin(rotation) * (radius / 2f));

            var position = Owner.Center + offset;

            Projectile.Center = Vector2.SmoothStep(Projectile.Center, position, 0.2f);

            Projectile.velocity += new Vector2(Main.rand.NextFloat(-0.3f, 0.3f), Main.rand.NextFloat(-0.3f, 0.3f));
        }
    }

    private void UpdateTeleport()
    {
        if (Projectile.DistanceSQ(Owner.Center) <= 100f * 16f * 100f * 16f)
        {
            return;
        }

        Projectile.Center = Owner.Center;
    }

    public override bool PreDraw(ref Color lightColor)
    {
        var texture = TextureAssets.Projectile[Type].Value;

        var offsetX = 0;
        var offsetY = 0;

        var originX = 0f;

        ProjectileLoader.DrawOffset(Projectile, ref offsetX, ref offsetY, ref originX);

        var positionOffset = new Vector2(offsetX, offsetY + Projectile.gfxOffY);
        var originOffset = new Vector2(originX, 0f);

        var frame = texture.Frame(1, Main.projFrames[Type], 0, Projectile.frame);

        var origin = frame.Size() / 2f + originOffset;
        var effects = Projectile.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

        var length = ProjectileID.Sets.TrailCacheLength[Type];

        for (var i = 0; i < length; i += 4)
        {
            var trailPosition = Projectile.oldPos[i] + Projectile.Size / 2f - Main.screenPosition + positionOffset;

            Main.EntitySpriteDraw
            (
                texture,
                trailPosition,
                frame,
                Projectile.GetAlpha(lightColor) * (1f - i / (float)length),
                Projectile.rotation,
                origin,
                Projectile.scale,
                effects
            );
        }

        var position = Projectile.Center - Main.screenPosition + positionOffset;

        Main.EntitySpriteDraw
        (
            texture,
            position,
            frame,
            Projectile.GetAlpha(lightColor),
            Projectile.rotation,
            origin,
            Projectile.scale,
            effects
        );

        return false;
    }
}