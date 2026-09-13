using System.Collections.Generic;
using GemstonesDefense.Common.Recipes;
using Terraria.DataStructures;

namespace GemstonesDefense.Content.Cabochon;

public sealed class CabochonCowlDrawLayer : PlayerDrawLayer
{
    /// <inheritdoc/>
    public override bool GetDefaultVisibility(PlayerDrawSet drawInfo) => drawInfo.drawPlayer.armor[0].type == ModContent.ItemType<CabochonCowlItem>() && drawInfo.drawPlayer.direction == -1;
    
    /// <inheritdoc/>
    public override Position GetDefaultPosition() => new AfterParent(PlayerDrawLayers.Head);
    
    /// <inheritdoc/>
    protected override void Draw(ref PlayerDrawSet drawInfo)
    {
        var texture = Assets.Images.Content.Cabochon.CabochonCowlItem_Head_Alt.Asset.Value;

        var player = drawInfo.drawPlayer;
        var origin = drawInfo.headVect;

        var offset = new Vector2
        (
            (int)(drawInfo.Position.X + player.width / 2f - player.bodyFrame.Width / 2f - Main.screenPosition.X),
            (int)(drawInfo.Position.Y + player.height - player.bodyFrame.Height + 4f - Main.screenPosition.Y)
        );

        var position = player.headPosition + drawInfo.headVect + offset;

        var data = new DrawData
        (
            texture,
            position,
            player.bodyFrame,
            drawInfo.colorArmorHead,
            player.headRotation,
            origin,
            1f,
            SpriteEffects.FlipHorizontally
        );

        drawInfo.DrawDataCache.Add(data);
    }
}

public sealed class CabochonCowlPlayer : ModPlayer
{
    /// <summary>
    ///     
    /// </summary>
    public bool Enabled { get; set; }
    
    public override void ResetEffects()
    {
        base.ResetEffects();

        Enabled = false;
    }
}

[AutoloadEquip(EquipType.Head)]
public class CabochonCowlItem : ModItem
{
    /// <inheritdoc/>
    public override string Texture => Assets.Images.Content.Cabochon.CabochonCowlItem.KEY;

    /// <inheritdoc/>
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.defense = 10;

        Item.width = 28;
        Item.height = 30;

        Item.rare = ItemRarityID.Green;
    }

    /// <inheritdoc/>
    public override void UpdateEquip(Player player)
    {
        base.UpdateEquip(player);

        player.luck += 0.01f;
        
        player.GetModPlayer<CabochonCowlPlayer>().Enabled = true;
    }

    /// <inheritdoc/>
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Obsidian, 10)
            .AddIngredient(ItemID.Diamond, 10)
            .AddRecipeGroup(GoldBarRecipeGroup.Group)
            .Register();
    }

    /// <inheritdoc/>
    public override void ModifyTooltips(List<TooltipLine> tooltips)
    {
        base.ModifyTooltips(tooltips);

        var line = new TooltipLine(Mod, $"{nameof(CabochonCowlItem)}:Ability", this.GetLocalizedValue("Ability"))
        {
            OverrideColor = new Color(112, 144, 219)
        };

        tooltips.Add(line);
    }
}

[AutoloadEquip(EquipType.Legs)]
public class CabochonBootsItem : ModItem
{
    /// <inheritdoc/>
    public override string Texture => Assets.Images.Content.Cabochon.CabochonBootsItem.KEY;

    /// <inheritdoc/>
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 22;
        Item.height = 20;

        Item.defense = 10;
    
        Item.rare = ItemRarityID.Green;
    }

    /// <inheritdoc/>
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Diamond, 5)
            .AddRecipeGroup(GoldBarRecipeGroup.Group, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}

[AutoloadEquip(EquipType.Body)]
public class CabochonChestplateItem : ModItem
{
    /// <inheritdoc/>
    public override string Texture => Assets.Images.Content.Cabochon.CabochonChestplateItem.KEY;

    /// <inheritdoc/>
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.width = 40;
        Item.height = 18;

        Item.defense = 10;

        Item.rare = ItemRarityID.Green;
    }

    /// <inheritdoc/>
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient(ItemID.Emerald, 10)
            .AddRecipeGroup(GoldBarRecipeGroup.Group, 10)
            .AddTile(TileID.Anvils)
            .Register();
    }
}