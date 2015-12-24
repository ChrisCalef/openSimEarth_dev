

singleton Material(MainRotor_ka50RotorTexture)
{
   mapTo = "unmapped_mat";
   diffuseColor[0] = "0.8 0.8 0.8 1";
   specular[0] = "0.32 0.32 0.32 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
   materialTag0 = "Miscellaneous";
};

singleton Material(blade_ka50_rotor)
{
   mapTo = "ka50_rotor";
   diffuseColor[0] = "0.8 0.8 0.8 1";
   specular[0] = "0.32 0.32 0.32 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(blade_ka50_rotor)
{
   mapTo = "unmapped_mat";
   diffuseColor[0] = "0.8 0.8 0.8 1";
   specular[0] = "0.32 0.32 0.32 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   materialTag0 = "Miscellaneous";
   doubleSided = "1";
};

singleton Material(ka50_prop_texture)
{
   mapTo = "ka50_prop";
   diffuseMap[0] = "art/shapes/FlightGear/ka50/Models/MainRotor/prop.png";
   specular[0] = "0.32 0.32 0.32 1";
   specularPower[0] = "50";
   doubleSided = "1";
   translucentBlendOp = "None";
};

singleton Material(ka50_rotor_texture)
{
   mapTo = "ka50_rotor";
   diffuseMap[0] = "art/shapes/FlightGear/ka50/Models/MainRotor/colors.png";
   specular[0] = "0.32 0.32 0.32 1";
   specularPower[0] = "50";
   translucentBlendOp = "None";
   materialTag0 = "Miscellaneous";
};
