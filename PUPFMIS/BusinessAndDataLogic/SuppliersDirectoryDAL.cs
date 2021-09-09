using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PUPFMIS.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace PUPFMIS.BusinessAndDataLogic
{
    public class SuppliersDirectoryDAL : Controller
    {
        private FMISDbContext db = new FMISDbContext();

        public MemoryStream PrintDirectory(string LogoPath)
        {
            Reports reports = new Reports();
            reports.ReportFilename = "SUPPLIERS_DIRECTORY";
            reports.CreateDocument();
            reports.AddDoubleColumnHeader(LogoPath, false, 1.25);
            reports.AddColumnHeader(
                new HeaderLine { Content = "Republic of the Philippines", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "Date Printed: " + DateTime.Now.ToString("dd MMMM yyyy hh:mm tt"), Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "POLYTECHNIC UNIVERSITY OF THE PHILIPPINES", Bold = true, Italic = false, FontSize = 12, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "This is system generated.", Bold = false, Italic = true, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "Anonas St., Sta. Mesa, Manila", Bold = false, Italic = false, FontSize = 8, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );
            reports.AddColumnHeader(
                new HeaderLine { Content = "PROCUREMENT MANAGEMENT OFFICE", Bold = true, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Left },
                new HeaderLine { Content = "", Bold = true, Italic = false, FontSize = 5, ParagraphAlignment = ParagraphAlignment.Left }
            );

            reports.AddNewLine();
            reports.AddNewLine();

            reports.AddSingleColumnHeader();
            reports.AddColumnHeader(
                new HeaderLine { Content = "SUPPLIERS' DIRECTORY", Bold = false, Italic = false, FontSize = 10, ParagraphAlignment = ParagraphAlignment.Center }
            );

            reports.AddNewLine();

            var columns = new List<ContentColumn>();
            var rows = new List<ContentCell>();

            columns = new List<ContentColumn>();
            columns.Add(new ContentColumn(2.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.00, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.25, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            columns.Add(new ContentColumn(2.25, new MigraDoc.DocumentObjectModel.Color(254, 159, 89)));
            reports.AddTable(columns, true);

            rows = new List<ContentCell>();
            rows.Add(new ContentCell("Contact Person", 0, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Telephone No.", 1, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Email Address", 2, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Website", 3, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Supplier Category", 4, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            rows.Add(new ContentCell("Item Type", 5, 8, true, false, ParagraphAlignment.Center, VerticalAlignment.Center));
            reports.AddRowContent(rows, 0.25);

            var suppliers = GetSuppliers();

            foreach (var supplier in suppliers)
            {
                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(12.5, new MigraDoc.DocumentObjectModel.Color(252, 207, 101)));
                reports.AddTable(columns, false);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(supplier.SupplierName, 0, 8.5, true, false, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, false, new Color(0,0,0), true, true, true));
                reports.AddRowContent(rows, 0.15);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(supplier.Address, 0, 7, false, true, ParagraphAlignment.Left, VerticalAlignment.Top, 0, 0, true, new Color(0, 0, 0), true, false, true));
                reports.AddRowContent(rows, 0.15);

                var categories = supplier.CategoryList.Count == 0 ? "Not Set" : string.Empty;
                var itemTypes = supplier.ItemTypesList.Count == 0 ? "Not Set" : string.Empty;
                for (int i = 0; i < supplier.CategoryList.Count; i++)
                {
                    if (i == supplier.CategoryList.Count - 1)
                    {
                        categories += supplier.CategoryList[i].Category;
                    }
                    else
                    {
                        categories += supplier.CategoryList[i].Category + ",\n";
                    }
                }

                for (int i = 0; i < supplier.ItemTypesList.Count; i++)
                {
                    if (i == supplier.ItemTypesList.Count - 1)
                    {
                        itemTypes += supplier.ItemTypesList[i].ItemType;
                    }
                    else
                    {
                        itemTypes += supplier.ItemTypesList[i].ItemType + ",\n";
                    }
                }

                columns = new List<ContentColumn>();
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(2.00));
                columns.Add(new ContentColumn(2.25));
                columns.Add(new ContentColumn(2.25));
                reports.AddTable(columns, true);

                rows = new List<ContentCell>();
                rows.Add(new ContentCell(supplier.ContactPerson.ToUpper(), 0, 7, true, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(supplier.ContactNumber + (supplier.AlternateContactNumber != null ? ("\n" + supplier.AlternateContactNumber) : String.Empty), 1, 7, false, false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(supplier.EmailAddress, 2, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                rows.Add(new ContentCell(supplier.Website, 3, 7, false, false, ParagraphAlignment.Center, VerticalAlignment.Top));
                rows.Add(new ContentCell(categories, 4, 7, false, categories == "Not Set" ? true: false, ParagraphAlignment.Left, VerticalAlignment.Top));
                rows.Add(new ContentCell(itemTypes, 5, 7, false, itemTypes == "Not Set" ? true : false, ParagraphAlignment.Left, VerticalAlignment.Top));
                reports.AddRowContent(rows, 0.25);

            }

            return reports.GenerateReport();
        }

        public List<ItemCategory> GetCategories()
        {
            return db.ItemCategories.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<ItemTypes> GetItemTypes()
        {
            return db.ItemTypes.Where(d => d.PurgeFlag == false).ToList();
        }
        public List<SupplierVM> GetSuppliers()
        {
            var suppliersList = db.Suppliers
                                .Where(d => d.PurgeFlag == false)
                                .Select(d => new SupplierVM {
                                    ID = d.ID,
                                    SupplierName = d.SupplierName,
                                    ContactPerson = d.ContactPerson,
                                    ContactNumber = d.ContactNumber,
                                    AlternateContactNumber = d.AlternateContactNumber,
                                    Address = d.Address,
                                    TaxIdNumber = d.TaxIdNumber,
                                    EmailAddress = d.EmailAddress,
                                    Website = d.Website,
                                    PurgeFlag = d.PurgeFlag,
                                    CreatedAt = d.CreatedAt,
                                    UpdatedAt = d.UpdatedAt,
                                    DeletedAt = d.DeletedAt
                                })
                                .OrderBy(d => d.SupplierName).ToList();
            foreach (var supplier in suppliersList)
            {
                supplier.CategoryList = db.SupplierCategories.Where(d => d.SupplierReference == supplier.ID).Select(d => new SupplierCategoriesVM { ID = d.FKCategoryReference.ID, Category = d.FKCategoryReference.ItemCategoryName }).ToList();
                supplier.ItemTypesList = db.SupplierItemTypes.Where(d => d.SupplierReference == supplier.ID).Select(d => new SupplierItemTypesVM { ID = d.FKItemTypeReference.ID, ItemType = d.FKItemTypeReference.ItemType }).ToList();
            }
            return suppliersList;
        }
        public SupplierVM GetSupplierDetails(int? SupplierID)
        {
            var supplierDetails = db.Suppliers.Where(d => d.ID == SupplierID).Select(d => new SupplierVM
            {
                ID = d.ID,
                SupplierName = d.SupplierName,
                ContactPerson = d.ContactPerson,
                ContactNumber = d.ContactNumber,
                AlternateContactNumber = d.AlternateContactNumber,
                Address = d.Address,
                TaxIdNumber = d.TaxIdNumber,
                EmailAddress = d.EmailAddress,
                Website = d.Website,
                PurgeFlag = d.PurgeFlag,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt,
                DeletedAt = d.DeletedAt
            }).FirstOrDefault();

            if(supplierDetails != null)
            {
                supplierDetails.CategoryList = db.SupplierCategories.Where(d => d.SupplierReference == supplierDetails.ID).Select(d => new SupplierCategoriesVM { ID = d.FKCategoryReference.ID, Category = d.FKCategoryReference.ItemCategoryName }).ToList();
                supplierDetails.ItemTypesList = db.SupplierItemTypes.Where(d => d.SupplierReference == supplierDetails.ID).Select(d => new SupplierItemTypesVM { ID = d.FKItemTypeReference.ID, ItemType = d.FKItemTypeReference.ItemType }).ToList();
            }

            return supplierDetails;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}