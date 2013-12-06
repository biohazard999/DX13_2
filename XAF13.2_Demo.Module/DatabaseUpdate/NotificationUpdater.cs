using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;

namespace XAF13_2_Demo.Module.DatabaseUpdate
{
    public class NotificationUpdater : ModuleUpdater
    {
        public NotificationUpdater(IObjectSpace objectSpace, Version currentDBVersion)
            : base(objectSpace, currentDBVersion)
        {
        }

        public override void UpdateDatabaseBeforeUpdateSchema()
        {
            UpdateStatus("DBUpdater", "UpdateDatabaseBeforeUpdateSchema", "Before updating the schema");
            base.UpdateDatabaseBeforeUpdateSchema();
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            UpdateStatus("DBUpdater", "UpdateDatabaseAfterUpdateSchema", "After updating the schema");
            base.UpdateDatabaseAfterUpdateSchema();
        }
    }
}
