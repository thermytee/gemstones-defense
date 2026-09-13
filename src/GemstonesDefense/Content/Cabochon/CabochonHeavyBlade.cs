namespace GemstonesDefense.Content.Cabochon;

public class CabochonHeavyBladeItem : ModItem
{
    public const float Brightness = 0.5f;
    
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.autoReuse = true;

        Item.DamageType = DamageClass.Melee;
        Item.knockBack = 7f;
        Item.damage = 100;
        Item.crit = 15;

        Item.width = 84;
        Item.height = 84;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item1;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.rare = ItemRarityID.LightRed;
        Item.value = Item.buyPrice(gold: 10);
    }

    public override void AddRecipes()
    {
        base.AddRecipes();

        CreateRecipe()
            .AddIngredient<CabochonBladeItem>()
            .AddIngredient(ItemID.CrystalShard, 50)
            .AddTile(TileID.MythrilAnvil)
            .Register();
    }

    public override void MeleeEffects(Player player, Rectangle hitbox)
    {
        base.MeleeEffects(player, hitbox);

        if (Main.dedServ)
        {
            return;
        }

        Lighting.AddLight(player.itemLocation, Brightness, Brightness, Brightness);
    }

    public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
    {
        base.PostDrawInWorld(spriteBatch, lightColor, alphaColor, rotation, scale, whoAmI);

        if (Main.dedServ)
        {
            return;
        }

        Lighting.AddLight(Item.Center, Brightness, Brightness, Brightness);
    }
}