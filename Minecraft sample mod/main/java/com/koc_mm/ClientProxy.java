package com.koc_mm;

import net.minecraftforge.fml.common.event.FMLPreInitializationEvent;

public class ClientProxy extends CommonProxy{
	@Override
	public void preInit(FMLPreInitializationEvent e){
		super.preInit(e);
		
		ModBlocks.initModels();
		ModItems.initModels();
	}
}
