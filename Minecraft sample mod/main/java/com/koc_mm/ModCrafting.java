package com.koc_mm;

import net.minecraft.init.Items;
import net.minecraft.item.ItemStack;
import net.minecraftforge.fml.common.registry.GameRegistry;

public class ModCrafting {
	
	public static void initCrafting(){
		GameRegistry.addRecipe(new ItemStack(ModBlocks.texturedBlock), 
				new Object[] {"# #", " I ", "# #",
				Character.valueOf('#'), Items.apple, 
				Character.valueOf('I'), ModBlocks.firstBlock});
		
	}
}
