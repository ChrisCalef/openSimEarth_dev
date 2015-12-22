singleton TSShapeConstructor(M4Dts)
{
   baseShape = "./M4.dts";
};

function M4Dts::onLoad(%this)
{
   %this.addSequence("art/shapes/m4_optimized/sequences/TPose.dsq", "tpose", "0", "-1");
   %this.addSequence("art/shapes/m4_optimized/sequences/Root4.dsq", "idle", "0", "-1");
   %this.addSequence("art/shapes/m4_optimized/sequences/CMU_16_22.dsq", "walk", "0", "-1");
   %this.addSequence("art/shapes/m4_optimized/sequences/MedRun6.dsq", "run", "0", "-1");
   
   %this.addNode("Col-1","root");
   %this.addCollisionDetail(-1,"box","bounds");   
} 
