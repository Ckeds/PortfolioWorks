using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ForeignStars
{
    //Acquired from StackOverflow, based on some sample code someone posted

    /// <summary>
    /// An interface that indicates a class is focusable upon by the camera
    /// 
    /// @author: Zachary Behrmann
    /// </summary>
    public interface IFocusable
    {
        Point Position2 { get; }
        Vector2 Position { get; }
        Rectangle RectPos { get; set; }
        Rectangle ActualPosition { get; }
    }
    /// <summary>
    /// An interface that indicates items a camera needs to implement
    /// 
    /// @author: Zachary Behrmann
    /// </summary>
    public interface ICamera2D
    {
        /// <summary>
        /// Gets or sets the position of the camera
        /// </summary>
        /// <value>The position.</value>
        Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the move speed of the camera.
        /// The camera will tween to its destination.
        /// </summary>
        /// <value>The move speed.</value>
        float MoveSpeed { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the camera.
        /// </summary>
        /// <value>The rotation.</value>
        float Rotation { get; set; }

        /// <summary>
        /// Gets the origin of the viewport (accounts for Scale)
        /// </summary>        
        /// <value>The origin.</value>
        Vector2 Origin { get; }

        /// <summary>
        /// Gets or sets the scale of the Camera
        /// The scalev is similar to a "zoom"
        /// </summary>
        /// <value>The scale.</value>
        float Scale { get; set; }

        /// <summary>
        /// Gets the screen center (does not account for Scale)
        /// </summary>
        /// <value>The screen center.</value>
        Vector2 ScreenCenter { get; }
        /*
                /// <summary>
                /// Gets the transform that can be applied to 
                /// the SpriteBatch Class.
                /// </summary>
                /// <see cref="SpriteBatch"/>
                /// <value>The transform.</value>
                Matrix Transform { get; }
                */
        /// <summary>
        /// Gets or sets the focus of the Camera.
        /// </summary>
        /// <seealso cref="IFocusable"/>
        /// <value>The focus.</value>
        IFocusable Focus { get; set; }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if the target is in view at the specified position; otherwise, <c>false</c>.
        /// </returns>
        bool IsInView(Rectangle position, Rectangle texture);
    }

    /// <summary>
    /// A camera that provides a transformation matrix for spriteBatch to use
    /// 
    /// @author: Zachary Behrmann
    /// </summary>
    public class Camera2D : Microsoft.Xna.Framework.GameComponent, ICamera2D
    {
        private Vector2 _position;
        protected float _viewportHeight;
        protected float _viewportWidth;
        protected int thresholdX;
        protected int thresholdY;
        public Camera2D(Game game)
            : base(game)
        { }

        #region Properties

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public float Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public float Scale { get; set; }
        public Vector2 ScreenCenter { get; protected set; }
        public Matrix Transform { get; set; }
        public IFocusable Focus { get; set; }
        public float MoveSpeed { get; set; }



        #endregion

        /// <summary>
        /// Called when the GameComponent needs to be initialized. 
        /// </summary>
        public override void Initialize()
        {
            _viewportWidth = Game.GraphicsDevice.Viewport.Width;
            _viewportHeight = Game.GraphicsDevice.Viewport.Height;

            ScreenCenter = new Vector2(_viewportWidth / 2, _viewportHeight / 2);

            Scale = 1;

            thresholdX = 32;
            thresholdY = 32;

            MoveSpeed = 7f;
            Rotation = 0f;

            base.Initialize();
        }

        /// <summary>
        /// Resets the position of the camera when a battle starts.
        /// 
        /// @author Ryan Conrad
        /// </summary>
        public void ResetPosition()
        {
            // sets the camera to the desired spot at the beginning of the battle (I prefer this over scrolling to that spot from (0,0) at the beginning.
            _position.X = (float)Focus.RectPos.X;
            _position.Y = (float)Focus.RectPos.Y;
        }

        public override void Update(GameTime gameTime)
        {
            Origin = ScreenCenter / Scale; // This being below Transform is what was causing that blasted one frame stutter at the beginning of each battle.

            // Move the Camera to the position that it needs to go
            var delta = (float)gameTime.ElapsedGameTime.TotalSeconds;

            //This below code is amazing, the movement is so smooth I LOVE IT
            _position.X += ((float)Focus.RectPos.X - Position.X) * MoveSpeed * delta;
            _position.Y += ((float)Focus.RectPos.Y - Position.Y) * MoveSpeed * delta;

            // Create the Transform used by any
            // spritebatch process
            Transform = Matrix.Identity *
                        Matrix.CreateTranslation(-Position.X, -Position.Y, 0) *
                        Matrix.CreateRotationZ(Rotation) *
                        Matrix.CreateTranslation(Origin.X, Origin.Y, 0) *
                        Matrix.CreateScale(new Vector3(Scale, Scale, Scale));

            //_position.X += (Focus.Position.X - Position.X); //* MoveSpeed * delta;
            //_position.Y += (Focus.Position.Y - Position.Y); //* MoveSpeed * delta;

            //Currently using points to get the position as opposed to Vector2
            //This code was done through half tutorial, half stack overflow
            //_position.X += ((float)Focus.Position2.X - Position.X); //* MoveSpeed * delta;
            //_position.Y += ((float)Focus.Position2.Y - Position.Y); //* MoveSpeed * delta;

            //This code is a solution, however it causes the awkward jump of the camera, the code below this is much better
            //_position.X += ((float)Focus.RectPos.X - Position.X); //* MoveSpeed * delta;
            //_position.Y += ((float)Focus.RectPos.Y - Position.Y); //* MoveSpeed * delta;

            base.Update(gameTime);
        }

        /// <summary>
        /// Determines whether the target is in view given the specified position.
        /// This can be used to increase performance by not drawing objects
        /// directly in the viewport
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="texture">The texture.</param>
        /// <returns>
        ///     <c>true</c> if [is in view] [the specified position]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInView(Rectangle position, Rectangle texture)
        {
            // If the object is not within the horizontal bounds of the screen

            if ((position.X + texture.Width) < (Position.X - Origin.X) || (position.X) > (Position.X + Origin.X))
                return false;

            // If the object is not within the vertical bounds of the screen
            if ((position.Y + texture.Height) < (Position.Y - Origin.Y) || (position.Y) > (Position.Y + Origin.Y))
                return false;

            // In View
            return true;
        }
    }
}
