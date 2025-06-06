﻿using MediatR;
using MercerStore.Web.Application.Handlers.Categories.Queries;
using MercerStore.Web.Application.Handlers.Products.Commands;
using MercerStore.Web.Application.Handlers.Products.Queries;
using MercerStore.Web.Areas.Admin.ViewModels.Products;
using MercerStore.Web.Infrastructure.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MercerStore.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = $"{RoleNames.Admin},{RoleNames.Manager}")]
public class ProductController : Controller
{
    private readonly IMediator _mediator;

    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> SelectCategory(CancellationToken ct)
    {
        var categories = await _mediator.Send(new GetCategoriesQuery(), ct);
        return View(categories);
    }

    public IActionResult SearchPage()
    {
        return View();
    }

    public IActionResult ProductPage()
    {
        return View();
    }

    [HttpGet("[area]/[controller]/create/{categoryId}")]
    public IActionResult CreateProduct(int categoryId)
    {
        var viewModel = new CreateProductViewModel
        {
            CategoryId = categoryId
        };
        return View(viewModel);
    }

    [HttpPost("[area]/[controller]/create/{categoryId}")]
    public async Task<IActionResult> CreateProduct(CreateProductViewModel createViewModel, int categoryId,
        CancellationToken ct)
    {
        if (!ModelState.IsValid) return View(createViewModel);

        await _mediator.Send(new CreateProductCommand(createViewModel, categoryId), ct);

        return RedirectToAction("CreateProduct");
    }

    public async Task<IActionResult> UpdateSkus(CancellationToken ct)
    {
        await _mediator.Send(new UpdateSkusCommand(), ct);
        return Ok("Skus обновление успешно");
    }

    [HttpGet("[area]/[controller]/update/{productId}")]
    public async Task<IActionResult> UpdateProduct(int productId, CancellationToken ct)
    {
        var updateProductViewModel = await _mediator.Send(new GetUpdateProductViewModelQuery(productId), ct);
        return View(updateProductViewModel);
    }

    [HttpPost("[area]/[controller]/update/{productId}")]
    public async Task<IActionResult> UpdateProduct(UpdateProductViewModel updateProductViewModel, CancellationToken ct)
    {
        var result = await _mediator.Send(new UpdateProductCommand(updateProductViewModel), ct);
        if (!result.IsSuccess)
        {
            ModelState.AddModelError("", result.ErrorMessage);
            return BadRequest();
        }

        return RedirectToAction("Index", "Dashboard");
    }

    [HttpGet]
    public async Task<IActionResult> IndexAllProducts(CancellationToken ct)
    {
        await _mediator.Send(new IndexAllProductsQuery(), ct);
        return Ok("Все продукты были проиндексированны.");
    }
}