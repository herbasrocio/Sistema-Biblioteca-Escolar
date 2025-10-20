using System;
using System.Data;
using DomainModel;
using DomainModel.Enums;

namespace DAL.Tools
{
    public static class HistorialEstadoEjemplarAdapter
    {
        public static HistorialEstadoEjemplar AdaptHistorial(DataRow row)
        {
            if (row == null)
                return null;

            return new HistorialEstadoEjemplar
            {
                IdHistorial = (Guid)row["IdHistorial"],
                IdEjemplar = (Guid)row["IdEjemplar"],
                EstadoAnterior = (EstadoMaterial)Convert.ToInt32(row["EstadoAnterior"]),
                EstadoNuevo = (EstadoMaterial)Convert.ToInt32(row["EstadoNuevo"]),
                FechaCambio = Convert.ToDateTime(row["FechaCambio"]),
                IdUsuario = row["IdUsuario"] != DBNull.Value ? (Guid?)row["IdUsuario"] : null,
                Motivo = row["Motivo"] != DBNull.Value ? row["Motivo"].ToString() : null,
                IdPrestamo = row["IdPrestamo"] != DBNull.Value ? (Guid?)row["IdPrestamo"] : null,
                IdDevolucion = row["IdDevolucion"] != DBNull.Value ? (Guid?)row["IdDevolucion"] : null,
                TipoCambio = ParseTipoCambio(row["TipoCambio"].ToString())
            };
        }

        private static TipoCambioEstado ParseTipoCambio(string tipoCambio)
        {
            switch (tipoCambio)
            {
                case "Manual":
                    return TipoCambioEstado.Manual;
                case "Prestamo":
                    return TipoCambioEstado.Prestamo;
                case "Devolucion":
                    return TipoCambioEstado.Devolucion;
                case "Sistema":
                    return TipoCambioEstado.Sistema;
                default:
                    return TipoCambioEstado.Manual;
            }
        }

        public static string TipoCambioToString(TipoCambioEstado tipoCambio)
        {
            switch (tipoCambio)
            {
                case TipoCambioEstado.Manual:
                    return "Manual";
                case TipoCambioEstado.Prestamo:
                    return "Prestamo";
                case TipoCambioEstado.Devolucion:
                    return "Devolucion";
                case TipoCambioEstado.Sistema:
                    return "Sistema";
                default:
                    return "Manual";
            }
        }
    }
}
