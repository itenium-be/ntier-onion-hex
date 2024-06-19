using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoItem;
using N_Tier.Application.Services;

namespace N_Tier.API.Controllers;

/// <summary>
/// ATTN 1: Show Project Dependencies
/// </summary>
[Authorize]
public class TodoItemsController : ApiController
{
    private readonly ITodoItemService _todoItemService;

    public TodoItemsController(ITodoItemService todoItemService)
    {
        // Controllers inject an Application Service from the layer below:
        _todoItemService = todoItemService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync(CreateTodoItemModel createTodoItemModel)
    {
        // Fuck debuggability:
        return Ok(ApiResult<CreateTodoItemResponseModel>.Success(
            await _todoItemService.CreateAsync(createTodoItemModel)));

        // Increase debuggability:
        var createdTodoItem = await _todoItemService.CreateAsync(createTodoItemModel);
        var result = ApiResult<CreateTodoItemResponseModel>.Success(createdTodoItem);
        return Ok(result);

        // Better alternative: Simplify with middleware!
        // var todoItem = await _todoItemService.CreateAsync(createTodoItemModel);
        // return todoItem;
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(Guid id, UpdateTodoItemModel updateTodoItemModel)
    {
        // Fuck maintainability:
        // TodoItem, CreateTodoItemModel, UpdateTodoItemModel, CreateTodoItemResponseModel
        // TodoItemResponseModel, UpdateTodoItemResponseModel
        // Oh, we're missing the TodoItemDTO, CreateTodoItemRequestModel etc here!!
        //
        // Code Complete: If a class is not pulling it's weight, why do you have it?
        return Ok(ApiResult<UpdateTodoItemResponseModel>.Success(
            await _todoItemService.UpdateAsync(id, updateTodoItemModel)));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        return Ok(ApiResult<BaseResponseModel>.Success(await _todoItemService.DeleteAsync(id)));
    }
}
