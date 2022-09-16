using Imm.DAL.Data;
using Imm.DAL.Data.Table;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Imm.BLL
{
    public class ServiceCommon
    {
        private readonly ILogger<AspNetUsers> _logger = null;
        private readonly NovaDbContext _context = null;

        public ServiceCommon(NovaDbContext context, ILogger<AspNetUsers> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger;
        }

        public async Task<List<Country>> GetCountryAsync()
        {
            return await _context.Country.Select(x => new Country()
            {
                CountryId = x.CountryId,
                CountryName = x.CountryName

            }).ToListAsync();
        }


        public async Task<List<State>> GetStateAsync(int countryId)
        {
            return await _context.State.Where(x => x.CountryId == countryId).Select(state => new State()
            {
                StateId = state.StateId,
                StateName = state.StateName
            }).ToListAsync();
        }


        public async Task<List<City>> GetCityAsync(int stateId)
        {
            return await _context.City.Where(x => x.StateId == stateId).Select(city => new City()
            {
                CityId = city.CityId,
                CityName = city.CityName
            }).ToListAsync();
        }


        public async Task<List<string>> MultipleFileUploadAsync(AspNetUsersDocs _file, string rootPath, string email)
        {
            string uniqueFileName = null;
            string filePath = null;
            List<string> url = new List<string>();
            var uId = _context.AspNetUsers.Where(x => x.Email == email).Select(o => o.UserId).FirstOrDefault();
            if (_file.Document != null && _file.Document.Count > 0 && uId > 0)
            {
                int count = 0;
                var fName = "";
                foreach (IFormFile item in _file.Document)
                {
                    count++;
                    if(item != null)
                    {
                        if (count == 1)
                            fName = uId + "_logo";
                        if (count == 2)
                            fName = uId + "_cover";
                        if (count == 3)
                            fName = uId + "_certificate";
                        /// string ext = Path.GetExtension(model.AgentDocs.BusinessCertificate.FileName).ToLower();
                        string uploadsFolder = Path.Combine(rootPath + "/images/img");
                        uniqueFileName = fName + "_" + item.FileName;
                        filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        item.CopyTo(new FileStream(filePath, FileMode.Create));
                        _logger.LogError("File uploaded succesfully, writing to database.");
                        url.Add(filePath);

                        AspNetUsersDocs finFile = new AspNetUsersDocs()
                        {
                            DocumentName = uniqueFileName,
                            DocumentURL = filePath,
                            UserId = uId
                        };

                        if (count == 1)
                            finFile.Type = "Business Logo";
                        if (count == 2)
                            finFile.Type = "Cover Photo";
                        if (count == 3)
                            finFile.Type = "Business Certificate";

                        _context.AspNetUserDocs.Add(finFile);
                            await _context.SaveChangesAsync();
                    }
                    continue; 
                }
                return url;
            }
            _logger.LogError("Something error in ServiceCommon");
            return null;
        }


        /// Currently not in use
        public AspNetUsersDocs SingleFileUploadAsync(AspNetUsersDocs _file, string rootPath)
        {
            string uniqueFileName = null;
            string filePath = null;
            if (_file.Document != null && _file.Document.Count == 1)
            {
                foreach (IFormFile item in _file.Document)
                {
                    string uploadsFolder = Path.Combine(rootPath + "/images/img");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + item.FileName;
                    filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    item.CopyTo(new FileStream(filePath, FileMode.Create));
                }
            }
            return _file;
        }
        /// Currently not in use End

        public async Task<string> ViewFileAsync(int docId)
        {
            var res = await _context.AspNetUserDocs.FindAsync(docId);
            return res.DocumentURL.ToString();
        }


        public async Task<bool> ResetPasswordAsync(AspNetUsers model)
        {
            try
            {
                var user = await _context.AspNetUsers.FindAsync(model.UserId);
                if (user != null)
                {
                    user.Password = model.Password;
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, "Something wrng with ServiceCommon.ResetpasswordAsync");
                return false;
            }
        }
    }
}
