using Microsoft.EntityFrameworkCore;
using Sandwich2Go.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace Sandwich2Go.UT.IngredientesController_test
{
    public class SelectIngredientes_test
    {
        private DbContextOptions<ApplicationDbContext> _contextOptions;
        private ApplicationDbContext context;
        Microsoft.AspNetCore.Http.DefaultHttpContext purchaseContext;

        public SelectIngredientes_test()
        {
            //Initialize the Database
            _contextOptions = UtilitiesForIngredientes.CreateNewContextOptions();
            context = new ApplicationDbContext(_contextOptions);
            context.Database.EnsureCreated();

            // Seed the InMemory database with test data.
            UtilitiesForIngredientes.InitializeDbMoviesForTests(context);

            //how to simulate the connection of a user
            System.Security.Principal.GenericIdentity user = new System.Security.Principal.GenericIdentity("peter@uclm.com");
            System.Security.Claims.ClaimsPrincipal identity = new System.Security.Claims.ClaimsPrincipal(user);
            purchaseContext = new Microsoft.AspNetCore.Http.DefaultHttpContext();
            purchaseContext.User = identity;

        }
        public static IEnumerable<object[]> TestCasesForSelectIngredientesForPurchase_get()
        {
            var allTests = new List<object[]>
            {
                new object[] { UtilitiesForIngredientes.GetMovies(0,3), UtilitiesForIngredientes.GetAlergenos(0,4), null, null },
                new object[] { UtilitiesForIngredientes.GetMovies(0,1), UtilitiesForMovies.GetGenres(0,4), "lord", null},
                new object[] { UtilitiesForMovies.GetMovies(2,1), UtilitiesForMovies.GetGenres(0,4), null, "Drama"},
            };

            return allTests;
        }

        [Theory]
        [MemberData(nameof(TestCasesForSelectMoviesForPurchase_get))]
        [Trait("LevelTesting", "Unit Testing")]
        public async Task SelectMoviesForPurchase_Get(List<Movie> expectedMovies, List<Genre> expectedGenres, string filterTitle, string filterGenre)
        {
            using (context)
            {

                // Arrange
                var controller = new MoviesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                var expectedGenresNames = expectedGenres.Select(g => new { nameofGenre = g.Name });

                // Act
                var result = controller.SelectMoviesForPurchase(filterTitle, filterGenre);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMoviesForPurchaseViewModel model = viewResult.Model as SelectMoviesForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                // You must implement Equals in Movies, otherwise Assert will fail
                Assert.Equal(expectedMovies, model.Movies);
                //check that both collections (expected and result) have the same names of Genre
                var modelGenres = model.Genres.Select(g => new { nameofGenre = g.Text });
                Assert.True(expectedGenresNames.SequenceEqual(modelGenres));
            }
        }


        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectMoviesForPurchase_Post_MoviesNotSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new MoviesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;
                //we create an array that is a list names of genres
                var expectedGenres = UtilitiesForMovies.GetGenres(0, 4).Select(g => new { nameofGenre = g.Name });
                var expectedMovies = UtilitiesForMovies.GetMovies(0, 3);

                SelectedMoviesForPurchaseViewModel selected = new SelectedMoviesForPurchaseViewModel { IdsToAdd = null };

                // Act
                var result = controller.SelectMoviesForPurchase(selected);

                //Assert
                var viewResult = Assert.IsType<ViewResult>(result); // Check the controller returns a view
                SelectMoviesForPurchaseViewModel model = viewResult.Model as SelectMoviesForPurchaseViewModel;

                // Check that both collections (expected and result returned) have the same elements with the same name
                Assert.Equal(expectedMovies, model.Movies);

                //check that both collections (expected and result) have the same names of Genre
                var modelGenres = model.Genres.Select(g => new { nameofGenre = g.Text });
                Assert.True(expectedGenres.SequenceEqual(modelGenres));

            }
        }

        [Fact]
        [Trait("LevelTesting", "Unit Testing")]
        public void SelectMoviesForPurchase_Post_MoviesSelected()
        {
            using (context)
            {

                // Arrange
                var controller = new MoviesController(context);
                controller.ControllerContext.HttpContext = purchaseContext;

                String[] ids = new string[1] { "1" };
                SelectedMoviesForPurchaseViewModel movies = new SelectedMoviesForPurchaseViewModel { IdsToAdd = ids };

                // Act
                var result = controller.SelectMoviesForPurchase(movies);

                //Assert
                var viewResult = Assert.IsType<RedirectToActionResult>(result);
                var currentMovies = viewResult.RouteValues.Values.First();
                Assert.Equal(movies.IdsToAdd, currentMovies);

            }
        }
    }
}
