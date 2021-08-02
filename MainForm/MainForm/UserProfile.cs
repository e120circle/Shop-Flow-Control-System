using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainForm
{
    public class UserProfile : Profile
    {
        //MapperConfiguration config = new MapperConfiguration(cfg =>
        //{
        //    cfg.CreateMap<Models.Employee.EmployeeModel, ViewModels.Employee.EmployeeViewModel>().ReverseMap();
        //});
        public UserProfile()
        {
            //Home mapper
            CreateMap<Models.Home.HomeModel, ViewModels.Home.HomeViewModel>();
            CreateMap<ViewModels.Home.HomeViewModel, Models.Home.HomeModel>();

            //Users mapper
            CreateMap<Models.Users.UsersModel, ViewModels.Users.UsersViewModel>();
            CreateMap<ViewModels.Users.UsersViewModel, Models.Users.UsersModel>();

            //Job mapper
            CreateMap<Models.Job.JobModel, ViewModels.Job.JobViewModel>();
            CreateMap<ViewModels.Job.JobViewModel, Models.Job.JobModel>();

            //Job work mapper
            CreateMap<Models.Job.WorkModel, ViewModels.Job.WorkViewModel>();
            CreateMap<ViewModels.Job.WorkViewModel, Models.Job.WorkModel>();

            //Job routing mapper
            CreateMap<Models.Job.RoutingModel, ViewModels.Job.RoutingViewModel>();
            CreateMap<ViewModels.Job.RoutingViewModel, Models.Job.RoutingModel>();

            //Organize mapper
            CreateMap<Models.Organize.OrganizeModel, ViewModels.Organize.OrganizeViewModel>();
            CreateMap<ViewModels.Organize.OrganizeViewModel, Models.Organize.OrganizeModel>();

            //Company mapper
            CreateMap<Models.Organize.CompanyModel, ViewModels.Organize.CompanyViewModel>();
            CreateMap<ViewModels.Organize.CompanyViewModel, Models.Organize.CompanyModel>();

            //businessunit mapper
            CreateMap<Models.Organize.BusinessUnitModel, ViewModels.Organize.BusinessUnitViewModel>();
            CreateMap<ViewModels.Organize.BusinessUnitViewModel, Models.Organize.BusinessUnitModel>();

            //factory mapper
            CreateMap<Models.Organize.FactoryModel, ViewModels.Organize.FactoryViewModel>();
            CreateMap<ViewModels.Organize.FactoryViewModel, Models.Organize.FactoryModel>();

            //Operations Resource mapper
            CreateMap<Models.OperationsResource.OperationsResourceModel, ViewModels.OperationsResource.OperationsResourceViewModel>();
            CreateMap<ViewModels.OperationsResource.OperationsResourceViewModel, Models.OperationsResource.OperationsResourceModel>();

            //Operations mapper
            CreateMap<Models.Operations.OperationsModel, ViewModels.Operations.OperationsViewModel>();
            CreateMap<ViewModels.Operations.OperationsViewModel, Models.Operations.OperationsModel>();

            //Operations Submit mapper
            CreateMap<Models.Operations.OperationsSubmitModel, ViewModels.Operations.OperationsSubmitViewModel>();
            CreateMap<ViewModels.Operations.OperationsSubmitViewModel, Models.Operations.OperationsSubmitModel>();

            //Operations Detail mapper
            CreateMap<Models.Operations.OperationsDetailModel, ViewModels.Operations.OperationsDetailViewModel>();
            CreateMap<ViewModels.Operations.OperationsDetailViewModel, Models.Operations.OperationsDetailModel>();

            //Report mapper
            CreateMap<Models.Report.ReportModel, ViewModels.Report.ReportViewModel>();
            CreateMap<ViewModels.Report.ReportViewModel, Models.Report.ReportModel>();

            //Users mapper
            CreateMap<Models.Users.UsersModel, ViewModels.Users.UsersViewModel>();
            CreateMap<ViewModels.Users.UsersViewModel, Models.Users.UsersModel>();

            //Users mapper
            CreateMap<Models.Users.UserModel, ViewModels.Users.UserViewModel>();
            CreateMap<ViewModels.Users.UserViewModel, Models.Users.UserModel>();
        }
    }
}
