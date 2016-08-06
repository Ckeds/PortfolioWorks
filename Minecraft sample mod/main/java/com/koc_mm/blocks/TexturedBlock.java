package com.koc_mm.blocks;


import net.minecraft.block.Block;
import net.minecraft.block.material.Material;
import net.minecraft.client.renderer.block.model.ModelResourceLocation;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.item.Item;
import net.minecraft.item.ItemBlock;
import net.minecraftforge.client.model.ModelLoader;
import net.minecraftforge.fml.common.registry.GameRegistry;
import net.minecraftforge.fml.relauncher.Side;
import net.minecraftforge.fml.relauncher.SideOnly;

public class TexturedBlock extends Block {
	
	public TexturedBlock(){
		//register block to the game
		super(Material.rock);
		setUnlocalizedName("texturedblock");
		setRegistryName("texturedblock");
		GameRegistry.register(this);
		GameRegistry.register(new ItemBlock(this), getRegistryName());
		this.setCreativeTab(CreativeTabs.tabBlock);
		
		//add a few variables to our block
		this.setHardness(1.8737F);
		this.setLightOpacity(11);
		this.setLightLevel(.75F);
	}
	
	//load the model of the block
	@SideOnly(Side.CLIENT)
	public void initModel(){
		ModelLoader.setCustomModelResourceLocation(Item.getItemFromBlock(this), 0, 
				new ModelResourceLocation(getRegistryName(), "inventory"));
	}
}
