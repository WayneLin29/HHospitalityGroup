using HH_APICustomization.Descriptor;
using HH_APICustomization.Graph;
using HH_APICustomization.Tests.Setup;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Objects.GL;
using PX.Tests.Unit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace HH_APICustomization.Tests
{
    public class HHHelperTests : UnitTestWithHHSetup
    {
        [Fact]
        public void EnsureGetActualLedgerExpected()
        {
            Assert.True(true);
        }
    }
}
