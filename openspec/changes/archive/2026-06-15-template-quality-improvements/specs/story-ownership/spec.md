## ADDED Requirements

### Requirement: Anonymous callers receive an empty-string user identity
When no authenticated user is present in the HTTP context, `GetClaimsFromUser.UserId` SHALL return `string.Empty` (or an explicit non-admin neutral value), not `"Admin"`.

#### Scenario: Unauthenticated request does not resolve to Admin
- **WHEN** an HTTP request arrives with no authenticated user (no claims principal or no Name claim)
- **THEN** `GetClaimsFromUser.UserId` SHALL NOT return `"Admin"`

#### Scenario: Unauthenticated request sees no rows
- **WHEN** a query is executed through a `TuskDbContext` whose `_userId` is `string.Empty`
- **THEN** the row-level query filter `x.OwnedBy == _userId` SHALL return zero rows

#### Scenario: Authenticated request resolves to the user's name
- **WHEN** an HTTP request arrives with a ClaimTypes.Name claim value of `"alice"`
- **THEN** `GetClaimsFromUser.UserId` SHALL return `"alice"`
