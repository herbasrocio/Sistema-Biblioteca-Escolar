using ServicesSecurity.DAL.Contracts;
using ServicesSecurity.DAL.Tools;
using ServicesSecurity.DomainModel.Security.Composite;
using ServicesSecurity.Services.Extensions;
using ServicesSecurity.DAL.Implementations;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesSecurity.DAL.Implementations
{

    public sealed class FamiliaPatenteRepository : IJoinRepository<Familia>
    {
        #region Singleton
        private readonly static FamiliaPatenteRepository _instance = new FamiliaPatenteRepository();

        public static FamiliaPatenteRepository Current
        {
            get
            {
                return _instance;
            }
        }

        private FamiliaPatenteRepository()
        {
            //Implement here the initialization code
        }
        #endregion
        public void Add(Familia obj)
        {
            try
            {
                foreach (var item in obj.GetChildrens())
                {
                    // Verificar si los hijos son patente (no familia)
                    if (item.ChildrenCount() == 0)
                    {
                        Patente patente = item as Patente;
                        SqlHelper.ExecuteNonQuery("Familia_Patente_Insert",
                            System.Data.CommandType.StoredProcedure,
                            new System.Data.SqlClient.SqlParameter[] {
                                new System.Data.SqlClient.SqlParameter("@IdFamilia", obj.IdComponent),
                                new System.Data.SqlClient.SqlParameter("@IdPatente", patente.IdComponent)
                            });
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public void Delete(Familia obj)
        {
            try
            {
                SqlHelper.ExecuteNonQuery("Familia_Patente_Delete",
                    System.Data.CommandType.StoredProcedure,
                    new System.Data.SqlClient.SqlParameter[] {
                        new System.Data.SqlClient.SqlParameter("@IdFamilia", obj.IdComponent)
                    });
            }
            catch (Exception ex)
            {
                ex.Handle(this);
                throw;
            }
        }

        public void GetChildren(Familia obj)
        {
            //3) Buscar en SP Familia_Patente_Select y retornar id de patentes relacionados
            //4) Iterar sobre esos ids -> LLamar al Adaptador y cargar las patentes...
            Patente patenteGet = null;

            try
            {
                using (var reader = SqlHelper.ExecuteReader("Familia_Patente_Select",
                                                        System.Data.CommandType.StoredProcedure,
                                                        new SqlParameter[] { new SqlParameter("@IdFamilia", obj.IdComponent) }))
                {
                    object[] values = new object[reader.FieldCount];

                    while (reader.Read())
                    {
                        reader.GetValues(values);
                        //Obtengo el id de familia relacionado a la familia principal...(obj)
                        Guid idPatenteRelacionada = Guid.Parse(values[1].ToString());

                        patenteGet = PatenteRepository.Current.SelectOne(idPatenteRelacionada);

                        obj.Add(patenteGet);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.Handle(this);
            }
        }
    }
}
