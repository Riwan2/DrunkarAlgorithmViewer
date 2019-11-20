using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Drunkar_Walk_Viewer
{
	public class Walker
	{
		public int CurrentX { get; private set; }
		public int CurrentY { get; private set; }
		public bool Dead { get; private set; } = false;

		enum Direction
		{
			Up,
			Down,
			Right,
			Left
		}
		private Direction currentDirection;

		private void ChooseDirection()
		{
			switch (util.getInt(1, 4)) {
				case 1:
					currentDirection = Direction.Up;
					break;
				case 2:
					currentDirection = Direction.Right;
					break;
				case 3:
					currentDirection = Direction.Down;
					break;
				case 4:
					currentDirection = Direction.Left;
					break;
			}
		}

		public Walker(int pX, int pY)
		{
			CurrentX = pX;
			CurrentY = pY;
			ChooseDirection();
		}

		public void Update()
		{
			if (util.getInt(0, 100) < (int)(100 * Info.DestroyWalker)) {
				Dead = true;
			}
			if (!Dead) {
				if (util.getInt(0, 100) < (int)(100 * Info.DirectionChange)) {
					ChooseDirection();
				}
				if (currentDirection == Direction.Down && CurrentY < Info.MapHeight-2) CurrentY++;
				else if (currentDirection == Direction.Left && CurrentX > 2) CurrentX--;
				else if (currentDirection == Direction.Right && CurrentX < Info.MapWidth-2) CurrentX++;
				else if (currentDirection == Direction.Up && CurrentY > 2) CurrentY--;
			}
		}
	}

	public static class Info
	{
		//Proba
		public static float DirectionChange = 0.5f;
		public static float DestroyWalker = 0.02f;
		public static float CreateWalker = 0.4f;
		//Info
		public static int NbWalker = 1000;
		public static int MapWidth = 100;
		public static int MapHeight = 50;
		public static int NbTileDestroy = (int)((MapWidth * MapHeight) / 2f);
	}

	public class Generate
	{
		Primitive primitive;
		MainGame mainGame;
		//Keyboard
		KeyboardState newKeyState;
		KeyboardState oldKeyState;
		MouseState newMouseState;
		MouseState oldMouseState;
		//util
		float timer;
		double UpdateTime = 0.01;
		bool update = false;
		bool pause = false;
		bool endAlgo = false;
		//Map
		int CellWidth = 10;
		int CellHeight = 10;
		int Space = 0;
		int offsetX = 80;
		int offsetY = 100;

		int[,] Map;
		int currentX;
		int currentY;
		int nbTileDestroyed;
		int lastX;
		int lastY;

		List<Walker> Walkers;

		public Generate(MainGame pMainGame)
		{
			mainGame = pMainGame;
			primitive = new Primitive(mainGame.spriteBatch);
			Load();
		}

		private void Load()
		{
			oldKeyState = Keyboard.GetState();
			oldMouseState = Mouse.GetState();
			endAlgo = false;

			//Map
			Map = new int[Info.MapHeight, Info.MapWidth];
			for (int y = 0; y < Info.MapHeight; y++) {
				for (int x = 0; x < Info.MapWidth; x++) {
					Map[y, x] = 1;
				}
			}
			nbTileDestroyed= 0;

			//Walker
			Walkers = new List<Walker>();
			currentX = (int)(Info.MapWidth / 2);
			currentY = (int)(Info.MapHeight / 2);
			lastX = currentX;
			lastY = currentY;

			for (int i = 0; i < Info.NbWalker; i++) {
				Walkers.Add(new Walker(currentX, currentY));
			}
		}

		public void Update(GameTime gameTime)
		{
			newKeyState = Keyboard.GetState();
			newMouseState = Mouse.GetState();

			Restart(newKeyState, oldKeyState);
			Pause(newKeyState, oldKeyState);
			UpdateTimer(gameTime);

			if (update && !endAlgo) {
				foreach (Walker walker in Walkers) {
					if (nbTileDestroyed <= Info.NbTileDestroy) {
						if (Map[walker.CurrentY, walker.CurrentX] == 1) {
							Map[walker.CurrentY, walker.CurrentX] = 0;
							nbTileDestroyed++;
						}
					} else {
						endAlgo = true;
						break;
					}
					walker.Update();
					lastX = walker.CurrentX;
					lastY = walker.CurrentY;
				}
				Walkers.RemoveAll((Walker obj) => obj.Dead);
				if (util.getInt(0, 100) < (int)(100 * Info.CreateWalker)) {
					Walkers.Add(new Walker(lastX, lastY));
				}
			}

			if (endAlgo) {
				Walkers.Clear();
			}

			oldKeyState = Keyboard.GetState();
			oldMouseState = Mouse.GetState();
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			//Base Grid draw;
			for (int y = 0; y < Info.MapHeight; y++) {
				for (int x = 0; x < Info.MapWidth; x++) {
					Point pos = new Point(offsetX + x * (CellWidth + Space), offsetY + y * (CellHeight + Space));
					if (Map[y, x] == 1) {
						primitive.DrawRectangle(pos.X, pos.Y, CellWidth, CellHeight, Color.LightGray);
					}
				}
			}

			foreach (Walker walker in Walkers) {
				Point pos = new Point(offsetX + walker.CurrentX * (CellWidth + Space), offsetY + walker.CurrentY * (CellHeight + Space));
				primitive.DrawRectangle(pos.X, pos.Y, CellWidth, CellHeight, Color.Red);
			}
		}

		private void Init()
		{
			update = false;
			timer = 0;
			pause = false;
			Load();
		}

		private void Restart(KeyboardState pNewKeyState, KeyboardState pOldKeyState)
		{
			if (pNewKeyState.IsKeyDown(Keys.Enter) && pOldKeyState.IsKeyUp(Keys.Enter)) {
				Init();
			}
		}

		private void Pause(KeyboardState pNewKeyState, KeyboardState pOldKeyState)
		{
			if (pNewKeyState.IsKeyDown(Keys.Space) && pOldKeyState.IsKeyUp(Keys.Space)) {
				pause = !pause;
			}

		}

		private void UpdateTimer(GameTime gameTime)
		{
			timer += gameTime.ElapsedGameTime.Milliseconds;
			if (timer > UpdateTime && !pause) {
				timer = 0;
				update = true;
			} else {
				update = false;
			}
		}
	}
}
