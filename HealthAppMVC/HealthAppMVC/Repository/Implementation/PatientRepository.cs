using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace HealthAppMVC.Repository.Implementation
{
    public class PatientRepository : IPatientRepository
    {
        private readonly string _connectionString;

        public PatientRepository()
        {
            _connectionString =
                ConfigurationManager
                .ConnectionStrings["HealthDb"]
                .ConnectionString;
        }

        public List<Patient> GetAll()
        {
            List<Patient> patients = new List<Patient>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT *
                      FROM Patients
                      ORDER BY FullName";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientId =
                            Convert.ToInt32(
                                reader["PatientId"]),

                        FullName =
                            reader["FullName"].ToString(),

                        DateOfBirth =
                            Convert.ToDateTime(
                                reader["DateOfBirth"]),

                        Gender =
   (GenderType)Convert.ToInt32(
        reader["Gender"]),

                        PhoneNumber =
                            reader["PhoneNumber"].ToString(),

                        Email =
                            reader["Email"].ToString(),

                        InsuranceId =
                            reader["InsuranceId"].ToString(),

                        CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"])
                    });
                }
            }

            return patients;
        }

        public Patient GetById(int id)
        {
            Patient patient = null;

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT *
                      FROM Patients
                      WHERE PatientId=@Id";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Id",
                    id);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    patient = new Patient
                    {
                        PatientId =
                            Convert.ToInt32(
                                reader["PatientId"]),

                        FullName =
                            reader["FullName"].ToString(),

                        DateOfBirth =
                            Convert.ToDateTime(
                                reader["DateOfBirth"]),
                        Gender =
    (GenderType)Convert.ToInt32(
        reader["Gender"]),

                        PhoneNumber =
                            reader["PhoneNumber"].ToString(),

                        Email =
                            reader["Email"].ToString(),

                        InsuranceId =
                            reader["InsuranceId"].ToString(),

                        CreatedDate =
                            Convert.ToDateTime(
                                reader["CreatedDate"])
                    };
                }
            }

            return patient;
        }

        public void Add(Patient patient)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"INSERT INTO Patients
                (
                    FullName,
                    DateOfBirth,
                    Gender,
                    PhoneNumber,
                    Email,
                    InsuranceId
                )
                VALUES
                (
                    @FullName,
                    @DateOfBirth,
                    @Gender,
                    @PhoneNumber,
                    @Email,
                    @InsuranceId
                )";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    patient.FullName);

                cmd.Parameters.AddWithValue(
                    "@DateOfBirth",
                    patient.DateOfBirth);

                cmd.Parameters.AddWithValue(
                    "@Gender",
                    (int)patient.Gender);

                cmd.Parameters.AddWithValue(
                    "@PhoneNumber",
                    patient.PhoneNumber);

                cmd.Parameters.AddWithValue(
                    "@Email",
                    patient.Email);

                cmd.Parameters.AddWithValue(
                    "@InsuranceId",
                    patient.InsuranceId ??
                    (object)DBNull.Value);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Patient patient)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"UPDATE Patients
                  SET
                      FullName=@FullName,
                      DateOfBirth=@DateOfBirth,
                      Gender=@Gender,
                      PhoneNumber=@PhoneNumber,
                      Email=@Email,
                      InsuranceId=@InsuranceId
                  WHERE PatientId=@PatientId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    patient.PatientId);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    patient.FullName);

                cmd.Parameters.AddWithValue(
                    "@DateOfBirth",
                    patient.DateOfBirth);

                cmd.Parameters.AddWithValue(
                    "@Gender",
                    (int)patient.Gender);

                cmd.Parameters.AddWithValue(
                    "@PhoneNumber",
                    patient.PhoneNumber);

                cmd.Parameters.AddWithValue(
                    "@Email",
                    patient.Email);

                cmd.Parameters.AddWithValue(
                    "@InsuranceId",
                    patient.InsuranceId ??
                    (object)DBNull.Value);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

       

        public bool EmailExists(string email)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM Patients
                      WHERE Email=@Email";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Email",
                    email);

                con.Open();

                int count =
                    (int)cmd.ExecuteScalar();

                return count > 0;
            }
        }

        public int GetAppointmentCount(int patientId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT COUNT(*)
                      FROM Appointments
                      WHERE PatientId=@PatientId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    patientId);

                con.Open();

                return (int)cmd.ExecuteScalar();
            }
        }

        public List<Patient> SearchByName(string name)
        {
            List<Patient> patients =
                new List<Patient>();

            using (SqlConnection con =
                   new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT *
        FROM Patients
        WHERE FullName LIKE '%' + @Name + '%'
        ORDER BY FullName";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Name",
                    name);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    patients.Add(new Patient
                    {
                        PatientId =
                            Convert.ToInt32(
                            reader["PatientId"]),

                        FullName =
                            reader["FullName"]
                            .ToString()
                    });
                }
            }

            return patients;
        }
    }
}