using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class AvatarSelectButton : Button
    {
        #region Fields
        Avatar avatar = null;
        public Avatar Avatar
        {
            get { return avatar; }
            set { avatar = value; }
        }
        #endregion


        public AvatarSelectButton(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, Texture texture, ClickHandler clickHandler, Avatar avatar)
        : base(lowerLeftCorner, upperRightCorner, texture, clickHandler)
        {
           
            this.avatar = avatar;
        }
    }
}
