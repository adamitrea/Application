using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application.Data;
using Microsoft.AspNetCore.Authorization;
using Application_DbAccess;

namespace Application.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly RepositoryMovie _repoMovie;

        public MoviesController(RepositoryMovie repoMovie)
        {
            _repoMovie = repoMovie;
        }

        // GET: Movies
        public IActionResult Index()
        {
            return View(_repoMovie.GetAll().ToList());
        }
    }
}
