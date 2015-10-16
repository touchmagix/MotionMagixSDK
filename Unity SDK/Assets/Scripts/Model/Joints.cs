using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace MotionMagixSimulator.Model
{
    [Serializable]
   
    public  class Joints
    {
        private Joint _Head;
        [XmlElement(ElementName = "joint_0")]
        public Joint Head
        {
            get
            {
                return _Head;
            }
            set
            {
                if(_Head !=value)
                {
                    _Head = value;
                }
            }
        }


        private Joint _Neck;
        [XmlElement(ElementName = "joint_1")]
        public Joint Neck
        {
            get
            {
                return _Neck;
            }
            set
            {
                if (_Neck != value)
                {
                    _Neck = value;
                }
            }
        }

        private Joint _LeftShoulder;
        [XmlElement(ElementName = "joint_2")]
        public Joint LeftShoulder
        {
            get
            {
                return _LeftShoulder;
            }
            set
            {
                if (_LeftShoulder != value)
                {
                    _LeftShoulder = value;
                }
            }
        }

        private Joint _RightShoulder;
        [XmlElement(ElementName = "joint_3")]
        public Joint RightShoulder
        {
            get
            {
                return _RightShoulder;
            }
            set
            {
                if (_RightShoulder != value)
                {
                    _RightShoulder = value;
                }
            }
        }

        private Joint _LeftElbow;
        [XmlElement(ElementName = "joint_4")]
        public Joint LeftElbow
        {
            get
            {
                return _LeftElbow;
            }
            set
            {
                if (_LeftElbow != value)
                {
                    _LeftElbow = value;
                }
            }
        }

        private Joint _RightElbow;
        [XmlElement(ElementName = "joint_5")]
        public Joint RightElbow
        {
            get
            {
                return _RightElbow;
            }
            set
            {
                if (_RightElbow != value)
                {
                    _RightElbow = value;
                }
            }
        }

        private Joint _LeftHand;
        [XmlElement(ElementName = "joint_6")]
        public Joint LeftHand
        {
            get
            {
                return _LeftHand;
            }
            set
            {
                if (_LeftHand != value)
                {
                    _LeftHand = value;
                }
            }
        }

        private Joint _RightHand;
        [XmlElement(ElementName = "joint_7")]
        public Joint RightHand
        {
            get
            {
                return _RightHand;
            }
            set
            {
                if (_RightHand != value)
                {
                    _RightHand = value;
                }
            }
        }

        private Joint _Torso;
        [XmlElement(ElementName = "joint_8")]
        public Joint Torso
        {
            get
            {
                return _Torso;
            }
            set
            {
                if (_Torso != value)
                {
                    _Torso = value;
                }
            }
        }

        private Joint _LeftHip;
        [XmlElement(ElementName = "joint_9")]
        public Joint LeftHip
        {
            get
            {
                return _LeftHip;
            }
            set
            {
                if (_LeftHip != value)
                {
                    _LeftHip = value;
                }
            }
        }

        private Joint _RightHip;
        [XmlElement(ElementName = "joint_10")]
        public Joint RightHip
        {
            get
            {
                return _RightHip;
            }
            set
            {
                if (_RightHip != value)
                {
                    _RightHip = value;
                }
            }
        }

        private Joint _LeftKnee;
        [XmlElement(ElementName = "joint_11")]
        public Joint LeftKnee
        {
            get
            {
                return _LeftKnee;
            }
            set
            {
                if (_LeftKnee != value)
                {
                    _LeftKnee = value;
                }
            }
        }

        private Joint _RightKnee;
        [XmlElement(ElementName = "joint_12")]
        public Joint RightKnee
        {
            get
            {
                return _RightKnee;
            }
            set
            {
                if (_RightKnee != value)
                {
                    _RightKnee = value;
                }
            }
        }

        private Joint _LeftFoot;
        [XmlElement(ElementName = "joint_13")]
        public Joint LeftFoot
        {
            get
            {
                return _LeftFoot;
            }
            set
            {
                if (_LeftFoot != value)
                {
                    _LeftFoot = value;
                }
            }
        }

        private Joint _RightFoot;
        [XmlElement(ElementName = "joint_14")]
        public Joint RightFoot
        {
            get
            {
                return _RightFoot;
            }
            set
            {
                if (_RightFoot != value)
                {
                    _RightFoot = value;
                }
            }
        }

        public Joints()
        {
            Head = new Joint();
            Neck = new Joint();
            LeftShoulder = new Joint();
            RightShoulder = new Joint();
            LeftElbow = new Joint();
            RightElbow = new Joint();
            LeftHand = new Joint();
            RightHand = new Joint();
            Torso = new Joint();
            LeftHip = new Joint();
            RightHip = new Joint();
            LeftKnee = new Joint();
            RightKnee = new Joint();
            LeftFoot = new Joint();
            RightFoot = new Joint();
        }
    }
}
