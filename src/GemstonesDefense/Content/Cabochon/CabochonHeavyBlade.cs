using Terraria.Audio;

namespace GemstonesDefense.Content.Cabochon;

public class CabochonHeavyBladeItem : ModItem
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Cabochon.CabochonHeavyBladeItem.KEY;
    
    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();
        
        Item.DamageType = DamageClass.Melee;
        
        Item.knockBack = 7f;
        Item.damage = 100;
        Item.crit = 15;

        Item.autoReuse = true;
        
        Item.width = 84;
        Item.height = 84;

        Item.UseSound = SoundID.Item1;
        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.rare = ItemRarityID.LightRed;
        
        Item.value = Item.buyPrice(gold: 10);
    }

    /// <inheritdoc/> 
    public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
    {
        base.OnHitNPC(player, target, hit, damageDone);
        
        SoundEngine.PlaySound(in SoundID.Item70, target.Center);
        SoundEngine.PlaySound(in SoundID.NPCHit42, target.Center);
    }

    /// <inheritdoc/> 
    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<CabochonBladeItem>()
            .AddIngredient(ItemID.CrystalShard, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }
}