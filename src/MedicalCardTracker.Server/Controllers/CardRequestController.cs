// Copyright (c) 2023 Mikulchik Vladislav Alekseevich <hardman.dev@pm.me>.
// This software is licensed under the MIT license.
// Please see the LICENSE file for more information.

using MedicalCardTracker.Application.Models.ViewModels;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.CreateCardRequest;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.DeleteCardRequest;
using MedicalCardTracker.Application.Requests.Commands.CardRequests.UpdateCardRequest;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestById;
using MedicalCardTracker.Application.Requests.Queries.CardRequests.GetCardRequestCollection;
using MedicalCardTracker.Application.Server.Exceptions;
using MedicalCardTracker.Server.Models.Dto;
using Microsoft.AspNetCore.Mvc;

namespace MedicalCardTracker.Server.Controllers;

public class CardRequestController : BaseController
{
    [HttpGet]
    public async Task<ActionResult<CardRequestVm>> Get(
        [FromQuery] GetCardRequestByIdQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            return await Mediator.Send(request, cancellationToken);
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }

    [HttpGet]
    public async Task<ActionResult<CardRequestCollectionVm>> GetCollection(
        [FromQuery] GetCardRequestCollectionQuery request,
        CancellationToken cancellationToken
    ) => await Mediator.Send(request, cancellationToken);

    [HttpPost]
    public async Task<ActionResult<CardRequestVm>> Create(
        [FromBody] CreateCardRequestCommand request,
        CancellationToken cancellationToken
    ) => await Mediator.Send(request, cancellationToken);

    [HttpPatch]
    public async Task<ActionResult<CardRequestVm>> Update(
        [FromQuery] Guid id,
        [FromBody] UpdateCardRequestCommandDto requestCommandDto,
        CancellationToken cancellationToken)
    {
        try
        {
            return await Mediator.Send(
                new UpdateCardRequestCommand
                {
                    Id = id,
                    CustomerName = requestCommandDto.CustomerName,
                    TargetAddress = requestCommandDto.TargetAddress,
                    PatientFullName = requestCommandDto.PatientFullName,
                    PatientBirthDate = requestCommandDto.PatientBirthDate,
                    Description = requestCommandDto.Description,
                    Status = requestCommandDto.Status,
                    Priority = requestCommandDto.Priority
                }, CancellationToken.None);
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(
        [FromQuery] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            await Mediator.Send(
                new DeleteCardRequestCommand
                {
                    Id = id
                }, CancellationToken.None);

            return Ok();
        }
        catch (Exception e)
        {
            if (e.GetType() == typeof(EntityNotFoundException)) return NotFound();
        }

        return BadRequest();
    }
}
