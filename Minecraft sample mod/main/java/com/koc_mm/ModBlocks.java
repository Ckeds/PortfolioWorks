package com.koc_mm;

import com.koc_mm.blocks.FirstBlock;
import com.koc_mm.blocks.TexturedBlock;
import net.minecraftforge.fml.relauncher.Side;
import net.minecraftforge.fml.relauncher.SideOnly;

public class ModBlocks {
	
	//Handling all of our custom blocks
	public static FirstBlock firstBlock;
	public static TexturedBlock texturedBlock;
	
	public static void init() {
		firstBlock = new FirstBlock();
		texturedBlock = new TexturedBlock();
	}
	
	@SideOnly(Side.CLIENT)
	public static void initModels(){
		texturedBlock.initModel();
	}
}
