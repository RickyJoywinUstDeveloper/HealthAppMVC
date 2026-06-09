using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace HealthAppMVC.Repository.Implementation
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly string _connectionString;

        public DoctorRepository()
        {
            _connectionString =
                ConfigurationManager
                .ConnectionStrings["HealthDb"]
                .ConnectionString;
        }

        public List<Doctor> GetAll()
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT *
                      FROM Doctors
                      ORDER BY FullName";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId =
                            Convert.ToInt32(
                                reader["DoctorId"]),

                        FullName =
                            reader["FullName"].ToString(),

                        Specialisation =
                             (SpecialisationType)
                             Convert.ToInt32(
                             reader["Specialisation"]),

                        DoctorPhoneNo =
                            reader["DoctorPhoneNo"]
                            .ToString(),

                        DoctorEmail =
                            reader["DoctorEmail"]
                            .ToString(),

                        YearsOfExperience =
                            Convert.ToInt32(
                                reader["YearsOfExperience"]),

                        ConsultationFee =
                            Convert.ToDecimal(
                                reader["ConsultationFee"]),

                        IsActive =
                            Convert.ToBoolean(
                                reader["IsActive"])
                    });
                }
            }

            return doctors;
        }

        public Doctor GetById(int id)
        {
            Doctor doctor = null;

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                    @"SELECT *
                      FROM Doctors
                      WHERE DoctorId=@Id";

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
                    doctor = new Doctor
                    {
                        DoctorId =
                            Convert.ToInt32(
                                reader["DoctorId"]),

                        FullName =
                            reader["FullName"].ToString(),

                        Specialisation =
                        (SpecialisationType)
                        Convert.ToInt32(
                        reader["Specialisation"]),

                        DoctorPhoneNo =
                            reader["DoctorPhoneNo"]
                            .ToString(),

                        DoctorEmail =
                            reader["DoctorEmail"]
                            .ToString(),

                        YearsOfExperience =
                            Convert.ToInt32(
                                reader["YearsOfExperience"]),

                        ConsultationFee =
                            Convert.ToDecimal(
                                reader["ConsultationFee"]),

                        IsActive =
                            Convert.ToBoolean(
                                reader["IsActive"])
                    };
                }
            }

            return doctor;
        }

        public void Add(Doctor doctor)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"INSERT INTO Doctors
                (
                    FullName,
                    Specialisation,
                    DoctorPhoneNo,
                    DoctorEmail,
                    YearsOfExperience,
                    ConsultationFee,
                    IsActive
                )
                VALUES
                (
                    @FullName,
                    @Specialisation,
                    @DoctorPhoneNo,
                    @DoctorEmail,
                    @YearsOfExperience,
                    @ConsultationFee,
                    @IsActive
                )";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    doctor.FullName);

                cmd.Parameters.AddWithValue(
                    "@Specialisation",
                     (int)doctor.Specialisation);

                cmd.Parameters.AddWithValue(
                    "@DoctorPhoneNo",
                    doctor.DoctorPhoneNo);

                cmd.Parameters.AddWithValue(
                    "@DoctorEmail",
                    doctor.DoctorEmail);

                cmd.Parameters.AddWithValue(
                    "@YearsOfExperience",
                    doctor.YearsOfExperience);

                cmd.Parameters.AddWithValue(
                    "@ConsultationFee",
                    doctor.ConsultationFee);

                cmd.Parameters.AddWithValue(
                    "@IsActive",
                    doctor.IsActive);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void Update(Doctor doctor)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"UPDATE Doctors
                  SET
                    FullName=@FullName,
                    Specialisation=@Specialisation,
                    DoctorPhoneNo=@DoctorPhoneNo,
                    DoctorEmail=@DoctorEmail,
                    YearsOfExperience=@YearsOfExperience,
                    ConsultationFee=@ConsultationFee,
                    IsActive=@IsActive
                  WHERE DoctorId=@DoctorId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctor.DoctorId);

                cmd.Parameters.AddWithValue(
                    "@FullName",
                    doctor.FullName);

                cmd.Parameters.AddWithValue(
                    "@Specialisation",
                    (int)doctor.Specialisation);

                cmd.Parameters.AddWithValue(
                    "@DoctorPhoneNo",
                    doctor.DoctorPhoneNo);

                cmd.Parameters.AddWithValue(
                    "@DoctorEmail",
                    doctor.DoctorEmail);

                cmd.Parameters.AddWithValue(
                    "@YearsOfExperience",
                    doctor.YearsOfExperience);

                cmd.Parameters.AddWithValue(
                    "@ConsultationFee",
                    doctor.ConsultationFee);

                cmd.Parameters.AddWithValue(
                    "@IsActive",
                    doctor.IsActive);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void ChangeStatus(
            int id,
            bool isActive)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"UPDATE Doctors
                  SET IsActive=@IsActive
                  WHERE DoctorId=@DoctorId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    id);

                cmd.Parameters.AddWithValue(
                    "@IsActive",
                    isActive);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public List<Doctor> SearchBySpecialisation(
            SpecialisationType specialisation)
        {
            List<Doctor> doctors =
                new List<Doctor>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT *
                  FROM Doctors
                  WHERE Specialisation=@Specialisation
                  ORDER BY FullName";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Specialisation",
                     (int)specialisation);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId =
                            Convert.ToInt32(
                                reader["DoctorId"]),

                        FullName =
                            reader["FullName"].ToString(),

                        Specialisation =
                             (SpecialisationType)
                             Convert.ToInt32(
                             reader["Specialisation"]),

                        DoctorPhoneNo =
                            reader["DoctorPhoneNo"]
                            .ToString(),

                        DoctorEmail =
                            reader["DoctorEmail"]
                            .ToString(),

                        YearsOfExperience =
                            Convert.ToInt32(
                                reader["YearsOfExperience"]),

                        ConsultationFee =
                            Convert.ToDecimal(
                                reader["ConsultationFee"]),

                        IsActive =
                            Convert.ToBoolean(
                                reader["IsActive"])
                    });
                }
            }

            return doctors;
        }

        public List<Doctor> SearchByName(string name)
        {
            List<Doctor> doctors = new List<Doctor>();

            using (SqlConnection con =
                   new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT *
        FROM Doctors
        WHERE FullName LIKE '%' + @Name + '%'
        ORDER BY FullName";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@Name", name);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    doctors.Add(new Doctor
                    {
                        DoctorId = Convert.ToInt32(reader["DoctorId"]),
                        FullName = reader["FullName"].ToString(),
                        Specialisation =
                            (SpecialisationType)
                            Convert.ToInt32(reader["Specialisation"]),
                        DoctorPhoneNo =
                            reader["DoctorPhoneNo"].ToString(),
                        DoctorEmail =
                            reader["DoctorEmail"].ToString(),
                        YearsOfExperience =
                            Convert.ToInt32(reader["YearsOfExperience"]),
                        ConsultationFee =
                            Convert.ToDecimal(reader["ConsultationFee"]),
                        IsActive =
                            Convert.ToBoolean(reader["IsActive"])
                    });
                }
            }

            return doctors;
        }
    }
}