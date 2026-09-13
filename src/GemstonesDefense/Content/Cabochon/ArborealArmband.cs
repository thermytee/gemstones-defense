using GemstonesDefense.Content.Mounts;

namespace GemstonesDefense.Content.Cabochon;

public class ArborealArmbandItem : ModItem
{
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.noMelee = true;

        Item.width = 32;
        Item.height = 32;

        Item.useTime = 20;
        Item.useAnimation = 20;
        Item.UseSound = SoundID.Item79;
        Item.useStyle = ItemUseStyleID.Swing;

        Item.mountType = ModContent.MountType<FaoladhsForestMount>();
    }
}