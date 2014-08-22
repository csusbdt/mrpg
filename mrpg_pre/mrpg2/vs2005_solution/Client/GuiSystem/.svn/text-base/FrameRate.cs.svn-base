using System;
using System.Collections.Generic;
using System.Text;

namespace Client
{
    class FrameRate : Text
    {
        double frameRateMovingAverage = 60.0;
        double weightOfNewFrame = 0.03;
        TimeSpan timeBetweenFrameRateAdjustments = TimeSpan.FromSeconds(1.0);
        TimeSpan timeToNextFrameRateAdjustment;

        public FrameRate(ScreenCoordinate lowerLeftCorner, ScreenCoordinate upperRightCorner, Texture texture)
            : base(lowerLeftCorner, upperRightCorner, texture, "fps: 60")
        {
            timeToNextFrameRateAdjustment = timeBetweenFrameRateAdjustments;
        }

        public override void Update(TimeSpan dt)
        {
            frameRateMovingAverage =
                frameRateMovingAverage * (1 - weightOfNewFrame) + weightOfNewFrame / dt.TotalSeconds;
            timeToNextFrameRateAdjustment -= dt;
            if (timeToNextFrameRateAdjustment <= TimeSpan.Zero)
            {
                timeToNextFrameRateAdjustment += timeBetweenFrameRateAdjustments;
                int frameRate = (int)frameRateMovingAverage;
                textValue = "fps: " + frameRate;
            }
        }
    }
}
