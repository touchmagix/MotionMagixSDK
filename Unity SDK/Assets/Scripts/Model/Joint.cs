using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace MotionMagixSimulator.Model
{
    [Serializable]
    public class Joint
    {
       
        private double _XCoordinate;
        [XmlElement(ElementName = "x")]
        public double XCoordinate
        {
            get
            {
                return _XCoordinate;
            }
            set
            {
                if (_XCoordinate != value)
                {
                    _XCoordinate = value;
                }
            }
        }

        private double _yCoordinate;
        [XmlElement(ElementName = "y")]
        public double YCoordinate
        {
            get
            {
                return _yCoordinate;
            }
            set
            {
                if (_yCoordinate != value)
                {
                    _yCoordinate = value;
                }
            }
        }

        private double _zCoordinate;
        [XmlElement(ElementName = "z")]
        public double ZCoordinate
        {
            get
            {
                return _zCoordinate;
            }
            set
            {
                if (_zCoordinate != value)
                {
                    _zCoordinate = value;
                }
            }
        }

        private double _OrientW;
        [XmlElement(ElementName = "OrientW")]
        public double OrientW
        {
            get
            {
                return _OrientW;
            }
            set
            {
                if (_OrientW != value)
                {
                    _OrientW = value;
                }
            }
        }

        private double _OrientX;
        [XmlElement(ElementName = "OrientX")]
        public double OrientX
        {
            get
            {
                return _OrientX;
            }
            set
            {
                if (_OrientX != value)
                {
                    _OrientX = value;
                }
            }
        }

        private double _Orienty;
        [XmlElement(ElementName = "OrientY")]
        public double OrientY
        {
            get
            {
                return _Orienty;
            }
            set
            {
                if (_Orienty != value)
                {
                    _Orienty = value;
                }
            }
        }

        private double _Orientz;
        [XmlElement(ElementName = "OrientZ")]
        public double OrientZ
        {
            get
            {
                return _Orientz;
            }
            set
            {
                if (_Orientz != value)
                {
                    _Orientz = value;
                }
            }
        }

        public Joint() { }
    }
}
