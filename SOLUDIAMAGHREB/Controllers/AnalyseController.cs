using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NPOI.XWPF.UserModel;
using SOLUDIAMAGHREB.Data;
using SOLUDIAMAGHREB.Models;
using SOLUDIAMAGHREB.Models.ViewModel;

namespace SOLUDIAMAGHREB.Controllers
{
    public class AnalyseController : Controller
    {
        private readonly DbsoludiaContext _dbcontext;
        private readonly ILogger<AnalyseController> _logger;
        public AnalyseController(DbsoludiaContext _context, ILogger<AnalyseController> logger)
        {
            _dbcontext = _context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetArticlesByDate(DateTime selectedDate)
        {
            try
            {
                // Fetch articles (Analyse) with their corresponding BordereauItem details
                var articles = _dbcontext.MyAnalyses
                    .Where(a => a.DateCreation.Date == selectedDate.Date)
                    .Select(a => new
                    {

                        a.Nlot,
                        a.Montant_total_DHS,

                        BordereauItemDetails = _dbcontext.bordereauItems
                            .Where(bi => bi.Nlot == a.Nlot && bi.NomberBordereau == a.NomberBordereau)
                            .Select(bi => new
                            {
                                bi.Quantite,
                                bi.Prix_Unit_TVA
                                // bi.Designation

                            })
                            .FirstOrDefault()
                    })
                    .ToList();

                // Transform the results to match the frontend requirements
                var result = articles.Select(a => new
                {
                    nlot = a.Nlot,
                    MontantTotalDHS = a.Montant_total_DHS,
                    Quantite = a.BordereauItemDetails?.Quantite ?? 0,
                    // Designation = a.BordereauItemDetails?.Designation ?? "",
                    PrixUnitaire = a.Montant_total_DHS / a.BordereauItemDetails?.Quantite
                }).ToList();

                return Json(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching articles by date");
                return StatusCode(500, "An error occurred while fetching articles");
            }
        }

        [HttpGet]
        public IActionResult GetAvailableArticle()
        {
            try
            {
                // Fetch unique dates from Analyse table
                var dates = _dbcontext.MyAnalyses
                    .Select(a => a.DateCreation.Date)
                    .Distinct()
                    .OrderByDescending(d => d)
                    .ToList();

                return Json(dates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching available dates");
                return StatusCode(500, "An error occurred while fetching dates");
            }
        }

        [HttpGet]
        public IActionResult GetUniqueArticles()
        {
            try
            {
                // Fetch unique articles (combinations of Nlot and Designation)
                var articles = _dbcontext.MyAnalyses
                    .Join(_dbcontext.bordereauItems,
                        a => new { a.Nlot, a.NomberBordereau },
                        bi => new { bi.Nlot, bi.NomberBordereau },
                        (a, bi) => new
                        {
                            Nlot = a.Nlot,
                            Designation = bi.Designation
                        })
                    .Distinct()
                    .OrderBy(a => a.Nlot)
                    .ToList();

                return Json(articles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching unique articles");
                return StatusCode(500, "An error occurred while fetching articles");
            }
        }

        [HttpGet]
        public IActionResult GetDatesByArticle(int nlot)
        {
            try
            {
                var dates = _dbcontext.MyAnalyses
                    .Where(a => a.Nlot == nlot)
                    .Select(a => a.DateCreation.Date)
                    .Distinct()
                    .OrderByDescending(d => d)
                    .ToList();

                return Json(dates);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching dates for article");
                return StatusCode(500, "An error occurred while fetching dates");
            }
        }


        [HttpGet]
        public IActionResult AvisAppelOffer()
        {

            //string AvisAppelOffers = GetNumAvisAppelOffer();
            // Get the list of bordereau numbers
            var bordereauNumbers = _dbcontext.bordereauManagers
                .Select(bm => bm.NomberBordereau)
                .ToList();

            //Create a new instance of the view model
            var viewModel = new AnalyseDataViewModel
            {
                //NomberBordereau = generatedNomberBordereau,
                //  idAvisAppelOff = AvisAppelOffers,
                BordereauNumbers = bordereauNumbers.Select(b => new SelectListItem { Value = b, Text = b }).ToList(),
            };

            return View(viewModel);


        }

        [HttpGet]

        public IActionResult SuivieArticle()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ImportAvisAppelOffre(IFormFile file, string nomberBordereau, DateTime dateCreation)
        {
            try
            {
                // Validate inputs
                if (file == null || file.Length == 0)
                {
                    return BadRequest("No file uploaded.");
                }

                // Parse lots from the Word document
                List<Analyse> lots = ParseLotsFromWordDocument(file);

                // Generate unique ID for Avis Appel Offre
                //  string idAvisAppelOff = GetNumAvisAppelOffer();

                // Add lots to database
                foreach (var lot in lots)
                {
                    // lot.idAvisAppelOff = idAvisAppelOff;
                    lot.NomberBordereau = nomberBordereau;
                    lot.DateCreation = dateCreation;

                    _dbcontext.MyAnalyses.Add(lot);
                }

                await _dbcontext.SaveChangesAsync();

                return Ok(new
                {
                    message = "Lots imported successfully",
                    //  IdAvisAppelOff = idAvisAppelOff,
                    lotsCount = lots.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error importing Avis Appel Offre");
                return StatusCode(500, "An error occurred while importing the file.");
            }
        }

        //private List<Analyse> ParseLotsFromWordDocument(IFormFile file)
        //{
        //    var lots = new List<Analyse>();

        //    using (var stream = file.OpenReadStream())
        //    {
        //        XWPFDocument document = new XWPFDocument(stream);

        //        // Get all text from the document
        //        string documentText = string.Join(" ",
        //            document.Paragraphs.Select(p => p.Text));

        //        // Regex pattern to extract lot information
        //        // Assumes lots are in a table-like format with consistent pattern
        //        var lotMatches = Regex.Matches(documentText,
        //            @"(\d+)\s+(\d+(?:\s*\d{3})*(?:,\d+)?)\s+([^\d]+)");

        //        foreach (Match match in lotMatches)
        //        {
        //            if (match.Groups.Count >= 4)
        //            {
        //                // Parse lot number, montant en chiffres, and montant en lettres
        //                if (int.TryParse(match.Groups[1].Value.Trim(), out int lotNumber) &&
        //                    decimal.TryParse(match.Groups[2].Value.Replace(" ", "").Replace(",", "."), out decimal montantChiffres))
        //                {
        //                    lots.Add(new Analyse
        //                    {
        //                        Nlot = lotNumber,
        //                        Montant_total_DHS = montantChiffres,
        //                        Montant_total_littre_DHS = match.Groups[3].Value.Trim()
        //                    });
        //                }
        //            }
        //        }
        //    }

        //    return lots;
        //}

        private List<Analyse> ParseLotsFromWordDocument(IFormFile file)
        {
            var lots = new List<Analyse>();

            using (var stream = file.OpenReadStream())
            {
                XWPFDocument document = new XWPFDocument(stream);

                // Extract tables from the document
                foreach (XWPFTable table in document.Tables)
                {
                    // Skip the header row
                    for (int i = 1; i < table.Rows.Count; i++)
                    {
                        XWPFTableRow row = table.Rows[i];

                        // Ensure the row has enough cells
                        if (row.GetTableCells().Count >= 3)
                        {
                            string lotNumber = row.GetTableCells()[0].GetText().Trim();
                            string montantChiffres = row.GetTableCells()[1].GetText().Trim().Replace(" ", "");
                            string montantLettres = row.GetTableCells()[2].GetText().Trim();

                            // Try to parse lot number and montant
                            if (int.TryParse(lotNumber, out int parsedLotNumber) &&
                                decimal.TryParse(montantChiffres, out decimal parsedMontant))
                            {
                                lots.Add(new Analyse
                                {
                                    Nlot = parsedLotNumber,
                                    Montant_total_DHS = parsedMontant,
                                    Montant_total_littre_DHS = montantLettres
                                });
                            }
                        }
                    }
                }

                // Fallback to regex parsing if no table found or parsing failed
                if (lots.Count == 0)
                {
                    // Get all text from the document
                    string documentText = string.Join(" ",
                        document.Paragraphs.Select(p => p.Text));

                    // Regex pattern to extract lot information
                    var lotMatches = Regex.Matches(documentText,
                        @"(\d+)\s+(\d+(?:\s*\d{3})*(?:,\d+)?)\s+([^\d]+)");

                    foreach (Match match in lotMatches)
                    {
                        if (match.Groups.Count >= 4)
                        {
                            // Parse lot number, montant en chiffres, and montant en lettres
                            if (int.TryParse(match.Groups[1].Value.Trim(), out int lotNumber) &&
                                decimal.TryParse(match.Groups[2].Value.Replace(" ", "").Replace(",", "."), out decimal montantChiffres))
                            {
                                lots.Add(new Analyse
                                {
                                    Nlot = lotNumber,
                                    Montant_total_DHS = montantChiffres,
                                    Montant_total_littre_DHS = match.Groups[3].Value.Trim()
                                });
                            }
                        }
                    }
                }
            }

            return lots;
        }
        //[HttpPost]
        //public IActionResult SaveAvisAppelOff([FromBody] AnalyseDataViewModel model)
        //{


        //    try
        //    {

        //        if (string.IsNullOrWhiteSpace(model.NomberBordereau))
        //        {
        //            return Json(new { success = false });


        //        }

        //        var AnalyseData = new Analyse
        //        {

        //            idAvisAppelOff = model.idAvisAppelOff,
        //            NomberBordereau = model.NomberBordereau,
        //            DateCreation = model.DateCreation,
        //            //Intitu_AppelOffres = model.Intitu_AppelOffres
        //        };


        //        _dbcontext.MyAnalyses.Add(AnalyseData);
        //        _dbcontext.SaveChanges();

        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error saving bordereau: {ex.Message}");
        //        return Json(new { success = false, message = ex.Message });
        //    }
        //}

        //[HttpPost]
        //public IActionResult EnviarDatos([FromForm] IFormFile ArchivoExcel, [FromForm] string NomberBordereau)
        //{
        //    if (ArchivoExcel == null || ArchivoExcel.Length == 0)
        //    {
        //        return BadRequest("No file uploaded");
        //    }

        //    try
        //    {
        //        using var stream = ArchivoExcel.OpenReadStream();
        //        IWorkbook workbook;

        //        if (Path.GetExtension(ArchivoExcel.FileName).ToLower() == ".docx")
        //        {
        //            workbook = new XSSFWorkbook(stream);
        //        }
        //        else
        //        {
        //            workbook = new HSSFWorkbook(stream);
        //        }

        //        var sheet = workbook.GetSheetAt(0);
        //        var rows = sheet.GetRowEnumerator();

        //        var Analyse = new List<Analyse>();

        //        // Skip header row
        //        rows.MoveNext();

        //        while (rows.MoveNext())
        //        {
        //            var row = (IRow)rows.Current;
        //            try
        //            {
        //                var item = new Analyse
        //                {
        //                    Nlot = GetCellValueAsInt(row.GetCell(0)),
        //                    Montant_total_DHS = GetCellValueAsDecimal(row.GetCell(2)),
        //                    Montant_total_littre_DHS = GetCellValueAsString(row.GetCell(3)),
        //                    NomberBordereau = NomberBordereau // Use the provided NomberBordereau

        //                };
        //                Analyse.Add(item);
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogError($"Error processing row {row.RowNum}: {ex.Message}");
        //                continue;
        //            }
        //        }

        //        _dbcontext.BulkInsert(Analyse);

        //        return Ok(new { mensaje = "ok" });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}




        ///////////////////////// Function and Methods ////////////////////////
        //private string GetNumAvisAppelOffer()
        //{
        //    var latestNumAvisAppelOff = _dbcontext.MyAnalyses
        //        .OrderByDescending(am => am.idAvisAppelOff)
        //        .FirstOrDefault();

        //    int nextIdAvisAppelOff = 1; // Start with 1 if there are no entries
        //    if (latestNumAvisAppelOff != null)
        //    {
        //        string latestIdAvisApploff = latestNumAvisAppelOff.idAvisAppelOff;

        //        // Check if the ID starts with "Nbord" and get the numeric part
        //        if (latestIdAvisApploff.StartsWith("N") && int.TryParse(latestIdAvisApploff.Substring(8), out int idNumber))
        //        {
        //            nextIdAvisAppelOff = idNumber + 1;
        //        }
        //    }

        //    // Return the new ID in the format "Nbord00001"
        //    return $"N°{nextIdAvisAppelOff:D5}";
        //}
    }
}
