using GemstonesDefense.Common.Recipes;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace GemstonesDefense.Content.Cabochon;

public class CabochonBladeItem : ModItem
{
    private int comboCount = 0;
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Cabochon.CabochonBladeItem.KEY;

    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DamageType = DamageClass.Melee;
        
        Item.knockBack = 5f;
        Item.damage = 50;
        Item.crit = 10;

        Item.noUseGraphic = true;
        Item.autoReuse = true;
        Item.noMelee = true;
        
        Item.width = 60;
        Item.height = 60;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 16;
        Item.useAnimation = 16;
        Item.useStyle = ItemUseStyleID.Shoot;

        Item.rare = ItemRarityID.LightRed;
        
        Item.value = Item.buyPrice(gold: 10);

        Item.shootSpeed = 0f;
        Item.shoot = ModContent.ProjectileType<CabochonBladeProjectile>();
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, ai0: comboCount);

        comboCount = comboCount == 0 ? 1 : 0;

        return false;
    }

    /// <inheritdoc/> 
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Diamond, 5)
            .AddIngredient(ItemID.Ruby, 5)
            .AddIngredient(ItemID.Sapphire, 5)
            .AddIngredient(ItemID.Emerald, 5)
            .AddIngredient(ItemID.Amethyst, 5)
            .AddIngredient(ItemID.Topaz, 5)
            .AddRecipeGroup(GoldBarRecipeGroup.Group)
            .AddTile(TileID.Anvils)
            .Register();
    }
    
    /// <inheritdoc/> 
    public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;
}

public class CabochonBladeProjectile : ModProjectile
{
    private Player Player => Main.player[Projectile.owner];

    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Cabochon.CabochonBladeItem.KEY;

    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.friendly = true;

        Projectile.width = 32;
        Projectile.height = 32;
    }

    /// <inheritdoc/> 
    public override void AI()
    {
        base.AI();

        var alive = Player.active && !Player.dead && !Player.ghost;

        if (!alive)
        {
            Projectile.Kill();
            return;
        }
        
        var progress = Player.itemTime / (float)Player.itemTimeMax;

        if (progress <= 0f)
        {
            Projectile.Kill();
            return;
        }

        Player.heldProj = Projectile.whoAmI;

        var direction = Player.direction;

        Projectile.direction = direction;
        Projectile.spriteDirection = direction;

        float start;
        float end;

        if (Projectile.ai[0] == 0f) {
            start = MathHelper.ToRadians(-45f) * direction;
            end = MathHelper.ToRadians(135f) * direction;
        }
        else
        {
            start = MathHelper.ToRadians(135f) * direction;
            end = MathHelper.ToRadians(-45f) * direction;
        }

        var left = direction == -1;
        
        if (left)
        {
            start -= MathHelper.Pi;
            end += MathHelper.Pi;
        }

        var ease = progress * progress * progress;
        
        Projectile.scale = MathHelper.Lerp(1.2f, 0.8f, 1f - ease);
        Projectile.rotation = Utils.AngleLerp(start, end, left ? ease : 1f - ease);
        
        Projectile.Center = Player.MountedCenter + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 48f;
        
        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, Projectile.rotation + MathHelper.Pi);
        Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi + MathHelper.PiOver4 * direction);
    }

    /// <inheritdoc/> 
    public override bool PreDraw(ref Color lightColor)
    {
        var texture = TextureAssets.Projectile[Type].Value;

        var direction = Projectile.spriteDirection;
        var left = direction == -1;
        
        var origin = new Vector2(left ? texture.Width : 0f, texture.Height);
        
        float drawRotation = Projectile.rotation + (left ? MathHelper.PiOver4 * 3f : MathHelper.PiOver4);
        
        var offset = new Vector2(0f, 8f);
        var position = Player.Center - Main.screenPosition + new Vector2(DrawOffsetX, Projectile.gfxOffY) - offset;
        
        var color = Projectile.GetAlpha(lightColor);
        
        var effects = left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        Main.EntitySpriteDraw(texture, position, null, color, Projectile.rotation, origin, Projectile.scale, effects);
        
        return false;
    }
}

/*
public class GreatwoodMalletProjectile : ModProjectile
{
    private Player Player => Main.player[Projectile.owner];
    
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Keep.GreatwoodMalletProjectile.KEY;

    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ProjectileID.Sets.TrailingMode[Type] = 2;
        ProjectileID.Sets.TrailCacheLength[Type] = 4;
    }

    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();

        Projectile.ownerHitCheck = true;
        Projectile.ignoreWater = true;
        Projectile.friendly = true;

        Projectile.width = 48;
        Projectile.height = 48;
        
        Projectile.penetrate = -1;

        Projectile.localNPCHitCooldown = -1;
        Projectile.usesLocalNPCImmunity = true;
    }
    
    public override bool OnTileCollide(Vector2 oldVelocity)
    {
        Main.instance.CameraModifiers.Add(new PunchCameraModifier(Projectile.Center, Vector2.UnitY, 1f, 10f, 20));
        
        Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height);
        
        SoundEngine.PlaySound(in SoundID.Item70, Projectile.Center);
        SoundEngine.PlaySound(in SoundID.NPCHit42, Projectile.Center);
        
        return true;
    }

    /// <inheritdoc/> 
    public override void AI()
    {
        base.AI();

        var alive = Player.active && !Player.dead && !Player.ghost;

        if (!alive)
        {
            Projectile.Kill();
            return;
        }
        
        var progress = Player.itemTime / (float)Player.itemTimeMax;

        if (progress <= 0f)
        {
            Projectile.Kill();
            return;
        }

        Player.heldProj = Projectile.whoAmI;

        var direction = Player.direction;

        Projectile.direction = direction;
        Projectile.spriteDirection = direction;

        var start = MathHelper.ToRadians(-45f) * direction;
        var end = MathHelper.ToRadians(135f) * direction;

        var left = direction == -1;
        
        if (left)
        {
            start -= MathHelper.Pi;
            end += MathHelper.Pi;
        }

        var ease = progress * progress * progress;
        
        Projectile.scale = MathHelper.Lerp(1.2f, 0.8f, 1f - ease);
        Projectile.rotation = Utils.AngleLerp(start, end, left ? ease : 1f - ease);
        
        Projectile.Center = Player.MountedCenter + (Projectile.rotation - MathHelper.PiOver2).ToRotationVector2() * 48f;
        
        Player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Quarter, Projectile.rotation + MathHelper.Pi);
        Player.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, Projectile.rotation + MathHelper.Pi + MathHelper.PiOver4 * direction);
    }
    
    /// <inheritdoc/> 
    public override bool PreDraw(ref Color lightColor)
    {
        var texture = TextureAssets.Projectile[Type].Value;

        var direction = Projectile.spriteDirection;
        var left = direction == -1;
        
        var origin = new Vector2((left ? texture.Width : 0f) + DrawOriginOffsetX, texture.Height + DrawOriginOffsetY);
        
        var offset = new Vector2(0f, 8f);
        var position = Player.Center - Main.screenPosition + new Vector2(DrawOffsetX, Projectile.gfxOffY) - offset;
        
        var color = Projectile.GetAlpha(lightColor);
        
        var effects = left ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
        
        var length = ProjectileID.Sets.TrailCacheLength[Type];

        const int step = 2;
        
        for (var i = 0; i < length; i += step)
        {
            var fade = (1f - i / (float)length);
            
            Main.EntitySpriteDraw(texture, position, null, color * fade, Projectile.oldRot[i], origin, Projectile.scale, effects);
        }
        
        Main.EntitySpriteDraw(texture, position, null, color, Projectile.rotation, origin, Projectile.scale, effects);
        
        return false;
    }
}
*/