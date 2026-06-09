using HealthAppMVC.Models;
using HealthAppMVC.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using HealthAppMVC.Constants;

namespace HealthAppMVC.Repository.Implementation
{
    public class AppointmentRepository
       : IAppointmentRepository
    {
        private readonly string _connectionString;

        public AppointmentRepository()
        {
            _connectionString =
                ConfigurationManager
                .ConnectionStrings["HealthDb"]
                .ConnectionString;
        }

        public List<Appointment> GetAll()
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT
                    A.*,
                    P.FullName AS PatientName,
                    D.FullName AS DoctorName
                  FROM Appointments A
                  INNER JOIN Patients P
                    ON A.PatientId = P.PatientId
                  INNER JOIN Doctors D
                    ON A.DoctorId = D.DoctorId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(
                        MapAppointment(reader));
                }
            }

            return appointments;
        }

        public Appointment GetById(int id)
        {
            Appointment appointment = null;

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT *
          FROM Appointments
          WHERE AppointmentId=@Id";

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
                    appointment = new Appointment
                    {
                        AppointmentId =
                            Convert.ToInt32(
                                reader["AppointmentId"]),

                        PatientId =
                            Convert.ToInt32(
                                reader["PatientId"]),

                        DoctorId =
                            Convert.ToInt32(
                                reader["DoctorId"]),

                        ScheduledDate =
                            Convert.ToDateTime(
                                reader["ScheduledDate"]),

                        TimeSlot =
                            reader["TimeSlot"]
                            .ToString(),

                        Status =
                        (AppointmentStatus)
                        Convert.ToInt32(
                        reader["Status"]),

                        CancellationReason =
                            reader["CancellationReason"]
                            == DBNull.Value
                            ? ""
                            : reader["CancellationReason"]
                            .ToString()
                    };
                }
            }

            return appointment;
        }

        public void Add(
            Appointment appointment)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"INSERT INTO Appointments
                (
                    PatientId,
                    DoctorId,
                    ScheduledDate,
                    TimeSlot,
                    Status,
                    CancellationReason
                )
                VALUES
                (
                    @PatientId,
                    @DoctorId,
                    @ScheduledDate,
                    @TimeSlot,
                    @Status,
                    @CancellationReason
                )";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    appointment.PatientId);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    appointment.DoctorId);

                cmd.Parameters.AddWithValue(
                    "@ScheduledDate",
                    appointment.ScheduledDate);

                cmd.Parameters.AddWithValue(
                    "@TimeSlot",
                    appointment.TimeSlot);

                cmd.Parameters.AddWithValue(
                    "@Status",
                     (int)appointment.Status);

                cmd.Parameters.AddWithValue(
                    "@CancellationReason",
                    string.IsNullOrEmpty(
                        appointment.CancellationReason)
                    ? (object)DBNull.Value
                    : appointment.CancellationReason);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStatus(
            int appointmentId,
            AppointmentStatus status,
            string cancellationReason)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"UPDATE Appointments
                  SET
                    Status=@Status,
                    CancellationReason=@Reason
                  WHERE AppointmentId=@Id";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Id",
                    appointmentId);

                cmd.Parameters.AddWithValue(
                    "@Status",
                    (int)status);

                cmd.Parameters.AddWithValue(
                    "@Reason",
                    string.IsNullOrEmpty(
                        cancellationReason)
                    ? (object)DBNull.Value
                    : cancellationReason);

                con.Open();

                cmd.ExecuteNonQuery();
            }
        }



        public bool IsSlotAvailable(
            int doctorId,
            string date,
            string timeSlot)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT COUNT(*)
                  FROM Appointments
                  WHERE DoctorId=@DoctorId
                  AND ScheduledDate=@Date
                  AND TimeSlot=@TimeSlot
                  AND Status <> @Cancelled";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctorId);

                cmd.Parameters.AddWithValue(
                    "@Date",
                    Convert.ToDateTime(date));

                cmd.Parameters.AddWithValue(
                    "@TimeSlot",
                    timeSlot);
                cmd.Parameters.AddWithValue(
    "@Cancelled",
    (int)AppointmentStatus.Cancelled);

                con.Open();

                int count =
                    (int)cmd.ExecuteScalar();

                return count == 0;
            }
        }

        public List<Appointment>
            GetAppointmentsByPatient(
            int patientId)
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT *
                  FROM Appointments
                  WHERE PatientId=@PatientId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    patientId);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(
                        new Appointment
                        {
                            AppointmentId =
                                Convert.ToInt32(
                                    reader["AppointmentId"]),

                            PatientId =
                                Convert.ToInt32(
                                    reader["PatientId"]),

                            DoctorId =
                                Convert.ToInt32(
                                    reader["DoctorId"]),

                            ScheduledDate =
                                Convert.ToDateTime(
                                    reader["ScheduledDate"]),

                            TimeSlot =
                                reader["TimeSlot"]
                                .ToString()
                        });
                }
            }

            return appointments;
        }

        public List<Appointment>
            GetAppointmentsByDoctor(
            int doctorId)
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT *
                  FROM Appointments
                  WHERE DoctorId=@DoctorId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctorId);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(
                        new Appointment
                        {
                            AppointmentId =
                                Convert.ToInt32(
                                    reader["AppointmentId"]),

                            PatientId =
                                Convert.ToInt32(
                                    reader["PatientId"]),

                            DoctorId =
                                Convert.ToInt32(
                                    reader["DoctorId"]),

                            ScheduledDate =
                                Convert.ToDateTime(
                                    reader["ScheduledDate"]),

                            TimeSlot =
                                reader["TimeSlot"]
                                .ToString()
                        });
                }
            }

            return appointments;
        }

        private Appointment
            MapAppointment(
            SqlDataReader reader)
        {
            return new Appointment
            {
                AppointmentId =
                    Convert.ToInt32(
                        reader["AppointmentId"]),

                PatientId =
                    Convert.ToInt32(
                        reader["PatientId"]),

                DoctorId =
                    Convert.ToInt32(
                        reader["DoctorId"]),

                ScheduledDate =
                    Convert.ToDateTime(
                        reader["ScheduledDate"]),

                TimeSlot =
                    reader["TimeSlot"]
                    .ToString(),

                Status =
                   (AppointmentStatus)
                   Convert.ToInt32(
                   reader["Status"]),

                CancellationReason =
                    reader["CancellationReason"]
                    == DBNull.Value
                    ? null
                    : reader["CancellationReason"]
                    .ToString(),

                PatientName =
                    reader["PatientName"]
                    .ToString(),

                DoctorName =
                    reader["DoctorName"]
                    .ToString()
            };
        }

        public bool IsDoctorSlotBooked(
    int doctorId,
    DateTime scheduledDate,
    string timeSlot)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT COUNT(*)
          FROM Appointments
          WHERE DoctorId=@DoctorId
          AND ScheduledDate=@ScheduledDate
          AND TimeSlot=@TimeSlot
          AND Status <> @Cancelled";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctorId);

                cmd.Parameters.AddWithValue(
                    "@ScheduledDate",
                    scheduledDate.Date);

                cmd.Parameters.AddWithValue(
                    "@TimeSlot",
                    timeSlot);

                cmd.Parameters.AddWithValue(
    "@Cancelled",
    (int)AppointmentStatus.Cancelled);

                con.Open();

                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return count > 0;
            }
        }

        public bool HasPatientAppointmentOnDate(
    int patientId,
    int doctorId,
    DateTime scheduledDate)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT COUNT(*)
          FROM Appointments
          WHERE PatientId=@PatientId
          AND DoctorId=@DoctorId
          AND ScheduledDate=@ScheduledDate
          AND Status <> @Cancelled";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    patientId);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctorId);

                cmd.Parameters.AddWithValue(
                    "@ScheduledDate",
                    scheduledDate.Date);

                cmd.Parameters.AddWithValue(
    "@Cancelled",
    (int)AppointmentStatus.Cancelled);

                con.Open();

                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return count > 0;
            }
        }

        public bool HasPatientSlotConflict(
    int patientId,
    DateTime scheduledDate,
    string timeSlot)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT COUNT(*)
          FROM Appointments
          WHERE PatientId=@PatientId
          AND ScheduledDate=@ScheduledDate
          AND TimeSlot=@TimeSlot
          AND Status <> @Cancelled";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@PatientId",
                    patientId);

                cmd.Parameters.AddWithValue(
                    "@ScheduledDate",
                    scheduledDate.Date);

                cmd.Parameters.AddWithValue(
                    "@TimeSlot",
                    timeSlot);

                cmd.Parameters.AddWithValue(
    "@Cancelled",
    (int)AppointmentStatus.Cancelled);

                con.Open();

                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return count > 0;
            }
        }

        public bool HealthRecordExists(
    int appointmentId)
        {
            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT COUNT(*)
          FROM HealthRecords
          WHERE AppointmentId=@AppointmentId";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@AppointmentId",
                    appointmentId);

                con.Open();

                int count =
                    Convert.ToInt32(
                        cmd.ExecuteScalar());

                return count > 0;
            }
        }

        public List<Appointment> GetUpcomingAppointments()
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT A.*,
               D.FullName AS DoctorName,
               P.FullName AS PatientName
        FROM Appointments A
        INNER JOIN Doctors D
            ON A.DoctorId = D.DoctorId
        INNER JOIN Patients P
            ON A.PatientId = P.PatientId
        WHERE ScheduledDate >= CAST(GETDATE() AS DATE)
        AND A.Status IN (0, 1)
        ORDER BY ScheduledDate";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(MapAppointment(reader));
                }
            }

            return appointments;
        }

        public List<Appointment>
GetUpcomingAppointmentsByDoctor(
    string doctorName)
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query = @"
        SELECT A.*,
               D.FullName AS DoctorName,
               P.FullName AS PatientName
        FROM Appointments A
        INNER JOIN Doctors D
            ON A.DoctorId = D.DoctorId
        INNER JOIN Patients P
            ON A.PatientId = P.PatientId
        WHERE D.FullName LIKE '%' + @Name + '%'
        AND ScheduledDate >= CAST(GETDATE() AS DATE)
        ORDER BY ScheduledDate";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Name",
                    doctorName);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(MapAppointment(reader));
                }
            }

            return appointments;
        }


        public List<string> GetAvailableSlots(
    int doctorId,
    DateTime scheduledDate)
        {
            List<string> bookedSlots =
                new List<string>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT TimeSlot
          FROM Appointments
          WHERE DoctorId=@DoctorId
          AND ScheduledDate=@Date
          AND Status <> @Cancelled";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@DoctorId",
                    doctorId);

                cmd.Parameters.AddWithValue(
                    "@Date",
                    scheduledDate.Date);

                cmd.Parameters.AddWithValue(
                    "@Cancelled",
                    (int)AppointmentStatus.Cancelled);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    bookedSlots.Add(
                        reader["TimeSlot"].ToString());
                }
            }

            return TimeSlots.Slots
                .Except(bookedSlots)
                .ToList();
        }

        public List<Appointment>
    GetAppointmentsByPatientName(
        string patientName)
        {
            List<Appointment> appointments =
                new List<Appointment>();

            using (SqlConnection con =
                new SqlConnection(_connectionString))
            {
                string query =
                @"SELECT
            A.*,
            P.FullName AS PatientName,
            D.FullName AS DoctorName
          FROM Appointments A
          INNER JOIN Patients P
            ON A.PatientId=P.PatientId
          INNER JOIN Doctors D
            ON A.DoctorId=D.DoctorId
          WHERE P.FullName
            LIKE '%' + @Name + '%'
          ORDER BY A.ScheduledDate DESC";

                SqlCommand cmd =
                    new SqlCommand(query, con);

                cmd.Parameters.AddWithValue(
                    "@Name",
                    patientName);

                con.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                while (reader.Read())
                {
                    appointments.Add(
                        MapAppointment(reader));
                }
            }

            return appointments;
        }

    }
}