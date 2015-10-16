package  
{
	import com.touchMagix.*;
	import com.greensock.*;
	import utils.RandomPlus;
	import flash.display.MovieClip;
	import flash.events.Event;
	import flash.events.TimerEvent;
	import flash.utils.Timer;
	import flash.utils.setTimeout;
	import flash.media.SoundMixer;
	import flash.media.Sound;
	import flash.media.SoundChannel;
	import flash.media.SoundTransform;
	
	public class Main extends MovieClip 
	{
		private var tmData:SinglePoint;
		private var objFruitMc:FruitMc;
		private var timer:Timer;
		private var timer2:Timer;
		private var fruitsArray:Array;
		private var gravity:Number;
		private var score:Number;
		private var objCatch_sound:Catch_sound;
		private var objBgSound:BgSound;
		private var objOhh_Sound:Ohh_Sound;
		private var objYiepee_Sound:Yiepee_Sound;
		private var st:SoundTransform = new SoundTransform(0.2);
		private var objS1:StawberrySound;
		private var objS2:GrapesSound;
		private var objS3:MangoSound;
		private var objS4:AppleSound;
		private var soundArray:Array;
		private var bgSoundChannel:SoundChannel;
		private var totalTimeInSec:Number;
		private var tmr:Timer;
		private var count:Number;
		private var frameNum:Number;
		private var rp:RandomPlus;
		var isGamePlaying: Boolean= false;
		
		public function Main() 
		{
			//Initialization
			init();
		}
		private function init()
		{
			//Screen Manager
			startScreen.visible = false;
			gameScreenMc.visible = false;
			gameOverScreenMc.visible = false;
			
			SoundMixer.stopAll(); // Stop all sounds
			
			gameScreenMc.score_panel.timerTxt.text = "60";
			objBgSound = new BgSound();//Background Music
			bgSoundChannel = new SoundChannel();
			bgSoundChannel = objBgSound.play();
			bgSoundChannel.soundTransform = st;
			bgSoundChannel.addEventListener(Event.SOUND_COMPLETE,onSoundCompleted);
			objCatch_sound = new Catch_sound();//Fruit Catching sound
			objYiepee_Sound = new Yiepee_Sound();//Sound after catching right fruit
			objOhh_Sound = new Ohh_Sound();//Sound after catching wrong fruit
			
			objS1 = new StawberrySound();
			objS2 = new GrapesSound();
			objS3 = new MangoSound();
			objS4 = new AppleSound();
			soundArray = new Array(objS1,objS2,objS3,objS4);
			score = 0 ;
			gravity =  0.025;
			fruitsArray = new Array();

			addChild(gameScreenMc.foodContainerMc);
			
			//socket initialization
			tmData = new SinglePoint();
			tmData.socketNumber = 12345;
			tmData.init(true);//Pass "true" for socket,"false" for mouse 
			this.addChild(tmData);
			
			rp =new RandomPlus(1,4);
			trace(rp);
			
			welcomeGame();
		}
		private function welcomeGame()
		{
			//Start screen
			startScreen.visible = true;
			gameScreenMc.visible = false;
			gameOverScreenMc.visible = false;
			
			timer = new Timer(1000);
			timer.addEventListener(TimerEvent.TIMER,onTick);
			
			timer2 = new Timer(9000);
			timer2.addEventListener(TimerEvent.TIMER,onTick2);
			this.addEventListener(Event.ENTER_FRAME,onLoop);
			
			tmr = new Timer(1000);
			tmr.addEventListener(TimerEvent.TIMER, onTick1);
			
			setTimeout(startGame,3000);
		}
		private function startGame()
		{
			//starting game play
			score=0;
			startScreen.visible = false;
			gameScreenMc.visible = true;
			gameOverScreenMc.visible = false;
			totalTimeInSec = 20;//Game time
			 tmr.start();
			 timer2.start();
			 timer.start();
			 isGamePlaying=true;
			setMode();
			gameScreenMc.score_panel.score_txt.text=String(score);
		}
		private function onTick1(event:TimerEvent=null):void
		{
			//Game time counter
		 	totalTimeInSec--;
			gameScreenMc.score_panel.timerTxt.text = String(totalTimeInSec);
			if(totalTimeInSec == 0)
			{
				tmr.stop();
				timer.stop();
				timer2.stop();
				gameOver();
			}
		}
		private function onSoundCompleted(event:Event)
		{
			bgSoundChannel = objBgSound.play();
			bgSoundChannel.soundTransform = st;
			bgSoundChannel.addEventListener(Event.SOUND_COMPLETE,onSoundCompleted);
		}
		private function onTick2(e:TimerEvent)
		{
			setMode();
		}
		private function setMode() 
		{
			count = rp.getNum();
			trace(count);
			frameNum = count
			gameScreenMc.fruitMc.gotoAndStop(frameNum);
			soundArray[count-1].play();
		}
		private function onTick(e:TimerEvent)
		{
			//creation of fruits
			createFruits();
		}
		private function createFruits()
		{
			objFruitMc = new FruitMc();
			fruitsArray.push(objFruitMc);
			objFruitMc.x = randomNumber(objFruitMc.width/2,stage.stageWidth - objFruitMc.width/2);
			objFruitMc.y = -objFruitMc.height;
			
			objFruitMc.speedY = randomNumber(2,4);
			objFruitMc.isHited = false;
			objFruitMc.isMissed = false;
			objFruitMc.scaleX = 0.9;
			objFruitMc.scaleY = 0.9;
			if(randomNumber(1,2)==1)
			{
				objFruitMc.gotoAndStop(frameNum);
			}else
			{
				objFruitMc.gotoAndStop(randomNumber(1,4));
			}
			gameScreenMc.foodContainerMc.addChild(objFruitMc);
		}
		private function onLoop(event:Event)
		{
			tmData.updateBlobs(); //updating blobs	
			if(isGamePlaying)
			{
				moveMixer();
				moveFruits();
				collisionCheck();
			}
		}
		//Mixer Movement
		private function moveMixer()
		{
			if(tmData._blob != null)
			{
				if(tmData._blob.x <= tmData._blob.width/2)
				{
					tmData._blob.x = tmData._blob.width/2;
					gameScreenMc.mixerMc.x = gameScreenMc.mixerMc.width/2;
				}
				else if(tmData._blob.x >= stage.stageWidth - tmData._blob.width/2)
				{
					tmData._blob.x = stage.stageWidth - tmData._blob.width/2;
					gameScreenMc.mixerMc.x = gameScreenMc.mixerMc.stage.stageWidth - gameScreenMc.mixerMc.width/2;
				}
				TweenLite.to(gameScreenMc.mixerMc,0.6,{x:tmData._blob.x});
			}
		}
		//Fruits Movement
		private function moveFruits()
		{
			for (var i:Number = 0; i < fruitsArray.length; i++)
			{
				if(!fruitsArray[i].isHit)
				{
					fruitsArray[i].speedY += gravity;
					fruitsArray[i].y +=  fruitsArray[i].speedY;
				}
			}
		}
		//Checking collision between fruits and mixer
		private function collisionCheck()
		{
			for (var i:Number = 0; i < fruitsArray.length; i++)
			{
				if(gameScreenMc.mixerMc.hitMc.hitTestObject(fruitsArray[i])&& !fruitsArray[i].isHited)
				{
					fruitsArray[i].isHited = true;
					objCatch_sound.play();
					if(count == fruitsArray[i].currentFrame)
					{
						gameScreenMc.mixerMc.gotoAndPlay(2);
						gameScreenMc.jar.gotoAndStop(gameScreenMc.jar.currentFrame+1);
						objYiepee_Sound.play();
						//trace("right");
						score++;
					}else
					{
						//trace("wrong");
						objOhh_Sound.play();
					}
					gameScreenMc.score_panel.score_txt.text = String(score);
					gameScreenMc.foodContainerMc.removeChild(fruitsArray[i]);
					fruitsArray.splice(i,1);
				}else
				{
					//trace("Called...");
				}
				
			}
		}
		//Game over screen
		private function gameOver()
		{
			isGamePlaying=false;
			gameScreenMc.visible = false;
			gameOverScreenMc.visible = true;
			
			gameOverScreenMc.score_txt.text = String(score);
			
			fruitsArray.splice(0);
			while(gameScreenMc.foodContainerMc.numChildren)
			{
				gameScreenMc.foodContainerMc.removeChildAt(0);
			}
			setTimeout(welcomeGame,3000);
		}
		//Generate Random Number;
		private function randomNumber(Low:Number,High:Number):Number
		{
			return (Math.floor(Math.random()*(1+High-Low))+Low);
		}
	}
}