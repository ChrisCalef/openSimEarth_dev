
singleton TSShapeConstructor(MainRotorDts)
{
   baseShape = "./MainRotor.dts";
};

function MainRotorDts::onLoad(%this)
{
   %this.addNode("Col-1", "", "0 0 0 0 0 1 0", "0");
   %this.addCollisionDetail("-1", "Convex Hulls", "mainrotor", "4", "30", "30", "32", "30", "30", "30");
}