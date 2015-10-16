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
	import flash.events.KeyboardEvent;
	import flash.display.Stage;
	import flash.ui.Mouse;
	import flash.utils.*;

	public class SinglePoint extends MovieClip
	{
		private var myTimer:Timer;
		private var delay:Number = 250;

		private var xmlSocket:XMLSocket;
		
		public var xpos:Number = 0;
		public var ypos:Number = 0;
		public var zpos:Number = 0;
		public var DirectionMode:Number = 1;
		
		private var oldXpos:Number = 0;
		private var oldYpos:Number = 0;
		private var blobRemoveTimeInSecond:Number = 2;
		private var seconds:Number = 0;
		private var blobWidth:Number = 100;
		private var blobHeight:Number = 50;
		
		//public var blob:MovieClip;
		public var socketNumber:Number = 12345;
		public var socketPort:String = "localhost";
		public var _blob:TMDataObject;

		private var isX:Boolean = true;
		private var isY:Boolean = true;

		private var isSocket:Boolean = false;
		private var myStage:Stage;
		
		public var isPointComing:Boolean = false;
		public var resetCounter:Number = 0;
		public var oldX:Number;
		public var oldY:Number;
		
		public var xmlData:XML = new XML  ;
		public var debug:Boolean = false;
		public var blobAlpha:Number = 0;
		
		var isCtrl:Boolean=false;
		var isD:Boolean=false;
		
		public function init(isTrue:Boolean)
		{
			trace("init "+isTrue)
			isSocket = isTrue; 
			this.mouseEnabled = false;
			this.mouseChildren = false;
			this.addEventListener(Event.ADDED_TO_STAGE,onAddedToStage);
			
			Mouse.hide();
		}
		
		function onAddedToStage(event:Event)
		{
			 addListeners();
			 trace("SinglePoint added to stage");
		}
		
		private function addListeners()
		{
			xpos = this.stage.width / 2;
			ypos = this.stage.height / 2;
			
			if (isSocket)
			{
				socketInit();
				
			}
			else
			{
				this.stage.addEventListener(MouseEvent.MOUSE_MOVE,onMouse);
				this.stage.addEventListener(MouseEvent.MOUSE_DOWN,onDown);
				this.stage.addEventListener(MouseEvent.MOUSE_UP,onUp);	
			}
			
			this.stage.addEventListener(KeyboardEvent.KEY_DOWN,onKeyPressed);
			this.stage.addEventListener(KeyboardEvent.KEY_UP,onKeyReleased);
		}
		
		private function onDown(event:MouseEvent)
		{
			DirectionMode = 2;
		}
		private function onUp(event:MouseEvent)
		{
			DirectionMode = 1;
		}
		
		
		private function onKeyPressed(event:KeyboardEvent)
		{
			
			//trace("Key Pressed : "+event.keyCode);
			
			if(event.keyCode==17)
			{
				isCtrl=true;
			}
			if(event.keyCode==68)
			{
				isD=true;
			}
			
			//trace("isCtrl : "+isCtrl+" - isD : "+isD)
			
			if(isCtrl && isD)
			{
				trace("debug mode");
				if(debug)
				{
					//blobAlpha = 0;
					debug = false;
				}
				else
				{
					//blobAlpha = 0.2;
					debug = true;
				}
				
				if(_blob != null)
				{
					_blob.debugMode(debug);
					//trace("Debug Me")
				}
			}
		}
		private function onKeyReleased(event:KeyboardEvent)
		{
			
			if(event.keyCode==17)
			{
				isCtrl=false;
			}
			if(event.keyCode==68)
			{
				isD=false;
			}
		}
		private function socketInit()
		{
			//Connecting socket;
			xmlSocket = new XMLSocket  ;
			xmlSocket.connect(socketPort,socketNumber);
			xmlSocket.addEventListener(DataEvent.DATA,onDataCome);
		}
		//Data Event
		private function onDataCome(event:DataEvent)
		{
			var str:String = event.data;
			var obj:XML = new XML(str);
			xpos = obj.child("object").child("x");
			ypos = obj.child("object").child("y");
			zpos = obj.child("object").child("z");
			DirectionMode = obj.child("object").child("DirectionMode");

			updateBlobLife();
		}
		//MouseEvent
		private function onMouse(event:MouseEvent)
		{
			xpos = this.stage.mouseX;
			ypos = this.stage.mouseY;

			updateBlobLife();
		}
		private function updateBlobLife()
		{
			if(_blob != null)
			{
				_blob.life = _blob.maxLife;
				dispatchEvent(new Event("BLOB_MOVED"));
			}
			else
			{
				createBlob();
			}
		}
		public function updateBlobs()
		{
			if(_blob != null)
			{
				_blob.olderX = _blob.oldX;
				_blob.olderY = _blob.oldY;
				_blob.oldX = _blob.x;
				_blob.oldY = _blob.y;
				_blob.life--;
				
				if (isX)
				{
					_blob.x = xpos;
				}
				if (isY)
				{
					_blob.y = ypos;
				}
				
				createEvents(xpos,ypos);
				//myTimer.start();
				checkBlobLife();
			}
		}
		private function checkBlobLife()
		{
			if (_blob.life <= 0 && _blob != null)
			{
				dispatchEvent(new Event("BLOB_REMOVED"));
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
		public function createBlob()
		{
			_blob = new TMDataObject(debug);
			_blob.oldX = _blob.x = xpos;
			_blob.oldY = _blob.y = ypos;
			
			addChild(_blob);
			
			dispatchEvent(new Event("BLOB_CREATED"));
		}
		public function removeBlob()
		{
			//seconds = 0;
			removeChild(_blob);
			_blob = null;
			dispatchEvent(new Event("BLOB_DELETED"));
			//myTimer.stop();
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