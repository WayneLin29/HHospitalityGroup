using Autofac;
using PX.Data;
using PX.Objects.CM.Extensions;
using PX.Objects.GL;
using PX.Objects.SO;
using PX.Tests.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_APICustomization.Tests.Setup
{
    public abstract class UnitTestWithHHSetup : TestBase
    {
        protected override void RegisterServices(ContainerBuilder builder)
        {
            base.RegisterServices(builder);
            builder.RegisterType<PX.Objects.Unit.CurrencyServiceMock>().As<IPXCurrencyService>();
        }

        protected void SetupHH<Graph>() where Graph : PXGraph
        {
        }
    }
}
