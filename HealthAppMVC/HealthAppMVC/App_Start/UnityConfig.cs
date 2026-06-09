using HealthAppMVC.Repository.Implementation;
using HealthAppMVC.Repository.Interface;
using HealthAppMVC.Services.Implementation;
using HealthAppMVC.Services.Interface;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace HealthAppMVC
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();

            container.RegisterType<
    IPatientRepository,
    PatientRepository>();

            container.RegisterType<
                IPatientService,
                PatientService>();

            container.RegisterType<
                IDoctorRepository,
                DoctorRepository>();

            container.RegisterType<
                IDoctorService,
                DoctorService>();

            container.RegisterType<
                IAppointmentRepository,
                AppointmentRepository>();

            container.RegisterType<
                IAppointmentService,
                AppointmentService>();

            container.RegisterType<
                IHealthRecordRepository,
                HealthRecordRepository>();

            container.RegisterType<
                IHealthRecordService,
                HealthRecordService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}