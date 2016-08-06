package com.koc_mm.items;

import net.minecraft.inventory.EntityEquipmentSlot;
import net.minecraft.item.ItemArmor;
import net.minecraftforge.fml.common.registry.GameRegistry;

public class CustomArmor extends ItemArmor{

	public CustomArmor(String uName, ArmorMaterial materialIn, int renderIndexIn, EntityEquipmentSlot equipmentSlotIn) {
		super(materialIn, renderIndexIn, equipmentSlotIn);
		// TODO Auto-generated constructor stub
		setRegistryName(uName);
		setUnlocalizedName(uName);
		GameRegistry.register(this);
	}
	
}
