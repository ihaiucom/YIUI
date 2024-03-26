## [plugin] ##
- Demo            	[demo project]
	- Resource     	[demo resource and generate json by editor]
	- Scene        	[demo scene]
	- Script       	[demo code]
- Editor          	[editor code]
- Runtime 			[action engine code]

## [deploy] ##
Enable USE_PLUGINWEAPONTRAIL macro in player settings when you getting this plugin.

## [other] ##
More about game develp at the [http://www.supercline.com](http://www.supercline.com)

## [quick start]
1. git clone https://github.com/SuperCLine/actioneditor.git
2. import CLine Action Editor 3.unitypackage
3. import demo.unitypackage (get resource from email or qq)

## [QQ Group: 134520584] ##

## [V3.0]
- using TAny to replace CustomProperty
- using PropertyName to define variable name
- fixed layout loading
- add event trigger type attribute to replace interface such as IEventWhenActionEndExecute
- add variable tag to replace count logic
- remove local player
- remove action break event instead of tag property and event
- add post update for pulse trigger such as input state
- add new ai framework
- add smooth rotation for player
- add animation cross fade time
- add new editor framework just like timeline
- support undo & redo & drag drog

## [V3.0.3]
- fixed track name not save
- fixed list enum type
- fixed KeyDown event
- using EditorGUILayout to draw inspector
- add event duplicate, copy & paste value
- add interrupt refresh

## [V3.0.4]
- auto reset timeline view with action
- auto named interrupt truck
- reset action total time by select animation event
- add inspector & property splitter
- searcher support

## [V3.0.5]
- fixed action id is null
- fixed search cancel button show
- fixed duration event right handle exceed total time
- remove path force lower limit, assign resRoot with your porject path at convient
- set json string to default empty