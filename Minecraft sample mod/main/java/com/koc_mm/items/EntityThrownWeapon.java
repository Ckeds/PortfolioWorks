package com.koc_mm.items;

import net.minecraft.entity.EntityLivingBase;
import net.minecraft.entity.monster.EntityBlaze;
import net.minecraft.entity.passive.EntityCow;
import net.minecraft.entity.passive.EntityPig;
import net.minecraft.entity.projectile.EntityThrowable;
import net.minecraft.util.DamageSource;
import net.minecraft.util.EnumParticleTypes;
import net.minecraft.util.math.RayTraceResult;
import net.minecraft.world.World;

public class EntityThrownWeapon extends EntityThrowable {

	public EntityThrownWeapon(World worldIn)
    {
        super(worldIn);
    }

    public EntityThrownWeapon(World worldIn, EntityLivingBase throwerIn)
    {
        super(worldIn, throwerIn);
    }

    public EntityThrownWeapon(World worldIn, double x, double y, double z)
    {
        super(worldIn, x, y, z);
    }

    /**
     * Called when this EntityThrowable hits a block or entity.
     */
    protected void onImpact(RayTraceResult result)
    {
        if (result.entityHit != null)
        {
            int i = 2;

            if (result.entityHit instanceof EntityPig
            		|| result.entityHit instanceof EntityCow)
            {
                i = 5;
            }

            result.entityHit.attackEntityFrom(DamageSource.causeThrownDamage(this, this.getThrower()), (float)i);
        }

        if (!this.worldObj.isRemote)
        {
            this.setDead();
        }
    }
}

