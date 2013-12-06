using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using XAF13_2_Demo.Module.Properties;

namespace XAF13_2_Demo.Module.BusinessObjects
{

    [NonPersistent]
    public abstract class BaseObject : XPObject
    {
        protected BaseObject(Session session) : base(session)
        {
        }

          [NotifyPropertyChangedInvocator]
        protected new virtual bool SetPropertyValue<T>(string propertyName, ref T propertyValueHolder, T newValue)
        {
            return base.SetPropertyValue(propertyName, ref propertyValueHolder, newValue);
        }
    }

    [DefaultClassOptions]
    public class LargeBusinessObject : BaseObject
    {
        private string _Property1;
        private string _Property2;
        private string _Property3;
        private string _Property4;
        private string _Property5;
        private string _Property6;
        private string _Property7;
        private string _Property8;
        private string _Property9;
        private string _Property10;
        private string _Name;

        public LargeBusinessObject(Session session) : base(session)
        {
        }

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue("Name", ref _Name, value); }
        }


        [Size(SizeAttribute.Unlimited)]
        public string Property1
        {
            get { return _Property1; }
            set { SetPropertyValue("Property1", ref _Property1, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property2
        {
            get { return _Property2; }
            set { SetPropertyValue("Property2", ref _Property2, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property3
        {
            get { return _Property3; }
            set { SetPropertyValue("Property3", ref _Property3, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property4
        {
            get { return _Property4; }
            set { SetPropertyValue("Property4", ref _Property4, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property5
        {
            get { return _Property5; }
            set { SetPropertyValue("Property5", ref _Property5, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property6
        {
            get { return _Property6; }
            set { SetPropertyValue("Property6", ref _Property6, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property7
        {
            get { return _Property7; }
            set { SetPropertyValue("Property7", ref _Property7, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property8
        {
            get { return _Property8; }
            set { SetPropertyValue("Property8", ref _Property8, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property9
        {
            get { return _Property9; }
            set { SetPropertyValue("Property9", ref _Property9, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Property10

        {
            get { return _Property10; }
            set { SetPropertyValue("Property10", ref _Property10, value); }
        }
    }
}
