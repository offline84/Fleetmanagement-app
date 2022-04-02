using System;
using Xunit;

namespace Fleetmanagement_Unit_Testing
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IUnitOfWork _uow = new UnitOfWork();
            Bestuurder b = new Bestuurder();
        }
    }
}
