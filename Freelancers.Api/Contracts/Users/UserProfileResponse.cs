﻿namespace Freelancers.Api.Contracts.Users;

public record UserProfileResponse(
	string Email,
	string FirstName,
	string LastName
);
