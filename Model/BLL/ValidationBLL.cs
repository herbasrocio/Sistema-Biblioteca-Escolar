using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DomainModel;
using DomainModel.Exceptions;

namespace BLL
{
    public static class ValidationBLL
    {
        /// <summary>
        /// Valida que un campo de texto no esté vacío o nulo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="fieldName">Nombre del campo para el mensaje de error</param>
        /// <exception cref="ValidacionException">Si el campo está vacío</exception>
        public static void ValidarCampoRequerido(string value, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ValidacionException($"El campo '{fieldName}' es requerido");
            }
        }

        /// <summary>
        /// Valida la longitud mínima de un campo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="minLength">Longitud mínima</param>
        /// <exception cref="ValidacionException">Si no cumple la longitud mínima</exception>
        public static void ValidarLongitudMinima(string value, string fieldName, int minLength)
        {
            if (value != null && value.Length < minLength)
            {
                throw new ValidacionException($"El campo '{fieldName}' debe tener al menos {minLength} caracteres");
            }
        }

        /// <summary>
        /// Valida la longitud máxima de un campo
        /// </summary>
        /// <param name="value">Valor a validar</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <param name="maxLength">Longitud máxima</param>
        /// <exception cref="ValidacionException">Si excede la longitud máxima</exception>
        public static void ValidarLongitudMaxima(string value, string fieldName, int maxLength)
        {
            if (!string.IsNullOrEmpty(value) && value.Length > maxLength)
            {
                throw new ValidacionException($"El campo '{fieldName}' no puede exceder {maxLength} caracteres");
            }
        }

        /// <summary>
        /// Valida formato de DNI argentino (7-8 dígitos numéricos)
        /// </summary>
        /// <param name="dni">DNI a validar</param>
        /// <exception cref="ValidacionException">Si el formato del DNI es inválido</exception>
        public static void ValidarFormatoDNI(string dni)
        {
            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new ValidacionException("El DNI es requerido");
            }

            // Eliminar espacios y puntos
            string dniLimpio = dni.Replace(".", "").Replace(" ", "").Trim();

            // Validar que sean solo números
            if (!Regex.IsMatch(dniLimpio, @"^\d{7,8}$"))
            {
                throw new ValidacionException("El DNI debe contener entre 7 y 8 dígitos numéricos");
            }
        }

        /// <summary>
        /// Valida formato de email
        /// </summary>
        /// <param name="email">Email a validar</param>
        /// <exception cref="ValidacionException">Si el formato del email es inválido</exception>
        public static void ValidarFormatoEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return; // Email es opcional
            }

            // Patrón simple pero efectivo para validar emails
            string patron = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(email, patron))
            {
                throw new ValidacionException("El formato del email es inválido");
            }
        }

        /// <summary>
        /// Valida formato de teléfono (opcional, solo números, guiones y espacios)
        /// </summary>
        /// <param name="telefono">Teléfono a validar</param>
        /// <exception cref="ValidacionException">Si el formato del teléfono es inválido</exception>
        public static void ValidarFormatoTelefono(string telefono)
        {
            if (string.IsNullOrWhiteSpace(telefono))
            {
                return; // Teléfono es opcional
            }

            // Permitir números, espacios, guiones y paréntesis
            string telefonoLimpio = telefono.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");

            if (!Regex.IsMatch(telefonoLimpio, @"^\d{7,15}$"))
            {
                throw new ValidacionException("El teléfono debe contener entre 7 y 15 dígitos");
            }
        }

        /// <summary>
        /// Valida que el nombre solo contenga letras, espacios y caracteres válidos
        /// </summary>
        /// <param name="nombre">Nombre a validar</param>
        /// <param name="fieldName">Nombre del campo</param>
        /// <exception cref="ValidacionException">Si el formato es inválido</exception>
        public static void ValidarFormatoNombre(string nombre, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                throw new ValidacionException($"El campo '{fieldName}' es requerido");
            }

            // Permitir letras (incluyendo acentos), espacios, apóstrofes y guiones
            if (!Regex.IsMatch(nombre, @"^[a-záéíóúüñA-ZÁÉÍÓÚÜÑ\s'\-]+$"))
            {
                throw new ValidacionException($"El campo '{fieldName}' solo puede contener letras, espacios, apóstrofes y guiones");
            }
        }

        /// <summary>
        /// Valida todos los campos de un Alumno
        /// </summary>
        /// <param name="alumno">Alumno a validar</param>
        /// <exception cref="ValidacionException">Si algún campo es inválido</exception>
        public static void ValidarAlumno(Alumno alumno)
        {
            if (alumno == null)
            {
                throw new ValidacionException("El alumno no puede ser nulo");
            }

            // Validar campos requeridos
            ValidarFormatoNombre(alumno.Nombre, "Nombre");
            ValidarFormatoNombre(alumno.Apellido, "Apellido");
            ValidarFormatoDNI(alumno.DNI);

            // Validar longitudes
            ValidarLongitudMaxima(alumno.Nombre, "Nombre", 100);
            ValidarLongitudMaxima(alumno.Apellido, "Apellido", 100);
            ValidarLongitudMaxima(alumno.Grado, "Grado", 50);
            ValidarLongitudMaxima(alumno.Division, "División", 10);
        }
    }
}
