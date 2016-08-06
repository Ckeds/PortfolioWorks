package com.koc_mm.items;


import net.minecraft.client.renderer.block.model.ModelResourceLocation;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.item.Item;
import net.minecraftforge.client.model.ModelLoader;
import net.minecraftforge.fml.common.registry.GameRegistry;
import net.minecraftforge.fml.relauncher.Side;
import net.minecraftforge.fml.relauncher.SideOnly;

public class FirstItem extends Item {

	public FirstItem(){
		//register the item to the game
		setRegistryName("firstitem");
		setUnlocalizedName("firstitem");
		GameRegistry.register(this);
		this.setCreativeTab(CreativeTabs.tabMaterials);
	}
	
	//Load the texture as our model
	@SideOnly(Side.CLIENT)
	public void initModel() {
		ModelLoader.setCustomModelResourceLocation(this, 0,
				new ModelResourceLocation(getRegistryName(), "inventory"));
	}
}


