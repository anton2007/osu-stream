using System;
using System.Drawing;
using OpenTK;
using osu_common.Tencho;
namespace osum
{
    public class TrackingPoint : ICloneable, bSerializable, IComparable<TrackingPoint>
    {
        public object Tag;
        
        private PointF location;
        /// <summary>
        /// The raw screen location 
        /// </summary>
        public PointF Location
        {
            get	{ return location; }
            set { 
                location = value;
                UpdatePositions();
            }
        }

        public TrackingPoint()
        {
        }
        
        public virtual void UpdatePositions()
        {
            Vector2 baseLast = BasePosition;
            BasePosition = new Vector2(GameBase.ScaleFactor * Location.X/GameBase.NativeSize.Width * GameBase.BaseSizeFixedWidth.X, GameBase.ScaleFactor * Location.Y/GameBase.NativeSize.Height * GameBase.BaseSizeFixedWidth.Y);
            WindowDelta = BasePosition - baseLast;
        }

        /// <summary>
        /// Increased for every press that is associated with the tracking point.
        /// </summary>
        int validity;
        
        public object HoveringObject;

        public TrackingPoint originalTrackingPoint;

        /// <summary>
        /// Each frame this will be set to false, and set to true when the previously hovering object
        /// is confirmed to still be the "highest" hovering object.
        /// </summary>
        public bool HoveringObjectConfirmed;
        
        /// <summary>
        /// Is this point still valid (active)?
        /// </summary>
        public bool Valid { get { return validity > 0; } }
        
        public TrackingPoint(PointF location) : this(location,null)
        {}
            
        public TrackingPoint(PointF location, object tag)
        {
            Tag = tag;
            Location = location;
            WindowDelta = Vector2.Zero; //no delta on first ctor.
            originalTrackingPoint = this;
        }

        public Vector2 BasePosition;
        public Vector2 WindowDelta;

        public virtual Vector2 GamefieldPosition
        {
            get
            {
                return GameBase.StandardToGamefield(BasePosition);
            }
        }

        internal void IncreaseValidity()
        {
            validity++;
        }

        internal void DecreaseValidity()
        {
            validity--;
        }

        #region ICloneable Members

        public object Clone()
        {
            TrackingPoint clone = MemberwiseClone() as TrackingPoint;
            clone.originalTrackingPoint = originalTrackingPoint;
            return clone;
        }

        #endregion

        #region bSerializable Members

        public void ReadFromStream(osu_common.Helpers.SerializationReader sr)
        {
            BasePosition = new Vector2(sr.ReadSingle(), sr.ReadSingle());
            WindowDelta = new Vector2(sr.ReadSingle(), sr.ReadSingle());
        }

        public void WriteToStream(osu_common.Helpers.SerializationWriter sw)
        {
            sw.Write(BasePosition.X);
            sw.Write(BasePosition.Y);
            sw.Write(WindowDelta.X);
            sw.Write(WindowDelta.Y);
        }

        #endregion

        #region IComparable<TrackingPoint> Members

        public int CompareTo(TrackingPoint other)
        {
            return 0;
        }

        #endregion
    }
}
