using ECommerceApp7.Models;
using ECommerceApp7.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace ECommerceApp7.Controllers
{
    [Authorize(Users = "admin_789@gmail.com")]
    public class AdminController : BaseController
    {
        /// <summary>
        /// Application DB context
        /// </summary>
        protected ApplicationDbContext ApplicationDbContext { get; set; }

        /// <summary>
        /// User manager - attached to application DB context
        /// </summary>
        protected UserManager<ApplicationUser> UserManager { get; set; }


        public AdminController()
        {
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(this.ApplicationDbContext));
        }

        public ActionResult Home()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateItem(CreateItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("CreateItem", viewModel);
            }

            //gets the CategoryId
            int getCatId = ApplicationDbContext.Categories
                                               .Where(i => i.CategoryName == viewModel.CategoryName)
                                               .Select(i => i.CategoryId).FirstOrDefault();

            //First builds the Item object and adds it to the db. afterward the new items id is available to add to the junction table.
            Item newItem = new Item
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                Price = viewModel.Price,
                CategoryId = getCatId,
                ImageReference = viewModel.ImageReference,
                DateAdded = DateTime.Now
            };

            ApplicationDbContext.Items.Add(newItem);
            ApplicationDbContext.SaveChanges();



            //Junction table for SubcategoryItems
            if (viewModel.SubCategories != null)
            {
                foreach (string sub in viewModel.SubCategories)
                {
                    int getSubId = ApplicationDbContext.SubCategories
                                                       .Where(i => i.SubCategoryName == sub)
                                                       .Select(i => i.SubCategoryId)
                                                       .FirstOrDefault();

                    int getItemId = ApplicationDbContext.Items
                                                        .Where(i => i.Name == viewModel.Name)
                                                        .Select(i => i.ItemId).FirstOrDefault();

                    Item getItem = ApplicationDbContext.Items
                                                        .FirstOrDefault(i => i.ItemId == getItemId);

                    if (getItem != null)
                    {

                        SubCategoryItems subCatItems = new SubCategoryItems
                        {
                            SubcategoryId = getSubId,
                            ItemId = getItemId
                        };

                        ApplicationDbContext.SubCategoryItems.Add(subCatItems);
                        ApplicationDbContext.SaveChanges();
                    }
                }
            }

            ModelState.Clear();

            //Pass subcategories to view
            ViewBag.subcategories = ApplicationDbContext.SubCategories.Select(i => i.SubCategoryName);

            return View();
        }

        public ActionResult CreateItem()
        {

            ViewBag.subcategories = ApplicationDbContext.SubCategories
                                                        .Select(i => i.SubCategoryName);

            return View();
        }

        public ActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCategory(CreateCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Category catName = new Category
            {
                CategoryName = viewModel.CategoryName,
                FilterButtonName = viewModel.FilterButtonName

            };

            ApplicationDbContext.Categories.Add(catName);
            ApplicationDbContext.SaveChanges();

            ModelState.Clear();
            return View();
        }

        [HttpGet]
        public ActionResult CreateSubCategories()
        {
            if (!ApplicationDbContext.Categories.Any())
            {
                return RedirectToAction("CreateCategory");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateSubCategories(CreateSubCategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (ApplicationDbContext.SubCategories
                                    .Select(i => i.SubCategoryName)
                                    .Contains(viewModel.SubCategoryName))
            {
                viewModel.ValidationMessage = "Sub-Category Name Must be Unique.";
                return View(viewModel);
            }

            int getCatId = ApplicationDbContext.Categories.Where(i => i.CategoryName == viewModel.CategoryName)
                                                          .Select(i => i.CategoryId).FirstOrDefault();


            SubCategory newSubCat = new SubCategory
            {
                CategoryId = getCatId,
                SubCategoryName = viewModel.SubCategoryName
            };

            ApplicationDbContext.SubCategories.Add(newSubCat);
            ApplicationDbContext.SaveChanges();

            ModelState.Clear();

            CreateSubCategoriesViewModel returnValues = new CreateSubCategoriesViewModel
            {
                CategoryName = viewModel.CategoryName,
                SubCategoryName = ""
            };

            return View(returnValues);
        }


        /****************************Update Item**************************************************/


        public ActionResult UpdateItem()
        {
            return RedirectToAction("SelectItemToUpdate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateItem(UpdateItemViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Item itemToUpdate = ApplicationDbContext.Items.First(i => i.ItemId == viewModel.ItemId);

            itemToUpdate.Name = viewModel.Name;
            itemToUpdate.Description = viewModel.Description;
            itemToUpdate.Price = viewModel.Price;
            itemToUpdate.ImageReference = viewModel.ImageReference;

            List<int> itemSubcategory = ApplicationDbContext.SubCategoryItems.Where(i => i.ItemId == viewModel.ItemId).Select(i => i.SubcategoryId).ToList();

            //Check for new subcategory to add to junction table. If sub count is 0 then all check will be added. if itemSubcategory does not contain the subcategoryId (sub) then it will be added
            //Add subcategories
            if (viewModel.Subcategories != null)
            {
                foreach (int sub in viewModel.Subcategories)
                {
                    if (itemSubcategory.Count < 1 || !itemSubcategory.Contains(sub))
                    {
                        SubCategoryItems newSubcatItem = new SubCategoryItems
                        {
                            SubcategoryId = sub,
                            ItemId = viewModel.ItemId
                        };
                        ApplicationDbContext.SubCategoryItems.Add(newSubcatItem);
                    }
                }
            }

            //Check to see if subcategory was uncheck and will be removed.
            //remove subcategories
            if (viewModel.Subcategories == null)
            {
                var removeAll = ApplicationDbContext.SubCategoryItems
                                                    .Where(i => i.ItemId == viewModel.ItemId);
                ApplicationDbContext.SubCategoryItems
                                    .RemoveRange(removeAll);
            }
            else
            {
                foreach (var itemSub in itemSubcategory)
                {
                    if (!viewModel.Subcategories.Contains(itemSub))
                    {
                        //remove unchecked subcategory from item
                        SubCategoryItems removeSingle = ApplicationDbContext.SubCategoryItems
                                                                            .Where(i => i.ItemId == viewModel.ItemId)
                                                                             .First(i => i.SubcategoryId == itemSub);

                        ApplicationDbContext.SubCategoryItems
                                            .Remove(removeSingle);

                        ApplicationDbContext.SaveChanges();
                    }
                }
            }

            ApplicationDbContext.SaveChanges();

            List<Category> categoryList = ApplicationDbContext.Categories.ToList();

            viewModel.Categories = categoryList;

            return View("UpdateItem", viewModel);
        }

        [HttpPost]
        public ActionResult GetUpdateItem(int itemId)
        {
            Item item = ApplicationDbContext.Items.FirstOrDefault(i => i.ItemId == itemId);

            Category categoryName = ApplicationDbContext.Categories.FirstOrDefault(i => i.CategoryId == item.CategoryId);

            List<Category> categoryList = ApplicationDbContext.Categories.ToList();

            UpdateItemViewModel viewModel = null;

            if (item != null && categoryName != null)
            {
                viewModel = new UpdateItemViewModel
                {
                    ItemId = item.ItemId,
                    Name = item.Name,
                    Price = item.Price,
                    ImageReference = item.ImageReference,
                    DateAdded = item.DateAdded,
                    Description = item.Description,
                    CategoryName = categoryName.CategoryName,
                    Categories = categoryList
                };
            }

            return View("UpdateItem", viewModel);
        }

        public ActionResult SelectItemToUpdate(SelectItemToUpdateViewModel viewModel)
        {
            viewModel.Items = ApplicationDbContext.Items.ToList();

            viewModel.CategoryName = ApplicationDbContext.Categories.ToList();

            return View(viewModel);
        }


        /***************************Update Category**********************************************/


        public ActionResult SelectCategoryToUpdate()
        {
            List<Category> category = ApplicationDbContext.Categories.ToList();
            ViewBag.subCategories = ApplicationDbContext.SubCategories.ToList();

            return View(category);
        }

        public ActionResult UpdateCategory()
        {
            return RedirectToAction("SelectCategoryToUpdate");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GetUpdateCategory(int categoryId)
        {
            var category = ApplicationDbContext.Categories.First(i => i.CategoryId == categoryId);

            var viewModel = new UpdateCategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                FilterButtonName = category.FilterButtonName
            };

            return View("UpdateCategory", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCategory(UpdateCategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            Category categoryToUpdate = ApplicationDbContext.Categories
                                                            .First(i => i.CategoryId == viewModel.CategoryId);

            categoryToUpdate.CategoryName = viewModel.CategoryName;
            categoryToUpdate.FilterButtonName = viewModel.FilterButtonName;

            ApplicationDbContext.SaveChanges();

            return View(viewModel);
        }

        public ActionResult LoadSubCategory(int categoryId)
        {
            IQueryable<SubCategory> model = ApplicationDbContext.SubCategories
                                                                .Where(i => i.CategoryId == categoryId);

            return PartialView("_SubCategoryRemovePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteSubCategory(int subCategoryId, int categoryId)
        {
            SubCategory subCat = ApplicationDbContext.SubCategories
                                                     .First(i => i.SubCategoryId == subCategoryId);

            IQueryable<SubCategoryItems> subCatItem = ApplicationDbContext.SubCategoryItems
                                                                          .Where(i => i.SubcategoryId == subCategoryId);

            ApplicationDbContext.SubCategories.Remove(subCat);

            foreach (SubCategoryItems subItem in subCatItem)
            {
                ApplicationDbContext.SubCategoryItems.Remove(subItem);
            }

            ApplicationDbContext.SaveChanges();

            IQueryable<SubCategory> model = ApplicationDbContext.SubCategories
                                                                .Where(i => i.CategoryId == categoryId);

            return PartialView("_SubCategoryRemovePartial", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubcategory(UpdateCategoryViewModel viewModel)
        {
            SubCategory addSub = new SubCategory
            {
                SubCategoryName = viewModel.SubcategoryName,
                CategoryId = viewModel.CategoryId
            };

            ApplicationDbContext.SubCategories.Add(addSub);
            ApplicationDbContext.SaveChanges();

            return RedirectToAction("LoadSubCategory", new { categoryId = viewModel.CategoryId });
        }


        /****************************Update Subcategory**********************************************/


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSubcategories(UpdateSubcategoriesViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            SubCategory subToUpdate = ApplicationDbContext.SubCategories
                                                          .First(i => i.SubCategoryId == viewModel.SubcategoryId);

            subToUpdate.SubCategoryName = viewModel.NewSubcategoryName;

            ApplicationDbContext.SaveChanges();

            ModelState.Clear();

            UpdateSubcategoriesViewModel updatedViewModel = new UpdateSubcategoriesViewModel
            {
                SubcategoryId = viewModel.SubcategoryId,
                SubcategoryName = viewModel.NewSubcategoryName,
                NewSubcategoryName = "",
            };

            return View(updatedViewModel);
        }

        public ActionResult SelectSubcategoriesToUpdate(SelectSubcategoryToUpdateViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.CategoryList = ApplicationDbContext.Categories.ToList();

            return View(viewModel);
        }

        public ActionResult GetUpdateSubcategories(GetUpdateSubcategoriesViewModel viewModel)
        {
            string subcategory = ApplicationDbContext.SubCategories
                 .Where(i => i.SubCategoryId == viewModel.SubcategoryId)
                 .Select(i => i.SubCategoryName).FirstOrDefault();

            UpdateSubcategoriesViewModel subcatViewModel = new UpdateSubcategoriesViewModel
            {
                SubcategoryName = subcategory
            };

            return View("UpdateSubcategories", subcatViewModel);
        }


        public ActionResult UpdateSubcategories()
        {
            return RedirectToAction("SelectSubcategoriesToUpdate");
        }


        /***********************Delete Item************************************/


        public ActionResult SelectItemToDelete(SelectItemToDeleteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.Items = ApplicationDbContext.Items.ToList();

            return View(viewModel);
        }

        public ActionResult DeleteItem()
        {
            return RedirectToAction("SelectItemToDelete");
        }

        [HttpPost]
        public ActionResult DeleteItem(int itemId)
        {
            Item getItemToDelete = ApplicationDbContext.Items.First(i => i.ItemId == itemId);

            ApplicationDbContext.Items.Remove(getItemToDelete);

            ApplicationDbContext.SaveChanges();

            return RedirectToAction("SelectItemToDelete");
        }


        /************************** Delete Category*******************************/


        public ActionResult SelectCategoryToDelete(SelectCategoryToDeleteViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            viewModel.Categories = ApplicationDbContext.Categories.ToList();

            return View(viewModel);
        }

        public ActionResult DeleteCategory()
        {
            return RedirectToAction("SelectCategoryToDelete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCategory(int categoryId)
        {
            Category getCategoryToDelete = ApplicationDbContext.Categories.First(i => i.CategoryId == categoryId);

            List<Item> getItemsToDelete = ApplicationDbContext.Items.Where(i => i.CategoryId == categoryId).ToList();

            List<SubCategory> getSubcategoriesToDelete = ApplicationDbContext.SubCategories.Where(i => i.CategoryId == categoryId).ToList();

            //Remove Category Items from junction table subcategoryItems
            if (getItemsToDelete.Count > 0)
            {
                try
                {
                    foreach (var item in getItemsToDelete)
                    {
                        SubCategoryItems subcatItem = ApplicationDbContext.SubCategoryItems.First(i => i.ItemId == item.ItemId);

                        ApplicationDbContext.SubCategoryItems.Remove(subcatItem);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            try
            {
                ApplicationDbContext.Categories.Remove(getCategoryToDelete);

                if (getSubcategoriesToDelete.Count > 0)
                {
                    foreach (var subcat in getSubcategoriesToDelete)
                    {
                        ApplicationDbContext.SubCategories.Remove(subcat);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            ApplicationDbContext.SaveChanges();

            return RedirectToAction("SelectCategoryToDelete");
        }
    }
}