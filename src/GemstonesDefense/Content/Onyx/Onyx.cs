namespace GemstonesDefense.Content.Onyx;

public class OnyxItem : ModItem
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Onyx.OnyxItem.KEY;

    /// <inheritdoc/> 
    public override void SetDefaults()
    {
        base.SetDefaults();

        Item.DefaultToPlaceableTile(ModContent.TileType<OnyxTile>());

        Item.width = 16;
        Item.height = 18;
    }
}

public class OnyxTile : ModTile
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Onyx.OnyxTile.KEY;
    
    /// <inheritdoc/> 
    public override void SetStaticDefaults()
    {
        base.SetStaticDefaults();

        Main.tileSolid[Type] = true;
        Main.tileLighted[Type] = true;
        Main.tileBlockLight[Type] = true;
        
        AddMapEntry(new Color(107, 76, 181));
        
        MineResist = 1f;
        
        HitSound = SoundID.Tink;
        DustType = ModContent.DustType<OnyxDust>();
    }

    /// <inheritdoc/> 
    public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
    {
        base.ModifyLight(i, j, ref r, ref g, ref b);

        r = 0.1f;
        g = 0f;
        b = 0.15f;
    }
    
    /// <inheritdoc/> 
    public override void NumDust(int i, int j, bool fail, ref int num)
    {
        base.NumDust(i, j, fail, ref num);

        num = fail ? 1 : 3;
    }
}

public class OnyxDust : ModDust
{
    /// <inheritdoc/> 
    public override string Texture => Assets.Images.Content.Onyx.OnyxDust.KEY;
    
    /// <inheritdoc/> 
    public override void OnSpawn(Dust dust)
    {
        base.OnSpawn(dust);
        
        dust.noLight = true;
        dust.noGravity = true;

        dust.frame = new Rectangle(0, Main.rand.Next(3) * 6, 6, 6);
    }

    /// <inheritdoc/> 
    public override bool Update(Dust dust)
    {
        if (dust.alpha >= 255)
        {
            dust.active = false;
        }
        else
        {
            dust.alpha += 15;
        }
        
        dust.position += dust.velocity;

        dust.velocity.Y += 0.2f;
        dust.velocity.X *= 0.99f;
        
        return false;
    }
}