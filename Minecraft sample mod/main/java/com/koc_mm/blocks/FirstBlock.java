package com.koc_mm.blocks;

import net.minecraft.block.Block;
import net.minecraft.block.material.Material;
import net.minecraft.creativetab.CreativeTabs;
import net.minecraft.item.ItemBlock;
import net.minecraftforge.fml.common.registry.GameRegistry;

public class FirstBlock extends Block {

	public FirstBlock() {
		super(Material.rock);
		// Register block to the game
		setUnlocalizedName("firstblock");
		setRegistryName("firstblock");
		GameRegistry.register(this);
		GameRegistry.register(new ItemBlock(this), getRegistryName());
		this.setCreativeTab(CreativeTabs.tabBlock);
	}
	
}
