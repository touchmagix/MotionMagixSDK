****Touchmagix Media Pvt. Ltd.****
==================================

Following SDK contain 2 games:
1.Angry balloon
2.Fruit Catch

Each content uses 3 base classes which handles communication between MotionMagix sytem and Content. They are:
1. MultiPoint.as
2. SinglePoint.as
3. TMDataObject.as

These also helps to process point and create object(blob) for further actions.
User can find this at " ..\com\touchMagix"  path and it contains following 

1. MultiPoint.as
Multipoint class written to handle Multi-Point gaming scenario.
It accepts data from socket sent by MotionMagix system and creates object from available points.
Depending upon stability of interaction points, it will decide object (blob) should exits or not in content. 

2. SinglePoint.as
It handle single object (blob) life cycle.

3. TMDataObject.as
It holds object structure and handle its local function calls.

### System Requirements  ###
1. Target : Falsh player 11.2 (or Latest Adobe Flash Player)
2. Platform Used : Adobe Flash CS6
3. Programming Language : Actionscript 3.0

**Note : You need to download and install latest version of Adobe Flash Player in order to run these contents. 