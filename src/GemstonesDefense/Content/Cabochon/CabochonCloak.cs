using System.Collections.Generic;
using GemstonesDefense.Common.Recipes;
using Terraria.DataStructures;

namespace GemstonesDefense.Content.Cabochon;

[AutoloadEquip(EquipType.Wings)]
public class CabochonCloakItem : ModItem
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Cabochon.CabochonCloakItem.KEY;

    /// <inheritdoc/> 
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(210, 2f);
    }

    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.accessory = true;

        Item.defense = 10;

        Item.width = 38;
        Item.height = 32;
    }

    /// <inheritdoc/> 
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Diamond, 10)
            .AddIngredient(ItemID.Ruby, 10)
            .AddIngredient(ItemID.Sapphire, 10)
            .AddIngredient(ItemID.Emerald, 10)
            .AddIngredient(ItemID.Amethyst, 10)
            .AddIngredient(ItemID.Topaz, 10)
            .AddRecipeGroup(GoldBarRecipeGroup.Group)
            .AddIngredient(ItemID.Silk)
            .Register();
    }
    
    /// <inheritdoc/> 
    public override void UpdateAccessory(Player player, bool hideVisual)
    {
        base.UpdateAccessory(player, hideVisual);

        var modPlayer = player.GetModPlayer<CabochonCloakPlayer>();
        
        Lighting.AddLight(player.Center, Main.DiscoColor.ToVector3() * 0.75f * MathHelper.Clamp(player.velocity.Length() / player.maxRunSpeed, 0f, 1f));

        modPlayer.Enabled = true;

        player.lifeRegen += (int)(player.lifeRegen * 0.1f);
        player.manaRegen += (int)(player.manaRegen * 0.1f);

        player.pickSpeed += 0.1f;

        player.GetCritChance(DamageClass.Generic) += 0.1f;
    }

    /// <inheritdoc/> 
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);

        var line = new TooltipLine(Mod, $"{nameof(CabochonCloakItem)}:Ability", this.GetLocalizedValue("Ability"))
        {
            OverrideColor = new Color(112, 144, 219)
        };

        tooltips.Add(line);
    }
}

public sealed class CabochonCloakPlayer : ModPlayer
{
    private int _counter;
    
    /// <summary>
    ///     Gets or sets a value indicating whether the effects of the Cabochon Cloak are enabled.
    /// </summary>
    /// <value>
    ///    <see langword="true"/> if the effects of the Cabochon Cloak are enabled; otherwise, <see langword="false"/>.
    /// </value>
    public bool Enabled { get; set; }

    /// <summary>
    ///     Gets the current frame of the Cabochon Cloak.
    /// </summary>
    public int Frame { get; private set; } = 4;
    
    /// <inheritdoc/> 
    public override void ResetEffects()
    {
        base.ResetEffects();

        Enabled = false;
    }

    public override void PostUpdate()
    {
        base.PostUpdate();
        
        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        _counter++;

        if (_counter < 5f)
        {
            return;
        }

        var flying = Player.position != Player.oldPosition && Player.velocity.Y != 0f && Player.wingTime > 0f;

        if (flying)
        {
            UpdateFlyingAnimation();
        }
        else
        {
            UpdateIdleAnimation();
        }

        _counter = 0;
    }

    private void UpdateFlyingAnimation()
    {
        if (Frame == 4)
        {
            Frame = 0;
        }
        else
        {
            Frame++;
        
            if (Frame < 4)
            {
                return;
            }
            
            Frame = 1;
        }
    }

    private void UpdateIdleAnimation()
    {
        if (Player.velocity.Y != 0f)
        {
            Frame = 0;
        }
        else
        {
            Frame = Frame != 0 && Frame != 4 ? 0 : 4;
        }
    }

    /// <inheritdoc/> 
    public override void HideDrawLayers(PlayerDrawSet drawInfo)
    {
        base.HideDrawLayers(drawInfo);

        if (!Enabled && !Main.gameMenu)
        {
            return;
        }
        
        PlayerDrawLayers.Wings.Hide();
    }

    /// <inheritdoc/> 
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(target, hit, damageDone);

        if (!Enabled || target.life > 0)
        {
            return;
        }

        target.NPCLoot_DropMoney(Player);
    }
}

public sealed class CabochonCloakWingsLayer : PlayerDrawLayer
{
    /// <inheritdoc/> 
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<CabochonCloakPlayer>().Enabled;
    
    /// <inheritdoc/> 
    public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.BackAcc);

    /// <inheritdoc/> 
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var player = drawInfo.drawPlayer;
        var modPlayer = player.GetModPlayer<CabochonCloakPlayer>();

        var texture = Assets.Images.Content.Cabochon.CabochonCloakItem_Wings.Asset.Value;
        
        var frame = texture.Frame(1, 5, 0, modPlayer.Frame);
        var origin = frame.Size() / 2f;

        var offset = new Vector2
        (
            (int)(drawInfo.Position.X + player.width / 2f - player.bodyFrame.Width / 2f - Main.screenPosition.X),
            (int)(drawInfo.Position.Y + player.height - player.bodyFrame.Height + 4f - Main.screenPosition.Y)
        );
        
        var position = player.bodyPosition + drawInfo.bodyVect + offset - new Vector2(10f * player.direction, 0f);

        var data = new DrawData
        (
            texture,
            position,
            frame,
            drawInfo.colorArmorBody,
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect
        );

        drawInfo.DrawDataCache.Add(data);
    }
}

public class CabochonCloakBodyLayer : PlayerDrawLayer
{
    /// <inheritdoc/> 
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.GetModPlayer<CabochonCloakPlayer>().Enabled;
    
    /// <inheritdoc/> 
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Torso);

    /// <inheritdoc/> 
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var player = drawInfo.drawPlayer;
        var origin = drawInfo.bodyVect;

        var texture = Assets.Images.Content.Cabochon.CabochonCloakItem_Body.Asset.Value;

        var offset = new Vector2
        (
            (int)(drawInfo.Position.X + player.width / 2f - player.bodyFrame.Width / 2f - Main.screenPosition.X),
            (int)(drawInfo.Position.Y + player.height - player.bodyFrame.Height + 4f - Main.screenPosition.Y)
        );
        
        var position = player.bodyPosition + drawInfo.bodyVect + offset;

        var data = new DrawData
        (
            texture,
            position,
            player.bodyFrame,
            drawInfo.colorArmorBody,
            player.bodyRotation,
            origin,
            1f,
            drawInfo.playerEffect
        );

        drawInfo.DrawDataCache.Add(data);
    }
}