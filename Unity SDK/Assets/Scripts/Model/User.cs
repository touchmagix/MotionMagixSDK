using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Collections.ObjectModel;
using System.Net.Sockets;

namespace MotionMagixSimulator.Model
{
    [Serializable]
    public class User
    {
        private int _id;
        [XmlElement(ElementName = "id")]
        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                if(_id!=value)
                {
                    _id = value;
                }
            }
        }

        private Joints _SketelonJoints;
        [XmlElement(ElementName = "joints")]
        public Joints SketelonJoints
        {
            get
            {
                return _SketelonJoints;
            }
            set
            {
                if (_SketelonJoints != value)
                {
                    _SketelonJoints = value;
                }
            }
        }

        public User()
        {
            SketelonJoints = new Joints();
        }

    }
    [Serializable]
    public class UserObject
    {
        private User _CurrentUser;
        [XmlElement(ElementName = "user")]
        public User CurrentUser
        {
            get
            {
                return _CurrentUser;
            }
            set
            {
                if(_CurrentUser !=value)
                {
                    _CurrentUser = value;
                }
            }
        }

        public UserObject()
        {
            CurrentUser = new User();
        }
    }
    [Serializable]
    [XmlRoot(ElementName = "packet")]
    public class Skeleton
    {
        private UserObject _UserObject;
        [XmlElement(ElementName = "object")]
        public UserObject UserObject
        {
            get
            {
                return _UserObject;
            }
            set
            {
                if(_UserObject !=value)
                {
                    _UserObject = value;
                }
            }
        }

        public Skeleton()
        {
            UserObject = new UserObject();
        }
    }


    [Serializable]
    [XmlRoot(ElementName = "packets")]
    public class SkeletonFrames
    {
        private List<Skeleton> _packets;
        [XmlElement(ElementName = "packet")]
        public List<Skeleton> Packets
        {
            get
            {
                return _packets;
            }
            set
            {
                if (_packets != value)
                {
                    _packets = value;
                }
            }
        }

        public SkeletonFrames()
        {
            Packets = new List<Skeleton>();
        }
    }

}

public class SkeletonEventArgs:EventArgs
{
	public MotionMagixSimulator.Model.Skeleton Skeleton;
}