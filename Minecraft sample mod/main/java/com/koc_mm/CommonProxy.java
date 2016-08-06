package com.koc_mm;

import net.minecraftforge.fml.common.event.FMLInitializationEvent;
import net.minecraftforge.fml.common.event.FMLPostInitializationEvent;
import net.minecraftforge.fml.common.event.FMLPreInitializationEvent;

public class CommonProxy {

	//preInit here
	public void preInit(FMLPreInitializationEvent e) {
		ModBlocks.init();
		ModItems.init();
		ModCrafting.initCrafting();
	}
	
	//init here
	public void init(FMLInitializationEvent e) {
		
	}
	
	//postInit here
	public void postInit(FMLPostInitializationEvent e) {
		
	}
}
