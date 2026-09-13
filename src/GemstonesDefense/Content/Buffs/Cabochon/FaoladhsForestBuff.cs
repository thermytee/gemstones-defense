using GemstonesDefense.Content.Mounts;

namespace GemstonesDefense.Content.Buffs.Cabochon;

public class FaoladhsForestBuff : ModBuff
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Cabochon.FaoladhsForestBuff.KEY;

    /// <inheritdoc/> 
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.buffNoTimeDisplay[Type] = true;
        Main.buffNoSave[Type] = true;
    }

    /// <inheritdoc/> 
    public override void Update(Player player, ref int buffIndex)
    {
        player.mount.SetMount(ModContent.MountType<FaoladhsForestMount>(), player);

        player.buffTime[buffIndex] = 2;
    }
}