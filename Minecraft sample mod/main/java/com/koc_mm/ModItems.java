package com.koc_mm;

import com.koc_mm.items.CustomArmor;
import com.koc_mm.items.CustomFood;
import com.koc_mm.items.FirstItem;
import com.koc_mm.items.ThrownWeapon;

import net.minecraft.inventory.EntityEquipmentSlot;
import net.minecraft.item.ItemArmor.ArmorMaterial;
import net.minecraftforge.common.util.EnumHelper;
import net.minecraftforge.fml.relauncher.Side;
import net.minecraftforge.fml.relauncher.SideOnly;

public class ModItems {
	public static ArmorMaterial ARMOR = EnumHelper.addArmorMaterial("ARMOR",
			"koc_mm:texture", 25, new int[] {2,7,5,3}, 15, ArmorMaterial.IRON.getSoundEvent());
	
	//Handling all of our custom items
	public static FirstItem firstItem;
	public static CustomFood customFood;
	public static ThrownWeapon thrownWeapon;
	public static CustomArmor head;
	public static CustomArmor chest;
	public static CustomArmor legs;
	public static CustomArmor feet;
	
	public static void init(){
		firstItem = new FirstItem();
		customFood = new CustomFood(1, 0F, false);
		thrownWeapon = new ThrownWeapon();
		head = new CustomArmor("modhead", ARMOR, 1, EntityEquipmentSlot.HEAD);
		chest = new CustomArmor("modchest", ARMOR, 1, EntityEquipmentSlot.CHEST);
		legs = new CustomArmor("modlegs", ARMOR, 1, EntityEquipmentSlot.LEGS);
		feet = new CustomArmor("modfeet", ARMOR, 1, EntityEquipmentSlot.FEET);
	}
	
	@SideOnly(Side.CLIENT)
	public static void initModels() {
		firstItem.initModel();
		customFood.initModel();
	}
}
