using System;
using System.Linq.Expressions;
using OSS.Core.DomainMos.Members.Mos;
using OSS.Core.RepDapper.OrmExtention;
using Xunit;

namespace OSS.Core.RepTests
{
    public class ExpressionTest
    {
        [Fact]
        public void Test()
        {

            var vistor = new SqlExpression();

            //var sqlUpdateFlag = new SqlVistorFlag(SqlVistorType.Update);
            //Expression<Func<UserInfoMo, object>> funExpression =
            //    u => new {name = u.nick_name, nick_name = "s" + u.nick_name, email = u.email};
            //vistor.Visit(funExpression, sqlUpdateFlag);

            //var sqlWhereFlag = new SqlVistorFlag(SqlVistorType.Where);
            //Expression<Func<UserInfoMo, bool>> booExpression =
            //   u => (u.Id&2) == 2  && u.Id + 2 == 3|| u.email == "test" &&  !u.email.Contains("ninin");

            string name = "cecee";
            var mmm=new UserInfoMo();
            mmm.mobile = "15922374";


            var sqlWhereFlag = new SqlVistorFlag(SqlVistorType.Where);
            Expression<Func<UserInfoMo, bool>> booExpression =
               u => u.nick_name==name&&u.mobile==mmm.mobile;

            vistor.Visit(booExpression, sqlWhereFlag);


        }



    }


    public class TestClass
    {
        public TestEnum Id { get; set; }
    }

    public enum TestEnum
    {
        id=2
    }
}
