using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Drunkar_Walk_Viewer
{
	public static class AssetManager
	{
		//Basic Point
		public static Texture2D PointTexture;

		public static void Load(MainGame mainGame)
		{
			ContentManager pContent = mainGame.Content;

			PointTexture = new Texture2D(mainGame.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			PointTexture.SetData(new[] { Color.White });
		}
	}
}
