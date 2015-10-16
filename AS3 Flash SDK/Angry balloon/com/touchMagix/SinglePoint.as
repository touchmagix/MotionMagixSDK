//Organization : TouchMagix Media

//****This is a singlepoint class.This will generate single point. 
package com.touchMagix
{

	import flash.display.MovieClip;
	import flash.events.TimerEvent;
	import flash.utils.Timer;
	import flash.net.XMLSocket;
	import flash.events.DataEvent;
	import flash.events.Event;
	import flash.net.URLRequest;
	import flash.net.URLLoader;
	import flash.events.MouseEvent;
	import flash.display.Stage;
	import flash.ui.Mouse;

	public class SinglePoint extends MovieClip
	{
		private var myTimer:Timer;
		private var delay:Number = 100;
		private var xmlSocket:XMLSocket;
		private var xpos:Number;
		private var ypos:Number;
		private var oldXpos:Number = 0;
		private var oldYpos:Number = 0;
		private var blobRemoveTimeInSecond:Number = 5;
		private var seconds:Number = 0;
		private var blobWidth:Number = 100;
		private var blobHeight:Number = 50;
		private var blobAlpha:Number = 0.5;
		public var _blob:MovieClip;
		private var isX:Boolean = true;
		private var isY:Boolean = true;
		private var isSocket:Boolean = false;
		private var myStage:Stage;
		public var isPointComing:Boolean = false;
		public var resetCounter:Number = 0;
		public var oldX:Number;
		public var oldY:Number;
		public var xmlData:XML = new XML  ;

		public function SinglePoint(tempStage:Stage,isTrue:Boolean)
		{
			myStage = tempStage;
			isSocket = isTrue
			
			xpos = myStage.width / 2;
			ypos = myStage.height / 2;
			
			setConfigValues();
			
			if (isSocket)
			{
				socketInit();
				Mouse.hide();
			}
			else
			{
				myStage.addEventListener(MouseEvent.MOUSE_MOVE,onMouse);
			}
		}
		private function setConfigValues()
		{
			//Jump Trace timer;
			myTimer = new Timer(delay);
			myTimer.addEventListener(TimerEvent.TIMER,onTick);
		}
		private function socketInit()
		{
			//Connecting socket;
			xmlSocket = new XMLSocket  ;
			xmlSocket.connect("localhost",12345);
			xmlSocket.addEventListener(DataEvent.DATA,onDataCome);
		}
		//Data Event
		private function onDataCome(event:DataEvent)
		{
			var str:String = event.data;
			var obj:XML = new XML(str);
			xpos = obj.child("object").child("x");
			ypos = obj.child("object").child("y");
			gamePlay();
		}
		//MouseEvent
		private function onMouse(event:MouseEvent)
		{
			xpos = myStage.mouseX;
			ypos = myStage.mouseY;

			gamePlay();
		}
		private function gamePlay()
		{
			myTimer.stop();
			seconds = 0;
			
			if (this.numChildren == 0)
			{
				createBlob();
			}

			if (isX)
			{
				_blob.x = xpos;
			}
			if (isY)
			{
				_blob.y = ypos;
			}
			
			createEvents(xpos,ypos);
			myTimer.start();
		}
		//Jump reset timer
		private function onTick(event:TimerEvent)
		{
			seconds++;
			//trace("seconds : "+seconds+" blobRemoveTimeInSecond : "+blobRemoveTimeInSecond);

			if (seconds >= blobRemoveTimeInSecond && _blob != null)
			{
				removeBlob();
			}
		}
		public function createEvents(xpos:int,ypos:int)
		{
			if(_blob != null)
			{
				var distanceX:Number = xpos - oldX;
				var distanceY:Number = ypos - oldY;
				var distance:Number = Math.sqrt(distanceX*distanceX + distanceY*distanceY);
				var angleInRadians : Number = Math.atan2(distanceY, distanceX);
				var andleInDegrees : Number = angleInRadians * (180 / Math.PI);
				//trace("distance : "+distance+" : andleInDegrees : "+andleInDegrees+" : angleInRadians : "+angleInRadians);
				if (distance > 50)
				{
					
					
					if(andleInDegrees > 0 && andleInDegrees < 90)
					{
						//trace("Right Down swipe");
						dispatchEvent(new Event("RIGHT_DOWN_SWIPE"));
						
					}
					else if(andleInDegrees > 90 && andleInDegrees < 180)
					{
						//trace("Left Down swipe");
						dispatchEvent(new Event("LEFT_DOWN_SWIPE"));
					}
					else if(andleInDegrees < 0 && andleInDegrees > -90)
					{
						//trace("Right Top swipe");
						dispatchEvent(new Event("RIGHT_TOP_SWIPE"));
					}
					else if(andleInDegrees > -180 && andleInDegrees < -90)
					{
						//trace("Left Top swipe");
						dispatchEvent(new Event("LEFT_TOP_SWIPE"));
					}
					
					//trace("oldX : "+oldX+" xpos : "+xpos+" distance : "+distance+" andleInDegrees : "+andleInDegrees);
				}
			}
			else
			{
				trace("Empty");
			}
			
			oldX = xpos;
			oldY = ypos;
			
			resetCounter = 0;
			
		}
		//This function will create the blob.
		public function createBlob()
		{
			_blob = new MovieClip();
			addChild(_blob);
			_blob.graphics.beginFill(0xFF794B,0.4);
        	_blob.graphics.drawCircle(0, 0, blobWidth/2);
         	_blob.graphics.endFill();
			
			dispatchEvent(new Event("BLOB_CREATED"));
		}
		public function removeBlob()
		{
			seconds = 0;
			removeChild(_blob);
			_blob = null;
			dispatchEvent(new Event("BLOB_DELETED"));
			myTimer.stop();
		}

		//setter getter and public methods;
		public function get getPosX()
		{
			return xpos;
		}
		public function get getPosY()
		{
			return ypos;
		}


	}

}