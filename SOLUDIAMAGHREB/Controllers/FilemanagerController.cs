using System.Security.Claims;
using EFCore.BulkExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Rotativa.AspNetCore;
using SOLUDIAMAGHREB.Data;
using SOLUDIAMAGHREB.Models;
using SOLUDIAMAGHREB.Models.ViewModel;
using SOLUDIAMAGHREB.Resources;


namespace SOLUDIAMAGHREB.Controllers
{
    //[Authorize]

    public class FilemanagerController : Controller
    {

        private readonly DbsoludiaContext _dbcontext;
        private readonly ILogger<FilemanagerController> _logger;
        public FilemanagerController(DbsoludiaContext _context, ILogger<FilemanagerController> logger)
        {
            _dbcontext = _context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GenerateDeclarationLH(string selectedBordereauDec = null)
        {
            var viewModel = new ViewModelDeclarationLh();
            // if (!string.IsNullOrEmpty(selectedBordereau))
            //  {

            viewModel.BordereauNumbers = _dbcontext.MyBorderauItems
                 .Select(b => b.NomberBordereau)
                 .Distinct()
                 .OrderBy(n => n)
                 .Select(n => new SelectListItem
                 {
                     Value = n,
                     Text = n,
                     Selected = n == selectedBordereauDec
                 })
                 .ToList();


            var result = _dbcontext.Actmanagers
              .Where(b => b.NomberBordereau == selectedBordereauDec)
              .Select(b => new
              {
                  b.Objet_du_Marche,
                  b.Capital,
                  b.Appel_Offres,
                  b.DateCreation
              })
              .FirstOrDefault();

            if (result != null)
            {
                viewModel.ObjectMarche = result.Objet_du_Marche;
                viewModel.Capital = result.Capital;
                viewModel.AppelOffer = result.Appel_Offres;
                viewModel.DateCreation = result.DateCreation;
            }
            else
            {
                viewModel.ObjectMarche = null;
                viewModel.Capital = null;
                viewModel.AppelOffer = null;
                viewModel.DateCreation = DateTime.Now;

            }


            return View(viewModel);
        }
        [HttpPost]
        public IActionResult GenerateDeclarationLH(ViewModelDeclarationLh model)
        {
            if (!string.IsNullOrEmpty(model.NomberBordereau))
            {
                // Redirect to GET action with selected bordereau
                return RedirectToAction("GenerateDeclarationLH", new { selectedBordereauDec = model.NomberBordereau });
            }
            return RedirectToAction("GenerateDeclarationLH");
        }
        [HttpPost]
        public IActionResult SaveDeclarationLh([FromBody] ViewModelDeclarationLh model)
        {

            try
            {
                // Generate unique ID
                string idDeclar = GenerateIdDeclar();


                bool declarationExists = _dbcontext.MyDeclarationlhs.Any(my => my.NomberBordereau == model.NomberBordereau);
                if (!declarationExists)
                {
                    var DeclarationLh = new Declarationlh
                    {
                        IdDeclar = idDeclar,
                        AppelOffer = model.AppelOffer,
                        ObjectMarche = model.ObjectMarche,
                        Capital = model.Capital,
                        DateCreation = model.DateCreation,
                        NomberBordereau = model.NomberBordereau
                    };

                    _dbcontext.MyDeclarationlhs.Add(DeclarationLh);
                    _dbcontext.SaveChanges();

                    return Json(new
                    {
                        success = true,
                        message = $"Déclaration sur l'honneur {idDeclar} saved successfully!",
                        idDeclar = idDeclar,


                    });
                    // return RedirectToAction("ListActEngage");
                }
                else
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Déclaration sur l'honneur with ID {idDeclar} already exists. Please try again."
                    });
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving Déclaration sur l'honneur: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Error saving Déclaration sur l'honneur: " + ex.Message
                });
            }

        }
        [HttpGet]
        public IActionResult ListeDeclarationLH()
        {
            List<Declarationlh> List = _dbcontext.MyDeclarationlhs.ToList();

            return View(List);

        }

        [HttpGet]
        public IActionResult ListActEngage()
        {
            List<Actmanager> List = _dbcontext.Actmanagers.ToList();
            return View(List);
        }
        [HttpGet]
        public IActionResult ListeColisage()
        {
            return View();
        }
        [HttpGet]

        public IActionResult GenerateActEngage(string selectedBordereau = null)
        {

            var viewModel = new ViewModelActManager();

            // Get all distinct bordereau numbers
            viewModel.BordereauNumbers = _dbcontext.MyBorderauItems
                .Select(b => b.NomberBordereau)
                .Distinct()
                .OrderBy(n => n)
                .Select(n => new SelectListItem
                {
                    Value = n,
                    Text = n,
                    Selected = n == selectedBordereau
                })
                .ToList();

            // If a bordereau is selected, get its lot numbers
            if (!string.IsNullOrEmpty(selectedBordereau))
            {
                viewModel.NLotNumbers = _dbcontext.MyBorderauItems
                    .Where(b => b.NomberBordereau == selectedBordereau)
                    .Select(b => b.Nlot)
                    .Distinct()
                    .OrderBy(n => n)
                    .Select(n => new SelectListItem
                    {
                        Value = n.ToString(),
                        Text = n.ToString()
                    })
                    .ToList();

                viewModel.NomberBordereau = selectedBordereau;

                // Get the Appel_Offres for the selected bordereau
                viewModel.Appel_Offres = _dbcontext.MyBorderauItems
                    .Where(b => b.NomberBordereau == selectedBordereau)
                    .Select(b => b.AppelOffres)
                    .FirstOrDefault();
            }
            else
            {
                viewModel.NLotNumbers = new List<SelectListItem>();
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> GenerateActEngage(ViewModelActManager model)
        {
            if (!string.IsNullOrEmpty(model.NomberBordereau))
            {
                // Redirect to GET action with selected bordereau
                return RedirectToAction("GenerateActEngage", new { selectedBordereau = model.NomberBordereau });
            }
            return RedirectToAction("GenerateActEngage");

        }
        [HttpGet]
        public IActionResult GenListeColisage()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetNLotDetails(string bordereau, string nLot)
        {
            var lotDetails = _dbcontext.MyBorderauItems
                .FirstOrDefault(b =>
                    b.NomberBordereau == bordereau &&
                    b.Nlot.ToString() == nLot);

            return Json(new
            {
                prixTotalTva = lotDetails?.Prix_Total_TVA ?? 0,
                montantTvaComprise = lotDetails?.Prix_Total_TVA ?? 0

            });
        }


        [HttpPost]
        public IActionResult SaveActEngagement([FromBody] ViewModelActManager model)
        {
            try
            {
                // Generate unique ID
                string idActEng = GenerateNextId();

                var actManager = new Actmanager
                {
                    IdactEng = idActEng,
                    Appel_Offres = model.Appel_Offres,
                    Objet_du_Marche = model.Objet_du_Marche,
                    Marche_passe = model.Marche_passe,
                    Capital = model.Capital,
                    NLot = model.NLot,
                    TauxTva = model.TauxTva,
                    MontantHtTva = model.MontantHtTva,
                    MontantTva = model.MontantTva,
                    MontantDh = model.MontantDh,
                    MontantTvaComprise = model.MontantTvaComprise,
                    DateCreation = model.DateCreation,
                    NomberBordereau = model.NomberBordereau
                };

                _dbcontext.Actmanagers.Add(actManager);
                _dbcontext.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = $"Act Engagement {idActEng} saved successfully!",
                    idActEng = idActEng,


                });
                // return RedirectToAction("ListActEngage");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving Act Engagement: {ex.Message}");
                return Json(new
                {
                    success = false,
                    message = "Error saving Act Engagement: " + ex.Message
                });
            }

        }



        /////////////////////////////////////
        ///


        [HttpGet]
        public IActionResult ListeBerdereauPrix()
        {
            List<BordereauManager> List = _dbcontext.bordereauManagers.ToList();
            return View(List);
        }
        [HttpGet]
        public IActionResult BordereauPrix()
        {
            // Generate NomberBordereau value
            string generatedNomberBordereau = GenNomberBordereau();

            // Create a new instance of the view model
            var viewModel = new BordereauPrixViewModel
            {
                NomberBordereau = generatedNomberBordereau,
                Items = new List<BordereauItemViewModel>() // Initialize the items list
            };

            // Pass the view model to the view
            return View(viewModel);
            //// Generate NomberBordereau value

        }

        [HttpPost]
        public IActionResult SaveBordereau([FromBody] BordereauPrixViewModel model)
        {
            try
            {
                var bordereauManager = new BordereauManager
                {
                    NomberBordereau = model.NomberBordereau,
                    DateCreation = model.DateCreation,
                    Intitu_AppelOffres = model.Intitu_AppelOffres
                };

                _dbcontext.bordereauManagers.Add(bordereauManager);
                _dbcontext.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving bordereau: {ex.Message}");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetBordereauOptions(string bordereauNumber)
        {
            try
            {
                var bordereauManager = await _dbcontext.bordereauManagers
                    .Where(bm => bm.NomberBordereau == bordereauNumber)
                    .Include(bm => bm.Items)
                    .FirstOrDefaultAsync();

                if (bordereauManager == null)
                {
                    return NotFound();
                }

                var options = new
                {
                    intitu_AppelOffres = bordereauManager.Intitu_AppelOffres, // Changed the property name
                    nlots = bordereauManager.Items.Select(b => b.Nlot).Distinct().ToList(),
                    designations = bordereauManager.Items.Select(b => b.Designation).Distinct().ToList(),
                    uniteComptes = bordereauManager.Items.Select(b => b.Unite_Compte).Distinct().ToList(),
                    quantites = bordereauManager.Items.Select(b => b.Quantite).Distinct().ToList()
                };

                return Json(options);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting bordereau options: {ex.Message}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult SaveMyBorderauItem([FromBody] MyBorderauItems item)
        {
            try
            {
                var exestingLot = _dbcontext.MyBorderauItems
                    .FirstOrDefault(x => x.NomberBordereau == item.NomberBordereau && x.Nlot == item.Nlot);
                if (exestingLot != null)
                {
                    return Json(new
                    {
                        success = false,
                        message = $"Lot number {item.Nlot} already exists for this bordereau. Please use a different lot number or edit bordereau."
                    });
                }
                _dbcontext.MyBorderauItems.Add(item);
                _dbcontext.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error saving bordereau item: {ex.Message}");
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult PreviewBordereau(string bordereauNumber, int nlot)
        {
            try
            {
                // Get the BordereauManager to access its DateCreation
                var bordereauManager = _dbcontext.bordereauManagers
                    .FirstOrDefault(b => b.NomberBordereau == bordereauNumber);
                // Get the bordereau items from the database
                var items = _dbcontext.MyBorderauItems
                    .Where(b => b.NomberBordereau == bordereauNumber && b.Nlot == nlot)
                    .OrderBy(b => b.Nlot)
                    .ToList();

                if (!items.Any())
                {
                    return NotFound("Lot not found in the specified bordereau.");
                }

                // Get the Appel_Offres value from the first item
                string appel_Offres = items.FirstOrDefault()?.AppelOffres ?? string.Empty;

                // Calculate totals
                decimal totalHorsTVA = items.Sum(i => i.Prix_Total_TVA);
                decimal tauxTVA = 0; // Set to 0% as per the template
                decimal totalTTC = totalHorsTVA; // Since TVA is 0%

                // Convert total TTC to words
                string totalTTCInWords = FrenchNumberToWordsConverter.ConvertToWords(totalTTC);

                // Create the view model with the data
                var viewModel = new BordereauPrixPreviewViewModel
                {
                    Items = items,
                    TotalHorsTVA = totalHorsTVA,
                    TauxTVA = tauxTVA,
                    TotalTTC = totalTTC,
                    TotalTTCChrac = totalTTCInWords,
                    DateCreation = bordereauManager?.DateCreation ?? DateTime.Now,
                    AppelOffres = appel_Offres
                };

                return new ViewAsPdf("_BordereauPreview", viewModel)
                {
                    // Show in browser, don't download
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error generating preview: {ex.Message}");
                return StatusCode(500, "Error generating preview");
            }
        }


        [HttpGet]
        public IActionResult GetBordereauItemDetails(string bordereauNumber, int nlot)
        {
            try
            {
                var item = _dbcontext.bordereauItems
                    .Where(b => b.NomberBordereau == bordereauNumber && b.Nlot == nlot)
                    .FirstOrDefault();

                if (item != null)
                {
                    return Json(new
                    {
                        designation = item.Designation,
                        uniteCompte = item.Unite_Compte,
                        quantite = item.Quantite,
                        prixUnitTVA = item.Prix_Unit_TVA,
                        prixTotalTVA = item.Prix_Total_TVA
                    });
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error getting bordereau item details: {ex.Message}");
                return StatusCode(500, "Error getting bordereau item details");
            }
        }


        [HttpGet]
        public IActionResult GenerateBordereauPrix()
        {
            // Generate NomberBordereau value
            string generatedNomberBordereau = GenNomberBordereau();

            // Get the list of bordereau numbers
            var bordereauNumbers = _dbcontext.bordereauManagers
                .Select(bm => bm.NomberBordereau)
                .ToList();

            //Create a new instance of the view model
            var viewModel = new BordereauPrixViewModel
            {
                NomberBordereau = generatedNomberBordereau,
                BordereauNumberOptions = bordereauNumbers.Select(b => new SelectListItem { Value = b, Text = b }).ToList(),
                Items = new List<BordereauItemViewModel>() // Initialize the items list
            };
            //  ViewBag.Roles = new SelectList(bordereauNumbers);

            // Pass the view model to the view
            return View(viewModel);
        }

        /// ////////////////////////////////////////////////////



        [HttpPost]
        public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel, [FromForm] string NomberBordereau)
        {
            if (ArchivoExcel == null || ArchivoExcel.Length == 0)
            {
                return BadRequest("No file uploaded");
            }

            try
            {
                using var stream = ArchivoExcel.OpenReadStream();
                IWorkbook workbook;

                if (Path.GetExtension(ArchivoExcel.FileName).ToLower() == ".xlsx")
                {
                    workbook = new XSSFWorkbook(stream);
                }
                else
                {
                    workbook = new HSSFWorkbook(stream);
                }

                var sheet = workbook.GetSheetAt(0);
                var rows = sheet.GetRowEnumerator();

                var bordereauItems = new List<BordereauItem>();

                // Skip header row
                rows.MoveNext();

                while (rows.MoveNext())
                {
                    var row = (IRow)rows.Current;
                    try
                    {
                        var item = new BordereauItem
                        {
                            Nlot = GetCellValueAsInt(row.GetCell(0)),
                            Designation = GetCellValueAsString(row.GetCell(1)),
                            Unite_Compte = GetCellValueAsString(row.GetCell(2)),
                            Quantite = GetCellValueAsInt(row.GetCell(3)),
                            Prix_Unit_TVA = GetCellValueAsDecimal(row.GetCell(4)),
                            Prix_Total_TVA = GetCellValueAsDecimal(row.GetCell(5)),
                            NomberBordereau = NomberBordereau // Use the provided NomberBordereau
                        };
                        bordereauItems.Add(item);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error processing row {row.RowNum}: {ex.Message}");
                        continue;
                    }
                }

                _dbcontext.BulkInsert(bordereauItems);

                return Ok(new { mensaje = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        // Helper methods for cell value extraction
        private string GetCellValueAsString(ICell cell)
        {
            if (cell == null) return string.Empty;
            return cell.CellType switch
            {
                CellType.String => cell.StringCellValue,
                CellType.Numeric => cell.NumericCellValue.ToString(),
                _ => cell.ToString()
            };
        }

        private int GetCellValueAsInt(ICell cell)
        {
            if (cell == null) return 0;
            return cell.CellType switch
            {
                CellType.Numeric => (int)cell.NumericCellValue,
                CellType.String => int.TryParse(cell.StringCellValue, out int result) ? result : 0,
                _ => 0
            };
        }

        private decimal GetCellValueAsDecimal(ICell cell)
        {
            if (cell == null) return 0m;
            return cell.CellType switch
            {
                CellType.Numeric => (decimal)cell.NumericCellValue,
                CellType.String => decimal.TryParse(cell.StringCellValue, out decimal result) ? result : 0m,
                _ => 0m
            };
        }


        /// ////////////////////////////////////////////////////


        public IActionResult ViewPdf(string IdActEngage)
        {
            ViewModelActManager model = _dbcontext.Actmanagers.Where(Id => Id.IdactEng == IdActEngage)
                .Select(v => new ViewModelActManager
                {
                    Appel_Offres = v.Appel_Offres,
                    Marche_passe = v.Marche_passe,
                    Objet_du_Marche = v.Objet_du_Marche,
                    NLot = v.NLot,
                    Capital = v.Capital,
                    TauxTva = v.TauxTva,
                    MontantHtTva = v.MontantHtTva,
                    MontantTva = v.MontantTva,
                    MontantDh = v.MontantDh,
                    MontantTvaComprise = v.MontantTvaComprise,
                    DateCreation = v.DateCreation,
                }).FirstOrDefault();

            // Generate the PDF and set it to show inline in the browser
            return new ViewAsPdf("_ViewActEngagement", model)
            {
                // Show in browser, don't download
            };
        }
        [HttpGet]
        public IActionResult ImprimerAct(string IdActEngage)
        {
            //TODO ESTO LO REEMPLAZAS CON TU PROPIA LÓGICA HACIA TU BASE DE DATOS
            ViewModelActManager modelo = _dbcontext.Actmanagers.Where(Id => Id.IdactEng == IdActEngage)
               .Select(v => new ViewModelActManager()
               {
                   Appel_Offres = v.Appel_Offres,
                   Marche_passe = v.Marche_passe,
                   Objet_du_Marche = v.Objet_du_Marche,
                   NLot = v.NLot,
                   Capital = v.Capital,
                   TauxTva = v.TauxTva,
                   MontantHtTva = v.MontantHtTva,
                   MontantTva = v.MontantTva,
                   MontantDh = v.MontantDh,
                   MontantTvaComprise = v.MontantTvaComprise,
                   DateCreation = v.DateCreation,
               }).FirstOrDefault();


            //return View();

            return new ViewAsPdf("_ViewActEngagement", modelo)
            {
                FileName = $"{IdActEngage}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                // ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Inline
                ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Attachment

            };
        }

        public IActionResult ViewPdfDeclar(string IdDeclar)
        {

            ViewModelDeclarationLh model = _dbcontext.MyDeclarationlhs.Where(id => id.IdDeclar == IdDeclar)
                .Select(v => new ViewModelDeclarationLh
                {
                    AppelOffer = v.AppelOffer,
                    ObjectMarche = v.ObjectMarche,
                    Capital = v.Capital,
                    DateCreation = v.DateCreation,

                }).FirstOrDefault();

            // Generate the PDF and set it to show inline in the browser
            return new ViewAsPdf("_DeclarationPreview", model)
            {
                // Show in browser, don't download
            };
        }
        [HttpGet]
        public IActionResult ImprimerDeclar(string IdDeclar)
        {

            ViewModelDeclarationLh model = _dbcontext.MyDeclarationlhs.Where(id => id.IdDeclar == IdDeclar)
                .Select(v => new ViewModelDeclarationLh
                {
                    AppelOffer = v.AppelOffer,
                    ObjectMarche = v.ObjectMarche,
                    Capital = v.Capital,
                    DateCreation = v.DateCreation,

                }).FirstOrDefault();


            //return View();

            return new ViewAsPdf("_DeclarationPreview", model)
            {
                FileName = $"{IdDeclar}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                // ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Inline
                ContentDisposition = Rotativa.AspNetCore.Options.ContentDisposition.Attachment

            };
        }


        //////// Scripts est Methods/////////
        private string GenerateIdDeclar()
        {
            var latestIdDeclar = _dbcontext.MyDeclarationlhs
                .OrderByDescending(id => id.IdDeclar)
                .FirstOrDefault();
            int nexIdDeclar = 1;
            if (latestIdDeclar != null)
            {
                string latestIddecl = latestIdDeclar.IdDeclar;
                if (latestIddecl.StartsWith("Declar") && int.TryParse(latestIddecl.Substring(6), out int idNumber))
                {
                    nexIdDeclar = idNumber + 1;
                }
            }
            return $"Declar{nexIdDeclar:D5}"; // Zero-padded to 4 digits

        }
        private string GenerateNextId()
        {
            // Get the latest IdactEng
            var latestActmanager = _dbcontext.Actmanagers
                .OrderByDescending(am => am.IdactEng)
                .FirstOrDefault();

            // Extract the numeric part and increment it
            int nextId = 1; // Default ID if no entries exist
            if (latestActmanager != null)
            {
                string latestId = latestActmanager.IdactEng;
                // Extract the numeric part
                if (latestId.StartsWith("Act") && int.TryParse(latestId.Substring(5), out int idNumber))
                {
                    nextId = idNumber + 1; // Increment by 1
                }
            }

            // Format the new ID as "Files0001", "Files0002", etc.
            return $"Act{nextId:D6}"; // Zero-padded to 4 digits
        }
        public string GenNomberBordereau()
        {
            var latestBordereaumanager = _dbcontext.bordereauManagers
                .OrderByDescending(am => am.NomberBordereau)
                .FirstOrDefault();

            int nextIdBorderau = 1; // Start with 1 if there are no entries
            if (latestBordereaumanager != null)
            {
                string latestIdBorderau = latestBordereaumanager.NomberBordereau;

                // Check if the ID starts with "Nbord" and get the numeric part
                if (latestIdBorderau.StartsWith("APLN°") && int.TryParse(latestIdBorderau.Substring(5), out int idNumber))
                {
                    nextIdBorderau = idNumber + 1;
                }
            }

            // Return the new ID in the format "Nbord00001"
            return $"APLN°{nextIdBorderau:D5}";
        }

        private void IdentityAuto()
        {
            ClaimsPrincipal claimuser = HttpContext.User;
            string Username = "";

            if (claimuser.Identity.IsAuthenticated)
            {
                Username = claimuser.Claims.Where(c => c.Type == ClaimTypes.Name)
                    .Select(c => c.Value).SingleOrDefault();
            }

            ViewData["Username"] = Username;
        }

    }


}

