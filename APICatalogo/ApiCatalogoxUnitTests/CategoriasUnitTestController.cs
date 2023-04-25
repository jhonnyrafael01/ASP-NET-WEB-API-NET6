using APICatalogo.Context;
using APICatalogo.Controllers;
using APICatalogo.DTOs;
using APICatalogo.DTOs.Mappings;
using APICatalogo.Repository;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Xunit;

namespace ApiCatalogoxUnitTests
{
    public class CategoriasUnitTestController
    {
        private IUnitOfWork repository;
        private IMapper mapper;

        public static DbContextOptions<AppDbContext> dbContextOptions { get; }

        public static string connectionString = "Server=localhost;DataBase=ApiCatalogoDB;Uid=root;Pwd=admin123";

        static CategoriasUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
               .UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
               .Options;
        }


        public CategoriasUnitTestController()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            });
            mapper = config.CreateMapper();

            var context = new AppDbContext(dbContextOptions);

            //DBUnitTestsMockInitializer db = new DBUnitTestsMockInitializer();
            //db.Seed(context);

            repository = new UnitOfWork(context);
        }

        //testes unitários ===================================================
        // testar o método GET
        [Fact]
        public async Task GetCategorias_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = await controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);
        }

        //GET - BadRequest
        [Fact]
        public async Task GetCategorias_Return_BadRequestResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = await controller.Get();
            var statusCode = (data.Result as StatusCodeResult)?.StatusCode;

            //Assert
            Assert.Equal(400, statusCode);
            // Assert.IsType<BadRequestResult>(data);
        }

        //GET retornar uma lista de objetos categoria
        [Fact]
        public async Task GetCategorias_MatchResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            //Act
            var data = await controller.Get();

            //Assert
            Assert.IsType<List<CategoriaDTO>>(data.Value);
            var cat = data.Value.Should().BeAssignableTo<List<CategoriaDTO>>().Subject;

            Assert.Equal("Bebidas", cat[0].Nome);
            Assert.Equal("bebidas.jpg", cat[0].ImagemUrl);

            Assert.Equal("Sobremesas", cat[2].Nome);
            Assert.Equal("sobremesas.jpg", cat[2].ImagemUrl);
        }
        //get por id // retornar um objeto CategoriaDTO
        [Fact]
        public async Task GetCategoriaById_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var carId = 1;

            //Act
            var data = await controller.Get(carId);

            //Assert
            Assert.IsType<CategoriaDTO>(data.Value);
        }

        //get por id -> notfound
        [Fact]
        public async Task GetCategoriaById_Return_Notfund()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var carId = 999;

            //Act
            var data = await controller.Get(carId);
            var statusCode = (data.Result as StatusCodeResult)?.StatusCode;

            //Assert
            //Assert.IsType<NotFoundResult>(data.Value);
            Assert.Equal(404, statusCode);
        }

        //POST - CreatedResult
        [Fact]
        public async Task Post_Categoria_AddValidData_Return_CreatedResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);

            var cat = new CategoriaDTO() { Nome = "Teste Unitario Inclusao", ImagemUrl = "testecatInclusao.jpg" };

            //Act
            var data = await controller.Post(cat);

            //Assert
            Assert.IsType<CreatedAtRouteResult>(data);
        }

       /* //put - alterar objeto categoria
        [Fact]
        public async Task Put_Categoria_Update_ValidData_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var carId = 5;

            //Act
            var existingPost = await controller.Get(carId);
            var result = existingPost.Value.Should().BeAssignableTo<CategoriaDTO>().Subject;

            var catDto = new CategoriaDTO();
            catDto.CategoriaId = carId;
            catDto.Nome = "Categoria Alterada - Testes 1";
            catDto.ImagemUrl = result.ImagemUrl;

            var updateData = await controller.Put(5, catDto);

            //Assert
            Assert.IsType<OkResult>(updateData);
        }*/

        //Delete --id -
        [Fact]
        public async Task Delete_Categoria_Return_OkResult()
        {
            //Arrange
            var controller = new CategoriasController(repository, mapper);
            var carId = 7;

            //Act
            var data = await controller.Delete(carId);

            //Assert
            Assert.IsType<OkResult>(data);
        }
    }
}
