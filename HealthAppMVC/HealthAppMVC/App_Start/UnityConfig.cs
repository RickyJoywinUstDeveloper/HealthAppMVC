
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

          

            container.RegisterType
     <IPatientApiService,
      PatientApiService>();

            container.RegisterType
                <IDoctorApiService,
                 DoctorApiService>();

            container.RegisterType
                <IAppointmentApiService,
                 AppointmentApiService>();

            container.RegisterType
                <IHealthRecordApiService,
                 HealthRecordApiService>();

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}