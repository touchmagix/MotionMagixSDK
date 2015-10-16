//Organization : TouchMagix Media

package com.touchMagix
{
	import flash.display.MovieClip;

	public class TMDataObject extends MovieClip 
	{		
		
		public var xPos:Number;
		public var yPos:Number;
		public var velocityX:Number = 0;
		public var velocityY:Number = 0;
		public var blobWidth:Number = 80;//100;
		private var color:Array = new Array ( 0xFFFFFF, 0xFF0000, 0x00FF00, 0x0000FF, 0x000000 );
		private var blobAlpha:Number = 0.5;
		public var life:int;
		
		
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// CONSTRUCTOR
//--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
		public function TMDataObject (debug:Boolean)
		{
			life = 3;//blob life which can be adjustible
			debugMode(debug);
			//createBlob();
		}
		
		public function debugMode(debug:Boolean)
		{
			if(debug)
			{
				blobAlpha = 0.5;
			}
			else
			{
				blobAlpha = 0;
			}
			if(this.numChildren > 0)
			{
				this.removeChildAt(0);
			}
			createBlob();
			
		}
		public function createBlob()
		{
			var _blob = new MovieClip();
			this.addChild(_blob);
			_blob.graphics.beginFill(color[randomNumber(color.length, 0)],blobAlpha);
        	_blob.graphics.drawCircle(0, 0, blobWidth/2);
         	_blob.graphics.endFill();
			
			//dispatchEvent(new Event("BLOB_CREATED"));
		}
		//Generate Random Number;
		function randomNumber(High:Number,Low:Number):Number
		{
			return (Math.floor(Math.random()*(1+High-Low))+Low);
		}
	}
}