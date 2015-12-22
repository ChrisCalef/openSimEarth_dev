

//=============================================================================
//
//                 OPEN SIM EARTH
//
//=============================================================================
$numScenes = 0;

function startSQL(%dbname)
{//Create the sqlite object that we will use in all the scripts.
   %sqlite = new SQLiteObject(sqlite);
   
   if (%sqlite.openDatabase(%dbname))
      echo("Successfully opened database: " @ %dbname );
   else {
      echo("We had a problem involving database: " @ %dbname );
      return;
   }
}

   //TESTING - SpatiaLite.  Exciting promise, disappointing failure... so far.
   //if (%sqlite.openDatabase("testDB.db"))
   //{
   //   echo("Successfully opened database: testDB.db" );
      //%query = "INSERT INTO testTable ( name, geom ) VALUES ('Test01',GeomFromText('POINT(1 2)'));";
      //%query = "";
      //%result = sqlite.query(%query, 0);
      //if (%result)
      //   echo("spatialite inserted into a table with a geom!");
      //else
      //   echo("spatialite failed to insert into a table with a geom!  "   );
   //   %sqlite.closeDatabase();
   //}   
   //NOW... apparently all we have to do is this, to gain access to all of SpatiaLite.
   //%query = "SELECT load_extension('libspatialite-2.dll');";
   //%result = sqlite.query(%query, 0);
   //echo( "Loaded SpatiaLite: " @ %result );
   //Except, maybe have to do this in the engine.
   
function stopSQL()
{
   sqlite.closeDatabase();
   sqlite.delete();      
}

function openSimEarthTick()
{
   if (($numScenes==0)&&($pref::opensimearth::autoLoadScenes)) //first time through, unless DB is missing or corrupt.
   {
      %query = "SELECT s.id,p.x AS pos_x,p.y AS pos_y,p.z AS pos_z " @
               "FROM scene s LEFT JOIN vector3 p ON p.id=s.pos_id;";
      %result = sqlite.query(%query, 0);
      echo("query: " @ %query);
      %i=0;
      if (%result)
      {	   
         while (!sqlite.endOfResult(%result))
         {
            %id = sqlite.getColumn(%result, "id");     
            %x = sqlite.getColumn(%result, "pos_x");
            %y = sqlite.getColumn(%result, "pos_y");
            %z = sqlite.getColumn(%result, "pos_z");
            //DatabaseSceneList.add(%name,%id);
            echo("scene " @ %id  @ " " @ %x @ " " @ %y @ " " @ %z);
            
            $scenePos[%i] = %x @ " " @ %y @ " " @ %z;
            $sceneId[%i] = %id;
            $sceneLoaded[%i] = false;
            $sceneDist[%i] = 5.0;//TEMP, add this to scenes table
            
            %i++;
            sqlite.nextRow(%result);
         }
      } 
      $numScenes = %i;
      echo("Num scenes: " @ %numScenes);
   }
   sqlite.clearResult(%result);
   
   if (($myPlayer)&&($pref::opensimearth::autoLoadScenes))
   {
      %pos = $myPlayer.getPosition();
      for (%i=0;%i<$numScenes;%i++)
      {
         %diff = VectorSub(%pos,$scenePos[%i]);
         
         if ((VectorLen(%diff)<$sceneDist[%i])&&($sceneLoaded[%i]==false))
         {
            loadScene($sceneId[%i]);
            $sceneLoaded[%i] = true;
         } 
         else if ((VectorLen(%diff)>$sceneDist[%i]*20)&&($sceneLoaded[%i]==true))//*20 completely arbitrary
         {
            unloadScene($sceneId[%i]);
            $sceneLoaded[%i] = false;              
         }
           
      }
      //echo("player position: " @ %pos );
   }
   
   schedule(60,0,"openSimEarthTick");

}
   
   

///////////////////////////////////////////////////////////////////////////////////////
//MOVE: probably these should go under tools/openSimEarth, if not obsoleted entirely.
function PhysicsShape::onStartup(%this)
{
   echo(%this @ " calling onStartup! position " @ %this.getPosition() @ " datablock " @ %this.dataBlock.getName());
   
   if (%this.dataBlock $= "M4Physics")
   {
      %this.setActionSeq("ambient","ambient");//This might not always be idle, could be just breathing
      %this.setActionSeq("idle","ambient");// and idle could be that plus fidgeting, etc.
      %this.setActionSeq("walk","walk");
      %this.setActionSeq("run","run");
      %this.setActionSeq("fall","runscerd");
      %this.setActionSeq("getup","rSideGetup");   
      %this.setActionSeq("attack","power_punch_down");
      %this.setActionSeq("block","punch_uppercut");//TEMP, don't have any blocking anims atm
           
      //%this.setIsRecording(true);
      
      %this.groundMove();
      
      echo("starting up a M4 physics shape!");      
   } 
   else if (%this.dataBlock $= "bo105Physics") 
   {
      %this.useDataSource = true;
      //%this.setIsRecording(true);
      //%this.showNodes();     
   } 
   else if (%this.dataBlock $= "dragonflyPhysics") 
   {
      %this.useDataSource = true;
   }
   else if (%this.dataBlock $= "ka50Physics") 
   {
      %this.useDataSource = true;
      //%this.showNodes();     
      %this.setName("ka50");
      %this.schedule(500,"showBlades");
   }
}

function PhysicsShape::orientTo(%this, %dest)
{
   %pos = isObject(%dest) ? %dest.getPosition() : %dest;
   %this.orientToPos(%pos);
}

function PhysicsShape::moveTo(%this, %dest, %slowDown)
{
   %pos = isObject(%dest) ? %dest.getPosition() : %dest;
   
   //This is how you print messages to the chat gui instead of the console:
   //%this.say("moving to " @ %pos);
   
   //echo(%this.getId() @ " moving to " @ %pos);
   
   %this.orientToPos(%pos);
   
   %this.actionSeq("walk");
   
   //%obj.atDestination = false;
}

function PhysicsShape::say(%this, %message)//Testing, does this only work for AIPlayers?
{
   chatMessageAll(%this, '\c3%1: %2', %this.getid(), %message);  
}
///////////////////////////////////////////////////////////////////////////////////////


//MOVE: these should be in a behaviorTrees folder, or at least a single file.
function onStartup::precondition(%this, %obj)
{
   if (%obj.startedUp != true)
      return true;
   else
      return false;
}

function onStartup::behavior(%this, %obj)
{
   //echo("calling onStartup!");   
   
   //Temp, store these in DB by shape and/or sceneShape
   %obj.setAmbientSeqByName("ambient");
   %obj.setIdleSeqByName("ambient");
   %obj.setWalkSeqByName("ambient");
   %obj.setRunSeqByName("run");
   %obj.setAttackSeqByName("power_punch_down");
   %obj.setBlockSeqByName("tpose");//TEMP, need block seq
   %obj.setFallSeqByName("ambient");
   %obj.setGetupSeqByName("rSideGetup");
   //Possibly these should not be named actions but should all be included in 
   //a sequenceActions table so it can be infinitely expanded.
   
   //Should this be automatic here, 
   %obj.groundMove();
   //or wait until we find out if we're more than just a ragdoll?
   
   %obj.startedUp = true;
   
   return SUCCESS;   
}

////////////// BEHAVIORS ///////////////////////////////

///////////////////////////////////
//[behaviorName]::precondition()
//[behaviorName]::onEnter()
//[behaviorName]::onExit()

//Do a raycast, either torque or physx, and find the ground directly below me.
//if below some threshold, then just move/interpolate us there. If above that, go to
//falling animation and/or ragdoll until we hit the ground and stop, then go to getUp task.

/* // No longer necessary... this is now done during processTick.
function goToGround::behavior(%this, %obj)
{
   %start = VectorAdd(%obj.position,"0 0 1.0");//Add a tiny bit (or, a huge amount)
                // so we don't get an error when we're actually on the ground.
                
   %contact = physx3CastGroundRay(%start);
   
   %obj.setPosition(%contact);
   echo(%this @ " is going to ground!!!!!!");
   %obj.setAmbientSeqByName("ambient");
   %obj.setIdleSeqByName("ambient");
   %obj.setWalkSeqByName("walk");
   %obj.setRunSeqByName("run");
   %obj.setAttackSeqByName("power_punch_down");
   %obj.setBlockSeqByName("tpose");
   %obj.setFallSeqByName("ambient");
   %obj.setGetupSeqByName("rSideGetup");
   
   return SUCCESS;
}
*/

///////////////////////////////////
//getUp::precondition()
//getUp::onEnter()
//getUp::onExit()

function getUp::behavior(%this, %obj)
{
   
   return SUCCESS;
}

///////////////////////////////////
//moveToPosition::precondition()
//moveToPosition::onEnter()
//moveToPosition::onExit()

function moveToPosition::behavior(%this, %obj)
{
   //echo("calling move to position!");
   %obj.groundMove();
   return SUCCESS;   
}

//////////////////////////////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////
/*
function openSimEarthGUIs()
{
   //Eventually load up all OpenSimEarth-related GUI objects from the DB.
   //For now this just fills the DatabaseSceneList dropdown.
   %query = "SELECT id,name,description from scene;";  
	%result = sqlite.query(%query, 0);
   if (%result)
   {	   
      while (!sqlite.endOfResult(%result))
      {
         %id = sqlite.getColumn(%result, "id");        
         %name = sqlite.getColumn(%result, "name");
         %descrip = sqlite.getColumn(%result, "description"); 
         
         //DatabaseSceneList.add(%name,%id);
         
         sqlite.nextRow(%result);
      }
   } 
   sqlite.clearResult(%result);
}*/

//Direct copy of EditorSaveMission from menuHandlers.ed.cs. This version exists
//because mission save is actually just SimObject::save, and that is way too deep 
//into T3D to be making application level changes. Instead, we just call this one,
//but we are still going to have problems with all the plugins until we keep them 
//from calling MissionGroup.save() on their own.
function openSimEarthSaveMission()
{
      // just save the mission without renaming it
   if(isFunction("getObjectLimit") && MissionGroup.getFullCount() >= getObjectLimit())
   {
      MessageBoxOKBuy( "Object Limit Reached", "You have exceeded the object limit of " @ getObjectLimit() @ " for this demo. You can remove objects if you would like to add more.", "", "Canvas.showPurchaseScreen(\"objectlimit\");" );
      return;
   }
   
   // first check for dirty and read-only files:
   if((EWorldEditor.isDirty || ETerrainEditor.isMissionDirty) && !isWriteableFileName($Server::MissionFile))
   {
      MessageBox("Error", "Mission file \""@ $Server::MissionFile @ "\" is read-only.  Continue?", "Ok", "Stop");
      return false;
   }
   if(ETerrainEditor.isDirty)
   {
      // Find all of the terrain files
      initContainerTypeSearch($TypeMasks::TerrainObjectType);

      while ((%terrainObject = containerSearchNext()) != 0)
      {
         if (!isWriteableFileName(%terrainObject.terrainFile))
         {
            if (MessageBox("Error", "Terrain file \""@ %terrainObject.terrainFile @ "\" is read-only.  Continue?", "Ok", "Stop") == $MROk)
               continue;
            else
               return false;
         }
      }
   }
  
   // now write the terrain and mission files out:
   
   
   ///////////////////////////   
   //For openSimEarth, we need to save many things to the database instead of to 
   //the mission. Starting with TSStatics.
   $tempStaticGroup = new SimSet();
   $tempRoadGroup = new SimSet();
   $tempForestGroup = new SimSet();
   
   if ($pref::OpenSimEarth::saveStatics)
      osePullStaticsAndSave($tempStaticGroup);
   else
      osePullStatics($tempStaticGroup);
   
   
   //TEMP - should actually define this so it's possible to save to mission
   if ($pref::OpenSimEarth::saveRoads)
      osePullRoadsAndSave($tempRoadGroup);
   else
      osePullRoads($tempRoadGroup);
   
   
   //if ($pref::OpenSimEarth::saveForests==false)
   //   osePullForest($tempForestGroup);
   
   
   if(EWorldEditor.isDirty || ETerrainEditor.isMissionDirty)
      MissionGroup.save($Server::MissionFile);
      
   osePushStatics($tempStaticGroup);
   osePushRoads($tempRoadGroup);
   //osePushForest($tempForestGroup);
   ///////////////////////////   
   
   
   if(ETerrainEditor.isDirty)
   {
      // Find all of the terrain files
      initContainerTypeSearch($TypeMasks::TerrainObjectType);

      while ((%terrainObject = containerSearchNext()) != 0)
         %terrainObject.save(%terrainObject.terrainFile);
   }

   ETerrainPersistMan.saveDirty();
      
   // Give EditorPlugins a chance to save.
   for ( %i = 0; %i < EditorPluginSet.getCount(); %i++ )
   {
      %obj = EditorPluginSet.getObject(%i);
      if ( %obj.isDirty() )
         %obj.onSaveMission( $Server::MissionFile );      
   } 
   
   EditorClearDirty();
   
   EditorGui.saveAs = false;
   
   return true;
}

function osePullStatics(%simGroup)
{//So, here we need to remove objects from the MissionGroup and put them into another simGroup.
   for (%i = 0; %i < MissionGroup.getCount();%i++)
   {
      %obj = MissionGroup.getObject(%i);  
      if (%obj.getClassName()$="TSStatic")
         %simGroup.add(%obj);
   }
   for (%i = 0; %i < %simGroup.getCount();%i++)
      MissionGroup.remove(%simGroup.getObject(%i));
}

function osePullStaticsAndSave(%simGroup)
{
   theTP.saveStaticShapes();
   
   for (%i = 0; %i < MissionGroup.getCount();%i++)
   {
      %obj = MissionGroup.getObject(%i);  
      if (%obj.getClassName()$="TSStatic")
      {
         %simGroup.add(%obj);
      }
   }
   
   for (%i = 0; %i < %simGroup.getCount();%i++)
   {
      MissionGroup.remove(%simGroup.getObject(%i));
   }
}

function osePushStatics(%simGroup)
{
   for (%i = 0; %i < %simGroup.getCount();%i++)
   {
      MissionGroup.add(%simGroup.getObject(%i));
   }
}

function osePullRoads(%simGroup)
{//So, here we need to remove objects from the MissionGroup and put them into another simGroup.
   for (%i = 0; %i < MissionGroup.getCount();%i++)
   {
      %obj = MissionGroup.getObject(%i);  
      if (%obj.getClassName()$="DecalRoad")// and/or MeshRoad
         %simGroup.add(%obj);
   }
   for (%i = 0; %i < %simGroup.getCount();%i++)
      MissionGroup.remove(%simGroup.getObject(%i));
}

function osePullRoadsAndSave(%simGroup)
{   
   theTP.saveRoads();
   
   for (%i = 0; %i < MissionGroup.getCount();%i++)
   {
      %obj = MissionGroup.getObject(%i);  
      if (%obj.getClassName()$="DecalRoad")// and/or MeshRoad
         %simGroup.add(%obj);
   }
   for (%i = 0; %i < %simGroup.getCount();%i++)
      MissionGroup.remove(%simGroup.getObject(%i));
}

function osePushRoads(%simGroup)
{
   for (%i = 0; %i < %simGroup.getCount();%i++)
   {
      MissionGroup.add(%simGroup.getObject(%i));
   }
}

function loadScene(%scene_id)
{
   //pdd(1);//physics debug draw
   
   %dyn = false;
   %grav = true;
   %ambient = true;
   
   	//%query = "SELECT ss.id as ss_id,shape_id,shapeGroup_id,behavior_tree," @ 
	         //"p.x as pos_x,p.y as pos_y,p.z as pos_z," @ 
	         //"sp.x as scene_pos_x,sp.y as scene_pos_y,sp.z as scene_pos_z," @ 
	         //"r.x as rot_x,r.y as rot_y,r.z as rot_z,r.angle as rot_angle, " @ 
	         //"FROM sceneShape ss " @ 
	         //"JOIN scene s ON s.id=" @ %scene_id @ " " @
	         //"LEFT JOIN vector3 p ON ss.pos_id=p.id " @ 
	         //"LEFT JOIN vector3 sp ON s.pos_id=sp.id " @ 
	         //"LEFT JOIN rotation r ON ss.rot_id=r.id " @ 
	         //"WHERE scene_id=" @ %scene_id @ ";";  

   //HERE: next step, we need to have a behavior tree name under sceneShape, and each
   //new shape needs to have its behavior assigned at create time, here.
	%query = "SELECT ss.id as ss_id,shape_id,shapeGroup_id,behavior_tree," @ 
	         "p.x as pos_x,p.y as pos_y,p.z as pos_z," @ 
	         "sp.x as scene_pos_x,sp.y as scene_pos_y,sp.z as scene_pos_z,r.x as rot_x," @ 
	         "r.y as rot_y,r.z as rot_z,r.angle as rot_angle, sh.datablock as datablock " @ 
	         "FROM sceneShape ss " @ 
	         "JOIN scene s ON s.id=scene_id " @
	         "LEFT JOIN vector3 p ON ss.pos_id=p.id " @ 
	         "LEFT JOIN vector3 sp ON s.pos_id=sp.id " @ 
	         "LEFT JOIN rotation r ON ss.rot_id=r.id " @ 
	         "JOIN physicsShape sh ON ss.shape_id=sh.id " @ 
	         "WHERE scene_id=" @ %scene_id @ ";";  
	%result = sqlite.query(%query, 0);
	
	echo("calling loadScene, result " @ %result);
   echo( "Query: " @ %query );	
	
   if (%result)
   {
      while (!sqlite.endOfResult(%result))
      {
         %sceneShape_id = sqlite.getColumn(%result, "ss_id");   
         %shape_id = sqlite.getColumn(%result, "shape_id");
         %shapeGroup_id = sqlite.getColumn(%result, "shapeGroup_id");//not used yet
         %behaviorTree = sqlite.getColumn(%result, "behavior_tree");
         
         %pos_x = sqlite.getColumn(%result, "pos_x");
         %pos_y = sqlite.getColumn(%result, "pos_y");
         %pos_z = sqlite.getColumn(%result, "pos_z");
         
         %scene_pos_x = sqlite.getColumn(%result, "scene_pos_x");
         %scene_pos_y = sqlite.getColumn(%result, "scene_pos_y");
         %scene_pos_z = sqlite.getColumn(%result, "scene_pos_z");
         
         %rot_x = sqlite.getColumn(%result, "rot_x");
         %rot_y = sqlite.getColumn(%result, "rot_y");
         %rot_z = sqlite.getColumn(%result, "rot_z");
         %rot_angle = sqlite.getColumn(%result, "rot_angle");
         
         %datablock = sqlite.getColumn(%result, "datablock");
         
         echo("Found a sceneShape: " @ %sceneShape_id @ " " @ %pos_x @ " " @ %pos_y @ " " @ %pos_z @
                " scenePos " @ %scene_pos_x @ " " @ %scene_pos_y @ " " @ %scene_pos_z );
                
         %position = (%pos_x + %scene_pos_x) @ " " @ (%pos_y + %scene_pos_y) @ " " @ (%pos_z + %scene_pos_z);
         %rotation = %rot_x @ " " @ %rot_y @ " " @ %rot_z @ " " @ %rot_angle;
         
         echo("loading scene, shape id " @ %shape_id @ " datablock " @ %datablock);
         //FIX!!!
         //if (%shape_id==1)
         //   %datablock = "M4Physics";
         //else if (%shape_id==2)
         //   %datablock = "DragonflyPhysics";
         //else if (%shape_id==3)
         //   %datablock = "bo105Physics";
         
         //TEMP
         %name = "";          
         if (%shape_id==4)
            %name = "ka50";   
         else if (%shape_id==3)
            %name = "bo105";
         else if (%shape_id==2)
            %name = "dragonfly";
            
            
         %temp =  new PhysicsShape(%name) {
            playAmbient = %ambient;
            dataBlock = %datablock;
            position = %position;
            rotation = %rotation;
            //scale = "0.5 0.5 0.5";
            canSave = "1";
            canSaveDynamicFields = "1";
            areaImpulse = "0";
            damageRadius = "0";
            invulnerable = "0";
            minDamageAmount = "0";
            radiusDamage = "0";
            hasGravity = %grav;
            isDynamic = %dyn;
            sceneShapeID = %sceneShape_id;
            sceneID = %scene_id;
            targetType = "Health";//"AmmoClip"
         };
         
         MissionGroup.add(%temp);   
         SceneShapes.add(%temp);   
         echo("Adding a scene shape: " @ %sceneShape_id @ ", position " @ %position );
         
         if (strlen(%behaviorTree)>0)
         {
            %temp.schedule(30,"setBehavior",%behaviorTree);
            echo(%temp.getId() @ " assigning behavior tree: " @ %behaviorTree );
         }

         sqlite.nextRow(%result);
      }
   }   
   sqlite.clearResult(%result);
   //schedule(40, 0, "startRecording");
} 

//function testSpatialite()
//{
//   //%query = "CREATE TABLE spatialTest ( id INTEGER, name TEXT NOT NULL, geom BLOB NOT NULL);";
//   %query = "INSERT INTO spatialTest ( id , name, geom ) VALUES (1,'Test01',GeomFromText('POINT(1 2)'));";
   
//   %result = sqlite.query(%query, 0);
	
//   if (%result)
//      echo("spatialite inserted into a table with a geom!");
//   else
//      echo("spatialite failed to insert into a table with a geom!  "  );
//}


function unloadScene(%scene_id)
{
   //HERE: look up all the sceneShapes from the scene in question, and drop them all from the current mission.
   %shapesCount = SceneShapes.getCount();
   for (%i=0;%i<%shapesCount;%i++)
   {
      %shape = SceneShapes.getObject(%i);  
      //echo("shapesCount " @ %shapesCount @ ", sceneShape id " @ %shape.sceneShapeID @ 
      //         " scene " @ %shape.sceneID ); 
      if (%shape.sceneID==%scene_id)
      {       
         MissionGroup.remove(%shape);
         SceneShapes.remove(%shape);//Wuh oh... removing from SceneShapes shortens the array...
         %shape.delete();//Maybe??
         
         %shapesCount = SceneShapes.getCount();
         if (%shapesCount>0)
            %i=-1;//So start over every time we remove one, until we loop through and remove none.
         else 
            %i=1;//Or else we run out of shapes, and just need to exit the loop.         
      }
   }   
}


function assignBehaviors()
{//This seems arbitrary, store initial behavior tree and dynamic status in sceneShape instead.
      
   for (%i=0;%i<SceneShapes.getCount();%i++)
   {
      %shape = SceneShapes.getObject(%i);  
      
      %shape.setBehavior("baseTree");
      
      //%shape.setDynamic(1);       
   }   
}

function startRecording()
{
   for (%i=0;%i<SceneShapes.getCount();%i++)
   {
      %shape = SceneShapes.getObject(%i);  
      %shape.setIsRecording(true);
   }   
}

function stopRecording()
{
   for (%i=0;%i<SceneShapes.getCount();%i++)
   {
      %shape = SceneShapes.getObject(%i);  
      %shape.setIsRecording(false);
   }   
}

function makeSequences()
{
   //OKAY... here we go. We now need to:
   // a) find our model's home directory   
   // b) in that directory, create a new directory with a naming protocol
   //       "scene_[%scene_id].[timestamp]"?
   // c) fill it with sequences
   
   //For now, just "workSeqs", if name changes we'll have to update M4.cs every time.
   %dirPath = %shape.getPath() @ "/scenes";
   createDirectory(%dirPath);//make shape/scenes folder first, if necessary.
   %dirPath = %shape.getPath() @ "/scenes/" @ %shape.sceneID ;//then make specific scene folder.
   for (%i=0;%i<SceneShapes.getCount();%i++)
   {
      %shape = SceneShapes.getObject(%i);  
      %dirPath = %shape.getPath() @ "/scenes/" @ %shape.sceneID ;
      %shape.makeSequence(%dirPath @ "/" @ %shape.getSceneShapeID());
   }
}

function loadOSM()  // OpenStreetMap XML data
{
   //here, read lat/long for each node as we get to it, convert it to xyz coords,
   //and save it in an array, to be used in the DecalRoad declaration.    
   
   %beforeTime = getRealTime();
   
   theTP.loadOSM($pref::OpenSimEarth::OSM,$pref::OpenSimEarth::OSMDB);     
   //theTP.loadOSM("min.osm");     
   //theTP.loadOSM("kincaid_map.osm");  
   //theTP.loadOSM("central_south_eug.osm");  
   //theTP.loadOSM("thirtieth_map.osm");
   //theTP.loadOSM("all_eugene.osm");  
   
   %loadTime = getRealTime() - %beforeTime;
   echo("OpenStreetMap file load time: " @ %loadTime );
}

function makeStreets()
{
   %mapDB = new SQLiteObject();
   %dbname = $pref::OpenSimEarth::OSMDB;//HERE: need to find this in prefs or something.
   %result = %mapDB.openDatabase(%dbname);
   //echo("tried to open osmdb: " @ %result);
   
   %query = "SELECT osmId,type,name FROM osmWay;";  
	%result = %mapDB.query(%query, 0);
   if (%result)
   {	   
      while (!%mapDB.endOfResult(%result))
      {
         %wayId = %mapDB.getColumn(%result, "osmId");
         %wayType = %mapDB.getColumn(%result, "type");         
         %wayName = %mapDB.getColumn(%result, "name");
         echo("found a way: " @ %wayName @ " id " @ %wayId);
         if ((%wayType $= "residential")||
               (%wayType $= "tertiary")||
               (%wayType $= "trunk")||
               (%wayType $= "trunk_link")||
               (%wayType $= "motorway")||
               (%wayType $= "motorway_link")||
               (%wayType $= "service")||
               (%wayType $= "footway")||
               (%wayType $= "path")||
               (%wayType $= "track"))
         {   
            
            //Width
            %roadWidth = 10.0;       
            if ((%wayType $= "tertiary")||(%wayType $= "trunk_link"))
               %roadWidth = 18.0; 
            else if ((%wayType $= "trunk")||(%wayType $= "motorway_link"))
               %roadWidth = 32.0; 
            else if (%wayType $= "motorway")
               %roadWidth = 40.0; 
            else if (%wayType $= "footway")
               %roadWidth = 2.5; 
            else if ((%wayType $= "path")||(%wayType $= "track"))
               %roadWidth = 5.0; 
            
            //Material
            %roadMaterial = "DefaultDecalRoadMaterial";
            if (%wayType $= "footway")
               %roadMaterial = "DefaultRoadMaterialPath";
            else if ((%wayType $= "service")||(%wayType $= "path"))
               %roadMaterial = "DefaultRoadMaterialOther";
               
            //now, query the osmWayNode and osmNode tables to get the list of points
            %node_query = "SELECT wn.nodeId,n.latitude,n.longitude,n.type,n.name from " @ 
                           "osmWayNode wn JOIN osmNode n ON wn.nodeId = n.osmId " @
                           "WHERE wn.wayID = " @ %wayId @ ";";
            %result2 = %mapDB.query(%node_query, 0);
            if (%result2)
            {	   
               //echo("query2 results: " @ mapDB.numRows(%result2));
               %nodeString = "";
               while (!%mapDB.endOfResult(%result2))
               {
                  %nodeId = %mapDB.getColumn(%result2, "nodeId");
                  %latitude = %mapDB.getColumn(%result2, "latitude");
                  %longitude = %mapDB.getColumn(%result2, "longitude");
                  %pos = theTP.convertLatLongToXYZ(%longitude @ " " @ %latitude @ " 0.0");
                  %type = %mapDB.getColumn(%result2, "type");         
                  %name = %mapDB.getColumn(%result2, "name");               
                  echo("  Node " @ %nodeId @ " longitude " @ %longitude @ " latitude " @ %latitude @ 
                       " type " @ %type @ " name " @ %name );
                  //%nodeString = %nodeString @ " Node = \"" @ %pos @ " " @ %roadWidth @ " 2 0 0 1\";";//2 = road depth, fix                  
                  %nodeString = %nodeString @ " Node = \"" @ %pos @ " " @ %roadWidth @ "\";";                  
                  %mapDB.nextRow(%result2);
               }            
               %mapDB.clearResult(%result2);
            }
            //Node = "-2263.4 -2753.58 233.796 10 5 0 0 1";
           // " Node = \"0.0 0.0 300.0 30.000000\";" @
            echo( %nodeString );
            //Then, do the new DecalRoad, execed in order to get a loop into the declaration.
            
            %roadString = "      new DecalRoad() {" @
               " InternalName = \"" @ %wayId @ "\";" @
               " Material = \"" @ %roadMaterial @ "\";" @
               " textureLength = \"25\";" @
               " breakAngle = \"3\";" @
               " renderPriority = \"10\";" @
               " position = \"" @ %pos @ "\";" @ //Better position of last node than nothing, I guess.
               " rotation = \"1 0 0 0\";" @
               " scale = \"1 1 1\";" @
               " canSave = \"1\";" @
               " canSaveDynamicFields = \"1\";" @
               %nodeString @
            "};";
            /*
            %roadString = "      new MeshRoad() {" @
            " topMaterial = \"DefaultRoadMaterialTop\";" @
            " bottomMaterial = \"DefaultRoadMaterialOther\";" @
            " sideMaterial = \"DefaultRoadMaterialOther\";" @
            " textureLength = \"5\";" @
            " breakAngle = \"3\";" @
            " widthSubdivisions = \"0\";" @
            " position = \"-2263.4 -2753.58 233.796\";" @
            " rotation = \"1 0 0 0\";" @
            " scale = \"1 1 1\";" @
            " canSave = \"1\";" @
            " canSaveDynamicFields = \"1\";" @
             %nodeString @
            "};";
         */
            eval(%roadString); 
         }
         
         %mapDB.nextRow(%result);
      }
      %mapDB.clearResult(%result);
   } else echo ("no results.");
   
   %mapDB.closeDatabase();
   %mapDB.delete();
}

/*
function streetMap()
 {   
    %xml = new SimXMLDocument() {};
    %xml.loadFile( "only_kincaid_map.osm" );
     
    // "Get" inside of the root element, "Students".     
    %result = %xml.pushChildElement("osm");  
    %version = %xml.attribute("version");     
    %generator = %xml.attribute("generator");      
    // "Get" into the first child element    
    %xml.pushFirstChildElement("bounds"); 
    %minlat = %xml.attribute("minlat");
    %maxlat = %xml.attribute("maxlat");
    echo("result: " @ %result @ " version: " @ %version @ ", generator " @ %generator @" minlat " @ %minlat @ " maxlat " @ %maxlat );
    while  (%xml.nextSiblingElement("node"))     
    {     
       %id = %xml.attribute("id"); 
       %lat = %xml.attribute("lat");     
       %lon = %xml.attribute("lon");    
       echo("node " @ %id @ " lat " @ %lat @ " long " @ %lon);   
       //HERE: store data in sqlite, and then read it back in the makeStreets function. 
       //Need at least a "way" table and a "node" table, plus other decorators I'm sure.
    } 
    %xml.nextSiblingElement("way");    
    echo("way: " @ %xml.attribute("id"));
    %xml.pushFirstChildElement("nd");
    echo("ref: " @ %xml.attribute("ref"));
    while (%xml.nextSiblingElement("nd")) 
    {
       echo("ref: " @ %xml.attribute("ref"));
    }
    while (%xml.nextSiblingElement("tag"))
    {
       echo("k: " @ %xml.attribute("k") @ "  v: " @ %xml.attribute("v") );
    }
    
 }  */
   
//Joint debugging, Chest Kinematic
function m4CK()
{
   $m4.setPartDynamic(2,0);
}
