﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Beeble.Api.Models;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Beeble.Api.UserManager;
using Beeble.Data.Models;
using Beeble.Domain.DTOs;

namespace Beeble.Api.Controllers
{
	[RoutePrefix("api/Account")]
	public class AccountController : AuthorizationController
	{
		private AuthRepository _repo = null;

		public AccountController()
		{
			_repo = new AuthRepository();
		}


		[HttpGet]
		[Authorize]
		[Route("get")]
		public OnlineUserDTO GetUser()
		{
			return _repo.GetUser(UserId);
		}

		// POST api/Account/Register
		[AllowAnonymous]
		[Route("Register")]
		public async Task<IHttpActionResult> Register(UserModel userModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			IdentityResult result = await _repo.RegisterUser(userModel);

			IHttpActionResult errorResult = GetErrorResult(result);

			if (errorResult != null)
			{
				return errorResult;
			}

			return Ok();
		}

		[HttpPost]
		[Authorize]
		[Route("edit")]
		public async Task<IHttpActionResult> Edit(UserModel userModel)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var result = await _repo.EditUser(userModel, UserId);
			return Ok(result);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				_repo.Dispose();
			}

			base.Dispose(disposing);
		}

		private IHttpActionResult GetErrorResult(IdentityResult result)
		{
			if (result == null)
			{
				return InternalServerError();
			}

			if (!result.Succeeded)
			{
				if (result.Errors != null)
				{
					foreach (string error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
				}

				if (ModelState.IsValid)
				{
					// No ModelState errors are available to send, so just return an empty BadRequest.
					return BadRequest();
				}

				return BadRequest(ModelState);
			}

			return null;
		}
	}
}
