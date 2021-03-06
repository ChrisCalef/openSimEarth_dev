//-----------------------------------------------------------------------------
// Copyright (c) 2012 GarageGames, LLC
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to
// deal in the Software without restriction, including without limitation the
// rights to use, copy, modify, merge, publish, distribute, sublicense, and/or
// sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS
// IN THE SOFTWARE.
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Sample grass
// ----------------------------------------------------------------------------

singleton Material(TerrainFX_grass1)
{
   mapTo = "grass1";
   footstepSoundId = 0;
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   effectColor[0] = "0.42 0.42 0 1";
   effectColor[1] = "0.42 0.42 0 1";
   impactSoundId = "0";
};


singleton Material(TerrainFX_grass2)
{
   mapTo = "grass2";
   footstepSoundId = 0;
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   effectColor[0] = "0.42 0.42 0 1";
   effectColor[1] = "0.42 0.42 0 1";
   impactSoundId = "0";
};


singleton Material()
{
   mapTo = "grass1_dry";
   footstepSoundId = 0;
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   effectColor[0] = "0.63 0.55 0 1";
   diffuseMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_07_Dif";
   diffuseSize = "500";
   normalMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_07_Nm";
   detailMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_07";
   detailSize = "5";
   detailStrength = "1";
   detailDistance = "50";
   internalName = "gravel_07";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
};


singleton Material()
{
   mapTo = "dirt_grass";
   footstepSoundId = 0;
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   effectColor[0] = "0.63 0.55 0 1";
   diffuseMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_01_Dif";
   diffuseSize = "500";
   normalMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_01_Nm";
   detailMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_01";
   detailDistance = "50";
   internalName = "earth_01";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
   detailSize = "2";
   detailStrength = "1";
};


// ----------------------------------------------------------------------------
// Sample rock
// ----------------------------------------------------------------------------

singleton Material()
{
   mapTo = "rocktest";
   footstepSoundId = "1";
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   impactSoundId = "1";
   effectColor[0] = "0.25 0.25 0.25 1";
   effectColor[1] = "0.25 0.25 0.25 0";
   diffuseMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_08_Dif";
   diffuseSize = "400";
   normalMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_08_Nm";
   detailMap = "art/terrains/TT_Materials/Cracked and Dry Earth/TT_Earth_08";
   detailSize = "3";
   detailDistance = "100";
   internalName = "earth_08";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
};


// ----------------------------------------------------------------------------
// Sample rock
// ----------------------------------------------------------------------------

singleton Material()
{
   mapTo = "stone";
   footstepSoundId = "1";
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   impactSoundId = "1";
   effectColor[0] = "0.25 0.25 0.25 1";
   effectColor[1] = "0.25 0.25 0.25 0";
   diffuseMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14_Dif";
   diffuseSize = "500";
   detailMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14";
   detailSize = "2";
   detailDistance = "50";
   internalName = "rock_14";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
   detailStrength = "1";
   parallaxScale = "0";
   normalMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14_Nm";
};

// ----------------------------------------------------------------------------
// Sample sand
// ----------------------------------------------------------------------------

singleton Material()
{
   mapTo = "sand";
   footstepSoundId = "3";
   terrainMaterials = "1";
   ShowDust = "1";
   showFootprints = "1";
   materialTag0 = "Terrain";
   specularPower[0] = "1";
   effectColor[0] = "0.84 0.71 0.5 1";
   effectColor[1] = "0.84 0.71 0.5 0.349";
   diffuseMap = "art/terrains/TT_Materials/Sand/TT_Sand_01_Dif";
   diffuseSize = "500";
   normalMap = "art/terrains/TT_Materials/Sand/TT_Sand_01_Nm";
   detailMap = "art/terrains/TT_Materials/Sand/TT_Sand_01";
   detailSize = "5";
   detailDistance = "50";
   internalName = "sand_01";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
   parallaxScale = "0";
   detailStrength = "1";
};


new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14_Dif";
   normalMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14_Nm";
   internalName = "warning_material";
   detailMap = "art/terrains/TT_Materials/Stone and Rock/TT_Rock_14";
};

new TerrainMaterial()
{
   //diffuseMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_02_Dif";
   diffuseMap = "art/terrains/forests_tile";//??
   normalMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_02_Nm";
   detailMap = "art/terrains/TT_Materials/Gravel and Rubble/TT_Gravel_02";
   internalName = "gravel_02";
   detailSize = "2";
   macroMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_14_Dif";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_01_Dif";
   diffuseSize = "200";
   normalMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_01_Nm";
   detailMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_01";
   detailSize = "2";
   internalName = "grass_01";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_20_Dif";
   diffuseSize = "200";
   normalMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_20_Nm";
   detailMap = "art/terrains/TT_Materials/Grass and Moss/TT_Grass_20";
   detailSize = "1.2";
   internalName = "grass_20";
   isManaged = "1";
   detailBrightness = "1";
   enabled = "1";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Snow and Ice/TT_Snow_01_Dif";
   normalMap = "art/terrains/TT_Materials/Snow and Ice/TT_Snow_01_Nm";
   detailMap = "art/terrains/TT_Materials/Snow and Ice/TT_Snow_01";
   detailSize = "1";
   internalName = "snow_01";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_07_Dif";
   normalMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_07_Nm";
   detailMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_07";
   detailSize = "1";
   internalName = "mud_07";
};

new TerrainMaterial()
{
   diffuseMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_14_Dif";
   normalMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_14_Nm";
   detailMap = "art/terrains/TT_Materials/Mud and Mulch/TT_Mud_14";
   detailSize = "1";
   internalName = "mud_14";
};
