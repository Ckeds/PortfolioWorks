package com.koc_mm;

import net.minecraftforge.fml.common.Mod;
import net.minecraftforge.fml.common.Mod.EventHandler;
import net.minecraftforge.fml.common.SidedProxy;
import net.minecraftforge.fml.common.event.FMLInitializationEvent;
import net.minecraftforge.fml.common.event.FMLPostInitializationEvent;
import net.minecraftforge.fml.common.event.FMLPreInitializationEvent;


@Mod(modid = Main.MODID, version = Main.VERSION)
public class Main
{
    public static final String MODID = "koc_mm";
    public static final String VERSION = "1.0";
    
    @SidedProxy(clientSide="com.koc_mm.ClientProxy", serverSide="com.koc_mm.ServerProxy")
    public static CommonProxy proxy;
    
    @Mod.Instance
    public static Main instance;
    //1 Minecraft Starts
    //2 Minecraft makes init calls
    //3 This mod answers Minecraft's calls
    
    @EventHandler
    public void preInit(FMLPreInitializationEvent e) {
    	proxy.preInit(e);
    	
    }
    
    @EventHandler
    public void init(FMLInitializationEvent e) {
    	proxy.init(e);
    	
    }
    
    @EventHandler
    public void postInit(FMLPostInitializationEvent e) {
    	proxy.postInit(e);
    }
}
