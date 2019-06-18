using AdministracionRH.Common;
using AdministracionRH.Models;
using AdministracionRH.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdministracionRH.Services
{
    public class EmployeeService
    {
        EmployeeRepository _employeeRepository;

        EmployeesModel _model { get; set; }
        // private User _user { get; set; }
        public EmployeeService()
        {
            _employeeRepository = new EmployeeRepository();
        }

        public EmployeeService(EmployeesModel model)
        {
            _employeeRepository = new EmployeeRepository();
            _model = model;
        }


        public List<Employee> GetAllEmployees()
        {
           List<Employee> lista = null;
            Result result = _employeeRepository.GetAllEmployess(
                    out lista,
                   _model.Nombre
                 , _model.PositionId
                 , _model.AreaId
                 , _model.FechaInicial
                 , _model.FechaFinal
                 , _model.BossId
                 , _model.Activo
                 , _model.SelectedEmployeeId
                 , _model.OperacionId
                 , _model.Rol
                 , _model.EmployeeIdConected
                 ,_model.FingerPrint
               );
            
            if (lista!=null)
            {
                _model.RecordCount = lista.Count();
                var query = OrdenaDatos(lista);
                _model.PageCount = query.Count() == 0 ? 1 : ((query.Count() - 1) / _model.PageSize) + 1;
                _model.LstEmployees = query.Skip((_model.CurrentPageIndex - 1) * _model.PageSize).Take(_model.PageSize).ToList();
            }
            if (!result.Success)
                return null;
            return _model.LstEmployees;
        }
        private IEnumerable<Employee> OrdenaDatos(List<Employee> lista)
        {

            IQueryable<Employee> query = null;
            query = lista.AsQueryable();
            switch (_model.SortField)
            {
                case "FirstName":
                    query = (_model.SortDirection == "ascending" ? query.OrderBy(c => c.FirstName).AsQueryable() : query.OrderByDescending(c => c.FirstName).AsQueryable());
                    break;
                case "LastName":
                    query = (_model.SortDirection == "ascending" ? query.OrderBy(c => c.LastName).AsQueryable() : query.OrderByDescending(c => c.LastName).AsQueryable());
                    break;

            }
            return query;
        }



        public Employee GetAOneEmployee(int id)
        {
            List<Catalogo> lstCatalogos = null;
            List<Jefe> lstJefes = null;
            Employee employee = null;
            Result result = _employeeRepository.GetOneEmployee(out employee, id);
            if (!result.Success)
                return null;
            result = _employeeRepository.GetCatalogos(out lstCatalogos);
            if (!result.Success)
                return null;
            employee.ListaAreas = lstCatalogos.Where(i => i.ORIGEN == "AREAS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.AreaId }).ToList();
            employee.ListaNacionalidad = lstCatalogos.Where(i => i.ORIGEN == "NACIONALIDAD").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.NationalityId }).ToList();
            employee.ListaPosition = lstCatalogos.Where(i => i.ORIGEN == "POSITIONS").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.PositionId }).ToList();
            employee.ListaLocalizacion = lstCatalogos.Where(i => i.ORIGEN == "LOCALIZACION").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.LocalizationId }).ToList();
            employee.ListaPayRoll = lstCatalogos.Where(i => i.ORIGEN == "PAYROLL").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.PayRollId }).ToList();
            employee.ListaCompanies = lstCatalogos.Where(i => i.ORIGEN == "COMPANIES").Select(i => new SelectListItem { Value = i.ID.ToString(), Text = i.DESCRIPTION, Selected = i.ID == employee.CompanyId }).ToList();

            result = _employeeRepository.GetAllJefes(out lstJefes);
            if (!result.Success)
                return null;
            employee.ListaJefes = lstJefes.Select(i => new SelectListItem { Value = i.BossId.ToString(), Text = i.FullName, Selected = i.BossId == employee.BossId }).ToList();
            return employee;
        }


        public Employee GetAOneEmployeeSystem(string CompanyEmail)
        {
         
            Employee employee = null;
            Result result = _employeeRepository.GetOneEmployeeSystem(out employee, CompanyEmail);
            if (!result.Success)
                return null;
            return employee;
        }



        public bool UpdateEmployee(Employee employee)
        {

            try
            {
                Result result = _employeeRepository.UpdateEmployee(employee);
                return result.Success;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public bool CreateEmployee(Employee employee)
        {
            try
            {
                Result result = _employeeRepository.CreateEmployee(employee);
                return result.Success;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<Catalogo> GetCatalogos()
        {
            List<Catalogo> lstCatalogos = null;
            try
            {
               
                Result result = _employeeRepository.GetCatalogos(out lstCatalogos);
                if (!result.Success)
                    return null;
            }
            catch (Exception)
            {

                return null;
            }
            return lstCatalogos;

        }

        public List<Jefe> GetAllJefes()
        {
            List<Jefe> lstJefes = null;
            try
            {

                Result result = _employeeRepository.GetAllJefes(out lstJefes);
                if (!result.Success)
                    return null;
            }
            catch (Exception)
            {

                return null;
            }
            return lstJefes;

        }

    }
}