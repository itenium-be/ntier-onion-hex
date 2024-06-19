﻿using AutoMapper;
using N_Tier.Application.Exceptions;
using N_Tier.Application.Models;
using N_Tier.Application.Models.TodoList;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using N_Tier.Shared.Services;

namespace N_Tier.Application.Services.Impl;

public class TodoListService : ITodoListService
{
    private readonly IClaimService _claimService;
    private readonly IMapper _mapper;
    private readonly ITodoListRepository _todoListRepository;

    public TodoListService(ITodoListRepository todoListRepository, IMapper mapper, IClaimService claimService)
    {
        _todoListRepository = todoListRepository;
        _mapper = mapper;
        _claimService = claimService;
    }

    public async Task<IEnumerable<TodoListResponseModel>> GetAllAsync()
    {
        var currentUserId = _claimService.GetUserId();

        var todoLists = await _todoListRepository.GetAllAsync(tl => tl.CreatedBy == currentUserId);

        return _mapper.Map<IEnumerable<TodoListResponseModel>>(todoLists);
    }

    public async Task<CreateTodoListResponseModel> CreateAsync(CreateTodoListModel createTodoListModel)
    {
        var todoList = _mapper.Map<TodoList>(createTodoListModel);

        var addedTodoList = await _todoListRepository.AddAsync(todoList);

        return new CreateTodoListResponseModel
        {
            Id = addedTodoList.Id
        };
    }

    public async Task<UpdateTodoListResponseModel> UpdateAsync(Guid id, UpdateTodoListModel updateTodoListModel)
    {
        var todoList = await _todoListRepository.GetFirstAsync(tl => tl.Id == id);

        var userId = _claimService.GetUserId();

        if (userId != todoList.CreatedBy)
            throw new BadRequestException("The selected list does not belong to you");

        todoList.Title = updateTodoListModel.Title;

        return new UpdateTodoListResponseModel
        {
            Id = (await _todoListRepository.UpdateAsync(todoList)).Id
        };
    }

    public async Task<BaseResponseModel> DeleteAsync(Guid id)
    {
        var todoList = await _todoListRepository.GetFirstAsync(tl => tl.Id == id);

        return new BaseResponseModel
        {
            Id = (await _todoListRepository.DeleteAsync(todoList)).Id
        };
    }
}
