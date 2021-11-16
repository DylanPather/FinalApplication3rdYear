using APPDEVInc2.Repository;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using APPDEVInc2.DataBaseModels;
using APPDEVInc2.Models;
using System.Data.SqlClient;
using PagedList.Mvc;

namespace APPDEVInc2.ViewModels.Customer
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        ApplicationDbContext context = new ApplicationDbContext();
        public IPagedList<TyresTbl> ListOfProducts { get; set; }
        public StockTbl StockTbl { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page)
        {
            SqlParameter[] param = new SqlParameter[]{
                new SqlParameter("@search",search??(object)DBNull.Value)
            };
            IPagedList<TyresTbl> data = context.Database.SqlQuery<TyresTbl>("GetBySearch @search", param).ToList().ToPagedList(page ?? 1, pageSize);
            return new HomeIndexViewModel
            {
                ListOfProducts = data,
                
            };
        }

       
    }
}