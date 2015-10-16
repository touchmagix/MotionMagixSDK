//Organization : TouchMagix Media
//****This is a multipoint class.It will generate one or more points.
package com.touchMagix
{
	import flash.events.Event;
	import flash.events.DataEvent;
	import flash.events.MouseEvent;
	import flash.events.IOErrorEvent;
	import flash.events.ProgressEvent;
	import flash.events.SecurityErrorEvent;
	import flash.net.XMLSocket;
	import flash.display.MovieClip;
	import flash.events.*;
	import flash.ui.*;
	import flash.utils.*;
	
	public class MultiPoint extends MovieClip
	{
		private var Socket1:XMLSocket;
		private var INITIALIZED:Boolean;
		public var OBJECT_ARRAY:Array;
		private var minThreshold:Number = 40000;
		var xoffset:int = 0;
		var yoffset:int = 0;
		var isCtrl:Boolean=false;
		var isD:Boolean=false;
		public var xStartPos:Number;
		public var yStartPos:Number;
		public var removeIndex:Number;
		public var debug:Boolean= false;
		public var isSocket:Boolean = true;
		public function init(isSocket:Boolean)
		{
			this.isSocket = isSocket;
			
			if (INITIALIZED)
			{
				trace("TMData Already Initialized! : "+isSocket);
				return;
			}
			INITIALIZED = true;
			OBJECT_ARRAY = new Array();

			Mouse.hide();		
			
			setTimeout(addListeners,500);
		}
		function addListeners()
		{
			//trace(this.stage);
			if (isSocket)
			{
				Socket1 = new XMLSocket();

				Socket1.addEventListener(Event.CLOSE, closeHandler);
				Socket1.addEventListener(Event.CONNECT, connectHandler);
				Socket1.addEventListener(DataEvent.DATA, dataHandler1);
				Socket1.addEventListener(IOErrorEvent.IO_ERROR, ioErrorHandler);
				Socket1.connect("127.0.0.1", 12345);
				Mouse.hide();
			}
			else
			{
				this.stage.addEventListener(MouseEvent.MOUSE_MOVE,onMove);
			}
			this.stage.addEventListener(KeyboardEvent.KEY_DOWN,onKeyPressed);
			this.stage.addEventListener(KeyboardEvent.KEY_UP,onKeyReleased);
		}
		private function onKeyPressed(event:KeyboardEvent)
		{
			trace("Key Pressed : "+event.keyCode);
			if(event.keyCode==68)
			{
				isD=true;
			}
			
			if(event.keyCode==17)
			{
				isCtrl=true;
			}
			
			trace("isCtrl : "+isCtrl+" - isD : "+isD)
			
			if(isCtrl && isD)
			{
				if(debug)
				{
					debug = false;
				}
				else
				{
					debug = true;
				}
				for(var i:Number= 0; i<OBJECT_ARRAY.length; i++)
				{
					OBJECT_ARRAY[i].debugMode(debug);
				}
			}
		}
		private function onKeyReleased(event:KeyboardEvent)
		{
			if(event.keyCode==68)
			{
				isD=false;
			}
			
			if(event.keyCode==17)
			{
				isCtrl=false;
			}
		}
		private function closeHandler(e:Event)
		{
			trace("Socket Closed !!!");
		}
		private function connectHandler(e:Event)
		{
			trace("Socket Connected");
		}
		private function ioErrorHandler(event:IOErrorEvent):void
		{
			trace("ioErrorHandler: " + event);
		}
		private function dataHandler1(e:DataEvent)
		{
			var recd_xml:XML = XML(e.data);
			//trace(recd_xml);
			dataHandler(recd_xml, xoffset, yoffset);
		}
		//Getting points on Mouse Move
		private function onMove(event:MouseEvent)
		{
			addBlob(this.mouseX, this.mouseY);
		}
		//Getting points from Data 
		private function dataHandler(recd_xml:XML, xoffset:Number, yoffset:Number)
		{
			var coords:XMLList = recd_xml.children().children();
			var xpos:Number = parseInt(coords[0].children(),10) + xoffset;
			var ypos:Number = parseInt(coords[1].children(),10) + yoffset;
			addBlob(xpos, ypos);
		}
		private function addBlob(xpos:int, ypos:int):void
		{
			var minDist:Number = minThreshold;
			var minDistIndex:int = -1;
			var dx:Number;
			var dy:Number;
			var currDist:Number;
			for (var i:int = 0; i < OBJECT_ARRAY.length; i++)
			{
				dx = OBJECT_ARRAY[i].x - xpos;
				dy = OBJECT_ARRAY[i].y - ypos;
				currDist = (dx * dx) + (dy * dy);
				//trace("CurrDist"+currDist);
				if (currDist<minDist)
				{
					OBJECT_ARRAY[i].velocityX =  -  dx;
					OBJECT_ARRAY[i].velocityY =  -  dy;
					minDist = currDist;
					minDistIndex = i;
				}
			}

			if (minDistIndex==-1)
			{
				var tmdataobj:TMDataObject = new TMDataObject(debug);
				tmdataobj.x = xpos;
				tmdataobj.y = ypos;
				addChild(tmdataobj);
				OBJECT_ARRAY.push(tmdataobj);
				xStartPos = xpos;
				yStartPos = ypos;
				dispatchEvent(new Event("BLOB_ADDED"));
				//trace("New Blob Added");
			}
			else
			{
				OBJECT_ARRAY[minDistIndex].x = xpos;
				OBJECT_ARRAY[minDistIndex].y = ypos;
				//Revive life to 20 if blob is still on stage
				OBJECT_ARRAY[minDistIndex].life = OBJECT_ARRAY[minDistIndex].maxLife;
			}
		}
		public function updateBlobs()
		{
			//trace ("In remove : " + OBJECT_ARRAY.length);
			for (var i:int = 0; i < OBJECT_ARRAY.length; i++)
			{
				//trace ("    OBJECT_ARRAY[" + i + "] : " + OBJECT_ARRAY[i].life);
				OBJECT_ARRAY[i].life--;
				if (OBJECT_ARRAY[i].life == 0)
				{
					removeChild(OBJECT_ARRAY[i]);
					OBJECT_ARRAY.splice(i, 1);
					removeIndex = i;
					dispatchEvent(new Event("BLOB_REMOVED"));
					//trace("Blob Removed");
					i--;
				}
			}
		}
		public function returnBlobs():Array
		{
			updateBlobs();
			return OBJECT_ARRAY;
		}
	}
}