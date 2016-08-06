package com.koc_mm.items;

import net.minecraft.client.renderer.block.model.ModelResourceLocation;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.entity.player.EntityPlayer;
import net.minecraft.item.ItemFood;
import net.minecraft.item.ItemStack;
import net.minecraft.potion.Potion;
import net.minecraft.potion.PotionEffect;
import net.minecraft.world.World;
import net.minecraftforge.client.model.ModelLoader;
import net.minecraftforge.fml.common.registry.GameRegistry;
import net.minecraftforge.fml.relauncher.Side;
import net.minecraftforge.fml.relauncher.SideOnly;

public class CustomFood extends ItemFood{
	
	private PotionEffect[] effects;
	
	public CustomFood(int amount, float saturation, boolean isWolfFood) {
		super(amount, saturation, isWolfFood);
		// Register the item
		setRegistryName("customfood");
		setUnlocalizedName("customfood");
		GameRegistry.register(this);
		this.setCreativeTab(CreativeTabs.tabFood);
		
		//this is a new line of code, it lets the player eat the food at any time
		this.setAlwaysEdible();
		
		//potion effects, see gamepedia for reference
		effects = new PotionEffect[] {
		                           new PotionEffect(Potion.getPotionById(1), 1200, 0),
		                           new PotionEffect(Potion.getPotionById(11), 200, 1),
		                           new PotionEffect(Potion.getPotionById(23), 1, -1)
									};
	}
	
	//Makes the model
	@SideOnly(Side.CLIENT)
	public void initModel() {
		ModelLoader.setCustomModelResourceLocation(this, 0,
				new ModelResourceLocation(getRegistryName(), "inventory"));
	}
	
	//Eating the food
	@Override
	protected void onFoodEaten(ItemStack stack, World world, EntityPlayer player){
		super.onFoodEaten(stack, world, player);
		
		//i++ is the same as i = i + 1
		for (int i = 0; i < effects.length; i++){
			if (!world.isRemote && effects[i] != null){
				player.addPotionEffect(new PotionEffect(this.effects[i]));
			}
		}
	}
	
}
