---
description: "List GitHub repositories for a user or organization"
argument-hint: "<username/org>"
---

List GitHub repositories for: $ARGUMENTS

I'll retrieve the repositories using the GitHub CLI.

First, let me check if you want to list repositories for a specific user/organization or your own repositories:

If $ARGUMENTS is provided, I'll list repositories for that user/organization.
If no arguments are provided, I'll list your own repositories.

Let me fetch the repository list using `gh repo list`.