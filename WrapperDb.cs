
namespace DbService
{
    public interface IDbServiceWrapper
    {
        IIVacInvDbService IVacInv { get; }
        IVaccTransDbService VaccTrans { get; }
        IVaccTranLogDbService VaccTranDetail { get; }        
    }
    class DbServiceWrapper : IDbServiceWrapper
    {
        IIVacInvDbService _IVacInv = null;
        IVaccTransDbService _VaccTrans = null;
        IVaccTranLogDbService _VaccTranLogr = null;        
        MasInvDbContext MasInvDb;
        public DbServiceWrapper()
        {
            string Invconn = "data source=ZCKSQLP03;initial catalog=MasInv;persist security info=True;user id=sysuser;password=sysuser;MultipleActiveResultSets=True;";
            MasInvDb = new MasInvDbContext(Invconn);           
        }
        public IIVacInvDbService IVacInv => new IVacInvDbService(MasInvDb);

        public IVaccTransDbService VaccTrans => new VaccTransDbService(MasInvDb);

        public IVaccTranLogDbService VaccTranDetail => new VaccTransDetailDbService(MasInvDb);
    }
}
