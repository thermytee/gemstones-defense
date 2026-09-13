namespace GemstonesDefense.Content.Buffs.Cabochon;

public class OnyxArmorBuff : ModBuff
{
    /// <inheritdoc/>
    public override string Texture => Assets.Images.Content.Cabochon.OnyxArmorBuff.KEY;
    
    /// <inheritdoc/> 
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.buffNoTimeDisplay[Type] = true;
    }

    /// <inheritdoc/>
    public override void Update(Player player, ref int buffIndex)
    {
        base.Update(player, ref buffIndex);

        player.immune = true;
        player.stoned = true;

        player.buffTime[buffIndex] = 2;
    }
}

